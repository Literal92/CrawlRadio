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
  public class EfSliderService : ISliderService
  {
    IUnitOfWork _uow;
    readonly DbSet<Slider> _Sliders;
    private readonly IUploadService _uploadService;
    private readonly IHostingEnvironment _hostingEnvironment;

    public EfSliderService(
      IUnitOfWork uow,
      IUploadService uploadService,
      IHostingEnvironment hostingEnvironment
    )
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));
      _uploadService = uploadService;
      _Sliders = _uow.Set<Slider>();
      _hostingEnvironment = hostingEnvironment;
    }




    public int AddNewSlider(Slider slider, IFormFile photo)
    {

      if (photo != null)
      {
        var fileName = "";
        long fileSize = 0;
        var size = new[] { 300 };
        var resized = new string[1];



        if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/slider"))
          Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/slider");


        fileName = _uploadService.UploadPicResize(
          photo, "/content/files/slider",
          1200,
          ref fileSize,
          size,
          EnumC.Dimensions.Width,
          ref resized
        );
        slider.Pic = fileName;
        // category.Thumbnail = size[0] + "/" + resized[0];
        slider.Thumbnail = size[0] + "/" + resized[0];
      }
      var id = _Sliders.Add(slider);
      _uow.SaveChanges();
      return id.Entity.Id;
    }

    public IList<Slider> GetAll(int typeId, string state, int pageNumber, int pageSize)
    {
      var offset = (pageSize * pageNumber) - pageSize;

      var query = typeId == 0 ?
        _Sliders :
        _Sliders.Where(l => l.TypeId == typeId);

      query = string.IsNullOrEmpty(state) ? query :
        query.Where(l => l.State.Contains(state));

      query = query.OrderByDescending(x => x.Id);
      return query.Skip(offset).Take(pageSize).ToList();
    }

    public int GetCount(int typeId, string title)
    {
      throw new NotImplementedException();
    }

    public async Task<IList<Slider>> GetAllAsync(int typeId, string state, int pageNumber, int pageSize)
    {
      var offset = (pageSize * pageNumber) - pageSize;
      var query = typeId == 0 ?
        _Sliders :
        _Sliders.Where(l => l.TypeId == typeId);

      query = string.IsNullOrEmpty(state) ? query :
        query.Where(l => l.State == state);
      query = query.OrderByDescending(x => x.Id);

      var result = await query.Skip(offset).Take(pageSize).ToListAsync();
      return result;
    }
    public async Task<PagedSliderViewModel> GetPagedSliderAsync(int pageNumber, int pageSize, SortOrder sortOrder, int type, string state)
    {
      var offset = (pageSize * pageNumber) - pageSize;
      var query = type == 0 ?
        _Sliders :
        _Sliders.Where(l => l.TypeId == type);

      query = string.IsNullOrEmpty(state) ? query :
        query.Where(l => l.State == state);
      query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
      return new PagedSliderViewModel
      {
        Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
        SliderItem = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false),
        State = state
      };
    }

    public Task<Slider> FindByIdAsync(int id)
    {
      return _Sliders.SingleOrDefaultAsync(p => p.Id == id);
    }

    public void UpdateSlider(Slider slider, IFormFile photo)
    {

      if (photo != null && photo.Length > 0)
      {
        if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/slider"))
          Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/slider");
        string fileName = "";
        long fileSize = 0;
        var size = new[] { 300 };
        var resized = new string[1];
        fileName = _uploadService.UploadPicResize(
                                                  photo, "/content/files/slider",
                                                  1200,
                                                  ref fileSize,
                                                  size,
                                                  EnumC.Dimensions.Width,
                                                  ref resized
                                                 );

        slider.Pic = fileName;
        slider.Thumbnail = size[0] + "/" + resized[0];
      }
      _Sliders.Update(slider);
      _uow.SaveChanges();
    }


    public void Delete(int id)
    {
      var itemToRemove = _Sliders.Find(id);
      if (itemToRemove != null)
      {
        _Sliders.Remove(itemToRemove);
        _uow.SaveChanges();
      }
    }
  }
}
