﻿namespace SkyDiveTicketing.Application.Helpers
{
    public class PdfHelper
    {
        public static MemoryStream TicketPdf(List<TicketPrintModel> models)
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
                        height: 100%;
                        width: 800px;
                        align-items: center;
                        font-family: 'B Nazanin';
                        margin: auto;
                      "">
                    ";
            
            foreach (var model in models)
            {
                html += $@"<div style=""display: inline-block; border: 4px solid black; padding: 0 50px; text-align: center; width: 250px; margin:50px 20px;"">
                        <h2>{model.Name}</h2>
                        <h2>نوع بلیت: {model.TicketType}</h2>
                        <h2>{model.TicketNumber}</h2>
                        <p style=""font-size: 1.3rem"">
                          <strong>محل رویداد:</strong>
                          <span>{model.Location}</span>
                        </p>
                        <div style=""display: flex; justify-content: space-around;"">
                            <p style=""font-size: 1.3rem""><strong>تاریخ: </strong>{model.Date}</p>
                            <p style=""font-size: 1.3rem""><strong>شماره پرواز: </strong>{model.FlightNumber}</p>
                        </div>
                        <p style=""font-size: 1.3rem"">
                          <strong>کد ملی:</strong>
                          <span>{model.NationalCode}</span>
                        </p>
                      </div>
                    ";

            }

            html += @"</div>
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
                      <div style=""border: 6px solid black; padding: 0 50px; text-align: center; width: 650px;"">
                        <h2>صورت حساب فروش</h2>
                        <div style=""width: 100%;position: relative;"">
                          <p style=""position: absolute; right: 0;"">فروشنده: باشگاه سقوط آزاد ایرانیان</p>
                          <p style=""position: absolute; left: 0;width: 35%;"">تاریخ: {date}</p>
                        </div>
                        <br>
                        <div style=""width: 100%;position: relative;"">
                          <p style=""position: absolute; right: 0;"">شماره: {invoiceNumber}</p>
                          <p style=""position: absolute; left: 0;width: 35%;"">خریدار: {buyer}</p>
                        </div>
                        <br>
                        <p style=""text-align: end;"">کد ملی: {nationalCode}</p>
                        <p style=""text-align: end;"">شماره موبایل: {phone}</p>
                        <table style=""width: 100%;"">
                          <tr>
                            <th style=""width: 35%;"">مبلغ ریال</th>
                            <th style=""width: 65%;"">شرح</th>
                          </tr>
                          <tr>
                            <td>{ticketAmount:n0}</td>
                            <td>خرید بلیت شماره: {ticketNumber}</td>
                          </tr>
                        </table>
                        <div style=""position: relative; margin-bottom: 10px;"">
                          <table style=""width: 35%; margin-top: 4px;"">
                            <tr><td>{vat:n0}</td></tr>
                            <tr><td>{amount:n0}</td></tr>
                          </table>
                          <div style=""margin-left: 10px;"">
                            <p style=""position: absolute; top: 0; left: 38%;"">ارزش افزوده</p>
                            <p style=""position: absolute; top: 27px; left: 38%;"">قابل پرداخت</p>
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

    public class TicketPrintModel
    {
        public TicketPrintModel(string name, string ticketType, string ticketNumber, string location, string date, string flightNumber, string nationalCode)
        {
            Name = name;
            TicketType = ticketType;
            TicketNumber = ticketNumber;
            Location = location;
            Date = date;
            FlightNumber = flightNumber;
            NationalCode = nationalCode;
        }

        public string Name { get; set; }
        public string TicketType { get; set; }
        public string TicketNumber { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string FlightNumber { get; set; }
        public string NationalCode { get; set; }
    }
}
