using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicProject.Common;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Content
{
  public class EfPodcastService : IPodcastService
  {
    IUnitOfWork _uow;
    readonly DbSet<Podcast> _Podcasts;
    private readonly IUploadService _uploadService;
    private readonly IHostingEnvironment _hostingEnvironment;

    public EfPodcastService(
      IUnitOfWork uow,
      IUploadService uploadService,
      IHostingEnvironment hostingEnvironment

    )
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));
      _uploadService = uploadService;
      _Podcasts = _uow.Set<Podcast>();
      _hostingEnvironment = hostingEnvironment;

    }
    

    public int AddNewPodcast(Podcast podcast, IFormFile photo, IFormFile video)
    {

      if (video != null)
      {
        if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/podcast"))
          Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/podcast");
        var allowed = new[]
        {
          //"application/pdf", "application/x-pdf"
          "video/mpeg",
          "video/mp4",
          "audio/mp3"
        };
        string fileName = "";
        long fileSize = 0;
        fileName = _uploadService.UploadFile(
          video, "/content/files/podcast",ref fileSize,allowed);
        podcast.Video = fileName;
      }


      if (photo != null)
      {
        var fileName = "";
        long fileSize = 0;
        var size = new[] { 300 };
        var resized = new string[1];
        
        if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/podcast"))
          Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/podcast");
        
        fileName = _uploadService.UploadPicResize(
          photo, "/content/files/podcast",
          1200,
          ref fileSize,
          size,
          EnumC.Dimensions.Width,
          ref resized
        );
        podcast.Pic = fileName;
        podcast.Thumbnail = size[0] + "/" + resized[0];
     
      }





      var id = _Podcasts.Add(podcast);
      _uow.SaveChanges();
      return id.Entity.Id;

    }

 
    public IList<Podcast> GetAllPodcats(int type)
    {
      return _Podcasts.Where(p => p.TypeId == type).ToList();
    }

    public IList<Podcast> GetTopByType(string typeId, int pageNumber, int pageSize, string title)
    {
      var offset = (pageSize * pageNumber) - pageSize;

      var query = string.IsNullOrWhiteSpace(typeId) ?
        _Podcasts :
        _Podcasts.Where(l => l.TypeId == Convert.ToInt32(typeId));

      query = string.IsNullOrEmpty(title) ? query :
        query.Where(l => l.Title.StartsWith(title));

      query = query.OrderByDescending(x => x.Id);
      return query.Skip(offset).Take(pageSize)
                                              .Include(g => g.PodcastTags)
                                              .ThenInclude(t => t.Tag).ToList();
    }

    
    public async Task<IList<Podcast>> GetTopByTypeAsync(string typeId, int pageNumber, int pageSize, int? tagId)
    {
      var offset = (pageSize * pageNumber) - pageSize;
      var query = string.IsNullOrWhiteSpace(typeId) ?
        _Podcasts :
        _Podcasts.Where(l => l.TypeId == Convert.ToInt32(typeId));
      query = query.OrderByDescending(x => x.Id);
      query = tagId == null ? query :
        query.Where(l => l.PodcastTags.Any(a => a.TagId == tagId));

      var result = await query.Skip(offset).Take(pageSize).Include(p => p.PodcastTags)
      .ThenInclude(tag => tag.Tag)
        .ToListAsync();
      return result;
    }
    

    public Podcast GetByTitle(string title)
    {
      return _Podcasts.SingleOrDefault(p => p.Title == title);
    }

    public async Task<PagedPodcastViewModel> GetPagedPodcastAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type,
      bool? archive, string title, int tagId
      )

    {
      var offset = (pageSize * pageNumber) - pageSize;

      var query = string.IsNullOrWhiteSpace(type) ?
        _Podcasts :
        _Podcasts.Where(l => l.TypeId == Convert.ToInt32(type));

      query = archive == null ? query :
        query.Where(l => l.IsPublish == archive);

      query = string.IsNullOrEmpty(title) ? query :
        query.Where(l => l.Title.Contains(title));
      query = tagId == 0 ? query : query.Where(p => p.PodcastTags.Any(t => t.TagId == tagId));
      query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);

      return new PagedPodcastViewModel
      {
        Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
        PodcastItem = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false),
        Title = title
      };
    }


    public Task<Podcast> FindByIdAsync(int id)
    {
      return _Podcasts.Include(p => p.PodcastTags).ThenInclude(p => p.Tag).SingleOrDefaultAsync(p => p.Id == id);
    }

    public void UpdatePodcast(Podcast podcast, IFormFile photo, IFormFile video)
    {

      if (photo != null && photo.Length > 0)
      {
        if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/podcast"))
          Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/podcast");
        string fileName = "";
        long fileSize = 0;
        var size = new[] { 300 };
        var resized = new string[1];
        fileName = _uploadService.UploadPicResize(photo, "/content/files/podcast" ,1200,ref fileSize,size,
                                                  EnumC.Dimensions.Width,
                                                  ref resized
                                                 );
        podcast.Pic = fileName;
        podcast.Thumbnail = size[0] + "/" + resized[0];

      }


      if (video != null)
      {
        var allowed = new[] { "video/mp4",  "audio/mp3", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif" };
        string fileName = "";
        long fileSize = 0;
        fileName = _uploadService.UploadFile(
          video, "/content/files/podcast",
          ref fileSize,
          allowed
        );
        podcast.Video =  fileName;
      }
      _Podcasts.Update(podcast);
      _uow.SaveChanges();
    }


    public void Delete(int id)
    {
      var itemToRemove = _Podcasts.Find(id);
      if (itemToRemove != null)
      {
        _Podcasts.Remove(itemToRemove);
        _uow.SaveChanges();
      }
    }
  }
}
