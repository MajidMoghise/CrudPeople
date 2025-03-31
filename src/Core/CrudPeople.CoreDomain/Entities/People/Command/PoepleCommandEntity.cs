using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.CoreDomain.Entities.People.Command
{
    public class PeopleCommandEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public byte PersonTypeId{ get; set; }
        public byte[] RowVersion { get; set; }
        public PersonType PersonType { get; set; }
        public DateTime ChangeDate { get; set; }
        public string DoingUserName { get; set; }
    }
}
