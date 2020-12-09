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
	/// Noise module that outputs a constant value.
	///
	/// This noise module is not useful by itself, but it is often used as a
	/// source module for other noise modules.
	/// </summary>
	public class Constant :PrimitiveModule, IModule4D, IModule3D, IModule2D, IModule1D{
		
		#region Constants

		/// <summary>
		/// 
		/// </summary>
		public const float DEFAULT_VALUE = 0.5f;

		#endregion

		#region Fields

		/// <summary>
		/// 
		/// </summary>
		protected float _constant = DEFAULT_VALUE;

		#endregion

		#region Accessors

		/// <summary>
		/// the constant output value for this noise module.
		/// </summary>
		public float ConstantValue {
			get { return _constant; }
			set { _constant = value; }
		}//end Constant

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Create a new noiws module with DEFAULT_VALUE
		/// </summary>
		public Constant() 
			:this(DEFAULT_VALUE){

		}//end Constant

		/// <summary>
		/// Create a new noise module width given value 
		/// </summary>
		/// <param name="value">The value to use</param>
		public Constant(float value) {
			_constant = value;
		}//end Constant


		#endregion

		#region IModule4D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <param name="t">The input coordinate on the t-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y, float z, float t) {
			return _constant;
		}//end GetValue

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
			return _constant;
		}//end GetValue

		#endregion

		#region IModule2D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y) {
			return _constant;
		}//end GetValue

		#endregion

		#region IModule1D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x) {
			return _constant;
		}//end GetValue

		#endregion

	}//end class

}//end namespace
