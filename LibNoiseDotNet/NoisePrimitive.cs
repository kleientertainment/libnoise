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

namespace LibNoiseDotNet.Graphics.Tools.Noise{

	/// <summary>
	/// 
	/// </summary>
	public enum NoisePrimitive: byte{
		
		Constant = 1,
		Spheres = 2,
		Cylinders = 3,
		BevinsValue = 4,
		BevinsGradient = 5,
		//ClassicPerlin = 6,
		ImprovedPerlin = 7,
		SimplexPerlin = 8

	}//end enum

}//end namespace
