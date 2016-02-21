using System;

namespace Bau.Libraries.LibMarkupLanguage.Services
{
	/// <summary>
	///		Excepciones del intérprete
	/// </summary>
	public class ParserException : Exception
	{
		public ParserException(string strMessage) : base(strMessage) {}

		public ParserException(string strMessage, Exception objInnerException) : base(strMessage, objInnerException) {}
	}
}