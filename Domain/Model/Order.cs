using Domain.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class Order : BaseEntity
{
    public DateTime Date { get; set; }
    public float Price { get; set; }
    public string PayTrOrderId { get; set; }
    public OrderStatusType OrderStatus { get; set; }

    #region Relation

    public int ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int AdvertCategoryId { get; set; }
    public AdvertCategory AdvertCategory { get; set; }

    #endregion
}

public enum OrderStatusType
{
    CREATED,
    APPROVED,
    FAILED,
}
