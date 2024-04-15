using ClosedXML.Excel;
using System.Reflection;

namespace SkyDiveTicketing.Application.Helpers
{
    internal class ExcelHelper
    {
        public static MemoryStream GenerateExcel<T>(IEnumerable<T> data, Dictionary<string, string> headers) where T : class
        {
            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet worksheet = wb.Worksheets.Add("گزارش بلیت ها");

            int colCounter = 0;
            foreach (var header in headers)
            {
                int rowCounter = 1;
                colCounter++;

                worksheet.Cell(1, colCounter).Value = header.Value;
                worksheet.Cell(1, colCounter).Style.Fill.BackgroundColor = XLColor.BlueGray;
                worksheet.Cell(1, colCounter).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                var colData = data.Select(s => typeof(T).GetProperty(header.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)!.GetValue(s));
                foreach (var col in colData)
                {
                    rowCounter++;
                    worksheet.Cell(rowCounter, colCounter).Value = col?.ToString() ?? string.Empty;
                    if (rowCounter % 2 == 1)
                        worksheet.Cell(rowCounter, colCounter).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(rowCounter, colCounter).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
            }

            worksheet.RightToLeft = true;
            worksheet.Cells().Style.Font.SetFontName("B Nazanin");
            worksheet.Cells().Style.Font.SetFontCharSet(XLFontCharSet.Arabic);
            worksheet.Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Rows("1").Style.Font.FontSize = 16;
            worksheet.Columns().Width = 30;

            var stream = FetchStream(wb);
            return stream;
        }

        private static MemoryStream FetchStream(IXLWorkbook excelWorkbook)
        {
            var memoryStream = new MemoryStream();
            excelWorkbook.SaveAs(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
