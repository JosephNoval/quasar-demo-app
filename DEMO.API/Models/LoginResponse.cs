using DEMO_SERVER.Core.DAL.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEMO.API.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
    }
}