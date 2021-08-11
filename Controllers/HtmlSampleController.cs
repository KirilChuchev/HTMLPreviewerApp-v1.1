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
            var currentUserHtmlSamples = await this.htmlSampleService.GetAllHtmlSamplesByUserId(currentUser.Id);

            var homeViewModel = new HtmlSampleHomeViewModel()
            {
                Id = currentHtmlSample?.Id,
                OriginalRawhtml = currentHtmlSample?.RawHtml,
                TempRawHtml = this.TempData["tempRawHtml"] as string ?? currentHtmlSample?.RawHtml,
                UserId = currentUser.Id,
                HtmlSamples = currentUserHtmlSamples.Select(x => new HtmlSampleViewModel()
                {
                    Id = x.Id,
                    RawHtml = x.RawHtml,
                    CreatedOn = x.CreatedOn,
                    LastEditedOn = x.LastEditedOn,
                    User = x.User,
                    UserId = x.UserId,
                    Url = URLGenerator.Generate(this.Request, nameof(this.Share), x.Id),
                }).ToArray(),
            };

            homeViewModel.IsEqualWithOriginal = this.TempData["checkOriginal"] as bool?;

            return this.View(homeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SwitchAction(HtmlSampleHomeViewModel homeViewModel, string submitButton)
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
                    Url = URLGenerator.Generate(this.Request, nameof(this.Share), x.Id),
                }).ToArray();

                homeViewModel.HtmlSamples = currentUserHtmlSampleViewModels;
                return this.View("Index", homeViewModel);
            }

            string id;
            this.TempData["submitButton"] = submitButton;
            this.TempData["tempRawHtml"] = homeViewModel.TempRawHtml;

            switch (submitButton)
            {
                case "Run":
                    id = homeViewModel.Id;
                    break;
                case "Save":
                    this.TempData["save-action-type"] = homeViewModel.Id == null ? "saved" : "edited";
                    homeViewModel.UserId = currentUser.Id;
                    id = await htmlSampleService.SaveHtmlSample(homeViewModel);
                    break;
                case "Check original":
                    this.TempData["checkOriginal"] = await this.htmlSampleService.CheckOriginal(homeViewModel);
                    id = homeViewModel.Id;
                    break;
                default:
                    id = homeViewModel?.Id;
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
