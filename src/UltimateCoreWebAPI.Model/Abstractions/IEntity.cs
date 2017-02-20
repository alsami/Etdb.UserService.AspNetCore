using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UltimateCoreWebAPI.Model.Abstractions
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
