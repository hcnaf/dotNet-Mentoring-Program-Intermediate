using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var context = new ProfileSampleEntities();

            var sources = context.ImgSources.Take(20).Select(x => x.Id);
            
            var model = await context.ImgSources
                .Take(20)
                .Select(x => new ImageModel()
                {
                    Name = x.Name,
                    Data = x.Data
                })
                .ToListAsync();

            return View(model);
        }

        public async Task<ActionResult> Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                var filesToAdd = new List<ImgSource>();

                foreach (var file in files)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        await stream.ReadAsync(buff, 0, (int) stream.Length);

                        filesToAdd.Add(new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        });
                    }
                }

                if (filesToAdd.Any())
                {
                    context.ImgSources.AddRange(filesToAdd);
                    await context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}