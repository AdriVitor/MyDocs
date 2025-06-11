namespace MyDocs.Features.Alerts.GetAlerts
{
    public class GetAlertsRequest
    {
        public int IdUser { get; set; }
        public StatusAlert Status { get; set; }
    }

    public enum StatusAlert
    {
        Expired = 1,
        NotExpired = 2
    }
}
