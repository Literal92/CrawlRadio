using MusicProject.Entities.AuditableEntity;
using MusicProject.Entities.Identity;

namespace MusicProject.Entities.Content
{
  public class Device : IAuditableEntity
  {
    public int Id { get; set; }
    public string Token { get; set; }
    public string FirebaseToken { get; set; }
    public string DeviceId { get; set; }
    public string Phone { get; set; }
    public bool PhoneConfirm { get; set; }
    public virtual User User { get; set; }
    public int? UserId { get; set; }
    public string ActiveCode { get; set; }


  }
}