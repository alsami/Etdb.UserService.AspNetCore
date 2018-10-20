using System;

namespace Etdb.UserService.Scaffold
{
    public class TestImmutable
    {
        public TestImmutable(Guid id, string someValue)
        {
            this.Id = id;
            this.SomeValue = someValue;
        }

        public Guid Id { get; }

        public string SomeValue { get; }
    }
}