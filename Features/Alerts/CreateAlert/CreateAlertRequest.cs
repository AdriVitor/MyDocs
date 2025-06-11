using MyDocs.Models.Enums;

namespace MyDocs.Features.Alerts.CreateAlert
{
    public class CreateAlertRequest
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AlertRecurrence RecurrenceOfSending { get; set; }
        public string Configuration { get; set; }
    }
}
