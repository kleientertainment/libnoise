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

namespace LibNoiseDotNet.Graphics.Tools.Noise{

	/// <summary>
	/// 
	/// </summary>
	public static class Libnoise {

		#region Constants

		/// <summary>
		/// Version
		/// </summary>
		public const string VERSION = "1.0.0 B";

		/// <summary>
		/// Pi
		/// </summary>
		public const float PI = 3.1415926535897932385f;

		/// <summary>
		/// Square root of 2.
		/// </summary>
		public const float SQRT_2 = 1.4142135623730950488f;

		/// <summary>
		/// Square root of 3.
		/// </summary>
		public const float SQRT_3 = 1.7320508075688772935f;

		/// <summary>
		/// Square root of 5.
		/// </summary>
		public const float SQRT_5 = 2.2360679774997896964f;

		/// <summary>
		/// Converts an angle from degrees to radians.
		/// </summary>
		public const float DEG2RAD = PI / 180.0f;

		/// <summary>
		/// Converts an angle from radians to degrees.
		/// </summary>
		public const float RAD2DEG = 1.0f / DEG2RAD;

		#endregion

		#region Misc
		/// <summary>
		/// Converts latitude/longitude coordinates on a unit sphere into 3D Cartesian coordinates.
		/// 
		/// </summary>
		/// <param name="lat">The latitude, in degrees. Must range from -90 to +90</param>
		/// <param name="lon">The longitude, in degrees. Must range from -180 to +180</param>
		/// <param name="x">By ref, this parameter contains the x coordinate</param>
		/// <param name="y">By ref, this parameter contains the y coordinate</param>
		/// <param name="z">By ref, this parameter contains the z coordinate</param>
		public static void LatLonToXYZ(float lat, float lon, ref float x, ref float y, ref float z) {

			float r = (float)Math.Cos(DEG2RAD * lat);
			x = r * (float)Math.Cos(DEG2RAD * lon);
			y =     (float)Math.Sin(DEG2RAD * lat);
			z = r * (float)Math.Sin(DEG2RAD * lon);

		}//endLatLonToXYZ

		#endregion

		#region Interpolation methods

		/// <summary>
		/// Performs linear interpolation between two byte-values by a.
		///
		/// The amount value should range from 0.0 to 1.0.  If the amount value is
		/// 0.0, this function returns n0.  If the amount value is 1.0, this
		/// function returns n1.
		/// </summary>
		/// <param name="n0">The first value.</param>
		/// <param name="n1">The second value</param>
		/// <param name="a">the amount to interpolate between the two values</param>
		/// <returns>The interpolated value</returns>
		public static byte Lerp(byte n0, byte n1, float a) {

			float c0 = (float)n0 / 255.0f;
			float c1 = (float)n1 / 255.0f;

			return (byte)((c0 + a * (c1 - c0)) * 255.0f);

		}//end Lerp

		/// <summary>
		/// Performs linear interpolation between two float-values by a.
		///
		/// The amount value should range from 0.0 to 1.0.  If the amount value is
		/// 0.0, this function returns n0.  If the amount value is 1.0, this
		/// function returns n1.
		/// </summary>
		/// <param name="n0">The first value.</param>
		/// <param name="n1">The second value</param>
		/// <param name="a">the amount to interpolate between the two values</param>
		/// <returns>The interpolated value</returns>
		public static float Lerp(float n0, float n1, float a) {
			//return ((1.0 - a) * n0) + (a * n1);
			return n0 + a * (n1 - n0);
		}//end Lerp

		/// <summary>
		/// Performs cubic interpolation between two values bound between two other values.
		///
		/// The amount value should range from 0.0 to 1.0.  If the amount value is
		/// 0.0, this function returns n1.  If the amount value is 1.0, this
		/// function returns n2.
		/// </summary>
		/// <param name="n0">The value before the first value</param>
		/// <param name="n1">The first value</param>
		/// <param name="n2">The second value</param>
		/// <param name="n3">The value after the second value</param>
		/// <param name="a">the amount to interpolate between the two values</param>
		/// <returns>The interpolated value.</returns>
		public static float Cerp(float n0, float n1, float n2, float n3, float a) {
			float p = (n3 - n2) - (n0 - n1);
			float q = (n0 - n1) - p;
			float r = n2 - n0;
			float s = n1;
			return p * a * a * a + q * a * a + r * a + s;
		}//end Cerp

