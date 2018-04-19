using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace webkyo.Models
{
    [DataContract]
    public class AsistenciaGrafico
    {
        public AsistenciaGrafico()
        {
            Meses.Add("Enero");
            Meses.Add("Febrero");
            Meses.Add("Marzo");
            Meses.Add("Abril");
            Meses.Add("Mayo");
            Meses.Add("Junio");
            Meses.Add("Julio");
            Meses.Add("Agosto");
            Meses.Add("Septiembre");
            Meses.Add("Octubre");
            Meses.Add("Noviembre");
            Meses.Add("Diciembre");
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "Meses")]
        public List<string> Meses = new List<string>();

        [DataMember(Name = "Anio")]
        public int Anio = 0;

        [DataMember(Name = "ColorBorde1")]
        public string ColorBorde1 = "#3e95cd";

        [DataMember(Name = "ColorBorde2")]
        public string ColorBorde2 = "#c45850";

        [DataMember(Name = "ColorFondo1")]
        public string ColorFondo1 = "#3e95cd";

        [DataMember(Name = "ColorFondo2")]
        public string ColorFondo2 = "#c45850";

        [DataMember(Name = "Detalle")]
        public List<DetalleMeses> Detalle = new List<DetalleMeses>();
    }

    public class DetalleMeses
    {
        [DataMember(Name = "DiasAsistio")]
        public int DiasAsistio = 0;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "DiasMes")]
        public int DiasMes = 0;
    }
}