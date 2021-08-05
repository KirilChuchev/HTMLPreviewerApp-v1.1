namespace HTMLPreviewerApp.Models.HTMLSample
{
    using HTMLPreviewerApp.Helper_Services;
    using HTMLPreviewerApp.Models.HtmlSample;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class HtmlSampleHomeViewModel : IValidatableObject
    {
        public HtmlSampleHomeViewModel()
        {
            this.HtmlSamples = new HashSet<HtmlSampleViewModel>();
        }

        public HtmlSampleViewModel CurrentHtmlSample { get; set; }

        public string TempRawHtml { get; set; }

        public bool IsEqualWithOriginal { get; set; }

        public ICollection<HtmlSampleViewModel> HtmlSamples { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.TempRawHtml))
            {
                yield return new ValidationResult("Полето не трябва да е празно.");
            }

            var htmlSampleSize = DiscSizeEstimator.Estimate(this.TempRawHtml);

            if (htmlSampleSize > 5)
            {
                yield return new ValidationResult($"Размерът на HTML кода не трябва да е над 5MB. Вашият код е с размер {htmlSampleSize}MB.");
            }
        }
    }
}
