namespace HTMLPreviewerApp.Services
{
    using HTMLPreviewerApp.Data;
    using HTMLPreviewerApp.Data.Models;
    using HTMLPreviewerApp.Models.HtmlSample;
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
        public async Task<HtmlSample> SaveHtmlSample(HtmlSample htmlSample)
        {
            var entity = await this.dbContext.HtmlSamples.AddAsync(htmlSample);

            await this.dbContext.SaveChangesAsync();

            return entity.Entity;
        }

        public async Task<HtmlSample> EditHtmlSample(string htmlSampleId, string rawHtml)
        {
            var entity = await this.dbContext.HtmlSamples.FirstOrDefaultAsync(x => x.Id == htmlSampleId);

            entity.RawHtml = rawHtml;
            entity.LastEditedOn = DateTime.Now;

            await this.dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<ICollection<HtmlSampleViewModel>> GetAllHtmlSamplesAsViewModel(ApplicationUser currentUser)
        {
            return await this.dbContext.HtmlSamples.Where(x => x.UserId == currentUser.Id).Select(x => new HtmlSampleViewModel
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
            return new HtmlSampleViewModel
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
    }
}
