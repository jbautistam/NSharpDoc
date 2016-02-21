using System;

namespace Bau.Libraries.LibHelper.Controllers.EventArguments
{
	/// <summary>
	///		Argumento para los eventos de progreso
	/// </summary>
	public class ProgressEventArgs : EventArgs
	{
		public ProgressEventArgs(int intActual, int intTotal, string strMessage = null)
		{ Actual = intActual;
			Total = intTotal;
			Message = strMessage;
		}

		/// <summary>
		///		Actual
		/// </summary>
		public int Actual { get; private set; }

		/// <summary>
		///		Total
		/// </summary>
		public int Total { get; private set; }

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; private set; }
	}
}
