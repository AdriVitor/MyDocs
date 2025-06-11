using Microsoft.EntityFrameworkCore;
using MyDocs.Infraestructure.ExternalServices.AzureBlob;
using MyDocs.Infraestructure.Persistence;
using MyDocs.Models;
using MyDocs.Shared.Services.DocumentService;

namespace MyDocs.Features.Documents.DownloadDocument
{
    public class DownloadDocumentService : IDownloadDocumentService
    {
        private readonly IDocumentService _documentService;
        private readonly IAzureBlobService _azureBlobService;
        private const string _messageError = "Não foi possível realizar o download do arquivo";
        public DownloadDocumentService(IDocumentService documentService, IAzureBlobService azureBlobService)
        {
            _documentService = documentService;
            _azureBlobService = azureBlobService;
        }

        public async Task<DownloadDocumentResponse> DownloadDocument(DownloadDocumentRequest request)
        {
            Document document = await _documentService.FindDocument(request.IdUser, request.IdDocument);
            if (document is null)
                throw new ArgumentNullException(_messageError);

            Stream content = await _azureBlobService.DownloadAsync(document.UniqueFileName);

            return new DownloadDocumentResponse(content, document.FileName, document.FileSize);
        }
    }
}
