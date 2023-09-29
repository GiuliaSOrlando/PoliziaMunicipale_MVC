using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoliziaMunicipale_MVC
{
    public class TrasgressoreVerbale
    {
        public int IdAnagrafica { get; set; }
        public string NomeTrasgressore { get; set; }
        public string CognomeTrasgressore { get; set; }
        public int TotaleVerbaliTrascritti { get; set; }
    }
}