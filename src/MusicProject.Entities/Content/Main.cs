using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;
using MusicProject.Entities.Identity;

namespace MusicProject.Entities.Content
{
  public class Main : IAuditableEntity
  {

    public int Id { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Link { get; set; }
    public string Music { get; set; }
    }
}
