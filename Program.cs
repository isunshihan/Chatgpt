using Chatgpt;
using Microsoft.Extensions.Configuration;
using System.Reflection;

var builder = new ConfigurationBuilder()
                          .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                          .AddJsonFile("config/config.json", optional: true, reloadOnChange: true);
IConfiguration configuration = builder.Build();
var classNames = configuration.GetSection("excutor").Value.Trim();
//string[] temps = classNames.Split("|", StringSplitOptions.RemoveEmptyEntries);
Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
IExcutor excutor = (IExcutor)assembly.CreateInstance("Chatgpt." + classNames); // 类的完全限定名（即包括命名空间）
await excutor.Go();