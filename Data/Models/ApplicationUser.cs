namespace HTMLPreviewerApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.HtmlSampleColection = new HashSet<HtmlSample>();
        }

        public ICollection<HtmlSample> HtmlSampleColection { get; set; }
    }
}
