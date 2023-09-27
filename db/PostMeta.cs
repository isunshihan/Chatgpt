using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class PostMeta
    {
        [SugarColumn(ColumnName = "meta_id", IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "post_id")] //文章id
        public int PostId { get; set; }

        [SugarColumn(ColumnName = "meta_key")] //文章字段
        public string MetaKey { get; set; }

        [SugarColumn(ColumnName = "meta_value")] //文章字段的值
        public string MetaValue { get; set; }
    }
}
