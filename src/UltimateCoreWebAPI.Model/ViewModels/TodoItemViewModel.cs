using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateCoreWebAPI.Model.Abstractions;

namespace UltimateCoreWebAPI.Model.ViewModels
{
    public class TodoItemViewModel : IViewModel
    {
        public int Id { get; set; }

        public string Task { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int TodoListId { get; set; }

        public TodoListViewModel TodoList { get; set; }

        public int TodoPriorityId { get; set; }

        public TodoPriorityViewModel TodoPriority { get; set; }
    }
}
