// This file is part of libnoise-dotnet.
//
// libnoise-dotnet is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// libnoise-dotnet is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with libnoise-dotnet.  If not, see <http://www.gnu.org/licenses/>.
// 
// From the original Jason Bevins's Libnoise (http://libnoise.sourceforge.net)

using System;
using LibNoiseDotNet.Graphics.Tools.Noise.Model;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder {

	/// <summary>
	/// Builds a cylindrical noise map.
	///
	/// This class builds a noise map by filling it with coherent-noise values
	/// generated from the surface of a cylinder.
	///
	/// This class describes these input values using an (angle, height)
	/// coordinate system.  After generating the coherent-noise value from the
	/// input value, it then "flattens" these coordinates onto a plane so that
	/// it can write the values into a two-dimensional noise map.
	///
	/// The cylinder model has a radius of 1.0 unit and has infinite height.
	/// The cylinder is oriented along the y axis.  Its center is at the
	/// origin.
	///
	/// The x coordinate in the noise map represents the angle around the
	/// cylinder's y axis.  The y coordinate in the noise map represents the
	/// height above the x-z plane.
	///
	/// The application must provide the lower and upper angle bounds of the
	/// noise map, in degrees, and the lower and upper height bounds of the
	/// noise map, in units.
	/// </summary>
	public class NoiseMapBuilderCylinder :NoiseMapBuilder {

		#region Fields

		/// <summary>
		/// Lower Angle boundary of the planar noise map, in units.
		/// </summary>
		float _lowerAngleBound;

		/// <summary>
		/// Lower Height boundary of the planar noise map, in units.
		/// </summary>
		float _lowerHeightBound;

		/// <summary>
		/// Upper Angle boundary of the planar noise map, in units.
		/// </summary>
		float _upperAngleBound;

		/// <summary>
		/// Upper Height boundary of the planar noise map, in units.
		/// </summary>
		float _upperHeightBound;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets the lower Height boundary of the planar noise map, in units.
		/// </summary>
		public float LowerHeightBound {
			get { return _lowerHeightBound; }
		}

		/// <summary>
		/// Gets the lower Angle boundary of the planar noise map, in units.
		/// </summary>
		public float LowerAngleBound {
			get { return _lowerAngleBound; }
		}

		/// <summary>
		/// Gets the upper Angle boundary of the planar noise map, in units.
		/// </summary>
		public float UpperAngleBound {
			get { return _upperAngleBound; }
		}

		/// <summary>
		/// Gets the upper Height boundary of the planar noise map, in units.
		/// </summary>
		public float UpperHeightBound {
			get { return _upperHeightBound; }
		}

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Default constructor
		/// </summary>
		public NoiseMapBuilderCylinder() {
			SetBounds(-180.0f, 180.0f, -10.0f, 10.0f); 
		}//end NoiseMapBuilder

		#endregion

		#region Interaction

		/// <summary>
		/// Sets the boundaries of the planar noise map.
		///
		/// @pre The lower Angle boundary is less than the upper Angle boundary.
		/// @pre The lower Height boundary is less than the upper Height boundary.
		///
		/// @throw ArgumentException See the preconditions.
		/// </summary>
		/// <param name="lowerXBound">The lower Angle boundary of the noise map, in units.</param>
		/// <param name="upperXBound">The upper Angle boundary of the noise map, in units.</param>
		/// <param name="lowerZBound">The lower Height boundary of the noise map, in units.</param>
		/// <param name="upperZBound">The upper Height boundary of the noise map, in units.</param>
		public void SetBounds(float lowerAngleBound, float upperAngleBound, float lowerHeightBound, float upperHeightBound) {

			if(lowerAngleBound >= upperAngleBound || lowerHeightBound >= upperHeightBound) {
				throw new ArgumentException("Incoherent bounds : lowerAngleBound >= upperAngleBound or lowerZBound >= upperHeightBound");
			}//end if

			_lowerAngleBound = lowerAngleBound;
			_upperAngleBound = upperAngleBound;
			_lowerHeightBound = lowerHeightBound;
			_upperHeightBound = upperHeightBound;

		}//end SetBounds

		/// <summary>
		/// Builds the noise map.
		///
		/// @pre SetBounds() was previously called.
		/// @pre NoiseMap was previously defined.
		/// @pre a SourceModule was previously defined.
		/// @pre The width and height values specified by SetSize() are
		/// positive.
		/// @pre The width and height values specified by SetSize() do not
		/// exceed the maximum possible width and height for the noise map.
		///
		/// @post The original contents of the destination noise map is
		/// destroyed.
		///
		/// @throw noise::ArgumentException See the preconditions.
		///
		/// If this method is successful, the destination noise map contains
		/// the coherent-noise values from the noise module specified by
		/// the SourceModule.
		/// </summary>
		public override void Build() {

			if(_lowerAngleBound >= _upperAngleBound || _lowerHeightBound >= _upperHeightBound) {
				throw new ArgumentException("Incoherent bounds : lowerAngleBound >= upperAngleBound or lowerZBound >= upperHeightBound");
			}//end if

			if(_width < 0 || _height < 0) {
				throw new ArgumentException("Dimension must be greater or equal 0");
			}//end if

			if(_sourceModule == null) {
				throw new ArgumentException("A source module must be provided");
			}//end if

			if(_noiseMap == null) {
				throw new ArgumentException("A noise map must be provided");
			}//end if

			// Resize the destination noise map so that it can store the new output
			// values from the source model.
			_noiseMap.SetSize(_width, _height);

			// Create the plane model.
			Cylinder model = new Cylinder((IModule3D)_sourceModule);

			float angleExtent  = _upperAngleBound  - _lowerAngleBound;
			float heightExtent = _upperHeightBound - _lowerHeightBound;

			float xDelta = angleExtent  / (float)_width;
			float yDelta = heightExtent / (float)_height;

			float curAngle  = _lowerAngleBound;
			float curHeight = _lowerHeightBound;

			// Fill every point in the noise map with the output values from the model.
			for(int y = 0; y < _height; y++) {

				curAngle = _lowerAngleBound;

				for(int x = 0; x < _width; x++) {


					float finalValue;
					FilterLevel level = FilterLevel.Source;

					if(_filter != null) {
						level = _filter.IsFiltered(x, y);
					}//end if

					if(level == FilterLevel.Constant) {
						finalValue = _filter.ConstantValue;
					}//end if
					else {

						finalValue = (float)model.GetValue(curAngle, curHeight);

						if(level == FilterLevel.Filter) {
							finalValue = _filter.FilterValue(x, y, finalValue);
						}//end if

					}//end else

					_noiseMap.SetValue(x, y, finalValue);

					curAngle += xDelta;

				}//end for

				curHeight += yDelta;

				if(_callBack != null) {
					_callBack(y);
				}//end if

			}//end for

		}//end Build

		#endregion

	}//end class

}//end namespace
