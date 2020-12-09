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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Tranformer {

	/// <summary>
	/// Noise module that moves the coordinates of the input value before
	/// returning the output value from a source module.
	///
	/// The GetValue() method moves the ( x, y, z ) coordinates of
	/// the input value by a translation amount before returning the output
	/// value from the source module. 
	///
	///
	/// </summary>
	public class TranslatePoint :TransformerModule, IModule3D {

		#region Connstant
		/// <summary>
		/// The default translation amount to apply to the x coordinate
		/// </summary>
		public const float DEFAULT_TRANSLATE_X = 1.0f;

		/// <summary>
		/// The default translation amount to apply to the y coordinate
		/// </summary>
		public const float DEFAULT_TRANSLATE_Y = 1.0f;

		/// <summary>
		/// The default translation amount to apply to the z coordinate
		/// </summary>
		public const float DEFAULT_TRANSLATE_Z = 1.0f;

		#endregion

		#region Fields
		/// <summary>
		/// The source input module
		/// </summary>
		protected IModule _sourceModule;

		/// <summary>
		/// the translation amount to apply to the x coordinate
		/// </summary>
		protected float _xTranslate = DEFAULT_TRANSLATE_X;

		/// <summary>
		/// the translation amount to apply to the y coordinate
		/// </summary>
		protected float _yTranslate = DEFAULT_TRANSLATE_Y;

		/// <summary>
		/// the translation amount to apply to the z coordinate
		/// </summary>
		protected float _zTranslate = DEFAULT_TRANSLATE_Z;

		#endregion

		#region Accessors
		/// <summary>
		/// Gets or sets the source module
		/// </summary>
		public IModule SourceModule {
			get { return _sourceModule; }
			set { _sourceModule = value; }
		}

		/// <summary>
		/// Gets or sets the translation amount to apply to the x coordinate
		/// </summary>
		public float XTranslate {
			get { return _xTranslate; }
			set { _xTranslate = value; }
		}

		/// <summary>
		/// Gets or sets the translation amount to apply to the y coordinate
		/// </summary>
		public float YTranslate {
			get { return _yTranslate; }
			set { _yTranslate = value; }
		}

		/// <summary>
		/// Gets or sets the translation amount to apply to the z coordinate
		/// </summary>
		public float ZTranslate {
			get { return _zTranslate; }
			set { _zTranslate = value; }
		}

		#endregion

		#region Ctor/Dtor
		/// <summary>
		/// Create a new noise module with default values
		/// </summary>
		public TranslatePoint() {

		}//end TranslatePoint


		/// <summary>
		/// Create a new noise module with given values
		/// </summary>
		/// <param name="source">the source module</param>
		public TranslatePoint(IModule source) {
			_sourceModule = source;
		}//end TranslatePoint

		/// <summary>
		/// Create a new noise module with given values
		/// </summary>
		/// <param name="source">the source module</param>
		/// <param name="x">the translation amount to apply to the x coordinate</param>
		/// <param name="y">the translation amount to apply to the y coordinate</param>
		/// <param name="z">the translation amount to apply to the z coordinate</param>
		public TranslatePoint(IModule source, float x, float y, float z)
			: this(source) {
			_xTranslate = x;
			_yTranslate = y;
			_zTranslate = z;
		}//end TranslatePoint

		#endregion


		#region IModule3D Members

		/// <summary>
		/// Generates an output value given the coordinates of the specified input value.
		/// </summary>
		/// <param name="x">The input coordinate on the x-axis.</param>
		/// <param name="y">The input coordinate on the y-axis.</param>
		/// <param name="z">The input coordinate on the z-axis.</param>
		/// <returns>The resulting output value.</returns>
		public float GetValue(float x, float y, float z) {
			return ((IModule3D)_sourceModule).GetValue(x + _xTranslate, y + _yTranslate, z + _zTranslate);
		}//end GetValue

		#endregion

	}//end class

}//end namespace
