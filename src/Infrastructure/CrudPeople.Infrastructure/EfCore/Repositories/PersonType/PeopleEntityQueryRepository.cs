using Microsoft.EntityFrameworkCore;
using ElasticLogger;
using Helpers.FilterSearch;
using CrudPeople.Infrastructure.EfCore.Repositories.Base.Queries;
using CrudPeople.Infrastructure.EfCore.Repositories.People.Queries;
using CrudPeople.CoreDomain.Entities.People.Query;
using CrudPeople.Infrastructure.EfCore.Context.Query;
using CrudPeople.CoreDomain.Contracts.Base.Commands;
using CrudPeople.CoreDomain.Contracts.People.Query.Models;
using CrudPeople.CoreDomain.Entities;
using CrudPeople.CoreDomain.Contracts.PersonType;
using CrudPeople.CoreDomain.Contracts.PersonType.Models;

namespace CrudPeople.Infrastructure.EfCore.Repositories.PersonType
{
    [LogCall]
    public class PersonTypeRepository : BaseQueryRepository<CrudPeople.CoreDomain.Entities.PersonType>, IPersonTypeRepository
    {
        private readonly PeopleQueryRepositoryMapper _mapper;
        public PersonTypeRepository(Ef_QueryDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _mapper = new PeopleQueryRepositoryMapper();

        }

        public async Task<PersonTypeResponseModel> GetById(byte id)
        {
            return await _entity.Select(s => _mapper.PersonTypeResponseModel(s)).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<PersonTypeResponseModel>> GetList()
        {
            return await _entity.Select(s => _mapper.PersonTypeResponseModel(s)).ToListAsync();


        }

    }
}
