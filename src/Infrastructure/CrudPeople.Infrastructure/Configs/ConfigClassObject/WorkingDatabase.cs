using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Configs.ConfigClassObject
{
    internal class WorkingDatabase
    {
        public bool Sql { get; set; }
        public bool MongoDb { get; set; }
    }
}
