// Project by Bauss
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyConquer.WorldApi.Tools
{
	/// <summary>
	/// In-line-algorithm wrapper.
	/// </summary>
	public sealed class ILA
    {
		/// <summary>
		/// The algorithm type.
		/// </summary>
        public enum Algorithm
        {
        	/// <summary>
        	/// DDA (Digital differential analyzer.)
        	/// </summary>
            DDA,
            /// <summary>
            /// Some math.
            /// </summary>
            SomeMath
        }
        
        /// <summary>
        /// A coordinate associated with the ILA algorithm.
        /// </summary>
        public struct ILACoordinate
        {
        	/// <summary>
        	/// The x coordinate.
        	/// </summary>
            public int X;
            
            /// <summary>
            /// The y coordinate.
            /// </summary>
            public int Y;

            /// <summary>
            /// Creates a new ILA coordinate.
            /// </summary>
            /// <param name="x">The x coordinate.</param>
            /// <param name="y">The y coordinate.</param>
            public ILACoordinate(double x, double y)
            {
                X = (int)x;
                Y = (int)y;
            }
        }
        
        /// <summary>
        /// A list of all line coordinates.
        /// </summary>
        private List<ILACoordinate> _lineCoordinates;
        
        /// <summary>
        /// The algorithm type.
        /// </summary>
        private Algorithm _algorithm;
        
        /// <summary>
        /// Gets the max distance.
        /// </summary>
        public byte MaxDistance { get; private set; }
        
        /// <summary>
        /// Gets the x1 coordinate.
        /// </summary>
        public ushort X1 { get; private set; }
        
        /// <summary>
        /// Gets the y1 coordinate.
        /// </summary>
        public ushort Y1 { get; private set; }
        
        /// <summary>
        /// Gets the x2 coordinate.
        /// </summary>
        public ushort X2 { get; private set; }
        
        /// <summary>
        /// Gets the y2 coordinate.
        /// </summary>
        public ushort Y2 { get; private set; }
        
        /// <summary>
        /// Gets the direction.
        /// </summary>
        public byte Direction { get; private set; }
        
        /// <summary>
        /// Checks whether a result of coordinates contains a coordinate.
        /// </summary>
        /// <param name="coords">The coordinates.</param>
        /// <param name="checkCoordinate">The coordinate to check.</param>
        /// <returns>True if the coordinates contains the coordinate.</returns>
        private bool Contains(List<ILACoordinate> coords, ILACoordinate checkCoordinate)
        {
        	foreach (var coord in coords)
            {
            	if (coord.X == checkCoordinate.X && checkCoordinate.Y == coord.Y)
            	{
            		return true;
            	}
            }
            
            return false;
        }
        
        /// <summary>
        /// Lines all coordinates.
        /// </summary>
        /// <param name="userx">The user x coordinate.</param>
        /// <param name="usery">The user y coordinate.</param>
        /// <param name="shotx">The shot x coordinate.</param>
        /// <param name="shoty">The shot y coordinate.</param>
        /// <returns>A list of all coordinates in line.</returns>
        private List<ILACoordinate> LineCoords(ushort userx, ushort usery, ushort shotx, ushort shoty)
        {
            return LineDDA(userx, usery, shotx, shoty);
        }
        
        /// <summary>
        /// Gets a list of all lines.
        /// </summary>
        /// <param name="xa">The xa.</param>
        /// <param name="ya">The ya.</param>
        /// <param name="xb">The xb.</param>
        /// <param name="yb">The yb.</param>
        /// <returns>A list of all coordinates.</returns>
        private List<ILACoordinate> LineDDA(int xa, int ya, int xb, int yb)
        {
            int dx = xb - xa, dy = yb - ya, steps, k;
            float xincrement, yincrement, x = xa, y = ya;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
            	steps = Math.Abs(dx);
            }
            else
            {
            	steps = Math.Abs(dy);
            }

            xincrement = dx / (float)steps;
            yincrement = dy / (float)steps;
            var thisLine = new List<ILACoordinate>();
            thisLine.Add(new ILACoordinate(Math.Round(x), Math.Round(y)));

            for (k = 0; k < MaxDistance; k++)
            {
                x += xincrement;
                y += yincrement;
                
                thisLine.Add(new ILACoordinate(Math.Round(x), Math.Round(y)));
            }
            
            return thisLine;
        }
        
        /// <summary>
        /// Creates a new ILA algorithm.
        /// </summary>
        /// <param name="x1">The x1 coordinate.</param>
        /// <param name="x2">The x2 coordinate.</param>
        /// <param name="y1">The y1 coordinate.</param>
        /// <param name="y2">The y2 coordinate.</param>
        /// <param name="maxDistance">The max distance.</param>
        /// <param name="algorithm">The algorithm type.</param>
        public ILA(ushort x1, ushort x2, ushort y1, ushort y2, byte maxDistance = 10, Algorithm algorithm = Algorithm.DDA)
        {
            _algorithm = algorithm;
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
            this.MaxDistance = maxDistance;
            
            if (_algorithm == Algorithm.DDA)
            {
            	_lineCoordinates = LineCoords(x1, y1, x2, y2);
            }

            Direction = (byte)RangeTools.GetAngle(x1, y1, x2, y2); ;
        }

        /// <summary>
        /// Checks whether a coordinate is in line.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>True if the coordinate is in line.</returns>
        public bool InLine(ushort x, ushort y)
        {
        	int mydst = (int)RangeTools.GetDistance((ushort)X1, (ushort)Y1, x, y);
            byte dir = (byte)RangeTools.GetAngle(X1, Y1, x, y);

            if (mydst <= MaxDistance)
            {
                if (_algorithm == Algorithm.SomeMath)
                {
                    if (dir != Direction)
                    {
                    	return false;
                    }
                    
                    //calculate line eq
                    if (X2 - X1 == 0)
                    {
                        //=> X - X1 = 0
                        //=> X = X1
                        return x == X1;
                    }
                    else if (Y2 - Y1 == 0)
                    {
                        //=> Y - Y1 = 0
                        //=> Y = Y1
                        return y == Y1;
                    }
                    else
                    {
                        double val1 = ((double)(x - X1)) / ((double)(X2 - X1));
                        double val2 = ((double)(y + Y1)) / ((double)(Y2 + Y1));
                        bool works = Math.Floor(val1) == Math.Floor(val2);
                        
                        return works;
                    }
                }
                else if (_algorithm == Algorithm.DDA)
                {
                	return Contains(_lineCoordinates, new ILACoordinate(x, y));
                }
            }
            
            return false;
        }
    }
}
