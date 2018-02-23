using Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    
    [DbConfigurationType(typeof(Repository.Configuration.DBConfiguration))]
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext() : base("AppContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<UsersDataEntity> UsersData { get; set; }
        public virtual DbSet<ImageDataEntity> ImageData { get; set; }
        public virtual DbSet<ImageTagEntity>  ImageTag { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
