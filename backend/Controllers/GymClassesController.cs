using GymApp.Mappings;
using GymApp.Models;
using GymApp.Services;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers;

public class GymClassesController : Controller
{
    private readonly IGymClassService _gymClassService;

    private const int PageSize = 6;

    public GymClassesController(IGymClassService gymClassService)
    {
        _gymClassService = gymClassService;
    }

    // GET: /GymClasses?page=1 — public
    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var totalClasses = await _gymClassService.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalClasses / (double)PageSize);

        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        var classes = await _gymClassService.GetPagedAsync(page, PageSize, cancellationToken);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(classes.ToViewModelList());
    }

    // GET: /GymClasses/Details/5 — public
    public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
    {
        if (id == null)
            return NotFound();

        var gymClass = await _gymClassService.GetByIdAsync(id.Value, cancellationToken);
        if (gymClass == null)
            return NotFound();

        return View(gymClass.ToViewModel());
    }

    // GET: /GymClasses/Create — Admin only (protected route)
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View(new CreateGymClassViewModel());

    // POST: /GymClasses/Create — Admin only
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGymClassViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var gymClass = new GymClass
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            DurationMinutes = viewModel.DurationMinutes,
            Category = viewModel.Category
        };

        await _gymClassService.AddAsync(gymClass, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // GET: /GymClasses/Edit/5 — Admin only
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
    {
        if (id == null)
            return NotFound();

        var gymClass = await _gymClassService.GetByIdAsync(id.Value, cancellationToken);
        if (gymClass == null)
            return NotFound();

        var viewModel = new EditGymClassViewModel
        {
            Id = gymClass.Id,
            Name = gymClass.Name,
            Description = gymClass.Description,
            DurationMinutes = gymClass.DurationMinutes,
            Category = gymClass.Category
        };

        return View(viewModel);
    }

    // POST: /GymClasses/Edit/5 — Admin only
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditGymClassViewModel viewModel, CancellationToken cancellationToken)
    {
        if (id != viewModel.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(viewModel);

        var gymClass = await _gymClassService.GetByIdAsync(id, cancellationToken);
        if (gymClass == null)
            return NotFound();

        gymClass.Name = viewModel.Name;
        gymClass.Description = viewModel.Description;
        gymClass.DurationMinutes = viewModel.DurationMinutes;
        gymClass.Category = viewModel.Category;

        await _gymClassService.UpdateAsync(gymClass, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // GET: /GymClasses/Delete/5 — Admin only
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
    {
        if (id == null)
            return NotFound();

        var gymClass = await _gymClassService.GetByIdAsync(id.Value, cancellationToken);
        if (gymClass == null)
            return NotFound();

        return View(gymClass.ToViewModel());
    }

    // POST: /GymClasses/Delete/5 — Admin only
    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _gymClassService.DeleteAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
