/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : DisplayedRect.cs
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Class that represent a rectangle that is drawable on screen.
    /// </summary>
    public class DisplayedRect
    {
        #region Fields
        /// <summary>
        /// Represent the rectangle that fills the inside space
        /// </summary>
        private Rectangle _innerRectangle;
        /// <summary>
        /// The 4 lines that create the borders
        /// </summary>
        private Rectangle[] _borders;
        /// <summary>
        /// The color inside the rectangle
        /// </summary>
        protected Color _innerColor;
        /// <summary>
        /// The color of the borders
        /// </summary>
        private Color _borderColor;
        /// <summary>
        /// The thickness in pixels of the borders
        /// </summary>
        private int _borderThickness;
        /// <summary>
        /// The texture in which the rectangle is drawn in
        /// </summary>
        private Texture2D _texture;
        #endregion

        #region Properties
        /// <summary>
        /// Represent the rectangle that fills the inside space
        /// Automatically recalculates the borders when changed
        /// </summary>
        protected virtual Rectangle InnerRectangle
        {
            get => _innerRectangle;
            set
            {
                _innerRectangle = value;
                _borders = CreateRectBorder(_innerRectangle);
            }
        }
        /// <summary>
        /// The color inside the rectangle
        /// </summary>
        public Color InnerColor { get => _innerColor; set => _innerColor = value; }
        #endregion
        #region Constructors
        /// <summary>
        /// A rectangle drawable on screen.
        /// </summary>
        /// <param name="texture">The texture in which the rectangle is drawn in</param>
        /// <param name="innerRectangle">Represent the rectangle that fills the inside space</param>
        /// <param name="borderThickness">The thickness in pixels of the borders</param>
        /// <param name="innerColor">The color inside the rectangle</param>
        /// <param name="borderColor">The color of the borders</param>
        public DisplayedRect(Rectangle innerRectangle, int borderThickness, Color innerColor, Color borderColor)
        {
            _texture = GameManager.MainTexture;
            InnerRectangle = innerRectangle;
            _innerColor = innerColor;
            _borderColor = borderColor;
            _borderThickness = borderThickness;

            _borders = CreateRectBorder(InnerRectangle);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draws the rectangle on the screen, 
        /// the spriteBatch must be started before calling this method
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, InnerRectangle, _innerColor);
            foreach (Rectangle border in _borders)
            {
                spriteBatch.Draw(_texture, border, _borderColor);
            }
        }
        /// <summary>
        /// Create the 4 borders depending on the rectangle given
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private Rectangle[] CreateRectBorder(Rectangle rect)
        {
            Rectangle[] borders = {
                new Rectangle(rect.Left, rect.Top, _borderThickness, rect.Height + _borderThickness),
                new Rectangle(rect.Right, rect.Top, _borderThickness, rect.Height + _borderThickness),
                new Rectangle(rect.Left, rect.Top, rect.Width + _borderThickness, _borderThickness),
                new Rectangle(rect.Left, rect.Bottom, rect.Width + _borderThickness, _borderThickness)
            };
            return borders;
        }
        /// <summary>
        /// Returns true if the given point is in the rectangle hitbox
        /// </summary>
        /// <param name="position">Point to test</param>
        public virtual bool IntersectWith(Point position)
        {
            return InnerRectangle.Contains(position);
        }
        #endregion
    }
}
