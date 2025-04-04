using CrudPeople.CoreDomain.Entities.People.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Mongo.Context.ModelCreating
{

    public class MongoMappingBuilder<TEntity>
    {
        public string CollectionName { get; private set; }

        public MongoMappingBuilder<TEntity> ToCollection(string collectionName)
        {
            CollectionName = collectionName;
            return this;
        }

        public MongoMappingBuilder<TEntity> WithIndex(Func<TEntity, object> indexExpression)
        {

            return this;
        }
    }
}
