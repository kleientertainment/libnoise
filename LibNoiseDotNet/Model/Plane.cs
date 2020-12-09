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
	/// Model that defines the surface of a plane.
	///
	/// This model returns an output value from a noise module given the
	/// coordinates of an input value located on the surface of an ( x, z ) plane.
	///
	/// To generate an output value, pass the (x, z) coordinates of
	/// an input value to the GetValue() method.
	///
	/// This model is useful for creating:
	/// - two-dimensional textures
	/// - terrain height maps for local areas
	///
	/// This plane extends infinitely in both directions.
	/// </summary>
	public class Plane :AbstractModel{


		#region Ctor/Dtor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Plane() 
			:base(){

		}//end Plane

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="module">The noise module that is used to generate the output values</param>
		public Plane(IModule module) 
			:base(module){
		}//end Plane

		#endregion

		#region Interaction

		/// <summary>
		/// Returns the output value from the noise module given the
		/// (x, z) coordinates of the specified input value located
		/// on the surface of the plane.
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <param name="z">The z coordinate of the input value</param>
		/// <returns>The output value from the noise module</returns>
		public float GetValue(float x, float z){
			return ((IModule3D)_sourceModule).GetValue(x, 0.0f, z);
		}//end GetValue

		#endregion

		#region Internal

		#endregion

	}//end class

}//end namespace
