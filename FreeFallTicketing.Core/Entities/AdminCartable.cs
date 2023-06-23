using SkyDiveTicketing.Core.Entities.Base;
using System.ComponentModel;

namespace SkyDiveTicketing.Core.Entities
{
    public class AdminCartable : BaseEntity
    {
        public AdminCartable()
        {

        }

        public AdminCartable(string title, User applicant, RequestType type) : base()
        {
            Title = title;
            Applicant = applicant;
            ApplicantId = applicant.Id;
            Done = false;
            RequestType = type;
        }

        public string Title { get; set; }
        public User Applicant { get; set; }
        public Guid ApplicantId { get; set; }
        public bool Done { get; private set; }
        public RequestType RequestType { get; set; }

        public void SetAsDone() => Done = true;
    }

    public enum RequestType
    {
        [Description("درخواست تایید اطلاعات کاربری")]
        UserInformationConfirmation,
        [Description("درخواست رزرو بلیت")]
        TicketReservation,
        [Description("درخواست لغو بلیت")]
        TicketCancellation,
        [Description("درخواست استرداد وجه")]
        Refund,
        [Description("شارژ کیف پول")]
        WalletCharging,
        [Description("منقضی شدن مدارک")]
        DocumentsExpiration,
        [Description("بروز رسانی مدارک")]
        UpdatingDocuments,
    }
}
