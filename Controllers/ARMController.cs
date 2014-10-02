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
        const string JwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjVUa0d0S1JrZ2FpZXpFWTJFc0xDMmdPTGpBNCJ9.eyJhdWQiOiJodHRwczovL21hbmFnZW1lbnQuY29yZS53aW5kb3dzLm5ldC8iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLXBwZS5uZXQvZGUzNzEwMTAtZTgwYy00MjU3LThmZGMtNGJmYTRkNmVmZTA4LyIsImlhdCI6MTQxMjIzMDIyNiwibmJmIjoxNDEyMjMwMjI2LCJleHAiOjE0MTIyMzQxMjYsInZlciI6IjEuMCIsInRpZCI6ImRlMzcxMDEwLWU4MGMtNDI1Ny04ZmRjLTRiZmE0ZDZlZmUwOCIsImFtciI6WyJwd2QiXSwiYWx0c2VjaWQiOiIxOmxpdmUuY29tOjAwMDNCRkZEQzNGODhBQjAiLCJpZHAiOiJsaXZlLmNvbSIsIm9pZCI6IjMwYjE2ZTk4LTg0ZWItNDM3Ny05ZDFjLTM2NjY1MDdjNmRjYSIsInN1YiI6IldDenpSeklGeGtIbW1zZmYyX2dWcVpoMmk3Y3VhQmZfa2l5V1RpUThaaGMiLCJlbWFpbCI6ImF1eHRtMjMwQGxpdmUuY29tIiwibmFtZSI6ImF1eHRtMjMwQGxpdmUuY29tIiwicHVpZCI6IjEwMDMzRkZGOEFGRjUxOEUiLCJ1bmlxdWVfbmFtZSI6ImxpdmUuY29tI2F1eHRtMjMwQGxpdmUuY29tIiwiYXBwaWQiOiIxOTUwYTI1OC0yMjdiLTRlMzEtYTljZi03MTc0OTU5NDVmYzIiLCJhcHBpZGFjciI6IjAiLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJhY3IiOiIxIn0.PpyZ8TX48QXQ8kPP5gh_YQ4fOf0j04g60gNJF1hVtqH35POVe4Lcb2K7GNiXSUcCuCYbiljkZxXjoThQomS8zo3Zsx8stj0GgWDyEYuP1M64Xf6676zBK0eUkdvTCokibCaM76DI3719DBO0JG8trWVXXChRYxT3mCrUVksJY886vN_kLnlOYa7s4znCMX_uTV6XRh3VLPr_v2faJ2S8-8K61aUI_W67x2C-_xsWD_9972hm5S-Yz_sD3onmp8_LQ3YczNeJMKale1QdJ11aNF3wcZilcF0XRTDXZ4ZV24i_vkKvcouawkend60YyS91k7muL_GpVGu083GAhU8Obw";
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