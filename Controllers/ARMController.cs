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
        const string JwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjVUa0d0S1JrZ2FpZXpFWTJFc0xDMmdPTGpBNCJ9.eyJhdWQiOiJodHRwczovL21hbmFnZW1lbnQuY29yZS53aW5kb3dzLm5ldC8iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLXBwZS5uZXQvZGUzNzEwMTAtZTgwYy00MjU3LThmZGMtNGJmYTRkNmVmZTA4LyIsImlhdCI6MTQxMjIzNTg4NCwibmJmIjoxNDEyMjM1ODg0LCJleHAiOjE0MTIyMzk3ODQsInZlciI6IjEuMCIsInRpZCI6ImRlMzcxMDEwLWU4MGMtNDI1Ny04ZmRjLTRiZmE0ZDZlZmUwOCIsImFtciI6WyJwd2QiXSwiYWx0c2VjaWQiOiIxOmxpdmUuY29tOjAwMDNCRkZEQzNGODhBQjAiLCJpZHAiOiJsaXZlLmNvbSIsIm9pZCI6IjMwYjE2ZTk4LTg0ZWItNDM3Ny05ZDFjLTM2NjY1MDdjNmRjYSIsInN1YiI6IldDenpSeklGeGtIbW1zZmYyX2dWcVpoMmk3Y3VhQmZfa2l5V1RpUThaaGMiLCJlbWFpbCI6ImF1eHRtMjMwQGxpdmUuY29tIiwibmFtZSI6ImF1eHRtMjMwQGxpdmUuY29tIiwicHVpZCI6IjEwMDMzRkZGOEFGRjUxOEUiLCJ1bmlxdWVfbmFtZSI6ImxpdmUuY29tI2F1eHRtMjMwQGxpdmUuY29tIiwiYXBwaWQiOiIxOTUwYTI1OC0yMjdiLTRlMzEtYTljZi03MTc0OTU5NDVmYzIiLCJhcHBpZGFjciI6IjAiLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJhY3IiOiIxIn0.a5RUavG8CbCOTRN2NPBzmEfrmMK2k890hycgNIDzbvycXE8A8Z-YJQkq8tYXqVENEB47ZKZNsqFlCfTouFFNHA9Kfv1fYFqBTGRF0d8DWXaZOgbOulaYXHCMVbVu7vcb4Xpz7oyXk_vwlWYnU55DUixvfRSyUCBhqju9Qqp-zusCJpAa4qr-Y1oRJSrGKw-0hTVB-abVd9aw--6soBiuuHUpeAU4Cm1JGxHSouRgCJIouvrRUnmE4O8nx9olyMFfk7_JttGu_5zqEH8wpHJ8EqX2WNsZXqtdlzC1mW1iGoamC_NUmtGAph1kHqcFZwQsf6j3398Z3mopyCe1Fp7S_w";
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