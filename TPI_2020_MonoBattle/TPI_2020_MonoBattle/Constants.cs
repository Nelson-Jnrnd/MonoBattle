/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : Constants.cs
 */
using Microsoft.Xna.Framework;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Class containing all of the parameters for the game
    /// </summary>
    public static class Constants
    {
        // Window parameters
        public const string DISPLAY_NAME = "MonoBattle";
        public const int SCREEN_SIZE_WIDTH = 1280;
        public const int SCREEN_SIZE_HEIGHT = 720;
        public static Color BACKGROUND_COLOR = Color.DarkGray;

        // Grid parameters
        public const int NB_SQUARE_ROW = 10;
        public const int SQUARE_SIZE = 50;
        public const int BORDER_WIDTH = 2;

        public const int PLAYER_GRID_X = 10;
        public const int PLAYER_GRID_Y = 100;
        public const int OPPONENT_GRID_X = SCREEN_SIZE_WIDTH - (SQUARE_SIZE * NB_SQUARE_ROW) - 10;
        public const int OPPONENT_GRID_Y = 100;

        public static Color EMPTY_COLOR = Color.Blue;
        public static Color SHIP_COLOR = Color.LightGray;
        public static Color HIT_COLOR = Color.Red;
        public static Color MISS_COLOR = Color.Green;
        public static Color SUNK_COLOR = Color.DarkGray;
        public static Color GRID_BORDER_COLOR = Color.Black;

        // UI parameters

        public const int TOOLTIP_WIDTH = 80;
        public const int TOOLTIP_HEIGHT = 80;
        public const int TOOLTIP_Y = SCREEN_SIZE_HEIGHT - TOOLTIP_HEIGHT - 10;

        public const int TOOLTIP_BORDER_WIDTH = 3;
        public static Color TOOLTIP_BORDER_COLOR = Color.Black;
        public static Color TOOLTIP_TEXT_COLOR = Color.Black;

        public const string TOOLTIP_HIT_TEXT = "Hit";
        public static Color TOOLTIP_HIT_BACKCOLOR = HIT_COLOR;
        public const int TOOLTIP_HIT_X = 10;

        public const string TOOLTIP_SINK_TEXT = "Sunk";
        public static Color TOOLTIP_SINK_BACKCOLOR = SUNK_COLOR;
        public const int TOOLTIP_SINK_X = TOOLTIP_HIT_X + TOOLTIP_WIDTH + 10;

        public const string TOOLTIP_MISS_TEXT = "Miss";
        public static Color TOOLTIP_MISS_BACKCOLOR = MISS_COLOR;
        public const int TOOLTIP_MISS_X = TOOLTIP_SINK_X + TOOLTIP_WIDTH + 10;


        public const int TURN_INDICATOR_WIDTH = 200;
        public const int TURN_INDICATOR_HEIGHT = 50;
        public const int TURN_INDICATOR_X = SCREEN_SIZE_WIDTH / 2 - TURN_INDICATOR_WIDTH / 2;
        public const int TURN_INDICATOR_Y = 10;
        public const int TURN_INDICATOR_BORDER_WIDTH = 3;
        public static Color TURN_INDICATOR_BORDER_COLOR = Color.Black;
        public static Color TURN_INDICATOR_LOCAL_TURN_BACKCOLOR = Color.LightGreen;
        public static Color TURN_INDICATOR_OPPONENT_TURN_BACKCOLOR = Color.Red;
        public static Color TURN_INDICATOR_TEXT_COLOR = Color.Black;
        public const string TURN_INDICATOR_LOCAL_TURN_TEXT = "Your turn";
        public const string TURN_INDICATOR_OPPONENT_TURN_TEXT = "Opponent's turn";

        public const int BTN_VALIDATE_WIDTH = 300;
        public const int BTN_VALIDATE_HEIGHT = 50;
        public const int BTN_VALIDATE_X = SCREEN_SIZE_WIDTH / 2 - TURN_INDICATOR_WIDTH / 2;
        public const int BTN_VALIDATE_Y = 10;
        public const int BTN_VALIDATE_BORDER_WIDTH = 3;
        public static Color BTN_VALIDATE_BORDER_COLOR = Color.Black;
        public static Color BTN_VALIDATE_BACKCOLOR = Color.LightGreen;
        public static Color BTN_VALIDATE_TEXT_COLOR = Color.Black;
        public const string BTN_VALIDATE_TEXT = "Validate placements";

        public const int BTN_SURRENDER_WIDTH = 300;
        public const int BTN_SURRENDER_HEIGHT = 50;
        public const int BTN_SURRENDER_X = 10;
        public const int BTN_SURRENDER_Y = 10;
        public const int BTN_SURRENDER_BORDER_WIDTH = 3;
        public static Color BTN_SURRENDER_BORDER_COLOR = Color.Black;
        public static Color BTN_SURRENDER_BACKCOLOR = Color.LightGreen;
        public static Color BTN_SURRENDER_TEXT_COLOR = Color.Black;
        public const string BTN_SURRENDER_TEXT = "Surrender";

        public const int BTN_RANDOM_WIDTH = 300;
        public const int BTN_RANDOM_HEIGHT = 50;
        public const int BTN_RANDOM_X = SCREEN_SIZE_WIDTH - BTN_RANDOM_WIDTH - 10;
        public const int BTN_RANDOM_Y = 10;
        public const int BTN_RANDOM_BORDER_WIDTH = 3;
        public static Color BTN_RANDOM_BORDER_COLOR = Color.Black;
        public static Color BTN_RANDOM_BACKCOLOR = Color.LightGreen;
        public static Color BTN_RANDOM_TEXT_COLOR = Color.Black;
        public const string BTN_RANDOM_TEXT = "Place ships at random";

        // Network parameters
        public const int CONNECTION_PORT = 51340;
        public const string TYPE_SEPARATOR = "%";
        public const string MESSAGE_END_SEPARATOR = "#";
        public const string ACCEPT_INVITE = "1";
        public const string REFUSE_INVITE = "0";
        public const string HIT_MESSAGE = "hit";
        public const string MISS_MESSAGE = "miss";
        public const string SINK_MESSAGE = "sink";
        public const string GAME_FINISHED_MESSAGE = "finish";
        public const string ERROR_MESSAGE = "error";
        public enum MESSAGE_TYPE
        {
            connectionRequest,
            shoot,
            endgame
        }
    }
}