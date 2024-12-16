:: The following upload script assumes that you have butler installed (https://itch.io/docs/butler/)
:: and have a Builds directory at the same level as the project directory with the following structure:
:: 	Builds/
:: 	  - Windows/
::	      - Windows.zip
::	  - macOS/
::	      - SpeedTypingGame.app.zip
::	  - Linux/
::	      - Linux.zip
::	  - WebGL.zip

@echo off

set target="remarci/speedtypinggame"

butler push ..\Builds\Windows\Windows.zip %target%:windows --userversion 1.0.0
butler push ..\Builds\macOS\SpeedTypingGame.app.zip %target%:macos --userversion 1.0.0
butler push ..\Builds\Linux\Linux.zip %target%:linux --userversion 1.0.0
butler push ..\Builds\WebGL.zip %target%:webgl --userversion 1.0.0