using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Etdb.UserService.Extensions.Converters
{
    public class MongoDbRefConverter : JsonConverter
    {        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ICollection<MongoDBRef> refs)
            {
                serializer.Serialize(writer, refs.Select(@ref => new
                {
                    Id = @ref.Id.AsGuid,
                    @ref.CollectionName
                }).ToArray());
                
                return;
            }

            var @refObject = (MongoDBRef) value;
            
            serializer.Serialize(writer, JToken.FromObject(new
            {
                Id = @refObject.Id.AsGuid,
                @refObject.CollectionName
            }));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (existingValue is ICollection<MongoDBRef> refs)
            {
                var jArray = JArray.Load(reader);

                foreach (var token in jArray)
                {
                    refs.Add(new MongoDBRef(token["CollectionName"].ToString(), Guid.Parse(token["Id"].ToString())));
                }
                
                return refs;
            }

            var jToken = JToken.Load(reader);

            return new MongoDBRef(jToken["CollectionName"].ToString(), Guid.Parse(jToken["Id"].ToString()));
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(MongoDBRef).IsAssignableFrom(objectType) || 
                typeof(IEnumerable<MongoDBRef>).IsAssignableFrom(objectType);
        }
    }
}