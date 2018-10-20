using System;
using Etdb.ServiceBase.Domain.Abstractions.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace Etdb.UserService.Domain.Base
{
    public abstract class GuidDocument : IDocument<Guid>
    {
        protected GuidDocument(Guid id)
        {
            this.Id = id;
        }

        [BsonId] public Guid Id { get; protected set; }
    }
}