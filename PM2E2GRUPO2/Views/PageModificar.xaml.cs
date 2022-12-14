using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM2E2GRUPO2.Models;
using PM2E2GRUPO2.Controllers;
using Nancy.Json;

using SignaturePad.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace PM2E2GRUPO2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageModificar : ContentPage
    {
        SitiosFirma _sitio;
        public PageModificar(SitiosFirma sitio)
        {
            InitializeComponent();

            _sitio = sitio;
            latitud.Text = sitio.Latitud;
            longitud.Text = sitio.Longitud;
            descripcion.Text = sitio.Descripcion;
        }

        private void limpiardescripcion_Clicked(object sender, EventArgs e)
        {
            descripcion.Text = null;
        }

        private async void btnactualizar_Clicked(object sender, EventArgs e)
        {
            bool flag1 = false;

            if (descripcion.Text == null || descripcion.Text == "")
            {
                flag1 = true;
                await DisplayAlert("Aviso", "Se necesita una breve descripción de la ubicación.", "OK");
            }

            if (!flag1)
            {
                byte[] ImageBytes = null;
                var firma = PadView.Strokes;
                

                //obtenemos la firma
                try
                {
                    var image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);

                    //Pasamos la firma a imagen a base 64
                    var mStream = (MemoryStream)image;
                    byte[] data = mStream.ToArray();
                    string base64Val = Convert.ToBase64String(data);
                    ImageBytes = Convert.FromBase64String(base64Val);
                }
                catch (Exception error)
                {
                    await DisplayAlert("Aviso", "No has ingresado tu firma", "OK");
                    return;
                }


                try
                {
                    var serializer = new JavaScriptSerializer();
                    var trazado = serializer.Serialize(firma);

                    SitiosFirma sitio = new SitiosFirma
                    {
                        Id = _sitio.Id,
                        Descripcion = descripcion.Text,
                        Latitud = latitud.Text,
                        Longitud = longitud.Text,
                        FirmaDigital = ImageBytes,
                        firma = trazado
                    };

                    await SitioController.UpdateSitio(sitio);
                    await DisplayAlert("Aviso", "Sitio modificado con éxito", "Aceptar");
                    await Navigation.PopAsync();
                }
                catch (Exception error)
                {
                    await DisplayAlert("Aviso", "" + error, "OK");
                }


            }
        }

        private async void btneliminar_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Aviso", "¿Seguro que desea eliminar el Registro?", "Confirmar", "Volver");
            if (answer)
            {
                await SitioController.DeleteSite("" + _sitio.Id);
                await Navigation.PopAsync();
            }

        }
    }
}