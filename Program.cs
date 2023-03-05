using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace PasswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            int passwordLength = GetPasswordLength();
            string passwordCharacters = GetPasswordCharacters();
            string password = GeneratePassword(passwordLength, passwordCharacters);
            Console.WriteLine("Your password is: " + password);
            Console.WriteLine("Password strength: " + GetPasswordStrength(password));
            Console.WriteLine("Do you want to save this password? (Y/N)");
            string response = Console.ReadLine();
            if (response.ToUpper() == "Y")
            {
                SavePassword(password);
                Console.WriteLine("Password saved.");
            }
            else
            {
                Console.WriteLine("Password not saved.");
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static int GetPasswordLength()
        {
            Console.WriteLine("Enter password length:");
            int passwordLength = int.Parse(Console.ReadLine());
            return passwordLength;
        }

        static string GetPasswordCharacters()
        {
            Console.WriteLine("Choose password character sets (comma separated):");
            Console.WriteLine("1. Lowercase letters");
            Console.WriteLine("2. Uppercase letters");
            Console.WriteLine("3. Numbers");
            Console.WriteLine("4. Symbols");
            string passwordCharacters = Console.ReadLine();
            return passwordCharacters;
        }

        static string GeneratePassword(int passwordLength, string passwordCharacters)
        {
            StringBuilder password = new StringBuilder();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] uintBuffer = new byte[sizeof(uint)];

            string[] characterSets = passwordCharacters.Split(',');
            foreach (string characterSet in characterSets)
            {
                int set;
                if (!int.TryParse(characterSet, out set))
                {
                    throw new ArgumentException("Invalid character set.");
                }

                string characters = GetCharactersForSet(set);
                if (string.IsNullOrEmpty(characters))
                {
                    throw new ArgumentException("Invalid character set.");
                }

                for (int i = 0; i < passwordLength; i++)
                {
                    rng.GetBytes(uintBuffer);
                    uint random = BitConverter.ToUInt32(uintBuffer, 0);
                    password.Append(characters[(int)(random % characters.Length)]);
                }
            }

            return password.ToString();
        }

        static string GetCharactersForSet(int set)
        {
            switch (set)
            {
                case 1:
                    return "abcdefghijklmnopqrstuvwxyz";
                case 2:
                    return "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                case 3:
                    return "0123456789";
                case 4:
                    return "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
                default:
                    return null;
            }
        }

        static string GetPasswordStrength(string password)
        {
            // Implement your password strength calculation logic here
            // and return a string indicating the strength of the password.
            // You can use any criteria you like, such as length, complexity, entropy, etc.
            return "Strong";
        }

        static void SavePassword(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes("password.bin", encryptedData);
        }
    }
}
