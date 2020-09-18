using DEMO.Core.DAL.Entities.Master;
using DEMO_SERVER.Core.Factories;
using DEMO_SERVER.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DEMO.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        ICustomerRepository repo;

        public CustomerController()
        {
            repo = new RepositoryFactory().CreateCustomerRepository();
        }

        [HttpGet]
        [Route("get-all")]
        public IEnumerable<Customer> GetCustomers()
        {
            try
            {
                return repo.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-customer-by-id")]
        public Customer GetCustomerbyID(long id)
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
        public long AddOrUpdateItem([FromBody]Customer model)
        {
            try
            {
                return repo.AddOrUpdate(model);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        [HttpGet]
        [Route("delete")]
        public string DeleteOrder(long id)
        {
            try
            {
                return repo.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}