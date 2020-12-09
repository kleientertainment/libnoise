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
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer {

	/// <summary>
	/// Implements a 32 bits Heightmap, a 2-dimensional array of float values (+-1.5 x 10^−45 to +-3.4 x 10^38)
	/// </summary>
	public class Heightmap32 :NoiseMap {

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public Heightmap32()
			: base() {
		}//End Heightmap32

		/// <summary>
		/// Create a new Heightmap32 with the given values
		/// The width and height values must be positive.
		/// 
		/// </summary>
		/// <param name="width">The width of the new noise map.</param>
		/// <param name="height">The height of the new noise map</param>
		public Heightmap32(int width, int height)
			: base(width, height) {
		}//End Heightmap32

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="copy">The heightmap to copy</param>
		public Heightmap32(Heightmap32 copy)  
			:base(copy){
		}//End Heightmap32

		#endregion

	}//end class

}//end namespace
