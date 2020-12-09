// This file is part of Libnoise-dotnet.
//
// Libnoise-dotnet is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Libnoise-dotnet is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Libnoise-dotnet.  If not, see <http://www.gnu.org/licenses/>.
// 
// From the original Jason Bevins's Libnoise (http://libnoise.sourceforge.net)
// From Frédéric Lecointre's c# port (frederic.lecointre@burnweb.net)

namespace LibNoiseDotNet.Graphics.Tools.Noise{

	/// <summary>
	/// Abstract interface for noise modules that calculates and outputs a value
	/// given a four-dimensional input value.
	/// </summary>
	public interface IModule4D :IModule {


		#region Interaction

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <param name="z">The input coordinate on the t-axis.</param>
		/// <returns>The resulting output value.</returns>
		float GetValue(float x, float y, float z, float t);


		#endregion


	}//end interface

}//end namespace
