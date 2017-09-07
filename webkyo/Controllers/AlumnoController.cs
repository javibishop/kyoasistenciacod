using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Kyo.Entidades;
using webkyo.Models;
using Microsoft.AspNet.Identity;
using System.Web.UI.WebControls;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace webkyo.Controllers
{
    [Authorize(Roles="Admin")]
	public class AlumnoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Alumnoes
        public ActionResult Index()
        {
            return View(db.Alumnos.ToList());
        }

		private void GetDojos(int id)
		{
			var dojos = db.Dojos.Select(x =>
								new SelectListItem
								{
									Value = x.Id.ToString(),
									Text = x.Nombre,
									Selected = x.Id == id ? true : false
								});
			ViewBag.Dojos = dojos.ToList<SelectListItem>();
		}

		private void GetCinturones(int id)
		{
			var cinturones = db.Cinturones.Select(x =>
								new SelectListItem
								{
									Value = x.Id.ToString(),
									Text = x.Nombre,
									Selected = x.Id == id ? true : false
								});

			ViewBag.Cinturones = cinturones.ToList<SelectListItem>();
		}

		private void SetAuditoria(Alumno alumno)
		{
			if (alumno.Id == 0)
			{
				//System.Web.HttpContext.Current.User.Identity.IsAuthenticated();
				alumno.FechaAlta = DateTime.Now;
				alumno.UsuarioAltaId = User.Identity.GetUserId();
				alumno.FechaModificacion = DateTime.Now;
				alumno.UsuarioModificacionId = User.Identity.GetUserId();
			}
			else
			{
				alumno.FechaModificacion = DateTime.Now;
				alumno.UsuarioModificacionId = User.Identity.GetUserId();

				//agregar los campos de auditoria como hiden.
			}
		}

        // GET: Alumnoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnos.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // GET: Alumnoes/Create
        public ActionResult Create()
        {
			//ModelState.Remove("Dojo");
			//ModelState.Remove("Cinturon");

			this.GetCinturones(0);
			this.GetDojos(0);

            return View();
        }

        // POST: Alumnoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellido,Telefono,Edad,Sexo,DojoId,CinturonId")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
				this.SetAuditoria(alumno);
				alumno.Dojo = db.Dojos.First(d => d.Id == alumno.DojoId);
				alumno.Cinturon = db.Cinturones.First(d => d.Id == alumno.CinturonId);
                db.Alumnos.Add(alumno);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alumno);
        }

		// GET: Alumnoes/VerAsistencia/5
		public ActionResult VerAsistencia(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnos.Find(id);
			if (alumno == null)
			{
				return HttpNotFound();
			}

			var asistencias = db.Asistencias.Where(a => a.AlumnoId == id);
			AsistenciaVM asistenciaVM = new AsistenciaVM();

			asistenciaVM.Alumno = alumno.Nombre + alumno.Apellido;
			foreach (Asistencia asistencia in asistencias)
			{
				asistenciaVM.Asistencias.Add(asistencia);
			}
			return View("VerAsistencia", asistenciaVM);
        }

		public ActionResult VerExamen(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnos.Find(id);
			if (alumno == null)
			{
				return HttpNotFound();
			}

			var examenes = db.Examenes.Where(a => a.AlumnoId == id);
			ExamenVM examenVM = new ExamenVM();

			examenVM.Alumno = alumno.Nombre + alumno.Apellido;
			foreach (Examen examen in examenes)
			{
				examenVM.Examenes.Add(examen);
			}
			return View("VerExamen", examenVM);
        }
		

		// GET: Alumnoes/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Alumno alumno = db.Alumnos.Find(id);
			if (alumno == null)
			{
				return HttpNotFound();
			}

			this.GetCinturones(alumno.Cinturon.Id);
			this.GetDojos(alumno.Dojo.Id);
			return View(alumno);
		}

        // POST: Alumnoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Nombre,Apellido,Telefono,Edad,Sexo,FechaAlta,FechaModificacion,UsuarioAltaId,UsuarioModificacionId")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
				this.SetAuditoria(alumno);
                db.Entry(alumno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alumno);
        }

        // GET: Alumnoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnos.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // POST: Alumnoes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			try
			{
				Alumno alumno = db.Alumnos.Find(id);
				db.Alumnos.Remove(alumno);
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
						return Content("<script language='javascript' type='text/javascript'>alert('No se puede eliminar este Alumno. Esta relacionado con otras entidades!');window.history.back();</script>");
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
