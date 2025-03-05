using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities;
public abstract class BaseEntity
{
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime? ModificationDate { get; set; } = null;
    public Guid? ModificatedBy { get; set; } = default!;
    public bool IsDeleted { get; set; } = false;
}