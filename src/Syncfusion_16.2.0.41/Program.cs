#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Hosting;

namespace DotnetFiddle
{
    public class Program
    {
        private static string version;

        // To get current .NET core version on which the application runs
        private static string GetNetCoreVersion()
        {
            var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
            var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
            if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                return assemblyPath[netCoreAppIndex + 1];
            return null;
        }


        public static void Main(string[] args)
        {
            // Read XML file to get dlls to be added 
            XmlReader xmlReader = XmlReader.Create("additional_dll.xml");
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Version"))
                {
                     version = xmlReader.GetAttribute("value");
                }

                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "DLL"))
                {
                    if (xmlReader.HasAttributes)
                    {
                        String name = xmlReader.GetAttribute("name") + ", Version = 16.2.0.41,  Culture=neutral, PublicKeyToken=null";
                        var an = new AssemblyName(name);
                        var FWversion = GetNetCoreVersion();
                        FWversion = FWversion.Substring(0, FWversion.Length - 2);
                        var Dll_path = $"C:\\Program Files (x86)\\Syncfusion\\Essential Studio\\ASP.NET CORE\\{version}\\NuGetPackages\\{xmlReader.GetAttribute("name")}\\{version}\\lib\\netcoreapp{FWversion}\\{xmlReader.GetAttribute("name")}.dll";
                        var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(@Dll_path);

                    }
                }
            }

            // configure host environment

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
				.UseUrls("http://*:5001")
                .UseStartup<Startup>()
                .UseApplicationInsights()
                 .CaptureStartupErrors(true)
                .Build();
            if (args.Length == 4)
                Console.Write("Generated url: localhost:5001/{0}/{1}\n", args[1], args[2]);
            else if (args.Length == 5)
                Console.Write("Generated url: localhost:5001/{0}/{1}/{2}/{3}\n", args[0], args[1], args[2], args[3]);

            host.Run();
        }
    }
}
