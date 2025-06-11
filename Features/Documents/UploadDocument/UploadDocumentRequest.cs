using FastEndpoints;

namespace MyDocs.Features.Documents.UploadDocument
{
    public class UploadDocumentRequest
    {
        [FromForm]
        public FileInfo FileInfo { get; set; }
    }

    public class FileInfo
    {
        public int IdUser { get; set; }
        public IFormFile File { get; set; }
    }
}
