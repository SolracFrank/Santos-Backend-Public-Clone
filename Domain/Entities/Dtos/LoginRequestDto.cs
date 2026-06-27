using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Dtos
{
    public class LoginRequestDto
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
