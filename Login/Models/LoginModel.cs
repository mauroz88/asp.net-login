using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string Correo { get; set; }
        public string Clave  { get; set; }
        //esta propiedad no esta en la tabla pero la necesitamos para el formulario
        public string  ConfirmarClave { get; set; }
    }
}