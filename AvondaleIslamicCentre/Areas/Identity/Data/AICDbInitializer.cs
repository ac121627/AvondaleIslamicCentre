using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvondaleIslamicCentre.Models;

namespace AvondaleIslamicCentre.Areas.Identity.Data
{
    public static class AICDbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AICDbContext>();
            var userManager = services.GetRequiredService<UserManager<AICUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Apply pending migrations
            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration error: {ex.Message}");
            }

            // Create roles
            var roles = new[] { "Admin", "Member" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create two users
            var adminEmail = "admin@aic.com";
            var memberEmail = "member@aic.com";

            AICUser adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new AICUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Site",
                    LastName = "Admin",
                    Phone = "+64 21-100-1001"
                };
                var res = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (!res.Succeeded)
                {
                    Console.WriteLine("Failed to create admin user: " + string.Join(';', res.Errors.Select(e => e.Description)));
                }
                else
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
                adminUser = await userManager.FindByEmailAsync(adminEmail);
            }

            AICUser memberUser = await userManager.FindByEmailAsync(memberEmail);
            if (memberUser == null)
            {
                var newMember = new AICUser
                {
                    UserName = memberEmail,
                    Email = memberEmail,
                    EmailConfirmed = true,
                    FirstName = "Regular",
                    LastName = "Member",
                    Phone = "+64 21-100-1002"
                };
                var resm = await userManager.CreateAsync(newMember, "Member@123");
                if (!resm.Succeeded)
                {
                    Console.WriteLine("Failed to create member user: " + string.Join(';', resm.Errors.Select(e => e.Description)));
                }
                else
                {
                    await userManager.AddToRoleAsync(newMember, "Member");
                }
                memberUser = await userManager.FindByEmailAsync(memberEmail);
            }

            // Ensure adminUser exists for seeding relationships
            if (adminUser == null)
            {
                Console.WriteLine("Admin user not available; seeding of user-owned records will use member user or skip.");
            }

           
            try
            {
                if (!context.Hall.Any())
                {
                    var halls = new List<Hall>
                    {
                        new Hall { Name = "Auditorium", Capacity = 150 },
                        new Hall { Name = "Multipurpose Room", Capacity = 60 },
                        new Hall { Name = "Lecture Theatre", Capacity = 80 },
                        new Hall { Name = "Activity Studio", Capacity = 30 }
                    };
                    context.Hall.AddRange(halls);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Halls: " + ex.Message);
            }

            // Seed Teachers (6)
            try
            {
                if (!context.Teachers.Any())
                {
                    var teachers = new List<Teacher>
                    {
                        new Teacher { FirstName = "Aisha", LastName = "Khan", Email = "aisha.khan@aic.nz", PhoneNumber = "+64 21-111-0001" },
                        new Teacher { FirstName = "David", LastName = "Ng", Email = "david.ng@aic.nz", PhoneNumber = "+64 21-111-0002" },
                        new Teacher { FirstName = "Sana", LastName = "Ali", Email = "sana.ali@aic.nz", PhoneNumber = "+64 21-111-0003" },
                        new Teacher { FirstName = "Liam", LastName = "Oconnor", Email = "liam.oconnor@aic.nz", PhoneNumber = "+64 21-111-0004" },
                        new Teacher { FirstName = "Maya", LastName = "Patel", Email = "maya.patel@aic.nz", PhoneNumber = "+64 21-111-0005" },
                        new Teacher { FirstName = "Noah", LastName = "Brown", Email = "noah.brown@aic.nz", PhoneNumber = "+64 21-111-0006" }
                    };
                    context.Teachers.AddRange(teachers);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Teachers: " + ex.Message);
            }

            // Seed Classes (5)
            try
            {
                if (!context.Class.Any())
                {
                    var teacherIds = context.Teachers.Select(t => t.TeacherId).ToList();
                    if (teacherIds.Count >= 5)
                    {
                        var classes = new List<Class>
                        {
                            new Class { ClassName = "Beginners Quran", Description = "Introductory Quran reading and basics for young learners.", CurrentStudents = 6, TeacherId = teacherIds[0] },
                            new Class { ClassName = "Intermediate Tajweed", Description = "Improve recitation with tajweed rules and practice.", CurrentStudents = 8, TeacherId = teacherIds[1] },
                            new Class { ClassName = "Hifz Preparation", Description = "Memorisation techniques and guided memorisation sessions.", CurrentStudents = 5, TeacherId = teacherIds[2] },
                            new Class { ClassName = "Youth Studies", Description = "Islamic studies for youth with interactive lessons.", CurrentStudents = 10, TeacherId = teacherIds[3] },
                            new Class { ClassName = "Adults Class", Description = "Evening class for adult learners focusing on study and reflection.", CurrentStudents = 7, TeacherId = teacherIds[4] }
                        };
                        context.Class.AddRange(classes);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Classes: " + ex.Message);
            }

            // Seed Students (20)
            try
            {
                if (!context.Students.Any())
                {
                    var classIds = context.Class.Select(c => c.ClassId).ToList();
                    var teacherIds = context.Teachers.Select(t => t.TeacherId).ToList();
                    var guardianNames = new[] { "Ahmad", "Leila", "Fatima", "Omar", "Hassan", "Sara", "Ibrahim", "Zara", "Hana", "Yusuf", "Amina", "Bilal", "Rania", "Karim", "Mona", "Rashid", "Nadia", "Samir", "Yara", "Zain" };
                    var studentFirst = new[] { "Adam", "Bella", "Cyrus", "Dina", "Elias", "Farah", "Gabe", "Huda", "Ilan", "Jana", "Kian", "Lina", "MayaS", "Noor", "OmarS", "Pia", "Qasim", "Rima", "Sami", "Tala" };
                    var studentLast = new[] { "Jones", "Smith", "Lee", "Wong", "Clark", "Evans", "Adams", "Baker", "Carter", "Dawson", "Ellis", "Foster", "Graham", "Hayes", "Ibrahim", "Jensen", "Khan", "Lopez", "Martin", "Nguyen" };

                    var students = new List<Student>();
                    for (int i = 0; i < 20; i++)
                    {
                        // generate a phone matching +64 21-xxx-xxxx
                        var local = 200 + (i % 700); // 3-digit
                        var subs = 1000 + i; // 4-digit
                        var phone = $"+64 21-{local:D3}-{subs:D4}";

                        students.Add(new Student
                        {
                            GuardianFirstName = guardianNames[i],
                            GuardianLastName = "Family",
                            FirstName = studentFirst[i],
                            LastName = studentLast[i],
                            Email = $"student{i+1}@example.com",
                            PhoneNumber = phone,
                            Gender = (i % 2 == 0) ? "Male" : "Female",
                            Ethnicity = (i % 3 == 0) ? "Pacific" : "Asian",
                            QuranNazira = (i % 4 == 0) ? "Advanced" : "Basic",
                            QuranHifz = (i % 5 == 0) ? "Partial" : "None",
                            Address = $"{i + 1} Example Street",
                            DateOfBirth = DateTime.Today.AddYears(-8).AddDays(i),
                            ClassId = classIds.Count > 0 ? classIds[i % classIds.Count] : 1,
                            TeacherId = teacherIds.Count > 0 ? teacherIds[i % teacherIds.Count] : 1
                        });
                    }
                    context.Students.AddRange(students);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Students: " + ex.Message);
            }

            // Seed Notices (20)
            try
            {
                if (!context.Notices.Any())
                {
                    var userIdForNotices = adminUser?.Id ?? memberUser?.Id ?? string.Empty;
                    var notices = Enumerable.Range(1, 20).Select(i => new Notice
                    {
                        Title = $"Community Update {i}",
                        Message = $"This is community announcement number {i} with details to inform members.",
                        PostedAt = DateTime.Now.AddDays(-i),
                        UpdatedAt = null,
                        AICUserId = userIdForNotices
                    }).ToList();
                    context.Notices.AddRange(notices);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Notices: " + ex.Message);
            }

            // Seed Donations (20)
            try
            {
                if (!context.Donations.Any())
                {
                    var userIdForDonations = adminUser?.Id ?? memberUser?.Id ?? string.Empty;
                    var donations = Enumerable.Range(1, 20).Select(i => new Donation
                    {
                        Amount = 10 + i,
                        DateDonated = DateTime.Now.AddDays(-i),
                        DonorName = $"Supporter{i}",
                        DonationType = (i % 2 == 0) ? "General" : "Event",
                        PaymentMethod = (i % 3 == 0) ? "Cash" : "Card",
                        Description = $"Donation for cause number {i}",
                        AICUserId = userIdForDonations
                    }).ToList();
                    context.Donations.AddRange(donations);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Donations: " + ex.Message);
            }

            // Seed Bookings (20)
            try
            {
                if (!context.Booking.Any())
                {
                    var hallIds = context.Hall.Select(h => h.HallId).ToList();
                    var userIdForBookings = adminUser?.Id ?? memberUser?.Id ?? string.Empty;
                    var bookings = new List<Booking>();
                    for (int i = 1; i <= 20; i++)
                    {
                        bookings.Add(new Booking
                        {
                            StartDateTime = DateTime.Today.AddDays(i % 7).AddHours(9 + (i % 6)),
                            EndDateTime = DateTime.Today.AddDays(i % 7).AddHours(10 + (i % 6)),
                            HallId = hallIds.Count > 0 ? hallIds[(i - 1) % hallIds.Count] : 1,
                            AICUserId = userIdForBookings
                        });
                    }
                    context.Booking.AddRange(bookings);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Bookings: " + ex.Message);
            }
        }
    }
}