using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WebClimaChile
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            listCiudad.DataSource = Enum.GetValues(typeof(Biblioteca.NombreCiudadesDisponibles));
            listCiudad.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            int indexNameCiudad = listCiudad.SelectedIndex;

            if (indexNameCiudad == 0)
            {
                lblMensaje.Text = "Seleccione una localidad!!";
            }
            else
            {
                lblMensaje.Text = "";
                Biblioteca.CodigoCiudadesDisponibles code = (Biblioteca.CodigoCiudadesDisponibles)indexNameCiudad - 1;
                BuscarJson(code.ToString());
            }

        }


        private void BuscarJson(string codigoCiudad)
        {
            Biblioteca.Root root;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://api.gael.cloud/general/public/clima/" + codigoCiudad);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                root = JsonConvert.DeserializeObject<Biblioteca.Root>(json);
                txtTemperatura.Text = root.Temp;
                txtEstacion.Text = root.Estacion;
                txtEstado.Text = root.Estado;
                txtHora.Text = root.HoraUpdate;
                txtHumedad.Text = root.Humedad;
                imgClima.ImageUrl = root.Icono;
            }
        }
    }
}