using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Passenger : BaseEntity
    {
        public Passenger()
        {
            NationalCode = string.Empty;
        }

        public Passenger(string nationalCode, string? cityAndState, string address, float? height, float? weight, string emergencyContact, string emergencyPhone)
            : base()
        {
            CityAndState = cityAndState;
            Height = height;
            Weight = weight;
            Address = address;
            NationalCode = nationalCode;
            EmergencyPhone = emergencyPhone;
            EmergencyContact = emergencyContact;
            CustomFields = new List<UserCustomField>();
            MedicalDocumentFiles = new List<MedicalDocument>();
            LogBookDocumentFiles = new List<LogBookDocument>();
            AttorneyDocumentFiles = new List<AttorneyDocument>();
            NationalCardDocumentFiles = new List<NationalCardDocument>();
        }


        /// <summary>
        /// کد ملی
        /// </summary>
        public string NationalCode { get; set; }

        /// <summary>
        /// شهر
        /// </summary>
        public string? CityAndState { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        public string? Address { get; set; }

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
        public string? EmergencyContact { get; set; }

        /// <summary>
        /// شماره تماس اضطراری
        /// </summary>
        public string? EmergencyPhone { get; set; }

        /// <summary>
        /// سایر اطلاعات مربوط به کاربران
        /// </summary>
        public IEnumerable<UserCustomField> CustomFields { get; set; }

        /// <summary>
        /// فایل سند پزشکی
        /// </summary>
        public ICollection<MedicalDocument> MedicalDocumentFiles { get; set; }

        /// <summary>
        /// سند لاگ بوک
        /// </summary>
        public ICollection<LogBookDocument> LogBookDocumentFiles { get; set; }

        /// <summary>
        /// سند وکالت نامه محضری
        /// </summary>
        public ICollection<AttorneyDocument> AttorneyDocumentFiles { get; set; }

        /// <summary>
        /// فایل کارت ملی
        /// </summary>
        public ICollection<NationalCardDocument> NationalCardDocumentFiles { get; set; }
    }
}
