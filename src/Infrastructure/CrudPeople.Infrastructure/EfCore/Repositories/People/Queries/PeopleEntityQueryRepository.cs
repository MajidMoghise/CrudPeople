using Microsoft.EntityFrameworkCore;
using ElasticLogger;
using Helpers.FilterSearch;
using CrudPeople.Infrastructure.EfCore.Repositories.Base.Queries;
using CrudPeople.CoreDomain.Contracts.People.Query;
using CrudPeople.Infrastructure.EfCore.Context.Query;
using CrudPeople.CoreDomain.Contracts.People.Query.Models;
using CrudPeople.Infrastructure.EfCore.Repositories.People.Queries;
using CrudPeople.CoreDomain.Entities.People.Query;
using CrudPeople.CoreDomain.Contracts.Base.Commands;

namespace CrudPeople.Infrastructure.EfCore.Repositories.People
{
    [LogCall]
    public class PeopleQueryRepository : BaseQueryRepository<PeopleQueryEntity>, IPeopleQueryRepository
    {
        private readonly PeopleQueryRepositoryMapper _mapper;
        public PeopleQueryRepository(Ef_QueryDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _mapper = new PeopleQueryRepositoryMapper();

        }

        public async Task<SearchResponseModel<PeopleResponseModel>> GetList(SearchRequestModel<PeopleRequestModel, PeopleResponseModel> request)
        {
            var query = request.ToSqlQuery();
           
          return await  _entity.FromSql(query)
                               .Select(s=> _mapper.PeopleResponseModel(s))
                               .ToPagedListAsync(request.Page, request.Size);
        }

        public async Task<PersonResponseModel> GetById(Guid id)
        {
            return await _entity.Select(s => _mapper.PersonResponseModel(s))
                          .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
