# Complete list of commands for the Javascript botent
`test` - Test the connection between the server and the client  
`alert [message]` - Display a classic alert box with the specified message  
`push-html [html content/local html file path]` - Replace the page with the specified content (kind of deface i guess)  
`append-html [html content/local html file path]` - Append html content at the end of the page  
`get-site` - Display the url of the current site  
`get-cookie` - Read cookies from the current site  
`form-infect` - Infect the form with a data dumping code  
**Note:** Results are sent when the form is submitted, then it gets saved at TutServer/formDump.txt with all field names, values and the clientID which submitted the form  
`get-info` - Get the screen size of the target  
`check-pop-up` - Check if a pop-up window can be displayed  
`check-activex` - Check if an ActiveX command can be executed / accepeted by the user/browser  
`play-audio [audio file link]` - Play's an audio file from the specified link  
`hijack-link [target link]` - Replace all links on the site with the specified link  
`prompt [message]` - Display a basic prompt with the specified message  
**Note:** The client will return the text the user responded with to the prompt.  
`redirect [target link]` - Redirect the page to the specified link  
`geolocate` - Try to get the users position latitude and longitude  
**Note:** The user will see a dialog and gets presented with yes/no if responds with yes you get the location, else you get an error permission denied.  
Results are sent when we got location or the user cancelled the prompt, if you switch the controlled client the result will still get displayed.  
`execute-js [javascript code/local javascript file]` - Execute raw javascript in the browser  
`ipscan [3rd octet]` - Scan the network for online IPs  
**Note:** The program can only work with 255.255.0.0 subnet mask meaning that you can't specify the first two octets.
The last octet gets scanned from 0 to 254 and you can specify the 3rd octet  
`ipscan` - Scan the network for common class C Adresses  
**Note:** This will only try the popular 3rd octets: xxx.xxx.0, 1, 10 match is not 100% chance  
`port-scan [IP Address] [port Number]` - Scans a specific port for a specific IP  
**Note:** Some ports a blocked by the browsers in this case the bot will respond with a message like "blocked by browser" and not port closed.  
`prevent-close` - Prevents the closing of the tab  
**Note:** This doesn't really work science browsers block this kind of behavior, becauase scammers used it to keep their page open.  
But the user can still choose to stay on the page, so that's why i left it in.  
`tabnab [target link] [wait time in seconds]` - redirects the page to the specified link after a speified time of inaactivity  
`execute-ax [command]` - Execute a command with ActiveX only on window and IE
