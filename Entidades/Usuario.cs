using System;
using System.Collections.Generic;

namespace Kyo.Entidades
{
	public class Usuario : EntidadBase
	{
		public string NombreUsuario { get; set; }
		public string Clave { get; set; }
		public List<Dojo> Dojos { get; set; }
	}
}
