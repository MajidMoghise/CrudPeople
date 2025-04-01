using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.FilterSearch
{
    public enum FilterOperator
    {
        Equal = 1,
        Greater = 2,
        Smaller = 3,
        GreaterEqual = 4,
        SmallerEqual = 5,
        Unequal = 6,
        LikeFromBothSides = 7,
        LikeFromBeginning = 8,
        LikeFromEnd = 9
    }
}
