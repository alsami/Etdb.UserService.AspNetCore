using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateCoreWebAPI.Model.Abstractions;

namespace UltimateCoreWebAPI.Model.ViewModels
{
    public class TodoListViewModel : IViewModel
    {
        public TodoListViewModel()
        {
            this.Todos = new List<TodoViewModel>();
        }

        public Guid Id { get; set; }

        public string Designation { get; set; }

        public virtual ICollection<TodoViewModel> Todos { get; private set; }

    }
}
