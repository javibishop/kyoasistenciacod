using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kyo.Entidades
{
	public class Examen : EntidadBase
	{

        public int AlumnoId { get; set; }
        [ForeignKey("AlumnoId")]
		public Alumno Alumno { get; set; }

		public DateTime Fecha { get; set; }
		public string Comentario { get; set; }
		public bool Aprobado { get; set; }

		
        public int CinturonActualId { get; set; }
		[ForeignKey("CinturonActualId")]

         [Display(Name = "Cinturon Actual")]
		public Cinturon CinturonActual { get; set; }
        
        public int CinturonProximoId { get; set; }
        [ForeignKey("CinturonProximoId")]

        [Display(Name = "Cinturon Proximo")]
        public Cinturon CinturonProximo { get; set; }
	}
}
