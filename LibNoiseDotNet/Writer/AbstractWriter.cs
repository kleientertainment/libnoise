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
	/// Abstract base class for all writer classes
	/// </summary>
	abstract public class AbstractWriter {


		#region Fields

		/// <summary>
		/// the name of the file to write.
		/// </summary>
		protected string _filename;

		/// <summary>
		/// A binary writer
		/// </summary>
		protected BinaryWriter _writer;


		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the name of the file to write.
		/// </summary>
		public string Filename {
			get { return _filename; }
			set { _filename = value; }
		}//end Filename


		#endregion

		#region Interaction

		/// <summary>
		/// Writes the destination content
		/// </summary>
		abstract public void WriteFile();

		#endregion

		#region internal

		/// <summary>
		/// Create a new BinaryWriter
		/// </summary>
		protected void OpenFile() {

			if(_writer != null) {
				return; // Should throw exception ?
			}//end if

			if(File.Exists(_filename)) {
				try {
					File.Delete(_filename);
				}//end try
				catch(Exception e) {
					throw new IOException("Unable to delete destination file", e);
				}//end catch
			}//end if

			BufferedStream stream;

			try {
				stream = new BufferedStream(new FileStream(_filename, FileMode.Create));
			}//end try
			catch(Exception e) {
				throw new IOException("Unable to create destination file", e);
			}//end catch

			_writer = new BinaryWriter(stream);

		}//end OpenFile

		/// <summary>
		/// Release a BinaryWriter previously opened
		/// </summary>
		protected void CloseFile() {

			try {
				_writer.Flush();
				_writer.Close();
				_writer = null;
			}//end try
			catch(Exception e) {
				throw new IOException("Unable to release stream", e);
			}//end catch

		}//end CloseFile

		#endregion

	}//end class

}//end namespace
