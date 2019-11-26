using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using Microsoft.AspNetCore.Http;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface ISliderService
  {
    int AddNewSlider(Slider slider, IFormFile photo);


    IList<Slider> GetAll(int typeId,string state, int pageNumber, int pageSize);
   int GetCount(int typeId,string title);
   Task<IList<Slider>> GetAllAsync(int typeId,string state, int pageNumber, int pageSize);

    Task<PagedSliderViewModel> GetPagedSliderAsync(int pageNumber, int pageSize, SortOrder sortOrder, int type,string state );
    Task<Slider> FindByIdAsync(int id);
    void UpdateSlider(Slider slider, IFormFile photo);
    void Delete(int id);
  }
}
