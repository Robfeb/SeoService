using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SEOService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SeoController : Controller
    {

        private Dictionary<int, string> UrlList;
        const string tokenCustomer = "123456";
        IConfiguration Configuration;
        public SeoController(IConfiguration configuration)
        {
            Configuration = configuration;
            UrlList = new Dictionary<int, string>();
            for (int i = 0; i < 20; i++)
            {
                var link = $"https://www.link{i}.com/link{i}";
                var text = $"link{i}";
                UrlList.Add(i, $"<a href='{link}'>{text}</a>");
            }
        }
        [HttpGet]
        public IActionResult Index(string token)
        {
            return GetLinks(token);
        }

        [HttpGet]
        [Route("scriptWithCode.js")]
        public IActionResult ScriptCode()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            var URL = Configuration["UrlBase"].ToString();
            
            string scriptContent = "var getData = fetch('"+URL+ "?token=123456').then(function(response) {return response.text()}).then(function(body) {var divLinks = document.createElement('div');divLinks.id = 'div_links';divLinks.innerHTML = body;document.body.appendChild(divLinks);return console.log(body);});";
            //scriptContent = "const getData = fetch('{URL}').then(response => response.json).then(jsonObject => console.log(jsonObject));";

            return Content(scriptContent);
        }

        [HttpGet]
        [Route("script.js")]
        public IActionResult Script()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            var URL = Configuration["UrlBase"].ToString();

            string scriptContent = "var key='000000'; if (typeof token !== 'undefined') {key=token;}var getData = fetch('" + URL + "?token='+key).then(function(response) {return response.text()}).then(function(body) {var divLinks = document.createElement('div');divLinks.id = 'div_links';divLinks.innerHTML = body;document.body.appendChild(divLinks);return console.log(body);});";
            //scriptContent = "const getData = fetch('{URL}').then(response => response.json).then(jsonObject => console.log(jsonObject));";

            return Content(scriptContent);
        }
        private IActionResult GetLinks(string token)
        {
            string urlsCustomer = string.Empty;
            if (isAuthenticated(token))
            {
                Random rnd = new Random();
                int customerRandomNumber = rnd.Next(0, UrlList.Count - 2);
                for (int i = customerRandomNumber; i < UrlList.Count; i++)
                {
                    urlsCustomer += UrlList[i];
                }
            }
            return Content(urlsCustomer);
        }
        private bool isAuthenticated(string token)
        {
            return !string.IsNullOrEmpty(token) && tokenCustomer.Equals(token);
        }
    }
}
