using DEMO.Core.DAL.Entities.Transaction;
using DEMO_SERVER.Core.Factories;
using DEMO_SERVER.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DEMO.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        IOrderRepository repo;

        public OrderController()
        {
            repo = new RepositoryFactory().CreateOrderRepository();
        }
        
        [HttpGet]
        [Route("get-all")]
        public IEnumerable<Order> GetOrders()
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
        [Route("get-order-by-id")]
        public Order GetOrderbyID(long id)
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
        public long AddOrUpdateOrder([FromBody]Order model)
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