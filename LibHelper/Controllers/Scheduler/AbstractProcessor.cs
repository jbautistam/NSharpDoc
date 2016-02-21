using System;

using Bau.Libraries.LibHelper.Controllers.EventArguments;

namespace Bau.Libraries.LibHelper.Controllers.Scheduler
{
	/// <summary>
	///		Clase de proceso abstracta
	/// </summary>
	public abstract class AbstractProcessor
	{ // Eventos
			public event EventHandler<ActionEventArgs> ActionProcess;
			public event EventHandler<ProgressActionEventArgs> ProgressAction;
			public event EventHandler<EndProcessEventArgs> EndProcess;
			public event EventHandler<ProgressEventArgs> Progress;
		// Variables privadas
			private string strID;

		public AbstractProcessor(string strSource)
		{ Source = strSource;
		}

		/// <summary>
		///		Procesa los datos
		/// </summary>
		public abstract void Process();

		/// <summary>
		///		Lanza el evento de inicio
		/// </summary>
		protected void RaiseEventStart(string strMessage)
		{ RaiseEvent(ActionEventArgs.ActionType.Start, strMessage);
		}

		/// <summary>
		///		Lanza un evento de error
		/// </summary>
		protected void RaiseEventError(string strMessage, Exception objException)
		{ if (objException != null)
				strMessage += Environment.NewLine + objException.Message;
			RaiseEvent(ActionEventArgs.ActionType.Error, strMessage);
		}

		/// <summary>
		///		Lanza el evento de fin
		/// </summary>
		protected void RaiseEventEnd(string strMessage)
		{ RaiseEvent(ActionEventArgs.ActionType.Start, strMessage);
		}

		/// <summary>
		///		Lanza un mensaje en un evento
		/// </summary>
		protected void RaiseMessageEvent(string strMessage)
		{ RaiseEvent(ActionEventArgs.ActionType.Other, strMessage);
		}

		/// <summary>
		///		Lanza un evento
		/// </summary>
		protected void RaiseEvent(ActionEventArgs.ActionType intAction, string strMessage)
		{ if (ActionProcess != null)
				ActionProcess(this, new ActionEventArgs(intAction, Source, strMessage));
		}

		/// <summary>
		///		Lanza un evento de progreso
		/// </summary>
		protected void RaiseEventProgress(int intActual, int intTotal, string strMessage)
		{ if (Progress != null)
				Progress(this, new EventArguments.ProgressEventArgs(intActual, intTotal, strMessage));
		}

		/// <summary>
		///		Lanza un evento de fin de proceso
		/// </summary>
		protected void RaiseEventEndProcess(string strMessage, System.Collections.Generic.List<string> objColErrors)
		{	if (EndProcess != null)
				EndProcess(this, new EndProcessEventArgs(strMessage, objColErrors));
		}

		/// <summary>
		///		Lanza un evento de progreso de una acción
		/// </summary>
		protected void RaiseEventProgressAction(string strID, string strAction, string strProcess, long lngActual, long lngTotal)
		{ if (ProgressAction != null)
				ProgressAction(this, new ProgressActionEventArgs(strID, Source, strAction, strProcess, lngActual, lngTotal));
		}

		/// <summary>
		///		Identificador del proceso
		/// </summary>		
		public string ID
		{ get 
				{ // Crea un ID si no existía
						if (strID == null)
							strID = Guid.NewGuid().ToString();
					// Devuelve el ID
						return strID; 
				}
			set { strID = value; }
		}

		/// <summary>
		///		Origen del proceso
		/// </summary>
		public string Source { get; private set; }
	}
}
