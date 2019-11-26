using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using Microsoft.AspNetCore.Http;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface ICategoryTagService
  {
    int AddNewCategoryTag(CategoryTag categoryTag);
 
  }
}
