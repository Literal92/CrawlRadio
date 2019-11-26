using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
  public class SessionRequest : IAuditableEntity
  {
    public int Id { get; set; }
    

    public string Name { get; set; }
    public string Phone { get; set; }
    public string Mobile { get; set; }
    public string Job { get; set; }
    public string ShopName { get; set; }
    public string ShopTell { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Unit { get; set; }
    public bool SendSms { get; set; }
    public bool Newsletter { get; set; }
    public string SessionTime { get; set; }
    public string SessionDate { get; set; }
  }
}
