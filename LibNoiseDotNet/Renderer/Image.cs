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

using LibNoiseDotNet.Graphics.Tools.Noise.Utils;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer {

	/// <summary>
	/// Implements an image, a 2-dimensional array of color values.
	///
	/// An image can be used to store a color texture.
	///
	/// These color values are of type Color.
	/// </summary>
	public class Image :DataMap<Color>, IMap2D<Color> {

		#region constants

		/// <summary>
		/// The maximum width of a raster.
		/// </summary>
		public const int RASTER_MAX_WIDTH = 32767;

		/// <summary>
		/// The maximum height of a raster.
		/// </summary>
		public const int RASTER_MAX_HEIGHT = 32767;

		#endregion

		#region Ctor/Dtor
		/// <summary>
		/// Create an empty Image
		/// </summary>
		public Image() {

			_hasMaxDimension = true;
			_maxHeight = RASTER_MAX_HEIGHT;
			_maxWidth = RASTER_MAX_WIDTH;

			_borderValue = Color.TRANSPARENT;
			AllocateBuffer();
		}//End NoiseMap

		/// <summary>
		/// Create a new Image with the given values
		///
		/// The width and height values are positive.
		/// The width and height values do not exceed the maximum
		/// possible width and height for the image.
		///
		/// @throw System.ArgumentException See the preconditions.
		/// @throw noise::ExceptionOutOfMemory Out of memory.
		///
		/// Creates a image with uninitialized values.
		///
		/// It is considered an error if the specified dimensions are not
		/// positive.
		/// </summary>
		/// <param name="width">The width of the new noise map.</param>
		/// <param name="height">The height of the new noise map</param>
		public Image(int width, int height){

			_hasMaxDimension = true;
			_maxHeight = RASTER_MAX_HEIGHT;
			_maxWidth = RASTER_MAX_WIDTH;

			_borderValue = Color.WHITE;
			AllocateBuffer(width, height);
			
		}//End NoiseMap

		/// <summary>
		/// Copy constructor
		/// @throw noise::ExceptionOutOfMemory Out of memory.
		/// </summary>
		/// <param name="copy">The image to copy</param>
		public Image(Image copy) {

			_hasMaxDimension = true;
			_maxHeight = RASTER_MAX_HEIGHT;
			_maxWidth = RASTER_MAX_WIDTH;

			_borderValue = Color.WHITE;
			CopyFrom(copy);
		}//End NoiseMap

		#endregion

		#region Interaction

		/// <summary>
		/// Find the lowest and highest value in the map
		/// </summary>
		/// Cannot implement this method in DataMap because 
		/// T < T or T > T does not compile (Unpredictable type of T)
		/// <param name="min">the lowest value</param>
		/// <param name="max">the highest value</param>
		public void MinMax(out Color min, out Color max) {

			min = max = MinvalofT();

			if(_data != null && _data.Length > 0) {

				// First value, min and max for now
				min = max = _data[0];

				for(int i = 0; i < _data.Length; i++) {

					if(min > _data[i]) {
						min = _data[i];
					}//end if
					else if(max < _data[i]) {
						max = _data[i];
					}//end else if

				}//end for

			}//end if

		}//end MinMax

		#endregion

		#region Internal

		/// <summary>
		/// Return the memory size of a Color
		/// 
		/// </summary>
		/// <returns>The memory size of a Color</returns>
		protected override int SizeofT() {
			return 64; // 4* byte(8) + 1 int(32)
		}//end protected

		/// <summary>
		/// Return the maximum value of a Color type (Solid white)
		/// </summary>
		/// <returns></returns>
		protected override Color MaxvalofT() {
			return Color.WHITE;
		}//end protected

		/// <summary>
		/// Return the minimum value of a Color type (Solid black)
		/// </summary>
		/// <returns></returns>
		protected override Color MinvalofT() {
			return Color.BLACK;
		}//end protected

		#endregion

	}//end class

}//end namespace
