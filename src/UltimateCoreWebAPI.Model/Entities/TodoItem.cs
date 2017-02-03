using System;
using UltimateCoreWebAPI.Model.Abstractions;

namespace UltimateCoreWebAPI.Model.Entities
{
    public class TodoItem : IPersistedData
    {
        public int Id { get; set; }

        public string Task { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int TodoListId { get; set; }

        public TodoList TodoList  { get; set; }

        public int TodoPriorityId { get; set; }

        public TodoPriority TodoPriority { get; set; }
    }
}