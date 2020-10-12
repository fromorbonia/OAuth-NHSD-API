using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;

namespace oauth_nhsd_api.Pages
{
    [Authorize]
    public class ProtectedPageModel : PageModel
    {
        public string ResResponse { get; set; }

        public string ResContent { get; set; }

        public DateTime SessionExpires { get; set; }

        private readonly IConfiguration _configuration;

        public ProtectedPageModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task OnGet()
        {

            var tokenAccess = await HttpContext.GetTokenAsync("access_token");
            var tokenRefresh = await HttpContext.GetTokenAsync("refresh_token");
            var tokenExpiresAt = await HttpContext.GetTokenAsync("expires_at");

            HttpRequestMessage req = new HttpRequestMessage(System.Net.Http.HttpMethod.Get,
                _configuration["NHSD:APIEndpoint"] + "/Patient/9000000009");

            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);
            req.Headers.Add("NHSD-Session-URID", "TestURID");

            HttpResponseMessage response = await new HttpClient().SendAsync(req);

            ResResponse = string.Format("{0} - {1}", (int)response.StatusCode, response.StatusCode);
            ResContent = await response.Content.ReadAsStringAsync();
            SessionExpires = Convert.ToDateTime(tokenExpiresAt);
        }
    }
}