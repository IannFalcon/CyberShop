using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft;

using CapaEntidades.PayPal;
using Newtonsoft.Json;
using System.Diagnostics;
using CapaEntidades;

namespace CapaNegocio
{
    public class CN_Paypal
    {
        private static string urlpaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private static string clientId = ConfigurationManager.AppSettings["ClientID"];
        private static string secret = ConfigurationManager.AppSettings["Secret"];
        private static string endpoint = "/v2/checkout/orders";

        public async Task<ResponsePaypal<ResponseCheckOut>> CrearSolicitud(CheckOutOrder orden)
        {
            ResponsePaypal<ResponseCheckOut> response_paypal = new ResponsePaypal<ResponseCheckOut>();

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(urlpaypal);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                    var json = JsonConvert.SerializeObject(orden);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(endpoint, content);

                    response_paypal.status = response.IsSuccessStatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Respuesta JSON: {jsonResponse}");

                        ResponseCheckOut checkout = JsonConvert.DeserializeObject<ResponseCheckOut>(jsonResponse);
                        response_paypal.response = checkout;
                    }
                    else
                    {
                        Debug.WriteLine($"Error HTTP: {response.StatusCode} - {response.ReasonPhrase}");
                        response_paypal.response = null;
                    }

                }
                catch (Exception ex)
                {
                    response_paypal.status = false;
                    Debug.WriteLine($"Error en la solicitud: {ex.Message}");
                }

                return response_paypal;
            }
        }

        public async Task<ResponsePaypal<ResponseCapture>> AprobarPago(string token)
        {
            ResponsePaypal<ResponseCapture> response_paypal = new ResponsePaypal<ResponseCapture>();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(urlpaypal);

                var authToken = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                response_paypal.status = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string jsonrespuesta = response.Content.ReadAsStringAsync().Result;

                    ResponseCapture capture = JsonConvert.DeserializeObject<ResponseCapture>(jsonrespuesta);
                    response_paypal.response = capture;

                }
                return response_paypal;
            }
        }
    }
}