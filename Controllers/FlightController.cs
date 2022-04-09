using Flight_Manager.Data;
using Flight_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Flight_Manager.Controllers
{
    
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
            _context = context;
        }  

        // GET: FlightController
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "1", Value = "1" });
            items.Add(new SelectListItem { Text = "10", Value = "10", Selected = true });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            //all flights
            var flights = (from f in _context.Flights select f).ToList();
            //search
            ViewData["CurrentFilter"] = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                flights = flights.Where(f => f.StartLocation.Contains(searchString)
                                       || f.EndLocation.Contains(searchString)).ToList();
            }

            pageSize = pageSize ?? 10;

            var model = new FlightsViewModel()
            {
                PageSize = pageSize ?? 10,
                PageSizeList = items,
                Flights = flights.ToPagedList(page ?? 1, pageSize.Value)
            };

            return View(model);

        }

        // GET: FlightController/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(flight);
        }

        [Authorize(Roles = "Admin")]
        // GET: FlightController/Create
        public ActionResult Create()
        {
            return View("Create", new Flight());
        }

        [Authorize(Roles = "Admin")]
        // POST: FlightController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, StartLocation, EndLocation, DepartureTime, LandingTime, PlaneId, PlaneModel, PilotName, EconomyClassSeats, BusinessClassSeats")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                if(flight.Id == Guid.Empty)
                {
                    flight.Id = Guid.NewGuid(); //Create
                    _context.Flights.Add(flight);
                    ViewBag.Flights = new SelectList(_context.Flights, "FlightId", "EndLocation");
                }
                else //Update
                {
                    _context.Entry(flight).State = EntityState.Modified;
                    _context.Update(flight);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        // GET: FlightController/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }     
            return View("Create", flight);
        }

        [Authorize(Roles = "Admin")]
        // GET: FlightController/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }
            else 
            {
                _context.Flights.Remove(flight);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
