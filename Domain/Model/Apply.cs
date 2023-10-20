using Domain.Model.Base;

namespace Domain.Model;

public class Apply : BaseEntity
{
    public DateTime ApplyDate { get; set; }

    #region Relation

    public int ApplicantUserId { get; set; }
    public ApplicationUser ApplicantUser { get; set; }
    public int AdvertId { get; set; }
    public Advert Advert { get; set; }
    public int CurrentResumeId { get; set; }
    public Resume CurrentResume { get; set; }

    #endregion
}
