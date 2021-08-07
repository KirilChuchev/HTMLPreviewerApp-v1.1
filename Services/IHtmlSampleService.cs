namespace HTMLPreviewerApp.Services
{
    using HTMLPreviewerApp.Models.HtmlSample;
    using HTMLPreviewerApp.Models.HTMLSample;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHtmlSampleService
    {
        Task<string> SaveHtmlSample(HtmlSampleHomeViewModel homeModel);

        Task<ICollection<HtmlSampleViewModel>> GetAllHtmlSampleViewModelsByUserId(string userId);

        Task<HtmlSampleViewModel> GetHtmlSampleViewModelById(string htmlSampleId);

        Task<bool> CheckOriginal(HtmlSampleHomeViewModel homeModel);
    }
}
