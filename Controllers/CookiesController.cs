using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CookiesController : ControllerBase
    {
        public class Cookie
        {
            public Cookie(string name, string path, string domain)
            {
                Name = name;
                Path = path;
                Domain = domain;
            }

            public string Name { get; set; } 
            public string Path { get; set; }
            public string Domain { get; set; }
        }

        private List<Cookie> _cookies;

        public CookiesController()
        {
            _cookies = new List<Cookie>
            {
                // these cookies will get deleted as expected
                {new Cookie("CookieWithPath", "/path1/", null) },
                {new Cookie("CookieWithPath", "/path2/", null) },

                // only the last cookie will get deleted
                {new Cookie("CookieWithDomainAndPath", "/path1/", "localhost") },
                {new Cookie("CookieWithDomainAndPath", "/path2/", "localhost") },
            };
        }

        [HttpGet]
        public IEnumerable<Cookie> Get()
        {
            _cookies.ForEach(cookie =>
            {
                HttpContext.Response.Cookies.Delete(cookie.Name, new CookieOptions
                {
                    Domain = cookie.Domain,
                    Path = cookie.Path
                });
            });

            return _cookies;
        }
    }
}
