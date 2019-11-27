using System;
using VerticalSlice.POC.Core;

namespace VerticalSlice.POC.Services
{
    public abstract class AuthenticationResponse : BaseResponse
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Username { get; set; }
        public Guid UserId { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Token))
                throw new CoreException("AuthenticationResponse => Token parameter should not be empty!");
            else if (string.IsNullOrEmpty(Username))
                throw new CoreException("AuthenticationResponse => Username parameter should not be empty! It should be the user's email address.");
            else if (UserId == Guid.Empty)
                throw new CoreException("AuthenticationResponse => UserId should not be empty.");
        }
    }
}
