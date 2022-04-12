/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : OpponentPlayer.cs
 */
using Microsoft.Xna.Framework;
using System;
using static TPI_2020_MonoBattle.Constants;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Class derived of player that represent the opponent of the local player
    /// </summary>
    public class OpponentPlayer : Player
    {
        #region Constructor
        public OpponentPlayer(Grid playerGrid) : base(playerGrid)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update the grid with the shot result given in parameter
        /// </summary>
        /// <param name="location"></param>
        /// <param name="shotResult"></param>
        public void ReceiveShot(Point location, string shotResult)
        {
            if (shotResult.Contains(SINK_MESSAGE))
            {
                string data = shotResult.Split(new string[] { TYPE_SEPARATOR}, StringSplitOptions.RemoveEmptyEntries)[1];
                string[] positions = data.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string position in positions)
                {
                    string[] positionXY = position.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    _playerGrid.SquareGrid[Convert.ToInt32(positionXY[0]), Convert.ToInt32(positionXY[1])] = Grid.SquareType.sunk;
                }
            }
            switch (shotResult)
            {
                case HIT_MESSAGE:
                    _playerGrid.SquareGrid[location.X, location.Y] = Grid.SquareType.hit;
                    break;
                case MISS_MESSAGE:
                    _playerGrid.SquareGrid[location.X, location.Y] = Grid.SquareType.miss;
                    break;
            }
        }
        #endregion
    }
}