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
namespace LibNoiseDotNet.Graphics.Tools.Noise.Utils {

	/// <summary>
	/// Abstract base class for a map of data, a 2-dimensional array of data, 
	/// designed to store values from any source.
	///
	/// The size (width and height) of the map can be specified during
	/// object construction or at any other time.
	///
	/// The GetValue() and SetValue() methods can be used to access individual
	/// values stored in the map.
	///
	/// If you specify a new size for the map and the new size is
	/// smaller than the current size, the allocated memory will not be
	/// reallocated.
	/// Call ReclaimMem() to reclaim the wasted memory.
	///
	/// <b>Border Values</b>
	///
	/// All of the values outside of the map are assumed to have a
	/// common value known as the <i>border value</i>.
	///
	/// To set the border value, call the BorderValue properties.
	///
	/// The GetValue() method returns the border value if the specified value
	/// lies outside of the map.
	///
	/// <b>Internal Noise Map Structure</b>
	///
	/// Internally, the values are organized into horizontal rows called
	/// slabs.  Slabs are ordered from bottom to top.
	///
	/// Each slab contains a contiguous row of values in memory.  The values
	/// in a slab are organized left to right.
	///
	/// The offset between the starting points of any two adjacent slabs is
	/// called the <i>stride amount</i>.  The stride amount is measured by
	/// the number of values between these two starting points.
	/// 
	/// </summary>
	abstract public class DataMap<T> {

		#region Fields

		/// <summary>
		/// The value used for all positions outside of the map.
		/// All positions outside of the map are assumed to have a
		/// common value known as the <i>border value</i>.
		/// </summary>
		protected T _borderValue;

		/// <summary>
		/// The width of the map
		/// </summary>
		protected int _width = 0;

		/// <summary>
		/// The height of the map
		/// </summary>
		protected int _height = 0;

		/// <summary>
		/// The stride amount of the map.
		/// </summary>
		private int _stride = 0;

		/// <summary>
		/// the memory used (bits) for this map.
		/// </summary>
		protected int _memoryUsage = 0;

		/// <summary>
		/// 
		/// </summary>
		protected int _cellsCount= 0;

		/// <summary>
		/// The noise map buffer.
		/// </summary>
		protected T[] _data = null;

		/// <summary>
		/// 
		/// </summary>
		protected bool _hasMaxDimension = false;

		/// <summary>
		/// 
		/// </summary>
		protected int _maxWidth = 0;

		/// <summary>
		/// 
		/// </summary>
		protected int _maxHeight = 0;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets the width of the map
		/// </summary>
		public int Width {
			get { return _width; }
		}

		/// <summary>
		/// Getsthe height of the map
		/// </summary>
		public int Height {
			get { return _height; }
		}

		/// <summary>
		/// The stride amount of the map.
		/// </summary>
		public int Stride {
			get { return _stride; }
			set { _stride = value; }
		}

		/// <summary>
		/// Gets or sets the value used for all positions outside of the map.
		/// </summary>
		public T BorderValue {
			get { return _borderValue; }
			set { _borderValue = value; }
		}

		/// <summary>
		/// Gets the memory used (bits) for this map
		/// </summary>
		public int MemoryUsage {
			get { return _memoryUsage; }
		}

		/// <summary>
		/// Gets the memory used (in Kb) for this map
		/// </summary>
		public float MemoryUsageKb {
			get { return MemoryUsage/8192.0f; }
		}

		/// <summary>
		/// Gets the memory used (in Kb) for this map
		/// </summary>
		public float MemoryUsageMo {
			get { return MemoryUsage/8388608.0f; }
		}

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Create an empty map
		/// </summary>
		public DataMap() {
			AllocateBuffer();
		}//End NoiseMap

