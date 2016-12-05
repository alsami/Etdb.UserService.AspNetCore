using PlaygroundBackend.Model.Abstractions;

namespace PlaygroundBackend.Model.Entities
{
    public class TodoItem : IPersistedData
    {
        public int Id { get; set; }

        public string Task { get; set; }

        public int TodoListId { get; set; }

        public TodoList TodoList  { get; set; }
    }
}