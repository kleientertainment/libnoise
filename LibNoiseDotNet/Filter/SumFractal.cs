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
//
// The following code is based on Ken Musgrave's explanations and sample
// source code in the book "Texturing and Modelling: A procedural approach"

namespace LibNoiseDotNet.Graphics.Tools.Noise.Filter {

	/// <summary>
	/// Noise module that outputs 3-dimensional Sum Fractal noise. This noise
	/// is also known as "Fractional BrownianMotion noise"
	///
	/// Sum Fractal noise is the sum of several coherent-noise functions of
	/// ever-increasing frequencies and ever-decreasing amplitudes.
	/// 
	/// This class implements the original noise::module::Perlin
	/// 
	/// </summary>
	public class SumFractal :FilterModule, IModule3D, IModule2D {

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public SumFractal(): base(){

		}//end SumFractal

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

			float signal;
			float value;
			int curOctave;

			x *= _frequency;
			y *= _frequency;
			z *= _frequency;

			// Initialize value, fBM starts with 0
			value = 0;

			// Inner loop of spectral construction, where the fractal is built
			for(curOctave = 0; curOctave < _octaveCount; curOctave++) {

				// Get the coherent-noise value.
				signal = _source3D.GetValue(x, y, z) * _spectralWeights[curOctave];

				// Add the signal to the output value.
				value += signal;

				// Go to the next octave.
				x *= _lacunarity;
				y *= _lacunarity;
				z *= _lacunarity;

			}//end for

			//take care of remainder in _octaveCount
			float remainder = _octaveCount - (int)_octaveCount;
			if(remainder > 0.0f) {
				value += remainder * _source3D.GetValue(x, y, z) * _spectralWeights[curOctave];
			}//end if

			return value;

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

			float signal;
			float value;
			int curOctave;

			x *= _frequency;
			y *= _frequency;

			// Initialize value, fBM starts with 0
			value = 0;

			// Inner loop of spectral construction, where the fractal is built

			for(curOctave = 0; curOctave < _octaveCount; curOctave++) {

				// Get the coherent-noise value.
				signal = _source2D.GetValue(x, y) * _spectralWeights[curOctave];

				// Add the signal to the output value.
				value += signal;

				// Go to the next octave.
				x *= _lacunarity;
				y *= _lacunarity;

			}//end for

			//take care of remainder in _octaveCount
			float remainder = _octaveCount - (int)_octaveCount;
			if(remainder > 0.0f) {
				value += remainder * _source2D.GetValue(x, y) * _spectralWeights[curOctave];
			}//end if

			return value;

		}//end GetValue

		#endregion

	}//end class

}//end namespace
