using Domain.Model.Base;

namespace Domain.Model;

public class AdvertCategory : BaseEntity
{
    public int QuotaCount { get; set; }
    public float Price { get; set; }
}
