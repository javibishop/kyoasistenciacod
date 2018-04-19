using System;
using System.Collections.Generic;
using System.Data;
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
	public class AsistenciaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Asistencias
        public ActionResult Index()
        {
            return View(db.Alumnos.ToList());
        }

        private void GetAnios()
		{
            List<SelectListItem> anios = new List<SelectListItem>();
            for(int i = 2017; i < DateTime.Now.Year + 1; i++)
            {
                anios.Add(new SelectListItem {Text= i.ToString(), Value = i.ToString() });
            }
			ViewBag.Anios = anios;
		}

		private void SetAuditoria(Asistencia asistencia)
		{
			if (asistencia.Id == 0)
			{
				//System.Web.HttpContext.Current.User.Identity.IsAuthenticated();
				asistencia.FechaAlta = DateTime.Now;
				asistencia.UsuarioAltaId = User.Identity.GetUserId();
				asistencia.FechaModificacion = DateTime.Now;
				asistencia.UsuarioModificacionId = User.Identity.GetUserId();
			}
			else
			{
				asistencia.FechaModificacion = DateTime.Now;
				asistencia.UsuarioModificacionId = User.Identity.GetUserId();
			}
		}

        // GET: Asistencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencia asistencia = db.Asistencias.Find(id);
            if (asistencia == null)
            {
                return HttpNotFound();
            }
            return View(asistencia);
        }

        // GET: Asistencias/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Graph(int alumnoid)
        {
            Kyo.Entidades.Alumno alumno = db.Alumnos.Find(alumnoid);
            ViewBag.Title = "Detalles de Asistencia de " + alumno.Nombre + " " + alumno.Apellido;
            this.GetAnios();
            return View();
        }

        //[ValidateAntiForgeryToken]
         [HttpGet]
        public ActionResult generargrafico(string anio, int alumnoId)
        {
            Kyo.Entidades.Alumno alumno = db.Alumnos.Find(alumnoId);
            Kyo.Entidades.Dojo dojo = db.Dojos.Find(alumno.DojoId);

            var asistenciasFechas = db.Asistencias.Where(asis => asis.AlumnoId == alumnoId && asis.Fecha.Year.ToString() == anio).Select(a => a.Fecha).OrderBy(a => a.Month);
            var fechasPorMes = asistenciasFechas.GroupBy(f => f.Month,  (key, g) => new { Mes = key, DiasMesAsistio = g.Count() });

            AsistenciaGrafico graficoData = null;
            foreach(var fecha in fechasPorMes)
            {
                int semanas = 0;
                DateTime mesAnio = new DateTime(int.Parse(anio), fecha.Mes, 1);
                int mes = mesAnio.Month;
                //calculo la cantidad de semanas
                while (mes == mesAnio.Month)
                {
                    mesAnio = mesAnio.AddDays(7);
                    semanas++;
                }

                int diasMes = semanas * dojo.DiasClasesSemanales;
                graficoData = new Models.AsistenciaGrafico();
                graficoData.Detalle.Add(new DetalleMeses { DiasAsistio = fecha.DiasMesAsistio, DiasMes = diasMes });
            }

            return Json(graficoData, JsonRequestBehavior.AllowGet); 
        }


        // POST: Asistencias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,Comentario")] Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
				this.SetAuditoria(asistencia);
                db.Asistencias.Add(asistencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(asistencia);
        }

        // GET: Asistencias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencia asistencia = db.Asistencias.Find(id);
            if (asistencia == null)
            {
                return HttpNotFound();
            }
            return View(asistencia);
        }

        // POST: Asistencias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Fecha,Comentario,FechaAlta,FechaModificacion,UsuarioAltaId,UsuarioModificacionId")] Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
				this.SetAuditoria(asistencia);
                db.Entry(asistencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asistencia);
        }


		// POST: Asistencias/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
//		[ValidateAntiForgeryToken]
		public ActionResult GuardarAsistencia(List<int> ids)
		{
            string xx = string.Empty;
            try
            {
                //var foger = Request.Headers["__RequestVerificationToken"];
                //var antiForgeryCookie = Request.Cookies[AntiForgeryConfig.CookieName];

                //var cookieValue = antiForgeryCookie != null
                //    ? antiForgeryCookie.Value
                //    : null;

                //AntiForgery.Validate(cookieValue, foger);
                xx = "paso vali foger";
                if (ids.Count > 0)
                {
                    var alumnosasistir = db.Alumnos.Where(a => ids.Contains(a.Id));
                    foreach (Alumno alumno in alumnosasistir)
                    {
                        Asistencia asistencia = new Asistencia();
                        asistencia.Alumno = alumno;
                        asistencia.Fecha = DateTime.Now;
                        this.SetAuditoria(asistencia);
                        db.Asistencias.Add(asistencia);
                        xx += "  agrego lista a aistencia";
                    }

                    db.SaveChanges();
                    xx += "  guardo cambios";

                    return Content("<script language='javascript' type='text/javascript'>alert('"+xx+"');</script>");
                }

                return View();
            }
            catch(Exception e)
            {
                throw e;
            }
		}

        // GET: Asistencias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencia asistencia = db.Asistencias.Find(id);
            if (asistencia == null)
            {
                return HttpNotFound();
            }
            return View(asistencia);
        }

        // POST: Asistencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Asistencia asistencia = db.Asistencias.Find(id);
				db.Asistencias.Remove(asistencia);
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
						return Content("<script language='javascript' type='text/javascript'>alert('No se puede eliminar esta Asistencia. Esta relacionado con otras entidades!');window.history.back();</script>");
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
