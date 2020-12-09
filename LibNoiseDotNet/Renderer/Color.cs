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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer {

	/// <summary>
	/// Defines a color.
	///
	/// A color object contains four 8-bit channels: red, green, blue, and an
	/// alpha (transparency) channel.  Channel values range from 0 to 255.
	///
	/// The alpha channel defines the transparency of the color.  If the alpha
	/// channel has a value of 0, the color is completely transparent.  If the
	/// alpha channel has a value of 255, the color is completely opaque.
	/// </summary>
	public class Color :IEquatable<Color>, IColor {

		#region fields

		/// <summary>
		/// Compute a grayscale value from the source color
		/// </summary>
		/// <param name="color">The source color</param>
		/// <returns>The computed channel value</returns>
		public delegate byte GrayscaleStrategy(IColor color);

		/// <summary>
		/// Value of the red channel
		/// </summary>
		protected byte _red = 0;

		/// <summary>
		/// Value of the green channel
		/// </summary>
		protected byte _green = 0;

		/// <summary>
		/// Value of the blue channel
		/// </summary>
		protected byte _blue = 0;

		/// <summary>
		/// Value of the alpha (transparency) channel.
		/// </summary>
		protected byte _alpha = 255;

		/// <summary>
		/// Internal hashcode
		/// </summary>
		protected int _hashcode;

		/// <summary>
		/// Static Random generator
		/// </summary>
		private static Random _rnd = new Random(666);

		#endregion

		#region Properties
		/// <summary>
		/// The red channel
		/// </summary>
		public byte Red {
			get { return _red; }
			set { _red = value; }
		}

		/// <summary>
		/// The green channel
		/// </summary>
		public byte Green {
			get { return _green; }
			set { _green = value; }
		}

		/// <summary>
		/// The blue channel
		/// </summary>
		public byte Blue {
			get { return _blue; }
			set { _blue = value; }
		}

		/// <summary>
		/// The alpha channel
		/// </summary>
		public byte Alpha {
			get { return _alpha; }
			set { _alpha = value; }
		}

		/// <summary>
		/// Create a black color
		/// </summary>
		public static Color BLACK {
			get { return new Color(0, 0, 0, 255); }
		}

		/// <summary>
		/// Create a white color
		/// </summary>
		public static Color WHITE {
			get { return new Color(255, 255, 255, 255); }
		}

		/// <summary>
		/// Create a solid red color
		/// </summary>
		public static Color RED {
			get { return new Color(255, 0, 0, 255); }
		}

		/// <summary>
		/// Create a solid green color
		/// </summary>
		public static Color GREEN {
			get { return new Color(0, 255, 0, 255); }
		}

		/// <summary>
		/// Create a solid blue color
		/// </summary>
		public static Color BLUE {
			get { return new Color(0, 0, 255, 255); }
		}

		/// <summary>
		/// Create a transparent color
		/// </summary>
		public static Color TRANSPARENT {
			get { return new Color(0, 0, 0, 0); }
		}

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor, a solid black color (0, 0, 0, 255)
		/// </summary>
		public Color() {
			_hashcode = (_red +_green +_blue) ^_rnd.Next();
		}//end Color


		/// <summary>
		/// Create a new Color
		/// </summary>
		/// <param name="r">Value of the red channel</param>
		/// <param name="g">Value of the green channel</param>
		/// <param name="b">Value of the blue channel</param>
		/// <param name="a">Value of the alpha channel</param>
		public Color(byte r, byte g, byte b, byte a) 
			:this(){
			_red = r;
			_green = g;
			_blue = b;
			_alpha = a;
		}//end Color

		/// <summary>
		/// Create a new Color
		/// </summary>
		/// <param name="r">Value of the red channel</param>
		/// <param name="g">Value of the green channel</param>
		/// <param name="b">Value of the blue channel</param>
		public Color(byte r, byte g, byte b)
			: this() {
			_red = r;
			_green = g;
			_blue = b;
			_alpha = 255;
		}//end Color

		#endregion

		#region Interface implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(Color other) {
			return (
				_red == other.Red 
				&& _green == other.Green 
				&& _blue == other.Blue
				&& _alpha == other.Alpha  
			);
		}//end Equals

		#endregion

		#region Utilities

		/// <summary>
		/// Performs linear interpolation between two colors only with rgb channels
		/// </summary>
		/// <param name="color0">The first color</param>
		/// <param name="color1">The second color</param>
		/// <param name="t">the amount to interpolate between the two colors</param>
		/// <param name="withAlphaChannel">Flag indicates if this method also interpolate alpha channel</param>
		/// <returns>The interpolated color, with the same type of color0</returns>
		public static IColor Lerp(IColor color0, IColor color1, float t, bool withAlphaChannel) {

			IColor color = (IColor)System.Activator.CreateInstance(color0.GetType());
			color.Red = Libnoise.Lerp(color0.Red, color1.Red, t);
			color.Green = Libnoise.Lerp(color0.Green, color1.Green, t);
			color.Blue = Libnoise.Lerp(color0.Blue, color1.Blue, t);
			color.Alpha = (withAlphaChannel) ? Libnoise.Lerp(color0.Alpha, color1.Alpha, t) : (byte)255;

			return color;

		}//end Lerp

		/// <summary>
		/// Performs linear interpolation between two colors, including alpha channel
		/// </summary>
		/// <param name="color0">The first color</param>
		/// <param name="color1">The second color</param>
		/// <param name="t">the amount to interpolate between the two colors</param>
		/// <returns>The interpolated color</returns>
		public static IColor Lerp(IColor color0, IColor color1, float t) {
			return Lerp(color0, color1, t, true);
		}//end Lerp

		/// <summary>
		/// Performs linear interpolation between two colors, including alpha channel
		/// </summary>
		/// <param name="color0">The first color</param>
		/// <param name="color1">The second color</param>
		/// <param name="t">the amount to interpolate between the two colors</param>
		/// <returns>The interpolated color</returns>
		public static IColor Lerp32(IColor color0, IColor color1, float t) {
			return Lerp(color0, color1, t, true);
		}//end Lerp

		/// <summary>
		/// Performs linear interpolation between two colors only with rgb channels
		/// </summary>
		/// <param name="color0">The first color</param>
		/// <param name="color1">The second color</param>
		/// <param name="t">the amount to interpolate between the two colors</param>
		/// <returns>The interpolated color</returns>
		public static IColor Lerp24(IColor color0, IColor color1, float t) {
			return Lerp(color0, color1, t, false);
		}//end Lerp

		/// <summary>
		/// Compute a grayscale value from the source color using Color.GrayscaleLuminosityStrategy.
		/// </summary>
		/// <param name="color">The source color</param>
		/// <returns>The grayscale color with the same type as color</returns>
		public static IColor Grayscale(IColor color) {

			IColor returnColor = (IColor)System.Activator.CreateInstance(color.GetType());
			returnColor.Red = returnColor.Green = returnColor.Blue = GrayscaleLuminosityStrategy(color);
			returnColor.Alpha = 255;
			return returnColor;

		}//end public

		/// <summary>
		/// Compute a grayscale value from the source color using the given Strategy.
		/// </summary>
		/// <param name="color">The source color</param>
		/// <param name="Strategy">The grayscale strategy</param>
		/// <returns>The grayscale color with the same type as color</returns>
		public static IColor Grayscale(IColor color, GrayscaleStrategy Strategy) {

			IColor returnColor = (IColor)System.Activator.CreateInstance(color.GetType());
			byte val = Strategy(color);
			returnColor.Red = returnColor.Green = returnColor.Blue = val;
			returnColor.Alpha = 255;
			return returnColor;

		}//end public

		/// <summary>
		/// A GrayscaleStrategy implementation
		/// The lightness strategy averages the most prominent and least prominent colors: 
		/// (max(R, G, B) + min(R, G, B)) / 2.
		/// </summary>
		/// <param name="color">The source color</param>
		/// <returns>the computed channel value</returns>
		public static byte GrayscaleLightnessStrategy(IColor color) {

			return (byte)(
				(
					Math.Max(color.Red, Math.Max(color.Green, color.Blue)) +
					Math.Min(color.Red, Math.Max(color.Green, color.Blue))
				)/2
			);

		}//end GrayscaleLightnessStrategy

		/// <summary>
		/// A GrayscaleStrategy implementation
		/// The average strategy simply averages the values: (R + G + B) / 3.
		/// </summary>
		/// <param name="color">The source color</param>
		/// <returns>the computed channel value</returns>
		public static byte GrayscaleAverageStrategy(IColor color) {
			return (byte)((color.Red + color.Green + color.Blue) /3);
		}//end GrayscaleAverageStrategy

		/// <summary>
		/// A GrayscaleStrategy implementation
		/// The luminosity strategy averages the values, but it forms a weighted average to account 
		/// for human perception. We’re more sensitive to green than other colors, 
		/// so green is weighted most heavily. 
		/// The formula for luminosity is 0.21f *R + 0.71f *G + 0.07f *B.
		/// </summary>
		/// <param name="color">The source color</param>
		/// <returns>the computed channel value</returns>
		public static byte GrayscaleLuminosityStrategy(IColor color) {
			return (byte)(0.21f * color.Red + 0.71f * color.Green + 0.07f * color.Blue);
		}//end GrayscaleLuminosityStrategy

		#endregion

		#region Overloading

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return String.Format("Color({0},{1},{2},{3})", Red, Green, Blue, Alpha);
		}//end ToString

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public override bool Equals(Object other) {
			if(other is IColor) {
				return
					_red == ((IColor)other).Red 
					&& _green == ((IColor)other).Green 
					&& _blue == ((IColor)other).Blue 
					&& _alpha == ((IColor)other).Alpha;

			}//end if
			else {
				return false;
			}//end else
		}//end Equals

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return _hashcode;
		}//end Equals

		/// <summary>
		/// Overloading '==' operator:
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(Color a, IColor b) {
			return a.Equals(b);
		}//end ==
		
		/// <summary>
		/// Overloading '!=' operator:
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(Color a, IColor b) {
			return !a.Equals(b);
		}//end !=

		/// <summary>
		/// Overloading '>' operator:
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator >(Color a, IColor b) {
			return
				a._red > b.Red 
				&& a._green > b.Green 
				&& a._blue > b.Blue 
				&& a._alpha > b.Alpha;
		}//end >

		/// <summary>
		/// Overloading '<' operator:
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator <(Color a, IColor b) {
			return
				a._red < b.Red 
				&& a._green < b.Green 
				&& a._blue < b.Blue 
				&& a._alpha < b.Alpha;
		}//end <

		/// <summary>
		/// Overloading '>=' operator:
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator >=(Color a, IColor b) {
			return a > b || a == b;
		}//end >=

		/// <summary>
		/// Overloading '<=' operator:
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator <=(Color a, IColor b) {
			return a < b || a == b;
		}//end >=

		#endregion
	}//end Struct

}//end namespace
