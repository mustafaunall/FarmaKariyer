using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Services;
using System.Globalization;
using WebUI.Extensions;
using DataAccess.Context;
using Domain.Model;
using IdentityServer4.Extensions;

namespace WebUI.Controllers
{
    public class PayTRController : BaseController
    {
        private readonly bool IsLocal = false;
        private const float BoostPrice = 250.0f;

        private readonly string MerchantId = string.Empty;
        private readonly string MerchantKey = string.Empty;
        private readonly string MerchantSalt = string.Empty;
        private readonly string Currency = string.Empty;
        private readonly string TestMode = string.Empty;

        private readonly UserDbContext _userContext;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly MailService _mailService;
        private readonly PdfService _pdfService;
        //private readonly MailContentService _mailContentService;
        private readonly DbService _dbService;

        public PayTRController(
            IConfiguration configuration,
            UserDbContext userContext,
            UserManager<ApplicationUser> userManager,
            //MailService mailService,
            PdfService pdfService,
            //MailContentService mailContentService,
            DbService dbService
            )
        {
            MerchantId = configuration["PayTRSettings:MerchantId"];
            MerchantKey = configuration["PayTRSettings:MerchantKey"];
            MerchantSalt = configuration["PayTRSettings:MerchantSalt"];
            Currency = configuration["PayTRSettings:Currency"];
            TestMode = configuration["PayTRSettings:TestMode"];
            _userContext = userContext;
            _userManager = userManager;
            //_mailService = mailService;
            _pdfService = pdfService;
            //_mailContentService = mailContentService;
            _dbService = dbService;
        }

        [NonAction]
        private string GetURL()
        {
            var url = $"{Request.Scheme}://{Request.Host}";
            return url;
        }

        //public IActionResult SatinAl(
        //    [FromQuery] string? islem,
        //    [FromQuery] string? kategori,
        //    [FromQuery] string? ilan
        //    )
        //{

        //    if (
        //        string.IsNullOrEmpty(islem)
        //        || string.IsNullOrEmpty(kategori)
        //        || string.IsNullOrEmpty(ilan)
        //        )
        //        return BadRequest();

        //    TempData["islem"] = islem;
        //    TempData["kategori"] = kategori;
        //    TempData["ilan"] = ilan;
        //    TempData.Keep();
        //    return RedirectToAction(nameof(Paytr));
        //}

