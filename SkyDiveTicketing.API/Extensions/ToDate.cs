using System.Globalization;

namespace SkyDiveTicketing.API.Extensions
{
    public static class ToDate
    {
        public static DateTime ToDateTime(this string value)
        {
            PersianCalendar pc = new PersianCalendar();
            var dateParts = value.Split('/', '-', '\\');
            return new DateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]), pc);
        }
    }
}
