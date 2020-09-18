using DEMO_SERVER.Core.DAL.Entities.Master;
using DEMO_SERVER.Core.Factories;
using DEMO_SERVER.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace DEMO.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserController : ApiController
    {
        IUserRepository repo;

        public UserController()
        {
            repo = new RepositoryFactory().CreateUserRepository();
        }

        [HttpGet]
        [Route("get-users")]
        public IEnumerable<User> GetUsers()
        {
            return repo.GetAll();
        }
        [HttpGet]
        [Route("get-user-by-id")]
        public User GetUserbyID(long id)
        {
            try
            {
                return repo.Get(x => x.ID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        [Route("add-or-update")]
        public long AddOrUpdateUser([FromBody]User user)
        {
            try
            {
                return repo.AddOrUpdateUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        [HttpGet]
        [Route("delete")]
        public string DeleteUser(int id)
        {
            try
            {
                return repo.Delete(id);
            }
            catch (Exception)
            {

                return "Unable to delete user. Please contact administrator.";
            }
        }

       
    }
}