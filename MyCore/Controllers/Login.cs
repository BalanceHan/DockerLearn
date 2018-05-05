using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCore.Controllers
{
    [Route("api/[controller]")]
    public class Login : Controller
    {
        public async Task<IActionResult> GetLogin()
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:57337");
            if (disco.IsError)
            {
                return Ok("error");
            }
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");

            if (tokenResponse.IsError)
            {
                return Ok(tokenResponse.Error);
            }
            return Ok(tokenResponse.AccessToken);
        }
    }
}
