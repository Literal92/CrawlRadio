using System;
using System.Collections.Generic;

using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Identity;
using MusicProject.Services.Contracts.Identity;


namespace MusicProject.Services.Identity
{
  public interface ITokenStoreService
  {
    void AddUserToken(CustomUserToken userToken);
    CustomUserToken RefreshTokenToken(string token);
  }

  public class TokenStoreService : ITokenStoreService
  {

    private readonly IUnitOfWork _uow;
    private readonly DbSet<CustomUserToken> _tokens;

    private readonly IApplicationRoleManager _rolesService;

    public TokenStoreService(
        IUnitOfWork uow,

        IApplicationRoleManager rolesService
  )
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));


      _rolesService = rolesService;
      _rolesService.CheckArgumentIsNull(nameof(rolesService));

      _tokens = _uow.Set<CustomUserToken>();

    }

    public void AddUserToken(CustomUserToken userToken)
    {
      _tokens.Add(userToken);
      _uow.SaveChanges();
    }
    public CustomUserToken RefreshTokenToken(string token)
    {
      var tokenEntity = _tokens.FirstOrDefault(p => p.Token == token);

      return tokenEntity;
    }
  }
}