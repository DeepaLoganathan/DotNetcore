console.log("Hello world!");

var express = require('express');
var bodyParser = require('body-parser');
var app     = express();
const spawn = require('child_process').spawn;
var myBatFilePath = "E:\\_work\\Dotnetcore\\Dotnet_finalsample\\refresh.bat";

//Define Syncfusion assembly version available in application
var sync_versions = ["16.1.0.37","16.2.0.41"];
var port;
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
app.listen(8080, function() {
    console.log('Server running at http://localhost:8080/');
  });
app.get('/', (req, res) => res.send('Hello World!'));


const handleForm = (req, res) => {
  return new Promise((resolve)=>{
    const bat = spawn('cmd.exe', ['/c', myBatFilePath, req.query.netcore_ver, req.query.sync_ver, req.query.controller,req.query.view]);
  
    // // Handle normal output
 
    bat.stdout.on('data', (data) => {
        var str = String.fromCharCode.apply(null, data);
        console.log(str);
       
    });
    
    // // // Handle error output
    bat.stderr.on('data', (data) => {
      console.log("error");
        var str = String.fromCharCode.apply(null, data);
        console.error(str);
    });
    // bat.on('exit', function (code) {
    //   console.log('child process exited with code ' + code);
      
    // });
  })
 
}


app.get('/submit_form', async function(req, res) {
     var result = await handleForm(req, res);
    //  console.log(result)
    //  if(result == "resolved")
    //  { 
    //    debugger
    //    console.log(result)

    //    if(req.query.sync_ver == sync_versions[0]) port = 5000;
    //   else if(req.query.sync_ver == sync_versions[1]) port = 5001;
      res.send("Generated URL:"+ Generated_URL);

    // }
}); 
