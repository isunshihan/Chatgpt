using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class Post
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "post_author")] //作者ID
        public long PostAuthor { get; set; }

        [SugarColumn(ColumnName = "post_date")] //发布日期
        public DateTime PostDate { get; set; }

        [SugarColumn(ColumnName = "post_date_gmt")] //发布日期GMT
        public DateTime PostDateGmt { get; set; }

        [SugarColumn(ColumnName = "post_modified")] //修改日期
        public DateTime PostMod { get; set; }

        [SugarColumn(ColumnName = "post_modified_gmt")] //修改日期GMT
        public DateTime PostModGmt { get; set; }

        private string postContent;
        [SugarColumn(ColumnName = "post_content")] //发布内容
        public string PostContent
        {
            get
            {
                return postContent;
            }
            set
            {
                postContent = String.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private string postTitle;
        [SugarColumn(ColumnName = "post_title")] //标题
        public string PostTitle
        {
            get
            {
                return postTitle;
            }
            set
            {
                postTitle = String.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private string postType;
        [SugarColumn(ColumnName = "post_type")] //标题
        public string PostType
        {
            get
            {
                return postType;
            }
            set
            {
                postType = String.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private string postExcerpt;
        [SugarColumn(ColumnName = "post_excerpt")] //摘录
        public string PostExcerpt
        {
            get
            {
                return postExcerpt;
            }
            set
            {
                postExcerpt = String.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        [SugarColumn(ColumnName = "post_status")]
        public string PostStatus { get; set; }

        [SugarColumn(ColumnName = "post_name")]
        public string PostName { get; set; }

        [SugarColumn(ColumnName = "ping_status")]
        public string PingStatus { get; set; }

        [SugarColumn(ColumnName = "to_ping")]
        public string ToPing { get; set; }

        [SugarColumn(ColumnName = "pinged")]
        public string Pinged { get; set; }

        [SugarColumn(ColumnName = "post_content_filtered")]
        public string PostContentFilter { get; set; }
    }
}
