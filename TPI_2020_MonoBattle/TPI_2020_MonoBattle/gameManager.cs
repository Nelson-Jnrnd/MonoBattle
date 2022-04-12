/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 25.05.2020
 * 
 * File         : GameManager.cs
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static TPI_2020_MonoBattle.Constants;
// We rename it because Forms is ambiguous XNA.Framework
using WF = System.Windows.Forms;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// The main class of the project, it gather the inputs of the player to transfer it to the classes.
    /// It's also the only class that can communicate with the NetworkManager.
    /// </summary>
    public class GameManager : Game
    {
        /// <summary>
        /// The different phases of the game
        /// </summary>
        public enum Phase
        {
            shipsPlacement,
            turnLocalPlayer,
            turnOpponent,
            endGame
        }

        #region Fields
        /// <summary>
        /// True if the local player plays first
        /// </summary>
        public static bool IsFirstPlayer = false;
        /// <summary>
        /// True if the game is local with a bot
        /// </summary>
        public static bool IsBotGame = false;
        /// <summary>
        /// Texture used for all the UI elements
        /// </summary>
        public static Texture2D MainTexture;
        /// <summary>
        /// Represent the phase the game is currently in.
        /// We use this variable in the update and draw loop to determine actions to take.
        /// </summary>
        private Phase _currentPhase;
        /// <summary>
        /// Is used to draw sprites and text onto the game screen
        /// </summary>
        private SpriteBatch _spriteBatch;
        /// <summary>
        /// The font used for the UI.
        /// </summary>
        private SpriteFont _interfaceFont;
        /// <summary>
        /// A box on top of the screen to indicate which player turn it is.
        /// </summary>
        private TextRect _turnIndicator;
        /// <summary>
        /// Button on top of the screen to valdiate the placement of the ships
        /// If they are not all placed, or missplaced the user is told to retry.
        /// Otherwise it ends the ship placement phase.
        /// </summary>
        private TextRect _btnValidateShip;
        /// <summary>
        /// Button on the top left of the screen.
        /// It's used to end the game at any moment.
        /// </summary>
        private TextRect _btnSurrender;
        /// <summary>
        /// Button on the top right of the screen during the ship placement phase.
        /// It places the ship on the grid randomly
        /// </summary>
        private TextRect _btnRandomPlacement;
        private TextRect _tooltipHit;
        private TextRect _tooltipMiss;
        private TextRect _tooltipSink;
        /// <summary>
        /// Represents the player that is on the computer running this application
        /// </summary>
        private LocalPlayer _localPlayer;
        /// <summary>
        /// Represent the other player, be it an AI or a real human.
        /// </summary>
        private OpponentPlayer _opponent;
        /// <summary>
        /// The bot used to replace the opponent decision making in a bot game
        /// Is null when IsBotGame is false
        /// </summary>
        private BotPlayer _botPlayer;
        /// <summary>
        /// This variable is used in the ship placement phase.
        /// It makes it so only one ship can be moved at a time to stop them from pilling up.
        /// </summary>
        private Ship _selectedShip = null;
        /// <summary>
        /// The state of the mouse, we keep this global to remember the last state beyond one update.
        /// </summary>
        private MouseState _mouse;
        /// <summary>
        /// True if the local player won the game
        /// </summary>
        private bool _localPlayerWonGame;
        #endregion

        #region Constructors
        /// <summary>
        /// Create the GameManager and initialize some necessary elements of the game
        /// </summary>
        public GameManager()
        {
            // Name of the folder with all the images and fonts
            Content.RootDirectory = "Content";

            // Apply some changes to the monogame window (name, visible mouse, resolution...)
            Window.Title = DISPLAY_NAME;
            IsMouseVisible = true;
            GraphicsDeviceManager _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = SCREEN_SIZE_HEIGHT;
            _graphics.PreferredBackBufferWidth = SCREEN_SIZE_WIDTH;
            _graphics.ApplyChanges();

            NetworkManager.GetInstance().GameInstance = this;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialization of monogame variables
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Initialize the variables for the start of the game.
            _currentPhase = Phase.shipsPlacement;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MainTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
            MainTexture.SetData(new[] { Color.White });
            _localPlayerWonGame = false;
            // We create all the boats for the placement phase and asign them to the player
            List<Ship> _boats = new List<Ship>() {
                new Ship(new Point(600, 400), 2),
                new Ship(new Point(700, 400), 3),
                new Ship(new Point(800, 400), 3),
                new Ship(new Point(900, 400), 4),
                new Ship(new Point(1000, 400), 5),
            };

            _localPlayer = new LocalPlayer(new Grid(new Point(PLAYER_GRID_X, PLAYER_GRID_Y)), _boats);
            _opponent = new OpponentPlayer(new Grid(new Point(OPPONENT_GRID_X, OPPONENT_GRID_Y)));
            if (IsBotGame)
            {
                List<Ship> _botBoats = new List<Ship>() {
                new Ship(new Point(600, 400), 2),
                new Ship(new Point(700, 400), 3),
                new Ship(new Point(800, 400), 3),
                new Ship(new Point(900, 400), 4),
                new Ship(new Point(1000, 400), 5),
                };
                _botPlayer = new BotPlayer(new Grid(new Point(OPPONENT_GRID_X, OPPONENT_GRID_Y)), _botBoats);
            }

            // Initialisation of all the UI elements
            _interfaceFont = Content.Load<SpriteFont>("UiFont");
            _turnIndicator = new TextRect(new Rectangle(TURN_INDICATOR_X, TURN_INDICATOR_Y, TURN_INDICATOR_WIDTH, TURN_INDICATOR_HEIGHT),
                TURN_INDICATOR_BORDER_WIDTH, TURN_INDICATOR_LOCAL_TURN_BACKCOLOR, TURN_INDICATOR_BORDER_COLOR,
                TURN_INDICATOR_LOCAL_TURN_TEXT, _interfaceFont, TURN_INDICATOR_TEXT_COLOR
                );
            _btnValidateShip = new TextRect(new Rectangle(BTN_VALIDATE_X, BTN_VALIDATE_Y, BTN_VALIDATE_WIDTH, BTN_VALIDATE_HEIGHT), BTN_VALIDATE_BORDER_WIDTH,
                BTN_VALIDATE_BACKCOLOR, BTN_VALIDATE_BORDER_COLOR, BTN_VALIDATE_TEXT, _interfaceFont, BTN_VALIDATE_TEXT_COLOR
                );
            _btnSurrender = new TextRect(new Rectangle(BTN_SURRENDER_X, BTN_SURRENDER_Y, BTN_SURRENDER_WIDTH, BTN_SURRENDER_HEIGHT), BTN_SURRENDER_BORDER_WIDTH,
                BTN_SURRENDER_BACKCOLOR, BTN_SURRENDER_BORDER_COLOR, BTN_SURRENDER_TEXT, _interfaceFont, BTN_SURRENDER_TEXT_COLOR
                );
            _btnRandomPlacement = new TextRect(new Rectangle(BTN_RANDOM_X, BTN_RANDOM_Y, BTN_RANDOM_WIDTH, BTN_RANDOM_HEIGHT), BTN_RANDOM_BORDER_WIDTH,
                BTN_RANDOM_BACKCOLOR, BTN_RANDOM_BORDER_COLOR, BTN_RANDOM_TEXT, _interfaceFont, BTN_RANDOM_TEXT_COLOR
                );
            _tooltipHit = new TextRect(new Rectangle(TOOLTIP_HIT_X, TOOLTIP_Y, TOOLTIP_WIDTH, TOOLTIP_HEIGHT), TOOLTIP_BORDER_WIDTH,
                TOOLTIP_HIT_BACKCOLOR, TOOLTIP_BORDER_COLOR, TOOLTIP_HIT_TEXT, _interfaceFont, TOOLTIP_TEXT_COLOR);
            _tooltipMiss = new TextRect(new Rectangle(TOOLTIP_MISS_X, TOOLTIP_Y, TOOLTIP_WIDTH, TOOLTIP_HEIGHT), TOOLTIP_BORDER_WIDTH,
                TOOLTIP_MISS_BACKCOLOR, TOOLTIP_BORDER_COLOR, TOOLTIP_MISS_TEXT, _interfaceFont, TOOLTIP_TEXT_COLOR);
            _tooltipSink = new TextRect(new Rectangle(TOOLTIP_SINK_X, TOOLTIP_Y, TOOLTIP_WIDTH, TOOLTIP_HEIGHT), TOOLTIP_BORDER_WIDTH,
                TOOLTIP_SINK_BACKCOLOR, TOOLTIP_BORDER_COLOR, TOOLTIP_SINK_TEXT, _interfaceFont, TOOLTIP_TEXT_COLOR); 
            _mouse = Mouse.GetState();
        }
        /// <summary>
        /// Main loop of the application. It's constantly called to check for inputs.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Keeping track of the old state for flank detection
            MouseState oldMouseState = _mouse;
            _mouse = Mouse.GetState();
            // Check if the surrender button is clicked
            if (_btnSurrender.IntersectWith(_mouse.Position) && _mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                // Prompt the user and ask for confirmation
                if (WF.MessageBox.Show("Are you sure you want to surrender ?", "Confirmation", WF.MessageBoxButtons.YesNo, WF.MessageBoxIcon.Warning) == WF.DialogResult.Yes)
                {
                    try
                    {
                        // Send the message that we surrendered to the opponent
                        NetworkManager.GetInstance().SendEndGameMessage();
                        EndGame(false);
                    }
                    catch (NullReferenceException)
                    {
                        WF.MessageBox.Show("Connection to opponent lost, game is closing now", "Connection error", WF.MessageBoxButtons.OK, WF.MessageBoxIcon.Warning);
                        Exit();
                    }

                }
            }
            // Different actions depending on the game phase
            switch (_currentPhase)
            {
                case Phase.shipsPlacement:
                    // If this is the first frame of the mouse being pressed
                    if (_mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        // We check if the buttons are hovered by the mouse, if so they are clicked.
                        if (_btnValidateShip.IntersectWith(_mouse.Position))
                        {
                            // We check the position of all the ships, if they are correct we start the game
                            if (_localPlayer.ValidateShips(true))
                            {
                                if (IsFirstPlayer)
                                {
                                    _currentPhase = Phase.turnLocalPlayer;
                                }
                                else
                                {
                                    _currentPhase = Phase.turnOpponent;
                                }
                            }
                            else
                            {
                                WF.MessageBox.Show("Not all ship are correctly placed on the grid", "Ship placement error", WF.MessageBoxButtons.OK, WF.MessageBoxIcon.Error);
                            }
                        }
                        if (_btnRandomPlacement.IntersectWith(_mouse.Position))
                        {
                            try
                            {
                                _localPlayer.PlaceShipsRandom();
                            }
                            catch (StackOverflowException)
                            {
                                WF.MessageBox.Show("Error", "No possible configuration");
                            }

                        }

                        // The ship that is hovered by the mouse is the one we want to interact with
                        foreach (Ship boat in _localPlayer.Ships)
                        {
                            if (boat.IntersectWith(_mouse.Position))
                            {
                                _selectedShip = boat;
                            }
                        }
                    }
                    // We reset the selected ship only if the mouse click is not held.
                    // It stops the game from selecting other ships while draging moving them around
                    // And it let move the ships in a top left direction, where they are not hovered by the mouse.
                    else if (_mouse.LeftButton == ButtonState.Released && _selectedShip != null)
                    {
                        // Once the ship is dropped we try to align it with the grid if possible
                        _selectedShip.Position = _localPlayer.PlayerGrid.FindClosestSquareAnchor(_selectedShip.Position);
                        _selectedShip = null;
                    }

                    // If a ship is held we can rotate it and it follows the mouse movements
                    if (_selectedShip != null)
                    {
                        _selectedShip.Position = _mouse.Position;
                        if (_mouse.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                        {
                            _selectedShip.Rotate();
                        }
                    }
                    break;
                case Phase.turnLocalPlayer:
                    // If this is the first frame of the mouse being pressed
                    if (_mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        try
                        {
                            Shoot(_opponent.PlayerGrid.FindSquare(_mouse.Position));
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            // Means the user clicked outside of the grid, it's not an issue so we don't prompt the user
                        }
                    }
                    break;
                case Phase.turnOpponent:
                    // If the game is solo, the bot plays otherwhise we wait for the opponent to respond
                    if (IsBotGame)
                    {
                        ReceiveShot(_botPlayer.Shoot());
                    }
                    break;
                case Phase.endGame:
                    // We show the correct end of game message
                    if (_localPlayerWonGame)
                    {
                        WF.MessageBox.Show("You destroyed the ennemy fleet !", "Victory !", WF.MessageBoxButtons.OK, WF.MessageBoxIcon.Information);
                        Exit();
                    }
                    else
                    {
                        WF.MessageBox.Show("All your ships have sunk !", "Defeat !", WF.MessageBoxButtons.OK, WF.MessageBoxIcon.Information);
                        Exit();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clears the last frame
            GraphicsDevice.Clear(BACKGROUND_COLOR);

            // Open the spriteBatch to draw all the sprites at the same time
            _spriteBatch.Begin();
            _btnSurrender.Draw(_spriteBatch);

            // Draw the scene depending on the phase
            switch (_currentPhase)
            {
                // We draw only the local player grid, his boats and the buttons
                case Phase.shipsPlacement:
                    _localPlayer.PlayerGrid.Draw(_spriteBatch);
                    foreach (Ship boat in _localPlayer.Ships)
                    {
                        boat.Draw(_spriteBatch);
                    }
                    _btnValidateShip.Draw(_spriteBatch);
                    _btnRandomPlacement.Draw(_spriteBatch);
                    break;
                case Phase.turnLocalPlayer:
                    _localPlayer.PlayerGrid.Draw(_spriteBatch);
                    _opponent.PlayerGrid.Draw(_spriteBatch);
                    // We change the colors/ text of the turn indicator.
                    if (_turnIndicator.DisplayText != TURN_INDICATOR_LOCAL_TURN_TEXT)
                    {
                        _turnIndicator.DisplayText = TURN_INDICATOR_LOCAL_TURN_TEXT;
                        _turnIndicator.InnerColor = TURN_INDICATOR_LOCAL_TURN_BACKCOLOR;

                    }
                    _turnIndicator.Draw(_spriteBatch);
                    _tooltipHit.Draw(_spriteBatch);
                    _tooltipMiss.Draw(_spriteBatch);
                    _tooltipSink.Draw(_spriteBatch);
                    break;
                case Phase.turnOpponent:
                    _localPlayer.PlayerGrid.Draw(_spriteBatch);
                    _opponent.PlayerGrid.Draw(_spriteBatch);
                    if (_turnIndicator.DisplayText != TURN_INDICATOR_OPPONENT_TURN_TEXT)
                    {
                        _turnIndicator.DisplayText = TURN_INDICATOR_OPPONENT_TURN_TEXT;
                        _turnIndicator.InnerColor = TURN_INDICATOR_OPPONENT_TURN_BACKCOLOR;

                    }
                    _turnIndicator.Draw(_spriteBatch);
                    _tooltipHit.Draw(_spriteBatch);
                    _tooltipMiss.Draw(_spriteBatch);
                    _tooltipSink.Draw(_spriteBatch);
                    break;
                case Phase.endGame:
                    _localPlayer.PlayerGrid.Draw(_spriteBatch);
                    _opponent.PlayerGrid.Draw(_spriteBatch);
                    _turnIndicator.Draw(_spriteBatch);
                    _tooltipHit.Draw(_spriteBatch);
                    _tooltipMiss.Draw(_spriteBatch);
                    _tooltipSink.Draw(_spriteBatch);
                    break;
            }
            // Close the sprite batch to draw all the sprites on the screen
            _spriteBatch.End();

        }
        #endregion

        /// <summary>
        /// Sends the information to the opponent that we guess the point in parameter.
        /// If the game is in solo Mode we send the data to the bot,
        /// otherwise we use the NetworkManager to send the data to the other application.
        /// Then, depending on the response, we update the opponent grid.
        /// </summary>
        /// <param name="location">The point we want to guess</param>
        private void Shoot(Point location)
        {
            if (_currentPhase == Phase.turnLocalPlayer)
            {
                // shotResult is what the cell contains : Ship, Nothing, Last part of a ship...
                string shotResult = "";
                if (IsBotGame)
                {
                    shotResult = _botPlayer.ReceiveShot(location);
                }
                else
                {
                    try
                    {
                        shotResult = NetworkManager.GetInstance().SendShootMessage(location);
                    }
                    catch (NullReferenceException)
                    {
                        WF.MessageBox.Show("Connection to opponent lost, game is closing now", "Connection error", WF.MessageBoxButtons.OK, WF.MessageBoxIcon.Warning);
                        Exit();
                    }
                }

                if (shotResult.Contains(GAME_FINISHED_MESSAGE))
                {
                    EndGame(true);
                }
                else if (!shotResult.Contains(ERROR_MESSAGE))
                {
                    // We apply the changes to the local grid and pass on the next turn
                    _opponent.ReceiveShot(location, shotResult);
                    NextTurn();
                }
            }
        }
        /// <summary>
        /// When the local player receive a shot guess from the opponent.
        /// We apply the shot to the display grid and return the result
        /// </summary>
        /// <param name="location">Coordinates of the guess</param>
        /// <returns>Result of the shot, see Constant.cs</returns>
        public string ReceiveShot(Point location)
        {
            // If it's not the opponent turns, he shouldn't send shots
            if (_currentPhase == Phase.turnOpponent)
            {
                string result = _localPlayer.ReceiveShot(location);
                if (result.Contains(GAME_FINISHED_MESSAGE))
                {
                    EndGame(false);
                    return GAME_FINISHED_MESSAGE;
                }
                else if (!result.Contains(ERROR_MESSAGE))
                {
                    NextTurn();
                }
                return result;
            }
            return ERROR_MESSAGE;
        }
        /// <summary>
        /// Change the phase to the next turn
        /// </summary>
        public void NextTurn()
        {
            if (_currentPhase == Phase.turnLocalPlayer)
            {
                _currentPhase = Phase.turnOpponent;
            }
            else
            {
                _currentPhase = Phase.turnLocalPlayer;
            }
        }
        /// <summary>
        /// Enter the end game phase
        /// </summary>
        /// <param name="didLocalPlayerWin">true if the local player won</param>
        public void EndGame(bool didLocalPlayerWin)
        {
            _localPlayerWonGame = didLocalPlayerWin;
            _currentPhase = Phase.endGame;
        }
        #region Events
        /// <summary>
        /// We override the OnExit method of Game.cs to tell the other player we left the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            if (!IsBotGame)
            {
                try
                {
                    NetworkManager.GetInstance().SendEndGameMessage();
                }
                catch (NullReferenceException)
                {
                    // Opponent already left, no need to send a message
                }
            }
            base.OnExiting(sender, args);
        }
        #endregion
    }
}
