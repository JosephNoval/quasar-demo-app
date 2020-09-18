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
    [RoutePrefix("api/item")]
    public class ItemController : ApiController
    {
        IItemRepository repo;

        public ItemController()
        {
            repo = new RepositoryFactory().CreateItemRepository();
        }

        [HttpGet]
        [Route("get-all")]
        public IEnumerable<Item> GetOrders()
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
        [Route("get-item-by-id")]
        public Item GetItembyID(long id)
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
        public long AddOrUpdateItem([FromBody]Item model)
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