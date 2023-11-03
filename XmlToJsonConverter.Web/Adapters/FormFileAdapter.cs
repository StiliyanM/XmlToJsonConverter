using XmlToJsonConverter.Application.Interfaces;

namespace XmlToJsonConverter.Web.Adapters
{
    public class FormFileAdapter : IApplicationFile
    {
        private readonly IFormFile _formFile;

        public FormFileAdapter(IFormFile formFile)
        {
            _formFile = formFile ?? throw new ArgumentNullException(nameof(formFile));
        }

        public string FileName => _formFile.FileName;
        public long Length => _formFile.Length;

        public Stream OpenReadStream()
        {
            return _formFile.OpenReadStream();
        }
    }
}
