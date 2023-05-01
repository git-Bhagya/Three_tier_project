using Ci_Project.Entities.Models;
using Ci_Project.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ci_Project.Controllers
{
    public class StoryController : Controller
    {
        private readonly ILogger<StoryController> _logger;
        private readonly IStoryRepository _storyRepository;

        public StoryController(ILogger<StoryController> logger, IStoryRepository storyRepository)
        {
            _logger = logger;
            _storyRepository = storyRepository;
        }
        //Story Listing Page
        [Authorize]
        public IActionResult StoryListingPage(int pageIndex = 1, int pageSize = 3)
        {
            var story = _storyRepository.StoryUser(pageIndex, pageSize);
            return View(story);
        }

        [Authorize]
        public IActionResult shareYourStoryPage(string missionid)
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            var detailRelatedStory = _storyRepository.getDataForShareYourStory(missionid, uid);

            return View(detailRelatedStory);
        }
        public JsonResult getAllMissions()
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            var missions = _storyRepository.getMissions(long.Parse(uid));
            return Json(missions);
        }


        [HttpPost]
        public IActionResult shareYourStoryPage(int mid, string sTitle, string? sDateAndTime, string sDesc, int userId, string[] images, string videoUrl)
        {
            var dataToFillStroyTable = _storyRepository.getDataForStoryTable(mid, sTitle, sDateAndTime, sDesc, userId, images, videoUrl);

            return RedirectToAction("StoryListingPage", "Story");

        }


        //press on submit button
        public IActionResult submit(long storyid)
        {
            _storyRepository.submit(storyid);
            return Json(new { redirectUrl = Url.Action("StoryListingPage", "Story") });
        }


        // Edit Data method
        public IActionResult EditShareStory(int mid, string sTitle, string sDesc, int userId, string[] images, string videoUrl)
        {
            var mission_id = mid;

            _storyRepository.editStory(mission_id, sTitle, sDesc, userId, images, videoUrl);

            return Json(new { redirectUrl = Url.Action("StoryListingPage", "Story") });

        }

        //method for edit story
        public IActionResult editStory(string missionid)
        {
            return Json(new { redirectUrl = Url.Action("shareYourStoryPage", "Story", new { missionid = missionid }) });
        }


    }
}
