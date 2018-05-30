# ServerlessIoTProcessingDemo
.NET Stammtisch Demo

# Prerequisites

For local development:

Azure Cosmos DB Emulator
https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator

Azure Storage Emulator
https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator

Plugin for Visual studio Code 
https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions

Plugin for Visual Studio
https://marketplace.visualstudio.com/items?itemName=VisualStudioWebandAzureTools.AzureFunctionsandWebJobsTools

# Prepare telemetry database

- Create a cosmos db (SQL Schema) named "testdb"
- Create the following collections within this database
  - telemetry
  - analytics
  - drivers
- Add drivers to your drivers collection following this schema:

{
    "id": "VEHICLE-ID",
    "Name": "NAME OF DRIVER",
    "IsCriminal": true,
}

- Send requests to your local http-endpoint with method POST and a payload like this:

{
	"Timestamp": "2018-05-07 07:18:10",
    "DeviceId": "VEHICLE-ID",
	"Velocity": 300
}
