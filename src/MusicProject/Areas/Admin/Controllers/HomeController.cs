
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicProject.Areas.Admin.Controllers
{

    [Area(AreaConstants.AdminArea)]
    //[Authorize(Policy = ConstantPolicies.DynamicPermission)]

    [DisplayName("ادمین")]
    public class HomeController : Controller
    {
        // GET: Home
        [DisplayName("ایندکس ادمین")]
        public async Task<ActionResult> Index()
        {
            ViewData["image"] = await startCrawlerasync();
            return View();
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<List<string>> startCrawlerasync()
        {
            //the url of the page
            var url = "https://www.automobile.tn/fr/neuf/bmw";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var divs =
                htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("versions-item")).ToList();
            List<string> Image = new List<string>();
            foreach(var div in divs)
            {
                Image.Add(div.Descendants("source").FirstOrDefault().ChildAttributes("srcset").FirstOrDefault().Value);
            }
            return Image;
        }
    }
}