using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Core.Entities
{
    public class Bus : BaseEntity
    {
        [StringLength(50)]
        public string Number { get; set; } = string.Empty;
    }
}
