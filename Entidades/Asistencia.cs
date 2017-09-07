using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kyo.Entidades
{
	public class Asistencia : EntidadBase
	{
		public int AlumnoId { get; set; }
		
		[ForeignKey("AlumnoId")]
		public Alumno Alumno { get; set; }

		public DateTime Fecha { get; set; }
		public string Comentario { get; set; }
	}
}
