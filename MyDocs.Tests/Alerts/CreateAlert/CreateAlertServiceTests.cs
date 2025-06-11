using Moq;
using MyDocs.Features.Alerts.CreateAlert;
using MyDocs.Infraestructure.Services.ScheduleAlertService;
using MyDocs.Models.Enums;
using MyDocs.Models;
using MyDocs.Tests.Shared;
using MyDocs.Shared.DTOs;

namespace MyDocs.Tests.Alerts.CreateAlert
{
    public class CreateAlertServiceTests
    {
        [Fact]
        public async Task AddAlert_ShouldCreateAlertSuccessfully()
        {
            var userId = 1;
            var email = "usuario@email.com";
            var request = new CreateAlertRequest
            {
                IdUser = userId,
                Name = "Alerta de Fatura",
                Description = "Alerta mensal para pagamento",
                RecurrenceOfSending = AlertRecurrence.Month
            };

            var expectedDate = DateTime.Now;

            using var context = MemoryDatabase.Create();

            context.UsersCredentials.Add(new UserCredentials
            {
                IdUser = userId,
                Email = email,
                Password = "Teste123"
            });
            context.SaveChanges();

            var processAlertServiceMock = new Mock<IScheduleAlertService>();
            processAlertServiceMock.Setup(x => x.ScheduleAlert(It.IsAny<ScheduleJobDTO>(), It.IsAny<EmailRequestDTO>()))
                                   .Returns(Task.CompletedTask);

            var service = new CreateAlertService(context, processAlertServiceMock.Object);

            await service.AddAlert(request);

            var createdAlert = context.Alerts.FirstOrDefault(a => a.IdUser == userId);

            Assert.NotNull(createdAlert);
            Assert.Equal(request.Name, createdAlert.Name);
            Assert.Equal(request.Description, createdAlert.Description);
            Assert.Equal((int)request.RecurrenceOfSending, createdAlert.RecurrenceOfSending);
            Assert.Equal(userId, createdAlert.IdUser);
            Assert.Equal(expectedDate.Date, createdAlert.CreationDate.Date);

            processAlertServiceMock.Verify(p =>
                p.ScheduleAlert(It.IsAny<ScheduleJobDTO>(), It.IsAny<EmailRequestDTO>()),
                Times.Once);
        }

    }
}
