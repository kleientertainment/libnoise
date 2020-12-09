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
// Simplex Noise in 2D, 3D and 4D. Based on the example code of this paper:
// http://staffwww.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf
// 
// From Stefan Gustavson, Linkping University, Sweden (stegu at itn dot liu dot se)
// From Karsten Schmidt (slight optimizations & restructuring)

using System;
namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive {

	/// <summary>
	/// Noise module that outputs 3-dimensional Simplex Perlin noise.
	/// 
	/// This algorithm has a computational cost of O(n+1) where n is the dimension.
	/// It's a lost faster than ImprovedPerlin (O(2^n))
	/// 
	/// Quality is not used here.
	/// 
	/// This noise module outputs values that usually range from
	/// -1.0 to +1.0, but there are no guarantees that all output values will
	/// exist within that range.
	/// </summary>
	/// </summary>
	public class SimplexPerlin :ImprovedPerlin, IModule4D, IModule3D, IModule2D {

		#region Fields

		/// Skewing and unskewing factors for 2D, 3D and 4D, 
		/// some of them pre-multiplied.
		protected static float F2 = 0.5f * (Libnoise.SQRT_3 - 1.0f);
		protected static float G2 = (3.0f - Libnoise.SQRT_3) / 6.0f;
		protected static float G22 = G2 * 2.0f - 1f;

		protected static float F3 = 1.0f / 3.0f;
		protected static float G3 = 1.0f / 6.0f;

		protected static float F4 = (Libnoise.SQRT_5 - 1.0f) / 4.0f;
		protected static float G4 = (5.0f - Libnoise.SQRT_5) / 20.0f;
		protected static float G42 = G4 * 2.0f;
		protected static float G43 = G4 * 3.0f;
		protected static float G44 = G4 * 4.0f - 1.0f;

		/// <summary>
		/// Gradient vectors for 3D (pointing to mid points of all edges of a unit
		/// cube)
		/// </summary>
		protected static int[][] _grad3 = new int[][]{ 
				new int[]{  1,  1,  0 } , new int[]{ -1, 1,  0 } , new int[]{  1, -1,  0 },
				new int[]{ -1, -1,  0 } , new int[]{  1, 0,  1 } , new int[]{ -1,  0,  1 }, 
				new int[]{  1,  0, -1 } , new int[]{ -1, 0, -1 } , new int[]{  0,  1,  1 }, 
				new int[]{  0, -1,  1 } , new int[]{  0, 1, -1 } , new int[]{  0, -1, -1 } 
		};

		/// <summary>
		/// Gradient vectors for 4D (pointing to mid points of all edges of a unit 4D
		/// hypercube)
		/// </summary>
		protected static int[][] _grad4 = { 
				new int[]{  0,  1, 1, 1 } , new int[]{  0,  1,  1, -1 }, new int[]{  0,  1, -1, 1 }, new int[]{  0,  1, -1, -1 }, 
				new int[]{  0, -1, 1, 1 } , new int[]{  0, -1,  1, -1 }, new int[]{  0, -1, -1, 1 }, new int[]{  0, -1, -1, -1 },
				new int[]{  1,  0, 1, 1 } , new int[]{  1,  0,  1, -1 }, new int[]{  1,  0, -1, 1 }, new int[]{  1,  0, -1, -1 },
				new int[]{ -1,  0, 1, 1 } , new int[]{ -1,  0,  1, -1 }, new int[]{ -1,  0, -1, 1 }, new int[]{ -1,  0, -1, -1 }, 
				new int[]{  1,  1, 0, 1 } , new int[]{  1,  1,  0, -1 }, new int[]{  1, -1,  0, 1 }, new int[]{  1, -1,  0, -1 }, 
				new int[]{ -1,  1, 0, 1 } , new int[]{ -1,  1,  0, -1 }, new int[]{ -1, -1,  0, 1 }, new int[]{ -1, -1,  0, -1 },
				new int[]{  1,  1, 1, 0 } , new int[]{  1,  1, -1,  0 }, new int[]{  1, -1,  1, 0 }, new int[]{  1, -1, -1,  0 },
				new int[]{ -1,  1, 1, 0 } , new int[]{ -1,  1, -1,  0 }, new int[]{ -1, -1,  1, 0 }, new int[]{ -1, -1, -1,  0 } 
		};

		/// <summary>
		/// A lookup table to traverse the simplex around a given point in 4D.
		/// Details can be found where this table is used, in the 4D noise method.
		/// </summary>
		protected static int[][] _simplex = { 
				new int[]{ 0, 1, 2, 3 }, new int[]{ 0, 1, 3, 2 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 2, 3, 1 }, 
				new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 1, 2, 3, 0 }, 
				new int[]{ 0, 2, 1, 3 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 3, 1, 2 }, new int[]{ 0, 3, 2, 1 }, 
				new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 1, 3, 2, 0 }, 
				new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, 
				new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, 
				new int[]{ 1, 2, 0, 3 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 1, 3, 0, 2 }, new int[]{ 0, 0, 0, 0 }, 
				new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 2, 3, 0, 1 }, new int[]{ 2, 3, 1, 0 }, 
				new int[]{ 1, 0, 2, 3 }, new int[]{ 1, 0, 3, 2 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, 
				new int[]{ 0, 0, 0, 0 }, new int[]{ 2, 0, 3, 1 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 2, 1, 3, 0 }, 
				new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 },
				new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, 
				new int[]{ 2, 0, 1, 3 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, 
				new int[]{ 3, 0, 1, 2 }, new int[]{ 3, 0, 2, 1 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 3, 1, 2, 0 }, 
				new int[]{ 2, 1, 0, 3 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 0, 0, 0, 0 }, 
				new int[]{ 3, 1, 0, 2 }, new int[]{ 0, 0, 0, 0 }, new int[]{ 3, 2, 0, 1 }, new int[]{ 3, 2, 1, 0 }  
		};

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public SimplexPerlin()
			: base(DEFAULT_SEED, DEFAULT_QUALITY) {

		}//end SimplexPerlin

		/// <summary>
		/// Create a new ImprovedPerlin with given values
		/// </summary>
		/// <param name="seed"></param>
		/// <param name="quality"></param>
		public SimplexPerlin(int seed, NoiseQuality quality)
			: base (seed, quality){
			
		}//end SimplexPerlin

		#endregion

		#region IModule4D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <param name="w">The input coordinate on the w-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y, float z, float w) {

			// The skewing and unskewing factors are hairy again for the 4D case
			// Noise contributions
			float n0 = 0, n1 = 0, n2 = 0, n3 = 0, n4 = 0; 

			// from the five corners
			// Skew the (x,y,z,w) space to determine which cell of 24 simplices
			float s = (x + y + z + w) * F4; // Factor for 4D skewing

			int i = Libnoise.FastFloor(x + s);
			int j = Libnoise.FastFloor(y + s);
			int k = Libnoise.FastFloor(z + s);
			int l = Libnoise.FastFloor(w + s);

			float t = (i + j + k + l) * G4; // Factor for 4D unskewing

			// The x,y,z,w distances from the cell origin
			float x0 = x - (i - t); 
			float y0 = y - (j - t);
			float z0 = z - (k - t);
			float w0 = w - (l - t);

			// For the 4D case, the simplex is a 4D shape I won't even try to
			// describe.
			// To find out which of the 24 possible simplices we're in, we need to
			// determine the magnitude ordering of x0, y0, z0 and w0.
			// The method below is a good way of finding the ordering of x,y,z,w and
			// then find the correct traversal order for the simplex were in.
			// First, six pair-wise comparisons are performed between each possible
			// pair of the four coordinates, and the results are used to add up
			// binary bits for an integer index.
			int c = 0;

			if(x0 > y0) {
				c = 0x20;
			}//end if

			if(x0 > z0) {
				c |= 0x10;
			}//end if

			if(y0 > z0) {
				c |= 0x08;
			}//end if

			if(x0 > w0) {
				c |= 0x04;
			}//end if

			if(y0 > w0) {
				c |= 0x02;
			}//end if

			if(z0 > w0) {
				c |= 0x01;
			}//end if

			int i1, j1, k1, l1; // The integer offsets for the second simplex corner
			int i2, j2, k2, l2; // The integer offsets for the third simplex corner
			int i3, j3, k3, l3; // The integer offsets for the fourth simplex corner

			// simplex[c] is a 4-vector with the numbers 0, 1, 2 and 3 in some
			// order. Many values of c will never occur, since e.g. x>y>z>w makes
			// x<z, y<w and x<w impossible. Only the 24 indices which have non-zero
			// entries make any sense. We use a thresholding to set the coordinates
			// in turn from the largest magnitude. The number 3 in the "simplex"
			// array is at the position of the largest coordinate.
			int[] sc = _simplex[c];

			i1 = sc[0] >= 3 ? 1 : 0;
			j1 = sc[1] >= 3 ? 1 : 0;
			k1 = sc[2] >= 3 ? 1 : 0;
			l1 = sc[3] >= 3 ? 1 : 0;

			// The number 2 in the "simplex" array is at the second largest coordinate.
			i2 = sc[0] >= 2 ? 1 : 0;
			j2 = sc[1] >= 2 ? 1 : 0;
			k2 = sc[2] >= 2 ? 1 : 0;
			l2 = sc[3] >= 2 ? 1 : 0;

			// The number 1 in the "simplex" array is at the second smallest coordinate.
			i3 = sc[0] >= 1 ? 1 : 0;
			j3 = sc[1] >= 1 ? 1 : 0;
			k3 = sc[2] >= 1 ? 1 : 0;
			l3 = sc[3] >= 1 ? 1 : 0;

			// The fifth corner has all coordinate offsets = 1, so no need to look that up.
			float x1 = x0 - i1 + G4; // Offsets for second corner in (x,y,z,w)
			float y1 = y0 - j1 + G4;
			float z1 = z0 - k1 + G4;
			float w1 = w0 - l1 + G4;

			float x2 = x0 - i2 + G42; // Offsets for third corner in (x,y,z,w)
			float y2 = y0 - j2 + G42;
			float z2 = z0 - k2 + G42;
			float w2 = w0 - l2 + G42;

			float x3 = x0 - i3 + G43; // Offsets for fourth corner in (x,y,z,w)
			float y3 = y0 - j3 + G43;
			float z3 = z0 - k3 + G43;
			float w3 = w0 - l3 + G43;

			float x4 = x0 + G44; // Offsets for last corner in (x,y,z,w)
			float y4 = y0 + G44;
			float z4 = z0 + G44;
			float w4 = w0 + G44;

			// Work out the hashed gradient indices of the five simplex corners
			int ii = i & 0xff;
			int jj = j & 0xff;
			int kk = k & 0xff;
			int ll = l & 0xff;

			// Calculate the contribution from the five corners
			float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0 - w0 * w0;

			if(t0 > 0) {
				t0 *= t0;
				int gi0 = _random[ii + _random[jj + _random[kk + _random[ll]]]] % 32;
				n0 = t0 * t0 * Dot(_grad4[gi0], x0, y0, z0, w0);
			}//end if

			float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1 - w1 * w1;
			if(t1 > 0) {
				t1 *= t1;
				int gi1 = _random[ii + i1 + _random[jj + j1 + _random[kk + k1 + _random[ll + l1]]]] % 32;
				n1 = t1 * t1 * Dot(_grad4[gi1], x1, y1, z1, w1);
			}//end if

			float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2 - w2 * w2;
			if(t2 > 0) {
				t2 *= t2;
				int gi2 = _random[ii + i2 + _random[jj + j2 + _random[kk + k2 + _random[ll + l2]]]] % 32;
				n2 = t2 * t2 * Dot(_grad4[gi2], x2, y2, z2, w2);
			}//end if

			float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3 - w3 * w3;
			if(t3 > 0) {
				t3 *= t3;
				int gi3 = _random[ii + i3 + _random[jj + j3 + _random[kk + k3 + _random[ll + l3]]]] % 32;
				n3 = t3 * t3 * Dot(_grad4[gi3], x3, y3, z3, w3);
			}//end if

			float t4 = 0.6f - x4 * x4 - y4 * y4 - z4 * z4 - w4 * w4;
			if(t4 > 0) {
				t4 *= t4;
				int gi4 = _random[ii + 1 + _random[jj + 1 + _random[kk + 1 + _random[ll + 1]]]] % 32;
				n4 = t4 * t4 * Dot(_grad4[gi4], x4, y4, z4, w4);
			}//end if

			// Sum up and scale the result to cover the range [-1,1]
			return 27.0f * (n0 + n1 + n2 + n3 + n4);

		}//end GetValue

		/// <summary>
		/// Computes dot product in 4D.
		/// </summary>
		/// <param name="g"4-vector (grid offset)</param>
		/// <param name="x">x coordinates</param>
		/// <param name="y">y coordinates</param>
		/// <param name="z">z coordinates</param>
		/// <param name="t">t coordinates</param>
		/// <returns>dot product</returns>
		protected float Dot(int[] g, float x, float y, float z, float t) {
			return g[0] * x + g[1] * y + g[2] * z + g[3] * t;
		}//end Dot

		#endregion

		#region IModule3D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <returns>The resulting output value.</returns>
		public new float GetValue(float x, float y, float z) {

			float n0 = 0, n1 = 0, n2 = 0, n3 = 0;

			// Noise contributions from the four corners
			// Skew the input space to determine which simplex cell we're in
			float s = (x + y + z) * F3;

			// for 3D
			int i = Libnoise.FastFloor(x + s);
			int j = Libnoise.FastFloor(y + s);
			int k = Libnoise.FastFloor(z + s);

			float t = (i + j + k) * G3;

			// The x,y,z distances from the cell origin
			float x0 = x - (i - t); 
			float y0 = y - (j - t);
			float z0 = z - (k - t);

			// For the 3D case, the simplex shape is a slightly irregular tetrahedron.
			// Determine which simplex we are in.
			// Offsets for second corner of simplex in (i,j,k)
			int i1, j1, k1; 

			// coords
			int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

			if(x0 >= y0) {
				if(y0 >= z0) {// X Y Z order
					i1 = 1;
					j1 = 0;
					k1 = 0;
					i2 = 1;
					j2 = 1;
					k2 = 0;
				} //end if
				else if(x0 >= z0) {// X Z Y order
					i1 = 1;
					j1 = 0;
					k1 = 0;
					i2 = 1;
					j2 = 0;
					k2 = 1;
				} //end if
				else {// Z X Y order
					i1 = 0;
					j1 = 0;
					k1 = 1;
					i2 = 1;
					j2 = 0;
					k2 = 1;
				}//end else
			}//end if
			else { // x0 < y0
				if(y0 < z0) {// Z Y X order
					i1 = 0;
					j1 = 0;
					k1 = 1;
					i2 = 0;
					j2 = 1;
					k2 = 1;
				} //end if
				else if(x0 < z0) {// Y Z X order
					i1 = 0;
					j1 = 1;
					k1 = 0;
					i2 = 0;
					j2 = 1;
					k2 = 1;
				} //end if
				else {// Y X Z order
					i1 = 0;
					j1 = 1;
					k1 = 0;
					i2 = 1;
					j2 = 1;
					k2 = 0;
				}//end else
			}//end else

			// A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
			// a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z),
			// and
			// a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z),
			// where c = 1/6.

			// Offsets for second corner in (x,y,z) coords
			float x1 = x0 - i1 + G3; 
			float y1 = y0 - j1 + G3;
			float z1 = z0 - k1 + G3;

			// Offsets for third corner in (x,y,z)
			float x2 = x0 - i2 + F3; 
			float y2 = y0 - j2 + F3;
			float z2 = z0 - k2 + F3;

			// Offsets for last corner in (x,y,z)
			float x3 = x0 - 0.5f; 
			float y3 = y0 - 0.5f;
			float z3 = z0 - 0.5f;

			// Work out the hashed gradient indices of the four simplex corners
			int ii = i & 0xff;
			int jj = j & 0xff;
			int kk = k & 0xff;

			// Calculate the contribution from the four corners
			float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
			if(t0 > 0) {
				t0 *= t0;
				int gi0 = _random[ii + _random[jj + _random[kk]]] % 12;
				n0 = t0 * t0 * Dot(_grad3[gi0], x0, y0, z0);
			}//end if

			float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
			if(t1 > 0) {
				t1 *= t1;
				int gi1 = _random[ii + i1 + _random[jj + j1 + _random[kk + k1]]] % 12;
				n1 = t1 * t1 * Dot(_grad3[gi1], x1, y1, z1);
			}//end if

			float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
			if(t2 > 0) {
				t2 *= t2;
				int gi2 = _random[ii + i2 + _random[jj + j2 + _random[kk + k2]]] % 12;
				n2 = t2 * t2 * Dot(_grad3[gi2], x2, y2, z2);
			}//end if

			float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
			if(t3 > 0) {
				t3 *= t3;
				int gi3 = _random[ii + 1 + _random[jj + 1 + _random[kk + 1]]] % 12;
				n3 = t3 * t3 * Dot(_grad3[gi3], x3, y3, z3);
			}//end if

			// Add contributions from each corner to get the final noise value.
			// The result is scaled to stay just inside [-1,1]
			return 32.0f * (n0 + n1 + n2 + n3);

		}//end GetValue


		/// <summary>
		/// Computes dot product in 3D.
		/// </summary>
		/// <param name="g"3-vector (grid offset)</param>
		/// <param name="x">x coordinates</param>
		/// <param name="y">y coordinates</param>
		/// <param name="z">z coordinates</param>
		/// <returns>dot product</returns>
		protected float Dot(int[] g, float x, float y, float z) {
			return g[0] * x + g[1] * y + g[2] * z;
		}//end Dot

		#endregion

		#region IModule2D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <returns>The resulting output value.</returns>
		public new float GetValue(float x, float y) {

			// Noise contributions from the three corners
			float n0 = 0, n1 = 0, n2 = 0; 

			// Skew the input space to determine which simplex cell we're in
			float s = (x + y) * F2; // Hairy factor for 2D

			int i = Libnoise.FastFloor(x + s);
			int j = Libnoise.FastFloor(y + s);

			float t = (i + j) * G2;

			// The x,y distances from the cell origin
			float x0 = x - (i - t); 
			float y0 = y - (j - t);

			// For the 2D case, the simplex shape is an equilateral triangle.
			// Determine which simplex we are in.

			// Offsets for second (middle) corner of simplex in (i,j)
			int i1, j1; 

			if(x0 > y0){// lower triangle, XY order: (0,0)->(1,0)->(1,1)
				i1 = 1;
				j1 = 0;
			}//end if
			else {// upper triangle, YX order: (0,0)->(0,1)->(1,1)
				i1 = 0;
				j1 = 1;
			}//end else

			// A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
			// a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
			// c = (3-sqrt(3))/6
			float x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed
			float y1 = y0 - j1 + G2;
			float x2 = x0 + G22; // Offsets for last corner in (x,y) unskewed
			float y2 = y0 + G22;

			// Work out the hashed gradient indices of the three simplex corners
			int ii = i & 0xff;
			int jj = j & 0xff;

			// Calculate the contribution from the three corners
			float t0 = 0.5f - x0 * x0 - y0 * y0;

			if(t0 > 0){
				t0 *= t0;
				int gi0 = _random[ii + _random[jj]] % 12;
				n0 = t0 * t0 * Dot(_grad3[gi0], x0, y0); // (x,y) of grad3 used for
				// 2D gradient
			}//end if

			float t1 = 0.5f - x1 * x1 - y1 * y1;

			if(t1 > 0){
				t1 *= t1;
				int gi1 = _random[ii + i1 + _random[jj + j1]] % 12;
				n1 = t1 * t1 * Dot(_grad3[gi1], x1, y1);
			}//end if

			float t2 = 0.5f - x2 * x2 - y2 * y2;
			if(t2 > 0){
				t2 *= t2;
				int gi2 = _random[ii + 1 + _random[jj + 1]] % 12;
				n2 = t2 * t2 * Dot(_grad3[gi2], x2, y2);
			}//end if

			// Add contributions from each corner to get the final noise value.
			// The result is scaled to return values in the interval [-1,1].
			return 70.0f * (n0 + n1 + n2);

		}//end GetValue

		/// <summary>
		/// Computes dot product in 2D.
		/// </summary>
		/// <param name="g">2-vector (grid offset)</param>
		/// <param name="x">x coordinates</param>
		/// <param name="y">y coordinates</param>
		/// <returns>dot product</returns>
		protected float Dot(int[] g, float x, float y) {
			return g[0] * x + g[1] * y;
		}//end Dot

		#endregion

	}//end class

}//end namespace
