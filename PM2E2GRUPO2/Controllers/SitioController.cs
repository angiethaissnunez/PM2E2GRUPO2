using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PM2E2GRUPO2.Models;

namespace PM2E2GRUPO2.Controllers
{
    public class SitioController
    {

        private static readonly string URL_SITIOS = "https://examenmovilgrupo2.uthmovil.com/apiexamen/";
        private static HttpClient client = new HttpClient();

        public static async Task<List<SitiosFirma>> GetAllSite()
        {
            List<SitiosFirma> listBooks = new List<SitiosFirma>();
            try
            {
                var uri = new Uri(URL_SITIOS + "listasitios.php");
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    listBooks = JsonConvert.DeserializeObject<List<SitiosFirma>>(content);
                    return listBooks;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listBooks;
        }

        public async static Task<bool> DeleteSite(string id)
        {
            try
            {
                var uri = new Uri(URL_SITIOS + "eliminarsitio.php?id=" + id);
                var result = await client.GetAsync(uri);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async static Task<bool> CreateSite(SitiosFirma sitio)
        {
            try
            {
                Uri requestUri = new Uri(URL_SITIOS + "crearsitio.php");
                var jsonObject = JsonConvert.SerializeObject(sitio);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }


        public async static Task<bool> UpdateSitio(SitiosFirma sitio)
        {
            try
            {
                Uri requestUri = new Uri(URL_SITIOS + "actualizarsitio.php");
                var jsonObject = JsonConvert.SerializeObject(sitio);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                //var response = await client.PutAsync(requestUri, content);
                var response = await client.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

    }
}
