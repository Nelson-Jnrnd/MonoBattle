/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : TextRect.cs
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Derived class of displayRect that let the user insert text
    /// </summary>
    class TextRect : DisplayedRect
    {
        #region Fields
        /// <summary>
        /// The text to display
        /// </summary>
        private string _displayText;
        /// <summary>
        /// The font used to display the text
        /// </summary>
        private SpriteFont _font;
        /// <summary>
        /// The color of the text displayed
        /// </summary>
        private Color _textColor;
        #endregion

        #region Properties
        /// <summary>
        /// The text to display
        /// </summary>
        public string DisplayText { get => _displayText; set => _displayText = value; }
        #endregion
        
        #region Constructor
        public TextRect(Rectangle innerRectangle, int borderThickness, Color innerColor, Color borderColor, string displayText, SpriteFont font, Color textColor)
            : base( innerRectangle, borderThickness, innerColor, borderColor)
        {
            _displayText = displayText;
            _font = font;
            _textColor = textColor;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draws the square on the spriteBatch with the text in the middle
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // We determine the position of the text using vectors
            spriteBatch.DrawString(_font, _displayText, InnerRectangle.Location.ToVector2() + InnerRectangle.Size.ToVector2() / 2 - _font.MeasureString(_displayText) / 2, _textColor);
        }

        #endregion
    }
}
