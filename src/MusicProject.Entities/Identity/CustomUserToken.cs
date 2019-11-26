using System;
using MusicProject.Entities.AuditableEntity;


namespace MusicProject.Entities.Identity
{
    public class CustomUserToken: IAuditableEntity
  {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTimeOffset TokenExpiresDateTime { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}