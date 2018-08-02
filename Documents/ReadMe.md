# .NET core sample to support multiple syncfusion assemblies

## Completion report

This application is divided into two web projects each supporting different syncfusion assemblies. As of now, I&#39;ve added support for &quot;16.1.0.37&quot; and &quot;16.2.0.41&quot; assemblies. Those versions can be modified for each release to support the latest versions.

Support to load DLLs at runtime has been given.

This project is executed using .NET CLI &quot;watch and run&quot; commands, so that the user can add controller and view pages at runtime and the application will be refreshed upon changes.

Developers will be given access to an Web APP that contains options to set the .NET core version and Syncfusion assembly version to which the application needs to be targeted and controller and view pages. Based on the given input the application will be built and run. User will be provided with generated URL.

Options to upload controller and view page is provided.

## Yet to do

1. Include source code in sample page..
2. Generate URL based on success message in html page or display error incase of failures.
3. Need to install the required software in server machine, once provided.
4. Assign task scheduler to delete the samples once in 30 days.
5. Restrict editing permission to configuration files in the sample


## Note for users (Syncfusion developers)

Input any of the syncfusion assembly versions that our application loads

Provide .NET core version in which your app needs to be executed (only major versions are inclusive)

Upload Controller and view page in application using browse options available in Homepage.

> Note: Use namespace: &quot; namespace DotnetFiddle.Controllers &quot;

URL will be generated based on the given controller and Action name

The samples will remain in server only for a period of 30 days. This information may be shared with our customers