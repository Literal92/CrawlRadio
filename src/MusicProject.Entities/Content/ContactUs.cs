using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
  public class ContactUs : IAuditableEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
  }
}
