using Domain.Model;

namespace Domain.ViewModel.Home;

public class HomeVM
{
    public List<Advert>? Last3Advert { get; set; }
    public List<Advert>? BoostedAdvertsTechnician { get; set; }
    public List<Advert>? BoostedAdvertsDermocosmetic { get; set; }
    public List<Advert>? BoostedAdvertsIntern { get; set; }
    public List<Advert>? BoostedAdvertsLicense { get; set; }
    public List<Advert>? BoostedAdvertsOther { get; set; }
    public float PackagePrice1 { get; set; } = 0.0f;
    public float PackagePrice2 { get; set; } = 0.0f;
    public float PackagePrice3 { get; set; } = 0.0f;
}
