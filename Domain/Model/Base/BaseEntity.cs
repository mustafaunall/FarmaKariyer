using System.ComponentModel.DataAnnotations;

namespace Domain.Model.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
