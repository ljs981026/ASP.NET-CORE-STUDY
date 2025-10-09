using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NetCore.Services.Config;

namespace NetCore.Services.Data
{

    public class CodeFirstDbContextFactory : IDesignTimeDbContextFactory<CodeFirstDbContext>
    {
        //private const string _configPath = @"/Users/jaeseok/ASP.NET-CORE-STUDY/NetCore.Web/appsettings.json";
        private const string _configPath = @"D:\Lecture\ASP.NET-CORE-STUDY\NetCore.Web\appsettings.json";

        public CodeFirstDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CodeFirstDbContext>();
            optionsBuilder.UseSqlServer(new DbConnector(_configPath).GetConnectionString());

            return new CodeFirstDbContext(optionsBuilder.Options);
        }
    }
}