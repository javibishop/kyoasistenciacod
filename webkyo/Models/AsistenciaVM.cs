using Kyo.Entidades;
using System.Collections.Generic;

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