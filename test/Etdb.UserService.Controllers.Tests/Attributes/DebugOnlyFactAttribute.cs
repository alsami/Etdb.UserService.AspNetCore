using System.Diagnostics;
using Xunit;

namespace Etdb.UserService.Controllers.Tests.Attributes
{
    public class DebugOnlyFactAttribute : FactAttribute
    {
        public DebugOnlyFactAttribute()
        {
            if (Debugger.IsAttached)
            {
                return;
            }

            this.Skip = "ONLY RUNNING IN DEBUG MODE DUDE!";
        }

        public sealed override string Skip { get; set; }
    }
}