using Core.DataAccess.Clientes.Interfaz;
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
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ReporteEnsayoClientIngresadoeHandler : IRequestHandler<ReporteEnsayoClienteIngresadoCom, byte[]>
    {
        private readonly IEnsayo _iEnsayo;

        public ReporteEnsayoClientIngresadoeHandler(IEnsayo iEnsayo)
        {
            _iEnsayo = iEnsayo;
        }

        public async Task<byte[]> Handle(ReporteEnsayoClienteIngresadoCom request, CancellationToken cancellationToken)
        {
            var datos = await _iEnsayo.ConsultarPorCedula(request.Cedula, request.FechaInicio, request.FechaFin);

            if (datos == null || !datos.Any()) return null;

            using MemoryStream stream = new();
            PdfWriter writer = new(stream);
            PdfDocument pdf = new(writer);
            Document document = new(pdf, PageSize.A4);
            document.SetMargins(40, 40, 40, 40);

            // Fuentes
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            // --- ENCABEZADO (Logo Izquierda, Nombre Derecha) ---
            Table headerTable = new Table(2).UseAllAvailableWidth();
            string rutaLogo = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cetvi.jpeg");

            if (File.Exists(rutaLogo))
            {
                Image logo = new Image(ImageDataFactory.Create(rutaLogo)).ScaleToFit(85, 85);
                headerTable.AddCell(new Cell().Add(logo).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
            }
            else
            {
                headerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
            }

            headerTable.AddCell(new Cell().Add(new Paragraph("LABORATORIO CETVI")
                .SetFont(boldFont).SetFontSize(18))
                .SetTextAlignment(TextAlignment.RIGHT).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(Border.NO_BORDER));

            document.Add(headerTable);

            // --- TÍTULO Y FECHA ---
            document.Add(new Paragraph("ENSAYOS CETVI").SetFont(boldFont).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(20));
            document.Add(new Paragraph("Fecha Generación: " + DateTime.Now.ToString("dd/MM/yyyy")).SetFont(normalFont).SetFontSize(9).SetMarginTop(10));

            // --- DATOS DEL CLIENTE ---
            var primerItem = datos.First();
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph().Add(new Text("CLIENTE: ").SetFont(boldFont)).Add(new Text(primerItem.NombreCompleto.ToUpper())).SetFontSize(10));
            document.Add(new Paragraph().Add(new Text("CÉDULA: ").SetFont(boldFont)).Add(new Text(primerItem.Cedula)).SetFontSize(10));
            document.Add(new Paragraph("\n"));

            // --- TABLA DE DATOS (Cabecera Blanca como la imagen 2) ---
            float[] columnas = { 6, 4 }; // Proporción de columnas
            Table table = new Table(UnitValue.CreatePercentArray(columnas)).UseAllAvailableWidth();

            Cell HeaderCell(string text) => new Cell()
                .Add(new Paragraph(text).SetFont(boldFont).SetFontSize(10))
                .SetBackgroundColor(ColorConstants.WHITE)
                .SetTextAlignment(TextAlignment.CENTER).SetPadding(5);

            table.AddHeaderCell(HeaderCell("ENSAYO A REALIZAR"));
            table.AddHeaderCell(HeaderCell("ESTADO / OBSERVACIÓN"));

            foreach (var item in datos)
            {
                var subensayos = await _iEnsayo.ObtenerEnsayosDetallados(item.IdEnsayo);
                foreach (var sub in subensayos)
                {
                    table.AddCell(new Cell().Add(new Paragraph(sub.NombreCatalogo?.ToUpper() ?? "ENSAYO").SetFont(normalFont).SetFontSize(9)));
                    table.AddCell(new Cell().Add(new Paragraph("PENDIENTE TÉCNICO").SetFont(normalFont).SetFontSize(9)).SetTextAlignment(TextAlignment.CENTER));
                }
            }
            document.Add(table);

            // --- FIRMA Y PIE ---
            document.Add(new Paragraph("\n\n\n\n"));
            document.Add(new Paragraph("________________________________________").SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph("VÍCTOR ALFONSO SANTILLÁN RIVERA").SetFont(boldFont).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("Documento generado automáticamente por el sistema - CETVI")
                .SetFontSize(8).SetFontColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER)
                .SetFixedPosition(40, 20, 515));

            document.Close();
            return stream.ToArray();
        }
    }
}