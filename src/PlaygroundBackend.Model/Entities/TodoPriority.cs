using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlaygroundBackend.Model.Abstractions;

namespace PlaygroundBackend.Model.Entities
{
    public class TodoPriority : IPersistedData
    {
        public int Id { get; set; }

        public short Prio { get; set; }

        public string Designation { get; set; }
    }
}
