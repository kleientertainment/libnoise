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

namespace LibNoiseDotNet.Graphics.Tools.Noise{

	/// <summary>
	/// Abstract interface for noise modules.
	///
	/// A <i>noise module</i> is an object that calculates and outputs a value
	/// given a N-dimensional input value.
	///
	/// Each type of noise module uses a specific method to calculate an
	/// output value.  Some of these methods include:
	///
	/// - Calculating a value using a coherent-noise function or some other
	///   mathematical function.
	/// - Mathematically changing the output value from another noise module
	///   in various ways.
	/// - Combining the output values from two noise modules in various ways.
	///
	/// </summary>
	public interface IModule{

	}//end class

}//end namespace
