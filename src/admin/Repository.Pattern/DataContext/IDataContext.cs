using System;
using Repository.Pattern.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using TrackableEntities;

namespace Repository.Pattern.DataContext
{
    [Obsolete("IDataContext has been deprecated. Instead use UnitOfWork which uses DbContext.")]
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, ITrackable;
        void SyncObjectsStatePostCommit();



        void SetAutoDetectChangesEnabled(bool enabled);
                     
        


    }
}