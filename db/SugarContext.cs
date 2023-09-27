using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class SugarContext
    {
        public SqlSugarClient Db;
        IConfiguration configuration;
        public SugarContext()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("config/config.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();
            Db = this.Instance;
            Db.MappingTables.Add("Article", configuration.GetSection("table").Value);           
        }

        public SqlSugarClient Instance
        {
            get
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = configuration.GetSection("connstr").Value,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
            }
        }
    }
}
