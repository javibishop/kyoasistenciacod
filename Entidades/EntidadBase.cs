using System;


namespace Kyo
{
    public class EntidadBase
    {
		public int Id { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime FechaModificacion { get; set; }
		public string UsuarioAltaId { get; set; }
		public string UsuarioModificacionId { get; set; }
    }
}
