using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using apos_gestor_caja.Domain.Models;

namespace SalesBookApp.Services
{
    public class ExcelService
    {
        public byte[] GenerateSalesBookExcel(List<SalesRecord> salesRecords, DateTime startDate, DateTime endDate, bool isDetailed = false)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Libro de Ventas");

                int maxInvoiceLength = 0;
                foreach (var record in salesRecords)
                {
                    if (!string.IsNullOrEmpty(record.StartInvoice))
                        maxInvoiceLength = Math.Max(maxInvoiceLength, record.StartInvoice.Length);
                    if (!string.IsNullOrEmpty(record.EndInvoice))
                        maxInvoiceLength = Math.Max(maxInvoiceLength, record.EndInvoice.Length);
                }

                worksheet.Cell(1, 1).Value = $"LIBRO DE VENTAS {(isDetailed ? "DETALLADO" : "RESUMIDO")}";
                var titleRange = worksheet.Range(1, 1, 1, 16);
                titleRange.Merge();
                titleRange.Style
                    .Font.SetBold(true)
                    .Font.SetFontSize(16)
                    .Font.SetFontColor(XLColor.White)
                    .Fill.SetBackgroundColor(XLColor.FromArgb(0, 51, 102))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium);

                worksheet.Cell(2, 1).Value = $"Período: {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
                var periodRange = worksheet.Range(2, 1, 2, 16);
                periodRange.Merge();
                periodRange.Style
                    .Font.SetBold(true)
                    .Font.SetFontSize(12)
                    .Font.SetFontColor(XLColor.FromArgb(51, 51, 51))
                    .Fill.SetBackgroundColor(XLColor.FromArgb(230, 230, 230))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                worksheet.Cell(3, 1).Value = "Factura";
                var facturaRange = worksheet.Range(3, 1, 3, 8);
                facturaRange.Merge();
                facturaRange.Style
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.White)
                    .Fill.SetBackgroundColor(XLColor.FromArgb(0, 102, 204))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium);

                worksheet.Cell(3, 9).Value = "Total Ventas";
                var totalVentasRange = worksheet.Range(3, 9, 3, 12);
                totalVentasRange.Merge();
                totalVentasRange.Style
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.White)
                    .Fill.SetBackgroundColor(XLColor.FromArgb(0, 102, 204))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium);

                worksheet.Cell(3, 13).Value = "No Contribuyente";
                var noContribuyente = worksheet.Range(3, 13, 3, 16);
                noContribuyente.Merge();
                noContribuyente.Style
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.White)
                    .Fill.SetBackgroundColor(XLColor.FromArgb(0, 102, 204))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium);

                string[] headers = new string[]
                {
                    "Fecha",
                    "Nro de RIF",
                    "Nombre o Razón Social del Cliente",
                    "Nro Máquina Fiscal",
                    "N Reporte Z",
                    "Tipo Documento",
                    "Factura Inicial",
                    "Factura Final",
                    "Base IGTF",
                    "IGTF 3%",
                    "Total Ventas",
                    "Exentas/Exoneradas no Sujetas",
                    "Base del 8%",
                    "IVA 8%",
                    "Base del 16%",
                    "IVA 16%"
                };

                worksheet.Row(4).Height = 25;
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(4, i + 1);
                    cell.Value = headers[i];
                    cell.Style
                        .Font.SetBold(true)
                        .Font.SetFontColor(XLColor.White)
                        .Fill.SetBackgroundColor(XLColor.FromArgb(0, 102, 204))
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                        .Alignment.SetWrapText(true);
                }

                int row = 5;
                bool alternate = false;
                foreach (var record in salesRecords)
                {
                    worksheet.Row(row).Height = 20;

                    worksheet.Cell(row, 1).Value = record.Date.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 2).Value = record.Rif; // Mostrar Rif tal como en la previsualización
                    worksheet.Cell(row, 3).Value = record.CustomerName; // Mostrar CustomerName tal como en la previsualización
                    worksheet.Cell(row, 4).Value = record.PrinterSerial;
                    worksheet.Cell(row, 5).Value = record.ReportZ;
                    worksheet.Cell(row, 6).Value = record.DocumentType;
                    worksheet.Cell(row, 7).Value = record.StartInvoice.PadLeft(maxInvoiceLength, '0');
                    worksheet.Cell(row, 8).Value = record.EndInvoice.PadLeft(maxInvoiceLength, '0');
                    worksheet.Cell(row, 9).Value = record.IgtfBase;
                    worksheet.Cell(row, 10).Value = record.IgtfAmount;
                    worksheet.Cell(row, 11).Value = record.TotalInvoice;
                    worksheet.Cell(row, 12).Value = record.ExemptAmount;
                    worksheet.Cell(row, 13).Value = record.Base8;
                    worksheet.Cell(row, 14).Value = record.Iva8;
                    worksheet.Cell(row, 15).Value = record.TaxableBase;
                    worksheet.Cell(row, 16).Value = record.Vat16;

                    var rowRange = worksheet.Range(row, 1, row, 16);
                    rowRange.Style
                        .Fill.SetBackgroundColor(alternate ? XLColor.FromArgb(245, 245, 245) : XLColor.White)
                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Border.SetInsideBorder(XLBorderStyleValues.Thin);

                    var currencyColumns = new[] { 9, 10, 11, 12, 13, 14, 15, 16 };
                    foreach (var col in currencyColumns)
                    {
                        worksheet.Cell(row, col).Style
                            .NumberFormat.SetFormat("#,##0.00")
                            .Font.SetFontColor(XLColor.FromArgb(0, 102, 0));
                    }

                    alternate = !alternate;
                    row++;
                }

                worksheet.Row(row).Height = 25;
                worksheet.Cell(row, 8).Value = "TOTALES:";
                worksheet.Cell(row, 8).Style
                    .Font.SetBold(true)
                    .Font.SetFontSize(12)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    .Fill.SetBackgroundColor(XLColor.FromArgb(230, 230, 230));

                var sumColumns = new[] { 9, 10, 11, 12, 13, 14, 15, 16 };
                foreach (int col in sumColumns)
                {
                    var cell = worksheet.Cell(row, col);
                    cell.FormulaA1 = $"SUM({worksheet.Cell(5, col).Address}:{worksheet.Cell(row - 1, col).Address})";
                    cell.Style
                        .Font.SetBold(true)
                        .Font.SetFontColor(XLColor.FromArgb(0, 102, 0))
                        .NumberFormat.SetFormat("#,##0.00")
                        .Fill.SetBackgroundColor(XLColor.FromArgb(230, 230, 230))
                        .Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                }

                worksheet.Column(1).Width = 12;
                worksheet.Column(2).Width = 15;
                worksheet.Column(3).Width = 35;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 15;
                worksheet.Column(7).Width = 15;
                worksheet.Column(8).Width = 15;
                worksheet.Column(9).Width = 15;
                worksheet.Column(10).Width = 15;
                worksheet.Column(11).Width = 15;
                worksheet.Column(12).Width = 20;
                worksheet.Column(13).Width = 15;
                worksheet.Column(14).Width = 15;
                worksheet.Column(15).Width = 15;
                worksheet.Column(16).Width = 15;

                worksheet.SheetView.FreezeRows(4);
                worksheet.Rows().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}