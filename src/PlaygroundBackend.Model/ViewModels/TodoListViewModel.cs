using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlaygroundBackend.Model.Abstractions;

namespace PlaygroundBackend.Model.ViewModels
{
    public class TodoListViewModel : IViewModel
    {
        public TodoListViewModel()
        {
            this.TodoItems = new List<TodoItemViewModel>();
        }

        public int Id { get; set; }

        public string Designation { get; set; }

        public virtual ICollection<TodoItemViewModel> TodoItems { get; private set; }
    }
}
