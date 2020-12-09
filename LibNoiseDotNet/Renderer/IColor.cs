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

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer {

	/// <summary>
	/// Interface for a portable color structure.
	/// </summary>
	public interface IColor{


		/// <summary>
		/// The alpha channel
		/// </summary>
		byte Alpha { get; set; }

		/// <summary>
		/// The blue channel
		/// </summary>
		byte Blue { get; set; }

		/// <summary>
		/// The green channel
		/// </summary>
		byte Green { get; set; }

		/// <summary>
		/// The red channel
		/// </summary>
		byte Red { get; set; }

	}//end IColor

}//end namespace
