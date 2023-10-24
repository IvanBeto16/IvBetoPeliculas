using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Venta
    {
        public List<object> Carrito { get; set; }
        public string titularPago { get; set; }
        public int numeroTarjeta { get; set; }
        public string fechaCaducidad { get; set; }
        public int CVV { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}
