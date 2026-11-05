
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrumbCount2.Models;

namespace CrumbCount2.Controllers
{
    public class WasteRecordingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WasteRecordingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WasteRecordings
        public async Task<IActionResult> Index()
        {
            var records = _context.WasteRecordings
                .Include(w => w.Product);

            return View(await records.ToListAsync());
        }

        // GET: WasteRecordings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var wasteRecording = await _context.WasteRecordings
                .Include(w => w.Product)
                .FirstOrDefaultAsync(w => w.WasteRecordingId == id);

            if (wasteRecording == null)
                return NotFound();

            return View(wasteRecording);
        }

        // GET: WasteRecordings/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(
                _context.Products,
                "ProductId",
                "ProductName"
            );

            return View();
        }

        // POST: WasteRecordings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("WasteRecordingId,ProductId,QuantityWasted,DateRecorded,ReasonForWaste,Remarks")]
            WasteRecording wasteRecording)
        {
            var product = await _context.Products
                .FindAsync(wasteRecording.ProductId);

            if (product == null)
            {
                ModelState.AddModelError(
                    "ProductId",
                    "Please select a valid product."
                );
            }

            if (ModelState.IsValid && product != null)
            {
                // Preserve the product cost at the time of waste.
                wasteRecording.UnitCostAtWaste = product.UnitCost;

                _context.WasteRecordings.Add(wasteRecording);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(
                _context.Products,
                "ProductId",
                "ProductName",
                wasteRecording.ProductId
            );

            return View(wasteRecording);
        }

        // GET: WasteRecordings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var wasteRecording = await _context.WasteRecordings
                .FindAsync(id);

            if (wasteRecording == null)
                return NotFound();

            ViewData["ProductId"] = new SelectList(
                _context.Products,
                "ProductId",
                "ProductName",
                wasteRecording.ProductId
            );

            return View(wasteRecording);
        }

        // POST: WasteRecordings/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("WasteRecordingId,ProductId,QuantityWasted,DateRecorded,ReasonForWaste,Remarks")]
            WasteRecording wasteRecording)
        {
            if (id != wasteRecording.WasteRecordingId)
                return NotFound();

            var existingRecord = await _context.WasteRecordings
                .FirstOrDefaultAsync(w => w.WasteRecordingId == id);

            if (existingRecord == null)
                return NotFound();

            var product = await _context.Products
                .FindAsync(wasteRecording.ProductId);

            if (product == null)
            {
                ModelState.AddModelError(
                    "ProductId",
                    "Please select a valid product."
                );
            }

            if (ModelState.IsValid && product != null)
            {
                // If the selected product changes, use its unit cost.
                // Otherwise, preserve the original historical cost.
                if (existingRecord.ProductId != wasteRecording.ProductId)
                {
                    existingRecord.UnitCostAtWaste = product.UnitCost;
                }

                existingRecord.ProductId = wasteRecording.ProductId;
                existingRecord.QuantityWasted = wasteRecording.QuantityWasted;
                existingRecord.DateRecorded = wasteRecording.DateRecorded;
                existingRecord.ReasonForWaste = wasteRecording.ReasonForWaste;
                existingRecord.Remarks = wasteRecording.Remarks;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(
                _context.Products,
                "ProductId",
                "ProductName",
                wasteRecording.ProductId
            );

            return View(wasteRecording);
        }

        // GET: WasteRecordings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var wasteRecording = await _context.WasteRecordings
                .Include(w => w.Product)
                .FirstOrDefaultAsync(w => w.WasteRecordingId == id);

            if (wasteRecording == null)
                return NotFound();

            return View(wasteRecording);
        }

        // POST: WasteRecordings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wasteRecording = await _context.WasteRecordings
                .FindAsync(id);

            if (wasteRecording != null)
            {
                _context.WasteRecordings.Remove(wasteRecording);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
