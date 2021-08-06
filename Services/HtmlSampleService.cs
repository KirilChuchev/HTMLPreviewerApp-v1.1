namespace HTMLPreviewerApp.Services
{
    using HTMLPreviewerApp.Data;
    using HTMLPreviewerApp.Data.Models;
    using HTMLPreviewerApp.Models.HtmlSample;
    using HTMLPreviewerApp.Models.HTMLSample;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HtmlSampleService : IHtmlSampleService
    {
        private readonly ApplicationDbContext dbContext;

        public HtmlSampleService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> SaveHtmlSample(HtmlSampleHomeViewModel homeModel)
        {
            var id =
                homeModel.CurrentHtmlSample.Id == null
                ? await this.SaveNewHtmlSample(homeModel)
                : await this.EditHtmlSample(homeModel);

            return id;
        }

        public async Task<ICollection<HtmlSampleViewModel>> GetAllHtmlSampleViewModelsByUserId(string userId)
        {
            return await this.dbContext.HtmlSamples.Where(x => x.UserId == userId).Select(x => new HtmlSampleViewModel()
            {
                Id = x.Id,
                RawHtml = x.RawHtml,
                CreatedOn = x.CreatedOn,
                LastEditedOn = x.LastEditedOn,
                User = x.User,
                UserId = x.UserId,
                Url = $"https://localhost:44382/HtmlSample/Share?htmlSampleId={x.Id}"
            }).ToArrayAsync();

        }

        public async Task<HtmlSampleViewModel> GetHtmlSampleViewModelById(string htmlSampleId)
        {
            var htmlSample = await this.dbContext.HtmlSamples.FirstOrDefaultAsync(x => x.Id == htmlSampleId);
            if (htmlSample == null)
            {
                return null;
            }
            return new HtmlSampleViewModel()
            {
                Id = htmlSample.Id,
                RawHtml = htmlSample.RawHtml,
                CreatedOn = htmlSample.CreatedOn,
                LastEditedOn = htmlSample.LastEditedOn,
                User = htmlSample.User,
                UserId = htmlSample.UserId,
                Url = $"https://localhost:44382/HtmlSample/Share?htmlSampleId={htmlSample.Id}"
            };
        }

        private async Task<string> SaveNewHtmlSample(HtmlSampleHomeViewModel homeModel)
        {
            var entity = await this.dbContext
                                   .HtmlSamples
                                   .AddAsync(new HtmlSample(homeModel.CurrentHtmlSample.UserId, homeModel.TempRawHtml));

            await this.dbContext.SaveChangesAsync();

            return entity.Entity.Id;
        }

        private async Task<string> EditHtmlSample(HtmlSampleHomeViewModel homeModel)
        {
            var entity = await this.dbContext.HtmlSamples.FirstOrDefaultAsync(x => x.Id == homeModel.CurrentHtmlSample.Id);

            entity.RawHtml = homeModel.TempRawHtml;
            entity.LastEditedOn = DateTime.Now;

            await this.dbContext.SaveChangesAsync();

            return entity.Id;
        }
    }
}
