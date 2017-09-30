using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Kyo.Entidades;
using webkyo.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace webkyo.Controllers
{
    [Authorize(Roles="Admin")]
	public class CinturonController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cinturons
        public ActionResult Index()
        {
            return View(db.Cinturones.ToList());
        }

		private void SetAuditoria(Cinturon Cinturon)
		{
			if (Cinturon.Id == 0)
			{
				//System.Web.HttpContext.Current.User.Identity.IsAuthenticated();
				Cinturon.FechaAlta = DateTime.Now;
				Cinturon.UsuarioAltaId = User.Identity.GetUserId();
				Cinturon.FechaModificacion = DateTime.Now;
				Cinturon.UsuarioModificacionId = User.Identity.GetUserId();
			}
			else
			{
				Cinturon.FechaModificacion = DateTime.Now;
				Cinturon.UsuarioModificacionId = User.Identity.GetUserId();
			}
		}

        // GET: Cinturons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinturon cinturon = db.Cinturones.Find(id);
            if (cinturon == null)
            {
                return HttpNotFound();
            }
            return View(cinturon);
        }

        // GET: Cinturons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cinturons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Nombre,ColorCodigo,Nivel,FechaAlta,FechaModificacion,UsuarioAltaId,UsuarioModificacionId")] Cinturon cinturon)
        {
            if (ModelState.IsValid)
            {
				this.SetAuditoria(cinturon);
                db.Cinturones.Add(cinturon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cinturon);
        }

        // GET: Cinturons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinturon cinturon = db.Cinturones.Find(id);
            if (cinturon == null)
            {
                return HttpNotFound();
            }
            return View(cinturon);
        }

        // POST: Cinturons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,ColorCodigo,Nivel,FechaAlta,FechaModificacion,UsuarioAltaId,UsuarioModificacionId")] Cinturon cinturon)
        {
            if (ModelState.IsValid)
            {
				this.SetAuditoria(cinturon);
                db.Entry(cinturon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinturon);
        }

        // GET: Cinturons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinturon cinturon = db.Cinturones.Find(id);
            if (cinturon == null)
            {
                return HttpNotFound();
            }
            return View(cinturon);
        }

        // POST: Cinturons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Cinturon cinturon = db.Cinturones.Find(id);
				db.Cinturones.Remove(cinturon);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			catch (DbUpdateException ex)
			{
				var sqlException = ex.GetBaseException() as SqlException;

				if (sqlException != null)
				{
					var number = sqlException.Number;

					if (number == 547)
					{
						return Content("<script language='javascript' type='text/javascript'>alert('No se puede eliminar este Cinturon. Esta relacionado con otras entidades!');window.history.back();</script>");
					}
				}

				return null;
			}
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
