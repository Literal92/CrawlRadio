using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
  public class Order : IAuditableEntity
  {
    public int Id { get; set; }
    public string Subject { get; set; }
    public string OrderType { get; set; }
    public string ProjectDuration { get; set; }
    public string Level { get; set; }
    public string Budget { get; set; }
    public string Mobile { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Place { get; set; }
    public string Speecher { get; set; }
    public string Senario { get; set; }
    public string Time { get; set; }
    public string Count { get; set; }
    public string HasSpeecher { get; set; }
    public string HasSenario { get; set; }
    public string HasEdit { get; set; }
    public string FrameCount { get; set; }
    public string File { get; set; }


  }
}
