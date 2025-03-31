using CrudPeople.CoreDomain.Contracts.PersonType.Models;

namespace CrudPeople.CoreDomain.Contracts.PersonType
{
    public interface IPersonTypeRepository
    {
        Task<List<PersonTypeResponseModel>> GetList();
        Task<PersonTypeResponseModel> GetById(byte id);
    }
}
