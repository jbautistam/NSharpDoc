using System;

namespace Bau.Libraries.LibHelper.Controllers.EventArguments
{
	/// <summary>
	///		Argumentos de los eventos de progreso
	/// </summary>
	public class ProgressActionEventArgs : EventArgs
	{
		public ProgressActionEventArgs(string strID, string strModule, string strAction, string strProcess, long lngActual, long lngTotal)
		{ ID = strID;
			Module = strModule;
			Action = strAction;
			Process = strProcess;
			Actual = lngActual;
			Total = lngTotal;
		}

		/// <summary>
		///		ID del proceso
		/// </summary>
		public string ID { get; private set; }

		/// <summary>
		///		Módulo que lanza el proceso
		/// </summary>
		public string Module { get; private set; }

		/// <summary>
		///		Acción realizada
		/// </summary>
		public string Action { get; private set; }

		/// <summary>
		///		Proceso
		/// </summary>
		public string Process { get; private set; }

		/// <summary>
		///		Progreso
		/// </summary>
		public long Actual { get; private set; }

		/// <summary>
		///		Total
		/// </summary>
		public long Total { get; private set; }
	}
}
