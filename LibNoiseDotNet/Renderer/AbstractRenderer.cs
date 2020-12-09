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

using LibNoiseDotNet.Graphics.Tools.Noise;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer {

	/// A delegate to a callback function used by the Renderer classes.
	///
	/// The renderer method calls this callback function each
	/// time it fills a row of the target struct.
	///
	/// This callback function has a single integer parameter that contains
	/// a count of the rows that have been completed.  It returns void.
	public delegate void RendererCallback(int row);

	/// <summary>
	/// Abstract base class for a renderer
	/// </summary>
	abstract public class AbstractRenderer {

		#region Fields
		/// <summary>
		/// The callback function that Render() calls each time it fills a
		/// row of the image.
		/// </summary>
		protected RendererCallback _callBack;

		/// <summary>
		/// The source noise map that contains the coherent-noise values.
		/// </summary>
		protected IMap2D<float> _noiseMap;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the source noise map
		/// </summary>
		public IMap2D<float> NoiseMap {
			get { return _noiseMap; }
			set { _noiseMap = value; }
		}

		/// <summary>
		/// Gets or sets the callback function
		/// </summary>
		public RendererCallback CallBack {
			get { return _callBack; }
			set { _callBack = value; }
		}

		#endregion

		#region Interaction

		/// <summary>
		/// Renders the destination image using the contents of the source
		/// noise map.
		///
		/// @pre NoiseMap has been defined.
		/// @pre Image has been defined.
		///
		/// @post The original contents of the destination image is destroyed.
		///
		/// @throw ArgumentException See the preconditions.
		/// </summary>
		abstract public void Render();

		#endregion

	}//end class

}//end namespace
