using Microsoft.EntityFrameworkCore;
using MyDocs.Infraestructure.Persistence;
using MyDocs.Infraestructure.Services.ProcessAlerts;
using MyDocs.Infraestructure.Services.ScheduleAlertService;
using MyDocs.Models;
using MyDocs.Models.Enums;
using MyDocs.Shared.DTOs;

namespace MyDocs.Features.Alerts.CreateAlert
{
    public class CreateAlertService : ICreateAlertService
    {
        private readonly Context _context;
        private readonly IScheduleAlertService _processAlertService;
        public CreateAlertService(Context context, IScheduleAlertService processAlertService)
        {
            _context = context;
            _processAlertService = processAlertService;
        }

        public async Task AddAlert(CreateAlertRequest request)
        {
            try
            {
                Alert alert = new Alert()
                {
                    IdUser = request.IdUser,
                    Name = request.Name,
                    Description = request.Description,
                    RecurrenceOfSending = (int)request.RecurrenceOfSending,
                    CreationDate = DateTime.Now,
                    EndDate = null
                };

                _context.Alerts.Add(alert);

                string dateSendingAlert = await ConfigureDateSendOfAlert(request.RecurrenceOfSending, alert.CreationDate);
                ScheduleJobDTO scheduleJobDTO = new ScheduleJobDTO(dateSendingAlert, "alerts");

                string emailUser = await FindEmailUser(request.IdUser);
                EmailRequestDTO emailRequestDTO = new EmailRequestDTO(emailUser, string.Concat("My Docs - Alerta ", alert.Name), $"<h1> Você possui um alerta de boleto para hoje  - {alert} </h1>");

                if(dateSendingAlert is not null && emailUser is not null)
                    await _processAlertService.ScheduleAlert(scheduleJobDTO, emailRequestDTO);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível cadastrar o alerta, tente novamente mais tarde");
            }
        }

        private async Task<string> FindEmailUser(int idUser)
        {
            var userCredential =  await _context
                                    .UsersCredentials
                                    .FirstOrDefaultAsync(uc => uc.IdUser == idUser);

            return userCredential?.Email;
        }

        private async Task<string> ConfigureDateSendOfAlert(AlertRecurrence alertRecurrence, DateTime dateSend)
        {
            switch (alertRecurrence)
            {
                case AlertRecurrence.JustOnce:
                    return dateSend.ToString(); break;
                case AlertRecurrence.Week:
                    return $"0 10 * * {dateSend.DayOfWeek}"; break;
                case AlertRecurrence.Month:
                    return $"0 10 {dateSend.Day} * *"; break;
                case AlertRecurrence.Year:
                    return $"0 10 {dateSend.Day} {dateSend.Month} *"; break;
                default:
                    throw new ArgumentException("Seleciona uma recorrência para o alerta");
            }
        }
    }
}
