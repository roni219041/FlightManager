using Flight_Manager.Data;
using Flight_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
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
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public ReservationController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        [Authorize(Roles = "Admin, Employee")]
        // GET: ReservationController
        public async Task<IActionResult> Index(int? page, int? pageSize, string searchString)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "1", Value = "1" });
            items.Add(new SelectListItem { Text = "10", Value = "10", Selected = true });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });

            var reservations = await _context.Reservations.ToListAsync();
            var data = new List<ReservationViewModel>();
            foreach(var reservation in reservations)
            {
                var m = new ReservationViewModel();
                var flight = (from f in _context.Flights
                              where f.Id == reservation.FlightId
                              select f).FirstOrDefault<Flight>();
                m.ReservationId = reservation.Id;
                m.CreatedOn = reservation.CreatedOn;
                m.FlightDepartureTime = flight.DepartureTime;
                m.FlightEndLocation = flight.EndLocation;
                m.FlightStartLocation = flight.StartLocation;
                m.PassengersCount = (from p in _context.Passengers where p.ReservationId == reservation.Id select p).Count();
                m.IsConfirmed = reservation.IsConfirmed;
                m.Email = reservation.Email;
                data.Add(m);
            }
            ViewData["CurrentFilter"] = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(r => r.FlightStartLocation.Contains(searchString)).ToList();
            }
            pageSize = pageSize ?? 10;

            var model = new ReservationsViewModel()
            {
                PageSize = pageSize ?? 10,
                PageSizeList = items,
                Reservations = data.ToPagedList(page ?? 1, pageSize.Value)
            };

            return View(model);

            //return View(data);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: ReservationController/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            var model = new List<ReservationDetailsViewModel>();
            var m = new ReservationDetailsViewModel();
            var flight = (from f in _context.Flights
                          where f.Id == reservation.FlightId
                          select f).FirstOrDefault<Flight>();
            m.ReservationId = reservation.Id;
            m.FlightDepartureTime = flight.DepartureTime;
            m.FlightArrivalTime = flight.LandingTime;
            m.FlightStartLocation = flight.StartLocation;
            m.FlightEndLocation = flight.EndLocation;
            m.CreatedOn = reservation.CreatedOn;
            m.Email = reservation.Email;
            m.Passengers = (from p in _context.Passengers where p.ReservationId == reservation.Id select p).ToList();
            model.Add(m);
            ViewBag.ReservationId = m.ReservationId;

            return View(model);
        }

        // GET: ReservationController/Create
        public ActionResult Create()
        {
            List<SelectListItem> flights = new List<SelectListItem>();
            foreach (var flight in _context.Flights)
            {
                flights.Add(new SelectListItem() { Text = "From:" +flight.StartLocation + "- To:" + flight.EndLocation + "- Departure:" + flight.DepartureTime + "- Arrival:" + flight.LandingTime, Value = flight.Id.ToString() });
            }

            var reservation = new CreateReservationViewModel()
            {
                Flights = flights,
                Passengers = new List<PassengerViewModel>()
            };

            return View(reservation);
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid flightId, string email)
        {
            if (ModelState.IsValid)
            {
                var reservation = new Reservation();
                reservation.FlightId = flightId;
                reservation.Email = email;

                for (int i = 0; i <= Request.Form.Count; i++)
                {
                    var newPassenger = new Passenger()
                    {
                        FirstName = Request.Form["FirstName[" + i + "]"],
                        Surname = Request.Form["Surname[" + i + "]"],
                        LastName = Request.Form["LastName[" + i + "]"],
                        PersonalNumber = Request.Form["PersonalNumber[" + i + "]"],
                        PhoneNumber = Request.Form["PhoneNumber[" + i + "]"],
                        Nationality = Request.Form["Nationality[" + i + "]"],
                        IsBusinessClass = Request.Form["Class[" + i + "]"] == "business"
                    };

                    if (newPassenger.FirstName != null)
                    {
                        reservation.Passengers.Add(newPassenger);
                    }                    
                }

                var reservationEconomyPassengers = reservation.Passengers.Where(p => p.IsBusinessClass == false).Count();
                var reservationBusinessPassengers = reservation.Passengers.Where(p => p.IsBusinessClass == true).Count();

                // confirm seats are available
                var flight = _context.Flights.Where(f => f.Id == reservation.FlightId).FirstOrDefault();
                var flightPasengers = await _context.Passengers.Where(p => p.Reservation.FlightId == flight.Id).ToListAsync();
                var flightEconomyPassengers = flightPasengers.Where(p => p.IsBusinessClass == false).Count();
                var flightBusinessPassengers = flightPasengers.Where(p => p.IsBusinessClass == true).Count();

                var hasSeats = flight.BusinessClassSeats >= flightBusinessPassengers + reservationBusinessPassengers && 
                    flight.EconomyClassSeats >= flightEconomyPassengers + reservationEconomyPassengers;

                if (hasSeats)
                {
                    // Confirm reservation
                    reservation.IsConfirmed = true;

                    // Send mail
                    await _emailSender.SendEmailAsync(reservation.Email, "Confirm your reservation",
                        $"Your reservation has been confirmed. Enjoy your trip!");

                    ViewBag.ConfirmMessage = "Your reservation has been confirmed!";
                    ViewBag.Message = "An email with reservation details is sent to your email.";
                }
                else
                {
                    ViewBag.ConfirmMessage = "Your reservation has been declined!";
                    ViewBag.Message = "No enough free seats in the flight!";
                }

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return View("ConfirmReservation");
            }
            return View();
        }

        // GET: ReservationController/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            if (reservation.IsConfirmed)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
