namespace HTMLPreviewerApp.Models.HtmlSample
{
    using HTMLPreviewerApp.Data.Models;
    using System;

    public class HtmlSampleViewModel
    {
        public string Id { get; set; }

        public string RawHtml { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Url { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastEditedOn { get; set; }
    }
}
