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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive {

	/// <summary>
	/// Noise module that outputs concentric cylinders.
	///
	/// This noise module outputs concentric cylinders centered on the origin.
	/// These cylinders are oriented along the y axis similar to the
	/// concentric rings of a tree.  Each cylinder extends infinitely along
	/// the y axis.
	///
	/// The first cylinder has a radius of 1.0.  Each subsequent cylinder has
	/// a radius that is 1.0 unit larger than the previous cylinder.
	///
	/// The output value from this noise module is determined by the distance
	/// between the input value and the the nearest cylinder surface.  The
	/// input values that are located on a cylinder surface are given the
	/// output value 1.0 and the input values that are equidistant from two
	/// cylinder surfaces are given the output value -1.0.
	///
	/// An application can change the frequency of the concentric cylinders.
	/// Increasing the frequency reduces the distances between cylinders.
	///
	/// This noise module, modified with some low-frequency, low-power
	/// turbulence, is useful for generating wood-like textures.
	/// 
	/// </summary>
	public class Cylinders :PrimitiveModule, IModule3D {

		#region Constants

		/// <summary>
		/// Frequency of the concentric spheres.
		/// </summary>
		public const float DEFAULT_FREQUENCY = 1.0f;

		#endregion

		#region Fields

		/// <summary>
		/// Frequency of the concentric cylinders.
		/// </summary>
		protected float _frequency = DEFAULT_FREQUENCY;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the frequency
		/// </summary>
		public float Frequency {
			get { return _frequency; }
			set { _frequency = value; }
		}//end
		#endregion

		#region Ctor/Dtor
		/// <summary>
		/// Create new Cylinders generator with default values
		/// </summary>
		public Cylinders() 
			: this(DEFAULT_FREQUENCY){

		}//end Cylinders

		/// <summary>
		/// Create a new Cylinders generator with given values
		/// </summary>
		/// <param name="frequency"></param>
		public Cylinders(float frequency) {
			
			_frequency = frequency;

		}//end Cylinders

		#endregion

		#region IModule3D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y, float z) {

			x *= _frequency;
			z *= _frequency;

			float distFromCenter = (float)System.Math.Sqrt(x * x + z * z);
			float distFromSmallerSphere = distFromCenter - (float)System.Math.Floor(distFromCenter);
			float distFromLargerSphere = 1.0f - distFromSmallerSphere;
			float nearestDist = System.Math.Min(distFromSmallerSphere, distFromLargerSphere);
			return 1.0f - (nearestDist * 4.0f); // Puts it in the -1.0 to +1.0 range.

		}//end GetValue

		#endregion

	}//end class

}//end namespace
