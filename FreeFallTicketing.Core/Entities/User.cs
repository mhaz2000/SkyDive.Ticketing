using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using static SkyDiveTicketing.Core.Entities.User;

namespace SkyDiveTicketing.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            SecurityStamp = Guid.NewGuid().ToString();
            Messages = new List<Message>();
            PersonalInformationIsCompeleted = false;
        }

        /// <summary>
        /// کد کاربری
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// تاریخ ثبت نام
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        public string? NationalCode { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// نوع حساب کاربری
        /// </summary>
        public UserType? UserType { get; set; }

        /// <summary>
        /// کد ارسالی
        /// </summary>
        public string? OtpCode { get; set; }

        /// <summary>
        /// زمان درخواست کد
        /// </summary>
        public DateTime? OtpRequestTime { get; set; }

        /// <summary>
        /// تعداد دفعاتی که ورود با خطا مواجه گشت.
        /// </summary>
        public int? LoginFailedAttempts { get; set; }

        /// <summary>
        /// آیا اطلاعات شخصی وارد شده است.
        /// </summary>
        public bool PersonalInformationIsCompeleted { get; set; }

        /// <summary>
        /// اطلاعات مسافر
        /// </summary>
        public Passenger? Passenger { get; set; }

        /// <summary>
        /// پیام های کاربر
        /// </summary>
        public ICollection<Message>? Messages { get; private set; }

        public List<RefreshToken> RefreshTokens { get; set; }

        public string FullName => FirstName + " " + LastName;


        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }

        public void AddRefreshToken(string token, string userId, double min = 10)
        {
            RefreshTokens.Add(new RefreshToken(token, DateTime.Now.AddMinutes(min), userId));
        }

        public class RefreshToken
        {
            public Guid Id { get; set; }
            public string Token { get; private set; }
            public DateTime Expires { get; private set; }
            public string UserId { get; private set; }
            public bool Active => DateTime.Now <= Expires;

            public RefreshToken(string token, DateTime expires, string userId)
            {
                Id = Guid.NewGuid();
                Token = token;
                Expires = expires;
                UserId = userId;
            }
        }
    }

    public enum UserStatus
    {
        [Description("فعال")]
        Active = 1,
        [Description("غیر فعال")]
        Inactive,
        [Description("در انتظار تایید")]
        Pending,
        [Description("در انتظار تکمیل")]
        AwaitingCompletion
    }
}
