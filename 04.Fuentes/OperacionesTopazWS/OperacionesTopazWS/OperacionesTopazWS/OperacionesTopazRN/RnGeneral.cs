using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace OperacionesTopazRN
{
    public class RnGeneral
    {
        public String sConexion { get; set; }

        public RnGeneral()  
        {
            sConexion = ConfigurationManager.ConnectionStrings["conConexion"].ConnectionString;
        }
    }
}