        public async Task<IActionResult> Buy(
            [FromQuery] string? proc,
            [FromQuery] int? categoryId,
            [FromQuery] int? advertId
            )
        {
            var url = GetURL();
            ViewBag.Session = HttpContext.Session.GetString("tokenn");

            float ucret = 0;
            if (proc == "advert")
            {
                var advertCategory = _userContext.AdvertCategories.FirstOrDefault(x => x.Id == categoryId);
                ucret = advertCategory!.Price;
            }
            else if (proc == "boost")
            {
                ucret = BoostPrice;
            }


            if (ucret == 0)
                return BadRequest("Fiyat 0 (sıfır) olamaz! PayTR sisteminde geçici bir hata olduğu gözlemleniyor, lütfen yöneticiye başvurun.");

            TempData.Keep();

            var u = await _userManager.GetUserAsync(User);
            var user = await _userManager.Users
                .SingleAsync(x => x.Email == u.Email);

            // ####################### DÜZENLEMESİ ZORUNLU ALANLAR #######################
            //
            // API Entegrasyon Bilgileri - Mağaza paneline giriş yaparak BİLGİ sayfasından alabilirsiniz.
            string merchant_id = MerchantId;
            string merchant_key = MerchantKey;
            string merchant_salt = MerchantSalt;
            //
            // Müşterinizin sitenizde kayıtlı veya form vasıtasıyla aldığınız eposta adresi
            string emailstr = user.Email;
            string ad = user.Name;
            string tel = user.PhoneNumber;
            //
            // Tahsil edilecek tutar. 9.99 için 9.99 * 100 = 999 gönderilmelidir.
            float? payment_amountstrFloat = ucret * 100;
            int payment_amountstr = Convert.ToInt32(payment_amountstrFloat);
            //
            // Sipariş numarası: Her işlemde benzersiz olmalıdır!! Bu bilgi bildirim sayfanıza yapılacak bildirimde geri gönderilir.
            Random rand = new Random();
            var rand1 = rand.Next(50000000, 100000000);
            string merchant_oid = (rand1).ToString();
            var order = new Order
            {
                ApplicationUserId = user.Id,
                Date = DateTime.Now,
                Price = ucret,
                PayTrOrderId = merchant_oid,
                OrderStatus = OrderStatusType.CREATED,
            };
            if (proc == "advert")
            {
                order.AdvertCategoryId = categoryId;
                order.OrderType = OrderTypeEnum.ADDQUOTA;
            }
            else if (proc == "boost")
            {
                order.AdvertId = advertId;
                order.OrderType = OrderTypeEnum.BOOST;
            }
            await _userContext.Orders.AddAsync(order);
            await _userContext.SaveChangesAsync();
            //
            // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız ad ve soyad bilgisi
            string user_namestr = ad;
            //
            // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız adres bilgisi
            string user_addressstr = "Nizip";
            //
            // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız telefon bilgisi
            string user_phonestr = tel;
            //
            // Başarılı ödeme sonrası müşterinizin yönlendirileceği sayfa
            // !!! Bu sayfa siparişi onaylayacağınız sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
            // !!! Siparişi onaylayacağız sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
            //var domain = Request.Url.GetLeftPart(UriPartial.Authority);
            var domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
            string merchant_ok_url = $"{url}/PayTR/Bildirim";
            //
            // Ödeme sürecinde beklenmedik bir hata oluşması durumunda müşterinizin yönlendirileceği sayfa
            // !!! Bu sayfa siparişi iptal edeceğiniz sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
            // !!! Siparişi iptal edeceğiniz sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
            //HtmlHelper helper = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "Index"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            string merchant_fail_url = $"{url}/PayTR/Basarisiz";
            //        
            // !!! Eğer bu örnek kodu sunucuda değil local makinanızda çalıştırıyorsanız
            // buraya dış ip adresinizi (https://www.whatismyip.com/) yazmalısınız. Aksi halde geçersiz paytr_token hatası alırsınız.
            //string user_ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //string user_ip = HttpContext.Connection.RemoteIpAddress.ToString();
            //string user_ip = "160.20.108.234";

            string user_ip = string.Empty;

            if (IsLocal)
            {
                using (var client = new HttpClient())
                {
                    user_ip = await client.GetStringAsync("https://icanhazip.com");
                    user_ip = user_ip.Trim();
                }
            }
            else
            {
                user_ip = HttpContext.Request.GetIpAddress();
            }

            // İşlem zaman aşımı süresi - dakika cinsinden
            string timeout_limit = "30";
            //
            // Hata mesajlarının ekrana basılması için entegrasyon ve test sürecinde 1 olarak bırakın. Daha sonra 0 yapabilirsiniz.
            string debug_on = "0";
            //
            // Mağaza canlı modda iken test işlem yapmak için 1 olarak gönderilebilir.
            string test_mode = TestMode;
            //
            // Taksit yapılmasını istemiyorsanız, sadece tek çekim sunacaksanız 1 yapın
            string no_installment = "0";
            //
            // Sayfada görüntülenecek taksit adedini sınırlamak istiyorsanız uygun şekilde değiştirin.
            // Sıfır (0) gönderilmesi durumunda yürürlükteki en fazla izin verilen taksit geçerli olur.
            string max_installment = "2";
            //
            // Para birimi olarak TL, EUR, USD gönderilebilir. USD ve EUR kullanmak için kurumsal@paytr.com 
            // üzerinden bilgi almanız gerekmektedir. Boş gönderilirse TL geçerli olur.
            string currency = Currency;
            //
            // Türkçe için tr veya İngilizce için en gönderilebilir. Boş gönderilirse tr geçerli olur.
            string lang = "tr";

            object[][] user_basket = {
                new object[] {"FarmaKariyer Dijital Ürün", ucret.ToString(), 1}, // Ürün (Ürün Ad - Birim Fiyat - Adet)
            };

            NameValueCollection data = new NameValueCollection();
            data["merchant_id"] = merchant_id;
            data["user_ip"] = user_ip;
            data["merchant_oid"] = merchant_oid;
            data["email"] = emailstr;
            data["payment_amount"] = payment_amountstr.ToString();

            string user_basket_json = JsonConvert.SerializeObject(user_basket);
            string user_basketstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(user_basket_json));
            data["user_basket"] = user_basketstr;

