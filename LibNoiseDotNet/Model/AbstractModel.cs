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


namespace LibNoiseDotNet.Graphics.Tools.Noise.Model {

	/// <summary>
	/// Abstract base class for all Model
	///
	/// Model must defined their own GetValue() method
	/// </summary>
	public class AbstractModel {

		#region Fields

		/// <summary>
		/// The source input module
		/// </summary>
		protected IModule _sourceModule;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the source module
		/// </summary>
		public IModule SourceModule {
			get { return _sourceModule; }
			set { _sourceModule = value; }
		}
		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Default constructor
		/// </summary>
		public AbstractModel() {

		}//end Plane

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="module">The noise module that is used to generate the output values</param>
		public AbstractModel(IModule module) {
			_sourceModule = module;
		}//end Plane

		#endregion

	}//end class

}//end namespace
