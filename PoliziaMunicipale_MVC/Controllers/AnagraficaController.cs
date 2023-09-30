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

    public class AnagraficaController : Controller
    {

        private string connectionString;
        public static List<Anagrafica> listaanagrafica = new List<Anagrafica>();
        public ActionResult Index()

        {
            TempData["controllerDorigine"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            List<Anagrafica> listaanagrafica = Anagrafica.GetDataFromDB(connectionString);
            return View(listaanagrafica);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Anagrafica nuovotrasgressore)
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand queryRegistraTrasgressore = new SqlCommand("INSERT INTO Anagrafica(Cognome, Nome, Indirizzo, Citta, CAP, Cod_Fisc) " +
                "VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @CAP, @Cod_Fisc)", conn);
                queryRegistraTrasgressore.Parameters.AddWithValue("@Cognome", nuovotrasgressore.Cognome);
                queryRegistraTrasgressore.Parameters.AddWithValue("@Nome", nuovotrasgressore.Nome);
                queryRegistraTrasgressore.Parameters.AddWithValue("@Indirizzo", nuovotrasgressore.Indirizzo);
                queryRegistraTrasgressore.Parameters.AddWithValue("@Citta", nuovotrasgressore.Citta);
                queryRegistraTrasgressore.Parameters.AddWithValue("@CAP", nuovotrasgressore.CAP);
                queryRegistraTrasgressore.Parameters.AddWithValue("@Cod_Fisc", nuovotrasgressore.CF);

                int registrazioneeffettuata = queryRegistraTrasgressore.ExecuteNonQuery();
            }
            catch
            {

            }
            finally { conn.Close(); }
            return View(nuovotrasgressore);
        }
    }
}