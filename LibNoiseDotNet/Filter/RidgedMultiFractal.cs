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
	/// Noise module that outputs 3-dimensional ridged-multifractal noise.
	///
	/// Ridged-multifractal noise is generated in much of the same way as 
	/// fractal noise, except the output of each octave is modified by an 
	/// absolute-value function.  Modifying the octave values in this way 
	/// produces ridge-like formations.
	///
	/// Ridged-multifractal noise does not use a persistence value.  This is
	/// because the persistence values of the octaves are based on the values
	/// generated from from previous octaves, creating a feedback loop (or
	/// that's what it looks like after reading the code.)
	///
	/// This noise module outputs ridged-multifractal-noise values that
	/// usually range from -1.0 to +1.0, but there are no guarantees that all
	/// output values will exist within that range.
	///
	/// For ridged-multifractal noise generated with only one octave,
	/// the output value ranges from -1.0 to 0.0.
	///
	/// Ridged-multifractal noise is often used to generate craggy mountainous
	/// terrain or marble-like textures.
	/// 
	/// Some good parameter values to start with: 
	///		spectralExponent: 0.9
	///		offset: 1
	///		gain:  2
	/// </summary>
	public class RidgedMultiFractal :FilterModule, IModule3D, IModule2D {

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public RidgedMultiFractal()
			: base() {
			_gain = 2.0f;
			_offset = 1.0f;
			_spectralExponent = 0.9f;

			ComputeSpectralWeights();

		}//end RidgedMultiFractal

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
			float weight;
			int curOctave;

			x *= _frequency;
			y *= _frequency;
			z *= _frequency;

			// Initialize value : 1st octave
			signal = _source3D.GetValue(x, y, z);

			// get absolute value of signal (this creates the ridges)
			if(signal < 0.0) {
				signal = -signal;
			}//end if

			// invert and translate (note that "offset" should be ~= 1.0)
			signal = _offset - signal;

			// Square the signal to increase the sharpness of the ridges.
			signal *= signal;

			// Add the signal to the output value.
			value = signal;

			weight = 1.0f;

			for(curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++) {

				x *= _lacunarity;
				y *= _lacunarity;
				z *= _lacunarity;

				// Weight successive contributions by the previous signal.
				weight = Libnoise.Clamp01(signal * _gain);

				// Get the coherent-noise value.
				signal = _source3D.GetValue(x, y, z);

				// Make the ridges.
				if(signal < 0.0f) {
					signal = -signal;
				}//end if

				signal = _offset - signal;

				// Square the signal to increase the sharpness of the ridges.
				signal *= signal;

				// The weighting from the previous octave is applied to the signal.
				// Larger values have higher weights, producing sharp points along the
				// ridges.
				signal *= weight;

				// Add the signal to the output value.
				value += (signal * _spectralWeights[curOctave]);

			}//end for

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
			float weight;
			int curOctave;

			x *= _frequency;
			y *= _frequency;

			// Initialize value : 1st octave
			signal = _source2D.GetValue(x, y);

			// get absolute value of signal (this creates the ridges)
			if(signal < 0.0f) {
				signal = -signal;
			}//end if

			// invert and translate (note that "offset" should be ~= 1.0)
			signal = _offset - signal;

			// Square the signal to increase the sharpness of the ridges.
			signal *= signal;

			// Add the signal to the output value.
			value = signal;

			weight = 1.0f;

			for(curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++) {

				x *= _lacunarity;
				y *= _lacunarity;

				// Weight successive contributions by the previous signal.
				weight = Libnoise.Clamp01(signal * _gain);

				// Get the coherent-noise value.
				signal = _source2D.GetValue(x, y);

				// Make the ridges.
				if(signal < 0.0) {
					signal = -signal;
				}//end if

				signal = _offset - signal;

				// Square the signal to increase the sharpness of the ridges.
				signal *= signal;

				// The weighting from the previous octave is applied to the signal.
				// Larger values have higher weights, producing sharp points along the
				// ridges.
				signal *= weight;

				// Add the signal to the output value.
				value += (signal * _spectralWeights[curOctave]);

			}//end for

			return value;

		}//end GetValue

		#endregion

	}//end class

}//end namespace
