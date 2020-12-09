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
	/// Windows bitmap image writer class.
	///
	/// This class creates a file in Windows bitmap (*.bmp) format given the
	/// contents of an image object.
	///
	/// <b>Writing the image</b>
	///
	/// To write the image to a file, perform the following steps:
	/// - Pass the filename to the Filename property.
	/// - Pass an Image object to the Image property.
	/// - Call the WriteFile().
	/// 
	/// TODO convert BMPWriter to an extensible writing strategy based on image format (bmp, png, jpg, ...)
	/// </summary>
	public class BMPWriter : AbstractWriter{

		#region constants
		
		/// <summary>
		/// Bitmap header size.
		/// </summary>
		public const int BMP_HEADER_SIZE = 54;

		#endregion

		#region Fields

		/// <summary>
		/// The destination image
		/// </summary>
		protected Image _image;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the destination image
		/// </summary>
		public Image Image {
			get { return _image; }
			set { _image = value; }
		}

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// 0-args constructor
		/// </summary>
		public BMPWriter() {

		}//end BMPWriter

		#endregion

		#region Interaction
		
		/// <summary>
		/// Writes the contents of the image object to the file.
		///
		/// @pre Filename has been previously defined.
		/// @pre Image has been previously defined.
		///
		/// @throw ArgumentException See the preconditions.
		/// @throw IOException An I/O exception occurred.
		/// 
		/// Possibly the file could not be written.
		/// 
		/// </summary>
		public override void WriteFile(){ 

			if(_image == null) {
				throw new ArgumentException("An image map must be provided");
			}//end id

			int width  = _image.Width;
			int height = _image.Height;

			// The width of one line in the file must be aligned on a 4-byte boundary.
			int bufferSize = CalcWidthByteCount(width);
			int destSize   = bufferSize * height;

			// This buffer holds one horizontal line in the destination file.
			// Allocate a buffer to hold one horizontal line in the bitmap.
			byte[] pLineBuffer = new byte[bufferSize];
			
			OpenFile();

			// Build and write the header.
			// A 32 bit buffer
			byte[] b4 = new byte[4];

			// A 16 bit buffer
			byte[] b2 = new byte[2];

			b2[0] = 0x42; //B
			b2[1] = 0x4D; //M

			try {
				_writer.Write(b2); //BM Magic number 424D 
				_writer.Write(Libnoise.UnpackLittleUint32(destSize + BMP_HEADER_SIZE, ref b4));

				_writer.Write(Libnoise.UnpackLittleUint32(0, ref b4));

				_writer.Write(Libnoise.UnpackLittleUint32(BMP_HEADER_SIZE, ref b4));
				_writer.Write(Libnoise.UnpackLittleUint32(40, ref b4)); // Palette offset
				_writer.Write(Libnoise.UnpackLittleUint32(width, ref b4)); // width
				_writer.Write(Libnoise.UnpackLittleUint32(height, ref b4)); // height
				_writer.Write(Libnoise.UnpackLittleUint16((short)1, ref b2)); // Planes per pixel
				_writer.Write(Libnoise.UnpackLittleUint16((short)24, ref b2)); // Bits per plane

				_writer.Write(Libnoise.UnpackLittleUint32(0, ref b4)); // Compression (0 = none)

				_writer.Write(Libnoise.UnpackLittleUint32(destSize, ref b4));
				_writer.Write(Libnoise.UnpackLittleUint32(2834, ref b4)); // X pixels per meter
				_writer.Write(Libnoise.UnpackLittleUint32(2834, ref b4)); // Y pixels per meter

				_writer.Write(Libnoise.UnpackLittleUint32(0, ref b4));
				_writer.Write(b4);

				// Build and write each horizontal line to the file.
				for (int y = 0; y < height; y++){

					int i = 0;

					// Each line is aligned to a 32-bit boundary (\0 padding)
					Array.Clear(pLineBuffer, 0, pLineBuffer.Length);

					Color pSource;

					for (int x = 0; x < width; x++) {

						pSource = _image.GetValue(x, y);

						// Little endian order : B G R
						pLineBuffer[i++] = pSource.Blue;
						pLineBuffer[i++] = pSource.Green;
						pLineBuffer[i++] = pSource.Red;

					}//end for

					_writer.Write(pLineBuffer);

				}//end for
			}//end try
			catch(Exception e) {
				throw new IOException("Unknown IO exception", e);
			}//end catch

			CloseFile();

		}//end WriteFile

		#endregion

		#region Internal

		/// <summary>
		/// Calculates the width of one horizontal line in the file, in bytes.
		///
		/// Windows bitmap files require that the width of one horizontal line
		/// must be aligned to a 32-bit boundary.
		/// </summary>
		/// <param name="width">The width of the image, in points</param>
		/// <returns>The width of one horizontal line in the file</returns>
		protected int CalcWidthByteCount(int width) {
			return ((width * 3) + 3) & ~0x03;
		}//end CalcWidthByteCount

		#endregion

	}//end class

}//end namespace
