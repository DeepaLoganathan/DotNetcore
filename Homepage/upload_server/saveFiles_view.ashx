<%@ WebHandler Language="C#" Class="saveFiles" %>

using System;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

public class Version{
    public string sync_ver { get; set; }
}

public class saveFiles : IHttpHandler
{
    private string _version;
    public void ProcessRequest(HttpContext context)
    {
         using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(context.Request.Form[0])))
        {
            // Deserialization from JSON  
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Version));
            Version bsObj2 = (Version)deserializer.ReadObject(ms);
            this._version = bsObj2.sync_ver;
        }
        string path = "//rootpath//Syncfusion_" + this._version + "//views";
        string targetFolder = HttpContext.Current.Server.MapPath(path);
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }
        HttpRequest request = context.Request;
      
        string requestPath = context.Request.Path;
        HttpFileCollection uploadedFiles = context.Request.Files;
        if (uploadedFiles != null && uploadedFiles.Count > 0 && uploadedFiles[0].FileName!="")
        {
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                if (uploadedFiles[i].FileName != null && uploadedFiles[i].FileName != "")
                {
                    string fileName = uploadedFiles[i].FileName;
                    int indx = fileName.LastIndexOf("\\");
                    if (indx > -1)
                    {
                        fileName = fileName.Substring(indx + 1);
                    }
                    uploadedFiles[i].SaveAs(targetFolder + "\\" + fileName);
                }
            }
            // To maintain the same web page instead of navigate to another page in Synchronous upload, we have sent response back.
            if (requestPath.Contains("synchronous.html") || requestPath.Contains("saveFiles.ashx"))
            {
                context.Response.Write("<span id='successmsg'> Successfully uploaded</span>");

                context.Response.WriteFile("uploadIframe.html");
                
            }
        }
        else
        {
            // To maintain the same web page instead of navigate to another page in Synchronous upload, we have sent response back.

            if (requestPath.Contains("synchronous.html") || requestPath.Contains("saveFiles.ashx"))
            {
                context.Response.Write("<span id='successmsg'> Successfully uploaded</span>");
                context.Response.WriteFile("uploadIframe.html");
            }
        }


    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}