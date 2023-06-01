using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Passenger : BaseEntity
    {
        public Passenger(string nationalCode, DefaultCity? city, string address, float? height, float? weight, string emergencyContact, string emergencyPhone) : base()
        {
            City = city;
            Height = height;
            Weight = weight;
            Address = address;
            NationalCode = nationalCode;
            EmergencyPhone = emergencyPhone;
            EmergencyContact = emergencyContact;
            CustomFields = new List<UserCustomField>();
            MedicalDocumentFile = new PassengerDocument();
            LogBookDocumentFile = new PassengerDocument();
            AttorneyDocumentFile = new PassengerDocument();
            NationalCardDocumentFile = new PassengerDocument();
        }


        /// <summary>
        /// کد ملی
        /// </summary>
        public string NationalCode { get; set; }

        /// <summary>
        /// شهر
        /// </summary>
        public DefaultCity? City { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// قد
        /// </summary>
        public float? Height { get; set; }

        /// <summary>
        /// وزن
        /// </summary>
        public float? Weight { get; set; }

        /// <summary>
        /// نام شخص برای تماس اضطراری
        /// </summary>
        public string EmergencyContact { get; set; }

        /// <summary>
        /// شماره تماس اضطراری
        /// </summary>
        public string EmergencyPhone { get; set; }

        /// <summary>
        /// سایر اطلاعات مربوط به کاربران
        /// </summary>
        public IEnumerable<UserCustomField> CustomFields { get; set; }

        /// <summary>
        /// فایل سند پزشکی
        /// </summary>
        public PassengerDocument MedicalDocumentFile { get; set; }

        /// <summary>
        /// سند لاگ بوک
        /// </summary>
        public PassengerDocument LogBookDocumentFile { get; set; }

        /// <summary>
        /// سند وکالت نامه محضری
        /// </summary>
        public PassengerDocument AttorneyDocumentFile { get; set; }

        /// <summary>
        /// فایل کارت ملی
        /// </summary>
        public PassengerDocument NationalCardDocumentFile { get; set; }
    }
}
