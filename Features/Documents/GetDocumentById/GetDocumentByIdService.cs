using MyDocs.Models;
using MyDocs.Shared.Services.DocumentService;

namespace MyDocs.Features.Documents.GetDocumentById
{
    public class GetDocumentByIdService : IGetDocumentByIdService
    {
        private readonly IDocumentService _documentService;
        public GetDocumentByIdService(IDocumentService context)
        {
            _documentService = context;
        }

        public async Task<GetDocumentByIdResponse> GetDocumentById(GetDocumentByIdRequest request)
        {
            Document document = await _documentService.FindDocument(request.IdUser, request.IdDocument);

            return document is not null ? 
                    new GetDocumentByIdResponse(document) : 
                    throw new ArgumentNullException("Documento não localizado");
        }
    }
}
