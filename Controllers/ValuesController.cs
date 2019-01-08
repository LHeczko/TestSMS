using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet()]
        public OkResult Get()
        {
            return Ok();
        }

        [HttpGet("SendSMS")]
        public OkObjectResult SendSMS()
        {
            System.Net.WebRequest vZadanie;
            Byte[] vTablicaZnakow;

            String wynik="";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;

                vZadanie = System.Net.WebRequest.Create("https://api.smslabs.net.pl/apiSms/sendSms");
                vZadanie.Headers.Add("Authorization", "Basic NjYxNGY4ZTYwNDU0YTdmMmNjMjliODNjNmE3NWVkOTYzOWQxYWZhZTpmOTBkMmZjMzgxZGJmMmI2NTYzZWNlOTAzZDA2MjExYmI3NDc5YzA2");
                vZadanie.Timeout = 1200000;
                vZadanie.Method = "PUT";
                vZadanie.ContentType = "application/x-www-form-urlencoded";

                vTablicaZnakow = System.Text.Encoding.UTF8.GetBytes("flash=0&expiration=0&phone_number=%2B48601172078&sender_id=SMS%20TEST&message=Test");

                vZadanie.ContentLength = vTablicaZnakow.Length;

                System.IO.Stream vStrumien2 = vZadanie.GetRequestStream();
                vStrumien2.Write(vTablicaZnakow, 0, vTablicaZnakow.Length);
                vStrumien2.Close();

                using (var vOdpowiedz = vZadanie.GetResponse())
                {
                    using (var vStrumien = vOdpowiedz.GetResponseStream())
                    {
                        using (var ms = new System.IO.MemoryStream())
                        {
                            vStrumien.CopyTo(ms);
                            ms.Position = 0;

                            using (var vCzytnik = new System.IO.StreamReader(ms))
                                wynik = vCzytnik.ReadToEnd();
                        }
                    }
                }
            }

            catch (Exception e)
            {
                wynik = e.Message;
            }
            return Ok(wynik);
        }
    }
}
