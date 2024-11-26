namespace Paletitas.Services
{
    public class PasswordHelper
    {
        // Método para encriptar la contraseña
        public static string EncryptPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EncryptPassword: {ex.Message}");
                throw;
            }
        }

        // Método para verificar la contraseña
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en VerifyPassword: {ex.Message}");
                return false;
            }
        }
    }
}
