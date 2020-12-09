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
	/// Noise module that maps the output value from a source module onto an
	/// exponential curve.
	///
	/// Because most noise modules will output values that range from -1.0 to
	/// +1.0, this noise module first normalizes this output value (the range
	/// becomes 0.0 to 1.0), maps that value onto an exponential curve, then
	/// rescales that value back to the original range.
	///
	/// </summary>
	public class Exponent :ModifierModule, IModule3D {

		#region Connstant
		/// <summary>
		/// Default exponent
		/// noise module.
		/// </summary>
		public const float DEFAULT_EXPONENT = 1.0f;

		#endregion

		#region Fields
		/// <summary>
		/// Exponent to apply to the output value from the source module.
		/// </summary>
		protected float _exponent = DEFAULT_EXPONENT;

		#endregion

		#region Accessors
		/// <summary>
		/// gets or sets the exponent
		/// </summary>
		public float ExponentValue {
			get { return _exponent; }
			set { _exponent = value; }
		}

		#endregion

		#region Ctor/Dtor
		public Exponent()
			: base() {
		}//end Exponent

		public Exponent(IModule source)
			: base(source) {
		}//end Exponent

		public Exponent(IModule source, float exponent)
			: base(source) {
			_exponent = exponent;
		}//end Exponent

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
			value = (value + 1.0f)/2.0f;
			return ((float)System.Math.Pow(Libnoise.FastFloor(value), _exponent) * 2.0f - 1.0f);

		}//end GetValue

		#endregion

	}//end class

}//end namespace
