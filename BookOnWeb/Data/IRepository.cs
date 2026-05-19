namespace BookOnWeb.Data
{
    public interface IRepository<TEntity, TPrimaryKey>
        where TEntity : Entity
    {
        AppDbContext UnitOfWork { get; }

        TEntity? Find(TPrimaryKey id);
        Task<TEntity?> FindAsync(TPrimaryKey id);

        IQueryable<TEntity> GetAll();
        Task<IQueryable<TEntity>> GetAllAsync();

        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);

        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
