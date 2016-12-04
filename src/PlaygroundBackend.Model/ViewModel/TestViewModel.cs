using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlaygroundBackend.Model.Abstractions;

namespace PlaygroundBackend.Model.ViewModel
{
    public class TestViewModel : IViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
