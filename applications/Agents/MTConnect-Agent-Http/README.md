# MTConnect Http Agent

## Overview
This implementation is designed to mimic the functionality of the traditional [c++ MTConnect Agent](https://github.com/mtconnect/cppagent) that has been in the industry since the introduction of the MTConnect standard. It uses the SHDR protocol to receive data from Adapters, an in-memory buffer, and a Http REST interface for retrieving data.

### Beta Notes
> We are nearing the end of our Beta and are getting close to a first official release! 

Please feel free to use and debug this application and it's source code. There are a few features that are missing as of now and they will completed before the first official release.
- Please use the [Issues](https://github.com/TrakHound/MTConnect.NET-Core/issues) tab to create issues for specific problems that you may encounter 
- Please feel free to use the [Pull Requests](https://github.com/TrakHound/MTConnect.NET-Core/pulls) tab for any suggested improvements to the source code
- For any other questions or feedback, please contact TrakHound directly at info@trakhound.com.

## Releases
Releases for this application are located under the Releases tab. The current release is listed below:
- [MTConnect Agent Current Release](https://github.com/TrakHound/MTConnect.NET/releases/tag/v0.5.1-beta-agents)

## Usage
The Agent can be run from a command line prompt or as a Windows Service using the format below:
```
agent [help|install|install-start|start|stop|remove|debug|run|run-service] [configuration_file] [http_port]

--------------------

Options :

           help  |  Prints usage information
        install  |  Install as a Service
  install-start  |  Install as a Service and Start the Service
          start  |  Start the Service
           stop  |  Stop the Service
         remove  |  Remove the Service
          debug  |  Runs the Agent in the terminal (with verbose logging)
            run  |  Runs the Agent in the terminal
    run-service  |  Runs the Agent as a Service

Arguments :
--------------------

  configuration_file  |  Specifies the Agent Configuration file to load
                         Default : agent.config.json

           http_port  |  Specifies the TCP Port to use for the HTTP Server
                         Note : This overrides what is read from the Configuration file
```
#### Example 1:
Install the Agent as a Windows Service (Note: requires Administrator Privileges)
 > agent install

#### Example 2:
Install the Agent as a Windows Service using the configuration file "agent-config.json" and Starts the service (Note: requires Administrator Privileges)
> agent install-start agent-config.json

#### Example 3:
Starts the Windows Service (Note: requires Administrator Privileges)
> agent start

#### Example 4:
Runs the Agent in the command line prompt using verbose logging and overrides the Http Port to 5001
> agent debug "" 5001

## Configuration
More information about [Configurations](https://github.com/TrakHound/MTConnect.NET/tree/master/src/MTConnect.NET-Common/Configurations). The default configuration file is shown below :
```json
{
    "devices": "devices.xml",
    "port": 5000,

    "adapters": [
        {
            "deviceKey": "OKUMA.Lathe",
            "hostname": "localhost",
            "port": 7878
        }
    ],

    "devicesNamespaces": [
        {
            "alias": "e",
            "location": "urn:okuma.com:OkumaDevices:1.3",
            "path": "/schemas/OkumaDevices_1.3.xsd"
        }
    ],

    "streamsNamespaces": [
        {
            "alias": "e",
            "location": "urn:okuma.com:OkumaStreams:1.3",
            "path": "/schemas/OkumaStreams_1.3.xsd"
        }
    ]
}
```

## Logging
Logging is done using [NLog](https://github.com/NLog/NLog) which allows for customized logging through the NLog.config file located in the application's install directory. The loggers are setup so that there is a separate logger for:
- **(agent-logger)** MTConnect Agent
- **(agent-validation-logger)** MTConnect Data Validation Errors
- **(http-logger)** Http Server
- **(adapter-logger)** MTConnect Adapters
- **(adapter-shdr-logger)** Raw SHDR lines read by the Adapter (used for debugging adapters)

The default [NLog Configuration File](NLog.config) is shown below:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

    <!-- the targets to write to -->
    <targets>

        <!--Console-->
        <target name="logconsole" xsi:type="Console" />
        
        <!--Agent Log File-->
        <target xsi:type="File" name="agent-file" fileName="logs\agent-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

        <!--Agent Validation Log File-->
        <target xsi:type="File" name="agent-validation-file" fileName="logs\agent-validation-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

        <!--Http Log File-->
        <target xsi:type="File" name="http-file" fileName="logs\http-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

        <!--Adapter Log File-->
        <target xsi:type="File" name="adapter-file" fileName="logs\adapter-${shortdate}.log"
                layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

        <!--Adapter SHDR Log File-->
        <target xsi:type="File" name="adapter-shdr-file" fileName="logs\adapter-shdr-${shortdate}.log"
                layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />
        
    </targets>

    <!-- rules to map from logger name to target -->
    <rules>

        <!--Write to Console-->
        <logger name="*" minlevel="Info" writeTo="logconsole" />

        <!--Agent Logger-->
        <logger name="agent-logger" minlevel="Info" writeTo="agent-file" final="true" />
        
        <!--Agent Validation Logger (Used to log Data Validation Errors)-->
        <logger name="agent-validation-logger" minlevel="Warning" writeTo="agent-validation-file" final="true" />
        
        <!--Http Logger-->
        <logger name="http-logger" minlevel="Info" writeTo="http-file" final="true" />
        
        <!--Adapter Logger-->
        <logger name="adapter-logger" minlevel="Info" writeTo="adapter-file" final="true" />
        
        <!--Adapter SHDR Logger (used to log raw SHDR data coming from an adapter)-->
        <logger name="adapter-shdr-logger" minlevel="Debug" writeTo="adapter-shdr-file" final="true" />

        <!--Skip non-critical Microsoft logs and so log only own logs (BlackHole) -->
        <logger name="Microsoft.*" maxlevel="Info" final="true" />
        <logger name="System.Net.Http.*" maxlevel="Info" final="true" />

    </rules>
</nlog>
```

## License
This application and it's source code is licensed under the [Apache Version 2.0 License](https://www.apache.org/licenses/LICENSE-2.0) and is free to use.
