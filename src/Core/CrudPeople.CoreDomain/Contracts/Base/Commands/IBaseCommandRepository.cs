using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.CoreDomain.Contracts.Base.Commands
{
    public interface IBaseCommandRepository<TEntity> 
    {

        Task<TEntity> AddAsync(TEntity entity);


    }
}
