using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Passenger : BaseEntity
    {
        public Passenger()
        {

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
            MedicalDocumentFile = new MedicalDocument();
            LogBookDocumentFile = new LogBookDocument();
            AttorneyDocumentFile = new AttorneyDocument();
            NationalCardDocumentFile = new NationalCardDocument();
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
        public MedicalDocument? MedicalDocumentFile { get; set; }

        /// <summary>
        /// سند لاگ بوک
        /// </summary>
        public LogBookDocument? LogBookDocumentFile { get; set; }

        /// <summary>
        /// سند وکالت نامه محضری
        /// </summary>
        public AttorneyDocument? AttorneyDocumentFile { get; set; }

        /// <summary>
        /// فایل کارت ملی
        /// </summary>
        public NationalCardDocument? NationalCardDocumentFile { get; set; }

        public Guid? MedicalDocumentFileId { get; set; }
        public Guid? LogBookDocumentFileId { get; set; }
        public Guid? AttorneyDocumentFileId { get; set; }
        public Guid? NationalCardDocumentFileId { get; set; }
    }
}
