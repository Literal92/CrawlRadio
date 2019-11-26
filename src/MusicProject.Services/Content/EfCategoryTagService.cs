using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Content
{
  public class EfCategoryTagService : ICategoryTagService
  {
    IUnitOfWork _uow;
    readonly DbSet<CategoryTag> _CategoryTag;
   

    public EfCategoryTagService(
      IUnitOfWork uow,
      IUploadService uploadService
    )
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));

      _CategoryTag = _uow.Set<CategoryTag>();
    }

    


    public int AddNewCategoryTag(CategoryTag categoryTag)
    {
     var id =_CategoryTag.Add(categoryTag).Entity.Id;
      _uow.SaveChanges();

      return id;

    }
  }
}
