using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MusicProject.Entities.AuditableEntity;
using MusicProject.Entities.Content;
using MusicProject.Entities.Identity;

namespace MusicProject.Entities.Comment
{

    public class Comment : IAuditableEntity
    {
        [DisplayName("آیدی")]
        public int Id { get; set; }
        [DisplayName("بخش")]
        public int TypeId { get; set; }
        [MaxLength(500, ErrorMessage = "نظر نامعتبر است")]
        [DisplayName("متن نظر")]
        [Required]
        public string Body { get; set; }
        [MaxLength(20, ErrorMessage = "نام نامعتبر است")]
        [DisplayName("نام فرستنده")]
        public string Name { get; set; }
        [MaxLength(50, ErrorMessage = "ایمیل نامعتبر است")]
        [DisplayName("ایمیل فرستنده")]
        public string Email { get; set; }
        [DisplayName("لایک")]
        public int Like { get; set; }
        [DisplayName("دیسلایک")]
        public int Dislike { get; set; }
        public int? ParentId { get; set; }
        [DisplayName("نظر والد")]
        public Comment Parent { get; set; }
        public ICollection<Comment> InverseParent { get; set; }
        [MaxLength(11, ErrorMessage = "شماره همراه نامعتبر است")]
        [DisplayName("تلفن")]
        public string Tell { get; set; }
        [ForeignKey("User")]
        public int UserCommentId { get; set; }
        public User User { get; set; }
        [DisplayName("انتشار")]
        public bool IsPublished { get; set; }
        [DisplayName("زمان")]
        public DateTimeOffset? CreatedDateTime { get; set; }
        [DisplayName("پیوست")]
        public Attachment Attachment { get; set; }
        public ICollection<CategoryComment> CategoryComments { get; set; }
        public ICollection<ContentComment> ContentComments { get; set; }
        public ICollection<TagComment> TagComments { get; set; }
        public ICollection<PodcastComment> PodcastComments { get; set; }
        public ICollection<ContentListComment> ContentListComments { get; set; }

    }
}
