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

    public class Version
    {
        public string sync_ver { get; set; }
    }
    public class DefaultController : Controller
    {
        private string generatedUrl;
        private string _version;
        private static string path;

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
                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
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
                    string output; int exitCode;
                    var timeout = 30000;
                    if (currentargument == "dotnet run")
                    {
                        KillDotNetProcess();
                    }

                    p.StandardInput.WriteLine(@"cd " + lstrRelPath);
                    if (currentargument != "dotnet run")
                    {
                        p.StandardInput.WriteLine(currentargument);
                        p.StandardInput.Flush();
                        p.StandardInput.Close();
                    }

                    if (currentargument == "dotnet run")
                    {
                        p.StandardInput.WriteLine(currentargument + " --framework " + mbProjectConfig.net_ver);
                        p.StandardInput.Flush();
                        p.StandardInput.Close();
                        var outputBuilder = new StringBuilder();
                        var errorBuilder = new StringBuilder();
                        //******************************

                        try
                        {
                            p.BeginOutputReadLine();
                            p.BeginErrorReadLine();
                            p.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                outputBuilder.AppendLine(e.Data);
                            }
                        };
                            p.ErrorDataReceived += (sender, e) =>
                            {
                                if (e.Data == null)
                                {
                                    errorWaitHandle.Set();
                                }
                                else
                                {
                                    outputBuilder.AppendLine(e.Data);
                                }
                            };

                            p.Start();


                            bool exited = p.WaitForExit(timeout);

                            //exitCode = p.ExitCode;
                            consoleoutput = outputBuilder.ToString();
                            var index = consoleoutput.IndexOf("http");
                           if (index > -1)
                            {
                                int lIntindex = consoleoutput.IndexOf("http");
                                int lIntLength = consoleoutput.IndexOf("Application") - lIntindex;
                                string url = consoleoutput.Substring(lIntindex, lIntLength);
                                generatedUrl = url + "/" + mbProjectConfig.controller + "/" + mbProjectConfig.view; ;
                            }
                            else
                            {
                                generatedUrl = "Retry after sometime";
                            }

                        }
                        finally
                        {
                            outputWaitHandle.WaitOne(0);
                            errorWaitHandle.WaitOne(0);
                        }
                    }

                    //******************************









                    //var output = "";
                    //if (currentargument == "dotnet run")
                    //{

                    //    p.BeginOutputReadLine();

                    //    p.OutputDataReceived += (sender, args) =>
                    //    {
                    //        output += args.Data;

                    //    };

                    //    //p.WaitForExit();
                    //    Thread.Sleep(15000);



                    //    consoleoutput = output;
                    //var index = consoleoutput.IndexOf("http");
                    //if (index > -1)
                    //{
                    //    int lIntindex = consoleoutput.IndexOf("http");
                    //    int lIntLength = consoleoutput.IndexOf("Application") - lIntindex;
                    //    string url = consoleoutput.Substring(lIntindex, lIntLength);
                    //    generatedUrl = url + "/" + mbProjectConfig.controller + "/" + mbProjectConfig.view; ;
                    //}
                    //else
                    //    generatedUrl = "Retry after sometime";
                }
            }
            return Content("Generated URL: " + generatedUrl);

        }

        public class Version
        {
            public string sync_ver { get; set; }
        }


        [AcceptVerbs("Post")]
        public void Save()
        {
            HttpContext lHttpContext = System.Web.HttpContext.Current;
            try
            {
                if (lHttpContext.Request.Files.AllKeys.Length > 0)
                {
                    var httpPostedFile = lHttpContext.Request.Files["controller_file"];

                    if (httpPostedFile != null)
                    {
                        if (httpPostedFile.FileName.EndsWith("cs"))
                        {
                            path = "//netcoreapp//Syncfusion_" + lHttpContext.Request.Form[0].ToString() + "//controllers";
                        }
                        else if (httpPostedFile.FileName.EndsWith("cshtml"))
                        {
                            path = "//netcoreapp//Syncfusion_" + lHttpContext.Request.Form[0].ToString() + "//views//" + lHttpContext.Request.Form[1].ToString();
                        }
                        string targetFolder = path;
                        var fileSave = lHttpContext.Server.MapPath(targetFolder);
                        var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                        if (!System.IO.File.Exists(fileSavePath))
                        {
                            httpPostedFile.SaveAs(fileSavePath);
                            HttpResponse Response = lHttpContext.Response;
                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.StatusDescription = "File uploaded succesfully";
                            Response.End();
                        }
                        else
                        {
                            HttpResponse Response = lHttpContext.Response;
                            Response.Clear();
                            Response.Status = "400 File already exists";
                            Response.StatusCode = 400;
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
                Response.StatusCode = 400;
                Response.Status = "400 No Content";
                Response.StatusDescription = e.Message;
                Response.End();
            }
        }


    }
}