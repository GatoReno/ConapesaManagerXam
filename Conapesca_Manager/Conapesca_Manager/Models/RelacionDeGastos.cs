using System;
using System.Collections.Generic;
using System.Text;

namespace Conapesca_Manager.Models
{
    public class RelacionDeGastos
    {
        public int ID { get; set; }
        public string Orden { get; set; }
        public string Fecha { get; set; }
        public string AreaDeAdscripcion { get; set; }
        public string NombreDelPrestador { get; set; }
        public string RFC { get; set; }
        public string CRUP { get; set; }
        public string Observaciones { get; set; }

        public string Firma_Dependencia { get; set; }
        public string Firma_Prestador { get; set; }
        public string Responsable_Area { get; set; }
        public string Firma_Tec_UPSZ { get; set; }
    }

    public class Concepto_Ticket {
     public int Num_Ticket { get; set; }
        public string Fecha { get; set; }
        public string Concepto { get; set; }
        public float Importe { get; set; }
        public float Sub_Total { get; set; }
    }

    public class Concepto_Factura
    {
        public int Num_Ticket { get; set; }
        public string Fecha { get; set; }
        public string Concepto { get; set; }
        public float Importe { get; set; }
        public float Sub_Total { get; set; }
    }
}
