using Helpers.Extentions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using MongoDB.Driver;
using Nest;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.EfCore.Context.Command
{
    public partial class Ef_CommandDbContext : DbContext
    {
        private bool changeQueryDb = false;
        private Func<bool> AddToDbQuery(EntityEntry entity, string commandTableName)
        {

            if (string.Equals(commandTableName, PeopleEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return PeopleQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, PersonAddressesEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return PersonAddressesQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, PersonEmailsEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return PersonEmailsQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, PersonGroupsEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return PersonGroupsQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, PersonPhonesEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return PersonPhonesQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, GroupsEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return GroupsQueryEntity(entity);
            }
            return () => { return false; };
        }
        private Func<bool> UpdateToDbQuery(EntityEntry entity, string commandTableName)
        {
            if (string.Equals(commandTableName, PeopleEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return UpdatePeopleQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, PersonAddressesEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {

                changeQueryDb = true;
                return UpdatePersonAddressesQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, PersonEmailsEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {

                changeQueryDb = true;
                return UpdatePersonEmailsQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, PersonGroupsEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {

                changeQueryDb = true;
                return UpdatePersonGroupsQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, PersonPhonesEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {

                changeQueryDb = true;
                return UpdatePersonPhonesEntityCommandQueryEntity(entity);
            }
            else if (string.Equals(commandTableName, GroupsEntityCommandModelCreating.NameOfTable, StringComparison.OrdinalIgnoreCase))
            {
                changeQueryDb = true;
                return UpdateGroupsQueryEntity(entity);

            }
            return () => { return false; };

        }



        private Func<bool> PeopleQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PeopleEntityCommand)entity.Entity;
                var personType = (Domain.DataModels.Enums.PersonType)entity.GetDatabaseValues().GetValue<byte>(nameof(PeopleEntityCommand.PersonTypeId));
                _queryContext.AddAsync(new PeopleDataEntityQuery
                {
                    Name = e.Name,
                    PersonTypeName = personType.GetEnumName(),
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    NationalCode = e.NationalCode,
                    IsDeleted = e.IsDeleted,
                    PersonTypeId = e.PersonTypeId,
                    Id = e.Id,
                    PersonRowVersion=e.RowVersion,
                    DoingUserName = e.DoingUserName,
                }).GetAwaiter();

                return true;
            };

        }
        private Func<bool> PersonAddressesQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PersonAddressesEntityCommand)entity.Entity;
                var addressType = (Domain.DataModels.Enums.AddressType)entity.GetDatabaseValues().GetValue<byte>(nameof(PersonAddressesEntityCommand.AddressTypeId));

                var upd = _queryContext.Peoples.AsNoTracking().FirstOrDefaultAsync(p => p.Id == e.PersonId).Result;
                if (upd is null)
                {
                    throw new Exception("Person is not exist in query database");
                }
                var at = _queryContext.Peoples.Attach(upd);
                upd.Address = e.Address;
                upd.PersonAddressId = e.Id;
                upd.AddressTypeName = e.AddressTypeId.GetEnumName<Domain.DataModels.Enums.AddressType, byte>();
                at.Property(p => p.Address).IsModified = true;
                at.Property(p => p.PersonAddressId).IsModified = true;
                at.Property(p => p.AddressTypeName).IsModified = true;

                return true;
            };

        }
        private Func<bool> GroupsQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (GroupsEntityCommand)entity.Entity;
                _queryContext.AddAsync(new GroupEntityQuery
                {
                    Name = e.Name,
                    RowVersion = e.RowVersion,
                    RootId = e.RootId,
                    ChangeDate = e.ChangeDate,
                    Description = e.Description,
                    DoingUserName = e.DoingUserName,
                    Id = e.Id,
                }).GetAwaiter();

                return true;
            };

        }
        private Func<bool> PersonPhonesQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PersonPhonesEntityCommand)entity.Entity;
                var PhoneType = (Domain.DataModels.Enums.PhoneType)entity.GetDatabaseValues().GetValue<byte>(nameof(PersonPhonesEntityCommand.PhoneTypeId));

                var upd = _queryContext.Peoples.AsNoTracking().FirstOrDefaultAsync(p => p.Id == e.PersonId).Result;
                if (upd is null)
                {
                    throw new Exception("Person is not exist in query database");
                }
                var at = _queryContext.Peoples.Attach(upd);
                upd.PhoneNumber = e.PhoneNumber;
                upd.PersonPhoneId = e.Id;
                upd.PhoneRowVersion = e.RowVersion;

                upd.PhoneTypeName = e.PhoneTypeId.GetEnumName<Domain.DataModels.Enums.PhoneType, byte>();
                at.Property(p => p.PhoneNumber).IsModified = true;
                at.Property(p => p.PhoneTypeName).IsModified = true;
                at.Property(p => p.PersonPhoneId).IsModified = true;
                at.Property(p => p.PhoneRowVersion).IsModified = true;

                return true;
            };
        }
        private Func<bool> PersonEmailsQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PersonEmailsEntityCommand)entity.Entity;
                var EmailType = (Domain.DataModels.Enums.EmailType)entity.GetDatabaseValues().GetValue<byte>(nameof(PersonEmailsEntityCommand.EmailTypeId));

                var upd = _queryContext.Peoples.AsNoTracking().FirstOrDefaultAsync(p => p.Id == e.PersonId).Result;
                if (upd is null)
                {
                    throw new Exception("Person is not exist in query database");
                }
                var at = _queryContext.Peoples.Attach(upd);
                upd.EmailAddress = e.EmailAddress;
                upd.PersonEmailId = e.Id;
                upd.AddressRowVersion = e.RowVersion;
                upd.EmailTypeName = e.EmailTypeId.GetEnumName<Domain.DataModels.Enums.EmailType, byte>();
                at.Property(p => p.EmailAddress).IsModified = true;
                at.Property(p => p.EmailTypeName).IsModified = true;
                at.Property(p => p.PersonEmailId).IsModified = true;
                at.Property(p => p.AddressRowVersion).IsModified = true;

                return true;
            };

        }

        private Func<bool> PersonGroupsQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PersonGroupsEntityCommand)entity.Entity;
                var upd = _queryContext.Peoples.AsNoTracking().FirstOrDefaultAsync(p => p.Id == e.PersonId).Result;
                var groupName = Groups.FirstOrDefaultAsync(f => f.Id == e.GroupId).Result.Name;
                if (upd is null)
                {
                    throw new Exception("Person is not exist in query database");
                }
                var at = _queryContext.Peoples.Attach(upd);
                upd.GroupName = groupName;
                upd.GroupId = e.GroupId;
                upd.PersonGroupId = e.Id;
                upd.GroupRowVersion = e.RowVersion;
                at.Property(p => p.GroupName).IsModified = true;
                at.Property(p => p.GroupId).IsModified = true;
                at.Property(p => p.PersonGroupId).IsModified = true;
                at.Property(p => p.GroupRowVersion).IsModified = true;

                return true;
            };

        }


        private Func<bool> UpdateGroupsQueryEntity(EntityEntry entity)
        {
            if (((GroupsEntityCommand)entity.Entity).IsDeleted == true)
            { return DeleteGroupsEntity(entity); }
            return () =>
            {

                var e = (GroupsEntityCommand)entity.Entity;

                var upd = _queryContext.Groups.AsNoTracking().FirstOrDefaultAsync(p => p.Id == e.Id).Result;
                if (upd is null)
                {
                    throw new Exception("Group is not exist in query database");
                }
                var at = _queryContext.Groups.Attach(upd);
                upd.Name = e.Name;
                upd.Description = e.Description;
                upd.ChangeDate = e.ChangeDate;
                upd.DoingUserName = e.DoingUserName;
                upd.RootId = e.RootId;
                upd.RowVersion = e.RowVersion;
                at.Property(p => p.Name).IsModified = true;
                at.Property(p => p.Description).IsModified = true;
                at.Property(p => p.ChangeDate).IsModified = true;
                at.Property(p => p.DoingUserName).IsModified = true;
                at.Property(p => p.RootId).IsModified = true;
                var udpGroups = _queryContext.Peoples.Where(p => p.GroupId == e.Id).ToListAsync().Result;
                foreach (var udpGroupInPerson in udpGroups)
                    if (udpGroupInPerson is not null)
                    {
                        var atG = _queryContext.Peoples.Attach(udpGroupInPerson);

                        udpGroupInPerson.GroupName = e.Name;
                        udpGroupInPerson.GroupDescription = e.Description;
                        udpGroupInPerson.GroupRowVersion = e.RowVersion;
                        udpGroupInPerson.PersonGroupRootId = e.RootId;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.PersonGroupId).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.PersonGroupDescription).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.PersonGroupRootId).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.GroupName).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.GroupDescription).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.GroupRowVersion).IsModified = true;


                    }
                return true;
            };
        }
        private Func<bool> UpdatePersonEmailsQueryEntity(EntityEntry entity)
        {
            if (((PersonEmailsEntityCommand)entity.Entity).IsDeleted == true)
            { return DeletePersonEmailsEntity(entity); }
            return PersonEmailsQueryEntity(entity);

        }
        private Func<bool> UpdatePersonGroupsQueryEntity(EntityEntry entity)
        {
            if (((PersonGroupsEntityCommand)entity.Entity).IsDeleted == true)
            { return DeletePersonGroupsQueryEntity(entity); }
            return PersonGroupsQueryEntity(entity);

        }

        private Func<bool> UpdatePeopleQueryEntity(EntityEntry entity)
        {
            if (((PeopleEntityCommand)entity.Entity).IsDeleted == true)
            { return DeletePeopleQueryEntity(entity); }
            return () =>
            {
                var e = (PeopleEntityCommand)entity.Entity;

                var data = _queryContext.Peoples.AsNoTracking().FirstOrDefaultAsync(f => f.Id == e.Id).Result;
                if (data is null)
                {
                    throw new Exception("In query Database Entity not found");
                }
                var at = _queryContext.Peoples.Attach(data);
                if (data.IsDeleted != e.IsDeleted)
                {
                    data.IsDeleted = e.IsDeleted;
                    at.Property(p => p.IsDeleted).IsModified = true;
                }
                if (data.Name != e.Name)
                {
                    data.Name = e.Name;
                    at.Property(p => p.Name).IsModified = true;
                }
                if (data.NationalCode != e.NationalCode)
                {
                    data.NationalCode = e.NationalCode;
                    at.Property(p => p.NationalCode).IsModified = true;
                }
                if (data.FirstName != e.FirstName)
                {
                    data.FirstName = e.FirstName;
                    at.Property(p => p.FirstName).IsModified = true;
                }
                if (data.LastName != e.LastName)
                {
                    data.LastName = e.LastName;
                    at.Property(p => p.LastName).IsModified = true;
                }
                if (data.PersonTypeId != e.PersonTypeId)
                {
                    data.PersonTypeId = e.PersonTypeId;
                    at.Property(p => p.PersonTypeId).IsModified = true;
                    var personName = e.PersonTypeId.GetEnumName<PersonType, byte>(true);
                    data.PersonTypeName = personName;
                }

                return true;
            };
        }
        private Func<bool> UpdatePersonAddressesQueryEntity(EntityEntry entity)
        {
            if (((PersonAddressesEntityCommand)entity.Entity).IsDeleted == true)
            { return DeletePersonAddressesQueryEntity(entity); }
            return PersonAddressesQueryEntity(entity);
        }

        private Func<bool> UpdatePersonPhonesEntityCommandQueryEntity(EntityEntry entity)
        {
            if (((PersonPhonesEntityCommand)entity.Entity).IsDeleted == true)
            { return DeletePersonPhonesQueryEntity(entity); }
            return PersonPhonesQueryEntity(entity);

        }

        private Func<bool> DeleteGroupsEntity(EntityEntry entity)
        {
            return () =>
            {

                var e = (GroupsEntityCommand)entity.Entity;

                var upd = _queryContext.Groups.AsNoTracking().FirstOrDefaultAsync(p => p.Id == e.Id).Result;
                if (upd is null)
                {
                    throw new Exception("Group is not exist in query database");
                }
                if (e.IsDeleted == true)
                {
                    var at = _queryContext.Groups.Attach(upd);
                    upd.Name = e.Name;
                    upd.Description = e.Description;
                    upd.ChangeDate = e.ChangeDate;
                    upd.DoingUserName = e.DoingUserName;
                    upd.RootId = e.RootId;
                    upd.RowVersion = e.RowVersion;
                    at.Property(p => p.Name).IsModified = true;
                    at.Property(p => p.Description).IsModified = true;
                    at.Property(p => p.ChangeDate).IsModified = true;
                    at.Property(p => p.DoingUserName).IsModified = true;
                    at.Property(p => p.RootId).IsModified = true;
                    at.Property(p => p.RowVersion).IsModified = true;

                }
                else
                {
                    var del = _queryContext.Groups.AsNoTracking().FirstOrDefaultAsync(p => p.Id == e.Id).Result;
                    _queryContext.Groups.Remove(del);


                }
                var udpGroups = _queryContext.Peoples.Where(p => p.GroupId == e.Id).ToListAsync().Result;
                foreach (var udpGroupInPerson in udpGroups)
                    if (udpGroupInPerson is not null)
                    {
                        var atG = _queryContext.Peoples.Attach(udpGroupInPerson);

                        udpGroupInPerson.GroupName = null;
                        udpGroupInPerson.GroupDescription = null;
                        udpGroupInPerson.GroupRowVersion = null;
                        udpGroupInPerson.PersonGroupRootId = (int?)null;
                        udpGroupInPerson.PersonGroupId = (int?)null;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.GroupId).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.PersonGroupId).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.PersonGroupDescription).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.PersonGroupRootId).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.GroupName).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.GroupDescription).IsModified = true;
                        _queryContext.Entry(udpGroupInPerson).Property(p => p.GroupRowVersion).IsModified = true;
                    }
                return true;
            };

        }
        private Func<bool> DeletePersonEmailsEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PersonEmailsEntityCommand)entity.Entity;
                var att = _queryContext.Peoples.FirstOrDefaultAsync(w => w.PersonEmailId == e.Id).Result;

                att.EmailAddress = null;
                att.PersonEmailId = (int?)null;
                att.EmailTypeName = null;
                att.EmailRowVersion= null;
                _queryContext.Peoples.Attach(att);
                _queryContext.Entry(att).Property(p => p.EmailAddress).IsModified = true;
                _queryContext.Entry(att).Property(p => p.PersonEmailId).IsModified = true;
                _queryContext.Entry(att).Property(p => p.EmailTypeName).IsModified = true;
                _queryContext.Entry(att).Property(p => p.EmailRowVersion).IsModified = true;

                return true;
            };

        }
        private Func<bool> DeletePersonGroupsQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PersonGroupsEntityCommand)entity.Entity;
                var item = _queryContext.Peoples.FirstOrDefaultAsync(w => w.PersonGroupId == e.Id).Result;

                item.GroupId = (int?)null;
                item.PersonGroupId = (int?)null;
                item.PersonGroupDescription = (string)null;
                item.PersonGroupRootId = (int?)null;
                item.GroupName = (string)null;
                item.GroupDescription = (string)null;
                item.GroupRowVersion = null;

                _queryContext.Peoples.Attach(item);
                _queryContext.Entry(item).Property(p => p.GroupId).IsModified = true;
                _queryContext.Entry(item).Property(p => p.PersonGroupId).IsModified = true;
                _queryContext.Entry(item).Property(p => p.PersonGroupDescription).IsModified = true;
                _queryContext.Entry(item).Property(p => p.PersonGroupRootId).IsModified = true;
                _queryContext.Entry(item).Property(p => p.GroupName).IsModified = true;
                _queryContext.Entry(item).Property(p => p.GroupDescription).IsModified = true;
                _queryContext.Entry(item).Property(p => p.GroupRowVersion).IsModified = true;

                return true;
            };

        }

        private Func<bool> DeletePeopleQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PeopleEntityCommand)entity.Entity;

                var data = _queryContext.Peoples.AsNoTracking().Where(w => w.Id == e.Id).ToListAsync().Result;
                if (!data.Any())
                {
                    throw new Exception("In query Database Entity not found");
                }
                foreach (var item in data)
                {
                    if (e.IsDeleted)
                    {
                        item.IsDeleted = true;
                        item.PersonRowVersion = e.RowVersion;
                        item.ChangeDate = e.ChangeDate;
                        item.DoingUserName = e.DoingUserName;
                        _queryContext.Peoples.Attach(item);
                        _queryContext.Entry(item).Property(p => p.IsDeleted).IsModified = true;
                        _queryContext.Entry(item).Property(p => p.PersonRowVersion).IsModified = true;
                        _queryContext.Entry(item).Property(p => p.ChangeDate).IsModified = true;
                        _queryContext.Entry(item).Property(p => p.DoingUserName).IsModified = true;
                    }
                    else
                    {
                        _queryContext.Peoples.Remove(item);
                    }
                }

                return true;
            };
        }
        private Func<bool> DeletePersonAddressesQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PersonAddressesEntityCommand)entity.Entity;
                var att = _queryContext.Peoples.FirstOrDefaultAsync(w => w.PersonAddressId == e.Id).Result;

                att.Address = null;
                att.PersonAddressId = (int?)null;
                att.AddressRowVersion = null;

                _queryContext.Peoples.Attach(att);
                _queryContext.Entry(att).Property(p => p.Address).IsModified = true;
                _queryContext.Entry(att).Property(p => p.PersonAddressId).IsModified = true;
                _queryContext.Entry(att).Property(p => p.AddressRowVersion).IsModified = true;

                return true;
            };

        }

        private Func<bool> DeletePersonPhonesQueryEntity(EntityEntry entity)
        {
            return () =>
            {
                var e = (PersonPhonesEntityCommand)entity.Entity;
                var att = _queryContext.Peoples.FirstOrDefaultAsync(w => w.PersonPhoneId == e.Id).Result;

                att.PhoneNumber = null;
                att.PersonPhoneId = (int?)null;
                att.PhoneRowVersion = null;

                _queryContext.Peoples.Attach(att);
                _queryContext.Entry(att).Property(p => p.PhoneNumber).IsModified = true;
                _queryContext.Entry(att).Property(p => p.PersonPhoneId).IsModified = true;
                _queryContext.Entry(att).Property(p => p.PhoneRowVersion).IsModified = true;

                return true;
            };

        }

    }

}
