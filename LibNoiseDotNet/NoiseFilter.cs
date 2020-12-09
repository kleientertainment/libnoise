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
	public enum NoiseFilter :byte{
		Pipe=0,
		SumFractal=1,
		SinFractal=2,
		Billow=19,
		MultiFractal=20,
		HeterogeneousMultiFractal=21,
		HybridMultiFractal=22,
		RidgedMultiFractal=23,
		Voronoi=30
	}//end class

}//end namespace
