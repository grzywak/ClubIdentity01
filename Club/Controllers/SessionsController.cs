using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Club.Data;
using Club.Models;
using Club.Models.DTOs;
using Club.Models.ViewModels;

namespace Club.Controllers
{
    public class SessionsController : Controller
    {
        private readonly MainContext _context;

        public SessionsController(MainContext context)
        {
            _context = context;
        }

        // GET: Sessions
        public async Task<IActionResult> Index()
        {
            var mainContext = _context.Sessions.Include(s => s.Section);
            return View(await mainContext.ToListAsync());
        }


        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.SessionReservations)
                    .ThenInclude(sr => sr.Member)
                .Include(s => s.TermsAndConditions)
                .FirstOrDefaultAsync(m => m.SessionID == id);

            if (session == null)
            {
                return NotFound();
            }

            var members = await _context.Members
                .Select(m => new { m.ID, FullName = m.Name + " " + m.LastName })
                .ToListAsync();

            var viewModel = new SessionDetailsViewModel
            {
                Session = session,
                Members = new SelectList(members, "ID", "FullName")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSessionReservation(SessionReservationDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Ponowne załadowanie danych w przypadku błędów walidacji
                var session = await _context.Sessions
                    .Include(s => s.SessionReservations)
                        .ThenInclude(sr => sr.Member)
                    .FirstOrDefaultAsync(m => m.SessionID == dto.SessionID);

                var members = await _context.Members
                    .Select(m => new { m.ID, FullName = m.Name + " " + m.LastName })
                    .ToListAsync();

                var viewModel = new SessionDetailsViewModel
                {
                    Session = session,
                    Members = new SelectList(members, "ID", "FullName")
                };

                return View("Details", viewModel);
            }

            // Sprawdzenie, czy członek jest już zapisany na wybraną sesję
            var existingReservation = await _context.SessionReservations
                .FirstOrDefaultAsync(sr => sr.MemberID == dto.MemberID && sr.SessionID == dto.SessionID);

            if (existingReservation != null)
            {
                ModelState.AddModelError(string.Empty, "Member is already registered for this session.");

                // Ponowne załadowanie danych w przypadku błędów walidacji
                var session = await _context.Sessions
                    .Include(s => s.SessionReservations)
                        .ThenInclude(sr => sr.Member)
                    .FirstOrDefaultAsync(m => m.SessionID == dto.SessionID);

                var members = await _context.Members
                    .Select(m => new { m.ID, FullName = m.Name + " " + m.LastName })
                    .ToListAsync();

                var viewModel = new SessionDetailsViewModel
                {
                    Session = session,
                    Members = new SelectList(members, "ID", "FullName")
                };

                return View("Details", viewModel);
            }

            // Pobieranie sesji do aktualizacji
            var sessionToUpdate = await _context.Sessions
                .Include(s => s.SessionReservations) // Pobierz istniejące rezerwacje dla sesji
                .FirstOrDefaultAsync(s => s.SessionID == dto.SessionID);

            if (sessionToUpdate == null || sessionToUpdate.AvailableSlots <= 0)
            {
                ModelState.AddModelError(string.Empty, "Selected session is not available.");

                // Ponowne załadowanie danych w przypadku błędów walidacji
                var session = await _context.Sessions
                    .Include(s => s.SessionReservations)
                        .ThenInclude(sr => sr.Member)
                    .FirstOrDefaultAsync(m => m.SessionID == dto.SessionID);

                var members = await _context.Members
                    .Select(m => new { m.ID, FullName = m.Name + " " + m.LastName })
                    .ToListAsync();

                var viewModel = new SessionDetailsViewModel
                {
                    Session = session,
                    Members = new SelectList(members, "ID", "FullName")
                };

                return View("Details", viewModel);
            }

            // Sprawdzenie liczby istniejących rezerwacji dla sesji
            int existingReservationCount = sessionToUpdate.SessionReservations.Count;
            var reservation = new SessionReservation
            {
                MemberID = dto.MemberID,
                SessionID = dto.SessionID,
                ReservationDate = DateTime.Now,
                // Ustawienie statusu rezerwacji na 'Confirmed', jeśli liczba istniejących rezerwacji >= 1
                Status = existingReservationCount >= 1 ? ReservationStatus.Confirmed : ReservationStatus.Created
            };

            try
            {
                // Dodanie nowej rezerwacji
                _context.SessionReservations.Add(reservation);
                sessionToUpdate.AvailableSlots--;

                // Jeśli po dodaniu nowej rezerwacji liczba rezerwacji wynosi co najmniej 2, zaktualizuj statusy na 'Confirmed'
                if (existingReservationCount + 1 >= 2)
                {
                    foreach (var res in sessionToUpdate.SessionReservations)
                    {
                        if (res.Status == ReservationStatus.Created)
                        {
                            res.Status = ReservationStatus.Confirmed;
                        }
                    }

                    reservation.Status = ReservationStatus.Confirmed; // Ustawienie statusu nowej rezerwacji na 'Confirmed'
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Logowanie wyjątku (dla celów przykładowych, po prostu drukujemy go do konsoli)
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while saving the reservation.");

                // Ponowne załadowanie danych w przypadku błędów walidacji
                var session = await _context.Sessions
                    .Include(s => s.SessionReservations)
                        .ThenInclude(sr => sr.Member)
                    .FirstOrDefaultAsync(m => m.SessionID == dto.SessionID);

                var members = await _context.Members
                    .Select(m => new { m.ID, FullName = m.Name + " " + m.LastName })
                    .ToListAsync();

                var viewModel = new SessionDetailsViewModel
                {
                    Session = session,
                    Members = new SelectList(members, "ID", "FullName")
                };

                return View("Details", viewModel);
            }

            // Przekierowanie do szczegółów sesji po dodaniu rezerwacji
            return RedirectToAction(nameof(Details), new { id = dto.SessionID });
        }



        // GET: Sessions/Create
        public IActionResult Create()
        {
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionID");
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionID,Name,StartTime,EndTime,MaxParticipants,AvailableSlots,SectionID")] Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionID", session.SectionID);
            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionID", session.SectionID);
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SessionID,Name,StartTime,EndTime,MaxParticipants,AvailableSlots,SectionID")] Session session)
        {
            if (id != session.SessionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.SessionID))
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
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionID", session.SectionID);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Section)
                .FirstOrDefaultAsync(m => m.SessionID == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.SessionID == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReservationStatus(int reservationId, ReservationStatus status)
        {
            var reservation = await _context.SessionReservations.FindAsync(reservationId);
            if (reservation == null)
            {
                return NotFound();
            }

            reservation.Status = status;
            try
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the status.");
            }

            return RedirectToAction(nameof(Details), new { id = reservation.SessionID });
        }

    }
}
