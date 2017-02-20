using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateCoreWebAPI.Model.Abstractions;

namespace UltimateCoreWebAPI.Model.ViewModels
{
    public class TodoViewModel : IViewModel
    {
        public Guid Id { get; set; }

        public string Task { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public Guid TodoListId { get; set; }

        public Guid TodoPriorityId { get; set; }

        public short TodoPriorityPrio { get; set; }

        public string TodoPriorityDesignation { get; set; }
    }
}
