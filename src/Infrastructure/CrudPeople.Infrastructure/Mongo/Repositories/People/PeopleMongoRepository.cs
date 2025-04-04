using CrudPeople.CoreDomain.Contracts.Base.Commands;
using CrudPeople.CoreDomain.Contracts.People.Command;
using CrudPeople.CoreDomain.Contracts.People.Command.Models;
using CrudPeople.CoreDomain.Contracts.People.Query;
using CrudPeople.CoreDomain.Contracts.People.Query.Models;
using CrudPeople.CoreDomain.Entities.People.Query;
using CrudPeople.Infrastructure.Mongo.Context;
using CrudPeople.Infrastructure.Mongo.Repositories.Base;
using Helpers.FilterSearch;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Mongo.Repositories.People
{
    public class PeopleMongoRepository : IPeopleCommandRepository, IPeopleQueryRepository
    {
        private readonly MongoContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMongoCollection<PeopleQueryEntity> _collection;
        private readonly PeopleMongoRepositoryMapper _mapper;

        public PeopleMongoRepository(MongoContext context, IUnitOfWork unitOfWork)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _collection = _context.GetCollection<PeopleQueryEntity>("People");
            _mapper = new PeopleMongoRepositoryMapper();
        }

        public async Task<Guid> CreatePerson(CreatePersonRequestModel request)
        {
            var model = _mapper.PeopleCommandEntity(request);
            _unitOfWork.BeginTransaction();
            await _collection.InsertOneAsync(_context.Transaction, model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task DeletePerson(DeletePersonRequestModel request)
        {
            _unitOfWork.BeginTransaction();
            var filter = Builders<PeopleQueryEntity>.Filter.Eq(x => x.Id, request.Id);
            var result = await _collection.DeleteOneAsync(_context.Transaction, filter);
            if (result.DeletedCount == 0)
            {
                throw new KeyNotFoundException("Person not found.");
            }
            await _context.SaveChangesAsync();

        }

        public async Task UpdatePerson(UpdatePersonRequestModel request)
        {
            _unitOfWork.BeginTransaction();
            var filter = Builders<PeopleQueryEntity>.Filter.Eq(x => x.Id, request.Id);
            var update = Builders<PeopleQueryEntity>.Update
                .Set(x => x.FirstName, request.FirstName)
                .Set(x => x.LastName, request.LastName)
                .Set(x => x.BirthDate, request.BirthDate)
                .Set(x => x.NationalCode, request.NationalCode)
                .Set(x => x.PersonTypeId, request.PersonTypeId);

            var result = await _collection.UpdateOneAsync(_context.Transaction, filter, update);

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException("Person not found for update.");
            }

            await _context.SaveChangesAsync();
        }

        public async Task<PersonResponseModel> GetById(Guid id)
        {
            var filter = Builders<PeopleQueryEntity>.Filter.Eq(x => x.Id, id);
            var person = await _collection.Find(filter).FirstOrDefaultAsync();

            if (person == null)
            {
                throw new KeyNotFoundException("Person not found.");
            }

            return _mapper.ToPersonResponseModel(person);
        }

        public async Task<SearchResponseModel<PeopleResponseModel>> GetList(SearchRequestModel<PeopleRequestModel, PeopleResponseModel> request)
        {
            var filter = request.ToMongoDbQuery();

            var totalCount = await _collection.CountDocumentsAsync(filter);
            var results = await _collection.ToPagedListAsync(
                                                                filter,
                                                                request.Page,
                                                                 request.Page
                                                             );

            var mappedResults = results.Data.Select(_mapper.ToPeopleResponseModel).ToList();

            return new SearchResponseModel<PeopleResponseModel>
            {
                Page = request.Page,
                Data = mappedResults,
                TotalCount = (int)totalCount,
                TotalOfPages = (int)Math.Ceiling((double)totalCount / request.Size)
            };
        }
    }
}
