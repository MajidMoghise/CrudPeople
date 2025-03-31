using Microsoft.EntityFrameworkCore;
using ElasticLogger;
using IDP.Infrastructure.Repositories.Attributes.Command;
using People.Domain.DataModels.DataModels.Commands;
using People.Infrastructure.Ef.Configs.DbContexts.Command;
using People.Domain.Contract.Repositories.Base.Commands;
using IDP.Domain.Contract.Repositories.Attributes.Command;
using People.Domain.Contract.Repositories.People.Command.Modles;
using Azure.Core;
using CrudPeople.Infrastructure.EfCore.Repositories.Base.Commands.Base;
using CrudPeople.CoreDomain.Contracts.Base.Commands.Models;

namespace People.Infrastructure.Ef.Repositories.People.Commands
{
    [LogCall]
    public class PeopleEntityCommandRepository : BaseCommandRepository<PeopleEntityCommand>, IPeopleEntityCommandRepository
    {
        private readonly PeopleCommandRepositoryMapper _mapper;
        public PeopleEntityCommandRepository(Ef_CommandDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _mapper = new PeopleCommandRepositoryMapper();

        }

        public async Task Update(PersonUpdateRequestModel entity)
        {

            var ent = PeopleEntityCommand.UpdateModel(entity.Id, entity.RowVersion,entity.DoingUserName);
            _unitOfWork.BeginTransaction();
            var at = _entity.Attach(ent);

            //at.Property(p => p.Pattern).IsModified = true;
            at.Property(p => p.DoingUserName).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLogicAsync(DeleteRequestModel<Guid> entity)
        {
            _unitOfWork.BeginTransaction();
            var ent = PeopleEntityCommand.DeleteLogicModel(entity.Id, entity.RowVersion, entity.DoingUserName);
            var entry = _entity.Attach(ent);

            entry.Property(p => p.IsDeleted).IsModified = true;
            entry.Property(p => p.DoingUserName).IsModified = true;
            entry.Property(p => p.ChangeDate).IsModified = true;
            
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(DeleteRequestModel<Guid> entity)
        {
            _unitOfWork.BeginTransaction();
            var entry = _entity.Remove(PeopleEntityCommand.CreateWithIdAndRowVersionModel(entity.Id, entity.RowVersion, entity.DoingUserName));

            

            await _context.SaveChangesAsync();
        }
    }
}
