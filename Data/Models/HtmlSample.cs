namespace HTMLPreviewerApp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HtmlSample
    {
        public HtmlSample(string userId)
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.Now;
            this.UserId = userId; 
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(5 * 1024)]
        public string RawHtml { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser.Id))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastEditedOn { get; set; }
    }
}
