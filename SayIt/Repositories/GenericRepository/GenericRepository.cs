﻿using Microsoft.EntityFrameworkCore;
using SayIt.Data;
using SayIt.Models.Base;

namespace SayIt.Repositories.GenericRepository
{
    public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly Context _dbContext;
        protected readonly DbSet<TEntity> _table;

        protected GenericRepository(Context db)
        {
            _dbContext = db;
            _table = _dbContext.Set<TEntity>();
        }

        // Get all
        public List<TEntity> GetAll()
        {
            return _table.AsNoTracking().ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _table.AsNoTracking().ToListAsync();
        }

        // !! In repo we return the data not queries
        //public IQueryable<TEntity> GetAllAsQueryable()
        //{
        //    return _table.AsNoTracking();

        //    // try not to use count, toList etc, before finishing the query/filtering data

        //    var entityList = _table.ToList();
        //    var entityListFiltered = _table.Where(x => entityList.Contains(x));


        //    // better version; data filtered in the query
        //    var entityListQuery = _table.AsQueryable();
        //    var entityListQueryFiltered = entityListQuery.Where(x => entityList.Contains(x));
        //    var result = entityListQueryFiltered.ToList();
        //}

        // Create
        public void Create(TEntity entity)
        {
            _table.Add(entity);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _table.AddAsync(entity);
        }

        public void CreateRange(IEnumerable<TEntity> entities)
        {
            _table.AddRange(entities);
        }

        public async Task CreateRangeAsync(IEnumerable<TEntity> entities)
        {
            await _table.AddRangeAsync(entities);
        }


        // Update

        public void Update(TEntity entity)
        {
            _table.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _table.UpdateRange(entities);
        }

        // Delete
        public void Delete(TEntity entity)
        {
            _table.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _table.RemoveRange(entities);
        }

        // Find
        public TEntity FindById(Guid id)
        {
            return _table.Find(id);
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await _table.FindAsync(id);
        }

        // Save
        public bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }
        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
