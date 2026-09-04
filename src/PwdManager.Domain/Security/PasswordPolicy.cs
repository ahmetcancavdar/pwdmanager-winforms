namespace PwdManager.Domain.Security;

/// <summary>
/// Parola kurallarının TEK kaynağı. Tüm giriş noktaları bunu kullanır:
/// kurulum sihirbazı, personel oluşturma, parola sıfırlama, parola değiştirme.
/// Kuralı değiştirmek için yalnızca burayı düzenle.
/// </summary>
public static class PasswordPolicy
{
    /// <summary>Kabul edilen en kısa parola uzunluğu.</summary>
    public const int MinLength = 10;

    /// <summary>Kullanıcıya önden gösterilecek kısa kural metni (placeholder/ipucu).</summary>
    public const string Hint = "En az 10 karakter.";

    /// <summary>Doğrulama başarısızsa gösterilecek mesaj.</summary>
    public const string RequirementMessage = "Parola en az 10 karakter olmalı.";

    /// <summary>Parola kurallara uyuyor mu? Uymuyorsa <paramref name="error"/> nedeni verir.</summary>
    public static bool IsValid(string? password, out string error)
    {
        if ((password ?? string.Empty).Length < MinLength)
        {
            error = RequirementMessage;
            return false;
        }

        error = string.Empty;
        return true;
    }

    /// <summary>
    /// Servis katmanı savunması: parola kurallara uymuyorsa açıklayıcı bir istisna fırlatır.
    /// (UI doğrulaması atlansa bile kullanıcı net bir mesaj görür.)
    /// </summary>
    public static void Ensure(string? password)
    {
        if (!IsValid(password, out string error))
            throw new InvalidOperationException(error);
    }
}
