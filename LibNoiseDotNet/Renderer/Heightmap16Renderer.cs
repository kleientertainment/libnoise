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

using LibNoiseDotNet.Graphics.Tools.Noise;
using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer {

	/// <summary>
	/// class for an 16bit-heightmap renderer
	/// </summary>
	public class Heightmap16Renderer :AbstractHeightmapRenderer {

		#region Fields

		/// <summary>
		/// The destination heightmap
		/// </summary>
		protected Heightmap16 _heightmap;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the destination heightmap
		/// </summary>
		public Heightmap16 Heightmap {
			get { return _heightmap; }
			set { _heightmap = value; }
		}//end Heightmap

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public Heightmap16Renderer()
			: base() {
		}//end Heightmap16Renderer

		#endregion

		#region internal
		/// <summary>
		/// Sets the new size for the target heightmap.
		/// 
		/// </summary>
		/// <param name="width">width The new width for the heightmap</param>
		/// <param name="height">height The new height for the heightmap</param>
		protected override void SetHeightmapSize(int width, int height) {
			_heightmap.SetSize(width, height);
		}//end protected

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override bool CheckHeightmap() {
			return _heightmap != null;
		}//end protected

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="source"></param>
		/// <param name="boundDiff"></param>
		protected override void RenderHeight(int x, int y, float source, float boundDiff) {

			ushort elevation;

			if(source <= _lowerHeightBound) {
				elevation = ushort.MinValue;
			}//end if
			else if(source >= _upperHeightBound) {
				elevation = ushort.MaxValue;
			}//end if
			else {
				elevation = (ushort)(((source - _lowerHeightBound) / boundDiff) *65535.0f);
			}//end else

			_heightmap.SetValue(x, y, elevation);

		}//end protected 

		#endregion

	}//end class

}//end namespace
