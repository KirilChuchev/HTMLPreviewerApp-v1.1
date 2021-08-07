namespace HTMLPreviewerApp.Models.HtmlSample
{
    using HTMLPreviewerApp.Data.Models;
    using System;

    public class HtmlSampleShareViewModel
    {
        public string RawHtml { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastEditedOn { get; set; }
    }
}
