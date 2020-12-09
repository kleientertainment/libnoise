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

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier {

	/// <summary>
	/// Noise module that outputs a weighted blend of the output values from
	/// two source modules given the output value supplied by a control module.
	///
	/// - LeftModule outputs one of the values to blend.
	/// - RightModule outputs one of the values to blend.
	/// - ControlModule is known as the <i>control
	///   module</i>.  The control module determines the weight of the
	///   blending operation.  Negative values weigh the blend towards the
	///   output value from the LeftModule.
	///   Positive values weigh the blend towards the output value from the
	///   ReftModule.
	///   
	/// This noise module uses linear interpolation to perform the blending
	/// operation.
	///
	/// </summary>
	public class Blend :SelectorModule, IModule3D {

		#region Fields

		/// <summary>
		/// The control module
		/// </summary>
		protected IModule _controlModule;

		/// <summary>
		/// The right input module
		/// </summary>
		protected IModule _rightModule;

		/// <summary>
		/// The left input module
		/// </summary>
		protected IModule _leftModule;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the left module
		/// </summary>
		public IModule LeftModule {
			get { return _leftModule; }
			set { _leftModule = value; }
		}

		/// <summary>
		/// Gets or sets the right module
		/// </summary>
		public IModule RightModule {
			get { return _rightModule; }
			set { _rightModule = value; }
		}

		/// <summary>
		/// Gets or sets the control module
		/// </summary>
		public IModule ControlModule {
			get { return _controlModule; }
			set { _controlModule = value; }
		}

		#endregion

		#region Ctor/Dtor
		public Blend() {

		}//end Blend

		public Blend(IModule controlModule, IModule rightModule, IModule leftModule) {
			_controlModule = controlModule;
			_leftModule = leftModule;
			_rightModule = rightModule;

		}//end Blend

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

			float v0 = ((IModule3D)_leftModule).GetValue(x, y, z);
			float v1 = ((IModule3D)_rightModule).GetValue(x, y, z);
			float alpha = (((IModule3D)_controlModule).GetValue(x, y, z) + 1.0f) / 2.0f;
			return Libnoise.Lerp(v0, v1, alpha);

		}//end GetValue

		#endregion

	}//end class

}//end namespace
