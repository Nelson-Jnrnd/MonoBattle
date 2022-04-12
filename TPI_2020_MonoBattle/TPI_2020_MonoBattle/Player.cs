/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : Player.cs
 */
namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Abstract Class that represent a player and contains his grid
    /// </summary>
    public abstract class Player
    {
        #region Fields
        protected Grid _playerGrid;
        #endregion
        #region Properties
        public Grid PlayerGrid { get => _playerGrid; }
        #endregion
        #region Constructors
        public Player(Grid playerGrid)
        {
            _playerGrid = playerGrid;
        }
        #endregion
    }
}