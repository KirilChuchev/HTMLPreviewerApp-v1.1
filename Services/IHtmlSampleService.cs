using HTMLPreviewerApp.Data.Models;
using HTMLPreviewerApp.Models.HtmlSample;
using HTMLPreviewerApp.Models.HTMLSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLPreviewerApp.Services
{
    public interface IHtmlSampleService
    {
        Task<HtmlSample> SaveHtmlSample(HtmlSample rawHtml);

        Task<HtmlSample> EditHtmlSample(string htmlSampleId, string rawHtml);

        Task<ICollection<HtmlSampleViewModel>> GetAllHtmlSamplesAsViewModel(ApplicationUser currentUser);

        Task<HtmlSampleViewModel> GetHtmlSampleViewModelById(string htmlSampleId);
    }
}
