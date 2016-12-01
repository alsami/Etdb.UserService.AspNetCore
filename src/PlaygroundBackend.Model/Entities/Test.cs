using PlaygroundBackend.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaygroundBackend.Model.Entities
{
    public class Test : IPersistedData
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
