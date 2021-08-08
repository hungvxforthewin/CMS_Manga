namespace CRMSite.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class ErrorResult
    {
        public string ErrorMessage { get; set; }
        public string Field { get; set; }
    }
}
