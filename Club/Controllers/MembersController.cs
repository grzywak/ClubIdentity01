using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Club.Data;
using Club.Models;
using Club.Models.ViewModels;
using Club.Models.DTOs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Club.Controllers
{
    public class MembersController : Controller
    {
        private readonly MainContext _context;

        public MembersController(MainContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.SessionReservations)
                    .ThenInclude(sr => sr.Session)
                .Include(m => m.ProgressCard)  // Include ProgressCard
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            var sessions = _context.Sessions
                .Where(s => s.AvailableSlots > 0)
                .Select(s => new { s.SessionID, s.Name })
                .ToList();

            var viewModel = new MemberDetailsViewModel
            {
                Member = member,
                Sessions = new SelectList(sessions, "SessionID", "Name"),
                ProgressCard = member.ProgressCard  // Set ProgressCard
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
                var member = await _context.Members
                    .Include(m => m.SessionReservations)
                        .ThenInclude(sr => sr.Session)
                    .FirstOrDefaultAsync(m => m.ID == dto.MemberID);
                var sessions = _context.Sessions
                    .Where(s => s.AvailableSlots > 0)
                    .Select(s => new { s.SessionID, s.Name })
                    .ToList();

                var viewModel = new MemberDetailsViewModel
                {
                    Member = member,
                    Sessions = new SelectList(sessions, "SessionID", "Name")
                };

                return View("Details", viewModel);
            }

            // Sprawdzenie, czy członek jest już zapisany na wybraną sesję
            var existingReservation = await _context.SessionReservations
                .FirstOrDefaultAsync(sr => sr.MemberID == dto.MemberID && sr.SessionID == dto.SessionID);

            if (existingReservation != null)
            {
                ModelState.AddModelError(string.Empty, "You are already registered for this session.");

                // Ponowne załadowanie danych w przypadku błędów walidacji
                var member = await _context.Members
                    .Include(m => m.SessionReservations)
                        .ThenInclude(sr => sr.Session)
                    .FirstOrDefaultAsync(m => m.ID == dto.MemberID);
                var sessions = _context.Sessions
                    .Where(s => s.AvailableSlots > 0)
                    .Select(s => new { s.SessionID, s.Name })
                    .ToList();

                var viewModel = new MemberDetailsViewModel
                {
                    Member = member,
                    Sessions = new SelectList(sessions, "SessionID", "Name")
                };

                return View("Details", viewModel);
            }

            // Pobieranie sesji i jej rezerwacji
            var session = await _context.Sessions
                .Include(s => s.SessionReservations)
                .FirstOrDefaultAsync(s => s.SessionID == dto.SessionID);
            if (session == null || session.AvailableSlots <= 0)
            {
                ModelState.AddModelError(string.Empty, "Selected session is not available.");
                // Ponowne załadowanie danych w przypadku błędów walidacji
                var member = await _context.Members
                    .Include(m => m.SessionReservations)
                        .ThenInclude(sr => sr.Session)
                    .FirstOrDefaultAsync(m => m.ID == dto.MemberID);
                var sessions = _context.Sessions
                    .Where(s => s.AvailableSlots > 0)
                    .Select(s => new { s.SessionID, s.Name })
                    .ToList();

                var viewModel = new MemberDetailsViewModel
                {
                    Member = member,
                    Sessions = new SelectList(sessions, "SessionID", "Name")
                };

                return View("Details", viewModel);
            }

            // Sprawdzenie liczby istniejących rezerwacji dla sesji
            int reservationCount = session.SessionReservations.Count;

            var reservation = new SessionReservation
            {
                MemberID = dto.MemberID,
                SessionID = dto.SessionID,
                ReservationDate = dto.ReservationDate,
                // Ustawienie statusu rezerwacji na 'Confirmed', jeśli liczba istniejących rezerwacji >= 1
                Status = reservationCount >= 1 ? ReservationStatus.Confirmed : ReservationStatus.Created
            };

            try
            {
                _context.SessionReservations.Add(reservation);
                session.AvailableSlots--;
                await _context.SaveChangesAsync();

                // Jeśli liczba istniejących rezerwacji >= 1, zaktualizuj status wszystkich rezerwacji na 'Confirmed'
                if (reservationCount >= 1)
                {
                    var sessionReservations = _context.SessionReservations.Where(sr => sr.SessionID == dto.SessionID).ToList();
                    foreach (var sr in sessionReservations)
                    {
                        sr.Status = ReservationStatus.Confirmed;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Logowanie wyjątku (dla celów przykładowych, po prostu drukujemy go do konsoli)
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while saving the reservation.");
                // Ponowne załadowanie danych w przypadku błędów walidacji
                var member = await _context.Members
                    .Include(m => m.SessionReservations)
                        .ThenInclude(sr => sr.Session)
                    .FirstOrDefaultAsync(m => m.ID == dto.MemberID);
                var sessions = _context.Sessions
                    .Where(s => s.AvailableSlots > 0)
                    .Select(s => new { s.SessionID, s.Name })
                    .ToList();

                var viewModel = new MemberDetailsViewModel
                {
                    Member = member,
                    Sessions = new SelectList(sessions, "SessionID", "Name")
                };

                return View("Details", viewModel);
            }

            return RedirectToAction(nameof(Details), new { id = dto.MemberID });
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberCreateDTO memberDTO)
        {
            if (ModelState.IsValid)
            {
                var member = new Member
                {
                    LastName = memberDTO.LastName,
                    Name = memberDTO.Name,
                    RegistrationDate = memberDTO.RegistrationDate,
                    Email = memberDTO.Email,
                    Password = memberDTO.Password,
                    ProgressCard = new ProgressCard
                    {
                        MeasurementDate = memberDTO.ProgressCard.MeasurementDate,
                        Weight = memberDTO.ProgressCard.Weight,
                        BodyFatPercentage = memberDTO.ProgressCard.BodyFatPercentage,
                        MuscleMass = memberDTO.ProgressCard.MuscleMass,
                        Height = memberDTO.ProgressCard.Height
                    }
                };

                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memberDTO);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,Name,RegistrationDate,Email,Password")] Member member)
        {
            if (id != member.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.ID))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }      

    }
}
