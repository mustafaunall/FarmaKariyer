using System.ComponentModel;

namespace Domain.Model.Enum;

public enum AdvertType
{
    [Description("Teknisyen")]
    TECHNICIAN,
    [Description("Stajyer")]
    INTERN,
    [Description("Yardımcı Eczacı")]
    ASSISTANT,
    [Description("İkinci Eczacı")]
    SECONDARY,
    [Description("Ruhsat Devir")]
    LICENSE,
    [Description("Diğer")]
    OTHER,
}
