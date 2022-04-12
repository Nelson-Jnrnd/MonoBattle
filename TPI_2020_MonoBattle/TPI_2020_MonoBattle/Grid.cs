/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : Grid.cs
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static TPI_2020_MonoBattle.Constants;

namespace TPI_2020_MonoBattle
{
    /// <summary>
    /// Class that represent a grid used by a player to mark their ships and/or hits.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// Different possible square types
        /// </summary>
        public enum SquareType
        {
            empty,
            ship,
            hit,
            miss,
            sunk
        }
        #region Fields
        /// <summary>
        /// Array of square type that represent the status of all the squares
        /// </summary>
        private SquareType[,] _squareGrid;
        /// <summary>
        /// Top left point of the grid
        /// </summary>
        private Point _anchorPoint;
        #endregion
        #region Properties
        /// <summary>
        /// Top left point of the grid
        /// </summary>
        public Point AnchorPoint { get => _anchorPoint; set => _anchorPoint = value; }
        /// <summary>
        /// Array of square type that represent the status of all the squares
        /// </summary>
        public SquareType[,] SquareGrid { get => _squareGrid; set => _squareGrid = value; }
        #endregion
        #region Constructor
        /// <summary>
        /// Create a an empty grid at the given location
        /// </summary>
        /// <param name="texture">texture used to draw the grid</param>
        /// <param name="anchorPoint">Top left point of the grid</param>
        public Grid(Point anchorPoint)
        {
            _anchorPoint = anchorPoint;
            // Initialize the cells
            InitializeGrid();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Set squareGrid to a X by X array where all the squares are empty
        /// X being the nb of square rows in constants.cs
        /// </summary>
        private void InitializeGrid()
        {
            SquareGrid = new SquareType[NB_SQUARE_ROW, NB_SQUARE_ROW];

            for (int line = 0; line < NB_SQUARE_ROW; line++)
            {
                for (int row = 0; row < NB_SQUARE_ROW; row++)
                {
                    SquareGrid[line, row] = SquareType.empty;
                }
            }
        }
        /// <summary>
        /// Draws the entire grid on the spriteBatch given in parameter
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int line = 0; line < NB_SQUARE_ROW; line++)
            {
                for (int row = 0; row < NB_SQUARE_ROW; row++)
                {
                    Color squareColor = new Color();
                    // Foreach squares we fill it with the right color for it's status
                    switch (SquareGrid[row, line])
                    {
                        case SquareType.empty:
                            squareColor = EMPTY_COLOR;
                            break;
                        case SquareType.ship:
                            squareColor = SHIP_COLOR;
                            break;
                        case SquareType.hit:
                            squareColor = HIT_COLOR;
                            break;
                        case SquareType.miss:
                            squareColor = MISS_COLOR;
                            break;
                        case SquareType.sunk:
                            squareColor = SUNK_COLOR;
                            break;
                    }
                    Color borderColor = GRID_BORDER_COLOR;

                    Rectangle innerRectangle = new Rectangle(row * SQUARE_SIZE, line * SQUARE_SIZE, SQUARE_SIZE, SQUARE_SIZE);
                    innerRectangle.Location += _anchorPoint;

                    new DisplayedRect(innerRectangle, BORDER_WIDTH, squareColor, borderColor).Draw(spriteBatch);
                }
            }
        }
        /// <summary>
        /// Finds the top left coordinates of the square in which the point is in
        /// </summary>
        /// <param name="inputPoint">The input point</param>
        /// <returns></returns>
        public Point FindClosestSquareAnchor(Point inputPoint)
        {
            try
            {
                Point SquareCoordinates = FindSquare(inputPoint);

                Point anchorPoint = new Point(SquareCoordinates.X * SQUARE_SIZE + AnchorPoint.X, SquareCoordinates.Y * SQUARE_SIZE + AnchorPoint.Y);
                return anchorPoint;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Point is not in grid
                return inputPoint;
            }

        }
        /// <summary>
        /// Find the square that contains the inputed point
        /// </summary>
        /// <param name="pointInSquare"></param>
        /// <returns></returns>
        public Point FindSquare(Point pointInSquare)
        {
            // We find the coordinates relative to the grid
            int pointRelativeX = pointInSquare.X - AnchorPoint.X;
            int pointRelativeY = pointInSquare.Y - AnchorPoint.Y;

            // We check if the point is in the grid
            int furthestPointXY = NB_SQUARE_ROW * SQUARE_SIZE;
            if (pointRelativeX > furthestPointXY || pointRelativeY > furthestPointXY || pointRelativeX < 0 || pointRelativeY < 0)
            {
                throw new ArgumentOutOfRangeException("The point is not in the Grid");
            }
            // We calculate the position of the cell the point is in by dividing his relative position by the size of a square
            double nbSquareX = Math.Ceiling(Convert.ToDouble(pointRelativeX / SQUARE_SIZE));
            double nbSquareY = Math.Ceiling(Convert.ToDouble(pointRelativeY / SQUARE_SIZE));

            return new Point(Convert.ToInt32(nbSquareX), Convert.ToInt32(nbSquareY));
        }
        #endregion
    }
}