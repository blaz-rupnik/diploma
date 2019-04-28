using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DbDataAccess;

namespace Diploma_serverless.Controllers
{
    public class VacationLeaveController : ApiController
    {
        public IEnumerable<APP_VacationLeave> Get()
        {
            using(var context = new DiplomaDbEntities())
            {
                return context.APP_VacationLeave.ToList();
            }
        }

        public APP_VacationLeave Post(APP_VacationLeave model)
        {
            using(var context = new DiplomaDbEntities())
            {
                model.ID_VacationLeave = Guid.NewGuid();
                context.APP_VacationLeave.Add(model);
                context.SaveChanges();
                return model;
            }
        }
    }
}
