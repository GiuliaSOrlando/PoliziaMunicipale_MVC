using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PoliziaMunicipale_MVC.Controllers
{
    public class ViolazioniController : Controller
    {

        private string connectionString;
        public static List<TipoDiViolazione> listaviolazioni = new List<TipoDiViolazione>();
        public ActionResult Index()
        {
            TempData["controllerDorigine"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            List<TipoDiViolazione> listaviolazioni = TipoDiViolazione.GetDataFromDB(connectionString);
            return View(listaviolazioni);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TipoDiViolazione nuovaviolazione)
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand queryRegistraViolazione = new SqlCommand("INSERT INTO Tipo_Violazione(Descrizione) VALUES (@Descrizione)", conn);
                queryRegistraViolazione.Parameters.AddWithValue("@Descrizione", nuovaviolazione.Descrizione);


                int registrazioneeffettuata = queryRegistraViolazione.ExecuteNonQuery();
            }
            catch
            {

            }
            finally { conn.Close(); }
            return View(nuovaviolazione);
        }

        

    }
}