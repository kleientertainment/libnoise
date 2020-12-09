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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{

	/// <summary>
	/// Noise module that outputs the value selected from one of two source
	/// modules chosen by the output value from a control module.
	///
	/// - LeftModule outputs a value.
	/// - RightModule outputs a value.
	/// - ControlModule is known as the <i>control
	///   module</i>.  The control module determines the value to select.  If
	///   the output value from the control module is within a range of values
	///   known as the <i>selection range</i>, this noise module outputs the
	///   value from the RightModule.  Otherwise, this noise module outputs the value 
	///   from the LeftModule
	///
	/// By default, there is an abrupt transition between the output values
	/// from the two source modules at the selection-range boundary.  To
	/// smooth the transition, pass a non-zero value to the EdgeFalloff
	/// method.  Higher values result in a smoother transition.
	///
	/// </summary>
	public class Select : SelectorModule, IModule3D
	{

		#region Constants
		/// <summary>
		/// Default edge-falloff value for the Select noise module.
		/// </summary>
		public const float DEFAULT_FALL_OFF = -1.0f;

		/// <summary>
		/// Default lower bound of the selection range for the
		/// Select noise module.
		/// </summary>
		public const float DEFAULT_LOWER_BOUND = -1.0f;

		/// <summary>
		/// Default upper bound of the selection range for the
		/// Select noise module.
		/// </summary>
		public const float DEFAULT_UPPER_BOUND = 1.0f;

		#endregion

		#region Fields
		/// <summary>
		/// Lower bound of the selection range.
		/// </summary>
		protected float _lowerBound = DEFAULT_LOWER_BOUND;

		/// <summary>
		/// Upper bound of the selection range.
		/// </summary>
		protected float _upperBound = DEFAULT_UPPER_BOUND;

		/// <summary>
		/// The falloff value is the width of the edge transition at either
		/// edge of the selection range.
		///
		/// By default, there is an abrupt transition between the values from
		/// the two source modules at the boundaries of the selection range.
		///
		/// For example, if the selection range is 0.5 to 0.8, and the edge
		/// falloff value is 0.1, then the GetValue() method outputs:
		/// - the output value from the source module with an index value of 0
		///   if the output value from the control module is less than 0.4
		///   ( = 0.5 - 0.1).
		/// - a linear blend between the two output values from the two source
		///   modules if the output value from the control module is between
		///   0.4 ( = 0.5 - 0.1) and 0.6 ( = 0.5 + 0.1).
		/// - the output value from the source module with an index value of 1
		///   if the output value from the control module is between 0.6
		///   ( = 0.5 + 0.1) and 0.7 ( = 0.8 - 0.1).
		/// - a linear blend between the output values from the two source
		///   modules if the output value from the control module is between
		///   0.7 ( = 0.8 - 0.1 ) and 0.9 ( = 0.8 + 0.1).
		/// - the output value from the source module with an index value of 0
		///   if the output value from the control module is greater than 0.9
		///   ( = 0.8 + 0.1).
		/// </summary>
		protected float _edgeFalloff = DEFAULT_FALL_OFF;

		/// <summary>
		/// 
		/// </summary>
		protected IModule _controlModule;

		/// <summary>
		/// The right input module
		/// </summary>
		protected IModule _rightModule;

		/// <summary>
		/// The left input module
		/// </summary>
		protected IModule _leftModule;

		#endregion

		#region Accessors
		/// <summary>
		/// gets the lower bound
		/// </summary>
		public float LowerBound
		{
			get { return _lowerBound; }
		}

		/// <summary>
		/// gets the upper bound
		/// </summary>
		public float UpperBound
		{
			get { return _upperBound; }
		}

		/// <summary>
		/// Gets or sets the falloff value at the edge transition.
		/// </summary>
		public float EdgeFalloff
		{
			get { return _edgeFalloff; }
			set
			{
				// Make sure that the edge falloff curves do not overlap.
				float boundSize = _upperBound - _lowerBound;
				_edgeFalloff = (value > boundSize / 2.0f) ? boundSize / 2.0f : value;
			}
		}

		/// <summary>
		/// Gets or sets the left module
		/// </summary>
		public IModule LeftModule
		{
			get { return _leftModule; }
			set { _leftModule = value; }
		}

		/// <summary>
		/// Gets or sets the right module
		/// </summary>
		public IModule RightModule
		{
			get { return _rightModule; }
			set { _rightModule = value; }
		}

		/// <summary>
		/// Gets or sets the control module
		/// </summary>
		public IModule ControlModule
		{
			get { return _controlModule; }
			set { _controlModule = value; }
		}

		#endregion

		#region Ctor/Dtor
		public Select()
		{

		}//end Select

		public Select(IModule controlModule, IModule rightModule, IModule leftModule, float lower, float upper, float edge)
		{

			_controlModule = controlModule;
			_leftModule = leftModule;
			_rightModule = rightModule;

			SetBounds(lower, upper);
			EdgeFalloff = edge;

		}//end Select

		#endregion

		#region Interaction

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lower"></param>
		/// <param name="upper"></param>
		public void SetBounds(float lower, float upper)
		{
			System.Diagnostics.Debug.Assert(_lowerBound < _upperBound, "Lower bound must lower than upper bound");
			_lowerBound = lower;
			_upperBound = upper;

			// Make sure that the edge falloff curves do not overlap.
			EdgeFalloff = _edgeFalloff;

		}//end SetBounds

		#endregion

		#region IModule3D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y, float z)
		{

			float controlValue = ((IModule3D)_controlModule).GetValue(x, y, z);
			float alpha;

			if (_edgeFalloff > 0.0)
			{

				if (controlValue < (_lowerBound - _edgeFalloff))
				{

					// The output value from the control module is below the selector
					// threshold; return the output value from the first source module.
					return ((IModule3D)_leftModule).GetValue(x, y, z);

				}//end if
				else if (controlValue < (_lowerBound + _edgeFalloff))
				{

					// The output value from the control module is near the lower end of the
					// selector threshold and within the smooth curve. Interpolate between
					// the output values from the first and second source modules.
					float lowerCurve = (_lowerBound - _edgeFalloff);
					float upperCurve = (_lowerBound + _edgeFalloff);

					alpha = Libnoise.SCurve3(
						(controlValue - lowerCurve) / (upperCurve - lowerCurve)
					);

					return Libnoise.Lerp(
						((IModule3D)_leftModule).GetValue(x, y, z),
						((IModule3D)_rightModule).GetValue(x, y, z),
						alpha
					);

				}//end elseif
				else if (controlValue < (_upperBound - _edgeFalloff))
				{

					// The output value from the control module is within the selector
					// threshold; return the output value from the second source module.
					return ((IModule3D)_rightModule).GetValue(x, y, z);

				}//end elseif
				else if (controlValue < (_upperBound + _edgeFalloff))
				{

					// The output value from the control module is near the upper end of the
					// selector threshold and within the smooth curve. Interpolate between
					// the output values from the first and second source modules.
					float lowerCurve = (_upperBound - _edgeFalloff);
					float upperCurve = (_upperBound + _edgeFalloff);

					alpha = Libnoise.SCurve3(
						(controlValue - lowerCurve) / (upperCurve - lowerCurve)
					);

					return Libnoise.Lerp(
						((IModule3D)_rightModule).GetValue(x, y, z),
						((IModule3D)_leftModule).GetValue(x, y, z),
						alpha
					);

				}//end elseif
				else
				{

					// Output value from the control module is above the selector threshold;
					// return the output value from the first source module.
					return ((IModule3D)_leftModule).GetValue(x, y, z);

				}//end else

			}//end if
			else
			{

				if (controlValue < _lowerBound || controlValue > _upperBound)
				{
					return ((IModule3D)_leftModule).GetValue(x, y, z);
				}//end if
				else
				{
					return ((IModule3D)_rightModule).GetValue(x, y, z);
				}//end else

			}//end else

		}//end GetValue

		#endregion

	}//end class

}//end namespace
