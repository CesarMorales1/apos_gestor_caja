using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MyApp.Infrastructure.Database;
using System.Data;

namespace SalesBookApp.Repositories
{
    public class SalesRepository
    {
        private readonly SqlHelper _sqlHelper;

        public SalesRepository()
        {
            _sqlHelper = new SqlHelper();
        }

        public async Task<List<SalesRecord>> GetSalesRecordsAsync(DateTime startDate, DateTime endDate, bool isDetailed = false, int? cashierStart = null, int? cashierEnd = null)
        {
            var salesRecords = new List<SalesRecord>();
            string query;
            string cashierFilter = "";

            // Construir filtro de cajas
            if (cashierStart.HasValue && cashierEnd.HasValue)
            {
                if (cashierStart == cashierEnd)
                {
                    cashierFilter = $" AND d1 = '{cashierStart}'";
                }
                else
                {
                    cashierFilter = $" AND CAST(d1 AS SIGNED) BETWEEN {cashierStart} AND {cashierEnd}";
                }
            }
            else if (cashierStart.HasValue)
            {
                cashierFilter = $" AND CAST(d1 AS SIGNED) >= {cashierStart}";
            }
            else if (cashierEnd.HasValue)
            {
                cashierFilter = $" AND CAST(d1 AS SIGNED) <= {cashierEnd}";
            }

            if (isDetailed)
            {
                // Reporte detallado: RIF detallados, cédulas y NC resumidos
                query = @"
                    -- Parte 1: Registros detallados para RIF (dos guiones en d4)
                    SELECT 
                        d1 as CashierNumber,
                        d30 as PrinterSerial,
                        STR_TO_DATE(d33, '%d/%m/%Y') as SaleDate,
                        CASE 
                            WHEN CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2)) >= 0 THEN 'Factura'
                            ELSE 'NC'
                        END as DocumentType,
                        d29 as StartInvoice,
                        d29 as EndInvoice,
                        d4 as Rif,
                        TRIM(d7) as CustomerName,
                        CAST(REPLACE(d11, ',', '.') AS DECIMAL(10,2)) as Subtotal,
                        CAST(REPLACE(d16, ',', '.') AS DECIMAL(10,2)) as TaxableBase,
                        CAST(REPLACE(d17, ',', '.') AS DECIMAL(10,2)) as Vat16,
                        CAST(REPLACE(d18, ',', '.') AS DECIMAL(10,2)) as Base8,
                        CAST(REPLACE(d19, ',', '.') AS DECIMAL(10,2)) as Iva8,
                        CAST(REPLACE(d15, ',', '.') AS DECIMAL(10,2)) as ExemptAmount,
                        CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2)) as TotalInvoice,
                        CAST(REPLACE(d13, ',', '.') AS DECIMAL(10,2)) as IgtfAmount
                    FROM apos08 
                    WHERE STR_TO_DATE(d33, '%d/%m/%Y') BETWEEN @startDate AND @endDate
                    AND SUBSTRING(d4, LOCATE('-', d4) + 1) LIKE '%-%' " + cashierFilter + @"

                    UNION ALL

                    -- Parte 2: Registros resumidos para cédulas (un guion) y notas de crédito
                    SELECT 
                        d1 as CashierNumber,
                        d30 as PrinterSerial,
                        STR_TO_DATE(d33, '%d/%m/%Y') as SaleDate,
                        CASE 
                            WHEN CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2)) >= 0 THEN 'Factura'
                            ELSE 'NC'
                        END as DocumentType,
                        MIN(d29) as StartInvoice,
                        MAX(d29) as EndInvoice,
                        '' as Rif,
                        'Resumen de Ventas Diarias' as CustomerName,
                        SUM(CAST(REPLACE(d11, ',', '.') AS DECIMAL(10,2))) as Subtotal,
                        SUM(CAST(REPLACE(d16, ',', '.') AS DECIMAL(10,2))) as TaxableBase,
                        SUM(CAST(REPLACE(d17, ',', '.') AS DECIMAL(10,2))) as Vat16,
                        SUM(CAST(REPLACE(d18, ',', '.') AS DECIMAL(10,2))) as Base8,
                        SUM(CAST(REPLACE(d19, ',', '.') AS DECIMAL(10,2))) as Iva8,
                        SUM(CAST(REPLACE(d15, ',', '.') AS DECIMAL(10,2))) as ExemptAmount,
                        SUM(CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2))) as TotalInvoice,
                        SUM(CAST(REPLACE(d13, ',', '.') AS DECIMAL(10,2))) as IgtfAmount
                    FROM apos08 
                    WHERE STR_TO_DATE(d33, '%d/%m/%Y') BETWEEN @startDate AND @endDate
                    AND (SUBSTRING(d4, LOCATE('-', d4) + 1) NOT LIKE '%-%' OR CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2)) < 0) " +
                    cashierFilter + @"
                    GROUP BY 
                        DATE(STR_TO_DATE(d33, '%d/%m/%Y')), 
                        d30, d1,
                        CASE 
                            WHEN CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2)) >= 0 THEN 'Factura'
                            ELSE 'NC'
                        END
                    ORDER BY SaleDate, PrinterSerial, DocumentType";
            }
            else
            {
                // Reporte resumido: lógica original con filtro de cajas
                query = @"
                    SELECT 
                        d1 as CashierNumber,
                        d30 as PrinterSerial,
                        STR_TO_DATE(d27, '%d/%m/%Y') as SaleDate,
                        CASE 
                            WHEN CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2)) >= 0 THEN 'Factura'
                            ELSE 'NC'
                        END as DocumentType,
                        MIN(d29) as StartInvoice,
                        MAX(d29) as EndInvoice,
                        SUM(CAST(REPLACE(d11, ',', '.') AS DECIMAL(10,2))) as Subtotal,
                        SUM(CAST(REPLACE(d16, ',', '.') AS DECIMAL(10,2))) as TaxableBase,
                        SUM(CAST(REPLACE(d17, ',', '.') AS DECIMAL(10,2))) as Vat16,
                        SUM(CAST(REPLACE(d18, ',', '.') AS DECIMAL(10,2))) as Base8,
                        SUM(CAST(REPLACE(d19, ',', '.') AS DECIMAL(10,2))) as Iva8,
                        SUM(CAST(REPLACE(d15, ',', '.') AS DECIMAL(10,2))) as ExemptAmount,
                        SUM(CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2))) as TotalInvoice,
                        SUM(CAST(REPLACE(d13, ',', '.') AS DECIMAL(10,2))) as IgtfAmount
                    FROM apos08 
                    WHERE STR_TO_DATE(d33, '%d/%m/%Y') BETWEEN @startDate AND @endDate " +
                    cashierFilter + @"
                    GROUP BY 
                        DATE(STR_TO_DATE(d27, '%d/%m/%Y')), 
                        d30, d1,
                        CASE 
                            WHEN CAST(REPLACE(d24, ',', '.') AS DECIMAL(10,2)) >= 0 THEN 'Factura'
                            ELSE 'NC'
                        END
                    ORDER BY SaleDate, PrinterSerial, DocumentType";
            }

            try
            {
                using (var connection = _sqlHelper.ObtenerConexion())
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startDate.Date);
                    command.Parameters.AddWithValue("@endDate", endDate.Date);

                    if (connection.State != ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var igtfAmount = Convert.ToDecimal(reader["IgtfAmount"] is DBNull ? 0 : reader["IgtfAmount"]);
                            var igtfBase = igtfAmount == 0 ? 0 : Math.Round(igtfAmount * 100 / 3, 2);

                            salesRecords.Add(new SalesRecord
                            {
                                CashierNumber = reader["CashierNumber"]?.ToString() ?? "",
                                PrinterSerial = reader["PrinterSerial"]?.ToString() ?? "",
                                Date = Convert.ToDateTime(reader["SaleDate"]),
                                DocumentType = reader["DocumentType"]?.ToString() ?? "",
                                StartInvoice = reader["StartInvoice"]?.ToString() ?? "",
                                EndInvoice = reader["EndInvoice"]?.ToString() ?? "",
                                Subtotal = Convert.ToDecimal(reader["Subtotal"] is DBNull ? 0 : reader["Subtotal"]),
                                TaxableBase = Convert.ToDecimal(reader["TaxableBase"] is DBNull ? 0 : reader["TaxableBase"]),
                                Vat16 = Convert.ToDecimal(reader["Vat16"] is DBNull ? 0 : reader["Vat16"]),
                                Base8 = Convert.ToDecimal(reader["Base8"] is DBNull ? 0 : reader["Base8"]),
                                Iva8 = Convert.ToDecimal(reader["Iva8"] is DBNull ? 0 : reader["Iva8"]),
                                ExemptAmount = Convert.ToDecimal(reader["ExemptAmount"] is DBNull ? 0 : reader["ExemptAmount"]),
                                TotalInvoice = Convert.ToDecimal(reader["TotalInvoice"] is DBNull ? 0 : reader["TotalInvoice"]),
                                IgtfBase = igtfBase,
                                IgtfAmount = igtfAmount,
                                BusinessName = isDetailed && reader["Rif"]?.ToString().Contains("-") == true && reader["Rif"]?.ToString().Substring(reader["Rif"].ToString().IndexOf('-') + 1).Contains("-") == true
                                    ? reader["CustomerName"]?.ToString() ?? ""
                                    : "Resumen de Ventas Diarias",
                                ReportZ = string.Empty,
                                Rif = isDetailed ? reader["Rif"]?.ToString() ?? "" : "",
                                CustomerName = isDetailed ? reader["CustomerName"]?.ToString() ?? "Resumen de Ventas Diarias" : "Resumen de Ventas Diarias"
                            });
                        }
                    }
                }
                return salesRecords;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving sales records: {ex.Message}", ex);
            }
        }
    }
}
