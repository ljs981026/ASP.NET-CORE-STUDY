using System;
using Microsoft.Extensions.Configuration;

namespace NetCore.Services.Config
{
	public class DbConnector
    {
		private readonly string _connectionString = string.Empty;

		public DbConnector(string configPath)
		{
			var configBuilder = new ConfigurationBuilder();
			configBuilder.AddJsonFile(configPath, false);

			//string connectionString = configBuilder.Build().GetSection("ConnectionStrings")
			//	.GetSection("DefaultConnection").Value;
			_connectionString = configBuilder.Build()["ConnectionStrings:DefaultConnection"];

		}

		public string GetConnectionString()
		{
			return _connectionString;
		}
	}
}