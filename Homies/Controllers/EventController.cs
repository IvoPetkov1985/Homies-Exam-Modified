using Homies.Contracts;
using Homies.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static Homies.Data.Common.DataConstants;

namespace Homies.Controllers
{
    public class EventController : BaseController
    {
        private readonly IEventService service;

        public EventController(IEventService eventService)
        {
            service = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await service.GetAllEventsAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new EventFormModel()
            {
                Types = await service.GetAllTypesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventFormModel model)
        {
            DateTime start;
            DateTime end;

            if (!DateTime.TryParseExact(model.Start, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out start))
            {
                ModelState.AddModelError(nameof(model.Start), InvalidDateMsg);
            }

            if (!DateTime.TryParseExact(model.End, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out end))
            {
                ModelState.AddModelError(nameof(model.End), InvalidDateMsg);
            }

            if (start > end)
            {
                ModelState.AddModelError(nameof(model.End), EndBeforeStartError);
            }

            var types = await service.GetAllTypesAsync();

            if (!types.Any(t => t.Id == model.TypeId))
            {
                ModelState.AddModelError(nameof(model.TypeId), NotExistingType);
            }

            if (!ModelState.IsValid)
            {
                model.Types = types;
                return View(model);
            }

            string userId = GetUserId();

            await service.CreateNewEventEntityAsync(model, userId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            bool isExisting = await service.IsEventExistingAsync(id);

            if (!isExisting)
            {
                return BadRequest();
            }

            var model = await service.CreateDetailsEventModelAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            bool isExisting = await service.IsEventExistingAsync(id);

            if (isExisting == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.JoinEventAsync(userId, id);

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var model = await service.CreateJoinedModelAsync(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            bool isExisting = await service.IsEventExistingAsync(id);

            if (isExisting == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.LeaveEventAsync(userId, id);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool isExisting = await service.IsEventExistingAsync(id);

            if (isExisting == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            bool isAuthorised = await service.IsUserAuthorised(userId, id);

            if (isAuthorised == false)
            {
                return Unauthorized();
            }

            var model = await service.CreateEditEventModel(id);
            model.Types = await service.GetAllTypesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventFormModel model, int id)
        {
            bool isExisting = await service.IsEventExistingAsync(id);

            if (isExisting == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            bool isAuthorised = await service.IsUserAuthorised(userId, id);

            if (isAuthorised == false)
            {
                return Unauthorized();
            }

            DateTime start;
            DateTime end;

            if (!DateTime.TryParseExact(model.Start, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out start))
            {
                ModelState.AddModelError(nameof(model.Start), InvalidDateMsg);
            }

            if (!DateTime.TryParseExact(model.End, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out end))
            {
                ModelState.AddModelError(nameof(model.End), InvalidDateMsg);
            }

            if (start > end)
            {
                ModelState.AddModelError(nameof(model.End), EndBeforeStartError);
            }

            var types = await service.GetAllTypesAsync();

            if (!types.Any(t => t.Id == model.TypeId))
            {
                ModelState.AddModelError(nameof(model.TypeId), NotExistingType);
            }

            if (!ModelState.IsValid)
            {
                model.Types = types;
                return View(model);
            }

            await service.EditEventAsync(model, id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool isExisting = await service.IsEventExistingAsync(id);

            if (isExisting == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            bool isAuthorised = await service.IsUserAuthorised(userId, id);

            if (isAuthorised == false)
            {
                return Unauthorized();
            }

            await service.DeleteEventAsync(id);

            return RedirectToAction(nameof(All));
        }
    }
}
