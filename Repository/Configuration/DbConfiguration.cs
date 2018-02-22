using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;

namespace Repository.Configuration
{
   public class DBConfiguration : DbConfiguration
    {
        public DBConfiguration()
        {
            this.SetExecutionStrategy("System.Data.SqlClient", () => new System.Data.Entity.SqlServer.SqlAzureExecutionStrategy());

        }

        protected void AddExecutionStrategy(Func<IDbExecutionStrategy> getExecutionStrategy, string serverName)
        {
            AddDependencyResolver(new ExecutionStrategyResolver<IDbExecutionStrategy>("SqlAzureExecutionStrategy", serverName, getExecutionStrategy));
        }
    }
}
