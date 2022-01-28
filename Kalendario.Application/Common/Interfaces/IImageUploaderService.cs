using System.IO;
using System.Threading.Tasks;

namespace Kalendario.Application.Common.Interfaces;

public interface IImageUploaderService
{
    Task<string> UploadImage(string name, Stream file);
}