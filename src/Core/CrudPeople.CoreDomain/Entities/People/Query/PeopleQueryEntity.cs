using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.CoreDomain.Entities.People.Query
{
    public class PeopleQueryEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName{ get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public byte PersonTypeId { get; set; }
        public string PersonTypeName { get; set; }
        public byte[] RowVersion { get; set; }
        public string DoingUserName { get; set; }
    }
}
