using CrudPeople.CoreDomain.Contracts.Base.Commands;
using CrudPeople.Infrastructure.EfCore.Context.Command;
using ElasticLogger;
using Microsoft.EntityFrameworkCore;

namespace CrudPeople.Infrastructure.EfCore.Repositories.Base.Commands.Base
{
    [LogCall]
    public class BaseCommandRepository<TEntity> : IBaseCommandRepository<TEntity> where TEntity : class
    {
        protected readonly Ef_CommandDbContext _context;
        protected readonly DbSet<TEntity> _entity;

        protected readonly IUnitOfWork _unitOfWork;

        public BaseCommandRepository(Ef_CommandDbContext context,
                IUnitOfWork unitOfWork)
        {

            _context = context;
            _entity = _context.Set<TEntity>();
            _unitOfWork = unitOfWork;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _unitOfWork.BeginTransactionAsync();
            await _entity.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //public async Task DeleteAsync(object Id,TEntity entity)
        //{
        //    await _unitOfWork.BeginTransactionAsync();
        //    var entriy =await _entity.FindAsync(Id);
        //    entriy.IsDeleted = true;
        //   // _entity.Entry(entity).Property(e => e.IsDeleted).IsModified = true;//.State = EntityState.Modified;
        //    if (transactional == false)
        //    {
        //        await _context.SaveChangesAsync();

        //    }

        //}

        //public async Task UpdateAsync(TEntity entity)
        //{
        //    await _unitOfWork.BeginTransactionAsync();

        //    _entity.Attach(entity);
        //    int indx = 0;

        //    foreach (var property in _context.Entry(entity).Properties) 
        //    {
        //        var typeOf = property.GetType();
        //        if(typeOf==DateTime.)
        //        if (property.CurrentValue != null &&indx>0)
        //        {
        //            property.IsModified = true; 
        //        }
        //        indx ++;    
        //    }

        //}
    }
}
