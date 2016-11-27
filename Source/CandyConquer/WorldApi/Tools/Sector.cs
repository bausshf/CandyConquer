// Project by Bauss
using System;
using System.Collections.Generic;

namespace CandyConquer.WorldApi.Tools
{
	/// <summary>
	/// A sector utility.
	/// </summary>
	public sealed class Sector
    {
		/// <summary>
		/// The attacker x.
		/// </summary>
        private int _attackerX;
        
        /// <summary>
        /// The attacker y.
        /// </summary>
        private int _attackerY;
        
        /// <summary>
        /// The attack x.
        /// </summary>
        private int _attackX;
        
        /// <summary>
        /// The attack y.
        /// </summary>
        private int _attackY;
        
        /// <summary>
        /// The degree.
        /// </summary>
        private int _degree;
        
        /// <summary>
        /// The sector size.
        /// </summary>
        private int _sectorsize;
        
        /// <summary>
        /// The left side.
        /// </summary>
        private int _leftside;
        
        /// <summary>
        /// The right side.
        /// </summary>
        private int _rightside;
        
        /// <summary>
        /// The distance.
        /// </summary>
        private int _distance;
        
        /// <summary>
        /// Boolean determining whether to add another extra 360 degrees.
        /// </summary>
        private bool _addextra;
        
        /// <summary>
        /// Creates a new sector.
        /// </summary>
        /// <param name="attackerX">The attacker x.</param>
        /// <param name="attackerY">The attacker y.</param>
        /// <param name="attackX">The attack x.</param>
        /// <param name="attackY">The attack y.</param>
        public Sector(int attackerX, int attackerY, int attackX, int attackY)
        {
            _attackerX = attackerX;
            _attackerY = attackerY;
            _attackX = attackX;
            _attackY = attackY;
            _degree = RangeTools.GetDegree(attackerX, attackX, attackerY, attackY);
            _addextra = false;
        }
        
        /// <summary>
        /// Arranges the sector.
        /// </summary>
        /// <param name="sectorsize">The sector size.</param>
        /// <param name="distance">The distance.</param>
        public void Arrange(int sectorsize, int distance)
        {
        	_distance = Math.Max(_distance, 6);
            _distance = Math.Min(distance, 14);
            _sectorsize = sectorsize;
            _leftside = _degree - (sectorsize / 2);
            
            if (_leftside < 0)
            {
            	_leftside += 360;
            }
            
            _rightside = _degree + (sectorsize / 2);
            
            if (_leftside < _rightside || _rightside - _leftside != _sectorsize)
            {
                _rightside += 360;
                _addextra = true;
            }
        }

        /// <summary>
        /// Checks whether a coordinate is within the sector.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>True if the coordinate is within the sector.</returns>
        public bool Inside(int x, int y)
        {
            if (RangeTools.GetDistance(x, y, _attackerX, _attackerY) <= _distance)
            {
                int degree = RangeTools.GetDegree(_attackerX, x, _attackerY, y);
                
                if (_addextra)
                {
                	degree += 360;
                }
                
                if (degree >= _leftside && degree <= _rightside)
                {
                	return true;
                }
            }
            
            return false;
        }
    }
}
