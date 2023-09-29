using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliziaMunicipale_MVC
{
    public class Anagrafica
    {
        public int IdAnagrafica { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Indirizzo { get; set; }
        public string Citta { get; set; }
        public string CAP {  get; set; }
        public string CF { get; set; }
        public static List<Anagrafica> GetDataFromDB(string connectionString)
        {
            List<Anagrafica> listaanagrafica = new List<Anagrafica>();

            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                {
                    conn.Open();
                    SqlCommand queryVisualizzaAnagrafica = new SqlCommand("SELECT * FROM Anagrafica", conn);
                    SqlDataReader readerAnagrafica = queryVisualizzaAnagrafica.ExecuteReader();
                    while (readerAnagrafica.Read())
                    {
                        Anagrafica anagrafica = new Anagrafica
                        {
                            IdAnagrafica = Convert.ToInt32(readerAnagrafica["IdAnagrafica"]),
                            Cognome = readerAnagrafica["Cognome"].ToString(),
                            Nome = readerAnagrafica["Nome"].ToString(),
                            Indirizzo = readerAnagrafica["Indirizzo"].ToString(),
                            Citta = readerAnagrafica["Citta"].ToString(),
                            CAP = readerAnagrafica["CAP"].ToString(),
                            CF = readerAnagrafica["Cod_Fisc"].ToString()
                        };

                        listaanagrafica.Add(anagrafica);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return listaanagrafica;
        }

        public static List<SelectListItem> ToSelectItems(List<Anagrafica> listaanagrafica)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (var anagrafica in listaanagrafica)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = $"{anagrafica.Cognome}, {anagrafica.Nome}",
                    Value = anagrafica.IdAnagrafica.ToString()
                });
            }

            return selectListItems;
        }
    }
}
