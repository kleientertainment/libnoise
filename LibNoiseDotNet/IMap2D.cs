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

namespace LibNoiseDotNet.Graphics.Tools.Noise{

	/// <summary>
	/// 
	/// </summary>
	public interface IMap2D<T> {

		#region Accessors

		/// <summary>
		/// Gets the width of the map
		/// </summary>
		int Width { get; }

		/// <summary>
		/// Gets the height of the map
		/// </summary>
		int Height { get; }

		/// <summary>
		/// Gets or sets the value used for all positions outside of the map.
		/// </summary>
		T BorderValue { get; set; }

		#endregion

		#region Interaction

		/// <summary>
		/// Returns a value from the specified position in the noise map.
		///
		/// This method returns the border value if the coordinates exist
		/// outside of the noise map.
		/// </summary>
		/// <param name="x">The x coordinate of the position</param>
		/// <param name="y">The y coordinate of the position</param>
		/// <returns>The value at that position</returns>
		T GetValue(int x, int y);

		/// <summary>
		/// Sets a value at a specified position in the map.
		///
		/// This method does nothing if the map object is empty or the
		/// position is outside the bounds of the noise map.
		/// </summary>
		/// <param name="x">The x coordinate of the position</param>
		/// <param name="y">The y coordinate of the position</param>
		/// <param name="value">The value to set at the given position</param>
		void SetValue(int x, int y, T value);

		/// <summary>
		/// Sets the new size for the map.
		/// 
		/// @pre The width and height values are positive.
		/// @pre The width and height values do not exceed the maximum
		/// possible width and height for the map.
		///
		/// @throw ArgumentException See the preconditions, the noise map is
		/// unmodified.
		/// 
		/// </summary>
		/// <param name="width">width The new width for the map</param>
		/// <param name="height">height The new height for the map</param>
		void SetSize(int width, int height);

		/// <summary>
		/// Resets the map object.
		/// This method is similar to the SetSize(0, 0)
		/// </summary>
		void Reset();

		/// <summary>
		/// Clears the map to a specified value.
		/// This method is a O(n) operation, where n is equal to width * height.
		/// </summary>
		/// <param name="value">The value that all positions within the map are cleared to.</param>
		void Clear(T value);

		/// <summary>
		/// Clears the map to a 0 value
		/// </summary>
		void Clear();

		/// <summary>
		/// Find the lowest and highest value in the map
		/// </summary>
		/// <param name="min">the lowest value</param>
		/// <param name="max">the highest value</param>
		void MinMax(out T min, out T max);

		#endregion

	}//end class

}//end namespace
