using Kyo.Entidades;
using System.Collections.Generic;

namespace webkyo.Models
{
    public class ExamenVM
	{
		public string Alumno { get; set; }
		public List<Examen> Examenes { get; set; }

		public ExamenVM()
		{
			this.Examenes = new List<Examen>();
		}
	}
}