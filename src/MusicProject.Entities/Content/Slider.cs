using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
  public class Slider : IAuditableEntity
  {
    public int Id { get; set; }
    public int TypeId { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Pic { get; set; }
    public string Thumbnail { get; set; }
    public string Link { get; set; }
    public string State { get; set; }
    //public Tag Tag { get; set; }
    //public int ContentId { get; set; }

  }
}
