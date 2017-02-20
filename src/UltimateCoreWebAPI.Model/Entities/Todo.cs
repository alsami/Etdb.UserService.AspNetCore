using System;
using UltimateCoreWebAPI.Model.Abstractions;

namespace UltimateCoreWebAPI.Model.Entities
{
    public class Todo : IEntity
    {
        public Guid Id { get; set; }

        public string Task { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public Guid TodoListId { get; set; }

        public TodoList TodoList  { get; set; }

        public Guid TodoPriorityId { get; set; }

        public TodoPriority TodoPriority { get; set; }
    }
}