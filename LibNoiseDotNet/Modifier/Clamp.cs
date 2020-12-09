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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier {

	/// <summary>
	/// Noise module that clamps the output value from a source module to a
	/// range of values.
	///
	/// The range of values in which to clamp the output value is called the
	/// <i>clamping range</i>.
	///
	/// If the output value from the source module is less than the lower
	/// bound of the clamping range, this noise module clamps that value to
	/// the lower bound.  If the output value from the source module is
	/// greater than the upper bound of the clamping range, this noise module
	/// clamps that value to the upper bound.
	///
	/// </summary>
	public class Clamp :ModifierModule, IModule3D {

		#region Connstant
		/// <summary>
		/// Default lower bound of the clamping range 
		/// noise module.
		/// </summary>
		public const float DEFAULT_LOWER_BOUND = -1.0f;

		/// <summary>
		/// Default upper bound of the clamping range
		/// noise module.
		/// </summary>
		public const float DEFAULT_UPPER_BOUND = 1.0f;

		#endregion

		#region Fields
		/// <summary>
		/// 
		/// </summary>
		protected float _lowerBound = DEFAULT_LOWER_BOUND;

		/// <summary>
		/// 
		/// </summary>
		protected float _upperBound = DEFAULT_UPPER_BOUND;

		#endregion

		#region Accessors
		/// <summary>
		/// 
		/// </summary>
		public float LowerBound {
			get { return _lowerBound; }
			set { _lowerBound = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public float UpperBound {
			get { return _upperBound; }
			set { _upperBound = value; }
		}

		#endregion

		#region Ctor/Dtor
		public Clamp()
			: base() {
		}//end Clamp

		public Clamp(IModule source)
			: base(source) {
		}//end Clamp

		public Clamp(IModule source, float lower, float upper)
			: base(source) {
			_lowerBound = lower;
			_upperBound = upper;
		}//end Clamp

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

			float value = ((IModule3D)_sourceModule).GetValue(x, y, z);

			if(value < _lowerBound) {
				return _lowerBound;
			}//end if
			else if(value > _upperBound) {
				return _upperBound;
			}//end elseif
			else {
				return value;
			}//end else

		}//end GetValue

		#endregion

	}//end class

}//end namespace
