using Dapper;
using DEMO.Core.DAL.Entities.Common;
using DEMO_SERVER.Core.DAL.Entities.Master;
using DEMO_SERVER.Core.Factories;
using DEMO_SERVER.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Repositories
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IDbConnection _db;
        public UserRepository(DbContext context) : base(context)
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDataContext"].ConnectionString);
        }
        public void AddOrUpdate(User model)
        {
            throw new NotImplementedException();
        }

        public long AddOrUpdateUser(User model)
        {
            try
            {
                //check if username already exist
                var user = db.Set<User>().FirstOrDefault(x => x.UserName.ToLower() == model.UserName.ToLower() && x.ID != model.ID);
                if (user != null)
                {
                    throw new Exception("Username already exist. Please choose a unique username");
                }

                var oldModel = Get(x => x.ID == model.ID).FirstOrDefault();

                using (var trx = db.Database.BeginTransaction())
                {
                    if (oldModel != null)
                    {
                        //update values
                        model.UpdatedOn = DateTime.Now;
                        //update values
                        db.Entry<User>(oldModel).CurrentValues.SetValues(model);
                        db.SaveChanges();

                    }
                    else
                    {
                        //insert new model
                        db.Set<User>().Add(model);
                        db.SaveChanges();

                    }

                    trx.Commit();

                    return model.ID;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Delete(long Id)
        {
            try
            {
                var model = Get(x => x.ID == Id).FirstOrDefault();

                if (model == null)
                {
                    throw new Exception($"Cannot find User with ID = {Id}");
                }
                using (var trx = db.Database.BeginTransaction())
                {
                    model.UpdatedOn = DateTime.Now;
                    model.RecordStatus = DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Deleted;

                    db.Set<User>().Attach(model);
                    db.Entry<User>(model).State = EntityState.Modified;
                    db.SaveChanges();

                    trx.Commit();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(User model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> Get(Expression<Func<User, bool>> condition)
        {
            return db.Set<User>()
                    .Where(condition)
                    .ToList()
                    .AsQueryable();
        }

        public IQueryable<User> GetAll()
        {
            return db.Set<User>()
                    .ToList().AsQueryable();
        }

        public User GetUser(string username, string password)
        {
            User user = null;
            var repo = new RepositoryFactory().CreateGenericRepository<User>();

            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDataContext"].ConnectionString))
                {
                    if (conn.State != ConnectionState.Open) { conn.Open(); }

                    var res = conn.Query<User>($"select * from Users where UserName = '{username}' collate SQL_Latin1_General_CP1_CS_AS and Password = '{password}' collate SQL_Latin1_General_CP1_CS_AS and RecordStatus = 0");
                    conn.Close();
                    if (res.Count() == 1)
                    {
                        Task.Run(async () => {
                            user = await repo.GetFirstAsync(x => x.UserName == username && x.Password == password && x.RecordStatus == BaseEntity.RecordStatusType.Active);
                        }).Wait();
                    }
                }
                return user;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
