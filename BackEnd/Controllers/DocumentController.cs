using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using FileServerServiceLogic.Managers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FileServerServiceLogic.Providers;
using FileServerServiceLogic.Contracts.DocumentsDownload;
using FileServerServiceLogic.Helpers;
using System.Net.Mime;
using Microsoft.VisualBasic;
using FileServerServiceLogic.Contracts.ShareAbleLink;

namespace BackEnd.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDocumentUploadManager _documentUploadManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDocumentsProvider _documentsProvider;

        public DocumentController(IWebHostEnvironment hostingEnvironment, IDocumentUploadManager documentUploadManager, IHttpContextAccessor httpContextAccessor, IDocumentsProvider documentsProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _documentUploadManager = documentUploadManager;
            _httpContextAccessor = httpContextAccessor;
            _documentsProvider = documentsProvider;
        }

        [HttpPost("upload")]
        [Authorize]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile()
        {
            //var userdata = User;
            //var userclaims = User.Claims;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (!Request.Form.Files.Any())
                return BadRequest("No files found in the request");

            try
            {
                string webRootPath = _hostingEnvironment.ContentRootPath;
                string uploadsDir = Path.Combine(webRootPath, "uploads");

                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                List<string> uploadedFileUrls = new List<string>();

                foreach (var file in Request.Form.Files)
                {
                    if (file.Length <= 0)
                        return BadRequest("Invalid file length, seems to be empty");

                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(uploadsDir, fileName);

                    var buffer = 1024 * 1024;
                    using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, buffer, useAsync: false);
                    await file.CopyToAsync(stream);
                    await stream.FlushAsync();

                    string location = $"images/{fileName}";
                    uploadedFileUrls.Add(location);

                    await _documentUploadManager.uploadDocuments(fileName, userName, Guid.Parse(userId));
                }

                var result = new
                {
                    message = "Upload successful",
                    urls = uploadedFileUrls
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Upload failed: " + ex.Message);
            }
        }

        [HttpGet("allfiles")]
        [Authorize]
        public async Task<IActionResult> GetAllFiles()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return BadRequest("User was not found in Token");

            try
            {
                var result = await _documentsProvider.GetAllDocs(Guid.Parse(userId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Files cannot be fetched: " + ex.Message);
            }
        }

        [HttpGet("download/{fileGuid}")]
        [Authorize]
        public async Task<IActionResult> DwonloadFile(Guid fileGuid)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return BadRequest("User was not found in Token");

            try
            {
                var fileData = await _documentsProvider.GetFileData(fileGuid);
                var contentDisposition = new ContentDisposition
                {
                    FileName = fileData.DocumentName,
                    Inline = false
                };

                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                return CreateFileResponse(fileData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "File cannot be downloaded: " + ex.Message);
            }
        }

        [HttpPost("download/multiple")]
        [Authorize]
        public async Task<IActionResult> DwonloadFiles([FromBody] List<Guid> fileGuids)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return BadRequest("User was not found in Token");

            try
            {
                var fileData = new List<ExtDownloadFileData>();

                foreach (Guid fileGuid in fileGuids)
                {
                    fileData.Add(await _documentsProvider.GetFileData(fileGuid));
                }

                var contentDisposition = new ContentDisposition
                {
                    FileName = "Docs.zip",
                    Inline = false
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                return CreateFilesResponse(fileData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "File cannot be downloaded: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("downloadlink/{fileGuid}")]
        public async Task<IActionResult> DownloadLink(Guid fileGuid)
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userId == null) return BadRequest("User was not found in Token");

            try
            {
                var fileData = await _documentsProvider.CheckShareAbleAndGetFileData(fileGuid);
                var contentDisposition = new ContentDisposition
                {
                    FileName = fileData.DocumentName,
                    Inline = false
                };

                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                return CreateFileResponse(fileData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "File cannot be downloaded: " + ex.Message);
            }
        }

        [HttpPost("makelink")]
        [Authorize]
        public async Task<IActionResult> MakeLink([FromBody] ExtShareAbleLink data)
        {
            if (data == null) return BadRequest("No Data Found");

            if (data.Days == 0 && data.Hours == 0) return BadRequest("At least 1 hour should be added");

            try
            {
                var fileData = await _documentUploadManager.generateLink(Guid.Parse(data.documentId), data.Days, data.Hours);

                return Ok(fileData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "ShareAble link was not generated " + ex.Message);
            }

        }

        private FileContentResult CreateFilesResponse(List<ExtDownloadFileData> filesData)
        {
            if (filesData.Count == 1)
            {
                var file = filesData.First();
                var fileExt = FileExtensionHelper.GetContentType(Path.GetExtension(file.DocumentName));
                return new FileContentResult(file.DocumentContents, fileExt)
                {
                    FileDownloadName = file.DocumentName
                };
            }

            var zipped = FilesCompressionHelper.ZipFilesBatch(filesData
                .Select(d => new Tuple<string, byte[]>(d.DocumentName, d.DocumentContents)).ToArray());

            return CreateFileResponse("Docs.zip", "application/zip", zipped);

        }

        private FileContentResult CreateFileResponse(ExtDownloadFileData fileData)
        {
            var fileExt = FileExtensionHelper.GetContentType(Path.GetExtension(fileData.DocumentName));
            return new FileContentResult(fileData.DocumentContents, fileExt)
            {
                FileDownloadName = fileData.DocumentName
            };
        }

        private FileContentResult CreateFileResponse(string downloadName, string contentType, byte[] fileData)
        {
            return new FileContentResult(fileData, contentType)
            {
                FileDownloadName = downloadName
            };
        }

    }
}
