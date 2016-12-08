using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlaygroundBackend.Model.Abstractions;

namespace PlaygroundBackend.Model.ViewModels
{
    public class TodoPriorityViewModel : IViewModel
    {
        public int Id { get; set; }

        public string Designation { get; set; }
    }
}
