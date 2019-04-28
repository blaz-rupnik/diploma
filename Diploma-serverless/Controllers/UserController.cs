using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DbDataAccess;

namespace Diploma_serverless.Controllers
{
    public class UserController : ApiController
    {
        public IEnumerable<APP_User> Get()
        {
            using(var context = new DiplomaDbEntities())
            {
                return context.APP_User.ToList();
            }
        }

        public APP_User Get(Guid userId)
        {
            using(var context = new DiplomaDbEntities())
            {
                return context.APP_User.FirstOrDefault(x => x.ID_User == userId);
            }
        }

        public APP_User Post(APP_User model)
        {
            using(var context = new DiplomaDbEntities())
            {
                model.ID_User = Guid.NewGuid();
                context.APP_User.Add(model);
                context.SaveChanges();
                return model;
            }
        }

        public HttpResponseMessage Put(APP_User model)
        {
            using(var context = new DiplomaDbEntities())
            {
                var result = context.APP_User.FirstOrDefault(x => x.ID_User == model.ID_User);
                if(result != null)
                {
                    result.Name = model.Name;
                    result.DateOfBirth = model.DateOfBirth;
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        public HttpResponseMessage Delete(Guid userId)
        {
            using(var context = new DiplomaDbEntities())
            {
                var itemToRemove = context.APP_User.SingleOrDefault(x => x.ID_User == userId);
                if(itemToRemove != null)
                {
                    context.APP_User.Remove(itemToRemove);
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
        }
    }
}