            string Birlestir = string.Concat(merchant_id, user_ip, merchant_oid, emailstr, payment_amountstr.ToString(), user_basketstr, no_installment, max_installment, currency, test_mode, merchant_salt);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
            data["paytr_token"] = Convert.ToBase64String(b);
            var tokenn = Convert.ToBase64String(b);

            data["debug_on"] = debug_on;
            data["test_mode"] = test_mode;
            data["no_installment"] = no_installment;
            data["max_installment"] = max_installment;
            data["user_name"] = user_namestr;
            data["user_address"] = user_addressstr;
            data["user_phone"] = user_phonestr;
            data["merchant_ok_url"] = merchant_ok_url;
            data["merchant_fail_url"] = merchant_fail_url;
            data["timeout_limit"] = timeout_limit;
            data["currency"] = currency;
            data["lang"] = lang;


            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] result = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
                string ResultAuthTicket = Encoding.UTF8.GetString(result);
                dynamic json = JValue.Parse(ResultAuthTicket);


                if (json.status == "success")
                {
                    HttpContext.Session.SetString("tokenn", $"https://www.paytr.com/odeme/guvenli/{json.token}");
                }
                else
                {
                    return BadRequest($"PAYTR IFRAME failed. reason:{json.reason}");
                }

            }


            return View();
        }

        public IActionResult Bildirim()
        {
            //var u = await _userManager.GetUserAsync(User);
            //var user = await _userManager.Users
            //    .SingleAsync(x => x.Email == u.Email);

            Notification("Satın alımınız başarıyla gerçekleşti, ilan hakkınız profilinize otomatik olarak tanımlandı.", NotificationType.Success);
            return Redirect($"/Pharmacy/Account/Profile");
        }

        public IActionResult Basarisiz()
        {
            Notification("Ödeme başarısız, tekrar deneyiniz!", NotificationType.Error);
            return Redirect("/Pharmacy/Account/Profile");
        }

        [HttpPost]
        public async Task<IActionResult> Bildirim(object a)
        {
            string merchant_key = MerchantKey;
            string merchant_salt = MerchantSalt;
            string merchant_oid = Request.Form["merchant_oid"];
            string status = Request.Form["status"];
            string total_amount = Request.Form["total_amount"];
            string hash = Request.Form["hash"];

            string Birlestir = string.Concat(merchant_oid, merchant_salt, status, total_amount);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
            string token = Convert.ToBase64String(b);

            var order = await _userContext.Orders
                .Include(x => x.ApplicationUser)
                .Include(x => x.AdvertCategory)
                .SingleAsync(x => x.PayTrOrderId == merchant_oid);

            if (hash.ToString() != token)
            {
                order.OrderStatus = OrderStatusType.FAILED;
                await _userContext.SaveChangesAsync();
                return BadRequest("PAYTR notification failed: bad hash");
            }

            if (status == "success")
            {
                //Ödeme Onaylandı

                if (order.OrderStatus == OrderStatusType.APPROVED)
                {
                    return Ok("OK");
                }


                if (order.OrderType == OrderTypeEnum.ADDQUOTA)
                {
                    var userId = order.ApplicationUserId;
                    var advertCategoryId = order.AdvertCategoryId;
                    var user = await _userContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    var advertCategory = await _userContext.AdvertCategories.FirstOrDefaultAsync(x => x.Id == advertCategoryId);
                    var quotaCount = advertCategory!.QuotaCount;
                    if (user != null)
                    {
                        if (quotaCount == -1)
                            user.AdvertPostingQuota = quotaCount;
                        else
                            user.AdvertPostingQuota = user.AdvertPostingQuota + quotaCount;
                    }
                }
                else if (order.OrderType == OrderTypeEnum.BOOST)
                {
                    var userId = order.ApplicationUserId;
                    var user = await _userContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    var advert = await _userContext.Adverts.FirstOrDefaultAsync(x => x.Id == order.AdvertId);
                    if (user != null && advert != null)
                    {
                        advert.IsBoosted = true;
                        advert.BoostCreateDate = DateTime.Now;
                    }
                }

                order.OrderStatus = OrderStatusType.APPROVED;
                await _userContext.SaveChangesAsync();

                HttpContext.Session.SetString("tokenn", token);

                return Ok("OK");
            }
            else
            {
                //Ödemeye Onay Verilmedi
                order.OrderStatus = OrderStatusType.FAILED;
                await _userContext.SaveChangesAsync();
                return Ok("OK");
            }
        }
    }
}
