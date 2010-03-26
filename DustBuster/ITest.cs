
using System;

namespace GameClay
{
	namespace DustBuster
	{
	
		/// <summary>
		/// Test result
		/// </summary>
		public enum Result
		{
			Passed,
			Failed
		}
		
		/// <summary>
		/// This is a really simple automated test.
		/// </summary>
		public interface ITest
		{
			/// <summary>
			/// Run the test, and return the result of the test.
			/// </summary>
			/// 
			/// <returns> Returns the <see cref="Result"/> of the test. </returns>
			Result RunTest(); // TODO: Pass in a test delegate which is fed a boolean?
		}
	}
}