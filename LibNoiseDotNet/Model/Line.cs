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


namespace LibNoiseDotNet.Graphics.Tools.Noise.Model {

	/// <summary>
	/// Model that defines the displacement of a line segment.
	///
	/// This model returns an output value from a noise module given the
	/// one-dimensional coordinate of an input value located on a line
	/// segment, which can be used as displacements.
	///
	/// This class is useful for creating:
	///  - roads and rivers
	///  - disaffected college students
	///
	/// To generate an output value, pass an input value between 0.0 and 1.0
	/// to the GetValue() method.  0.0 represents the start position of the
	/// line segment and 1.0 represents the end position of the line segment.
	/// 
	/// </summary>
	public class Line :AbstractModel {

		/// <summary>
		/// Internal struct that represent a 3D position
		/// </summary>
		protected struct Position {

			#region fields
			/// <summary>
			/// x coordinate of a position.
			/// </summary>
			public float x;

			/// <summary>
			/// y coordinate of a position.
			/// </summary>
			public float y;

			/// <summary>
			/// z coordinate of a position.
			/// </summary>
			public float z;

			#endregion

			public Position(float x, float y, float z) {
				this.x = x;
				this.y = y;
				this.z = z;
			}//end POsition

		}//end Struct

		#region fields

		/// <summary>
		/// A flag indicating that the output value is to be attenuated
		/// (moved toward 0.0) as the ends of the line segment are approached.
		/// </summary>
		protected bool _attenuate = true;

		/// <summary>
		/// The position of the start of the line segment.
		/// </summary>
		protected Position _startPosition = new Position(0, 0, 0);

		/// <summary>
		/// The position of the end of the line segment.
		/// </summary>
		protected Position _endPosition = new Position(0, 0, 0);

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets a flag indicating that the output value is to be attenuated
		/// (moved toward 0.0) as the ends of the line segment are approached.
		/// </summary>
		public bool Attenuate {
			get { return _attenuate; }
			set { _attenuate = value; }
		}

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Line()
			: base() {

		}//end Plane

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="module">The noise module that is used to generate the output values</param>
		public Line(IModule module)
			: base(module) {
		}//end Plane

		#endregion

		#region Interaction

		/// <summary>
		/// Sets the position ( x, y, z ) of the start of the line
		/// segment to choose values along.
		/// </summary>
		/// <param name="x">x coordinate of the start position</param>
		/// <param name="y">y coordinate of the start position</param>
		/// <param name="z">z coordinate of the start position</param>
		public void SetStartPoint(float x, float y, float z) {
			_startPosition.x = x;
			_startPosition.y = y;
			_startPosition.z = z;
		}//end public

		/// <summary>
		/// Sets the position ( x, y, z ) of the end of the line
		/// segment to choose values along.
		/// </summary>
		/// <param name="x">x coordinate of the end position</param>
		/// <param name="y">y coordinate of the end position</param>
		/// <param name="z">z coordinate of the end position</param>
		public void SetEndPoint(float x, float y, float z) {
			_endPosition.x = x;
			_endPosition.y = y;
			_endPosition.z = z;
		}//end public


		/// <summary>
        /// Returns the output value from the noise module given the
        /// one-dimensional coordinate of the specified input value located
        /// on the line segment. This value may be attenuated (moved toward
        /// 0.0) as p approaches either end of the line segment; this is
        /// the default behavior.
		/// 
        /// If the value is not to be attenuated, p can safely range
        /// outside the 0.0 to 1.0 range; the output value will be
        /// extrapolated along the line that this segment is part of.
        ///
		/// </summary>
		/// <param name="p">The distance along the line segment (ranges from 0.0 to 1.0)</param>
		/// <returns>The output value from the noise module</returns>
		public float GetValue(float p) {

			float x = (_endPosition.x - _startPosition.x) * p + _startPosition.x;
			float y = (_endPosition.y - _startPosition.y) * p + _startPosition.y;
			float z = (_endPosition.z - _startPosition.z) * p + _startPosition.z;

			float value = ((IModule3D)_sourceModule).GetValue(x, y, z);

			if(_attenuate) {
				return p * (1.0f - p) * 4.0f * value;
			}//end if
			else {
				return value;
			}//end emse

		}//end GetValue

		#endregion

	}//end class

}//end namespace
