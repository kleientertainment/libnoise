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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier {

	/// <summary>
	/// This structure defines a control point.
	/// Control points are used for defining splines.
	/// </summary>
	public struct ControlPoint :IEquatable<ControlPoint> {

		#region fields
		/// <summary>
		/// The input value stored in the control point
		/// </summary>
		public float Input;

		/// <summary>
		/// The output value stored in the control point
		/// </summary>
		public float Output;
		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Create a new ControlPoint with given values
		/// </summary>
		/// <param name="input">The input value stored in the control point</param>
		/// <param name="output">The output value stored in the control point</param>
		public ControlPoint(float input, float output) {
			Input = input;
			Output = output;
		}//end ControlPoint

		#endregion

		#region Interface implementation

		public bool Equals(ControlPoint other) {
			return this.Input == other.Input;
		}//end Equals

		#endregion

	}//end Struct

}//end namespace
