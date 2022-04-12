/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : BotPlayer.cs
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static TPI_2020_MonoBattle.Constants;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Derived class of LocalPlayer that place his ships by himself and emulate a player shooting 
    /// </summary>
    internal class BotPlayer : LocalPlayer
    {
        #region Fields
        /// <summary>
        /// List of all the points already tried by the bot,
        /// used so he doesn't shoot twice at the same place
        /// </summary>
        private List<Point> _positionsAlreadyTested;
        #endregion
        #region Constructor
        /// <summary>
        /// Derived class of LocalPlayer that place his ships by himself and emulate a player shooting 
        /// </summary>
        /// <param name="playerGrid">Grid where the bot plays</param>
        /// <param name="ships">Ships that the bot places</param>
        internal BotPlayer(Grid playerGrid, List<Ship> ships) : base(playerGrid, ships)
        {
            _positionsAlreadyTested = new List<Point>();
            // Place ships randomly on the grid
            PlaceShipsRandom();
            ValidateShips(true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a random point in grid limits 
        /// that hasn't already been returned with this function
        /// </summary>
        public Point Shoot()
        {
            Random rnd = new Random();
            Point pointToTest;
            // As long as we don't find a unused point, we loop
            do
            {
                pointToTest = new Point(rnd.Next(0, NB_SQUARE_ROW), rnd.Next(0, NB_SQUARE_ROW));
            } while (_positionsAlreadyTested.Contains(pointToTest));
            
            _positionsAlreadyTested.Add(pointToTest);
            return pointToTest;
        }
        #endregion
    }
}
