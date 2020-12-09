// This file is part of Libnoise-dotnet.
//
// Libnoise-dotnet is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Libnoise-dotnet is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Libnoise-dotnet.  If not, see <http://www.gnu.org/licenses/>.

using LibNoiseDotNet.Graphics.Tools.Noise.Renderer;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder {

	/// <summary>
	/// 
	/// </summary>
	public class ShapeFilter :IBuilderFilter {

		/// <summary>
		/// A simple 2d-coordinates struct used as a cached value
		/// </summary>
		protected struct LevelCache{

			/// <summary>
			/// 
			/// </summary>
			int x;

			/// <summary>
			/// 
			/// </summary>
			int y;

			/// <summary>
			/// 
			/// </summary>
			public byte level;

			/// <summary>
			/// Default constructor
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="level"></param>
			public LevelCache(int x, int y, byte level) {
				this.x = x;
				this.y = y;
				this.level = level;
			}//end LevelCache

			/// <summary>
			/// 
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="color"></param>
			/// <returns></returns>
			public bool IsCached(int x, int y) {
				return this.x == x && this.y == y;
			}//end Equals

			/// <summary>
			/// 
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="level"></param>
			/// <returns></returns>
			public void Update(int x, int y, byte level) {
				this.x = x;
				this.y = y;
				this.level = level;
			}//end Equals

		}//end struct

		#region Constants

		/// <summary>
		/// 
		/// </summary>
		public const float DEFAULT_VALUE = -0.5f;

		#endregion

		#region Fields

		/// <summary>
		/// 
		/// </summary>
		protected float _constant = DEFAULT_VALUE;

		/// <summary>
		/// The shape image
		/// </summary>
		protected IMap2D<IColor> _shape;

		/// <summary>
		/// 
		/// </summary>
		protected LevelCache _cache = new LevelCache(-1, -1, 0);

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the shape image
		/// </summary>
		public IMap2D<IColor> Shape {
			get { return _shape; }
			set { _shape = value; }
		}

		/// <summary>
		/// the constant output value.
		/// </summary>
		public float ConstantValue {
			get { return _constant; }
			set { _constant = value; }
		}//end Constant

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// 
		/// </summary>
		public ShapeFilter() {

		}//end ShapeFilter

		#endregion

		#region Interaction

		/// <summary>
		/// Return the filter level at this position
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public FilterLevel IsFiltered(int x, int y) {

			byte level = GetGreyscaleLevel(x, y);

			if(level == byte.MinValue) {
				return FilterLevel.Constant;
			}//end if
			else if(level == byte.MaxValue) {
				return FilterLevel.Source;
			}//end if
			else {
				return FilterLevel.Filter;
			}//end else

		}//end IsFiltered

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public float FilterValue(int x, int y, float source) {

			byte level = GetGreyscaleLevel(x, y);

			if(level == byte.MaxValue) {//|| source > _constant
				return source;
			}//end if
			else if(level == byte.MinValue) {
				return _constant;
			}//end if
			else {
				return Libnoise.Lerp(
					_constant,
					source,
					level / 255.0f
				);
			}//end if

		}//end FilterValue

		#endregion

		#region Internal

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		protected byte GetGreyscaleLevel(int x, int y) {

			// Is this position is stored in cache ?
			if(!_cache.IsCached(x, y)) {

				// Assuming controlColor is a greyscale value
				// just test the red channel
				_cache.Update(x, y, _shape.GetValue(x, y).Red);

			}//end else

			return _cache.level;

		}//end 

		#endregion

	}//end class

}//end namespace