		/// <summary>
		/// Maps a value onto a cubic S-curve.
		/// a should range from 0.0 to 1.0.
		/// The derivitive of a cubic S-curve is zero at a = 0.0 and a = 1.0
		/// </summary>
		/// <param name="a">The value to map onto a cubic S-curve</param>
		/// <returns>The mapped value</returns>
		public static float SCurve3(float a) {
			return (a * a * (3.0f - 2.0f * a));
		}//end SCurve3

		/// <summary>
		/// Maps a value onto a quintic S-curve.
		/// a should range from 0.0 to 1.0.
		/// The first derivitive of a quintic S-curve is zero at a = 0.0 and a = 1.0
		/// The second derivitive of a quintic S-curve is zero at a = 0.0 and a = 1.0
		/// </summary>
		/// <param name="a">The value to map onto a quintic S-curve</param>
		/// <returns>The mapped value</returns>
		public static float SCurve5(float a) {
			return a * a * a * (a * (a * 6.0f - 15.0f) + 10.0f);

			/* original libnoise code
			double a3 = a * a * a;
			double a4 = a3 * a;
			double a5 = a4 * a;
			return (6.0 * a5) - (15.0 * a4) + (10.0 * a3);
			*/
		}//end SCurve5

		#endregion

		#region Variables utility

		/// <summary>
		/// Clamps a value onto a clamping range.
		///
		/// This function does not modify any parameters.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="lowerBound">The lower bound of the clamping range</param>
		/// <param name="upperBound">The upper bound of the clamping range</param>
		/// <returns>		
		/// - value if value lies between lowerBound and upperBound.
		/// - lowerBound if value is less than lowerBound.
		/// - upperBound if value is greater than upperBound.
		/// </returns>
		public static int Clamp(int value, int lowerBound, int upperBound) {
			if(value < lowerBound) {
				return lowerBound;
			}//end if
			else if(value > upperBound) {
				return upperBound;
			}//end elseif
			else {
				return value;
			}//end else
		}//end Clamp

		public static float Clamp(float value, float lowerBound, float upperBound) {
			if(value < lowerBound) {
				return lowerBound;
			}//end if
			else if(value > upperBound) {
				return upperBound;
			}//end elseif
			else {
				return value;
			}//end else
		}//end Clamp

		public static double Clamp(double value, double lowerBound, double upperBound) {
			if(value < lowerBound) {
				return lowerBound;
			}//end if
			else if(value > upperBound) {
				return upperBound;
			}//end elseif
			else {
				return value;
			}//end else
		}//end Clamp

		public static int Clamp01(int value) {
			return Clamp(value, 0, 1);
		}//end Clamp

		public static float Clamp01(float value) {
			return Clamp(value, 0, 1);
		}//end Clamp

		public static double Clamp01(double value) {
			return Clamp(value, 0, 1);
		}//end Clamp
		
		/// <summary>
		/// Swaps two values.
		/// 
		/// The values within the the two variables are swapped.
		/// </summary>
		/// <param name="a">A variable containing the first value.</param>
		/// <param name="b">A variable containing the second value.</param>
		public static void SwapValues<T>(ref T a, ref T b) {
			T c = a;
			a = b;
			b = c;
		}//end SwapValues

		public static void SwapValues(ref double a, ref double b) {
			SwapValues<double>(ref a, ref b);
		}//end SwapValues

		public static void SwapValues(ref int a, ref int b) {
			SwapValues<int>(ref a, ref b);
		}//end SwapValues

		public static void SwapValues(ref float a, ref float b) {
			SwapValues<float>(ref a, ref b);
		}//end SwapValues

		/// <summary>
		/// Modifies a floating-point value so that it can be stored in a
		/// int 32 bits variable.
		///
		/// In libnoise, the noise-generating algorithms are all integer-based;
		/// they use variables of type int 32 bits.  Before calling a noise
		/// function, pass the x, y, and z coordinates to this function to
		/// ensure that these coordinates can be cast to a int 32 bits value.
		///
		/// Although you could do a straight cast from double to int 32 bits, the
		/// resulting value may differ between platforms.  By using this function,
		/// you ensure that the resulting value is identical between platforms.
		/// </summary>
		/// <param name="value">A floating-point number</param>
		/// <returns>The modified floating-point number</returns>
		public static double ToInt32Range(double value) {

			if(value >= 1073741824.0) { 
				return (2.0 * Math.IEEERemainder(value, 1073741824.0)) - 1073741824.0; 
			}//end if
			else if(value <= -1073741824.0) { 
				return (2.0 * Math.IEEERemainder(value, 1073741824.0)) + 1073741824.0; 
			}//end elseif
			else { 
				return value; 
			}//end else

		}//end MakeInt32Range

