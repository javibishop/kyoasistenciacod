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
    public class ExamenController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Examen
        public ActionResult Index()
        {
            return View(db.Examenes.ToList());
        }
		private void GetCinturoneDesde(int id)
		{
			var cinturones = db.Cinturones.Select(x =>
								new SelectListItem
								{
									Value = x.Id.ToString(),
									Text = x.Nombre,
									Selected = x.Id == id ? true : false
								});
			ViewBag.CinturonDesde = cinturones.ToList<SelectListItem>();
		}

        private void GetCinturoneHasta(int id)
		{
			var cinturones = db.Cinturones.Select(x =>
								new SelectListItem
								{
									Value = x.Id.ToString(),
									Text = x.Nombre,
									Selected = x.Id == id ? true : false
								});
			ViewBag.CinturonHasta = cinturones.ToList<SelectListItem>();
		}

		private void GetAlumnos(int id)
		{
			var alumnos = db.Alumnos.Select(x =>
								new SelectListItem
								{
									Value = x.Id.ToString(),
									Text = x.Nombre,
									Selected = x.Id == id ? true : false
								});

			ViewBag.Alumnos = alumnos.ToList<SelectListItem>();
		}

		private void SetAuditoria(Examen examen)
		{
			if (examen.Id == 0)
			{
				//System.Web.HttpContext.Current.User.Identity.IsAuthenticated();
				examen.FechaAlta = DateTime.Now;
				examen.UsuarioAltaId = User.Identity.GetUserId();
				examen.FechaModificacion = DateTime.Now;
				examen.UsuarioModificacionId = User.Identity.GetUserId();
			}
			else
			{
				examen.FechaModificacion = DateTime.Now;
				examen.UsuarioModificacionId = User.Identity.GetUserId();
			}
		}

        // GET: Examen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examenes.Find(id);
            examen.Alumno = db.Alumnos.First(d => d.Id == examen.AlumnoId);
			examen.CinturonActual = db.Cinturones.First(d => d.Id == examen.CinturonActualId);
            examen.CinturonProximo = db.Cinturones.First(d => d.Id == examen.CinturonProximoId);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // GET: Examen/Create
        public ActionResult Create()
        {
            this.GetAlumnos(0);
            this.GetCinturoneDesde(0);
            this.GetCinturoneHasta(0);

            return View();
        }

        // POST: Examen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,AlumnoId,Fecha,CinturonActualId,CinturonProximoId,Comentario,Aprobado,FechaAlta,FechaModificacion,UsuarioAltaId,UsuarioModificacionId")] Examen examen)
        {
			//ApplicationUserManager
			//var user = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>(System.Web.HttpContext.Current.User.Identity.Name).Users.f;
			
			if (ModelState.IsValid)
            {
                examen.Alumno = db.Alumnos.First(d => d.Id == examen.AlumnoId);
				examen.CinturonActual = db.Cinturones.First(d => d.Id == examen.CinturonActualId);
                examen.CinturonProximo = db.Cinturones.First(d => d.Id == examen.CinturonProximoId);

				this.SetAuditoria(examen);
				if(examen.Aprobado)
                {
                    examen.Alumno.Cinturon = examen.CinturonProximo;
                }
                db.Examenes.Add(examen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(examen);
        }

        // GET: Examen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examenes.Find(id);
             examen.Alumno = db.Alumnos.First(d => d.Id == examen.AlumnoId);
				examen.CinturonActual = db.Cinturones.First(d => d.Id == examen.CinturonActualId);
                examen.CinturonProximo = db.Cinturones.First(d => d.Id == examen.CinturonProximoId);
            if (examen == null)
            {
                return HttpNotFound();
            }

            this.GetAlumnos(examen.AlumnoId);
            this.GetCinturoneDesde(examen.CinturonActualId);
            this.GetCinturoneHasta(examen.CinturonProximoId);

            return View(examen);
        }

        // POST: Examen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AlumnoId,Fecha,CinturonActualId,CinturonProximoId,Comentario,Aprobado,FechaAlta,FechaModificacion,UsuarioAltaId,UsuarioModificacionId")] Examen examen)
        {
            if (ModelState.IsValid)
            {
                examen.Alumno = db.Alumnos.First(d => d.Id == examen.AlumnoId);
				examen.CinturonActual = db.Cinturones.First(d => d.Id == examen.CinturonActualId);
                examen.CinturonProximo = db.Cinturones.First(d => d.Id == examen.CinturonProximoId);

				this.SetAuditoria(examen);

                db.Entry(examen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(examen);
        }

        // GET: Examen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examenes.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // POST: Examen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Examen examen = db.Examenes.Find(id);
				db.Examenes.Remove(examen);
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
						return Content("<script language='javascript' type='text/javascript'>alert('No se puede eliminar este Examen. Esta relacionado con otras entidades!');window.history.back();</script>");
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
