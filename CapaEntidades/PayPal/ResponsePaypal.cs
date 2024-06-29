using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades.PayPal
{
    public class ResponsePaypal<T>
    {
        public bool status { get; set; }
        public T response { get; set; }
    }
}