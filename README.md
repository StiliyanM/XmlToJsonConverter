# XML to JSON Converter

## Overview
The XML to JSON Converter is a web service that enables users to upload XML files, convert them to JSON format, and download the resulting JSON.

## Local Setup
To get started with the converter on your local environment, follow these steps:

1. Clone the repository:
`git clone https://github.com/StiliyanM/XmlToJsonConverter.git`
2. Ensure you have the .NET 7.0 SDK installed.
3. Navigate to the repository directory and launch the application:
`cd [Repository Directory]`
`dotnet run`
4. The converter will then be available at https://localhost:44392

## Using the Web Interface
The converter offers a user-friendly web interface:

1. Open your web browser and go to https://localhost:44392.
2. Use the provided file input to select the XML file you want to convert.
3. Click "Upload" to perform the conversion.
4. You will receive a notification within the web page indicating the success or failure of the conversion.

The web interface is designed for ease of use and requires no special tools or knowledge of API endpoints.

## Configuration

Below is a snippet of the application's configuration file (appsettings.json) that specifies the output directory for the converted JSON files:

`"FileSettings": {
    "OutputDirectory": "D:\\OutputFiles" // Converted JSON files will be saved in this directory.
  }`

Please ensure that the OutputDirectory path exists on your system and your user account has write permissions for this directory. If the path does not exist or permissions are inadequate, the application may not function as expected.
With this additional information, users should be fully aware of where the converted files will be located after using your XML to JSON Converter service.
