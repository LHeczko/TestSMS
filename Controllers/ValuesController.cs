using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public async System.Threading.Tasks.Task<OkObjectResult> SendSMS()
        {
            string aKomunikat = "";
            bool WyslijSMSTestResult = false;

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;

                var req = new HttpRequestMessage(HttpMethod.Put, "https://api.smslabs.net.pl/apiSms/sendSms");
                req.Headers.Add("Authorization", "Basic NjYxNGY4ZTYwNDU0YTdmMmNjMjliODNjNmE3NWVkOTYzOWQxYWZhZTpmOTBkMmZjMzgxZGJmMmI2NTYzZWNlOTAzZDA2MjExYmI3NDc5YzA2");
                //req.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                req.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("flash=0&expiration=0&phone_number=%2B48601172078&sender_id=SMS%20TEST&message=Test"));

                var fac = (IHttpClientFactory)HttpContext.RequestServices.GetService(typeof(IHttpClientFactory));
                var client = fac.CreateClient();
                client.Timeout = new TimeSpan(0, 20, 0);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var resp = await client.SendAsync(req);
                if (resp.IsSuccessStatusCode)
                {
                    aKomunikat = await resp.Content.ReadAsStringAsync();
                    WyslijSMSTestResult = true;
                }
                else
                {
                    WyslijSMSTestResult = false;
                }


                /*


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
                 */
            }
            catch (Exception e)
            {
                aKomunikat = e.Message + " " + e.InnerException;
                WyslijSMSTestResult = false;
            }
            return Ok(new { WyslijSMSTestResult, aKomunikat });
        }
    }
}
