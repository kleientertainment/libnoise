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

	/// <summary>
	/// Abstract base class for an image renderer
	/// </summary>
	abstract public class AbstractImageRenderer : AbstractRenderer{

		#region Fields

		/// <summary>
		/// The destination image
		/// </summary>
		protected IMap2D<IColor> _image;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the destination image
		/// </summary>
		public IMap2D<IColor> Image {
			get { return _image; }
			set { _image = value; }
		}

		#endregion

	}//end class

}//end namespace
