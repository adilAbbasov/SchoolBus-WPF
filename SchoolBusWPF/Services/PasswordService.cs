using System.Security.Cryptography;
using System.Text;

namespace SchoolBusWPF.Services
{
	public class PasswordService
	{
		public static string HashPassword(string password)
		{
			var passwordBytes = Encoding.UTF8.GetBytes(password);
			var hashedPasswordBytes = SHA256.HashData(passwordBytes);

			return BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();
		}

		public static string EncodePassword(string? password)
		{
			if (password is null)
				throw new Exception();

			byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

			return Convert.ToBase64String(passwordBytes);
		}

		public static string DecodePassword(string? encodedPassword)
		{
            if (encodedPassword is null)
                throw new Exception();

            byte[] passwordBytes = Convert.FromBase64String(encodedPassword);

			return Encoding.UTF8.GetString(passwordBytes);
		}
	}
}
