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

            // Get Identity user and role managers for user/role operations.
            var userManager = services.GetRequiredService<UserManager<AICUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Attempt to apply any pending EF Core migrations so the database schema is up to date.
            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                // Log the migration error to console. In production you may want structured logging.
                Console.WriteLine($"Migration error: {ex.Message}");
            }

            // Define roles that the app expects to exist.
            var roles = new[] { "Admin", "Member" };

            // Ensure each role exists; create it if missing.
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Predefined emails for two seed users: an admin and a regular member.
            var adminEmail = "admin@aic.com";
            var memberEmail = "member@aic.com";

            // Try to find the admin user by email. If null, we will create it.
            AICUser adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Build a new admin user object with basic profile fields.
                var newAdmin = new AICUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Site",
                    LastName = "Admin",
                    Phone = "+64 21 100 1001"
                };

                // Create the user with a plain text password (only appropriate for seeding in dev).
                var res = await userManager.CreateAsync(newAdmin, "Admin@123");

                // If creation failed, write errors to console; otherwise assign the Admin role.
                if (!res.Succeeded)
                {
                    Console.WriteLine("Failed to create admin user: " + string.Join(';', res.Errors.Select(e => e.Description)));
                }
                else
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }

                // Re-query the user to obtain the user object with Id and other fields populated.
                adminUser = await userManager.FindByEmailAsync(adminEmail);
            }

            // Try to find the member user by email. If null, create it similarly to admin.
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
                    Phone = "+64 21 100 1002"
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

                // Re-query to get the fully populated member user.
                memberUser = await userManager.FindByEmailAsync(memberEmail);
            }

            // Simple check and info message if admin is still not available.
            // This can affect seeding of records that reference an admin user id.
            if (adminUser == null)
            {
                Console.WriteLine("Admin user not available; seeding of user-owned records will use member user or skip.");
            }

            // Seed Halls if none exist.
            try
            {
                // Only seed if the Hall table is empty.
                if (!context.Hall.Any())
                {
                    var halls = new List<Hall>
                    {
                        new Hall { Name = "Men's Main Hall", Capacity = 200 },
                        new Hall { Name = "Men's Back Hall", Capacity = 180 },
                        new Hall { Name = "Ladies Main Hall", Capacity = 100 },
                        new Hall { Name = "Ladies Small Hall", Capacity = 50 }
                    };

                    // Add the hall list and persist changes.
                    context.Hall.AddRange(halls);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Report any errors encountered while seeding halls.
                Console.WriteLine("Error seeding Halls: " + ex.Message);
            }

            // Seed Teachers if none exist.
            try
            {
                if (!context.Teachers.Any())
                {
                    var teachers = new List<Teacher>
                    {
                        new Teacher { FirstName = "Aisha", LastName = "Khan", Email = "aisha.khan@aic.nz", PhoneNumber = "+64 21 111 0001" },
                        new Teacher { FirstName = "David", LastName = "Ng", Email = "david.ng@aic.nz", PhoneNumber = "+64 21 111 0002" },
                        new Teacher { FirstName = "Sana", LastName = "Ali", Email = "sana.ali@aic.nz", PhoneNumber = "+64 21 111 0003" },
                        new Teacher { FirstName = "Liam", LastName = "Oconnor", Email = "liam.oconnor@aic.nz", PhoneNumber = "+64 21 111 0004" },
                        new Teacher { FirstName = "Maya", LastName = "Patel", Email = "maya.patel@aic.nz", PhoneNumber = "+64 21 111 0005" },
                        new Teacher { FirstName = "Noah", LastName = "Brown", Email = "noah.brown@aic.nz", PhoneNumber = "+64 21 111 0006" }
                    };

                    context.Teachers.AddRange(teachers);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Report teacher seeding issues.
                Console.WriteLine("Error seeding Teachers: " + ex.Message);
            }

            // Seed Classes if table is empty.
            try
            {
                if (!context.Class.Any())
                {
                    // Gather teacher ids to assign each class to an existing teacher.
                    var teacherIds = context.Teachers.Select(t => t.TeacherId).ToList();

                    // Only proceed if we have at least 5 teachers as expected by the class seed.
                    if (teacherIds.Count >= 5)
                    {
                        var classes = new List<Class>
                        {
                            new Class { ClassName = "Hifz Class", Description = "Memorization of the Holy Quran", CurrentStudents = 6, TeacherId = teacherIds[0] },
                            new Class { ClassName = "Grade 7", Description = "Year / Grade 7.", CurrentStudents = 3, TeacherId = teacherIds[1] },
                            new Class { ClassName = "Grade 6", Description = "Year / Grade 6.", CurrentStudents = 10, TeacherId = teacherIds[2] },
                            new Class { ClassName = "Grade 5", Description = "Year / Grade 5.", CurrentStudents = 15, TeacherId = teacherIds[3] },
                            new Class { ClassName = "Grade 3", Description = "Year / Grade 3.", CurrentStudents = 20, TeacherId = teacherIds[4] }
                        };

                        context.Class.AddRange(classes);
                        await context.SaveChangesAsync();
                    }
                    // If there are fewer teachers, the code silently skips class seeding.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Classes: " + ex.Message);
            }

            // Seed Students if none exist.
            try
            {
                if (!context.Students.Any())
                {
                    // Collect class and teacher ids for relational assignment.
                    var classIds = context.Class.Select(c => c.ClassId).ToList();
                    var teacherIds = context.Teachers.Select(t => t.TeacherId).ToList();

                    // Example seed data arrays for guardians and student names.
                    var guardianNames = new[] { "Ahmad", "Leila", "Fatima", "Omar", "Hassan", "Sara", "Ibrahim", "Zara", "Hana", "Yusuf", "Amina", "Bilal", "Rania", "Karim", "Mona", "Rashid", "Nadia", "Samir", "Yara", "Zain" };
                    var studentFirst = new[] { "Adam", "Bella", "Cyrus", "Dina", "Elias", "Farah", "Gabe", "Huda", "Ilan", "Jana", "Kian", "Lina", "MayaS", "Noor", "OmarS", "Pia", "Qasim", "Rima", "Sami", "Tala" };
                    var studentLast = new[] { "Jones", "Smith", "Lee", "Wong", "Clark", "Evans", "Adams", "Baker", "Carter", "Dawson", "Ellis", "Foster", "Graham", "Hayes", "Ibrahim", "Jensen", "Khan", "Lopez", "Martin", "Nguyen" };

                    var students = new List<Student>();

                    // Loop to create 20 sample students.
                    for (int i = 0; i < 20; i++)
                    {
                        // Build a pseudo-random but deterministic local phone number using index.
                        var local = 200 + (i % 700); // keep local part in a sensible range
                        var subs = 1000 + i;         // subscriber part increments by index
                        var phone = $"+64 21 {local:D3} {subs:D4}"; // format phone number

                        // Add a student object with a variety of fields set.
                        students.Add(new Student
                        {
                            GuardianFirstName = guardianNames[i],
                            GuardianLastName = "Family",
                            FirstName = studentFirst[i],
                            LastName = studentLast[i],
                            Email = $"student{i + 1}@example.com",
                            PhoneNumber = phone,
                            // Alternate gender based on index parity.
                            Gender = (i % 2 == 0) ? Gender.Male : Gender.Female,
                            // Use modulo to cycle through enum values safely.
                            Ethnicity = (Ethnicity)(i % Enum.GetValues(typeof(Ethnicity)).Length),
                            QuranNazira = (QuranLevel)(i % Enum.GetValues(typeof(QuranLevel)).Length),
                            // Offset the Hifz level by 1 to vary values compared to Nazira.
                            QuranHifz = (QuranLevel)((i + 1) % Enum.GetValues(typeof(QuranLevel)).Length),
                            Address = $"{i + 1} Example Street",
                            // Use DateTime.Today and subtract years to approximate age; then add days to vary birthdays.
                            DateOfBirth = DateTime.Today.AddYears(-8).AddDays(i),
                            // Assign class and teacher by cycling through available ids or falling back to 1 if none exist.
                            ClassId = classIds.Count > 0 ? classIds[i % classIds.Count] : 1,
                            TeacherId = teacherIds.Count > 0 ? teacherIds[i % teacherIds.Count] : 1
                        });
                    }

                    // Persist the students to the database.
                    context.Students.AddRange(students);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding Students: " + ex.Message);
            }

            // Seed Notices if none exist.
            try
            {
                if (!context.Notices.Any())
                {
                    // Choose a user id to associate with posted notices.
                    // Prefer the admin user id, otherwise fall back to member user id, otherwise empty string.
                    var userIdForNotices = adminUser?.Id ?? memberUser?.Id ?? string.Empty;

                    // Create 20 notices with decreasing posted dates.
                    var notices = Enumerable.Range(1, 20).Select(i => new Notice
                    {
                        Title = $"Community Update {i}",
                        Message = $"This is community announcement number {i} with details to inform members.",
                        PostedAt = DateTime.Now.AddDays(-i), // Posted i days ago
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

            // Seed Donations if none exist.
            try
            {
                if (!context.Donations.Any())
                {
                    // Choose a user id to associate with donations (admin preferred).
                    var userIdForDonations = adminUser?.Id ?? memberUser?.Id ?? string.Empty;

                    // Create 20 donation records with different enums and timestamps.
                    var donations = Enumerable.Range(1, 20).Select(i => new Donation
                    {
                        Amount = 10 + i, // small incremental amounts
                        DateDonated = DateTime.Now.AddDays(-i),
                        // Cycle through DonationType and PaymentMethod enums using modulo.
                        DonationType = (DonationType)(i % Enum.GetValues(typeof(DonationType)).Length),
                        PaymentMethod = (PaymentMethod)(i % Enum.GetValues(typeof(PaymentMethod)).Length),
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

            // Seed Bookings if none exist.
            try
            {
                if (!context.Booking.Any())
                {
                    // Get existing hall ids to associate bookings with halls.
                    var hallIds = context.Hall.Select(h => h.HallId).ToList();

                    // Choose a user id to associate with bookings.
                    var userIdForBookings = adminUser?.Id ?? memberUser?.Id ?? string.Empty;

                    var bookings = new List<Booking>();

                    // Create 20 booking entries.
                    for (int i = 1; i <= 20; i++)
                    {
                        // Compute a start date/time for the booking.
                        // - AddDays uses i % 7 so bookings are spread across days of the week.
                        // - AddHours uses 9 + (i % 6) to produce start times between 9 and 14 hours.
                        var start = DateTime.Today.AddDays(i % 7).AddHours(9 + (i % 6));

                        // End time is defined as one hour after start.
                        var end = start.AddHours(1);

                        bookings.Add(new Booking
                        {
                            StartDateTime = start,
                            // Only storing the time portion as EndDateTime.TimeOfDay
                            // This means the model stores the end as a TimeSpan rather than a full DateTime.
                            EndDateTime = end.TimeOfDay,
                            HallId = hallIds.Count > 0 ? hallIds[(i - 1) % hallIds.Count] : 1,
                            AICUserId = userIdForBookings
                        });
                    }

                    // Persist the bookings to the database.
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
