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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Open(string id)
        {
            return this.RedirectToAction("Index", "HtmlSample", new { id });
        }

        public async Task<IActionResult> Index(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var currentHtmlSample = await this.htmlSampleService.GetHtmlSampleViewModelById(id);

            var homeModel = new HtmlSampleHomeViewModel()
            {
                CurrentHtmlSample = currentHtmlSample,
                HtmlSamples = await this.htmlSampleService.GetAllHtmlSampleViewModelsByUserId(currentUser.Id),
                TempRawHtml = this.TempData["tempRawHtml"] as string,
            };

            homeModel.TempRawHtml = TempData["tempRawHtml"] as string ?? homeModel.CurrentHtmlSample?.RawHtml;
            homeModel.IsEqualWithOriginal = homeModel.CurrentHtmlSample?.RawHtml == homeModel.TempRawHtml;

            return this.View(homeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SwitchAction(HtmlSampleHomeViewModel homeModel, string submitButton)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                homeModel.HtmlSamples = await this.htmlSampleService.GetAllHtmlSampleViewModelsByUserId(currentUser.Id);
                return this.View("Index", homeModel);
            }
            
            string id;
            this.TempData["submitButton"] = submitButton;
            this.TempData["tempRawHtml"] = homeModel.TempRawHtml;

            switch (submitButton)
            {
                case "Run":
                    id = homeModel.CurrentHtmlSample?.Id;
                    break;
                case "Save":
                    TempData["save-action-type"] = homeModel.CurrentHtmlSample?.Id == null ? "saved" : "edited";
                    homeModel.CurrentHtmlSample.UserId = currentUser.Id;
                    id = await htmlSampleService.SaveHtmlSample(homeModel);
                    break;
                case "Check original":
                    id = homeModel.CurrentHtmlSample.Id;
                    break;
                default:
                    id = homeModel?.CurrentHtmlSample?.Id;
                    break;
            }

            return this.RedirectToAction("Index", "HtmlSample", new { id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Share(string htmlSampleId)
        {
            var htmlSample = await this.htmlSampleService.GetHtmlSampleViewModelById(htmlSampleId);
            var model = new HtmlSampleShareViewModel()
            {
                RawHtml = htmlSample.RawHtml,
                User = await this.userManager.FindByIdAsync(htmlSample.UserId),
                CreatedOn = htmlSample.CreatedOn,
                LastEditedOn = htmlSample.LastEditedOn,
            };
            return View(model);
        }
    }
}
