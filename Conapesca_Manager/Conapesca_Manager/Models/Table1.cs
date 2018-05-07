using System;
using System.Collections.Generic;
using System.Text;

namespace Conapesca_Manager.Models
{
    public class Table1
    {

        //Datos de orden
        public int idOrden { get; set; }
        public string noOrden { get; set; }
        public DateTime fecha { get; set; }
        public string orden { get; set; }
        public string proyecto { get; set; }


        //just one orden    
        public int idEmpleado { get; set; }
        public string objetivo { get; set; }
        public string medioTransporte { get; set; }
        public string intinerario { get; set; }
        public double terrestre { get; set; }
        public double aereo { get; set; }
        public string recibi { get; set; }

        // datos de destino

        public string destino { get; set; }
        public DateTime inicio { get; set; }
        public DateTime termino { get; set; }
        public int dias { get; set; }
        public string cuota { get; set; }
        public string importes { get; set; }

        //datos vuelo
        public string aerolinea { get; set; }
        public string vuelo { get; set; }
        public string salida { get; set; }
        public DateTime sFecha { get; set; }
        public string sHora { get; set; }
        public string regreso { get; set; }
        public DateTime rFecha { get; set; }
        public string rHora { get; set; }
        public string motivo { get; set; }

        //otros
        public bool delega  { set; get; }



    }


    public class Table
    {
        public string noOrden { get; set; }
        public DateTime fecha { get; set; }
        public string orden { get; set; }
        public string proyecto { get; set; }
        public string destino { get; set; }
        public DateTime inicio { get; set; }
        public DateTime termino { get; set; }
        public int dias { get; set; }
        public int cuota { get; set; }
        public int importes { get; set; }

        //otros
        public string email { get; set; }

        public List<Dato> datos { get; set; }

    }

    public class Tablas
    {
        public List<Table1> Table1 { get; set; }
        public List<Table> Table { get; set; }
        public List<string> campos { get; set; }
    }
   

    public class DatosEnvioJson
    {
    }


    public class Dato
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public int AdminType { get; set; }
    }

    public class Root
    {
        public DatosEnvio DatosEnvio { get; set; }
        public DatosEnvioJson DatosEnvioJson { get; set; }
        public Tablas tablas { get; set; }
        public string bandera { get; set; }
        public string mensaje { get; set; }
    }
}
