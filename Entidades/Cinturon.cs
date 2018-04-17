using System;
using System.ComponentModel.DataAnnotations;

namespace Kyo.Entidades
{
	public class Cinturon : EntidadBase
	{
		public string Nombre { get; set; }

        [Display(Name = "Color")]
        public string ColorCodigo { get; set; }
		public short Nivel { get; set; }
	}
}
