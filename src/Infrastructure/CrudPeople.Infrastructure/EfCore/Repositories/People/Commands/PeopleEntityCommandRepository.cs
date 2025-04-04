using Microsoft.EntityFrameworkCore;
using ElasticLogger;
using IDP.Infrastructure.Repositories.Attributes.Command;
using CrudPeople.Infrastructure.EfCore.Repositories.Base.Commands.Base;
using CrudPeople.CoreDomain.Contracts.Base.Commands.Models;
using CrudPeople.CoreDomain.Contracts.Base.Commands;
using CrudPeople.CoreDomain.Entities.People.Command;
using CrudPeople.CoreDomain.Contracts.People.Command;
using CrudPeople.Infrastructure.EfCore.Context.Command;
using CrudPeople.CoreDomain.Contracts.People.Command.Models;
using CrudPeople.CoreDomain.Helper;
using SharpCompress.Common;

namespace People.Infrastructure.Ef.Repositories.People.Commands
{
    [LogCall]
    public class PeopleCommandRepository : BaseCommandRepository<PeopleCommandEntity>, IPeopleCommandRepository
    {
        private readonly PeopleCommandRepositoryMapper _mapper;
        public PeopleCommandRepository(Ef_CommandDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _mapper = new PeopleCommandRepositoryMapper();

        }

        public async Task<Guid> CreatePerson(CreatePersonRequestModel entity)
        {
            var create = _mapper.PeopleCommandEntity(entity);
            _unitOfWork.BeginTransaction();
            _entity.Add(create);
            await _context.SaveChangesAsync();
            return create.Id;
        }

        public async Task UpdatePerson(UpdatePersonRequestModel entity)
        {


            var ent = _mapper.PeopleCommandEntity(entity);
            _unitOfWork.BeginTransaction();
            var at = _entity.Attach(ent);
            if (!string.IsNullOrEmpty(entity.FirstName))
            {
                at.Property(p => p.FirstName).IsModified = true;
            }
            if (!string.IsNullOrEmpty(entity.LastName))
            {
                at.Property(p => p.LastName).IsModified = true;

            }
            if (!string.IsNullOrEmpty(entity.NationalCode))
            {
                at.Property(p => p.NationalCode).IsModified = true;

            }
            if (entity.BirthDate is not null)
            {
                at.Property(p => p.BirthDate).IsModified = true;

            }
            if (entity.PersonTypeId is not null)
            {
                at.Property(p => p.PersonTypeId).IsModified = true;

            }
            at.Property(p => p.DoingUserName).IsModified = true;
            at.Property(p => p.RowVersion).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeletePerson(DeletePersonRequestModel entity)
        {
            _unitOfWork.BeginTransaction();
            var entry = _entity.FirstOrDefault(f => f.Id == entity.Id);
            if (entry is null)
            {
                throw CrudPeopleException.NotFound("Person is not found");
            }
            if (entry.RowVersion != entity.RowVersion)
            {
                throw CrudPeopleException.NotFound("Version of Person Data is not correct");
            }
            _entity.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
