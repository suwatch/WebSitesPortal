using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebSitesPortal.Controllers
{
    public class ARMController : ApiController
    {
        const string JwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjVUa0d0S1JrZ2FpZXpFWTJFc0xDMmdPTGpBNCJ9.eyJhdWQiOiJodHRwczovL21hbmFnZW1lbnQuY29yZS53aW5kb3dzLm5ldC8iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLXBwZS5uZXQvZGUzNzEwMTAtZTgwYy00MjU3LThmZGMtNGJmYTRkNmVmZTA4LyIsImlhdCI6MTQxMjI2Njg3NywibmJmIjoxNDEyMjY2ODc3LCJleHAiOjE0MTIyNzA3NzcsInZlciI6IjEuMCIsInRpZCI6ImRlMzcxMDEwLWU4MGMtNDI1Ny04ZmRjLTRiZmE0ZDZlZmUwOCIsImFtciI6WyJwd2QiXSwiYWx0c2VjaWQiOiIxOmxpdmUuY29tOjAwMDNCRkZEQzNGODhBQjAiLCJpZHAiOiJsaXZlLmNvbSIsIm9pZCI6IjMwYjE2ZTk4LTg0ZWItNDM3Ny05ZDFjLTM2NjY1MDdjNmRjYSIsInN1YiI6IldDenpSeklGeGtIbW1zZmYyX2dWcVpoMmk3Y3VhQmZfa2l5V1RpUThaaGMiLCJlbWFpbCI6ImF1eHRtMjMwQGxpdmUuY29tIiwibmFtZSI6ImF1eHRtMjMwQGxpdmUuY29tIiwicHVpZCI6IjEwMDMzRkZGOEFGRjUxOEUiLCJ1bmlxdWVfbmFtZSI6ImxpdmUuY29tI2F1eHRtMjMwQGxpdmUuY29tIiwiYXBwaWQiOiIxOTUwYTI1OC0yMjdiLTRlMzEtYTljZi03MTc0OTU5NDVmYzIiLCJhcHBpZGFjciI6IjAiLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJhY3IiOiIxIn0.VKz3uix7UJ8kiP0qiaYQKRv1fkrK6J9epSGFduGT21PJi-QwZ1lbp8ztBGLA-nWIUbuLrMOo2AW92U2fSTmZukSnYJSMD8-dp7Bqh8IG9tgcGGw4MnjqwRN83tjXw612I3ut3lrlhxFc2X93n18aVRb16de4ITi24uaQA1AAwt9m3zVCPMbB9s0sRLEJ0xbbukwEiphLOpp8Z-Vzv8eBnMR3tloM_i-MPIUEXmEFclGi2L-XsD1YWz7gCFyx4F0mAfXTv1u13OQhz0e9AY2fiOmkxXRgiQS1nXfFK3ZOHOG8PWAlCWAgx6snCTLInXgKbRJHmzJ96qUN9gCHrAnQvw";
        const string ScmUri = "https://migrate.scm.kudu1.antares-test.windows-int.net/";
        //const string ArmUri = "https://api-next.resources.windows-int.net";
        const string ArmUri = "https://api-current.resources.windows-int.net";
        //const string ArmUri = "https://api-dogfood.resources.windows-int.net";
        //const string ArmUri = "https://management.azure.com";

        static ARMController()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public async Task<HttpResponseMessage> GetTenants()
        {
            using (var client = GetClient(ScmUri ?? Request.RequestUri.GetLeftPart(UriPartial.Authority)))
            {
                return await client.GetAsync("tenantdetails");
            }
        }

        public async Task<HttpResponseMessage> Get()
        {
            IHttpRouteData routeData = Request.GetRouteData();
            string path = routeData.Values["path"] as string;
            if (String.IsNullOrEmpty(path))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "root path is not supported");
            }

            using (var client = GetClient(ArmUri ?? "https://management.azure.com"))
            {
                return await client.GetAsync(path + "?api-version=2014-04-01");
            }
        }

        public static HttpClient GetClient(string baseUri)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            return client;
        }
    }
}