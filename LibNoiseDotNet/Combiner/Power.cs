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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Combiner {

	/// <summary>
	/// Noise module that raises the output value from the left source module
	/// to the power of the output value from the right source module.
	/// </summary>
	public class Power :CombinerModule, IModule3D {

		#region Ctor/Dtor

		public Power()
			: base() {
		}//end Power

		public Power(IModule left, IModule right)
			: base(left, right) {
		}//end Power

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
			return (float)System.Math.Pow(((IModule3D)_leftModule).GetValue(x, y, z), ((IModule3D)_rightModule).GetValue(x, y, z));
		}//end GetValue

		#endregion

	}//end class

}//end namespace
