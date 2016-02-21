using System;

namespace Bau.Libraries.LibHelper.Controllers.EventArguments
{
	/// <summary>
	///		Eventos de sincronización
	/// </summary>
	public class ActionEventArgs : EventArgs
	{
		/// <summary>
		///		Tipo de acción
		/// </summary>
		public enum ActionType
			{ 
				/// <summary>Arranca el proceso</summary>
				Start,
				/// <summary>Otros procesos</summary>
				Other,
				/// <summary>Error</summary>
				Error,
				/// <summary>Finaliza el proceso</summary>
				End
			}

		public ActionEventArgs(ActionType intType, string strModule, string strMessage)
		{ Action = intType;
			Module = strModule;
			Message = strMessage;
		}

		/// <summary>
		///		Módulo que lanza el proceso
		/// </summary>
		public string Module { get; private set; }

		/// <summary>
		///		Tipo de acción
		/// </summary>
		public ActionType Action { get; private set; }

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; private set; }
	}
}
