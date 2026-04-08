using Microsoft.AspNetCore.Identity;
using SIGEDAA.Models;

namespace SIGEDAA.Services;

public interface IPasswordService
{
    string HashPassword(Usuario usuario, string password);
    PasswordVerificationResult VerifyPassword(Usuario usuario, string storedPassword, string providedPassword);
    bool IsHashed(string storedPassword);
}

public sealed class PasswordService : IPasswordService
{
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public string HashPassword(Usuario usuario, string password)
    {
        return _passwordHasher.HashPassword(usuario, password);
    }

    public PasswordVerificationResult VerifyPassword(Usuario usuario, string storedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(storedPassword) || string.IsNullOrWhiteSpace(providedPassword))
        {
            return PasswordVerificationResult.Failed;
        }

        if (IsHashed(storedPassword))
        {
            return _passwordHasher.VerifyHashedPassword(usuario, storedPassword, providedPassword);
        }

        return storedPassword == providedPassword
            ? PasswordVerificationResult.SuccessRehashNeeded
            : PasswordVerificationResult.Failed;
    }

    public bool IsHashed(string storedPassword)
    {
        return !string.IsNullOrWhiteSpace(storedPassword) && storedPassword.StartsWith("AQAAAA", StringComparison.Ordinal);
    }
}
