using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace SEOService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeoController : Controller
    {
        private Dictionary<int, string> UrlList;
        const string tokenCustomer = "123456";
        public SeoController()
        {
            UrlList = new Dictionary<int,string>();
            for (int i = 0; i < 20; i++)
            {
                var link = $"https://www.link{i}.com/link{i}";
                var text = $"link{i}";
                UrlList.Add(i,$"<a href='{link}'>{text}</a>");
            }
        }
        [HttpGet]
        public IActionResult Index(string token)
        {
            string urlsCustomer=string.Empty;
            if (isAuthenticated(token))
            {
                Random rnd = new Random();
                int customerRandomNumber = rnd.Next(0, UrlList.Count-2);
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
