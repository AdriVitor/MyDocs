using Moq;
using MyDocs.Features.Documents.DeleteDocument;
using MyDocs.Infraestructure.ExternalServices.AzureBlob;
using MyDocs.Shared.Services.DocumentService;
using MyDocs.Tests.Shared;

namespace MyDocs.Tests.Documents.DeleteDocument
{
    public class DeleteDocumentServiceTests
    {
        [Fact]
        public async Task DeleteDocument_ShouldThrowException_WhenDocumentIsNull()
        {
            using var context = MemoryDatabase.Create();
            var document = GenerateModelsService.CreateDocument(1, 1, "arquivo.pdf", ".pdf", 1024, "unique-name.pdf");
            context.Documents.Add(document);
            context.SaveChanges();

            var request = new DeleteDocumentRequest() { IdUser = 2, IdDocument = 2 };

            var documentService = new Mock<IDocumentService>();
            var azureBlobServiceMock = new Mock<IAzureBlobService>();

            var service = new DeleteDocumentService(documentService.Object, azureBlobServiceMock.Object, context);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.DeleteDocument(request));
        }
    }
}
