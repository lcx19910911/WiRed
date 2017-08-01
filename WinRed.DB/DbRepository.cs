using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using WinRed.Core.Util;
using WinRed.Model;

namespace WinRed.DB
{
    public class DbRepository : DbContext
    {

        public DbRepository()
           : base("name=DbRepository")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DbRepository>());

            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Configuration.AutoDetectChangesEnabled = false;//关闭自动跟踪对象的属性变化
            //this.Configuration.ValidateOnSaveEnabled = false; //关闭保存时的实体验证
            //this.Configuration.UseDatabaseNullSemantics = false;//关闭数据库null比较行为


        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="entry">entry对象</param>
        private void InitObject(DbEntityEntry entry)
        {
            if (entry.Entity is BaseEntity)
            {
                var entity = entry.Entity as BaseEntity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        //初始化这些值，如果这些值为null时，自动赋值
                        if (entity.CreatedTime == new DateTime())
                            entity.CreatedTime = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            try
            {
                var entries = from e in this.ChangeTracker.Entries()
                              where e.State != EntityState.Unchanged
                              select e;   //过滤所有修改了的实体，包括：增加 / 修改 / 删除


                foreach (var entry in entries)
                {

                    InitObject(entry);
                }
                if (entries.Count() == 0)
                    return 1;
                else
                    return base.SaveChanges();

            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
                //并发冲突数据
                if (ex.GetType() == typeof(DbUpdateConcurrencyException))
                {
                    return -1;
                }
                return 0;
            }

        }
        

        public DbSet<User> User { get; set; }
    }

}
