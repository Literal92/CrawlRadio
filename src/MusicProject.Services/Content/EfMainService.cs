using DNTPersianUtils.Core;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicProject.Common;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Entities.Identity;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Content;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicProject.Services.Content
{
    public class EfMainService : IMainService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        IUnitOfWork _uow;
        private readonly IUploadService _uploadService;
        private readonly ICategoryService _categoryService;
        private readonly IContentService _contentService;
        private readonly DbSet<Main> _products;
        public EfMainService(IUnitOfWork uow,
          IUploadService uploadService,
          ICategoryService categoryService,
          IHostingEnvironment hostingEnvironment,
          IContentSelectedService contentSelectedService,
          IContentService contentService
          )
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _uploadService = uploadService;
            _products = _uow.Set<Entities.Content.Main>();
            _categoryService = categoryService;
            _hostingEnvironment = hostingEnvironment;
            _contentService = contentService;
        }
        public IList<Main> GetAllContents()
        {
            return _products.ToList();
        }
        public void Delete(int id)
        {
            var itemToRemove = _products.FirstOrDefault(x => x.Id.Equals(id));
            if (itemToRemove != null)
            {
                _products.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }

        public async Task<bool> CheckDuplicate(string term, string Subterm)
        {
            var item = await _products.Where(x => x.SubTitle == Subterm).FirstOrDefaultAsync();
            if (item == null)
            {
                return false;
            }
            else if (item.Title == term)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task Create(Main items)
        {
            try
            {
                var newItem = new Main
                {
                    Title = items.Title,
                    SubTitle = items.SubTitle,
                    Link = items.Link,
                    Music = items.Music
                };
                await _products.AddAsync(newItem);
                await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<PagedContentItemsViewModel> GetPagedContentsAsync(
            int pageNumber,
            int pageSize,
            SortOrder sortOrder,
            string type,
             bool? archive,
            int categoryId,
            string from,
            string to,
            string title,
            int artisId = 0,
            int styleId = 0,
            int musicId = 0

           )
        {
            var offset = (pageSize * pageNumber) - pageSize;

            var query = _products;



            return new PagedContentItemsViewModel
            {
                Paging =
              {
                  TotalItems = await query.CountAsync().ConfigureAwait(false)
              },
                Mains = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false)
            };
        }



        public async Task startCrawlerasync()
        {
            try
            {
                //the url of the page
                var url = "https://www.radiojavan.com/mp3s";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                var divs =
                    htmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "").Equals("songInfo")).ToList();
                Main data = new Main();
                foreach (var div in divs)
                {
                    foreach (var item in div.Descendants("span"))
                    {
                        if (item.ChildAttributes("class").FirstOrDefault().Value == "artist")
                        {
                            data.Title = item.InnerText;
                        }
                        if (item.ChildAttributes("class").FirstOrDefault().Value == "song")
                        {
                            data.SubTitle = item.InnerText;
                            var charsToRemove = new string[] { "&quot;", "&amp;", "&apos;" };
                            foreach (var c in charsToRemove)
                            {
                                data.Title = data.Title.Replace(c, "");
                                data.Title = data.Title.Replace("  ", " ");
                                data.SubTitle = data.SubTitle.Replace(c, "");
                                data.SubTitle = data.SubTitle.Replace("  ", " ");
                            }
                            data.Title = data.Title.Replace(" ", "-");
                            data.SubTitle = data.SubTitle.Replace(" ", "-");
                            data.Link = "https://www.radiojavan.com/mp3s/mp3/" + data.Title + "-" + data.SubTitle;
                            var check = await CheckDuplicate(data.Title, data.SubTitle);
                            if (!check)
                            {
                                await Create(data);
                            }
                            //get pic
                            var urlPic = "https://www.radiojavan.com/mp3s/mp3/" + data.Title + "-" + data.SubTitle;
                            var htmlPic = await httpClient.GetStringAsync(urlPic);
                            htmlDocument.LoadHtml(htmlPic);
                            var src = htmlDocument.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value;
                            //send to download
                            //https://host1.rj-mw1.com/media/mp3/mp3-256/Masih-Arash-AP-To-Ke-Nisti-Pisham.mp3
                            //var link = "https://host2.rj-mw1.com/media/mp3/mp3-256/Masih-Arash-AP-To-Ke-Nisti-Pisham.mp3";
                            var link = "https://host2.rj-mw1.com/media/mp3/mp3-256/" + data.Title + "-" + data.SubTitle + ".mp3";
                            var res = await DownMusic(link, data.Title, data.SubTitle, src);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public async Task<bool> DownMusic(string link, string Title = null, string SubTitle = null, string src = null)
        {
            try
            {
                //download file
                var main = new Main();
                var net = new System.Net.WebClient();
                var adress = _hostingEnvironment.WebRootPath + "/content/files/" + Title + "/music";
                if (!Directory.Exists(adress))
                    Directory.CreateDirectory(adress);
                net.DownloadFile(link, adress + @"\" + SubTitle + ".mp3");
                net.DownloadFile(src, adress + @"\" + SubTitle + ".jpg");

                //save to content entity
                Title = Title.Replace("-", " ");
                SubTitle = SubTitle.Replace("-", " ");
                var content = new MusicProject.Entities.Content.Content()
                {
                    Title = Title,
                    SubTitle = SubTitle,
                    Mp3320 = "/content/files/" + Title + "/music/" + SubTitle + ".mp3",
                    Pic = "/content/files/" + Title + "/music/" + SubTitle + ".jpg",
                    TypeId = 10
                };
                _contentService.Create(content);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
