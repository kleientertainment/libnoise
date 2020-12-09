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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Model {

	/// <summary>
	/// Model that defines the surface of a cylinder.
	///
	/// This model returns an output value from a noise module given the
	/// coordinates of an input value located on the surface of a cylinder.
	///
	/// To generate an output value, pass the (angle, height) coordinates of
	/// an input value to the GetValue() method.
	///
	/// This model is useful for creating:
	/// - seamless textures that can be mapped onto a cylinder
	///
	/// This cylinder has a radius of 1.0 unit and has infinite height.  It is
	/// oriented along the y axis.  Its center is located at the origin.
	/// </summary>
	public class Cylinder :AbstractModel {


		#region Ctor/Dtor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Cylinder()
			: base() {

		}//end Plane

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="module">The noise module that is used to generate the output values</param>
		public Cylinder(IModule3D module)
			: base(module) {
		}//end Plane

		#endregion

		#region Interaction

		/// <summary>
		/// Returns the output value from the noise module given the
        /// (angle, height) coordinates of the specified input value located
        /// on the surface of the cylinder.
        ///
        /// This cylinder has a radius of 1.0 unit and has infinite height.
        /// It is oriented along the y axis.  Its center is located at the
        /// origin.
		/// </summary>
		/// <param name="angle">The angle around the cylinder's center, in degrees</param>
		/// <param name="height">The height along the y axis</param>
		/// <returns>The output value from the noise module</returns>
		public float GetValue(float angle, float height) {

			float x, y, z;

			x = (float)System.Math.Cos(angle * Libnoise.DEG2RAD);
			y = height;
			z = (float)System.Math.Sin(angle * Libnoise.DEG2RAD);

			return ((IModule3D)_sourceModule).GetValue(x, y, z);

		}//end GetValue

		#endregion

	}//end class

}//end namespace
