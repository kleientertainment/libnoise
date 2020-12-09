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
	/// Enumerates the noise quality.
	/// </summary>
	public enum NoiseQuality :byte{

		/// Generates coherent noise quickly.  When a coherent-noise function with
		/// this quality setting is used to generate a bump-map image, there are
		/// noticeable "creasing" artifacts in the resulting image.  This is
		/// because the derivative of that function is discontinuous at integer
		/// boundaries.
		Fast=0,

		/// Generates standard-quality coherent noise.  When a coherent-noise
		/// function with this quality setting is used to generate a bump-map
		/// image, there are some minor "creasing" artifacts in the resulting
		/// image.  This is because the second derivative of that function is
		/// discontinuous at integer boundaries.
		Standard=1,

		/// Generates the best-quality coherent noise.  When a coherent-noise
		/// function with this quality setting is used to generate a bump-map
		/// image, there are no "creasing" artifacts in the resulting image.  This
		/// is because the first and second derivatives of that function are
		/// continuous at integer boundaries.
		Best=2

	}//end enum

}//end namespace
