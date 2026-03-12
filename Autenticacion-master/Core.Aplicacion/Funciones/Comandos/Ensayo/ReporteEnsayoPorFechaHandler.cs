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
using Polly;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    /// <summary>
    /// Logica de negocio para generar reporte en formato .pdf con los ensayos realizados en un rango de fechas específico. Esta clase se encarga de manejar la solicitud de generación del reporte, obteniendo los datos necesarios y formateándolos adecuadamente para su presentación en el documento PDF, incluyendo el diseño del encabezado, la tabla de datos y el resumen de totales. El resultado es un archivo PDF que puede ser descargado o visualizado por el usuario, facilitando el análisis y seguimiento de las actividades de ensayo dentro del período seleccionado.
    /// </summary>
    public class ReporteEnsayoPorFechaHandler(IEnsayo repository, ICacheServicio cacheServicio, IHttpContextAccessor context) : IRequestHandler<ReporteEnsayoPorFechaCom, byte[]>
    {
        /// <summary>
        /// Serviico de acceso a datos para ensayos, utilizado para obtener la información necesaria para generar el reporte de ensayos por fecha. Este servicio es esencial para realizar las consultas necesarias para obtener los datos requeridos por la solicitud, y se inyecta a través del constructor para facilitar la separación de responsabilidades y mejorar la testabilidad de la clase.
        /// </summary>
        private readonly IEnsayo _repository = repository;

        /// <summary>
        /// Servicio de cache utilizado para almacenar temporalmente los resultados de las consultas, mejorando el rendimiento y reduciendo la carga en la base de datos al evitar consultas repetitivas para la misma información dentro de un período de tiempo determinado. Este servicio se inyecta a través del constructor para facilitar la separación de responsabilidades y mejorar la testabilidad de la clase.
        /// </summary>
        private readonly ICacheServicio _iCacheServicio = cacheServicio;

        /// <summary>
        /// HttpContextAccessor utilizado para acceder al contexto HTTP actual, lo que permite obtener información relevante de la solicitud, como los encabezados, que pueden ser útiles para la gestión de sesiones o para personalizar la respuesta según el usuario o la sesión. Este servicio se inyecta a través del constructor para facilitar la separación de responsabilidades y mejorar la testabilidad de la clase.
        /// </summary>
        private readonly IHttpContextAccessor context = context;

        /// <summary>
        /// Logica de negocio para manejar la solicitud de generación del reporte de ensayos por fecha. Se obtiene la lista de ensayos realizados dentro del rango de fechas especificado, y se formatea adecuadamente para su presentación en un documento PDF, incluyendo el diseño del encabezado, la tabla de datos y el resumen de totales. El resultado es un archivo PDF que puede ser descargado o visualizado por el usuario, facilitando el análisis y seguimiento de las actividades de ensayo dentro del período seleccionado.
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Reporte en formato byte</returns>
        public async Task<byte[]> Handle(ReporteEnsayoPorFechaCom request, CancellationToken cancellationToken)
        {
            string idSesion = context.HttpContext.Request.Headers["IdSesion"].ToString();
            IEnumerable<ClienteDeudorModel> ensayos =  await _iCacheServicio.Obtener<IEnumerable<ClienteDeudorModel>>($"ConsultarEnsayoFechas_{idSesion}");

            using MemoryStream stream = new();
            PdfWriter writer = new(stream);
            PdfDocument pdf = new(writer);
            Document document = new(pdf, PageSize.A4);
            document.SetMargins(40, 40, 40, 40);

            // =============================
            // FUENTES Y COLORES (Negro Institucional)
            // =============================
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Color negro = ColorConstants.BLACK;

            // =============================
            // ENCABEZADO (Solo LABORATORIO CETVI - Grande y Negro)
            // =============================
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

            // Texto más grande y solo el nombre de la empresa
            Paragraph infoEmpresa = new Paragraph("LABORATORIO CETVI")
                .SetFont(boldFont)
                .SetFontSize(18) // Tamaño aumentado
                .SetFontColor(negro);

            headerTable.AddCell(new Cell().Add(infoEmpresa)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBorder(Border.NO_BORDER));

            document.Add(headerTable);
            document.Add(new Paragraph("\n"));

            // =============================
            // TÍTULO (Sin raya)
            // =============================
            Paragraph titulo = new Paragraph("ENSAYOS CETVI")
                .SetFont(boldFont)
                .SetFontSize(16)
                .SetFontColor(negro)
                .SetTextAlignment(TextAlignment.CENTER);

            document.Add(titulo);

            // =============================
            // FECHAS (A la izquierda, en negro, sin cuadro)
            // =============================
            document.Add(new Paragraph("\n"));
            //document.Add(new Paragraph("Fecha de Inicio: " + request.FechaInicio.ToShortDateString())
            //    .SetFont(boldFont).SetFontSize(10).SetFontColor(negro));
            //document.Add(new Paragraph("Fecha de Fin: " + request.FechaFin.ToShortDateString())
            //    .SetFont(boldFont).SetFontSize(10).SetFontColor(negro));
            document.Add(new Paragraph("Fecha Generación: " + DateTime.Now.ToString("dd/MM/yyyy"))
                .SetFont(normalFont).SetFontSize(9).SetFontColor(negro));
            document.Add(new Paragraph("\n"));

            // =============================
            // TABLA DE DATOS (Fondo Blanco)
            // =============================
            float[] columnas = { 4, 2, 2, 2, 2 };
            Table table = new Table(UnitValue.CreatePercentArray(columnas)).UseAllAvailableWidth();

            Cell HeaderCell(string text) => new Cell()
                .Add(new Paragraph(text).SetFont(boldFont).SetFontSize(10).SetFontColor(negro))
                .SetBackgroundColor(ColorConstants.WHITE) // Fondo blanco solicitado
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(5);

            table.AddHeaderCell(HeaderCell("CLIENTE"));
            table.AddHeaderCell(HeaderCell("FECHA INGRESO"));
            table.AddHeaderCell(HeaderCell("TOTAL"));
            table.AddHeaderCell(HeaderCell("ABONO"));
            table.AddHeaderCell(HeaderCell("SALDO"));

            decimal totalFacturado = 0, totalAbonado = 0, totalPendiente = 0;

            foreach (var item in ensayos)
            {
                table.AddCell(new Cell().Add(new Paragraph(item.NombreCompleto.ToUpper()).SetFont(normalFont).SetFontSize(9)));
                table.AddCell(new Cell().Add(new Paragraph(item.FechaRegistro.ToShortDateString()).SetFont(normalFont).SetFontSize(9)).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Cell().Add(new Paragraph("$ " + item.TotalAPagar.ToString("N2")).SetFont(normalFont).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT));
                table.AddCell(new Cell().Add(new Paragraph("$ " + item.TotalAbonado.ToString("N2")).SetFont(normalFont).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT));
                table.AddCell(new Cell().Add(new Paragraph("$ " + item.SaldoPendiente.ToString("N2")).SetFont(boldFont).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT));

                totalFacturado += item.TotalAPagar;
                totalAbonado += item.TotalAbonado;
                totalPendiente += item.SaldoPendiente;
            }
            document.Add(table);

            // =============================
            // RESUMEN DE TOTALES (En Negro y Negrita)
            // =============================
            document.Add(new Paragraph("\n"));
            Table totalTable = new Table(2).SetWidth(UnitValue.CreatePercentValue(40)).SetHorizontalAlignment(HorizontalAlignment.RIGHT);

            void AddTotalRow(string label, decimal value, PdfFont font)
            {
                totalTable.AddCell(new Cell().Add(new Paragraph(label).SetFont(font).SetFontSize(10).SetFontColor(negro)).SetBorder(Border.NO_BORDER));
                totalTable.AddCell(new Cell().Add(new Paragraph("$ " + value.ToString("N2")).SetFont(font).SetFontSize(10).SetFontColor(negro)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
            }

            AddTotalRow("TOTAL FACTURADO:", totalFacturado, normalFont);
            AddTotalRow("TOTAL ABONADO:", totalAbonado, normalFont);
            // Total Pendiente en negrita y negro
            AddTotalRow("TOTAL PENDIENTE:", totalPendiente, boldFont);
            document.Add(totalTable);

            // =============================
            // FIRMA Y PIE DE PÁGINA
            // =============================
            document.Add(new Paragraph("\n\n\n\n"));
            document.Add(new Paragraph("________________________________________")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(negro));
            document.Add(new Paragraph("VÍCTOR ALFONSO SANTILLÁN RIVERA")
                .SetFont(boldFont)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("Documento generado automáticamente por el sistema - CETVI")
                .SetFontSize(8)
                .SetFontColor(ColorConstants.GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFixedPosition(40, 20, 515));

            document.Close();
            return stream.ToArray();
        }
    }
}
