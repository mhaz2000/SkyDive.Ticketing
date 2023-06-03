﻿using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class SkyDiveEvent : BaseEntity
    {
        public SkyDiveEvent(int code, string title, string location, bool voidable, bool subjecToVAT, Guid image, DateTime startDate, DateTime endDate,
            SkyDiveEventStatus status) : base()
        {
            Code = code;
            Title = title;
            Images = new List<Guid>() { image };
            StartDate = startDate;
            EndDate = endDate;
            IsActive = false;
            Location = location;
            Status = status;
            Voidable = voidable;
            SubjecToVAT = subjecToVAT;
            TypesAmount = new LinkedList<SkyDiveEventTicketTypeAmount>();

            Items = new List<SkyDiveEventItem>();

            for (int i = 0; i < (startDate - endDate).TotalDays + 1; i++)
            {
                Items.Add(new SkyDiveEventItem(startDate.AddDays(i), 0));
            }
        }

        /// <summary>
        /// کد رویداد
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// عنوان رویداد
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// محل رویداد
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// تصاویر رویداد
        /// </summary>
        public List<Guid> Images { get; set; }

        /// <summary>
        /// تاریخ شروع رویداد
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاریخ پایان رویداد
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// ظرفیت رویداد
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// مشمول مالیات ارزش افزوده
        /// </summary>
        public bool SubjecToVAT { get; set; }

        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// قابل لغو
        /// </summary>
        public bool Voidable { get; set; }

        /// <summary>
        /// قوانین و شرایط
        /// </summary>
        public string? TermsAndConditions { get; set; }

        /// <summary>
        /// وضعیت رویداد
        /// </summary>
        public SkyDiveEventStatus Status { get; set; }

        /// <summary>
        /// به ازای روز های رویداد
        /// </summary>
        public ICollection<SkyDiveEventItem> Items { get; set; }

        /// <summary>
        /// قیمت واحد هر نوع بلیت
        /// </summary>
        public ICollection<SkyDiveEventTicketTypeAmount> TypesAmount { get; set; }


        public void ToggleActivation() => IsActive = !IsActive;
    }

    public class SkyDiveEventItem : BaseEntity
    {
        public SkyDiveEventItem(DateTime date, int capacity) : base()
        {
            Date = date;
            Capacity = capacity;
            FlightLoads = new List<FlightLoad>();
        }

        /// <summary>
        /// تاریخ پرواز
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// تعداد پرواز
        /// </summary>
        public int FlightNumber { get; set; }

        /// <summary>
        /// گنجایش
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// پرواز ها
        /// </summary>
        public ICollection<FlightLoad> FlightLoads { get; set; }
    }

    public class SkyDiveEventTicketTypeAmount : BaseEntity
    {
        public SkyDiveEventTicketTypeAmount(SkyDiveEventTicketType type, double amount) : base()
        {
            Type = type;
            Amount = amount;
        }

        public SkyDiveEventTicketType Type { get; set; }
        public double Amount { get; set; }
    }
}