		/// <summary>
		/// Create a new map with the given values
		///
		/// The width and height values are positive.
		/// The width and height values do not exceed the maximum
		/// possible width and height for the map.
		///
		/// @throw System.ArgumentException See the preconditions.
		/// @throw noise::ExceptionOutOfMemory Out of memory.
		///
		/// Creates a map with uninitialized values.
		///
		/// It is considered an error if the specified dimensions are not
		/// positive.
		/// </summary>
		/// <param name="width">The width of the new map.</param>
		/// <param name="height">The height of the new map</param>
		public DataMap(int width, int height) {

			AllocateBuffer(width, height);

		}//End NoiseMap

		/// <summary>
		/// Copy constructor
		/// @throw noise::ExceptionOutOfMemory Out of memory.
		/// </summary>
		/// <param name="copy">The map to copy</param>
		public DataMap(DataMap<T> copy) {
			CopyFrom(copy);
		}//End NoiseMap

		#endregion

		#region Interaction

		/// <summary>
		/// Returns a copy of a slab.
		///
		/// This method returns slab filled with the borderValue 
		/// if the coordinates exist outside the map
		/// </summary>
		/// <param name="y">The y coordinate of the position</param>
		/// <returns>The slab at that position</returns>
		public T[] GetSlab(int y) {

			T[] temp = new T[_stride];

			if(_data != null 
				&& (y >= 0 && y < _height)
			) {
				Array.Copy(_data, y * _stride, temp, 0, _stride);

			}//end if
			else {
				for(int i =0; i < temp.Length; i++) {
					temp[i] = _borderValue;
				}//end for
			}//end else

			return temp;

		}//end GetValue

		/// <summary>
		/// Returns a value from the specified position in the noise map.
		///
		/// This method returns the border value if the coordinates exist
		/// outside of the noise map.
		/// </summary>
		/// <param name="x">The x coordinate of the position</param>
		/// <param name="y">The y coordinate of the position</param>
		/// <returns>The value at that position</returns>
		public T GetValue(int x, int y) {

			if(_data != null 
				&& (x >= 0 && x < _width) 
				&& (y >= 0 && y < _height)
			) {
				return _data[y * _stride + x];
			}//end if

			return _borderValue;

		}//end GetValue

		/// <summary>
		/// Sets a value at a specified position in the map.
		///
		/// This method does nothing if the map object is empty or the
		/// position is outside the bounds of the noise map.
		/// </summary>
		/// <param name="x">The x coordinate of the position</param>
		/// <param name="y">The y coordinate of the position</param>
		/// <param name="value">The value to set at the given position</param>
		public void SetValue(int x, int y, T value) {

			if(_data != null 
				&& (x >= 0 && x < _width) 
				&& (y >= 0 && y < _height)
			) {
				_data[y * _stride + x] = value;
			}//end if

		}//end Clear

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
		public void SetSize(int width, int height) {

			if(width < 0 || height < 0) {
				throw new ArgumentException("Map dimension must be greater or equal 0");
			}//end if
			else if(_hasMaxDimension && (width > _maxWidth || height > _maxHeight)) {
				throw new ArgumentException(String.Format("Map dimension must be lower than {0} * {1}", _maxWidth, _maxHeight));
			}//end if
			else {
				AllocateBuffer(width, height);
			}//end else

		}//end SetSize

		/// <summary>
		/// Copies the contents of the buffer in the source map into
		/// this map, considering source is a trusted source
		/// </summary>
		/// <param name="source">The source map</param>
		public void CopyFrom(DataMap<T> source) {

			AllocateBuffer(source._width, source._height);

			if(_cellsCount > 0) {
				Array.Copy(source._data, 0, _data, 0, _cellsCount);
			}//end if

			// Copy the borderValue as well
			_borderValue = source._borderValue;

		}//end Copy

		/// <summary>
		/// Copies up to width * height cells from internal data buffer into the data buffer of the given map
		/// This method use dest.CopyFrom(DataMap<T> source) where sours is the current object.
		/// </summary>
		/// <param name="dest">The destination map</param>
		public void CopyTo(DataMap<T> dest) {

			if(dest == null) {
				throw new ArgumentNullException("Dest is null");
			}//end if
			else{
				dest.CopyFrom(this);
			}//end elseif

		}//end Copy

