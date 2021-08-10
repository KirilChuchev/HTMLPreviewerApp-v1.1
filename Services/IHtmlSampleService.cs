namespace HTMLPreviewerApp.Services
{
    using HTMLPreviewerApp.Data.Models;
    using HTMLPreviewerApp.Models.HTMLSample;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHtmlSampleService
    {
        Task<string> SaveHtmlSample(HtmlSampleHomeViewModel homeModel);

        Task<ICollection<HtmlSample>> GetAllHtmlSamplesByUserId(string userId);

        Task<HtmlSample> GetHtmlSampleById(string htmlSampleId);

        Task<bool> CheckOriginal(HtmlSampleHomeViewModel homeModel);
    }
}
