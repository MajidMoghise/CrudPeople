using Microsoft.EntityFrameworkCore;
using ElasticLogger;
using IDP.Infrastructure.Repositories.Attributes.Command;
using People.Domain.DataModels.DataModels.Commands;
using People.Infrastructure.Ef.Repositories.Base.Commands.Base;
using People.Infrastructure.Ef.Configs.DbContexts.Command;
using People.Domain.Contract.Repositories.Base.Commands;
using IDP.Domain.Contract.Repositories.Attributes.Command;
using People.Domain.Contract.Repositories.People.Command.Modles;
using Azure.Core;
using IDP.Domain.Contract.Repositories.Attributes.Query;
using People.Domain.DataModels.DataModels.Queries;
using IDP.Infrastructure.Repositories.Attributes.Queries;
using People.Infrastructure.Ef.Configs.DbContexts.Query;
using People.Domain.Contract.Repositories.People.Query.Modles;
using Helpers.FilterSearch;
using System.Runtime.CompilerServices;
using CrudPeople.Infrastructure.EfCore.Repositories.Base.Queries;
using CrudPeople.CoreDomain.Contracts.Base.Queries.Models;

namespace People.Infrastructure.Ef.Repositories.People.Queries
{
    [LogCall]
    public class PeopleEntityQueryRepository : BaseQueryRepository<PeopleDataEntityQuery>, IPeopleEntityQueryRepository
    {
        private readonly PeopleQueryRepositoryMapper _mapper;
        public PeopleEntityQueryRepository(Ef_QueryDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _mapper = new PeopleQueryRepositoryMapper();

        }

        public async Task<SearchResponseModel<PeopleGetResponseModel>> GetPeopleByFilter(SearchModel<PeopleGetRequestModel, PeopleGetResponseModel> request)
        {
            var query = request.ToSqlQuery();
           
          return await  _entity.FromSql(query)
                               .Select(s=> _mapper.PeopleGetResponseModel(s))
                               .ToPagedListAsync(request.Page, request.Size);
        }

        public Task<PeopleDataEntityQuery> GetPoepleByIdAsync(Guid id)
        {
            return _entity.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
