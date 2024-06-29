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

namespace CapaNegocio
{
    public class CN_Paypal
    {
        private static string urlpaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private static string clientID = ConfigurationManager.AppSettings["ClientID"];
        private static string secret = ConfigurationManager.AppSettings["Secret"];

        public async Task<Response_Paypal<Response_CheckOut>> crearSolicitud(CheckOut_Order orden)
        {
            Response_Paypal<Response_CheckOut> responsePaypal = new Response_Paypal<Response_CheckOut>();

            using (var client = new HttpClient()) { 

                client.BaseAddress = new Uri(urlpaypal);

                var authToken = Encoding.ASCII.GetBytes($"{clientID} : {secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            
                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                responsePaypal.Status = response.IsSuccessStatusCode;
                if(response.IsSuccessStatusCode)
                {
                    string jsonrespuesta = response.Content.ReadAsStringAsync().Result;

                    Response_CheckOut checkout = JsonConvert.DeserializeObject<Response_CheckOut>(jsonrespuesta);
                    responsePaypal.Response = checkout;

                }
                return responsePaypal;
            }
        }

        public async Task<Response_Paypal<Response_Capture>> AprobaPago(string token)
        {
            Response_Paypal<Response_Capture> responsePaypal = new Response_Paypal<Response_Capture>();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(urlpaypal);

                var authToken = Encoding.ASCII.GetBytes($"{clientID} : {secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                responsePaypal.Status = response.IsSuccessStatusCode;
                if (response.IsSuccessStatusCode)
                {
                    string jsonrespuesta = response.Content.ReadAsStringAsync().Result;

                    Response_Capture capture = JsonConvert.DeserializeObject<Response_Capture>(jsonrespuesta);
                    responsePaypal.Response = capture;

                }
                return responsePaypal;
            }
        }

        public async Task<Response_Paypal<Response_CheckOut>> crearSolicitud(CheckOut_Order.Checkout_Order oCheckoutOrder)
        {
            throw new NotImplementedException();
        }
    }
}
