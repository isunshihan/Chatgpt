using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class PubHelper
    {
        IConfiguration configuration;
        SugarContext sugarContext = new SugarContext();

        public PubHelper()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("Config/Main.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();
        }

        public async Task Post(string title, string html, long category, long author, List<string> tags,
            Dictionary<string, string> metas)
        {
            try
            {
                var post = new Post()
                {
                    PostAuthor = author,
                    PostDate = DateTime.Now.ToLocalTime(),
                    PostDateGmt = DateTime.Now.ToLocalTime(),
                    PostContent = html,
                    PostMod = DateTime.Now.ToLocalTime(),
                    PostModGmt = DateTime.Now.ToLocalTime(),
                    PostTitle = title,
                    PostStatus = "publish",
                    PingStatus = "closed",
                    PostExcerpt = string.Empty,
                    ToPing = string.Empty,
                    Pinged = string.Empty,
                    PostName = Guid.NewGuid().ToString(),
                    PostContentFilter = string.Empty,
                };
                var id = await sugarContext.Db.Insertable(post).ExecuteReturnIdentityAsync();
                Console.WriteLine($"成功发布文章{title}，id为{id}");
                var c = await sugarContext.Db.Queryable<TermTaxonomy>().Where(t => t.TermId == category).FirstAsync();
                long termTaxonomyId = c == null ? 1 : c.Id; //查出该分类对应的TermTaxonomy的ID
                var relationships = new Relationships()
                {
                    PostId = id,
                    CateId = termTaxonomyId
                };
                await sugarContext.Db.Insertable(relationships).ExecuteCommandAsync();

                if (tags.Count > 0)
                {
                    foreach (var tag in tags)
                    {
                        var term = sugarContext.Db.Queryable<Terms>().Where(t => t.Name == tag).First();
                        if (term == null) //如果该tag不存在，新建标签以及对应的TermTaxonomy
                        {
                            term = new Terms()
                            {
                                Name = tag,
                                Slug = Helper.GetPinyin(tag),
                                TermGroup = 0
                            };
                            term.Id = sugarContext.Db.Insertable(term).ExecuteReturnIdentity();
                            var termTaxonomy = new TermTaxonomy()
                            {
                                TermId = term.Id,
                                Taxonomy = "post_tag",
                                Parent = 0,
                                Count = 0,
                                Description = "post_tag"
                            };
                            termTaxonomyId = sugarContext.Db.Insertable(termTaxonomy).ExecuteReturnIdentity();
                        }
                        else //如果tag已存在，则查出它在termTaxonomy表中的termTaxonomyId
                        {
                            termTaxonomyId = sugarContext.Db.Queryable<TermTaxonomy>().Where(t => t.TermId == term.Id).First().Id;
                        }
                        relationships = new Relationships()
                        {
                            PostId = id,
                            CateId = termTaxonomyId
                        };
                        sugarContext.Db.Insertable(relationships).ExecuteCommand();
                        Console.WriteLine($"给文章打上{tag}标签");
                    }
                }
                PostMeta postMeta;
                foreach (var item in metas)
                {
                    postMeta = new PostMeta()
                    {
                        PostId = id,
                        MetaKey = item.Key,
                        MetaValue = item.Value
                    };
                    await sugarContext.Db.Insertable(postMeta).ExecuteCommandAsync();
                    Console.WriteLine("写入文章meta数据");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine);
            }
        }
    }
}
