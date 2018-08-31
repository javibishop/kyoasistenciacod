using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kyo.Entidades
{
	public class Alumno : EntidadBase
	{
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public string Telefono { get; set; }
        public string Email { get; set; }
		public short Edad { get; set; }
		public short Sexo { get; set; }
		public int DojoId { get; set; }
		[ForeignKey("DojoId")]
		public virtual  Dojo Dojo { get; set; }
		
		public int CinturonId { get; set; }
		[ForeignKey("CinturonId")]
		public virtual Cinturon Cinturon { get; set; }

		//https://msdn.microsoft.com/en-us/data/jj679962
	}
}
