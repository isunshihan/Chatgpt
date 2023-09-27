using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class TermTaxonomy
    {
        [SugarColumn(ColumnName = "term_taxonomy_id", IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "term_id")]
        public long TermId { get; set; }

        [SugarColumn(ColumnName = "taxonomy")]
        public string Taxonomy { get; set; }

        [SugarColumn(ColumnName = "description")]
        public string Description { get; set; }

        [SugarColumn(ColumnName = "parent")]
        public long Parent { get; set; }

        [SugarColumn(ColumnName = "count")]
        public long Count { get; set; }
    }
}
