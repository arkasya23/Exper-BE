using ExperBE.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using ExperBE.Dtos.Users;
using ExperBE.Libraries;

namespace ExperBE.Models.Entities
{
    public class User : BaseEntity
    {
        private User()
        {
        }

        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string PasswordSalt { get; set; } = "";
        public ICollection<Trip> Trips { get; set; } = default!;

        public static User CreateNew(string email, string password)
        {
            var user = new User();
            user.Email = email;
            user.SetPassword(password);
            return user;
        }

        public void SetPassword(string password)
        {
            (Password, PasswordSalt) = PasswordHash.Encrypt(password);
        }

        public bool ValidatePassword(string password)
        {
            return PasswordHash.Verify(password, Password, PasswordSalt);
        }

        public UserTokenDto GenerateJwtToken()
        {
            var token = Jwt.GenerateJwtToken(Id.ToString(), Email);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new UserTokenDto(tokenString, token.ValidTo);
        }
    }
}
