namespace CRMSite.Common
{
    public class SiteConst
    {
        #region Characters
        public const char SeparateChar = '|';
        public const char SubstractChar = '-';
        public const char SlashChar = '/';
        public const char CommaChar = ',';
        public const char DoubleBackSlash = '\\';
        public const char Underscore = '_';
        #endregion

        #region Pagination
        public const int PageSize = 10;
        public const int PageNumber = 5;
        #endregion

        #region Template
        public const string RangeTemplate = @"^\s*([0-9]+)\s*-\s*([0-9]+)\s*$";
        public const string NumberTemplate = @"^[0-9]+$";
        #endregion

        #region Message
        public const string SystemError = "Lỗi hệ thống";

        public const string NotFoundError = "Không tìm thấy dữ liệu";
        #endregion

        #region Format
        public class Format
        {
            public const string MonthYearFormat = "yyyy/MM";
            public const string DateFormat = "dd/MM/yyyy";
            public const string FullDateTimeFormat = "dd/MM/yyyy HH:mm";
            public const string TimeFormat = "h\\:mm";
        }
        #endregion

        public class FileFormat
        {
            public const string Image = "Image";
            public const string Doc = "Doc";
            public const string Excel = "Excel";
            public const string All = "*";
        }

        public class SessionKey
        {
            public const string LOGIN_SESSION = "LoginSession";

            public const string FILE_NAME = "FileName";

            public const string OTP_VALUE = "OtpValue";

            public const string INVESTOR_EMAIL = "InvestorEmail";

            public const string OWN_SHARE = "OwnShare";

            public const string EVENT = "Event";
        }

        public class TokenKey
        {
            public const string BRANCHCODE = "BranchCode";

            public const string OFFICECODE = "OfficeCode";

            public const string DEPARTMENTCODE = "DepartmentCode";

            public const string TEAMCODE = "TeamCode";

            public const string FULLNAME = "FullName";

            public const string USERNAME = "Username";
        }

        public enum UploadStatus
        {
            TOO_BIG = 0,
            NO_CONTENT = 1,
            NOT_SELECT_FILE = 2,
            INVALID_FORMAT = 3,
            FAILURE = 4,
            SUCCESS = 5
        }
        public class CCCD
        {
            public const string CSDK = "Cục cảnh sát đăng ký quản lý cư trú và dữ liệu quốc gia về dân cư";
            public const string CSDK_EN = "Police Department of Residence Registration and Management and National Population Database";
            public const string CSHC = "Cục Cảnh sát quản lý hành chính về trật tự xã hội";
            public const string CSHC_EN = "Police Department on Administrative Management of Social Order";
        }
    }
}
