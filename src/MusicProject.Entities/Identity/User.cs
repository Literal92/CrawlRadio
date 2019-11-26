using MusicProject.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using MusicProject.Entities.Content;

namespace MusicProject.Entities.Identity
{
  /// <summary>
  /// More info: http://www.dotnettips.info/post/2577
  /// and http://www.dotnettips.info/post/2578
  /// plus http://www.dotnettips.info/post/2559
  /// </summary>
  public class User : IdentityUser<int>, IAuditableEntity
  {
    public User()
    {
      UserUsedPasswords = new HashSet<UserUsedPassword>();
      UserTokens = new HashSet<UserToken>();
    }

    [StringLength(450)]
    public string FirstName { get; set; }

    [StringLength(450)]
    public string LastName { get; set; }

    [NotMapped]
    public string DisplayName
    {
      get
      {
        var displayName = $"{FirstName} {LastName}";
        return string.IsNullOrWhiteSpace(displayName) ? UserName : displayName;
      }
    }

    [StringLength(450)]
    public string PhotoFileName { get; set; }

    [StringLength(5)]
    public string ActivationCode { get; set; }

    public DateTimeOffset? BirthDate { get; set; }

    public DateTimeOffset? CreatedDateTime { get; set; }

    public DateTimeOffset? LastVisitDateTime { get; set; }

    public bool IsEmailPublic { get; set; }

    
    public bool? Gender { get; set; }

    public string Location { set; get; }

    public bool IsActive { get; set; } = true;

    public virtual ICollection<UserUsedPassword> UserUsedPasswords { get; set; }

    public virtual ICollection<UserToken> UserTokens { get; set; }

    public virtual ICollection<UserRole> Roles { get; set; }

    public virtual ICollection<UserLogin> Logins { get; set; }

    public virtual ICollection<UserClaim> Claims { get; set; }

    public virtual ICollection<Device> Devices { get; set; }

    public virtual ICollection<CustomUserToken> CustomUserTokens { get; set; }
    public virtual ICollection<Comment.Comment> Comments { get; set; }

  }
}