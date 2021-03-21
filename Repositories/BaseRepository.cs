using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Data;
using ExperBE.Models.Entities.Base;
using ExperBE.Repositories.Interfaces;

namespace ExperBE.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly ExperDbContext _context;

        protected BaseRepository(ExperDbContext context) 
            => _context = context;

        public IQueryable<TEntity> GetAll()
            => _context.Set<TEntity>();

        public void Add(TEntity entity)
            => _context.Set<TEntity>().Add(entity);

        public void AddRange(IEnumerable<TEntity> entities)
            => _context.Set<TEntity>().AddRange(entities);

        public void Remove(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);
    }
}
