using Moq;
using MyDocs.Features.Documents.GetDocumentById;
using MyDocs.Models;
using MyDocs.Shared.Services.DocumentService;
using MyDocs.Tests.Shared;
using System.Security.Cryptography;

namespace MyDocs.Tests.Documents.GetDocumentById
{
    public class GetDocumentByIdServiceTests
    {

        [Fact]
        public async Task GetDocumentById_ShouldThrowException_WhenDocumentIsNull()
        {
            using var context = MemoryDatabase.Create();
            var document = GenerateModelsService.CreateDocument(3, 3, "arquivo.pdf", ".pdf", 1024, "unique-name.pdf");
            context.Documents.Add(document);
            context.SaveChanges();

            var request = new GetDocumentByIdRequest() { IdUser = 2, IdDocument = 2 };

            var documentService = new Mock<IDocumentService>();

            var service = new GetDocumentByIdService(documentService.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetDocumentById(request));
        }
    }
}
