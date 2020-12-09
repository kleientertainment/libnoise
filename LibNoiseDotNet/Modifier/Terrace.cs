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
using System.Collections.Generic;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier {

	/// <summary>
	/// Noise module that maps the output value from a source module onto a
	/// terrace-forming curve.
	///
	/// This noise module maps the output value from the source module onto a
	/// terrace-forming curve.  The start of this curve has a slope of zero;
	/// its slope then smoothly increases.  This curve also contains
	/// <i>control points</i> which resets the slope to zero at that point,
	/// producing a "terracing" effect.  Refer to the following illustration:
	///
	/// To add a control point to this noise module, call the
	/// AddControlPoint() method.
	///
	/// An application must add a minimum of two control points to the curve.
	/// If this is not done, the GetValue() method fails.  The control points
	/// can have any value, although no two control points can have the same
	/// value.  There is no limit to the number of control points that can be
	/// added to the curve.
	///
	/// This noise module clamps the output value from the source module if
	/// that value is less than the value of the lowest control point or
	/// greater than the value of the highest control point.
	///
	/// This noise module is often used to generate terrain features such as
	/// your stereotypical desert canyon.
	/// </summary>
	public class Terrace :ModifierModule, IModule3D {

		#region Fields
		/// <summary>
		/// 
		/// </summary>
		protected List<float> _controlPoints = new List<float>(2);

		/// <summary>
		/// Enables or disables the inversion of the terrace-forming curve
		/// between the control points.
		/// </summary>
		protected bool _invert = false;

		#endregion

		#region Accessors
		/// <summary>
		/// gets or sets the inversion of the terrace-forming curve between the control points
		/// </summary>
		public bool Invert {
			get { return _invert; }
			set { _invert = value; }
		}

		#endregion

		#region Ctor/Dtor

		public Terrace()
			: base() {
		}//end Terrace

		public Terrace(IModule source)
			: base(source){
		}//end Terrace

		public Terrace(IModule source, bool invert)
			: base(source) {
			_invert = invert;
		}//end Terrace

		#endregion

		#region Interaction

		/// <summary>
		/// Adds a control point to the curve.
		///
		/// No two control points have the same input value.
		///
		/// @throw System.ArgumentException if two control points have the same input value.
		///
		/// It does not matter which order these points are added.
		/// </summary>
		/// <param name="input">The input value stored in the control point.</param>
		public void AddControlPoint(float input) {

			if(_controlPoints.Contains(input)) {
				throw new ArgumentException(String.Format("Cannont insert ControlPoint({0}) : Each control point is required to contain a unique input value", input));
			}//end if
			else {
				_controlPoints.Add(input);
				SortControlPoints();
			}//end else

		}//end AddControlPoint

		/// <summary>
		/// Return the size of the ControlPoint list
		/// </summary>
		/// <returns>The number of ControlPoint in the list</returns>
		public int CountControlPoints() {
			return _controlPoints.Count;
		}//end  ClearControlPoints

		/// <summary>
		/// Returns a read-only IList<ControlPoint> wrapper for the current ControlPoint list.
		/// </summary>
		/// <returns>The read only list</returns>
		public IList<float> getControlPoints() {
			return _controlPoints.AsReadOnly();
		}//end  ClearControlPoints

		/// <summary>
		/// Deletes all the control points on the curve.
		/// </summary>
		public void ClearControlPoints() {
			_controlPoints.Clear();
		}//end  ClearControlPoints

		/// <summary>
		/// Creates a number of equally-spaced control points that range from
		/// -1 to +1.
		///
		/// The number of control points must be greater than or equal to 2
		/// The previous control points on the terrace-forming curve are deleted.
		///
		/// Two or more control points define the terrace-forming curve.  The
		/// start of this curve has a slope of zero; its slope then smoothly
		/// increases.  At the control points, its slope resets to zero.
		/// 
		/// @throw ArgumentException if an invalid parameter was
		/// specified
		/// </summary>
		/// <param name="controlPointCount">The number of control points to generate.</param>
		public void MakeControlPoints (int controlPointCount){

			if (controlPointCount < 2) {
				throw new ArgumentException( "Two or more control points must be specified.");
			}//end if

			ClearControlPoints();

			float terraceStep = 2.0f / ((float)controlPointCount - 1.0f);
			float curValue = -1.0f;
			for (int i = 0; i < (int)controlPointCount; i++) {
				AddControlPoint(curValue);
				curValue += terraceStep;
			}//end for
		}//end MakeControlPoints

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

			// Get the output value from the source module.
			float sourceModuleValue = ((IModule3D)_sourceModule).GetValue(x, y, z);

			// Find the first element in the control point array that has a value
			// larger than the output value from the source module.
			int indexPos;
			for(indexPos = 0; indexPos < _controlPoints.Count; indexPos++) {
				if(sourceModuleValue < _controlPoints[indexPos]) {
					break;
				}//end if
			}//end for

			// Find the two nearest control points so that we can map their values
			// onto a quadratic curve.
			int index0 = Libnoise.Clamp(indexPos - 1, 0, _controlPoints.Count - 1);
			int index1 = Libnoise.Clamp(indexPos, 0, _controlPoints.Count - 1);

			// If some control points are missing (which occurs if the output value from
			// the source module is greater than the largest value or less than the
			// smallest value of the control point array), get the value of the nearest
			// control point and exit now.
			if(index0 == index1) {
				return _controlPoints[index1];
			}//end if

			// Compute the alpha value used for linear interpolation.
			float value0 = _controlPoints[index0];
			float value1 = _controlPoints[index1];
			float alpha = (sourceModuleValue - value0) / (value1 - value0);

			if(_invert) {
				alpha = 1.0f - alpha;
				Libnoise.SwapValues(ref value0, ref value1);
			}//end if

			// Squaring the alpha produces the terrace effect.
			alpha *= alpha;

			// Now perform the linear interpolation given the alpha value.
			return Libnoise.Lerp(value0, value1, alpha);

		}//end GetValue

		#endregion

		#region Internal

		/// <summary>
		/// 
		/// </summary>
		protected void SortControlPoints() {

			_controlPoints.Sort(delegate(float p1, float p2) {

				if(p1 > p2) {
					return 1;
				}//end if
				else if(p1 < p2) {
					return -1;
				}//end if
				else {
					return 0;
				}//end else

			});

		}//end SortControlPoints

		#endregion

	}//end class

}//end namespace
