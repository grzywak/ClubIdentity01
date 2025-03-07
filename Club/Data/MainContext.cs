using Club.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Club.Data
{
    public class MainContext : DbContext
    {


        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }
      
        public DbSet<Member> Members { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionReservation> SessionReservations { get; set; }    
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<CoachAssignment> CoachAssignments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<CardioEquipment> CardioEquipments { get; set; }
        public DbSet<StrengthEquipment> StrengthEquipments { get; set; }
        public DbSet<SessionEquipment> SessionEquipments { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<ProgressCard> ProgressCards { get; set; }
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
   
            //domyslnie nazwy tabel utworzylyby sie w liczbie mnogiej wiec tu nazywam
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<Session>().ToTable("Session");
            modelBuilder.Entity<SessionReservation>().ToTable("SessionReservation");
            modelBuilder.Entity<Coach>().ToTable("Coach");
            modelBuilder.Entity<CoachAssignment>().ToTable("CoachAssignment");
            modelBuilder.Entity<Section>().ToTable("Section");
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<Equipment>().ToTable("Equipment");
            modelBuilder.Entity<Maintenance>().ToTable("Maintenance");
            modelBuilder.Entity<StrengthEquipment>().ToTable("StrengthEquipment");
            modelBuilder.Entity<CardioEquipment>().ToTable("CardioEquipment");
            modelBuilder.Entity<SessionEquipment>().ToTable("SessionEquipment");
            modelBuilder.Entity<ProgressCard>().ToTable("ProgressCard");
            modelBuilder.Entity<TermsAndConditions>().ToTable("TermsAndConditions");

            // Konfiguracja dla Member
            modelBuilder.Entity<Member>()
                .HasKey(m => m.ID);
            // Konfiguracja relacji 1:1 między Member a ProgressCard
            modelBuilder.Entity<Member>()
                .HasOne(m => m.ProgressCard)
                .WithOne(pc => pc.Member)
                .HasForeignKey<ProgressCard>(pc => pc.MemberID)
                .OnDelete(DeleteBehavior.Cascade); // Zapewnienie kaskadowego usuwania - kompozycja
                                                   //Usunięcie obiektu Member powoduje automatyczne usunięcie powiązanego obiektu ProgressCard 
                                                   // Konfiguracja relacji 1:1 między Member a ProgressCard

            // Konfiguracja dla Session
            modelBuilder.Entity<Session>()
                .HasKey(s => s.SessionID); // Ustawienie klucza podstawowego dla Session

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Section)
                .WithMany(sec => sec.Sessions)
                .HasForeignKey(s => s.SectionID); // Konfiguracja relacji wiele-do-jednego między Session a Section

            modelBuilder.Entity<Session>()
                .HasOne(s => s.TermsAndConditions)
                .WithOne(tc => tc.Session)
                .HasForeignKey<TermsAndConditions>(tc => tc.SessionID)
                .OnDelete(DeleteBehavior.Cascade); // Konfiguracja relacji jeden-do-jednego między Session a TermsAndConditions z kaskadowym usuwaniem

            modelBuilder.Entity<Session>()
                .HasMany(s => s.CoachAssignments)
                .WithOne(ca => ca.Session)
                .HasForeignKey(ca => ca.SessionID); // Konfiguracja relacji jeden-do-wielu między Session a CoachAssignments

            modelBuilder.Entity<Session>()
              .HasMany(s => s.Feedbacks)
              .WithOne(f => f.Session)
              .HasForeignKey(f => f.SessionId)
              .OnDelete(DeleteBehavior.Cascade); // Kaskadowe usuwanie opinii (Feedback) powiązanych z Session

            // Konfiguracja dla Feedback
            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.FeedbackId); // Ustawienie klucza podstawowego dla Feedback

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Session)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.SessionId)
                .IsRequired(); // Feedback musi być powiązany z Session

            // Konfiguracja dla SessionReservation
            modelBuilder.Entity<SessionReservation>()
                .HasKey(sr => sr.SessionReservationID); // Ustawienie klucza podstawowego dla SessionReservation

            modelBuilder.Entity<SessionReservation>()
                .HasOne(sr => sr.Session)
                .WithMany(s => s.SessionReservations)
                .HasForeignKey(sr => sr.SessionID); // Konfiguracja relacji wiele-do-jednego między SessionReservation a Session

            modelBuilder.Entity<SessionReservation>()
                .HasOne(sr => sr.Member)
                .WithMany(m => m.SessionReservations)
                .HasForeignKey(sr => sr.MemberID); // Konfiguracja relacji wiele-do-jednego między SessionReservation a Member

            // Konfiguracja dla Coach
            modelBuilder.Entity<Coach>()
                .HasKey(c => c.CoachID); // Ustawienie klucza podstawowego dla Coach

            modelBuilder.Entity<Coach>()
                .HasMany(c => c.CoachAssignments)
                .WithOne(ca => ca.Coach)
                .HasForeignKey(ca => ca.CoachID); // Konfiguracja relacji jeden-do-wielu między Coach a CoachAssignment

            // Konfiguracja klucza podstawowego dla CoachAssignment
            modelBuilder.Entity<CoachAssignment>()
                .HasKey(ca => ca.CoachAssignmentID);
            //Konfiguracja relacji wiele do wiele między Coach a Session
            // Konfiguracja kluczy obcych dla CoachAssignment
            modelBuilder.Entity<CoachAssignment>()
                .HasOne(ca => ca.Coach)
                .WithMany(c => c.CoachAssignments)
                .HasForeignKey(ca => ca.CoachID)
                .OnDelete(DeleteBehavior.Restrict); // nie usuwam Coach wraz z CoachAssignment

            modelBuilder.Entity<CoachAssignment>()
                .HasOne(ca => ca.Session)
                .WithMany(s => s.CoachAssignments)
                .HasForeignKey(ca => ca.SessionID)
                .OnDelete(DeleteBehavior.Restrict); // nie usuwam Session wraz z CoachAssignment

            // Konfiguracja klucza podstawowego dla SessionEquipment
            modelBuilder.Entity<SessionEquipment>()
                .HasKey(se => se.SessionEquipmentID);

            // Konfiguracja kluczy obcych dla SessionEquipment
            modelBuilder.Entity<SessionEquipment>()
                .HasOne(se => se.Session)
                .WithMany(s => s.SessionEquipments)
                .HasForeignKey(se => se.SessionID);

            modelBuilder.Entity<SessionEquipment>()
                .HasOne(se => se.Equipment)
                .WithMany(e => e.SessionEquipments)
                .HasForeignKey(se => se.EquipmentID);

            // Relacja 1 : wiele między Equipment a Maintenance
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Equipment)
                .WithMany(e => e.Maintenances)
                .HasForeignKey(m => m.EquipmentID);

            // Relacja 1:1 Session a TermsAndConditions
            modelBuilder.Entity<Session>()
            .HasOne(s => s.TermsAndConditions)
            .WithOne(tc => tc.Session)
            .HasForeignKey<TermsAndConditions>(tc => tc.SessionID)
            .OnDelete(DeleteBehavior.Cascade);


            // przykladowe dane

            modelBuilder.Entity<Member>().HasData(
             new Member { ID = 1, Name = "Jan", LastName = "Kowalski", RegistrationDate = DateTime.Parse("2022-09-01"), Email = "jan.kowalski@com.pl", Password = "aaa123" },
             new Member { ID = 2, Name = "Anna", LastName = "Nowak", RegistrationDate = DateTime.Parse("2022-09-01"), Email = "anna.nowak@com.pl", Password = "bbb123" },
             new Member { ID = 3, Name = "Piotr", LastName = "Wiśniewski", RegistrationDate = DateTime.Parse("2023-09-01"), Email = "piotr.wisniewski@com.pl", Password = "eee123" },
             new Member { ID = 4, Name = "Katarzyna", LastName = "Wójcik", RegistrationDate = DateTime.Parse("2022-12-01"), Email = "katarzyna.wojcik@com.pl", Password = "fff123" },
             new Member { ID = 5, Name = "Marek", LastName = "Kowalczyk", RegistrationDate = DateTime.Parse("2023-01-15"), Email = "marek.kowalczyk@com.pl", Password = "ggg123" }
            );


            modelBuilder.Entity<Session>().HasData(
           new Session { SessionID = 1, Name = "Morning Cardio", StartTime = DateTime.Parse("2024-05-26T10:00:00"), EndTime = DateTime.Parse("2024-05-26T11:00:00"), MaxParticipants = 20, AvailableSlots = 20, SectionID = 1, TermsAndConditionsID = 1 },
           new Session { SessionID = 2, Name = "Evening Strength", StartTime = DateTime.Parse("2024-05-27T15:00:00"), EndTime = DateTime.Parse("2024-05-27T16:00:00"), MaxParticipants = 15, AvailableSlots = 15, SectionID = 2, TermsAndConditionsID = 2 },
           new Session { SessionID = 3, Name = "Yoga", StartTime = DateTime.Parse("2024-05-27T15:00:00"), EndTime = DateTime.Parse("2024-05-27T16:00:00"), MaxParticipants = 15, AvailableSlots = 15, SectionID = 3, TermsAndConditionsID = 3 },
           new Session { SessionID = 4, Name = "Pilates", StartTime = DateTime.Parse("2024-05-28T10:00:00"), EndTime = DateTime.Parse("2024-05-28T11:00:00"), MaxParticipants = 10, AvailableSlots = 10, SectionID = 1 , TermsAndConditionsID = 4 },
           new Session { SessionID = 5, Name = "HIIT", StartTime = DateTime.Parse("2024-05-29T17:00:00"), EndTime = DateTime.Parse("2024-05-29T18:00:00"), MaxParticipants = 20, AvailableSlots = 20, SectionID = 2, TermsAndConditionsID = 5 }
            );

            modelBuilder.Entity<SessionReservation>().HasData(
           new SessionReservation { SessionReservationID = 1, SessionID = 1, MemberID = 3, ReservationDate = DateTime.Parse("2024-05-26T10:00:00"), Status = ReservationStatus.Confirmed},
           new SessionReservation { SessionReservationID = 2, SessionID = 3, MemberID = 2, ReservationDate = DateTime.Parse("2024-05-26T10:00:00"), Status = ReservationStatus.Confirmed },
           new SessionReservation { SessionReservationID = 3, SessionID = 2, MemberID = 1, ReservationDate = DateTime.Parse("2024-05-26T10:00:00"), Status = ReservationStatus.Confirmed },
           new SessionReservation { SessionReservationID = 4, SessionID = 4, MemberID = 4, ReservationDate = DateTime.Parse("2024-05-28T10:00:00"), Status = ReservationStatus.Confirmed },
           new SessionReservation { SessionReservationID = 5, SessionID = 5, MemberID = 5, ReservationDate = DateTime.Parse("2024-05-29T17:00:00"), Status = ReservationStatus.Confirmed }
           );

            modelBuilder.Entity<ProgressCard>().HasData(
              new ProgressCard { ProgressCardID = 1, MeasurementDate = DateTime.Now, Weight = 70, BodyFatPercentage = 15, MuscleMass = 30, Height = 175, MemberID = 1 },
              new ProgressCard { ProgressCardID = 2, MeasurementDate = DateTime.Now, Weight = 65, BodyFatPercentage = 20, MuscleMass = 25, Height = 165, MemberID = 2 },
              new ProgressCard { ProgressCardID = 3, MeasurementDate = DateTime.Now, Weight = 80, BodyFatPercentage = 18, MuscleMass = 32, Height = 180, MemberID = 3 },
              new ProgressCard { ProgressCardID = 4, MeasurementDate = DateTime.Now, Weight = 55, BodyFatPercentage = 22, MuscleMass = 20, Height = 160, MemberID = 4 },
              new ProgressCard { ProgressCardID = 5, MeasurementDate = DateTime.Now, Weight = 75, BodyFatPercentage = 17, MuscleMass = 28, Height = 170, MemberID = 5 }
            );

            modelBuilder.Entity<Coach>().HasData(
              new Coach { CoachID = 1, Name = "Tomasz", LastName = "Wojcik", HireDate = DateTime.Parse("2021-09-01") },
              new Coach { CoachID = 2, Name = "Barbara", LastName = "Nowak", HireDate = DateTime.Parse("2022-05-01") },
              new Coach { CoachID = 3, Name = "Bartosz", LastName = "Kowalski", HireDate = DateTime.Parse("2021-09-01") }
              );

            modelBuilder.Entity<CoachAssignment>().HasData(
            new CoachAssignment { CoachAssignmentID = 1, CoachID = 1, SessionID = 1, Compensation = 100, IsLeadCoach = true },
            new CoachAssignment { CoachAssignmentID = 2, CoachID = 2, SessionID = 2, Compensation = 120, IsLeadCoach = false },
            new CoachAssignment { CoachAssignmentID = 3, CoachID = 3, SessionID = 3, Compensation = 150, IsLeadCoach = true },
            new CoachAssignment { CoachAssignmentID = 4, CoachID = 1, SessionID = 4, Compensation = 100, IsLeadCoach = true },
            new CoachAssignment { CoachAssignmentID = 5, CoachID = 2, SessionID = 5, Compensation = 120, IsLeadCoach = false }
            );

            modelBuilder.Entity<Section>().HasData(
                new Section { SectionID = 1, Name = "Cardio", Desc = "Cardio workouts" },
                new Section { SectionID = 2, Name = "Strength", Desc = "Strength training" },
                new Section { SectionID = 3, Name = "Flexibility", Desc = "Flexibility and stretching" }
            );

            modelBuilder.Entity<Feedback>().HasData(
            new Feedback { FeedbackId = 1, SessionId = 1, Rating = 5, Comment = "Great workout! Very well conducted." },
            new Feedback { FeedbackId = 2, SessionId = 3, Rating = 4, Comment = "Very enjoyable yoga class. Highly recommend!" },
            new Feedback { FeedbackId = 3, SessionId = 2, Rating = 4, Comment = "Intense session, I got a good workout." },
            new Feedback { FeedbackId = 4, SessionId = 4, Rating = 5, Comment = "Pilates really helps me stay in shape." },
            new Feedback { FeedbackId = 5, SessionId = 5, Rating = 5, Comment = "Best HIIT class I've ever attended!" }
            );

            modelBuilder.Entity<CardioEquipment>().HasData(
                new CardioEquipment { EquipmentID = 1, Name = "Treadmill", Brand = "FitBrand", PurchaseDate = DateTime.Parse("2022-01-01"), MaxSpeed = 20, MaxIncline = 15 },
                new CardioEquipment { EquipmentID = 3, Name = "Elliptical", Brand = "Endurance", PurchaseDate = DateTime.Parse("2021-07-20"), MaxSpeed = 12, MaxIncline = 10 }
            );

            modelBuilder.Entity<StrengthEquipment>().HasData(
                new StrengthEquipment { EquipmentID = 2, Name = "Bench Press", Brand = "StrongBrand", PurchaseDate = DateTime.Parse("2021-06-15"), MaxWeight = 300, MuscleGroup = "Chest" },
                new StrengthEquipment { EquipmentID = 4, Name = "Squat Rack", Brand = "PowerBrand", PurchaseDate = DateTime.Parse("2021-12-01"), MaxWeight = 500, MuscleGroup = "Legs" }
            );

            modelBuilder.Entity<Maintenance>().HasData(
                new Maintenance { MaintenanceID = 1, EquipmentID = 1, MaintenanceDate = DateTime.Parse("2023-05-01"), Description = "Regular maintenance" },
                new Maintenance { MaintenanceID = 2, EquipmentID = 2, MaintenanceDate = DateTime.Parse("2023-06-15"), Description = "Weight calibration" },
                new Maintenance { MaintenanceID = 3, EquipmentID = 1, MaintenanceDate = DateTime.Parse("2023-11-01"), Description = "Belt replacement" },
                new Maintenance { MaintenanceID = 4, EquipmentID = 3, MaintenanceDate = DateTime.Parse("2023-09-20"), Description = "Resistance check" },
                new Maintenance { MaintenanceID = 5, EquipmentID = 4, MaintenanceDate = DateTime.Parse("2024-02-10"), Description = "Frame inspection" }
            );

            modelBuilder.Entity<SessionEquipment>().HasData(
            new SessionEquipment { SessionEquipmentID = 1, SessionID = 1, EquipmentID = 1, Quantity = 5 },
            new SessionEquipment { SessionEquipmentID = 2, SessionID = 2, EquipmentID = 2, Quantity = 10 },
            new SessionEquipment { SessionEquipmentID = 3, SessionID = 3, EquipmentID = 3, Quantity = 15 },
            new SessionEquipment { SessionEquipmentID = 4, SessionID = 4, EquipmentID = 4, Quantity = 10 },
            new SessionEquipment { SessionEquipmentID = 5, SessionID = 5, EquipmentID = 4, Quantity = 15 }
            );

            modelBuilder.Entity<TermsAndConditions>().HasData(
            new TermsAndConditions { TermsAndConditionsID = 1, SessionID = 1, Content = "Standard terms and conditions for Morning Cardio. Participants must adhere to all club rules and regulations." },
            new TermsAndConditions { TermsAndConditionsID = 2, SessionID = 2, Content = "Standard terms and conditions for Evening Strength. Participants must adhere to all club rules and regulations." },
            new TermsAndConditions { TermsAndConditionsID = 3, SessionID = 3, Content = "Standard terms and conditions for Yoga. Participants must adhere to all club rules and regulations." },
            new TermsAndConditions { TermsAndConditionsID = 4, SessionID = 4, Content = "Standard terms and conditions for Pilates. Participants must adhere to all club rules and regulations." },
            new TermsAndConditions { TermsAndConditionsID = 5, SessionID = 5, Content = "Standard terms and conditions for HIIT. Participants must adhere to all club rules and regulations." }
        );

        }
    }
}
