namespace HTMLPreviewerApp.Services
{
    using System;
    using System.Linq;
    using HTMLPreviewerApp.Data;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using HTMLPreviewerApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using HTMLPreviewerApp.Models.HTMLSample;

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
                homeModel.Id == null
                ? await this.SaveNewHtmlSample(homeModel)
                : await this.EditHtmlSample(homeModel);

            return id;
        }

        public async Task<ICollection<HtmlSample>> GetAllHtmlSamplesByUserId(string userId)
        {
            return await this.dbContext.HtmlSamples.Where(x => x.UserId == userId).ToArrayAsync();
        }

        public async Task<HtmlSample> GetHtmlSampleById(string htmlSampleId)
        {
            return await this.dbContext.HtmlSamples.FirstOrDefaultAsync(x => x.Id == htmlSampleId);
        }

        public async Task<bool> CheckOriginal(HtmlSampleHomeViewModel homeViewModel)
        {
            //var htmlSample = await this.dbContext.HtmlSamples.FirstOrDefaultAsync(x => x.Id == homeViewModel.CurrentHtmlSample.Id);

            //return htmlSample.RawHtml == homeViewModel.TempRawHtml;
            //return false;

            var entity = await this.dbContext
                                    .HtmlSamples
                                    .FirstOrDefaultAsync(x => x.Id != homeViewModel.Id && x.RawHtml == homeViewModel.TempRawHtml);

            return entity != null;
        }

        private async Task<string> SaveNewHtmlSample(HtmlSampleHomeViewModel homeModel)
        {
            var entity = await this.dbContext
                                   .HtmlSamples
                                   .AddAsync(new HtmlSample(homeModel.UserId, homeModel.TempRawHtml));

            await this.dbContext.SaveChangesAsync();

            return entity.Entity.Id;
        }

        private async Task<string> EditHtmlSample(HtmlSampleHomeViewModel homeModel)
        {
            var entity = await this.dbContext.HtmlSamples.FirstOrDefaultAsync(x => x.Id == homeModel.Id);

            entity.RawHtml = homeModel.TempRawHtml;
            entity.LastEditedOn = DateTime.Now;

            await this.dbContext.SaveChangesAsync();

            return entity.Id;
        }
    }
}
