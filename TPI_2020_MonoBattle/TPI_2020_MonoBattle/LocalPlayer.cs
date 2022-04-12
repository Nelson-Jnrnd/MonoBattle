/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : LocalPlayer.cs
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static TPI_2020_MonoBattle.Constants;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Class that represent the player that is playing on the machine the app is running.
    /// </summary>
    public class LocalPlayer : Player
    {
        #region Fields
        /// <summary>
        /// All the ships the player have
        /// </summary>
        private List<Ship> _ships;
        #endregion

        #region Properties
        internal List<Ship> Ships { get => _ships; set => _ships = value; }
        #endregion
        #region Constructor
        /// <summary>
        /// represent the player that is playing on the machine the app is running.
        /// </summary>
        /// <param name="playerGrid">Grid where the player place his boats</param>
        /// <param name="ships">Ships that the palyer places</param>
        public LocalPlayer(Grid playerGrid, List<Ship> ships) : base(playerGrid)
        {
            _ships = ships;
        }
        #endregion

        #region Methods
        /// <summary>
        /// The opponent player shot at the given location.
        /// Looks at the square at this location and return the state of it.
        /// Marks the square as shot by opponent on the grid.
        /// And if a ship is shot check if it has sunk
        /// </summary>
        /// <param name="location">Coordiantes the opponent shot</param>
        /// <returns></returns>
        public string ReceiveShot(Point location)
        {
            try
            {
                if (_playerGrid.SquareGrid[location.X, location.Y] == Grid.SquareType.ship)
                {
                    // If it hit a ship we search which on got hit and adjust his number of time hit.
                    // If it has too many he sinks.
                    foreach (Ship ship in _ships)
                    {
                        if (ship.SquareOccupied.Contains(location))
                        {
                            ship.NbTimeHit++;
                            if (ship.IsSunk)
                            {
                                if (_ships.Find(s => s.IsSunk == false) == null)
                                {
                                    return GAME_FINISHED_MESSAGE;
                                }
                                // We build a string containing the positions of the other squares that contains the ship
                                // That way, the opponent can marks the whole ship as sunk on his side.
                                string positionsString = TYPE_SEPARATOR;
                                foreach (Point squareOccupiedLocation in ship.SquareOccupied)
                                {
                                    _playerGrid.SquareGrid[squareOccupiedLocation.X, squareOccupiedLocation.Y] = Grid.SquareType.sunk;
                                    positionsString += ";" + squareOccupiedLocation.X + "," + squareOccupiedLocation.Y;
                                }
                                return SINK_MESSAGE + positionsString;
                            }
                        }
                    }
                    _playerGrid.SquareGrid[location.X, location.Y] = Grid.SquareType.hit;
                    return HIT_MESSAGE;
                }
                else if (_playerGrid.SquareGrid[location.X, location.Y] == Grid.SquareType.empty)
                {
                    _playerGrid.SquareGrid[location.X, location.Y] = Grid.SquareType.miss;
                    return MISS_MESSAGE;
                }
                return ERROR_MESSAGE;
            }
            catch (Exception)
            {
                return ERROR_MESSAGE;
            }  
        }

        /// <summary>
        /// Check if the ships are correctly placed on the grid
        /// </summary>
        /// <param name="imprintOnGrid">If true change the status of cells as ships</param>
        /// <returns></returns>
        public bool ValidateShips(bool imprintOnGrid)
        {
            // List of all the squares already occupied by ships
            List<Point> squareAlreadyOccupied = new List<Point>();

            foreach (Ship ship in _ships)
            {
                try
                {
                    Point shipHead = _playerGrid.FindSquare(ship.Position);
                    // Determines the coordinates of the squares of the rest of the ship
                    for (int i = 0; i < ship.Length; i++)
                    {
                        Point squareInShip = new Point();
                        if (ship.IsVertical)
                        {
                            squareInShip = new Point(shipHead.X, shipHead.Y + i);
                        }
                        else
                        {
                            squareInShip = new Point(shipHead.X + i, shipHead.Y);
                        }
                        if (squareInShip.X >= NB_SQUARE_ROW || squareInShip.Y >= NB_SQUARE_ROW)
                        {
                            return false;
                        }
                        if (squareAlreadyOccupied.Contains(squareInShip))
                        {
                            return false;
                        }
                        squareAlreadyOccupied.Add(squareInShip);
                        ship.SquareOccupied[i] = squareInShip;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // A ship is out of the grid
                    return false;
                }
            }
            if (imprintOnGrid)
            {
                foreach (Point squarePosition in squareAlreadyOccupied)
                {
                    _playerGrid.SquareGrid[squarePosition.X, squarePosition.Y] = Grid.SquareType.ship;
                }
            }

            return true;
        }
        /// <summary>
        /// Place the ship randomly
        /// </summary>
        public void PlaceShipsRandom()
        {
            // As long as the ship placement is not correct we replace the ships
            do
            {
                Random rnd = new Random();
                foreach (Ship ship in _ships)
                {
                    if (rnd.Next(2) == 0)
                    {
                        ship.Rotate();
                    }
                    ship.Position = _playerGrid.FindClosestSquareAnchor(new Point(
                        rnd.Next(_playerGrid.AnchorPoint.X, _playerGrid.AnchorPoint.X + ( NB_SQUARE_ROW * SQUARE_SIZE )),
                        rnd.Next(_playerGrid.AnchorPoint.Y, _playerGrid.AnchorPoint.Y + ( NB_SQUARE_ROW * SQUARE_SIZE ))));
                }
            } while (!ValidateShips(false));
        }
        #endregion
    }
}