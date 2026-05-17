using HumanResourcesApi.Models;
using Microsoft.AspNetCore.Identity;

namespace HumanResourcesApi.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var roles = new[] { "Admin", "IK", "Personel" };

        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Rol oluşturulamadı: {role}. Hatalar: {errors}");
                }
            }
        }

        await CreateUserIfNotExistsAsync(
            userManager,
            fullName: "Admin Kullanıcı",
            email: "admin@example.com",
            password: "123456",
            role: "Admin"
        );

        await CreateUserIfNotExistsAsync(
            userManager,
            fullName: "İK Kullanıcı",
            email: "ik@example.com",
            password: "123456",
            role: "IK"
        );

        await CreateUserIfNotExistsAsync(
            userManager,
            fullName: "Personel Kullanıcı",
            email: "personel@example.com",
            password: "123456",
            role: "Personel"
        );
    }

    private static async Task CreateUserIfNotExistsAsync(
        UserManager<ApplicationUser> userManager,
        string fullName,
        string email,
        string password,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                UserName = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Kullanıcı oluşturulamadı: {email}. Hatalar: {errors}");
            }
        }

        var isInRole = await userManager.IsInRoleAsync(user, role);

        if (!isInRole)
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"{email} kullanıcısına {role} rolü atanamadı. Hatalar: {errors}");
            }
        }
    }
}