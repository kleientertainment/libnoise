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
	/// Noise module that outputs a checkerboard pattern.
	///
	/// This noise module outputs unit-sized blocks of alternating values.
	/// The values of these blocks alternate between -1.0 and +1.0.
	///
	/// This noise module is not really useful by itself, but it is often used
	/// for debugging purposes.
	/// 
	/// </summary>
	public class Checkerboard :PrimitiveModule, IModule3D {

		#region Ctor/Dtor
		/// <summary>
		/// Create new Checkerboard generator with default values
		/// </summary>
		public Checkerboard(){

		}//end Checkerboard

		#endregion

		#region Interaction

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y, float z) {

			// Fast floor
			int ix = (x > 0.0 ? (int)x: (int)x - 1);
			int iy = (y > 0.0 ? (int)y: (int)y - 1);
			int iz = (z > 0.0 ? (int)z: (int)z - 1);

			return (ix & 1 ^ iy & 1 ^ iz & 1) != 0 ? -1.0f: 1.0f;

		}//end GetValue

		#endregion


	}//end class

}//end namespace
