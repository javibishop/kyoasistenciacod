using Kyo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webkyo.Models
{
	public class AsistenciaVM
	{
		public string Alumno { get; set; }
		public List<Asistencia> Asistencias { get; set; }

		public AsistenciaVM()
		{
			this.Asistencias = new List<Asistencia>();
		}
	}
}