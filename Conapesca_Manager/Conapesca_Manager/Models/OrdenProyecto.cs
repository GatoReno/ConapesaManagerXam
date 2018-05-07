using System;
using System.Collections.Generic;
using System.Text;

namespace Conapesca_Manager.Models
{
    public class OrdenProyecto
    {
        public int ID { get; set; }
        public string Orden { get; set; }
        public string Fecha { get; set; }
        public string Proyecto { get; set; }
        public string NombreDelPrestador { get; set; }
        public string RFC { get; set; }
        public string CRUP { get; set; }
        public string AreaDeAdscripcion { get; set; }
        public string ObjetivoDeActividades { get; set; }
        public string MedioDeTransporte { get; set; }
        public string Itinerario { get; set; }
        public string Destino { get; set; }
        public string Inicio { get; set; }
        public string Termino { get; set; }
        public int Dias { get; set; }
        public float CuotaDiaria { get; set; }
        public float Importes { get; set; }
        public float TransporteTerrestre { get; set; }
        public float BoletoAereo { get; set; }
        public float TotalOrden { get; set; }
        public string CantidadRecibida { get; set; }
        public string Firma_Dependencia { get; set; }
        public string Firma_Prestador { get; set; }
        public string Responsable_Area { get; set; }
        public string Firma_Tec_UPSZ { get; set; }
    }
}
