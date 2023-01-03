using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AesEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            // Klucz i wektor inicjalizacyjny dla szyfru AES
            // Zmień je na swoje własne wartości || Pamiętaj o tym że musi mieć 16 znaków
            const string key = "bezpieczenstwo12";
            const string iv = "misiakbartlomiej";

            // Sprawdzenie długości klucza i wektora inicjalizacyjnego
            if (key.Length != 16 || iv.Length != 16)
            {
                Console.WriteLine("Klucz i wektor inicjalizacyjny muszą być długości 128 bitów (16 bajtów)");
                return;
            }
            // Podaj tekst do zaszyfrowania
            Console.WriteLine("Podaj tekst do zaszyfrowania");
            string message = Console.ReadLine();
            // Tekst do zaszyfrowania
            string plainText = message;

            // Szyfrowanie tekstu
            string encryptedText = Encrypt(plainText, key, iv);
            Console.WriteLine("Zaszyfrowany tekst: " + encryptedText);

            // Deszyfrowanie tekstu
            string decryptedText = Decrypt(encryptedText, key, iv);
            Console.WriteLine("Odszyfrowany tekst: " + decryptedText);
        }

        static string Encrypt(string plainText, string key, string iv)
        {
            // Utworzenie nowej instancji klasy Aes
            Aes aes = Aes.Create();

            // Ustawienie klucza i wektora inicjalizacyjnego
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            // Utworzenie obiektu szyfrującego
            ICryptoTransform encryptor = aes.CreateEncryptor();

            // Otwarcie strumienia dla danych
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Utworzenie strumienia szyfrującego, który będzie używany do zapisu danych
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    // Zmiana tekstu na bajty i zapisanie ich w strumieniu szyfrującym
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    // Pobranie szyfrowanych danych z pamięci i zamiana ich na tekst
                    byte[] encryptedBytes = memoryStream.ToArray();
                    string encryptedText = Convert.ToBase64String(encryptedBytes);

                    return encryptedText;


                }
            }
        }

        static string Decrypt(string encryptedText, string key, string iv)
        {
            // Utworzenie nowej instancji klasy Aes
            Aes aes = Aes.Create();

            // Ustawienie klucza i wektora inicjalizacyjnego
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            // Utworzenie obiektu deszyfrującego
            ICryptoTransform decryptor = aes.CreateDecryptor();

            // Otwarcie strumienia dla danych
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Utworzenie strumienia deszyfrującego, który będzie używany do zapisu danych
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                {
                    // Zmiana tekstu na bajty i zapisanie ich w strumieniu deszyfrującym
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    // Pobranie odszyfrowanych danych z pamięci i zamiana ich na tekst
                    byte[] decryptedBytes = memoryStream.ToArray();
                    string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                    return decryptedText;
                }
            }
        }

    }
}