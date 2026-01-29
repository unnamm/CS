using DinkToPdf;

namespace HtmlToPdf
{
    public class Process
    {
        public async Task Run()
        {
            if (!File.Exists("libwkhtmltox.dll"))
            {
                throw new Exception("libwkhtmltox.dll 없음");
            }

            var textData = await File.ReadAllTextAsync(@"D:\Hanwha\Report\26_01_16_162607_Report.html");

            var converter = new SynchronizedConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Out = @"D:\Hanwha\Report\26_01_16_162607_Report.pdf"
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = textData,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            converter.Convert(doc);
        }
    }
}
