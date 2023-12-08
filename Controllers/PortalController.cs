using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using gcai.Models;
using gcai.Services;
using gcai.Data;

namespace gcai.Controllers
{
    [BindProperties(SupportsGet = true)]
    public class PortalController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment _env;
        public PortalController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //retrievs loged in user account details from .net database
            DataAccess dataService = new DataAccess();
            AppUser user = new AppUser();
            user.UserName = User.Identity.Name;
            DataAccess dataAccess = new DataAccess();
            AppUser foundUser = new AppUser();
            foundUser = dataAccess.getUserProfile(user);

            ViewData["screenname"] = foundUser.ScreenName;

            return View(foundUser);
        }
        /*
         * removes a favorite
         * 
         */
        [HttpGet]
        [Authorize]
        [Route("PortalController/RemoveFavorite/{PostRefIn}")]
        public async Task<IActionResult> RemoveFavorite([FromRoute] string? PostRefIn)
        {

            DataAccess dataService = new DataAccess();

            //retrievs logged in user account details from .net database
            AppUser user = new AppUser();
            user.UserName = User.Identity.Name;
            AppUser foundUser = new AppUser();
            foundUser = dataService.getUserProfile(user);

            dataService.RemoveFavorite(PostRefIn, foundUser.UserName);

            ViewData["screenname"] = foundUser.ScreenName;

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize]
        [Route("PortalController/Edit/{PostRefIn}")]
        public async Task<IActionResult> Edit([FromRoute] string? PostRefIn)
        {
            DataAccess dataService = new DataAccess();
            PostModel editPost = new PostModel();
            if (PostRefIn != null)
            {
                editPost = dataService.GetPost(PostRefIn);
            }
            else
            {
                Console.WriteLine("A post was requested but was missing the postRefIn");
            }
            return View(editPost);
        }
        /*
         * method edits the a post
         */
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPost([Bind("idPostModel, UserId, ScreenName, PostType, PostDate, Truth, Humor, Problem, Solution")] PostModel PostIn)
        {

            DataAccess dataService = new DataAccess();
            dataService.UpdatePost(PostIn);

            //retrievs loged in user account details from .net database
            AppUser user = new AppUser();
            user.UserName = User.Identity.Name;
            AppUser foundUser = new AppUser();
            foundUser = dataService.getUserProfile(user);

            ViewData["screenname"] = foundUser.ScreenName;

            return RedirectToAction(nameof(Index));
        }

    }
}
