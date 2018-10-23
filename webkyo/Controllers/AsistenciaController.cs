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
using Newtonsoft.Json;
using System.Web.Helpers;

namespace webkyo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AsistenciaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Asistencias
        public ActionResult Index()
        {
            ViewBag.fecha = DateTime.Now;
            return View(db.Alumnos.ToList().OrderBy(a => a.Apellido));
        }

        private void GetAnios()
        {
            List<SelectListItem> anios = new List<SelectListItem>();
            for (int i = 2017; i < DateTime.Now.Year + 1; i++)
            {
                anios.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.Anios = anios;
        }

        private void GetMeses()
        {
            List<SelectListItem> meses = new List<SelectListItem>();
            meses.Add(new SelectListItem { Value = "1", Text = "Enero" });
            meses.Add(new SelectListItem { Value = "2", Text = "Febrero" });
            meses.Add(new SelectListItem { Value = "3", Text = "Marzo" });
            meses.Add(new SelectListItem { Value = "4", Text = "Abril" });
            meses.Add(new SelectListItem { Value = "5", Text = "Mayo" });
            meses.Add(new SelectListItem { Value = "6", Text = "Junio" });
            meses.Add(new SelectListItem { Value = "7", Text = "Julio" });
            meses.Add(new SelectListItem { Value = "8", Text = "Agosto" });
            meses.Add(new SelectListItem { Value = "9", Text = "Septiembre" });
            meses.Add(new SelectListItem { Value = "10", Text = "Octubre" });
            meses.Add(new SelectListItem { Value = "11", Text = "Noviembre" });
            meses.Add(new SelectListItem { Value = "12", Text = "Diciembre" });
            ViewBag.Meses = meses;
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

        public ActionResult RedirectGraficosMes()
        {
            return RedirectToAction("Asistencia/GraphTodosMes", "Asistencia");
        }
        public ActionResult GraphTodosMes()
        {
            ViewBag.Title = "Detalles de Asistencias por mes";
            this.GetAnios();
            this.GetMeses();
            return View("GraphTodosMes");
        }
        

        //[ValidateAntiForgeryToken]
        [HttpGet]
        public ActionResult generargrafico(string anio, int alumnoId)
        {
            Kyo.Entidades.Alumno alumno = db.Alumnos.Find(alumnoId);
            Kyo.Entidades.Dojo dojo = db.Dojos.Find(alumno.DojoId);
            AsistenciaGrafico graficoData = new Models.AsistenciaGrafico();
            graficoData.InitMeses();
            for (int i = 1; i < 13; i++)
            {
                var asistenciasFechas = db.Asistencias.Where(asis => asis.AlumnoId == alumnoId && asis.Fecha.Year.ToString() == anio && asis.Fecha.Month == i).Select(a => a.Fecha).OrderBy(a => a.Month);
                int semanas = 0;
                DateTime mesAnio = new DateTime(int.Parse(anio), i, 1);
                int mes = mesAnio.Month;
                //calculo la cantidad de semanas
                while (mes == mesAnio.Month)
                {
                    mesAnio = mesAnio.AddDays(7);
                    semanas++;
                }

                if (asistenciasFechas.Count() > 0)
                {
                    var fechasPorMes = asistenciasFechas.GroupBy(f => f.Month, (key, g) => new { Mes = key, DiasMesAsistio = g.Count() });
                    foreach (var fecha in fechasPorMes)
                    {
                        int diasMes = semanas * dojo.DiasClasesSemanales;
                        graficoData.Detalle.Add(new DetalleMeses { DiasAsistio = fecha.DiasMesAsistio, DiasMes = diasMes });
                    }
                }
                else
                {
                    int diasMes = semanas * dojo.DiasClasesSemanales;
                    graficoData.Detalle.Add(new DetalleMeses { DiasAsistio = 0, DiasMes = diasMes });
                }
            }
            

            return Json(graficoData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult generargraficoAlumnosMes(string anio, string mes, int dojoId = 1)
        {
            var alumnos = db.Alumnos.ToList();
            Kyo.Entidades.Dojo dojo = db.Dojos.Find(dojoId);
            int semanas = 0;
            int mess = int.Parse(mes);
            DateTime mesAnio = new DateTime(int.Parse(anio), mess, 1);

            //calculo la cantidad de semanas
            while (int.Parse(mes) == mesAnio.Month)
            {
                mesAnio = mesAnio.AddDays(7);
                semanas++;
            }

            int diasMes = semanas * dojo.DiasClasesSemanales;
            AsistenciaGrafico graficoData = new Models.AsistenciaGrafico();

            foreach (var alumno in alumnos)
            {
                var asistenciasFechas = db.Asistencias.Where(asis => asis.AlumnoId == alumno.Id && asis.Fecha.Year.ToString() == anio && asis.Fecha.Month == mess).Select(a => a.Fecha).OrderBy(a => a.Month);
                if(asistenciasFechas.Count() > 0)
                {
                    var fechasPorMes = asistenciasFechas.GroupBy(f => f.Month, (key, g) => new { Mes = key, DiasMesAsistio = g.Count() });

                    foreach (var fecha in fechasPorMes)
                    {
                        graficoData.Texto.Add(alumno.Nombre[0].ToString() +"."+ alumno.Apellido.Substring(0, 2));
                        graficoData.Detalle.Add(new DetalleMeses { DiasAsistio = fecha.DiasMesAsistio, DiasMes = diasMes });
                    }
                }
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
        //[ValidateAntiForgeryToken]
        public ActionResult GuardarAsistencia(Datos datos)
        {
            try
            {
                //var foger = Request.Headers["__RequestVerificationToken"];
                //var antiForgeryCookie = Request.Cookies[AntiForgeryConfig.CookieName];
                //var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;
                //AntiForgery.Validate(cookieValue, foger);

                if (datos.ids.Count > 0)
                {
                    var fechaSplit = datos.fechaString.Split('/');
                    var fecha = new DateTime(int.Parse(fechaSplit[2].Trim()), int.Parse(fechaSplit[0].Trim()), int.Parse(fechaSplit[1].Trim()));
                    var alumnosasistir = db.Alumnos.Where(a => datos.ids.Contains(a.Id));
                    foreach (Alumno alumno in alumnosasistir)
                    {
                        Asistencia asistencia = new Asistencia();
                        asistencia.Alumno = alumno;
                        switch (datos.turno)
                        {
                            case "0":
                                asistencia.Fecha = fecha.AddHours(8); //8 de la maniana
                                break;
                            case "1":
                                asistencia.Fecha = fecha.AddHours(15); //3 de la tarde
                                break;
                            case "2":
                                asistencia.Fecha = fecha.AddHours(20); //8 de la noche.
                                break;
                        }

                        if (!this.ValidarExisteAsistencia(alumno.Id, asistencia.Fecha))
                        {
                            this.SetAuditoria(asistencia);
                            db.Asistencias.Add(asistencia);
                        }
                    }

                    db.SaveChanges();
                    //return Content("<script language='javascript' type='text/javascript'>alert('" + xx + "');</script>");
                    return Json(new
                    {
                        success = true,
                        status = "OK",
                        responseText = "Asistencias guardadas con exito."
                    });
                }
                else
                    return Json(new
                    {
                        success = true,
                        status = "OK",
                        responseText = "No hay datos para guardar."
                    });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    status = "Error",
                    responseText = "Error al guardar la asistencia - " + e.Message + e.InnerException.Message
                });
            }
        }

        private bool ValidarExisteAsistencia(int alumnoId, DateTime fecha)
        {
            var asistencia = db.Asistencias.Where(a => a.AlumnoId == alumnoId && a.Fecha == fecha).FirstOrDefault();
            return asistencia != null ? true : false;
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

    public class Datos
    {
        public List<int> ids { get; set; }
        public string turno { get; set; }
        public string fechaString { get; set; }
        public DateTime fecha { get; set; }
    }
}
