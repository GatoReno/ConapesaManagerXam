
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace Conapesca_Manager.Models
    {
     
    public class Tabla
    {
        public List<Table> Table { get; set; }
    }

    public class RootObject
    {
        public DatosEnvio DatosEnvio { get; set; }
        public DatosEnvioJson DatosEnvioJson { get; set; }
        public Tablas Tablas { get; set; }
        public string bandera { get; set; }
        public string mensaje { get; set; }
    }

    public class DatosEnvio
    {
        public string Usuario { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public int AdminType { get; set; }

    }

}