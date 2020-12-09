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
using System.IO;
using LibNoiseDotNet.Graphics.Tools.Noise.Renderer;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Writer {

	/// <summary>
	/// Heightmap writer class, raw format.
	/// </summary>
	public class Heightmap8RawWriter :AbstractWriter {

		#region Fields

		/// <summary>
		/// The heightmap to write
		/// </summary>
		protected Heightmap8 _heightmap;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the heightmap to write
		/// </summary>
		public Heightmap8 Heightmap {
			get { return _heightmap; }
			set { _heightmap = value; }
		}

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public Heightmap8RawWriter() {

		}//end Heightmap8RawWriter

		#endregion

		#region Interaction

		/// <summary>
		/// Writes the contents of the heightmap into the file.
		/// 
		/// @throw IOException An I/O exception occurred.
		/// 
		/// Possibly the file could not be written.
		/// 
		/// </summary>
		/// <param name="heightmap"></param>
		public override void WriteFile() {

			if(_heightmap == null) {
				throw new ArgumentException("An heightmap must be provided");
			}//end id

			OpenFile();

			try {
				// ... Raw format ...
				_writer.Write(_heightmap.Share());
			}//end try
			catch(Exception e) {
				throw new IOException("Unknown IO exception", e);
			}//end catch

			CloseFile();

		}//end WriteFile

		#endregion

	}//end class

}//end namespace
