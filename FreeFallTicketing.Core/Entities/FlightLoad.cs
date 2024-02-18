using SkyDiveTicketing.Core.Entities.Base;
using System.ComponentModel;

namespace SkyDiveTicketing.Core.Entities
{
    public class FlightLoad : BaseEntity
    {
        public FlightLoad()
        {

        }

        public FlightLoad(int number, DateTime date, int capacity, int voidableNumber)
        {
            //CancellationTypes = new List<FlightLoadCancellationType>();
            FlightLoadItems = new List<FlightLoadItem>();
            Number = number;
            Date = date;
            Capacity = capacity;
            VoidableNumber = voidableNumber;
            Status = FlightStatus.NotDone;
            Name = string.Empty;
        }

        public int Number { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Capacity { get; set; }

        /// <summary>
        /// غیر قابل رزرو
        /// </summary>
        public int VoidableNumber { get; set; }

        public FlightStatus Status { get; set; }


        public ICollection<FlightLoadItem> FlightLoadItems { get; set; }

        //public IList<FlightLoadCancellationType> CancellationTypes { get; set; }

        //public void AddCancellationTypes(FlightLoadCancellationType cancellationType)
        //{
        //    CancellationTypes.Add(cancellationType);
        //}
    }

    public class FlightLoadItem : BaseEntity
    {
        public FlightLoadItem()
        {

        }

        public FlightLoadItem(SkyDiveEventTicketType flightLoadType, int seatNumber) : base()
        {
            FlightLoadType = flightLoadType;
            SeatNumber = seatNumber;
            Tickets = new List<Ticket>();
        }

        public SkyDiveEventTicketType FlightLoadType { get; set; }
        public int SeatNumber { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }

    public enum FlightStatus
    {
        [Description("انجام نشده")]
        NotDone,
        [Description("معلق شده")]
        Suspended,
        [Description("کنسل شده")]
        Canceled,
        [Description("انجام شده")]
        Done
    }

}
