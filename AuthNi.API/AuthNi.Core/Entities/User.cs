using AuthNi.Core.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AuthNi.Core.Entities
{
    internal class User
    {
        public Guid Id { get; }
        public string Username { get; }
        public string Email { get; }
        public string PasswordHash { get; } // Хэшированный пароль


        private static readonly string FixedSalt = "MySecretSalt1234";

        private static readonly int HashSize = 20; // Размер хэша в байтах
        private static readonly int Iterations = 10000; // Количество итераций

        // Соль для хэширования (опционально)

        public User(Guid id, string username, string email, string passwordHash)
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
         
        }
        public static (User User, string Error) Create(Guid id, string username, string email, string password)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(password) && string.IsNullOrEmpty(username)){
                error = "password or username is null";

            }
            string passwordHash = HashPassword(password);
            var user = new User(id, username, email, passwordHash);
            return (user, error);
        }
        private static string HashPassword(string password)
        {
            //create a salt
            byte[] saltBytes = Encoding.UTF8.GetBytes(FixedSalt);


            //generic a hash
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            //combinatorics salt and hash
            byte[] hashBytes = new byte[saltBytes.Length + HashSize];
            Array.Copy(saltBytes, 0, hashBytes, 0, saltBytes.Length);
            Array.Copy(hash, 0, hashBytes, saltBytes.Length, HashSize);

            // Преобразуем комбинированный хэш в строку
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Преобразуем сохраненный хэш из строки в массив байтов
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // we extract salt
            // Извлекаем соль
            byte[] salt = Encoding.UTF8.GetBytes(FixedSalt);
            Array.Copy(hashBytes, 0, salt, 0, salt.Length);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Проверяем, совпадают ли хэши
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + salt.Length] != hash[i])
                {
                    return false;
                }
            }

            return true;

        }
        // Дополнительные поля
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Дата создания пользователя
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
