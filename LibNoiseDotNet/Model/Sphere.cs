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
	/// Model that defines the surface of a sphere.
	///
	/// This model returns an output value from a noise module given the
	/// coordinates of an input value located on the surface of a sphere.
	///
	/// To generate an output value, pass the (latitude, longitude)
	/// coordinates of an input value to the GetValue() method.
	///
	/// This model is useful for creating:
	/// - seamless textures that can be mapped onto a sphere
	/// - terrain height maps for entire planets
	///
	/// This sphere has a radius of 1.0 unit and its center is located at
	/// the origin.
	/// </summary>
	public class Sphere :AbstractModel {


		#region Ctor/Dtor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Sphere()
			: base() {

		}//end Plane

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="module">The noise module that is used to generate the output values</param>
		public Sphere(IModule3D module)
			: base(module) {
		}//end Plane

		#endregion

		#region Interaction

		/// <summary>
		/// Returns the output value from the noise module given the
        /// (latitude, longitude) coordinates of the specified input value
        /// located on the surface of the sphere.
        ///
        /// Use a negative latitude if the input value is located on the
        /// southern hemisphere.
        ///
        /// Use a negative longitude if the input value is located on the
        /// western hemisphere.
		/// </summary>
		/// <param name="lat">The latitude of the input value, in degrees</param>
		/// <param name="lon">The longitude of the input value, in degrees</param>
		/// <returns>The output value from the noise module</returns>
		public float GetValue(float lat, float lon) {

			float x = 0.0f, y = 0.0f, z = 0.0f;
			Libnoise.LatLonToXYZ(lat, lon, ref x, ref y, ref z);
			return ((IModule3D)_sourceModule).GetValue(x, y, z);

		}//end GetValue

		#endregion

		#region Internal

		#endregion

	}//end class

}//end namespace
