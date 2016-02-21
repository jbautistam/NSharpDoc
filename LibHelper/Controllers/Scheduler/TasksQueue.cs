using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bau.Libraries.LibHelper.Controllers.Scheduler
{
	/// <summary>
	///		Cola de tareas
	/// </summary>
	public class TasksQueue
	{	// Eventos
			public event EventHandler<EventArguments.ActionEventArgs> ActionProcess;
			public event EventHandler<EventArguments.ProgressActionEventArgs> ProgressAction;
			public event EventHandler<EventArguments.EndProcessEventArgs> EndProcess;
			public event EventHandler<EventArguments.ProgressEventArgs> Progress;

		public TasksQueue()
		{ Queue = new List<AbstractProcessor>();
		}

		/// <summary>
		///		Comprueba si existe un procesador por su tipo
		/// </summary>
		public bool ExistsByType(Type objType)
		{ return Queue.FirstOrDefault<AbstractProcessor>(objItem => objItem.GetType().Equals(objType)) != null;
		}

		/// <summary>
		///		Añade una tarea a la cola y la ejecuta
		/// </summary>
		public void Process(AbstractProcessor objProcessor)
		{ Task objTask;
				
				// Añade el procesador a la cola
					Queue.Add(objProcessor);
				// Asigna los manejador de eventos
					objProcessor.ActionProcess += (objSender, objEventArgs) =>
																						{ if (ActionProcess != null)
																								ActionProcess(objSender, objEventArgs);
																						};
					objProcessor.Progress += (objSender, objEventArgs) =>
																			{ if (Progress != null)
																					Progress(objSender, objEventArgs);
																			};
					objProcessor.ProgressAction += (objSender, objEventArgs) =>
																						{ if (ProgressAction != null)
																								ProgressAction(objSender, objEventArgs);
																						};
					objProcessor.EndProcess += (objSender, objEventArgs) =>
																			{ TreatEndProcess(objSender as AbstractProcessor, objEventArgs);
																			};
				// Crea la tarea para la compilación en otro hilo
					objTask = new Task(() => objProcessor.Process());
				// Arranca la tarea de generación
					try
						{ objTask.Start();
						}
					catch (Exception objException)
						{ TreatEndProcess(objProcessor, 
															new EventArguments.EndProcessEventArgs("Error al lanzar el proceso" + Environment.NewLine + objException.Message,
																																		 new List<string> {objException.Message } ));
						}
		}

		/// <summary>
		///		Trata el final del proceso
		/// </summary>
		private void TreatEndProcess(AbstractProcessor objProcessor, EventArguments.EndProcessEventArgs objEventArgs)
		{ // Elimina el procesador de la cola
				Queue.Remove(objProcessor);
			// Lanza el evento de fin de proceso
				if (EndProcess != null)
					EndProcess(objProcessor, objEventArgs);
		}

		/// <summary>
		///		Cola de procesos
		/// </summary>
		public List<AbstractProcessor> Queue { get; private set; }
	}
}
