using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ReporteEnsayoClienteHandler(IEnsayo repository, ICacheServicio cacheServicio, IHttpContextAccessor context) : IRequestHandler<ReporteEnsayoClienteCom, byte[]>
    {
        private readonly IEnsayo _repository = repository;
        private readonly ICacheServicio _iCacheServicio = cacheServicio;
        private readonly IHttpContextAccessor _context = context;

        public async Task<byte[]> Handle(ReporteEnsayoClienteCom request, CancellationToken cancellationToken)
        {
            // 1. Obtención segura del ID de Sesión
            var headers = _context.HttpContext.Request.Headers;
            string idSesion = headers.ContainsKey("IdSesion") ? headers["IdSesion"].ToString() : string.Empty;

            // 2. Recuperación de datos del Cache
            IEnumerable<ClienteDeudorModel> ensayos = await _iCacheServicio.Obtener<IEnumerable<ClienteDeudorModel>>($"ConsultarPorCedula_{idSesion}");

            // --- VALIDACIÓN CRUCIAL PARA EVITAR NULLREFERENCEEXCEPTION ---
            // Si el cache es nulo o está vacío, devolvemos un array vacío para que el Controller maneje el error
            if (ensayos == null || !ensayos.Any())
            {
                return Array.Empty<byte>();
            }

            using MemoryStream stream = new();
            PdfWriter writer = new(stream);
            PdfDocument pdf = new(writer);
            Document document = new(pdf, PageSize.A4);
            document.SetMargins(40, 40, 40, 40);

            // Fuentes y Colores
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Color negro = ColorConstants.BLACK;

            // Encabezado
            Table headerTable = new Table(2).UseAllAvailableWidth();
            string rutaLogo = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot/images/cetvi.jpeg");

            if (System.IO.File.Exists(rutaLogo))
            {
                Image logo = new Image(ImageDataFactory.Create(rutaLogo)).ScaleToFit(85, 85);
                headerTable.AddCell(new Cell().Add(logo).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
            }
            else
            {
                headerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
            }

            Paragraph infoEmpresa = new Paragraph("LABORATORIO CETVI")
                .SetFont(boldFont)
                .SetFontSize(18)
                .SetFontColor(negro);

            headerTable.AddCell(new Cell().Add(infoEmpresa)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBorder(Border.NO_BORDER));

            document.Add(headerTable);
            document.Add(new Paragraph("\n"));

            // Título
            document.Add(new Paragraph("ENSAYOS CETVI")
                .SetFont(boldFont).SetFontSize(16).SetFontColor(negro).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("Fecha Generación: " + DateTime.Now.ToString("dd/MM/yyyy"))
                .SetFont(normalFont).SetFontSize(9).SetFontColor(negro).SetMarginTop(10));

            // Tabla de Datos
            float[] columnas = { 4, 2, 2, 2, 2 };
            Table table = new Table(UnitValue.CreatePercentArray(columnas)).UseAllAvailableWidth().SetMarginTop(10);

            Cell HeaderCell(string text) => new Cell()
                .Add(new Paragraph(text).SetFont(boldFont).SetFontSize(10).SetFontColor(negro))
                .SetBackgroundColor(ColorConstants.WHITE)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(5);

            table.AddHeaderCell(HeaderCell("CLIENTE"));
            table.AddHeaderCell(HeaderCell("FECHA"));
            table.AddHeaderCell(HeaderCell("TOTAL"));
            table.AddHeaderCell(HeaderCell("ABONO"));
            table.AddHeaderCell(HeaderCell("SALDO"));

            decimal totalFacturado = 0, totalAbonado = 0, totalPendiente = 0;

            foreach (var item in ensayos)
            {
                table.AddCell(new Cell().Add(new Paragraph(item.NombreCompleto?.ToUpper() ?? "SIN NOMBRE").SetFont(normalFont).SetFontSize(9)));
                table.AddCell(new Cell().Add(new Paragraph(item.FechaRegistro.ToShortDateString()).SetFont(normalFont).SetFontSize(9)).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Cell().Add(new Paragraph("$ " + item.TotalAPagar.ToString("N2")).SetFont(normalFont).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT));
                table.AddCell(new Cell().Add(new Paragraph("$ " + item.TotalAbonado.ToString("N2")).SetFont(normalFont).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT));
                table.AddCell(new Cell().Add(new Paragraph("$ " + item.SaldoPendiente.ToString("N2")).SetFont(boldFont).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT));

                totalFacturado += item.TotalAPagar;
                totalAbonado += item.TotalAbonado;
                totalPendiente += item.SaldoPendiente;
            }
            document.Add(table);

            // Resumen de Totales
            document.Add(new Paragraph("\n"));
            Table totalTable = new Table(2).SetWidth(UnitValue.CreatePercentValue(40)).SetHorizontalAlignment(HorizontalAlignment.RIGHT);

            void AddTotalRow(string label, decimal value, PdfFont font)
            {
                totalTable.AddCell(new Cell().Add(new Paragraph(label).SetFont(font).SetFontSize(10).SetFontColor(negro)).SetBorder(Border.NO_BORDER));
                totalTable.AddCell(new Cell().Add(new Paragraph("$ " + value.ToString("N2")).SetFont(font).SetFontSize(10).SetFontColor(negro)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
            }

            AddTotalRow("TOTAL FACTURADO:", totalFacturado, normalFont);
            AddTotalRow("TOTAL ABONADO:", totalAbonado, normalFont);
            AddTotalRow("TOTAL PENDIENTE:", totalPendiente, boldFont);
            document.Add(totalTable);

            // Firma
            document.Add(new Paragraph("\n\n\n\n"));
            document.Add(new Paragraph("________________________________________").SetTextAlignment(TextAlignment.CENTER).SetFontColor(negro));
            document.Add(new Paragraph("VÍCTOR ALFONSO SANTILLÁN RIVERA").SetFont(boldFont).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("Documento generado automáticamente por el sistema - CETVI")
                .SetFontSize(8).SetFontColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER)
                .SetFixedPosition(40, 20, 515));

            document.Close();
            return stream.ToArray();
        }
    }
}