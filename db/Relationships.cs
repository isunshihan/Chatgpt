using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class Relationships
    {
        [SugarColumn(ColumnName = "object_id")]
        public long PostId { get; set; }

        [SugarColumn(ColumnName = "term_taxonomy_id")]
        public long CateId { get; set; }

        [SugarColumn(ColumnName = "term_order")]
        public int TermOrder { get; set; }
    }
}
