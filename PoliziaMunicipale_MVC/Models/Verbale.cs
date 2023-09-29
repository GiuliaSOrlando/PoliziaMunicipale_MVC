using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PoliziaMunicipale_MVC
{
    public class Verbale
    {
        public int IdVerbale { get; set; }
        [Display(Name = "Data in cui è stata commessa la violazione")]
        public DateTime DataViolazione { get; set; }
        [Display(Name = "Indirizzo della violazione")]
        public string IndirizzoViolazione {  set; get; }
        [Display(Name = "Nominativo agente")]
        public string NominativoAgente { get; set; }
        [Display(Name = "Data in cui è stata trascritta la violazione")]
        public DateTime DataTrascrizioneVerbale { get; set; }
        public decimal Importo { get; set; }
        [Display(Name = "Punti decurtati")]
        public int DecurtamentoPunti { get; set; }
        [Display(Name = "Nominativo trasgressore")]
        public int IdAnagrafica { get; set; }
        public Anagrafica Anagrafica { get; set; }
        [Display(Name = "Tipo di violazione")]
        public int IdViolazione { get; set; }
        public TipoDiViolazione TipoDiViolazione { get; set; }

    }

}

