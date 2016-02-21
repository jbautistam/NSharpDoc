using System;

namespace Bau.Libraries.LibHelper.Controllers.EventArguments
{
	/// <summary>
	///		Argumento para los eventos de fin de proceso
	/// </summary>
	public class EndProcessEventArgs : EventArgs
	{
		public EndProcessEventArgs(string strMessage = null, System.Collections.Generic.List<string> objColErrors = null)
		{ Message = strMessage;
			Errors = objColErrors;
		}

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; private set; }

		/// <summary>
		///		Errores del proceso
		/// </summary>
		public System.Collections.Generic.List<string> Errors { get; private set; }
	}
}
