/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : NetworkManager.cs
 */
using Microsoft.Xna.Framework;
using SimpleTCP;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using static TPI_2020_MonoBattle.Constants;
// We rename it because Forms.Message is ambiguous with SimpleTCP.Message
using WF = System.Windows.Forms;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Singleton class that uses simpleTCP NuGet to connect the players
    /// </summary>
    public class NetworkManager
    {
        #region Fields
        private static NetworkManager _instance;

        /// <summary>
        /// Returns the ip of the user
        /// </summary>
        private IPAddress _localIp;
        /// <summary>
        /// Server that receive data from the other machine
        /// </summary>
        private SimpleTcpServer _server;
        /// <summary>
        /// Represents the other application, if it's null then there is no app connected.
        /// </summary>
        private SimpleTcpClient _client;
        /// <summary>
        /// If a client connected to the application
        /// </summary>
        private bool _clientConnected;
        #endregion
        #region Propriétés
        public GameManager GameInstance { get; set; }
        #endregion
        #region Events
        /// <summary>
        /// Event that is called whenever we reply to a game request
        /// It informs the local form about if it should close to launch the game
        /// </summary>
        public event EventHandler<bool> ReceivedInvite;
        #endregion

        /// <summary>
        /// Create a new instance of the class
        /// ! Only to be called from GetInstance !
        /// </summary>
        private NetworkManager()
        {
            _localIp = GetLocalIPAddress();
            _server = new SimpleTcpServer();
            _client = new SimpleTcpClient();
            _clientConnected = false;
        }

        /// <summary>
        /// Returns the single instance of the class
        /// </summary>
        public static NetworkManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NetworkManager();
            }
            return _instance;
        }
        /// <summary>
        /// Start the local server and initialize all the variables, Delimiter...
        /// </summary>
        public void StartServer()
        {
            if (!_server.IsStarted)
            {
                // Start the server and indicates which port he needs to listen on
                _server.Start(_localIp, CONNECTION_PORT);
                // Everytime the server receive a \n it calls the DelimiterDataReceived event.
                _server.Delimiter = 0x13;
                _server.DelimiterDataReceived += Server_DelimiterDataReceived;
            }
        }
        /// <summary>
        /// Called everytime the server receive 1 full data package (indicated by a \n),
        /// it paints the cell who's location correspond with the data received.
        /// </summary>
        private void Server_DelimiterDataReceived(object sender, Message e)
        {
            // The message received is formatted as such "TypeOfMessage" + Separator + "Data" + EndOfMessageSeparator
            // We split the messaged based on those separators
            // 0 is the type of the message and 1 is the data
            string[] messageSplited = e.MessageString.Split(new string[] { TYPE_SEPARATOR, MESSAGE_END_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            // We convert the string message received to a MessageType variable
            Enum.TryParse(messageSplited[0], out MESSAGE_TYPE messageType);

            switch (messageType)
            {
                // If we received a request we prompt the user about it then send back the response to the other app.
                case MESSAGE_TYPE.connectionRequest:
                    // In the case of a request the data is always the adress of the other computer
                    string ipAdress = messageSplited[1];
                    // We prompt the user about the message
                    // In case we receive a request while already in game
                    if (!_clientConnected)
                    {
                        if (WF.MessageBox.Show("You received a game request from " + ipAdress + " would you like to accept it ?", "Game request !", WF.MessageBoxButtons.YesNo, WF.MessageBoxIcon.Question) == WF.DialogResult.Yes)
                        {
                            // Since we want to be able to communicate with the other app in the future we create a communication canal
                            ConnectClient(ipAdress);
                            // Trigger the form event to warn the view about the result of the connection.
                            ReceivedInvite.Invoke(this, true);
                            // We send back the response 1 is for yes
                            GameManager.IsFirstPlayer = false;
                            e.Reply(ACCEPT_INVITE);
                        }
                        else
                        {
                            // Trigger the form event to warn the view about the result of the connection.
                            ReceivedInvite.Invoke(this, false);
                            // We send back the response 0 is for no
                            e.Reply(REFUSE_INVITE);
                        }
                    }
                    e.Reply(REFUSE_INVITE);
                    break;
                case MESSAGE_TYPE.shoot:
                    if (GameInstance != null)
                    {
                        // Separates the data
                        char[] separators = { ',' };
                        string[] stringPosition = messageSplited[1].Split(separators);
                        // Convert the positions from string to int so it can be used
                        int positionX = Convert.ToInt32(stringPosition[0]);
                        int positionY = Convert.ToInt32(stringPosition[1]);
                        e.Reply(GameInstance.ReceiveShot(new Point(positionX, positionY)));
                    }
                    break;
                case MESSAGE_TYPE.endgame:
                    GameInstance.EndGame(true);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Connects the user to set peer
        /// </summary>
        public void ConnectClient(string ip)
        {
            // We check the syntax of the ip and if it's correct create a TCP connection
            if (ValidateIPv4(ip.ToString()))
            {
                try
                {
                    _client.Connect(ip.ToString(), CONNECTION_PORT);
                    _clientConnected = true;
                }
                catch (SocketException)
                {
                    throw new SocketException(61);
                }
            }
        }
        /// <summary>
        /// Sends a request to the given Ip and return bool coresponding to the state of the result
        /// </summary>
        /// <param name="message"></param>
        public bool SendConnectionRequest(IPAddress ip)
        {
            try
            {
                ConnectClient(ip.ToString());
                if (_client.TcpClient.Connected)
                {
                    // Send a request type message to the ip with our ip so he can send us back a response.
                    // End the message with \n to indicate the end of the message.
                    Message reply = _client.WriteLineAndGetReply(MESSAGE_TYPE.connectionRequest +
                        TYPE_SEPARATOR + _localIp.ToString() + MESSAGE_END_SEPARATOR, TimeSpan.FromSeconds(20));
                    if (reply != null)
                    {
                        if (reply.MessageString == ACCEPT_INVITE)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (SocketException)
            {
                throw new SocketException(61);
            }

        }
        /// <summary>
        /// Sends a message to the peer to indicate the game's finished (and he won)
        /// </summary>
        public void SendEndGameMessage()
        {
            if (_client.TcpClient.Connected)
            {
                // End the message with \n to indicate the end of the message.
                try
                {
                    _client.WriteLine(MESSAGE_TYPE.endgame.ToString() +
                       TYPE_SEPARATOR + MESSAGE_END_SEPARATOR);
                }
                catch (System.IO.IOException)
                {
                    throw new NullReferenceException("Client is not connected");
                }

            }
            else
            {
                throw new NullReferenceException("Client is not connected");
            }
        }
        /// <summary>
        /// Sends a shot message at the coordiantes given in parameter
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public string SendShootMessage(Point coordinates)
        {
            if (_client.TcpClient.Connected)
            {
                // End the message with \n to indicate the end of the message.
                try
                {
                    Message reply = _client.WriteLineAndGetReply(MESSAGE_TYPE.shoot.ToString() +
                       TYPE_SEPARATOR + coordinates.X.ToString() + "," + coordinates.Y.ToString() + MESSAGE_END_SEPARATOR, TimeSpan.FromSeconds(20));

                    return reply.MessageString;
                }
                catch (System.IO.IOException)
                {
                    throw new NullReferenceException("Client is not connected");
                }

            }
            else
            {
                throw new NullReferenceException("Client is not connected");
            }
        }
        /// <summary>
        /// Returns the current IP adress of the machine
        /// From : https://stackoverflow.com/questions/6803073/get-local-ip-address
        /// </summary>
        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        /// <summary>
        /// We check the syntax of the IP adress, first if it contains whitespaces or is null, then if there is 4 parts and finaly we parse the 4 strings
        /// From : https://stackoverflow.com/questions/11412956/what-is-the-best-way-of-validating-an-ip-address
        /// </summary>
        /// <param name="ipString"></param>
        /// <returns></returns>
        public static bool ValidateIPv4(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
    }
}