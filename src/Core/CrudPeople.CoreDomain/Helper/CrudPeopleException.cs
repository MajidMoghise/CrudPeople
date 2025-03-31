using CrudPeople.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.CoreDomain.Helper
{
    public class CrudPeopleException : Exception
    {
        private CrudPeopleException() { }
        private CrudPeopleException(string message) : base(message) { }
        public ExceptionType ExceptionType { get; private set; }
        public static CrudPeopleException NotFound()
        {
            return new CrudPeopleException()
            {
                ExceptionType = ExceptionType.NotFound
            };
        }
        public static CrudPeopleException NotFound(string message)
        {
            return new CrudPeopleException(message)
            {
                ExceptionType = ExceptionType.NotFound
            };
        }
        public static CrudPeopleException Validation(string message)
        {
            return new CrudPeopleException(message)
            {
                ExceptionType = ExceptionType.Validation
            };
        }
        public static CrudPeopleException ThridPartyError(string message)
        {
            return new CrudPeopleException(message)
            {
                ExceptionType = ExceptionType.ThridPartyError
            };
        }

    }
}
