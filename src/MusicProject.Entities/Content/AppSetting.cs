using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
  public class AppSetting : IAuditableEntity
  {
    public int Id { get; set; }
    public string AboutUs { get; set; }
    public string CafebazarLink { get; set; }
    public string ChangeLog { get; set; }
    public string Site { get; set; }
    public string Instagram { get; set; }
    public string Telegram { get; set; }
    public string Soundcloud { get; set; }
    public int TypeId { get; set; }
  }
}
