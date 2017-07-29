using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Base;
using System.IO;
using trobadoo.com.web.Entities;

namespace trobadoo.com.web.Controllers.Ajax
{
    public class AjaxFileHandlerController : BaseController //: AjaxBaseController
    {
        //public List<Photo> AjaxResponse = new List<Photo>();

        //public override void ProcessData(IValueProvider valueProvider)
        //{
        //    if (Request.Files.Count > 0)
        //    {
        //        var path = Server.MapPath("/uploadImages");
        //        var webPath = string.Empty;
        //        var index = 0;
        //        foreach (HttpPostedFile file in Request.Files)
        //        {
        //            var fileType = file.ContentType;
        //            var photoToken = Request["photoToken"];
        //            //Creamos un directoria para guardar la imagen
        //            path += "/" + photoToken;
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            if (fileType.Contains("image") && (file.FileName.Contains(".jpg") || file.FileName.Contains(".jpeg") || file.FileName.Contains(".gif") || file.FileName.Contains(".png")))
        //            {
        //                try
        //                {
        //                    //Save the image
        //                    var filename = System.IO.Path.Combine(path, index.ToString());
        //                    file.SaveAs(filename);

        //                    //Seteamos la url web de la imagen
        //                    webPath = "/uploadImages/" + photoToken;

        //                    //Devolvemos la respuesta
        //                    AjaxResponse.Add(new Photo
        //                    {
        //                        name = file.FileName,
        //                        size = file.InputStream.Length,
        //                        url = webPath.Replace("\\", "/") + "/" + filename,
        //                        deleteUrl = webPath.Replace("\\", "/") + "/" + filename,
        //                        physicalPath = filename,
        //                        thumbnailUrl = webPath.Replace("\\", "/") + "/" + filename
        //                    });
        //                    index++;
        //                }
        //                catch (Exception ex)
        //                {
        //                    AjaxResponse.Add(new Photo
        //                    {
        //                        name = file.FileName,
        //                        size = file.InputStream.Length,
        //                        error = "Error saving file. Ex: " + ex.ToString()
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                AjaxResponse.Add(new Photo
        //                    {
        //                        name = file.FileName,
        //                        size = file.InputStream.Length,
        //                        error = "Solo se permiten imagenes (gif,jpg,png)"
        //                    });
        //            }
        //        }
        //    }
        //}

        private string _path;
        private string _webPath;

        private string StorageRoot
        {
            get { return Path.Combine(Server.MapPath("/uploadImages")); }
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public void Delete(string id)
        {
            var filename = id;
            var filePath = Path.Combine(StorageRoot, filename);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var r = new List<ViewDataUploadFilesResult>();

            var index = 0;
            var photoToken = Request["photoToken"];
            //Creamos un directoria para guardar la imagen
            _path = Path.Combine(StorageRoot, photoToken);
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            //Seteamos la url web de la imagen
            _webPath = "/uploadImages/" + photoToken;

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(Request, statuses);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }

                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";

                return result;
            }

            return Json(r);
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;

            var fullName = Path.Combine(StorageRoot, Path.GetFileName(fileName));

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new ViewDataUploadFilesResult()
            {
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = _webPath + "/" + fileName,
                delete_url = "/AjaxFileHandler/Delete/" + fileName,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                delete_type = "GET",
            });
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var fullPath = Path.Combine(_path, Path.GetFileName(file.FileName));

                file.SaveAs(fullPath);

                statuses.Add(new ViewDataUploadFilesResult()
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = _webPath + "/" + file.FileName,
                    delete_url = "/AjaxFileHandler/Delete/" + file.FileName,
                    thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                    delete_type = "GET",
                });
            }
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }
    }
}
