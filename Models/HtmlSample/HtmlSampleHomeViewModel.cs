namespace HTMLPreviewerApp.Models.HTMLSample
{
    using HTMLPreviewerApp.Models.HtmlSample;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class HtmlSampleHomeViewModel
    {
        public HtmlSampleHomeViewModel()
        {
            this.HtmlSamples = new HashSet<HtmlSampleViewModel>();
        }

        [Required(ErrorMessage = "This field should not be empty!")]
        public HtmlSampleViewModel CurrentHtmlSample { get; set; }

        public string TempRawHtml { get; set; }

        public ICollection<HtmlSampleViewModel> HtmlSamples { get; set; }
    }
}
