﻿using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Identity
{
    public class UserUsedPassword : IAuditableEntity
    {
        public int Id { get; set; }

        public string HashedPassword { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}