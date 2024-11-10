# SyncfusionHelpDesk

![Screenshot](SyncfusionHelpDesk.png)

## Covered in the Book:
[Blazor Succinctly](https://www.syncfusion.com/ebooks/blazor-succinctly)

### To Install

1) Create a Database on your SQL server, and run scripts in **!SQL directory**
2) Edit *appsettings.json* to set the database connection in the **DefaultConnection** property

### To Enable Syncfusion

1) Get an **API key** from [Syncfusion.com](https://support.syncfusion.com/kb/article/9795/how-to-get-community-license-and-install-it)
2) Open **appsettings.json**: 
- For **SYNCFUSION_APIKEY** and enter your Syncfusion API key

### To Enable Emails

1) Get an **API key** from [app.sendgrid.com](https://app.sendgrid.com)
2) Open **appsettings.json**: 
- For **SENDGRID_APIKEY** enter your SendGrid API key 
- For **SenderEmail** enter your Email address 

### To Enable OpenAI or Azure Open AI

1) Open **appsettings.json**: 
- For **OpenAI/apiKey** enter your *Open AI* API key
- If using *Azure Open AI* also enter **OpenAI/deploymentName** and **OpenAI/endpoint**

### Also See
* [SyncfusionHelpDesk - Blazor WebAssembly version](https://github.com/ADefWebserver/SyncfusionHelpDeskClient)
