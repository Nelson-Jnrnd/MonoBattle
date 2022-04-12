/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : FrmConnection.cs
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// This is the forms that opens up when you launch the application.
    /// She let the user enter the IP address of the other player and launch the game
    /// </summary>
    public partial class frmConnection : Form
    {
        #region Fields
        /// <summary>
        /// Determine if the application will be launched when this forms closes.
        /// </summary>
        private bool _needlaunchApplication;
        /// <summary>
        /// Delegate for the LaunchGame Method
        /// </summary>
        private delegate void SafeClosingDelegate();
        #endregion
        #region Properties
        /// <summary>
        /// Determine if the application will be launched when this forms closes.
        /// </summary>
        public bool NeedlaunchApplication { get => _needlaunchApplication; }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor of the setting form : Initialize the variables and start the local server.
        /// </summary>
        public frmConnection()
        {
            InitializeComponent();
            // By default the game doesn't start when the form closes
            _needlaunchApplication = false;
            // Start the local server to receive game request
            NetworkManager.GetInstance().StartServer();
            // Add the FormRequest event from the NetworkManager Class, it's called everytime the server receive a game request.
            NetworkManager.GetInstance().ReceivedInvite += SettingMenuForm_ReceivedInvite;
            // Display the local ip in a textbox for the user
            tbxSelfIp.Text = NetworkManager.GetLocalIPAddress().ToString();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sends a connection request to the server at the given IP, if the connection is accepted the game will be launched.
        /// Otherwise a corresponding error message is displayed.
        /// </summary>
        /// <param name="ipPeer"></param>
        private void SendRequest(string ipPeer)
        {
            try
            {
                // Parse the entered adress and sends a connection request message to it.
                // This if block is executed only if the adress reply positivly to the request.
                if (NetworkManager.GetInstance().SendConnectionRequest(IPAddress.Parse(ipPeer)))
                {
                    // Protecting from the issue where the player enters his own adress
                    if (ipPeer != NetworkManager.GetLocalIPAddress().ToString())
                    {
                        GameManager.IsFirstPlayer = true;
                        LaunchGame();
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                else
                {
                    MessageBox.Show("The IP did not accept the request", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a correct IP address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SocketException)
            {
                MessageBox.Show("We couldn't connect to the IP", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// When called, this methods prompt the user about the game starting before forcing the form to close.
        /// </summary>
        private void LaunchGame()
        {
            // When accessing windows form properties from another thread you need to make it safe
            // Source : https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/how-to-make-thread-safe-calls-to-windows-forms-controls?redirectedfrom=MSDN
            if (InvokeRequired)
            {
                // Call this method from the current thread instead of the NetworkManager's thread
                Invoke(new SafeClosingDelegate(LaunchGame));
            }
            else
            {
                MessageBox.Show("The game will begin now", "Game is starting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // By setting this to true the game will start when the form closes.
                _needlaunchApplication = true;
                Close();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Event called when the button sendRequest is clicked, it sends a request to the given ip adress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendRequest_Click(object sender, EventArgs e)
        {
            SendRequest(tbxIpAddressOpponent.Text);
        }
        /// <summary>
        /// Starts the game in local with the bot activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnectLocal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The game will begin now", "Game is starting", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // By setting this to true the game will start when the form closes.
            _needlaunchApplication = true;
            GameManager.IsBotGame = true;
            GameManager.IsFirstPlayer = true;
            Close();
        }
        /// <summary>
        /// Called when the NetworkManager triggers a ReceivedInvite action (when he receive a request response).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="result">The result of the request, we only launch the game if the result is true</param>
        private void SettingMenuForm_ReceivedInvite(object sender, bool result)
        {
            // If true forms need to launch application
            if (result)
            {
                LaunchGame();
            }
        }
        #endregion
    }
}
