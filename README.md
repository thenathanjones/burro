What is it?
===========
Burro is a tool for parsing output from build servers in a consistent way.

Why do this?
============
Why not?  I had an idea one day to do a generic build monitoring suite of tools for Windows.  It made sense that the desktop monitoring tool, the large screen monitoring tool and the build light tool would all use the same parsing code.

Surely this exists?
===================
If it does, Google is having trouble finding it at of the 18th of September 2011.  I've used Greenscreen (Ruby) and BigVisibleWall (Scala) but wanted a .NET version to tie in with the USB library I'd written, [NUSB](https://github.com/thenathanjones/nusb).

How do I use this?
==================
Pre-requisites
--------------
I've built Burro as a .NET 4 project, so the runtime will need to be installed.
Configuration
-------------
What Burro parses is configured using a YAML file that is passed in upon initialisation.  Going forward there may be some sample configurations for various CI server, but for the moment, here is an example:

      # YAML file for parsing a Cruise Control Build Server
      -
        servertype: CruiseControl
        url: http://10.1.1.2:8153/go/cctray.xml
        username: 
        password: 
        pipelines:
          -
            name: "Trunk :: spec"

The structure is very simple.  At this point in time, it uses the following fields:

* servertype - This is used to configure what type of server is being checked, and in turn what parser is used to interpret the output.
* url - This is dependent on the servertype used, but is the path to the server itself.  In the case of CruiseControl-like output, it's the address of the XML output.
* username - This is the username used to authenticate with the server.  If not required, leave this field blank.
* password - This is the password used to authenticate with the server.  If not required, leave this field blank.
* pipelines - This is a list of the pipelines that we're interested in reporting on from the target server.  In particular at this stage Burro is only concerned with the name.

Usage
-----
I'll put some examples up as things progress, for now you'll just have to work it out yourself.  For a working example, see [Luces](https://github.com/thenathanjones/luces) which uses Burro to parse output which is used for the logic in controlling build lights.

Limitations
===========
At this stage, the only supported build output is CruiseControl, as most of the build server tools publish the Cruise Control XML output.  