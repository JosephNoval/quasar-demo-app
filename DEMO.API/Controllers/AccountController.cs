using DEMO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace DEMO.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/account")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : ApiController
    {

        [HttpPost]
        [Route("login")]
        public LoginResponse SignIn([FromBody]SignInModel model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                return new LoginResponse() { Message = "Username is required." };
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                return new LoginResponse() { Message = "Password is required." };
            }

            if (!model.Login())
            {
                return new LoginResponse() { Message = model.Msg };
            }

            return new LoginResponse() { Message = model.Msg, Token = model.Token, User = model.User };
        }
    }
}