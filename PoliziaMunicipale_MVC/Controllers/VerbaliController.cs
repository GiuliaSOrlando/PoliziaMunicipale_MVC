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
    public class VerbaliController : Controller
    {

        private string connectionString;
        public static List<Verbale> listaverbali = new List<Verbale>();
        public ActionResult Index()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand queryVisualizzaVerbali = new SqlCommand("SELECT * FROM Verbale", conn);
                SqlDataReader readerVerbali = queryVisualizzaVerbali.ExecuteReader();
                while (readerVerbali.Read())
                {
                    Verbale verbale = new Verbale
                    {
                        IdVerbale = Convert.ToInt32(readerVerbali["IdVerbale"]),
                        DataViolazione = Convert.ToDateTime(readerVerbali["DataViolazione"]),
                        IndirizzoViolazione = readerVerbali["IndirizzoViolazione"].ToString(),
                        NominativoAgente = readerVerbali["Nominativo_Agente"].ToString(),
                        DataTrascrizioneVerbale = Convert.ToDateTime(readerVerbali["DataTrascrizioneVerbale"]),
                        Importo = Convert.ToDecimal(readerVerbali["Importo"]),
                        DecurtamentoPunti = Convert.ToInt32(readerVerbali["DecurtamentoPunti"]),
                        IdAnagrafica = Convert.ToInt32(readerVerbali["IdAnagrafica"]),
                        IdViolazione = Convert.ToInt32(readerVerbali["IdViolazione"])
                    };
                    listaverbali.Add(verbale);
                }
            }
            catch
            {

            }
            finally { conn.Close(); }
            return View(listaverbali);
        }

        public ActionResult Create()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;

            List<Anagrafica> listaanagrafica = Anagrafica.GetDataFromDB(connectionString);
            List<SelectListItem> listaAnagrafica = Anagrafica.ToSelectItems(listaanagrafica);

            List<TipoDiViolazione> listaviolazioni = TipoDiViolazione.GetDataFromDB(connectionString);
            List<SelectListItem> listaViolazioni = TipoDiViolazione.ToSelectList(listaviolazioni);

            ViewBag.ListaAnagrafica = listaAnagrafica;
            ViewBag.ListaViolazioni = listaViolazioni;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Verbale nuovoverbale)
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand queryRegistraVerbale = new SqlCommand("INSERT INTO Verbale (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, IdAnagrafica, IdViolazione) " +
                "VALUES (@DataViolazione, @IndirizzoViolazione, @NominativoAgente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @IdAnagrafica, @IdViolazione)", conn);

                queryRegistraVerbale.Parameters.AddWithValue("@DataViolazione", nuovoverbale.DataViolazione);
                queryRegistraVerbale.Parameters.AddWithValue("@IndirizzoViolazione", nuovoverbale.IndirizzoViolazione);
                queryRegistraVerbale.Parameters.AddWithValue("@NominativoAgente", nuovoverbale.NominativoAgente);
                queryRegistraVerbale.Parameters.AddWithValue("@DataTrascrizioneVerbale", nuovoverbale.DataTrascrizioneVerbale);
                queryRegistraVerbale.Parameters.AddWithValue("@Importo", nuovoverbale.Importo);
                queryRegistraVerbale.Parameters.AddWithValue("@DecurtamentoPunti", nuovoverbale.DecurtamentoPunti);
                queryRegistraVerbale.Parameters.AddWithValue("@IdAnagrafica", nuovoverbale.IdAnagrafica);
                queryRegistraVerbale.Parameters.AddWithValue("@IdViolazione", nuovoverbale.IdViolazione);

                int registrazioneeffettuata = queryRegistraVerbale.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                
            }
            finally { conn.Close(); }

            return RedirectToAction("Index");
        }
        public ActionResult VisualizzaTotaleVerbaliTrascrittiPerTrasgressore()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            List<TrasgressoreVerbale> trasgressoriVerbali = new List<TrasgressoreVerbale>();

            try
            {
                conn.Open();
                SqlCommand queryVisualizzaTotaleVerbaliPerTrasgressore = new SqlCommand(@"
            SELECT
                Verbale.IdAnagrafica,
                Anagrafica.Nome AS NomeTrasgressore,
                Anagrafica.Cognome AS CognomeTrasgressore,
                COUNT(*) AS TotaleVerbaliTrascritti
            FROM Verbale
            INNER JOIN Anagrafica ON Verbale.IdAnagrafica = Anagrafica.IdAnagrafica
            GROUP BY Verbale.IdAnagrafica, Anagrafica.Nome, Anagrafica.Cognome", conn);

                SqlDataReader reader = queryVisualizzaTotaleVerbaliPerTrasgressore.ExecuteReader();

                while (reader.Read())
                {
                    TrasgressoreVerbale trasgressoreVerbali = new TrasgressoreVerbale
                    {
                        IdAnagrafica = Convert.ToInt32(reader["IdAnagrafica"]),
                        NomeTrasgressore = reader["NomeTrasgressore"].ToString(),
                        CognomeTrasgressore = reader["CognomeTrasgressore"].ToString(),
                        TotaleVerbaliTrascritti = Convert.ToInt32(reader["TotaleVerbaliTrascritti"])
                    };

                    trasgressoriVerbali.Add(trasgressoreVerbali);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }

            return View(trasgressoriVerbali);
        }

        public ActionResult VisualizzaTotalePuntiDecurtatiPerTrasgressore()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            List<TrasgressorePunti> trasgressoriPuntiDecurtati = new List<TrasgressorePunti>();

            try
            {
                conn.Open();
                SqlCommand queryVisualizzaTotalePuntiDecurtatiPerTrasgressore = new SqlCommand(@"
            SELECT
                Verbale.IdAnagrafica,
                Anagrafica.Nome AS NomeTrasgressore,
                Anagrafica.Cognome AS CognomeTrasgressore,
                SUM(Verbale.DecurtamentoPunti) AS TotalePuntiDecurtati
            FROM Verbale
            INNER JOIN Anagrafica ON Verbale.IdAnagrafica = Anagrafica.IdAnagrafica
            GROUP BY Verbale.IdAnagrafica, Anagrafica.Nome, Anagrafica.Cognome", conn);

                SqlDataReader reader = queryVisualizzaTotalePuntiDecurtatiPerTrasgressore.ExecuteReader();

                while (reader.Read())
                {
                    TrasgressorePunti trasgressorePuntiDecurtati = new TrasgressorePunti
                    {
                        IdAnagrafica = Convert.ToInt32(reader["IdAnagrafica"]),
                        NomeTrasgressore = reader["NomeTrasgressore"].ToString(),
                        CognomeTrasgressore = reader["CognomeTrasgressore"].ToString(),
                        TotalePuntiDecurtati = Convert.ToInt32(reader["TotalePuntiDecurtati"])
                    };

                    trasgressoriPuntiDecurtati.Add(trasgressorePuntiDecurtati);
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessaggioErrore =  ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return View(trasgressoriPuntiDecurtati);
        }

        public ActionResult VisualizzaViolazioniDieci()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            List<ViolazionePuntiDieciQuattrocento> listaViolazionePuntiDieci = new List<ViolazionePuntiDieciQuattrocento>();

            try
            {
                conn.Open();
                SqlCommand queryVisualizzaViolazioniDieci = new SqlCommand(@"
            SELECT
                Verbale.Importo,
                Anagrafica.Cognome AS CognomeTrasgressore,
                Anagrafica.Nome AS NomeTrasgressore,
                Verbale.DataViolazione,
                Verbale.DecurtamentoPunti
            FROM Verbale
            INNER JOIN Anagrafica ON Verbale.IdAnagrafica = Anagrafica.IdAnagrafica
            WHERE Verbale.DecurtamentoPunti > 10", conn);

                SqlDataReader reader = queryVisualizzaViolazioniDieci.ExecuteReader();

                while (reader.Read())
                {
                    ViolazionePuntiDieciQuattrocento violazionePuntiDieci = new ViolazionePuntiDieciQuattrocento
                    {
                        Importo = Convert.ToDecimal(reader["Importo"]),
                        CognomeTrasgressore = reader["CognomeTrasgressore"].ToString(),
                        NomeTrasgressore = reader["NomeTrasgressore"].ToString(),
                        DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                        DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"])
                    };

                    listaViolazionePuntiDieci.Add(violazionePuntiDieci);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }

            return View(listaViolazionePuntiDieci);
        }

        public ActionResult VisualizzaViolazioniConImportoElevato()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PoliziaMunicipale"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            List<ViolazionePuntiDieciQuattrocento> violazioniImportoElevato = new List<ViolazionePuntiDieciQuattrocento>();

            try
            {
                conn.Open();
                SqlCommand queryVisualizzaViolazioniConImportoElevato = new SqlCommand(@"
            SELECT
                Verbale.Importo,
                Anagrafica.Cognome AS CognomeTrasgressore,
                Anagrafica.Nome AS NomeTrasgressore,
                Verbale.DataViolazione,
                Verbale.DecurtamentoPunti
            FROM Verbale
            INNER JOIN Anagrafica ON Verbale.IdAnagrafica = Anagrafica.IdAnagrafica
            WHERE Verbale.Importo > 400", conn);

                SqlDataReader reader = queryVisualizzaViolazioniConImportoElevato.ExecuteReader();

                while (reader.Read())
                {
                    ViolazionePuntiDieciQuattrocento violazioneImportoElevato = new ViolazionePuntiDieciQuattrocento
                    {
                        Importo = Convert.ToDecimal(reader["Importo"]),
                        CognomeTrasgressore = reader["CognomeTrasgressore"].ToString(),
                        NomeTrasgressore = reader["NomeTrasgressore"].ToString(),
                        DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                        DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"])
                    };

                    violazioniImportoElevato.Add(violazioneImportoElevato);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return View(violazioniImportoElevato);
        }

    }
}