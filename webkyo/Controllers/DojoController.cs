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
    public class DojoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dojo
        public ActionResult Index()
        {
            return View(db.Dojos.ToList());
        }

        // GET: Dojo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dojo dojo = db.Dojos.Find(id);
            if (dojo == null)
            {
                return HttpNotFound();
            }
            return View(dojo);
        }

        // GET: Dojo/Create
        public ActionResult Create()
        {
            return View();
        }

		private void SetAuditoria(Dojo dojo)
		{
			if (dojo.Id == 0)
			{
				//System.Web.HttpContext.Current.User.Identity.IsAuthenticated();
				dojo.FechaAlta = DateTime.Now;
				dojo.UsuarioAltaId = User.Identity.GetUserId();
				dojo.FechaModificacion = DateTime.Now;
				dojo.UsuarioModificacionId = User.Identity.GetUserId();
			}
			else
			{
				dojo.FechaModificacion = DateTime.Now;
				dojo.UsuarioModificacionId = User.Identity.GetUserId();
			}
		}
        // POST: Dojo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Direccion,DiasClasesSemanales")] Dojo dojo)
        {
            if (ModelState.IsValid)
            {
				this.SetAuditoria(dojo);
                db.Dojos.Add(dojo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dojo);
        }

        // GET: Dojo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dojo dojo = db.Dojos.Find(id);

            if (dojo == null)
            {
                return HttpNotFound();
            }
            return View(dojo);
        }

        // POST: Dojo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Nombre,Direccion,DiasClasesSemanales,FechaAlta,FechaModificacion,UsuarioAltaId,UsuarioModificacionId")] Dojo dojo)
        {
            if (ModelState.IsValid)
            {
				this.SetAuditoria(dojo);
                db.Entry(dojo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dojo);
        }

        // GET: Dojo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dojo dojo = db.Dojos.Find(id);
            if (dojo == null)
            {
                return HttpNotFound();
            }
            return View(dojo);
        }

        // POST: Dojo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Dojo dojo = db.Dojos.Find(id);
				db.Dojos.Remove(dojo);
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
						return Content("<script language='javascript' type='text/javascript'>alert('No se puede eliminar este Dojo. Esta relacionado con otras entidades!');window.history.back();</script>");
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
