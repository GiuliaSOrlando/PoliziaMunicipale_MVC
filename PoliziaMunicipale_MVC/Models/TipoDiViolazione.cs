using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliziaMunicipale_MVC
{
    public class TipoDiViolazione
    {
        public int IdViolazione { get; set; }
        public string Descrizione { get; set; }

        public static List<TipoDiViolazione> GetDataFromDB(string connectionString)
        {
            List<TipoDiViolazione> listaviolazioni = new List<TipoDiViolazione>();

            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                {
                    conn.Open();
                    SqlCommand queryVisualizzaViolazioni = new SqlCommand("SELECT * FROM Tipo_Violazione", conn);
                    SqlDataReader readerViolazioni = queryVisualizzaViolazioni.ExecuteReader();
                    while (readerViolazioni.Read())
                    {
                        TipoDiViolazione violazione = new TipoDiViolazione
                        {
                            IdViolazione = Convert.ToInt32(readerViolazioni["IDViolazione"]),
                            Descrizione = readerViolazioni["Descrizione"].ToString()
                        };

                        listaviolazioni.Add(violazione);
                    }
                }
            }
            catch
            {

            }

            return listaviolazioni;
        }
        public static List<SelectListItem> ToSelectList(List<TipoDiViolazione> tipoDiViolazioni)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var tipoDiViolazione in tipoDiViolazioni)
            {
                selectList.Add(new SelectListItem
                {
                    Text = tipoDiViolazione.Descrizione,
                    Value = tipoDiViolazione.IdViolazione.ToString()
                });
            }

            return selectList;
        }
    }
}