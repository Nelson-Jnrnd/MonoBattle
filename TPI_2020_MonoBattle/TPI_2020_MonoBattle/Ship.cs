/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : Ship.cs
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static TPI_2020_MonoBattle.Constants;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Class derived from DispalyedRect that represent a ship
    /// </summary>
    public class Ship : DisplayedRect
    {
        #region Fields
        /// <summary>
        /// The rectangle of the entire ship
        /// </summary>
        private Rectangle _hitbox;
        /// <summary>
        /// Length of the ship in squares
        /// </summary>
        private int _length;
        /// <summary>
        /// How the ship is positionned spatialy on the grid
        /// </summary>
        private bool _isVertical;
        /// <summary>
        /// The coordinates of all the square occupied by the ship
        /// </summary>
        private Point[] _squareOccupied;
        /// <summary>
        /// The number of time the ship has been hit by a player. 
        /// If this number is higher or equal than his length, the ship sink
        /// </summary>
        private int _nbTimeHit;
        #endregion
        #region Properties
        /// <summary>
        /// The "head" rectangle of the ship
        /// </summary>
        protected override Rectangle InnerRectangle
        {
            get => base.InnerRectangle;
            // We recalculate the hitbox everytime the base rectangle changes
            set
            {
                base.InnerRectangle = value;
                if (_isVertical)
                {
                    _hitbox = new Rectangle(InnerRectangle.X, InnerRectangle.Y, InnerRectangle.Width, InnerRectangle.Height * _length);
                }
                else
                {
                    _hitbox = new Rectangle(InnerRectangle.X, InnerRectangle.Y, InnerRectangle.Width * _length, InnerRectangle.Height);
                }
            }
        }
        /// <summary>
        /// The top left position of the ship
        /// </summary>
        public Point Position
        {
            get
            {
                return InnerRectangle.Location;
            }
            set
            {
                InnerRectangle = new Rectangle(value, InnerRectangle.Size);
            }
        }
        /// <summary>
        /// Returns if the ship has sunk already,
        /// if the nbOfTimeHit is higher or equal than his length, the ship sink
        /// </summary>
        public bool IsSunk
        {
            get
            {
                return _nbTimeHit >= _length;
            }
        }
        /// <summary>
        /// The coordinates of all the square occupied by the ship
        /// </summary>
        public Point[] SquareOccupied { get => _squareOccupied; set => _squareOccupied = value; }
        /// <summary>
        /// How the ship is positionned spatialy on the grid
        /// </summary>
        public bool IsVertical { get => _isVertical; }
        /// <summary>
        /// Length of the ship in squares
        /// </summary>
        public int Length { get => _length; }
        /// <summary>
        /// The number of time the ship has been hit by a player. 
        /// If this number is higher or equal than his length, the ship sink
        /// </summary>
        public int NbTimeHit { get => _nbTimeHit; set => _nbTimeHit = value; }
        #endregion
        #region Constructor
        public Ship(Rectangle innerRectangle, int borderThickness, Color innerColor, Color borderColor, int length) : base(innerRectangle, borderThickness, innerColor, borderColor)
        {
            _length = length;
            _hitbox = new Rectangle(InnerRectangle.X, InnerRectangle.Y, InnerRectangle.Width, InnerRectangle.Height * _length);
            _isVertical = true;
            _squareOccupied = new Point[_length];
            _nbTimeHit = 0;
        }
        public Ship(Point position, int length) :this(new Rectangle(position.X, position.Y, SQUARE_SIZE, SQUARE_SIZE), BORDER_WIDTH, SHIP_COLOR, Color.Black, length)
        {

        }
        #endregion
        #region Methods
        /// <summary>
        /// Draws the ship on the spriteBatch given in parameter
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // We loop through his length and determine the position of his body to draw each of the squares.
            Rectangle originalRectangle = InnerRectangle;
            base.Draw(spriteBatch);
            for (int i = 1; i < _length; i++)
            {
                if (_isVertical)
                {
                    InnerRectangle = new Rectangle(InnerRectangle.X, InnerRectangle.Y + InnerRectangle.Height, InnerRectangle.Width, InnerRectangle.Height);
                }
                else
                {
                    InnerRectangle = new Rectangle(InnerRectangle.X + InnerRectangle.Width, InnerRectangle.Y, InnerRectangle.Width, InnerRectangle.Height);
                }
                base.Draw(spriteBatch);
            }
            InnerRectangle = originalRectangle;
        }
        /// <summary>
        /// Returns true if the given point is in the ship hitbox
        /// </summary>
        /// <param name="position">Point to test</param>
        public override bool IntersectWith(Point position)
        {
            return _hitbox.Contains(position);
        }
        /// <summary>
        /// Change the rotation of the ship
        /// </summary>
        public void Rotate()
        {
            _isVertical = !_isVertical;
        }
        #endregion
    }
}