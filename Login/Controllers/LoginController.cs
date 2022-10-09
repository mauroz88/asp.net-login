using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Login.Models;
using System.Data;  
using System.Data.SqlClient;


namespace Login.Controllers
{
    public class LoginController : Controller
    {   // variable cadena para la conexion con la base de datos.
        static string cadena = "data source = DESKTOP-530G8NK\\WINCCPLUSMIG2008; Initial Catalog = DbLogin; Integrated Security = true";
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Salir()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Registrar( Usuario usuario)
        {
            bool registrado;
            string mensaje;

            if (usuario.Clave == usuario.ConfirmarClave)
            {
                usuario.Clave = usuario.Clave;

            }
            else {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena)) {
                SqlCommand cmd = new SqlCommand("RegistrarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo",usuario.Correo);
                cmd.Parameters.AddWithValue("Clave", usuario.Clave);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }            
        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {

            usuario.Clave = usuario.Clave; // aqui se gurda la clave encriptada

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("Clave", usuario.Clave);
                
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                usuario.idUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());    // respuesta del procesure lo convierto en variable int            

            }

            if (usuario.idUsuario != 0)
            {
                Session["usuario"] = usuario;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "usuario no encontrado";
                return View();
            }            
        }
    }
}