using System;
using System.Runtime.CompilerServices;
using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Scaffold
{
    public class TestImmutable
    {
        private readonly Guid id;
        private readonly string someValue;

        public TestImmutable(Guid id, string someValue)
        {
            this.id = id;
            this.someValue = someValue;
        }

        public Guid Id => this.id;

        public string SomeValue => this.someValue;
    }
}