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
	/// Noise module that maps the output value from a source module onto an
	/// arbitrary function curve.
	///
	/// This noise module maps the output value from the source module onto an
	/// application-defined curve.  This curve is defined by a number of
	/// <i>control points</i>; each control point has an <i>input value</i>
	/// that maps to an <i>output value</i>.  Refer to the following
	/// illustration:
	///
	/// To add the control points to this curve, call the AddControlPoint()
	/// method.
	///
	/// Since this curve is a cubic spline, an application must add a minimum
	/// of four control points to the curve.  If this is not done, the
	/// GetValue() method fails.  Each control point can have any input and
	/// output value, although no two control points can have the same input
	/// value.  There is no limit to the number of control points that can be
	/// added to the curve. 
	/// </summary>
	public class Curve :ModifierModule, IModule3D {

		#region Fields
		/// <summary>
		/// 
		/// </summary>
		protected List<ControlPoint> _controlPoints = new List<ControlPoint>(4);

		#endregion

		#region Ctor/Dtor

		public Curve()
			: base() {
		}//end Curve

		public Curve(IModule source)
			: base(source) {
		}//end Curve

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
		/// <param name="output">The output value stored in the control point.</param>
		public void AddControlPoint(float input, float output){
			AddControlPoint(new ControlPoint(input, output));
		}//end AddControlPoint

		/// <summary>
		/// Adds a control point to the curve.
		///
		/// No two control points have the same input value.
		///
		/// @throw System.ArgumentException if two control points have the same input value.
		///
		/// It does not matter which order these points are added.
		/// </summary>
		/// <param name="point"></param>
		public void AddControlPoint(ControlPoint point) {

			if(_controlPoints.Contains(point)) {
				throw new ArgumentException(String.Format("Cannont insert ControlPoint({0}, {1}) : Each control point is required to contain a unique input value", point.Input, point.Output));
			}//end if
			else {
				_controlPoints.Add(point);
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
		public IList<ControlPoint> getControlPoints() {
			return _controlPoints.AsReadOnly();
		}//end  ClearControlPoints

		/// <summary>
		/// Deletes all the control points on the curve.
		/// </summary>
		public void ClearControlPoints(){
			_controlPoints.Clear();
		}//end  ClearControlPoints
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

			// Find the first element in the control point array that has an input value
			// larger than the output value from the source module.
			int indexPos;
			for(indexPos = 0; indexPos < _controlPoints.Count; indexPos++) {
				if(sourceModuleValue < _controlPoints[indexPos].Input) {
					break;
				}//end if
			}//end for

			// Find the four nearest control points so that we can perform cubic
			// interpolation.
			int index0 = Libnoise.Clamp(indexPos - 2, 0, _controlPoints.Count - 1);
			int index1 = Libnoise.Clamp(indexPos - 1, 0, _controlPoints.Count - 1);
			int index2 = Libnoise.Clamp(indexPos, 0, _controlPoints.Count - 1);
			int index3 = Libnoise.Clamp(indexPos + 1, 0, _controlPoints.Count - 1);

			// If some control points are missing (which occurs if the value from the
			// source module is greater than the largest input value or less than the
			// smallest input value of the control point array), get the corresponding
			// output value of the nearest control point and exit now.
			if(index1 == index2) {
				return _controlPoints[index1].Output;
			}//end if

			// Compute the alpha value used for cubic interpolation.
			float input0 = _controlPoints[index1].Input;
			float input1 = _controlPoints[index2].Input;
			float alpha = (sourceModuleValue - input0) / (input1 - input0);

			// Now perform the cubic interpolation given the alpha value.
			return Libnoise.Cerp(
			  _controlPoints[index0].Output,
			  _controlPoints[index1].Output,
			  _controlPoints[index2].Output,
			  _controlPoints[index3].Output,
			  alpha);

		}//end GetValue

		#endregion

		#region Internal

		/// <summary>
		/// 
		/// </summary>
		protected void SortControlPoints() {

			_controlPoints.Sort(delegate(ControlPoint p1, ControlPoint p2) {

				if(p1.Input > p2.Input) {
					return 1;
				}//end if
				else if(p1.Input < p2.Input) {
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
