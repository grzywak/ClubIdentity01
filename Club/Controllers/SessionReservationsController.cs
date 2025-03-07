using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Club.Data;
using Club.Models;

namespace Club.Controllers
{
    public class SessionReservationsController : Controller
    {
        private readonly MainContext _context;

        public SessionReservationsController(MainContext context)
        {
            _context = context;
        }

        // GET: SessionReservations
        public async Task<IActionResult> Index()
        {
            var mainContext = _context.SessionReservations.Include(s => s.Member).Include(s => s.Session);
            return View(await mainContext.ToListAsync());
        }

        // GET: SessionReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionReservation = await _context.SessionReservations
                .Include(s => s.Member)
                .Include(s => s.Session)
                .FirstOrDefaultAsync(m => m.SessionReservationID == id);
            if (sessionReservation == null)
            {
                return NotFound();
            }

            return View(sessionReservation);
        }

        // GET: SessionReservations/Create
        public IActionResult Create()
        {
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "Email");
            ViewData["SessionID"] = new SelectList(_context.Sessions, "SessionID", "Name");
            return View();
        }

        // POST: SessionReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionReservationID,SessionID,MemberID,ReservationDate,Status")] SessionReservation sessionReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sessionReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "Email", sessionReservation.MemberID);
            ViewData["SessionID"] = new SelectList(_context.Sessions, "SessionID", "Name", sessionReservation.SessionID);
            return View(sessionReservation);
        }

        // GET: SessionReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionReservation = await _context.SessionReservations.FindAsync(id);
            if (sessionReservation == null)
            {
                return NotFound();
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "Email", sessionReservation.MemberID);
            ViewData["SessionID"] = new SelectList(_context.Sessions, "SessionID", "Name", sessionReservation.SessionID);
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(ReservationStatus)));
            return View(sessionReservation);
        }

        // POST: SessionReservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SessionReservationID,SessionID,MemberID,ReservationDate,Status")] SessionReservation sessionReservation)
        {
            if (id != sessionReservation.SessionReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sessionReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionReservationExists(sessionReservation.SessionReservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "Email", sessionReservation.MemberID);
            ViewData["SessionID"] = new SelectList(_context.Sessions, "SessionID", "Name", sessionReservation.SessionID);
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(ReservationStatus)));
            return View(sessionReservation);
        }

        // GET: SessionReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionReservation = await _context.SessionReservations
                .Include(s => s.Member)
                .Include(s => s.Session)
                .FirstOrDefaultAsync(m => m.SessionReservationID == id);
            if (sessionReservation == null)
            {
                return NotFound();
            }

            return View(sessionReservation);
        }

        // POST: SessionReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessionReservation = await _context.SessionReservations.FindAsync(id);
            if (sessionReservation != null)
            {
                _context.SessionReservations.Remove(sessionReservation);
                await _context.SaveChangesAsync();

                // Pobierz sesję, do której należała rezerwacja
                var session = await _context.Sessions
                    .Include(s => s.SessionReservations)
                    .FirstOrDefaultAsync(s => s.SessionID == sessionReservation.SessionID);

                if (session != null)
                {
                    // Zwiększ liczbę dostępnych miejsc
                    session.AvailableSlots++;

                    // Sprawdź liczbę pozostałych rezerwacji dla tej sesji
                    int reservationCount = session.SessionReservations.Count;

                    // Jeśli liczba rezerwacji jest mniejsza niż 2, ustaw status na Created
                    if (reservationCount < 2)
                    {
                        foreach (var reservation in session.SessionReservations)
                        {
                            reservation.Status = ReservationStatus.Created;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckAndCancelReservations()
        {
            // Pobranie wszystkich sesji wraz z rezerwacjami
            var sessions = await _context.Sessions
                .Include(s => s.SessionReservations)
                .ToListAsync();

            // Iteracja przez wszystkie sesje
            foreach (var session in sessions)
            {
                // Sprawdzenie, czy czas sesji minął i czy liczba rezerwacji jest mniejsza niż 2
                if (session.StartTime < DateTime.Now && session.SessionReservations.Count < 2)
                {
                    // Odwołanie wszystkich rezerwacji dla tej sesji
                    foreach (var reservation in session.SessionReservations)
                    {
                        reservation.Status = ReservationStatus.Cancelled;
                    }
                }
            }

            // Zapisanie zmian w bazie danych
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool SessionReservationExists(int id)
        {
            return _context.SessionReservations.Any(e => e.SessionReservationID == id);
        }

    }
}
