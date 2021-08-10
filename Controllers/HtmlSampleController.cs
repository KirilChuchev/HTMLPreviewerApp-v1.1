namespace HTMLPreviewerApp.Controllers
{
    using HTMLPreviewerApp.Data.Models;
    using HTMLPreviewerApp.Helper_Services;
    using HTMLPreviewerApp.Models.HtmlSample;
    using HTMLPreviewerApp.Models.HTMLSample;
    using HTMLPreviewerApp.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
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

        public async Task<IActionResult> Index(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var currentHtmlSample = await this.htmlSampleService.GetHtmlSampleById(id);

            var currentHtmlSampleViewModel = currentHtmlSample != null ? new HtmlSampleViewModel()
            {
                Id = currentHtmlSample.Id,
                RawHtml = currentHtmlSample.RawHtml,
                CreatedOn = currentHtmlSample.CreatedOn,
                LastEditedOn = currentHtmlSample.LastEditedOn,
                User = currentHtmlSample.User,
                UserId = currentHtmlSample.UserId,
                Url = $"https://localhost:44382/HtmlSample/Share?htmlSampleId={currentHtmlSample.Id}"
            } : null;

            var currentUserHtmlSamples = await this.htmlSampleService.GetAllHtmlSamplesByUserId(currentUser.Id);
            var currentUserHtmlSampleViewModels = currentUserHtmlSamples.Select(x => new HtmlSampleViewModel()
            {
                Id = x.Id,
                RawHtml = x.RawHtml,
                CreatedOn = x.CreatedOn,
                LastEditedOn = x.LastEditedOn,
                User = x.User,
                UserId = x.UserId,
                Url = URLGenerator.Generate(this.Request, nameof(this.Share), x.Id),
            }).ToArray();

            var homeModel = new HtmlSampleHomeViewModel()
            {
                CurrentHtmlSample = currentHtmlSampleViewModel,
                HtmlSamples = currentUserHtmlSampleViewModels,
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
                var currentUserHtmlSamples = await this.htmlSampleService.GetAllHtmlSamplesByUserId(currentUser.Id);
                var currentUserHtmlSampleViewModels = currentUserHtmlSamples.Select(x => new HtmlSampleViewModel()
                {
                    Id = x.Id,
                    RawHtml = x.RawHtml,
                    CreatedOn = x.CreatedOn,
                    LastEditedOn = x.LastEditedOn,
                    User = x.User,
                    UserId = x.UserId,
                    Url = $"https://localhost:44382/HtmlSample/Share?htmlSampleId={x.Id}"
                }).ToArray();

                homeModel.HtmlSamples = currentUserHtmlSampleViewModels;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Open(string id)
        {
            return this.RedirectToAction("Index", "HtmlSample", new { id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Share(string id)
        {
            var currentHtmlSample = await this.htmlSampleService.GetHtmlSampleById(id);

            var model = new HtmlSampleShareViewModel()
            {
                RawHtml = currentHtmlSample.RawHtml,
                User = await this.userManager.FindByIdAsync(currentHtmlSample.UserId),
                CreatedOn = currentHtmlSample.CreatedOn,
                LastEditedOn = currentHtmlSample.LastEditedOn,
            };
            return View(model);
        }
    }
}
