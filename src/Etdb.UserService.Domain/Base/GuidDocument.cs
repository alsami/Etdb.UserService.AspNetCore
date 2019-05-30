using System;
using Etdb.ServiceBase.Domain.Abstractions.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace Etdb.UserService.Domain.Base
{
    public abstract class GuidDocument : IDocument<Guid>
    {
        [BsonId]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public Guid Id { get; protected set; }

        protected GuidDocument(Guid id)
        {
            this.Id = id;
        }
    }
}