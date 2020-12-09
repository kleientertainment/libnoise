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
namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer {

	/// <summary>
    /// Renders a normal map from a noise map.
    ///
    /// This class renders an image containing the normal vectors from a noise
    /// map object.  This image can then be used as a bump map for a 3D
    /// application or game.
    ///
    /// This class encodes the (x, y, z) components of the normal vector into
    /// the (red, green, blue) channels of the image.  Like any 24-bit
    /// true-color image, the channel values range from 0 to 255.  0
    /// represents a normal coordinate of -1.0 and 255 represents a normal
    /// coordinate of +1.0.
    ///
    /// You should also specify the <i>bump height</i> before rendering the
    /// normal map.  The bump height specifies the ratio of spatial resolution
    /// to elevation resolution.  For example, if your noise map has a spatial
    /// resolution of 30 meters and an elevation resolution of one meter, set
    /// the bump height to 1.0 / 30.0.
    ///
    /// <b>Rendering the normal map</b>
    ///
    /// To render the image containing the normal map, perform the following
    /// steps:
	/// - Pass a IMap2D<float> object to the NoiseMap property.
	/// - Pass an IMap2D<Color> object to the Image property.
	/// - Call the Render() method.
	/// </summary>
	public class NormalMapRenderer :AbstractImageRenderer {

		#region Fields

		/// <summary>
        /// This object requires three points (the initial point and the right
        /// and up neighbors) to calculate the normal vector at that point.
        /// If wrapping is/ enabled, and the initial point is on the edge of
        /// the noise map, the appropriate neighbors that lie outside of the
        /// noise map will "wrap" to the opposite side(s) of the noise map.
        /// Otherwise, the appropriate neighbors are cropped to the edge of
        /// the noise map.
        ///
        /// Enabling wrapping is useful when creating spherical and tileable
        /// normal maps.
		/// </summary>
		protected bool _WrapEnabled;

		/// <summary>
        /// The bump height specifies the ratio of spatial resolution to
        /// elevation resolution.  For example, if your noise map has a
        /// spatial resolution of 30 meters and an elevation resolution of one
        /// meter, set the bump height to 1.0 / 30.0.
        ///
        /// The spatial resolution and elevation resolution are determined by
        /// the application.
		/// </summary>
		protected float _bumpHeight;

		#endregion

		#region Accessors

		/// <summary>
		/// Enables or disables noise-map wrapping.
		///
		/// This object requires five points (the initial point and its four
		/// neighbors) to calculate light shading.  If wrapping is enabled,
		/// and the initial point is on the edge of the noise map, the
		/// appropriate neighbors that lie outside of the noise map will
		/// "wrap" to the opposite side(s) of the noise map.  Otherwise, the
		/// appropriate neighbors are cropped to the edge of the noise map.
		///
		/// Enabling wrapping is useful when creating spherical renderings and
		/// tileable textures.
		/// </summary>
		public bool WrapEnabled {
			get { return _WrapEnabled; }
			set { _WrapEnabled = value; }
		}

		/// <summary>
		/// Gets or Sets the bump height
		/// </summary>
		public float BumpHeight {
			get { return _bumpHeight; }
			set { _bumpHeight = value; }
		}


		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Default constructor
		/// </summary>
		public NormalMapRenderer() {
			_WrapEnabled = false;
			_bumpHeight = 1.0f;
		}//end NormalMapRenderer

		#endregion

		#region Interaction

		/// <summary>
		/// Renders the noise map to the destination image.
		/// </summary>
		public override void Render() {

			if(_noiseMap == null) {
				throw new ArgumentException("A noise map must be provided");
			}//end if

			if(_image == null) {
				throw new ArgumentException("An image map must be provided");
			}//end if

			if(_noiseMap.Width <= 0 || _noiseMap.Height <= 0) {
				throw new ArgumentException("Incoherent noise map size (0,0)");
			}//end if

			int width  = _noiseMap.Width;
			int height = _noiseMap.Height;
			int rightEdge = width -1;
			int topEdge = height -1;
			int leftEdgeOffset = -rightEdge;
			int bottomEdgeOffset = -topEdge;

			_image.SetSize(width, height);

			for(int y = 0; y < height; y++) {

				for(int x = 0; x < width; x++) {

					 // Calculate the positions of the current point's right and up
					 // neighbors.
					int yUpOffset, xRightOffset;

					if(_WrapEnabled) {

						if(x == rightEdge) {// right edge
							xRightOffset = leftEdgeOffset; // left edge
						}//end else 
						else { // anywhere
							xRightOffset = 1; // next
						}//end else

						if(y == topEdge) { // top edge
							yUpOffset   = bottomEdgeOffset; //bottom edge
						}//end if
						else { // anywhere
							yUpOffset   = 1; // above
						}//end else

					}//end if
					else {

						if(x == rightEdge) { // right edge
							xRightOffset = 0; // same
						}//end if
						else { // anywhere
							xRightOffset = 1; // next
						}//end else

						if(y == topEdge) { // top edge
							yUpOffset   = 0; // same
						}//end if
						else {
							yUpOffset   = 1; // above
						}//end else

					}//end else

					// Get the noise value of the current point in the source noise map
					// and the noise values of its right and up neighbors.
					float nc  = _noiseMap.GetValue(x, y);
					float nr = _noiseMap.GetValue(x + xRightOffset, y);
					float nu = _noiseMap.GetValue(x, y + yUpOffset);

					// Blend the source color, background color, and the light
					// intensity together, then update the destination image with that
					// color.
					_image.SetValue(x, y, CalcNormalColor(nc, nr, nu, _bumpHeight));

				}//end for x

				if(_callBack != null) {
					_callBack(y);
				}//end if

			}//end for y

		}//end Render

		#endregion

		#region Internal

		/// <summary>
		/// Calculates the normal vector at a given point on the noise map.
		/// 
		/// This method encodes the (x, y, z) components of the normal vector
		/// into the (red, green, blue) channels of the returned color.  In
		/// order to represent the vector as a color, each coordinate of the
		/// normal is mapped from the -1.0 to 1.0 range to the 0 to 255 range.
		///
		/// The bump height specifies the ratio of spatial resolution to
		/// elevation resolution.  For example, if your noise map has a
		/// spatial resolution of 30 meters and an elevation resolution of one
		/// meter, set the bump height to 1.0 / 30.0.
		/// 
		/// The spatial resolution and elevation resolution are determined by
		/// the application.
		/// </summary>
		/// <param name="nc">The height of the given point in the noise map</param>
		/// <param name="nr">The height of the left neighbor</param>
		/// <param name="nu">The height of the up neighbor</param>
		/// <param name="bumpHeight">The bump height</param>
		/// <returns>The normal vector represented as a color</returns>
		IColor CalcNormalColor(float nc, float nr, float nu, float bumpHeight) {

			// Calculate the surface normal.
			nc *= bumpHeight;
			nr *= bumpHeight;
			nu *= bumpHeight;

			float ncr = (nc - nr);
			float ncu = (nc - nu);
			float d = (float)Math.Sqrt((ncu * ncu) + (ncr * ncr) + 1);
			float vxc = (nc - nr) / d;
			float vyc = (nc - nu) / d;
			float vzc = 1.0f / d;

			// Map the normal range from the (-1.0 .. +1.0) range to the (0 .. 255)
			// range.
			byte xc, yc, zc;

			xc = (byte)(Libnoise.FastFloor((vxc + 1.0f) * 127.5f) & 0xff);
			yc = (byte)(Libnoise.FastFloor((vyc + 1.0f) * 127.5f) & 0xff);
			zc = (byte)(Libnoise.FastFloor((vzc + 1.0f) * 127.5f) & 0xff);

			//
			//zc = (byte)((int)((floor)((vzc + 1.0f) * 127.5f)) & 0xff); 

			return new Color(xc, yc, zc, 255);

		}//end CalcNormalColor

		
		#endregion

	}//end class

}//end namespace
