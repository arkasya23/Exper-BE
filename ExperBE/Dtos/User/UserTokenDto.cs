using System;

namespace ExperBE.Dtos.User
{
    public class UserTokenDto
    {
        public UserTokenDto(string token, DateTime validUntil)
        {
            Token = token;
            ValidUntil = validUntil;
        }

        public string Token { get; }
        public DateTime ValidUntil { get; }
    }
}
