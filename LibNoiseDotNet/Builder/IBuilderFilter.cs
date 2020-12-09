// This file is part of Libnoise-dotnet.
//
// Libnoise-dotnet is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Libnoise-dotnet is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Libnoise-dotnet.  If not, see <http://www.gnu.org/licenses/>.


namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder {

	/// <summary>
	/// ConstantOutput : caller should use Constant property
	/// SourceOutput : caller should use source module value
	/// FilterOutput : caller should use FilterValue method
	/// </summary>
	public enum FilterLevel {
		Constant,
		Source,
		Filter
	}//end FilterLevel

	/// <summary>
	/// 
	/// </summary>
	public interface IBuilderFilter {

		#region Accessors

		/// <summary>
		/// 
		/// </summary>
		float ConstantValue { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		float FilterValue(int x, int y, float source);

		#endregion

		#region Interaction

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		FilterLevel IsFiltered(int x, int y);

		#endregion
	}//end interface
}//end namespace
