using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.CoreDomain.Enums
{
    public enum ExceptionType
    {
        NotFound=1,
        Validation=2,
        ThridPartyError=3,
    }
}
