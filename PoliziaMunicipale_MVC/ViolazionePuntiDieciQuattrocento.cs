using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoliziaMunicipale_MVC
{
    public class ViolazionePuntiDieciQuattrocento
    {
        public decimal Importo { get; set; }
        public string CognomeTrasgressore { get; set; }
        public string NomeTrasgressore { get; set; }
        public DateTime DataViolazione { get; set; }
        
        public int DecurtamentoPunti { get; set; }
    }
}