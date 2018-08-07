using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using sample.Model;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Management;
using System.Text;
using System.Runtime.Serialization.Json;

namespace sample.Controllers
{
    public class DefaultController : Controller
    {
        private string generatedUrl;
        private string _version;


        // GET: Default
        public ActionResult Index()
        {
            ViewBag.data = new DropdownModel().List();
            ViewBag.sync_ver = new DropdownModel_Sync().List();
            return View();
        }

        public static void KillDotNetProcess()
        {
            try
            {
                ManagementClass Win32Process = new ManagementClass("Win32_Process");
                foreach (ManagementObject process in Win32Process.GetInstances())
                {
                    try
                    {
                        if (process["Name"].ToString().ToLower().Contains("dotnet"))
                        {
                            process.Delete();
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //if (File.Exists(logFilePath))
                    //    File.AppendAllText(logFilePath, "\r\n\r\nDate Time : " + DateTime.Now.ToString() + "\r\n\r\nError Details : " + ex.Message.ToString() + "\r\n\r\n" + new string('*', 65));
                    //else
                    //    File.WriteAllText(logFilePath, "Date Time : " + DateTime.Now.ToString() + "\r\n\r\nError Details : " + ex.Message.ToString() + "\r\n\r\n" + new string('*', 65));
                }
                catch { }
            }
        }

        [HttpPost]
        public ActionResult Index(ProjectConfig mbProjectConfig)
        {

                ViewBag.dummy = "SUCCESS";
            String currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
            String grandParentPath = currentDirectoryInfo.Parent.Parent.Parent.FullName;
            string lstrRelPath = grandParentPath + "\\src\\Syncfusion_" + mbProjectConfig.sync_ver;
            string lstrCommand = $"{mbProjectConfig.net_ver} {mbProjectConfig.sync_ver} {mbProjectConfig.controller } {mbProjectConfig.view}";
            string consoleoutput;
            string[] arguments = new string[] { "dotnet restore", "dotnet build", "dotnet run" };

            foreach (string currentargument in arguments)
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = currentargument;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.WorkingDirectory = @"E:\";
                p.StartInfo.CreateNoWindow = true;

                p.Start();


                if (currentargument == "dotnet run")
                {
                    KillDotNetProcess();
                }

                p.StandardInput.WriteLine(@"cd " + lstrRelPath);
                if (currentargument == "dotnet run")
                    p.StandardInput.WriteLine(currentargument + " --framework " + mbProjectConfig.net_ver);
                else
                    p.StandardInput.WriteLine(currentargument);
                p.StandardInput.Flush();
                p.StandardInput.Close();
                
                var output = "";
                if (currentargument == "dotnet run")
                {

                    p.BeginOutputReadLine();

                    p.OutputDataReceived += (sender, args) =>
                    {
                        output += args.Data;

                    };

                    //p.WaitForExit();
                    Thread.Sleep(15000);

                    consoleoutput = output;
                    var index = consoleoutput.IndexOf("http");
                    if (index > -1)
                    {
                        int lIntindex = consoleoutput.IndexOf("http");
                    int lIntLength = consoleoutput.IndexOf("Application") - lIntindex;
                    string url = consoleoutput.Substring(lIntindex, lIntLength);
                    generatedUrl = url + "/" + mbProjectConfig.controller + "/" + mbProjectConfig.view; ;
                }
                else
                    generatedUrl = "Retry after sometime";
            }
            }
            return Content(generatedUrl);


        }

        public class Version
        {
            public string sync_ver { get; set; }
        }


        [AcceptVerbs("Post")]
        public void Save(IEnumerable<HttpPostedFileBase> UploadDefault, string UploadDefault_data)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Length > 0)
                {
                    var httpPostedChunkFile = System.Web.HttpContext.Current.Request.Files["chunkFile"];
                    if (httpPostedChunkFile != null)
                    {
                        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(HttpContext.Request.Form[0])))
                        {
                            // Deserialization from JSON  
                            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Version));
                            Version bsObj2 = (Version)deserializer.ReadObject(ms);
                            this._version = bsObj2.sync_ver;
                        }
                        string path = "//netcoreapp//Syncfusion_" + this._version + "//controllers";
                        var saveFile = System.Web.HttpContext.Current.Server.MapPath("UploadedFiles");
                        var SaveFilePath = Path.Combine(saveFile, httpPostedChunkFile.FileName + ".part");
                        var chunkIndex = System.Web.HttpContext.Current.Request.Form["chunk-index"];
                        if (chunkIndex == "0")
                        {
                            //httpPostedChunkFile.SaveAs(SaveFilePath);
                        }
                        else
                        {
                            //  MergeChunkFile(SaveFilePath, httpPostedChunkFile.InputStream);
                            var totalChunk = System.Web.HttpContext.Current.Request.Form["total-chunk"];
                            if (Convert.ToInt32(chunkIndex) == (Convert.ToInt32(totalChunk) - 1))
                            {
                                var savedFile = System.Web.HttpContext.Current.Server.MapPath("UploadedFiles");
                                var originalFilePath = Path.Combine(savedFile, httpPostedChunkFile.FileName);
                                System.IO.File.Move(SaveFilePath, originalFilePath);
                            }
                        }
                        HttpResponse ChunkResponse = System.Web.HttpContext.Current.Response;
                        ChunkResponse.Clear();
                        ChunkResponse.ContentType = "application/json; charset=utf-8";
                        ChunkResponse.StatusDescription = "File uploaded succesfully";
                        ChunkResponse.End();
                    }
                    var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadFiles"];

                    if (httpPostedFile != null)
                    {
                        var fileSave = System.Web.HttpContext.Current.Server.MapPath("UploadedFiles");
                        var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                        if (!System.IO.File.Exists(fileSavePath))
                        {
                            httpPostedFile.SaveAs(fileSavePath);
                            HttpResponse Response = System.Web.HttpContext.Current.Response;
                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.StatusDescription = "File uploaded succesfully";
                            Response.End();
                        }
                        else
                        {
                            HttpResponse Response = System.Web.HttpContext.Current.Response;
                            Response.Clear();
                            Response.Status = "204 File already exists";
                            Response.StatusCode = 204;
                            Response.StatusDescription = "File already exists";
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 204;
                Response.Status = "204 No Content";
                Response.StatusDescription = e.Message;
                Response.End();
            }
        }


    }
}