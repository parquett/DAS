// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadController.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the UploadController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Lib.BusinessObjects;
using Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Weblib.Controllers;
using Lib.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Hosting;

namespace Galex.Controllers
{
    /// <summary>
    /// The Upload controller.
    /// </summary>
    [Route("Upload")]
    public class UploadController : BaseController
    {
        private readonly IWebHostEnvironment environment;
        [HttpPost]
        [Route("DoUploadImage")]

       
        public async Task<ActionResult> DoUploadImage(List<IFormFile> files)
        {
            //if (!Authentication.CheckUser(this.HttpContext)) //TBD
            //{
            //    return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            //}
            //if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)AMAP.ApiContracts.Enums.Permissions.CPAccess | (long)AMAP.ApiContracts.Enums.Permissions.SMIAccess))
            //{
            //    return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            //}

            General.TraceWarn("DoUploadImage Start");
            foreach (var file in files)
            {
                if (file != null
                    && !string.IsNullOrEmpty(Request.Query["AdminWidth"])
                    && !string.IsNullOrEmpty(Request.Query["AdminHeight"])
                    && !string.IsNullOrEmpty(Request.Query["Width"])
                    && !string.IsNullOrEmpty(Request.Query["Height"])
                    && !string.IsNullOrEmpty(Request.Query["BOName"]))
                {
                    try
                    {
                        var BOName = Request.Query["BOName"];
                        var AdminWidth = Convert.ToInt32(Request.Query["AdminWidth"]);
                        var AdminHeight = Convert.ToInt32(Request.Query["AdminHeight"]);
                        var Width = Convert.ToInt32(Request.Query["Width"]);
                        var Height = Convert.ToInt32(Request.Query["Height"]);

                        string pic = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string ext = System.IO.Path.GetExtension(file.FileName);

                        General.TraceWarn(ext);

                        if (ext.ToLower() != ".png" && ext.ToLower() != ".jpg" && ext.ToLower() != ".jpeg" && ext.ToLower() != ".gif" && ext.ToLower() != ".bmp")
                        {
                            return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "Wrong Extension" });
                        }
                        if (!IsImage(file))
                        {
                            return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "Uploaded File is not an Image" });
                        }
                        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart"))))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart")));
                        }
                        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart") + BOName)))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart") + BOName));
                        }
                        var i = 0;
                        while (System.IO.File.Exists(System.IO.Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart") + BOName), pic + "_original" + ext)))
                        {
                            i++;
                            pic = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + "_" + i.ToString();
                        }
                        string path = System.IO.Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart") + BOName), pic + "_original" + ext);

                        // file is uploaded

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        Lib.Tools.Utils.ImageResizer.ResizeImageAndRatio(path, System.IO.Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart") + BOName), pic + "_adminthumb.jpeg"), AdminWidth, AdminHeight);
                        Lib.Tools.Utils.ImageResizer.ResizeImageAndRatio(path, System.IO.Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart") + BOName), pic + ".jpeg"), Width, Height);

                        Graphic uploadedImage = new Graphic();
                        uploadedImage.BOName = BOName;
                        uploadedImage.Ext = ext.Replace(".", "");
                        uploadedImage.Name = pic;

                        uploadedImage.Insert(uploadedImage);
                        var Data = new Dictionary<string, object>();
                        Data.Add("Id", uploadedImage.Id);
                        Data.Add("thumb", uploadedImage.AdminThumbnail);

                        return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
                    }
                    catch (Exception ex)
                    {

                        General.TraceWarn(ex.ToString());
                        return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "File Uploading Failed:" + ex.ToString() });
                    }
                }
            }

            return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "File Uploading Failed" });
        }


        [HttpGet]
        [Route("Download")]

        public FileResult Download()
        {
            var filepath = Path.Combine(environment.WebRootPath, "Uploads/APK", "AMAP-app.apk");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "AMAP-app.apk");
        }
        [HttpPost]
        [Route("DoUploadFile")]
        public async Task<ActionResult> DoUploadFile(List<IFormFile> files)
        {
            //if (!Authentication.CheckUser(this.HttpContext)) //TBD
            //{
            //    return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            //}
            //if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)AMAP.ApiContracts.Enums.Permissions.AccessDocumentManagement))
            //{
            //    return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            //}

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    try
                    {
                        if (file.Length > 0)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("UploadPart"), file.FileName);

                            string name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string ext = System.IO.Path.GetExtension(file.FileName);

                            General.TraceWarn(ext);

                            if (ext.ToLower() != ".pdf" && ext.ToLower() != ".doc" && ext.ToLower() != ".docx" && ext.ToLower() != ".xls" && ext.ToLower() != ".xlsx" && ext.ToLower() != ".png" && ext.ToLower() != ".jpg" && ext.ToLower() != ".jpeg" && ext.ToLower() != ".gif" && ext.ToLower() != ".bmp")
                            {
                                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "Wrong Extension" });
                            }
                            //if (!Directory.Exists(Server.MapPath(Config.GetConfigValue("UploadPart"))))
                            //{
                            //    Directory.CreateDirectory(Server.MapPath(Config.GetConfigValue("UploadPart")));
                            //}
                            var i = 0;
                            while (System.IO.File.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory() + "/Uploads", name + ext)))
                            {
                                i++;
                                name = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + "_" + i.ToString();
                            }
                            //string path = System.IO.Path.Combine(Server.MapPath(Config.GetConfigValue("UploadPart")), name + ext);
                            // file is uploaded
                            //file.SaveAs(path);

                            Document uploadedDoc = new Document();
                            uploadedDoc.Ext = ext.Replace(".", "");
                            uploadedDoc.Name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            uploadedDoc.FileName = name;

                            uploadedDoc.Insert(uploadedDoc);
                            var Data = new Dictionary<string, object>();
                            Data.Add(key: "Id", value: uploadedDoc.Id);
                            Data.Add(key: "file", value: uploadedDoc.File);
                            Data.Add(key: "name", value: uploadedDoc.Name);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
                        }
                    }
                    catch (Exception ex)
                    {

                        return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "File Uploading Failed:" + ex.ToString() });
                    }
                }
                return this.Json(new RequestResult() { Result = RequestResultType.Success });
            }

            return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "File Uploading Failed" });
        }

        public const int ImageMinimumBytes = 512;

        public UploadController(ICompositeViewEngine viewEngine, IWebHostEnvironment hostEnvironment) : base(viewEngine)
        {
            environment = hostEnvironment;
        }

        private static bool IsImage(IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }

                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                postedFile.OpenReadStream().Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.OpenReadStream()))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
