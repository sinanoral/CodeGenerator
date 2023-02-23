using System.Text;
using System.Security.Cryptography;


namespace CodeGenerator
{
    internal class Program
    {
        private const string AllowedCharacters = "ACDEFGHKLMNPRTXYZ234579";
        private const int NumKeys = 10000;
        private const int KeyLength = 5;

        private static readonly SHA256 Sha256 = SHA256.Create();

        static void Main()
        {
            string[] keys = GenerateKeys();
            foreach(var key in keys)
            {
                Console.WriteLine(key);
            }

            string keyToValidate = keys[0];
            bool isValid = ValidateKey(keyToValidate);
            Console.WriteLine($"Key '{keyToValidate}' is {(isValid ? "valid" : "invalid")}");

        }

        public static string GenerateKey(string counter)
        {
            byte[] counterBytes = Encoding.ASCII.GetBytes(counter);

            byte[] hash = Sha256.ComputeHash(counterBytes);

            var sb = new StringBuilder(KeyLength);
            for (int i = 0; i < KeyLength; i++)
            {
                int index = hash[i] % AllowedCharacters.Length;
                sb.Append(AllowedCharacters[index]);
            }

            return sb.Insert(0, counter).ToString();
        }

        public static string[] GenerateKeys()
        {
            string[] keys = new string[NumKeys];

            for (int num10 = 0; num10 < NumKeys; num10++)
            {
                string numCustom = "";
                int customBase = AllowedCharacters.Length;
                int quotient = num10;
                while (quotient > 0)
                {
                    int remainder = quotient % customBase;
                    numCustom = AllowedCharacters[remainder] + numCustom;
                    quotient /= customBase;
                }

                string key = GenerateKey(numCustom.PadLeft(3, 'A'));
                keys[num10] = key;
            }

            return keys;
        }

        private static bool ValidateKey(string key)
        {
            string expectedKey = GenerateKey(key[..3]);

            return expectedKey == key;
        }
    }
}
