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
	/// Defines a point used to build a color gradient.
	///
	/// A color gradient is a list of gradually-changing colors.  A color
	/// gradient is defined by a list of <i>gradient points</i>.  Each
	/// gradient point has a position and a color.  In a color gradient, the
	/// colors between two adjacent gradient points are linearly interpolated.
	///
	/// The ColorGradient class defines a color gradient by a list of these
	/// objects.
	/// </summary>
	public struct GradientPoint :IEquatable<GradientPoint> {

		#region fields

		/// <summary>
		/// The color of this gradient point.
		/// </summary>
		public IColor Color;

		/// <summary>
		/// The position of this gradient point.
		/// </summary>
		public float Position;

		/// <summary>
		/// Internal hashcode
		/// </summary>
		private int _hashcode;

		#endregion

		#region Ctor/Dtor

		public GradientPoint(float position, IColor color) {
			Color = color;
			Position = position;
			_hashcode = (int)Position ^Color.GetHashCode();
		}//end GradientPoint

		#endregion

		#region Interface implementation

		public bool Equals(GradientPoint other) {
			return this.Position == other.Position;
		}//end Equals

		#endregion

		#region Overloading

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return _hashcode;
		}//end Equals

		#endregion

	}//end Struct

}//end namespace
