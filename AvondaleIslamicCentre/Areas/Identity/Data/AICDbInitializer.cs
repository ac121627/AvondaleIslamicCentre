using AvondaleIslamicCentre.Areas.Identity.Data;
using AvondaleIslamicCentre.Models;
using Microsoft.EntityFrameworkCore;

namespace AvondaleIslamicCentre.Data
{
    public static class AICDbInitializer
    {
        public static void Initialize(AICDbContext context)
        {
            context.Database.Migrate(); // ensures DB is created & migrated

            // Ensure database exists
            context.Database.EnsureCreated();

            // Only seed if there’s no data
            if (context.Donations.Any())
            {
                return; // DB has been seeded
            }

            // ----- Halls -----
            if (!context.Hall.Any())
            {
                var halls = Enumerable.Range(1, 20).Select(i => new Hall
                {
                    Name = $"Hall {i}",
                    Capacity = 50 + i,
                    AvailableFrom = DateTime.Now.AddDays(1).Date.AddHours(9),
                    AvailableTo = DateTime.Now.AddDays(1).Date.AddHours(17)
                }).ToList();

                context.Hall.AddRange(halls);
                //context.SaveChanges();
            }

            // ----- Teachers -----
            if (!context.Teachers.Any())
            {
                var teachers = Enumerable.Range(1, 20).Select(i => new Teacher
                {
                    FirstName = $"Teacher{i}",
                    LastName = $"Last{i}",
                    Email = $"teacher{i}@school.com",
                    PhoneNumber = $"+64 21-1234-56{i:D2}"
                }).ToList();

                context.Teachers.AddRange(teachers);
                //context.SaveChanges();
            }

            // ----- Classes -----
            if (!context.Class.Any())
            {
                var teacherIds = context.Teachers.Select(t => t.TeacherId).ToList();
                var classes = Enumerable.Range(1, 20).Select(i => new Class
                {
                    ClassName = $"Class {i}",
                    Description = $"This is class {i} description",
                    CurrentStudents = i * 2,
                    TeacherId = teacherIds[i % teacherIds.Count]
                }).ToList();

                context.Class.AddRange(classes);
                //context.SaveChanges();
            }

            // ----- Students -----
            if (!context.Students.Any())
            {
                var classIds = context.Class.Select(c => c.ClassId).ToList();
                var teacherIds = context.Teachers.Select(t => t.TeacherId).ToList();

                var students = Enumerable.Range(1, 20).Select(i => new Student
                {
                    GuardianFirstName = $"Guardian{i}",
                    GuardianLastName = $"Surname{i}",
                    FirstName = $"Student{i}",
                    LastName = $"Last{i}",
                    Email = $"student{i}@mail.com",
                    PhoneNumber = $"+64 22-9876-5{i:D2}",
                    Gender = i % 2 == 0 ? "Male" : "Female",
                    Ethnicity = "Asian",
                    QuranNazira = "Yes",
                    QuranHifz = "No",
                    Address = $"123 Street {i}",
                    DateOfBirth = DateTime.Now.AddYears(-10).AddDays(i),
                    ClassId = classIds[i % classIds.Count],
                    TeacherId = teacherIds[i % teacherIds.Count]
                }).ToList();

                context.Students.AddRange(students);
                //context.SaveChanges();
            }

            // ----- Donations -----
            if (!context.Donations.Any())
            {
                var donations = Enumerable.Range(1, 20).Select(i => new Donation
                {
                    Amount = 10 * i,
                    DateDonated = DateTime.Now.AddDays(-i),
                    DonorName = $"Donor {i}",
                    DonationType = "General",
                    PaymentMethod = "Card",
                    Description = $"Donation {i}",
                    AICUserId = "demo-user-id" // you’ll replace with actual seeded user later
                }).ToList();

                context.Donations.AddRange(donations);
                //context.SaveChanges();
            }

            // ----- Notices -----
            if (!context.Notices.Any())
            {
                var notices = Enumerable.Range(1, 20).Select(i => new Notice
                {
                    Title = $"Notice {i}",
                    Message = $"This is notice {i} message.",
                    PostedAt = DateTime.Now.AddDays(-i),
                    UpdatedAt = null,
                    AICUserId = "demo-user-id"
                }).ToList();

                context.Notices.AddRange(notices);
                //context.SaveChanges();
            }

            // ----- Reports -----
            if (!context.Report.Any())
            {
                var demoUser = context.Users.FirstOrDefault(u => u.Id == "demo-user-id");
                var reports = Enumerable.Range(1, 20).Select(i => new Report
                {
                    FirstName = $"Report{i}",
                    LastName = $"Lastname{i}",
                    Description = $"This is report {i} description",
                    CreatedAt = DateTime.Now.AddDays(-i),
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    AICUserId = "demo-user-id",
                    AICUser = demoUser
                }).ToList();

                context.Report.AddRange(reports);
                //context.SaveChanges();
            }

            // ----- Bookings -----
            if (!context.Booking.Any())
            {
                var hallIds = context.Hall.Select(h => h.HallId).ToList();

                var bookings = Enumerable.Range(1, 20).Select(i => new Booking
                {
                    StartDateTime = DateTime.Now.AddDays(i).Date.AddHours(10),
                    EndDateTime = DateTime.Now.AddDays(i).Date.AddHours(12),
                    HallId = hallIds[i % hallIds.Count],
                    AICUserId = "demo-user-id"
                }).ToList();

                context.Booking.AddRange(bookings);
                //context.SaveChanges();
            }

            //context.SaveChanges();
        }
    }
}
