namespace HTMLPreviewerApp.Controllers
{
    using HTMLPreviewerApp.Data.Models;
    using HTMLPreviewerApp.Models.HtmlSample;
    using HTMLPreviewerApp.Models.HTMLSample;
    using HTMLPreviewerApp.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize]
    public class HtmlSampleController : Controller
    {
        private readonly IHtmlSampleService htmlSampleService;
        private readonly UserManager<ApplicationUser> userManager;

        public HtmlSampleController(IHtmlSampleService htmlSampleService, UserManager<ApplicationUser> userManager)
        {
            this.htmlSampleService = htmlSampleService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string htmlSampleId, string tempRawHtml = null)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var model = new HtmlSampleHomeViewModel()
            {
                HtmlSamples = await this.htmlSampleService.GetAllHtmlSamplesAsViewModel(currentUser),
                CurrentHtmlSample = await this.htmlSampleService.GetHtmlSampleViewModelById(htmlSampleId),
            };

            if (tempRawHtml != null)
            {
                model.TempRawHtml = tempRawHtml;
            }
            else
            {
                model.TempRawHtml = model?.CurrentHtmlSample?.RawHtml;
            }



            return this.View(model);
        }

        public async Task<IActionResult> SwitchAction(HtmlSampleHomeViewModel model, string submitButton)
        {
            if (submitButton == "Run")
            {
                return this.RunHtmlSample(model);
            }
            else if (submitButton == "Save")
            {
                return await this.SaveHtmlSample(model);
            }
            else if (submitButton == "Check original")
            {
                return this.CheckOriginal(model);
            }
            else
            {
                var htmlSampleId = model?.CurrentHtmlSample?.Id;
                return this.RedirectToAction("Index", "HtmlSample", new { htmlSampleId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHtmlSample(HtmlSampleHomeViewModel model)
        {
            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);

                if (!this.ModelState.IsValid)
                {
                    model.HtmlSamples = await this.htmlSampleService.GetAllHtmlSamplesAsViewModel(currentUser);
                    return this.View("Index", model);
                }

                
                var htmlSample = new HtmlSample(currentUser.Id);

                if (model.CurrentHtmlSample.Id != null)
                {
                    this.TempData["submitButton"] = "edited";
                    htmlSample = await this.htmlSampleService.EditHtmlSample(model.CurrentHtmlSample.Id, model.TempRawHtml);
                }
                else
                {
                    this.TempData["submitButton"] = "saved";
                    htmlSample = await this.htmlSampleService.SaveHtmlSample(new HtmlSample(currentUser.Id)
                    {
                        RawHtml = model.TempRawHtml,
                    });
                }

                var htmlSampleId = htmlSample.Id;

                return this.RedirectToAction("Index", "HtmlSample", new { htmlSampleId });
            }
            catch
            {
                var route = this.Request.Path.Value;
                return this.View("~/Views/CustomErrors/Error.cshtml", route);
            }
        }

        public IActionResult RunHtmlSample(HtmlSampleHomeViewModel model)
        {
            this.TempData["Run"] = true;
            var htmlSampleId = model.CurrentHtmlSample.Id;
            var tempRawHtml = model.TempRawHtml;
            return this.RedirectToAction("Index", "HtmlSample", new { htmlSampleId, tempRawHtml });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Share(string htmlSampleId)
        {
            var htmlSample = await this.htmlSampleService.GetHtmlSampleViewModelById(htmlSampleId);
            var model = new HtmlSampleShareViewModel()
            {
                Rawhtml = htmlSample.RawHtml,
                User = await this.userManager.FindByIdAsync(htmlSample.UserId),
                CreatedOn = htmlSample.CreatedOn,
                LastEditedOn = htmlSample.LastEditedOn,
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public IActionResult CheckOriginal(HtmlSampleHomeViewModel model)
        {
            if (model.TempRawHtml != null)
            {
                TempData["checkOriginal"] = true;
            }
            var htmlSampleId = model.CurrentHtmlSample.Id;
            var tempRawHtml = model.TempRawHtml;
            return this.RedirectToAction("Index", "HtmlSample", new { htmlSampleId, tempRawHtml });
        }
    }
}