		/// <summary>
		/// Unpack the given integer (int32) value to an array of 4 bytes in big endian format.
		/// If the length of the buffer is too smal, it wil be resized.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="convert">the output buffer</param>
		static public byte[] UnpackBigUint32(int value, ref byte[] buffer) {

			if(buffer.Length < 4){
				Array.Resize<byte>(ref buffer, 4);
			}//end if

			buffer[0] = (byte)(value >> 24);
			buffer[1] = (byte)(value >> 16);
			buffer[2] = (byte)(value >> 8);
			buffer[3] = (byte)(value);

			return buffer;

		}// end public

		/// <summary>
		/// Unpack the given float to an array of 4 bytes in big endian format.
		/// If the length of the buffer is too smal, it wil be resized.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="convert">the output buffer</param>
		static public byte[] UnpackBigFloat(float value, ref byte[] buffer) {

			throw new NotImplementedException();
			/*
			if(buffer.Length < 4) {
				Array.Resize<byte>(ref buffer, 4);
			}//end if
			
			buffer[0] = (byte)(value >> 24);
			buffer[1] = (byte)(value >> 16);
			buffer[2] = (byte)(value >> 8);
			buffer[3] = (byte)(value);
			
			return buffer;
			*/
		}// end public

		/// <summary>
		/// Unpack the given short (int16) value to an array of 2 bytes in big endian format.
		/// If the length of the buffer is too smal, it wil be resized.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="convert">the output buffer</param>
		static public byte[] UnpackBigUint16(short value, ref byte[] buffer) {

			if(buffer.Length < 2) {
				Array.Resize<byte>(ref buffer, 2);
			}//end if

			buffer[0] = (byte)(value >> 8);
			buffer[1] = (byte)(value);

			return buffer;

		}// end public

		/// <summary>
		/// Unpack the given short (int16) to an array of 2 bytes  in little endian format.
		/// If the length of the buffer is too smal, it wil be resized.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="convert">the output buffer</param>
		static public byte[] UnpackLittleUint16(short value, ref byte[] buffer) {

			if(buffer.Length < 2) {
				Array.Resize<byte>(ref buffer, 2);
			}//end if

			buffer[0] = (byte)(value & 0x00ff);
			buffer[1] = (byte)((value & 0xff00) >> 8);

			return buffer;

		}// end public

		/// <summary>
		/// Unpack the given integer (int32) to an array of 4 bytes  in little endian format.
		/// If the length of the buffer is too smal, it wil be resized.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="convert">the output buffer</param>
		static public byte[] UnpackLittleUint32(int value, ref byte[] buffer) {

			if(buffer.Length < 4) {
				Array.Resize<byte>(ref buffer, 4);
			}//end if

			buffer[0] = (byte)(value & 0x00ff);
			buffer[1] = (byte)((value & 0xff00) >> 8);
			buffer[2] = (byte)((value & 0x00ff0000) >> 16);
			buffer[3] = (byte)((value & 0xff000000) >> 24);

			return buffer;

		}// end public

		/// <summary>
		/// Unpack the given float (int32) to an array of 4 bytes  in little endian format.
		/// If the length of the buffer is too smal, it wil be resized.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="convert">the output buffer</param>
		static public byte[] UnpackLittleFloat(float value, ref byte[] buffer) {
			
			throw new NotImplementedException();

			/*
			if(buffer.Length < 4) {
				Array.Resize<byte>(ref buffer, 4);
			}//end if

			buffer[0] = (byte)(value & 0x00ff);
			buffer[1] = (byte)((value & 0xff00) >> 8);
			buffer[2] = (byte)((value & 0x00ff0000) >> 16);
			buffer[3] = (byte)((value & 0xff000000) >> 24);
			
			return buffer;
*/
		}// end public

		/// <summary>
		/// faster methid than using (int)Math.floor(x).
		/// </summary>
		/// <param name="x"></param>
		static public int FastFloor(double x){
			return x >= 0 ? (int) x : (int) x - 1;
		}//end FastFloor

		/// <summary>
		/// faster methid than using (int)Math.floor(x).
		/// </summary>
		/// <param name="x"></param>
		static public int FastFloor(float x) {
			return x >= 0 ? (int)x : (int)x - 1;
		}//end FastFloor

		#endregion

	}//end class

}//end namespace
