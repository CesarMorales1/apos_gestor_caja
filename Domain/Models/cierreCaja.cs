using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apos_gestor_caja.Domain.Models
{
    public class CierreCaja
    {//el archivo se encuentra en este orden a diferencia de la base de datos donde cada uno representa d1 para caja, d2 credito, d3 cajero ...

        public int Caja { get; set; }
        public int Credito { get; set; } //si es 1 es normal si es dos es credito
        public int Cajero { get; set; } //numero del cajero en la db
        public int Emisor { get; set; } // depende del tipo de pago si es tarjeta de credito o debito es un banco si no cesta tickect numero 75 es dolares y de lo contrario paypal o credito es 0

        public string Cedula { get; set; } //cedula del cliente
        public string CajaRespaldo { get; set; } //caja de la que se esta haciendo el cierre
        public int NroFactura { get; set; } //numero de factura
        public int FormaPago { get; set; } //forma de pago

        public double cantidad { get; set; } //cantidad de bauches 1 si se recibio 0 si no se recibio

        public double Monto { get; set; } //monto de la factura o tasa en dado caso que el emisor sea 75 da la tasa del dia para hacer la conversion a moneda local

        public int Referencia { get; set; } //referencia de la factura si es pago en tarjeta
        public string Fecha { get; set; } //fecha de la factura en formato dd/mm/yyyy
        public string Hora { get; set; } //hora de la factura

    }
}
