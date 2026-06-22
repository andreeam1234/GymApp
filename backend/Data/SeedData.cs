using GymApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (context.Database.IsRelational())
            context.Database.Migrate();
        else
            context.Database.EnsureCreated();

        // Roles
        string[] roleNames = ["Admin", "Trainer", "Member"];
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // Admin user
        var adminEmail = "admin@gymapp.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "GymApp",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Trainer user
        var trainerEmail = "trainer@gymapp.com";
        var trainer = await userManager.FindByEmailAsync(trainerEmail);
        if (trainer == null)
        {
            trainer = new ApplicationUser
            {
                UserName = "trainer",
                Email = trainerEmail,
                FirstName = "Alex",
                LastName = "Pop",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(trainer, "Trainer@123");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(trainer, "Trainer");
        }

        // Demo member user
        var memberEmail = "member@gymapp.com";
        var member = await userManager.FindByEmailAsync(memberEmail);
        if (member == null)
        {
            member = new ApplicationUser
            {
                UserName = "member",
                Email = memberEmail,
                FirstName = "Maria",
                LastName = "Ionescu",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(member, "Member@123");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(member, "Member");
        }

        // Gym classes
        if (!context.GymClasses.Any())
        {
            var yoga = new GymClass { Name = "Yoga", Description = "Clasa de yoga pentru flexibilitate si relaxare.", DurationMinutes = 60, Category = "Flexibility" };
            var crossfit = new GymClass { Name = "CrossFit", Description = "Antrenament functional de intensitate ridicata.", DurationMinutes = 45, Category = "Strength" };
            var spinning = new GymClass { Name = "Spinning", Description = "Cardio pe bicicleta in ritm de muzica.", DurationMinutes = 50, Category = "Cardio" };

            context.GymClasses.AddRange(yoga, crossfit, spinning);
            await context.SaveChangesAsync();

            context.ClassSessions.AddRange(
                new ClassSession { GymClassId = yoga.Id, TrainerId = trainer!.Id, StartTime = DateTime.UtcNow.AddDays(1).Date.AddHours(9), EndTime = DateTime.UtcNow.AddDays(1).Date.AddHours(10), Capacity = 15, Room = "Sala 1" },
                new ClassSession { GymClassId = crossfit.Id, TrainerId = trainer.Id, StartTime = DateTime.UtcNow.AddDays(1).Date.AddHours(18), EndTime = DateTime.UtcNow.AddDays(1).Date.AddHours(18).AddMinutes(45), Capacity = 20, Room = "Sala 2" },
                new ClassSession { GymClassId = spinning.Id, TrainerId = trainer.Id, StartTime = DateTime.UtcNow.AddDays(2).Date.AddHours(19), EndTime = DateTime.UtcNow.AddDays(2).Date.AddHours(19).AddMinutes(50), Capacity = 12, Room = "Sala 3" }
            );
            await context.SaveChangesAsync();
        }

        // Exercises
        if (!context.Exercises.Any())
        {
            context.Exercises.AddRange(
                new Exercise { Name = "Bench Press", MuscleGroup = "Chest", Equipment = "Barbell", Description = "Impins din culcat pentru piept." },
                new Exercise { Name = "Squat", MuscleGroup = "Legs", Equipment = "Barbell", Description = "Genuflexiuni cu bara." },
                new Exercise { Name = "Deadlift", MuscleGroup = "Back", Equipment = "Barbell", Description = "Ridicare greutate de la sol." },
                new Exercise { Name = "Pull-up", MuscleGroup = "Back", Equipment = "Bodyweight", Description = "Tractiuni la bara fixa." },
                new Exercise { Name = "Plank", MuscleGroup = "Core", Equipment = "Bodyweight", Description = "Mentinere pozitie pentru core." }
            );
            await context.SaveChangesAsync();
        }
    }
}
