using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilitarioCrk;

namespace OperacionesTopazWeb.App_Start
{
    public abstract class InitialPage<T> : WebViewPage<T>
    {
        public static string sRutaMaestra = System.Web.Configuration.WebConfigurationManager.AppSettings["RutaFileServer"].ToString();
        protected override void InitializePage()
        {
            SetViewBagDefaultProperties();
            base.InitializePage();
        }
        private void SetViewBagDefaultProperties()
        {
            ViewBag.vRuta = sRutaMaestra;
            ViewBag.sTitulo = "Operaciones Topaz";
            ViewBag.sNombreCompletoI = UtlAuditoria.ObtenerNombreCompleto();
            ViewBag.sNombreUsuario = UtlAuditoria.ObtenerNombreUsuario();
            ViewBag.iIdUsuario = UtlAuditoria.ObtenerIdUsuario();
            ViewBag.sPrimeroNombre = UtlAuditoria.ObtenerPrimeroNombre();
            ViewBag.sDatosOficina = UtlAuditoria.ObtenerOficina();
            ViewBag.sFecha = DateTime.Now.ToString("dd/MM/yyyy");

            //Obtiene el menú
            List<EnMenu> lstMenu = new List<EnMenu>();
            string sMenu = UtlAuditoria.ObtenerMenu();
            if (!sMenu.Equals(""))
            {
                lstMenu = JsonConvert.DeserializeObject<List<EnMenu>>(sMenu);
            }
            else
            {
                ViewBag.message = "Ocurrió un error al obtener el menú de la Operaciones Topaz";
            }
            var lstMenuP = lstMenu.Where(x => x.idPadre == 0).ToList();
            lstMenu = lstMenu.Where(x => x.idPadre != 0).ToList<EnMenu>();
            ViewBag.lstMenuP = lstMenuP;
            ViewBag.lstMenu = lstMenu;
        }
    }
    public class EnMenu
    {
        public int iidMenu { get; set; }
        public int idPadre { get; set; }
        public string sNombre { get; set; }
        public string sRuta { get; set; }
        public int iAccion { get; set; }

    }
}