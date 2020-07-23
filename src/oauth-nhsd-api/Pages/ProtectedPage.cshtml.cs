using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace oauth_nhsd_api.Pages
{
    [Authorize]
    public class ProtectedPageModel : PageModel
    {
        public string ResResponse { get; set; }

        public string ResContent { get; set; }

        private readonly IConfiguration _configuration;

        public ProtectedPageModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task OnGet()
        {
            var user = HttpContext.User;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var authenticationInfo = await HttpContext.AuthenticateAsync();

            HttpRequestMessage req = new HttpRequestMessage(System.Net.Http.HttpMethod.Get,
                _configuration["NHSD:APIEndpoint"] + "/Patient/9000000009");

            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await new HttpClient().SendAsync(req);

            ResResponse = response.StatusCode.ToString();
            ResContent = response.ToString();
        }
    }
}