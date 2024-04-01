using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;

namespace CeetahDAL
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        public DbSet<TEntity> Items;
        protected DataContext context;

        public BaseRepository(DataContext context)
        {
            this.context = context;

            Items = context.Set<TEntity>();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public virtual EntityEntry<TEntity> Add(TEntity entity)
        {
            
            return Items.Add(entity);
        }

        public virtual EntityEntry<TEntity> Update(TEntity entity)
        {
            
            return Items.Update(entity);
        }

        public EntityEntry<TEntity> AddOrUpdate(TEntity entity)
        {
            //typeof(TEntity)
            var key = context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties[0];
            if (key.PropertyInfo.PropertyType == typeof(Guid) &&
                (Guid)entity.GetType().GetProperty(key.Name).GetValue(entity, null) == Guid.Empty)
                return Add(entity);
            return Update(entity);
        }

        public List<TEntity> AddOrUpdateList(List<TEntity> entities)
        {
            var updatesEntities = new List<TEntity>();
            foreach (var Entity in entities)
            {
                var result = AddOrUpdate(Entity);
                updatesEntities.Add(result.Entity);
            }
            return updatesEntities;
        }
        public void RemoveById(object id)
        {

            TEntity existing = Items.Find(id);
            Items.Remove(existing);
        }
        public IEnumerable<TEntity> All()
        {
            return Items.ToList();
        }


        public void Remove(TEntity entity)
        {

            Items.Remove(entity);
        }
        public void AddOrUpdateReferences(TEntity old, TEntity update)
        {
            foreach (var reference in context.Entry(update).References)
            {
                if (reference.TargetEntry != null && reference.TargetEntry.Metadata.FindPrimaryKey().Properties.Count == 1)
                {
                    var keyProp = reference.TargetEntry.Metadata.FindPrimaryKey().Properties[0];

                    if (keyProp != null && keyProp.ClrType == typeof(Guid))
                    {
                        if (reference.TargetEntry.CurrentValues.GetValue<Guid>(keyProp.Name) == Guid.Empty)
                        {
                            context.Add(reference.CurrentValue);
                            old.GetType().GetProperty(reference.Metadata.Name).SetValue(old, reference.CurrentValue);
                        }
                        else
                            context.Update(reference.CurrentValue);
                    }

                }
            }

        }
    }
}