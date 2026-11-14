using System;

namespace ObliSem4Api.DTO;

public class UserRegisterDto
{
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
}
