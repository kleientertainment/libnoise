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
	/// Builds a spherical noise map.
	///
	/// This class builds a noise map by filling it with coherent-noise values
	/// generated from the surface of a sphere.
	///
	/// This class describes these input values using a (latitude, longitude)
	/// coordinate system.  After generating the coherent-noise value from the
	/// input value, it then "flattens" these coordinates onto a plane so that
	/// it can write the values into a two-dimensional noise map.
	///
	/// The sphere model has a radius of 1.0 unit.  Its center is at the
	/// origin.
	///
	/// The x coordinate in the noise map represents the longitude.  The y
	/// coordinate in the noise map represents the latitude.  
	///
	/// The application must provide the southern, northern, western, and
	/// eastern bounds of the noise map, in degrees.
	/// </summary>
	public class NoiseMapBuilderSphere :NoiseMapBuilder {

		#region Fields
		/// <summary>
		/// Eastern boundary of the spherical noise map, in degrees.
		/// </summary>
		float _eastLonBound;

		/// <summary>
		/// Northern boundary of the spherical noise map, in degrees.
		/// </summary>
		float _northLatBound;

		/// <summary>
		/// Southern boundary of the spherical noise map, in degrees.
		/// </summary>
		float _southLatBound;

		/// <summary>
		/// Western boundary of the spherical noise map, in degrees.
		/// </summary>
		float _westLonBound;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets the eastern boundary of the spherical noise map, in degrees.
		/// </summary>
		public float EastLonBound {
			get { return _eastLonBound; }
		}

		/// <summary>
		/// Gets the northern boundary of the spherical noise map, in degrees.
		/// </summary>
		public float NorthLatBound {
			get { return _northLatBound; }
		}

		/// <summary>
		/// Gets the southern boundary of the spherical noise map, in degrees.
		/// </summary>
		public float SouthLatBound {
			get { return _southLatBound; }
		}

		/// <summary>
		/// Gets the western boundary of the spherical noise map, in degrees.
		/// </summary>
		public float WestLonBound {
			get { return _westLonBound; }
		}

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Default constructor
		/// </summary>
		public NoiseMapBuilderSphere() {
			SetBounds(-90f, 90f, -180f, 180f); // degrees
		}//end NoiseMapBuilder

		#endregion

		#region Interaction

		/// <summary>
		/// Sets the coordinate boundaries of the noise map.
		///
		/// @pre The southern boundary is less than the northern boundary.
		/// @pre The western boundary is less than the eastern boundary.
		/// </summary>
		/// <param name="southLatBound"></param>
		/// <param name="northLatBound"></param>
		/// <param name="westLonBound"></param>
		/// <param name="eastLonBound"></param>
		public void SetBounds(float southLatBound, float northLatBound, float westLonBound, float eastLonBound) {

			if(southLatBound >= northLatBound || westLonBound >= eastLonBound) {
				throw new ArgumentException("Incoherent bounds : southLatBound >= northLatBound or westLonBound >= eastLonBound");
			}//end if

			_southLatBound = southLatBound;
			_northLatBound = northLatBound;
			_westLonBound  = westLonBound;
			_eastLonBound  = eastLonBound;

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
		public override void Build(){

			if(_southLatBound >= _northLatBound || _westLonBound >= _eastLonBound) {
				throw new ArgumentException("Incoherent bounds : southLatBound >= northLatBound or westLonBound >= eastLonBound");
			}//end if

			if(_width < 0 || _height < 0) {
				throw new ArgumentException("Dimension must be greater or equal 0");
			}//end if

			if(_sourceModule == null){
				throw new ArgumentException("A source module must be provided");
			}//end if

			if(_noiseMap == null){
				throw new ArgumentException("A noise map must be provided");
			}//end if

			// Resize the destination noise map so that it can store the new output
			// values from the source model.
			_noiseMap.SetSize(_width, _height);

			// Create the plane model.
			Sphere model = new Sphere((IModule3D)_sourceModule);

			float lonExtent = _eastLonBound  - _westLonBound ;
			float latExtent = _northLatBound - _southLatBound;

			float xDelta = lonExtent / (float)_width ;
			float yDelta = latExtent / (float)_height;

			float curLon = _westLonBound ;
			float curLat = _southLatBound;

			// Fill every point in the noise map with the output values from the model.
			for (int y = 0; y < _height; y++) {

				curLon = _westLonBound;

				for (int x = 0; x < _width; x++) {

					float finalValue;
					FilterLevel level = FilterLevel.Source;

					if(_filter != null) {
						level = _filter.IsFiltered(x, y);
					}//end if

					if(level == FilterLevel.Constant) {
						finalValue = _filter.ConstantValue;
					}//end if
					else {
						finalValue = model.GetValue(curLat, curLon);

						if(level == FilterLevel.Filter) {
							finalValue = _filter.FilterValue(x, y, finalValue);
						}//end if

					}//end else

					_noiseMap.SetValue(x, y, finalValue);

					curLon += xDelta;

				}//end for

				curLat += yDelta;

				if (_callBack != null) {
					_callBack(y);
				}//end if

			}//end for

		}//end Build

		#endregion

	}//end class

}//end namespace
