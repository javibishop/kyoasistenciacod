using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace webkyo.Models
{
    [DataContract]
    public class AsistenciaGrafico
    {
        //DataContract for Serializing Data - required to serve in JSON format

        public AsistenciaGrafico()
        {
            //Meses.AddRange("1","2","3","4","5","6","7","8","9","10","11","12"));
            Meses.Add("1");
            Meses.Add("2");
            Meses.Add("3");
            Meses.Add("4");
            Meses.Add("5");
            Meses.Add("6");
            Meses.Add("7");
            Meses.Add("8");
            Meses.Add("9");
            Meses.Add("10");
            Meses.Add("11");
            Meses.Add("12");
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "Meses")]
        public List<string> Meses = new List<string>();

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