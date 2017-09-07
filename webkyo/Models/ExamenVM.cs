using Kyo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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