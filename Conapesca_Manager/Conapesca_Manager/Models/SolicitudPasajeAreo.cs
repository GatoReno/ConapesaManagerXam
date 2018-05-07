using System;
using System.Collections.Generic;
using System.Text;

namespace Conapesca_Manager.Models
{
    public class SolicitudPasajeAreo
    {
        public int ID { get; set; }
        public string Orden { get; set; }
        public string Fecha { get; set; }
        public string NombrePrestador { get; set; }
        public string RFC { get; set; }
        public string CRUP { get; set; }
        public string Area_Adscripcion { get; set; }
        public string AreoLinea { get; set; }
        public string Vuelo { get; set; }
        public string Salida { get; set; }
        public string Salida_Fecha { get; set; }
        public string Salida_Horario { get; set; }
        public string Regreso { get; set; }
        public string Regreso_Fecha { get; set; }
        public string Regreso_Horario { get; set; }
        public string MotivoDeViaje { get; set; }
        public string Firma_Dependencia { get; set; }
        public string Firma_Prestador { get; set; }
        public string Responsable_Area { get; set; }
        public string Firma_Tec_UPSZ { get; set; }
    }
}
