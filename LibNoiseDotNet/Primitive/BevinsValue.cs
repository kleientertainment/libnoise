﻿// This file is part of libnoise-dotnet.
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

// Specifies the version of the coherent-noise functions to use.

// - define LIBNOISE_VERSION_2 to use the current version.
// - define LIBNOISE_VERSION_1 to use the flawed version from the original version of libnoise.
//
// If your application requires coherent-noise values that were generated by
// an earlier version of libnoise, change this constant to the appropriate
// value and recompile libnoise.
//#define LIBNOISE_VERSION_1;
#define LIBNOISE_VERSION_2

using LibNoiseDotNet.Graphics.Tools.Noise;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive {

	/// <summary>
	/// Base class for all value noise generator module
	/// </summary>
	public class BevinsValue :PrimitiveModule, IModule3D, IModule2D, IModule1D {

		#region Constants

		// These constants control certain parameters that all coherent-noise
		// functions require.
#if LIBNOISE_VERSION_2

		// Constants used by the current version of libnoise.
		public const int X_NOISE_GEN = 1619;
		public const int Y_NOISE_GEN = 31337;
		public const int Z_NOISE_GEN = 6971;
		public const int SEED_NOISE_GEN = 1013;
		public const int SHIFT_NOISE_GEN = 8;

#else

		// Constants used by the original version of libnoise.
		// Because X_NOISE_GEN is not relatively prime to the other values, and
		// Z_NOISE_GEN is close to 256 (the number of random gradient vectors),
		// patterns show up in high-frequency coherent noise.
		public const int X_NOISE_GEN = 1;
		public const int Y_NOISE_GEN = 31337;
		public const int Z_NOISE_GEN = 263;
		public const int SEED_NOISE_GEN = 1013;
		public const int SHIFT_NOISE_GEN = 13;

#endif

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public BevinsValue()
			: this(DEFAULT_SEED, DEFAULT_QUALITY) {

		}//end BevinsValue

		/// <summary>
		/// Create a new BevinsValueNoise with given values
		/// </summary>
		/// <param name="seed"></param>
		/// <param name="quality"></param>
		public BevinsValue(int seed, NoiseQuality quality) {
			
			_seed = seed;
			_quality = quality;

		}//end BevinsValue

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
			return ValueCoherentNoise3D(x, y, z, _seed, _quality);
		}//end GetValue

		#endregion

		#region ValueCoherentNoise3D

		/// <summary>
		/// Generates a value-coherent-noise value from the coordinates of a
		/// three-dimensional input value.
		///
		/// The return value ranges from -1.0 to +1.0.
		///
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <param name="y">The y coordinate of the input value</param>
		/// <param name="z">The z coordinate of the input value</param>
		/// <param name="seed">The random number seed</param>
		/// <param name="quality">The quality of the coherent-noise</param>
		/// <returns>The generated value-coherent-noise value</returns>
		public static float ValueCoherentNoise3D(float x, float y, float z, long seed, NoiseQuality quality) {

			// Create a unit-length cube aligned along an integer boundary.  
			// This cube surrounds the input point.
			int x0 = (x > 0.0 ? (int)x: (int)x - 1);
			int x1 = x0 + 1;

			int y0 = (y > 0.0 ? (int)y: (int)y - 1);
			int y1 = y0 + 1;

			int z0 = (z > 0.0 ? (int)z: (int)z - 1);
			int z1 = z0 + 1;

			// Map the difference between the coordinates of the input value and the
			// coordinates of the cube's outer-lower-left vertex onto an S-curve.
			float xs = 0, ys = 0, zs = 0;

			switch(quality) {

				case NoiseQuality.Fast:
					xs = (x - (float)x0);
					ys = (y - (float)y0);
					zs = (z - (float)z0);
					break;

				case NoiseQuality.Standard:
					xs = Libnoise.SCurve3(x - (float)x0);
					ys = Libnoise.SCurve3(y - (float)y0);
					zs = Libnoise.SCurve3(z - (float)z0);
					break;

				case NoiseQuality.Best:
					xs = Libnoise.SCurve5(x - (float)x0);
					ys = Libnoise.SCurve5(y - (float)y0);
					zs = Libnoise.SCurve5(z - (float)z0);
					break;
			}//end switch

			// Now calculate the noise values at each vertex of the cube.  To generate
			// the coherent-noise value at the input point, interpolate these eight
			// noise values using the S-curve value as the interpolant (trilinear
			// interpolation.)
			float n0, n1, ix0, ix1, iy0, iy1;
			n0   = ValueNoise3D(x0, y0, z0, seed);
			n1   = ValueNoise3D(x1, y0, z0, seed);
			ix0  = Libnoise.Lerp(n0, n1, xs);

			n0   = ValueNoise3D(x0, y1, z0, seed);
			n1   = ValueNoise3D(x1, y1, z0, seed);
			ix1  = Libnoise.Lerp(n0, n1, xs);
			iy0  = Libnoise.Lerp(ix0, ix1, ys);

			n0   = ValueNoise3D(x0, y0, z1, seed);
			n1   = ValueNoise3D(x1, y0, z1, seed);
			ix0  = Libnoise.Lerp(n0, n1, xs);

			n0   = ValueNoise3D(x0, y1, z1, seed);
			n1   = ValueNoise3D(x1, y1, z1, seed);
			ix1  = Libnoise.Lerp(n0, n1, xs);
			iy1  = Libnoise.Lerp(ix0, ix1, ys);

			return Libnoise.Lerp(iy0, iy1, zs);

		}//end ValueCoherentNoise3D

		/// <summary>
		/// Generates a value-noise value from the coordinates of a
		/// three-dimensional input value.
		///
		/// The return value ranges from -1.0 to +1.0.
		///
		/// A noise function differs from a random-number generator because it
		/// always returns the same output value if the same input value is passed
		/// to it.
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <param name="y">The y coordinate of the input value</param>
		/// <param name="z">The z coordinate of the input value</param>
		/// <param name="seed">A random number seed</param>
		/// <returns>The generated value-noise value</returns>
		public static float ValueNoise3D(int x, int y, int z, long seed) {
			return 1.0f - ((float)IntValueNoise3D(x, y, z, seed) / 1073741824.0f);
		}  //end ValueNoise 

		/// <summary>
		/// Generates an integer-noise value from the coordinates of a
		/// three-dimensional input value.
		/// 
		/// The return value ranges from 0 to 2147483647.
		///
		/// A noise function differs from a random-number generator because it
		/// always returns the same output value if the same input value is passed
		/// to it.
		/// </summary>
		/// <param name="x">The integer x coordinate of the input value.</param>
		/// <param name="y">The integer y coordinate of the input value</param>
		/// <param name="z">The integer z coordinate of the input value</param>
		/// <param name="seed">A random number seed</param>
		/// <returns>The generated integer-noise value</returns>
		protected static int IntValueNoise3D(int x, int y, int z, long seed) {
			// All constants are primes and must remain prime in order for this noise
			// function to work correctly.
			long n = (
				X_NOISE_GEN * x
              + Y_NOISE_GEN * y
              + Z_NOISE_GEN * z
              + SEED_NOISE_GEN * seed
			 )& 0x7fffffff;

			n = (n >> 13) ^ n;
			return (int)(n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;

		}//end IntValueNoise3D

		#endregion

		#region IModule2D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y) {
			return ValueCoherentNoise2D(x, y, _seed, _quality);
		}//end GetValue

		#endregion

		#region ValueCoherentNoise2D

		/// <summary>
		/// Generates a value-coherent-noise value from the coordinates of a
		/// two-dimensional input value.
		///
		/// The return value ranges from -1.0 to +1.0.
		///
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <param name="y">The y coordinate of the input value</param>
		/// <param name="seed">The random number seed</param>
		/// <param name="quality">The quality of the coherent-noise</param>
		/// <returns>The generated value-coherent-noise value</returns>
		public float ValueCoherentNoise2D(float x, float y, long seed, NoiseQuality quality) {

			// Create a unit-length square aligned along an integer boundary.  
			// This square surrounds the input point.
			int x0 = (x > 0.0 ? (int)x: (int)x - 1);
			int x1 = x0 + 1;

			int y0 = (y > 0.0 ? (int)y: (int)y - 1);
			int y1 = y0 + 1;


			// Map the difference between the coordinates of the input value and the
			// coordinates of the cube's outer-lower-left vertex onto an S-curve.
			float xs = 0, ys = 0;

			switch(quality) {

				case NoiseQuality.Fast:
					xs = (x - (float)x0);
					ys = (y - (float)y0);
					break;

				case NoiseQuality.Standard:
					xs = Libnoise.SCurve3(x - (float)x0);
					ys = Libnoise.SCurve3(y - (float)y0);
					break;

				case NoiseQuality.Best:
					xs = Libnoise.SCurve5(x - (float)x0);
					ys = Libnoise.SCurve5(y - (float)y0);
					break;
			}//end switch

			// Now calculate the noise values at each point of the square.  To generate
			// the coherent-noise value at the input point, interpolate these four
			// noise values using the S-curve value as the interpolant (trilinear
			// interpolation.)
			float n0, n1, ix0, ix1;

			n0   = ValueNoise2D(x0, y0, seed);
			n1   = ValueNoise2D(x1, y0, seed);
			ix0  = Libnoise.Lerp(n0, n1, xs);

			n0   = ValueNoise2D(x0, y1, seed);
			n1   = ValueNoise2D(x1, y1, seed);
			ix1  = Libnoise.Lerp(n0, n1, xs);

			return Libnoise.Lerp(ix0, ix1, ys);

		}//end ValueCoherentNoise2D

		/// <summary>
		/// Generates a value-noise value from the coordinates of a
		/// two-dimensional input value.
		///
		/// The return value ranges from -1.0 to +1.0.
		///
		/// A noise function differs from a random-number generator because it
		/// always returns the same output value if the same input value is passed
		/// to it.
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <param name="y">The y coordinate of the input value</param>
		/// <param name="seed">A random number seed</param>
		/// <returns>The generated value-noise value</returns>
		public  float ValueNoise2D(int x, int y, long seed) {
			return 1.0f - ((float)IntValueNoise2D(x, y, seed) / 1073741824.0f);
		}  //end ValueNoise 

		/// <summary>
		/// Generates a value-noise value from the coordinates of a
		/// two-dimensional input value.
		///
		/// The return value ranges from -1.0 to +1.0.
		/// 
		/// it use ValueNoise2D(int x, int y, long seed) with a seed number of 0
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <param name="y">The y coordinate of the input value</param>
		/// <returns></returns>
		public  float ValueNoise2D(int x, int y) {
			return ValueNoise2D(x, y, _seed);
		}  //end ValueNoise 

		/// <summary>
		/// Generates an integer-noise value from the coordinates of a
		/// two-dimensional input value.
		/// 
		/// The return value ranges from 0 to 2147483647.
		///
		/// A noise function differs from a random-number generator because it
		/// always returns the same output value if the same input value is passed
		/// to it.
		/// </summary>
		/// <param name="x">The integer x coordinate of the input value.</param>
		/// <param name="y">The integer y coordinate of the input value</param>
		/// <param name="seed">A random number seed</param>
		/// <returns>The generated integer-noise value</returns>
		protected  int IntValueNoise2D(int x, int y, long seed) {
			// All constants are primes and must remain prime in order for this noise
			// function to work correctly.
			long n = (
				X_NOISE_GEN * x
              + Y_NOISE_GEN * y
              + SEED_NOISE_GEN * seed
			 )& 0x7fffffff;

			n = (n >> 13) ^ n;
			return (int)(n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;

		}//end IntValueNoise2D

		#endregion

		#region IModule1D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x) {
			return ValueCoherentNoise1D(x, _seed, _quality);
		}//end GetValue

		#endregion

		#region ValueNoise1D

		/// <summary>
		/// Generates a value-coherent-noise value from the coordinates of a
		/// one-dimensional input value.
		///
		/// The return value ranges from -1.0 to +1.0.
		///
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <param name="seed">The random number seed</param>
		/// <param name="quality">The quality of the coherent-noise</param>
		/// <returns>The generated value-coherent-noise value</returns>
		public static float ValueCoherentNoise1D(float x, long seed, NoiseQuality quality) {

			// Create a unit-length line aligned along an integer boundary.  
			// This line surrounds the input point.
			int x0 = (x > 0.0 ? (int)x: (int)x - 1);
			int x1 = x0 + 1;

			float xs = 0;

			switch(quality) {

				case NoiseQuality.Fast:
					xs = (x - (float)x0);
					break;

				case NoiseQuality.Standard:
					xs = Libnoise.SCurve3(x - (float)x0);
					break;

				case NoiseQuality.Best:
					xs = Libnoise.SCurve5(x - (float)x0);
					break;
			}//end switch

			// Now calculate the noise values at each point of the line.  To generate
			// the coherent-noise value at the input point, interpolate these two
			// noise values using the S-curve value as the interpolant (trilinear
			// interpolation.)
			float n0, n1;

			n0   = ValueNoise1D(x0, seed);
			n1   = ValueNoise1D(x1, seed);
			return Libnoise.Lerp(n0, n1, xs);

		}//end ValueCoherentNoise1D

		/// <summary>
		/// Generates a value-noise value from the coordinates of a
		/// one-dimensional input value.
		///
		/// The return value ranges from -1.0 to +1.0.
		///
		/// A noise function differs from a random-number generator because it
		/// always returns the same output value if the same input value is passed
		/// to it.
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <param name="seed">A random number seed</param>
		/// <returns>The generated value-noise value</returns>
		public static float ValueNoise1D(int x, long seed) {
			return 1.0f - ((float)IntValueNoise1D(x, seed) / 1073741824.0f);
		}  //end ValueNoise 

		/// <summary>
		/// Generates a value-noise value from the coordinates of a
		/// one-dimensional input value.
		///
		/// The return value ranges from -1.0 to +1.0.
		/// 
		/// it use ValueNoise1D(int x, long seed) with a seed number of 0
		/// </summary>
		/// <param name="x">The x coordinate of the input value</param>
		/// <returns></returns>
		public static float ValueNoise1D(int x) {
			return ValueNoise1D(x, 0);
		}  //end ValueNoise1D 

		/// <summary>
		/// Generates an integer-noise value from the coordinates of a
		/// one-dimensional input value.
		/// 
		/// The return value ranges from 0 to 2147483647.
		///
		/// A noise function differs from a random-number generator because it
		/// always returns the same output value if the same input value is passed
		/// to it.
		/// </summary>
		/// <param name="x">The integer x coordinate of the input value.</param>
		/// <param name="seed">A random number seed</param>
		/// <returns>The generated integer-noise value</returns>
		protected static int IntValueNoise1D(int x, long seed) {
			// All constants are primes and must remain prime in order for this noise
			// function to work correctly.
			long n = (
				X_NOISE_GEN * x
              + SEED_NOISE_GEN * seed
			 )& 0x7fffffff;

			n = (n >> 13) ^ n;
			return (int)(n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;

		}//end IntValueNoise2D

		#endregion

	}//end class

}//end namespace