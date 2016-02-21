using System;

namespace Bau.Libraries.LibDBProvidersBase.DBExceptions
{
	/// <summary>
	///		Excepción de base de datos
	/// </summary>
	public class DBException : Exception
	{
		public DBException(string strMessage) : base(strMessage) {}

		public DBException(string strMessage, Exception objInnerException) : base(strMessage, objInnerException) {}
	}
}
