using MyDocs.Shared.Abstracts.Alerts;

namespace MyDocs.Features.Alerts.UpdateAlert
{
    public class UpdateAlertRequest : BaseRequestGetAlert
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RecurrenceOfSending { get; set; }
        public DateTime? EndDate { get; set; }
        public string Configuration { get; set; }
    }
}
