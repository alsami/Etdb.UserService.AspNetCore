using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UltimateCoreWebAPI.Model.Abstractions;

namespace UltimateCoreWebAPI.Model.Entities
{
    public class TodoList : IEntity
    {
        public TodoList()
        {
            this.TodoItems = new List<Todo>();
        }

        public Guid Id { get; set; }

        public string Designation { get; set; }

        public virtual ICollection<Todo> TodoItems { get; private set; }
    }
}
