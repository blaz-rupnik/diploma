using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Diploma_serverless.Models;

namespace Diploma_serverless.Controllers
{
    [EnableCors("*", "*", "*")]
    public class VacationLeavesController : ApiController
    {
        private UsersServiceContext db = new UsersServiceContext();

        // GET: api/VacationLeaves
        public IQueryable<VacationLeave> GetVacationLeaves()
        {
            return db.VacationLeaves;
        }

        // GET: api/VacationLeaves/5
        [ResponseType(typeof(VacationLeave))]
        public async Task<IHttpActionResult> GetVacationLeave(Guid id)
        {
            VacationLeave vacationLeave = await db.VacationLeaves.FindAsync(id);
            if (vacationLeave == null)
            {
                return NotFound();
            }

            return Ok(vacationLeave);
        }

        // PUT: api/VacationLeaves/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVacationLeave(Guid id, VacationLeave vacationLeave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vacationLeave.Id)
            {
                return BadRequest();
            }

            db.Entry(vacationLeave).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacationLeaveExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/VacationLeaves
        [ResponseType(typeof(VacationLeave))]
        public async Task<IHttpActionResult> PostVacationLeave(VacationLeave vacationLeave)
        {
            vacationLeave.Id = Guid.NewGuid();
            //alway first in pending
            vacationLeave.StatusId = Constants.VacationRequestStatus_Pending;
            vacationLeave.DaysPending = 0;

            var vacationLeaveCollides = db.VacationLeaves
                .Where(x => (x.UserId == vacationLeave.UserId && vacationLeave.DateFrom >= x.DateFrom && vacationLeave.DateFrom < x.DateTo) ||
                (x.UserId == vacationLeave.UserId && vacationLeave.DateTo >= x.DateFrom && vacationLeave.DateTo < x.DateTo))
                .FirstOrDefault();

            if(vacationLeaveCollides != null)
            {
                return Conflict();
            }

            db.VacationLeaves.Add(vacationLeave);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VacationLeaveExists(vacationLeave.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            VacationLeave vacationLeaveReturn = db.VacationLeaves.Include(y => y.User).Where(x => x.Id == vacationLeave.Id).FirstOrDefault();
            return CreatedAtRoute("DefaultApi", new { id = vacationLeave.Id }, vacationLeaveReturn);
        }

        // DELETE: api/VacationLeaves/5
        [ResponseType(typeof(VacationLeave))]
        public async Task<IHttpActionResult> DeleteVacationLeave(Guid id)
        {
            VacationLeave vacationLeave = await db.VacationLeaves.FindAsync(id);
            if (vacationLeave == null)
            {
                return NotFound();
            }

            db.VacationLeaves.Remove(vacationLeave);
            await db.SaveChangesAsync();

            return Ok(vacationLeave);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VacationLeaveExists(Guid id)
        {
            return db.VacationLeaves.Count(e => e.Id == id) > 0;
        }
    }
}