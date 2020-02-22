using System;
using Etdb.ServiceBase.Domain.Abstractions.Documents;

namespace Etdb.UserService.Domain.Base
{
    public abstract class GuidDocument : IDocument<Guid>
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public Guid Id { get; protected set; }

        protected GuidDocument(Guid id)
        {
            this.Id = id;
        }
    }
}