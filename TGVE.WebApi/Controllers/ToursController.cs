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
using System.Web.Http.Description;
using TGVE.WebApi.Models;

namespace TGVE.WebApi.Controllers
{
    public class ToursController : ApiController
    {
        private TGVEEntities db = new TGVEEntities();

        // GET: api/Tours
        public IQueryable<Tour> GetTours()
        {
            return db.Tours;
        }

        // GET: api/Tours/5
        [ResponseType(typeof(Tour))]
        public async Task<IHttpActionResult> GetTour(int id)
        {
            Tour tour = await db.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            return Ok(tour);
        }

        // PUT: api/Tours/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTour(int id, Tour tour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tour.Id)
            {
                return BadRequest();
            }

            db.Entry(tour).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourExists(id))
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

        // POST: api/Tours
        [ResponseType(typeof(Tour))]
        public async Task<IHttpActionResult> PostTour(Tour tour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tours.Add(tour);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tour.Id }, tour);
        }

        // DELETE: api/Tours/5
        [ResponseType(typeof(Tour))]
        public async Task<IHttpActionResult> DeleteTour(int id)
        {
            Tour tour = await db.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            db.Tours.Remove(tour);
            await db.SaveChangesAsync();

            return Ok(tour);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TourExists(int id)
        {
            return db.Tours.Count(e => e.Id == id) > 0;
        }
    }
}