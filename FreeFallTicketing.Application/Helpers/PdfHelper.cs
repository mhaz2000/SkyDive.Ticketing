namespace SkyDiveTicketing.Application.Helpers
{
    public class PdfHelper
    {
        public static MemoryStream TicketPdf(string name, string ticketType, string ticketNumber, string location, string date, string flightNumber, string nationalCode)
        {
            var html = $@"
                <html lang=""en"">
                  <head>
                    <meta charset=""UTF-8"" />
                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                    <title>Document</title>
                  </head>
                  <body>
                    <div
                      style=""
                        display: flex;
                        justify-content: center;
                        height: 100%;
                        align-items: center;
                        font-family: 'B Nazanin';
                      "">
                      <div style=""border: 4px solid black; padding: 0 50px; text-align: center; width: 300px;"">
                        <h2>{name}</h2>
                        <h2>نوع بلیت: {ticketType}</h2>
                        <h2>{ticketNumber}</h2>
                        <p style=""font-size: 1.3rem"">
                          <strong>محل رویداد:</strong>
                          <span>{location}</span>
                        </p>
                        <div style=""display: flex; justify-content: space-around;"">
                            <p style=""font-size: 1.3rem""><strong>تاریخ: </strong>{date}</p>
                            <p style=""font-size: 1.3rem""><strong>شماره پرواز: </strong>{flightNumber}</p>
                        </div>
                        <p style=""font-size: 1.3rem"">
                          <strong>کد ملی:</strong>
                          <span>{nationalCode}</span>
                        </p>
                      </div>
                    </div>
                  </body>
                </html>";

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(html);

            return new MemoryStream(pdfBytes);
        }

        public static MemoryStream InvoicePdf(string date, int invoiceNumber, string buyer, string nationalCode, string phone, double ticketAmount, string ticketNumber, double vat, double amount)
        {
            var html = $@"
                <html lang=""en"">
                  <head>
                    <meta charset=""UTF-8"" />
                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                    <title>Document</title>
                    <style>
                      p{{
                        text-align: end;
                        margin-top: 7px;
                        margin-bottom: 7px;
                      }}
                
                      table, th, td {{
                        border: 1px solid;
                        text-align: center;
                        font-weight: inherit;
                      }}
                
                    </style>
                  </head>
                  <body>
                    <div
                      style=""
                        display: flex;
                        justify-content: center;
                        height: 100%;
                        align-items: center;
                        font-family: 'B Nazanin';
                        font-size: large;
                        font-weight: 600;
                      ""
                    >
                      <div style=""border: 6px solid black; padding: 0 50px; text-align: center; width: 500px;"">
                        <h2>صورت حساب فروش</h2>
                        <div style=""display: flex; justify-content: space-between; width: 100%;"">
                          <p style=""width: 25%;"">تاریخ: {date}</p>
                          <p>فروشنده: شرکت پرواز های تفریحی</p>
                        </div>
                        <div style=""display: flex; justify-content: space-between;"">
                          <p style=""width: 25%;"">شماره: {invoiceNumber}</p>
                          <p>خریدار: {buyer}</p>
                        </div>
                        <p style=""text-align: end;"">کد ملی: {nationalCode}</p>
                        <p style=""text-align: end;"">شماره موبایل: {phone}</p>
                        <table style=""width: 100%;"">
                          <tr>
                            <th style=""width: 30%;"">مبلغ ریال</th>
                            <th style=""width: 70%;"">شرح</th>
                          </tr>
                          <tr>
                            <td>{ticketAmount:n0}</td>
                            <td>خرید بلیت شماره: {ticketNumber}</td>
                          </tr>
                        </table>
                        <div style=""display: flex; margin-bottom: 10px;"">
                          <table style=""width: 30%; margin-top: 4px;"">
                            <tr><td>{vat:n0}</td></tr>
                            <tr><td>{amount:n0}</td></tr>
                          </table>
                          <div style=""margin-left: 10px;"">
                            <p>ارزش افزوده</p>
                            <p>قابل پرداخت</p>
                          </div>
                        </div>
                    </div>
                  </body>
                </html>";

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(html);

            return new MemoryStream(pdfBytes);
        }
    }
}
