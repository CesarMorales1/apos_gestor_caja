using System;

public class SalesRecord
{
    public string CashierNumber { get; set; }
    public string PrinterSerial { get; set; }
    public DateTime Date       { get; set; }
    public string DocumentType { get; set; }
    public string StartInvoice { get; set; }
    public string EndInvoice { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxableBase { get; set; }
    public decimal Vat16 { get; set; }
    public decimal Base8 { get; set; }
    public decimal Iva8 { get; set; }
    public decimal ExemptAmount { get; set; }
    public decimal TotalInvoice { get; set; }
    public decimal IgtfBase { get; set; }
    public decimal IgtfAmount { get; set; }
    public string BusinessName { get; set; }
    public string ReportZ { get; set; }
    public string Rif { get; set; }        // Para el número de RIF (d4)
    public string CustomerName { get; set; } // Para el nombre del cliente (d7)
}