		/// <summary>
		/// Copies up to width * height cells from internal data buffer into the given buffer
		/// </summary>
		/// <param name="buffer">The destination buffer</param>
		public void CopyTo(ref T[] buffer) {

			if(_data == null) {
				return;
			}//end if
			else if(buffer == null) {
				buffer = new T[_cellsCount];
			}//end elseif

			int size = (_data.Length > buffer.Length) ? buffer.Length : _data.Length;
			Array.Copy(_data, 0, buffer, 0, size);

		}//end Copy

		/// <summary>
		/// Share the internal buffer
		/// </summary>
		/// <param name="buffer">The internal buffer</param>
		public T[] Share() {

			if(_data == null) {
				throw new NullReferenceException("The internal buffer is null");
			}//end if

			return _data;

		}//end Copy

		/// <summary>
		/// Resets the map object.
		///
		/// This method is similar to the SetSize(0, 0)
		/// </summary>
		public void Reset() {
			AllocateBuffer(0, 0);
		}//end DeleteAndReset

		/// <summary>
		/// Resets the map object.
		///
		/// This method is similar to SetSize(0, 0) or Reset(), except this method
		/// also deletes the buffer
		/// </summary>
		public void DeleteAndReset() {
			_data = null;
			AllocateBuffer(0, 0);
		}//end DeleteAndReset

		/// <summary>
		/// Reallocates the map to recover wasted memory.
		/// </summary>
		public void ReclaimMemory() {

			if(_data != null) {

				// There is wasted memory.  
				// Create the smallest buffer that can fit the
				// data and copy the data to it.
				if(_data.Length > _cellsCount) {
					Array.Resize<T>(ref _data, _cellsCount);
				}//end if

			}//end if

		}//end ReclaimMemory

		/// <summary>
		/// Clears the map to a specified value.
		/// This method is a O(n) operation, where  n is equal to width * height.
		/// </summary>
		/// <param name="value">The value that all positions within the map are cleared to.</param>
		public void Clear(T value) {

			if(_data != null) {

				for(int i = 0; i <= _cellsCount; i++) {
					_data[i] = value;
				}//end for

			}//end if

		}//end Clear

		/// <summary>
		/// Clears the map to a 0 value
		/// </summary>
		public void Clear() {
			if(_data != null) {
				Array.Clear(_data, 0, _cellsCount);
			}//end if
		}//end Clear

		#endregion

		#region Internal

		/// <summary>
		/// Return the memory size of the type of data.
		/// Children must implement this method to calculate the memory usage
		/// </summary>
		/// <returns>The memory size of the type of data</returns>
		abstract protected int SizeofT();

		/// <summary>
		/// Return the minimum value of the type of data
		/// </summary>
		/// <returns></returns>
		abstract protected T MinvalofT();
	
		/// <summary>
		/// Return the maximum value of the type of data
		/// </summary>
		/// <returns></returns>
		abstract protected T MaxvalofT();

		/// <summary>
		/// Allocate a buffer
		/// </summary>
		protected void AllocateBuffer() {

			_cellsCount = _width * _height;
			_stride = _width;
			_memoryUsage = _cellsCount * SizeofT();

			if(_cellsCount == 0) {
				return;
			}//end if
			else if(_data == null) {
				_data = new T[_cellsCount];
			}//end if
			else if(_data.Length < _cellsCount) {
				// Buffer is too small
				// Create the smallest buffer that can fit the
				// data and copy the data to it.
				Array.Resize<T>(ref _data, _cellsCount);
			}//end elseif

		}//end AllocateBuffer

		/// <summary>
		/// Allocate a buffer, assuming width and height are correct values @see SetSize(int, int)
		/// 
		/// </summary>
		/// <param name="width">width The new width</param>
		/// <param name="height">height The new height</param>
		protected void AllocateBuffer(int width, int height) {

			_width = width;
			_height = height;

			AllocateBuffer();

		}//end AllocateBuffer

		#endregion

	}//end class

}//end namespace
