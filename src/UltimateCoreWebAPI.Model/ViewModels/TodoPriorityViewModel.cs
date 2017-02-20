using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateCoreWebAPI.Model.Abstractions;

namespace UltimateCoreWebAPI.Model.ViewModels
{
    public class TodoPriorityViewModel : IViewModel
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }
    }
}
