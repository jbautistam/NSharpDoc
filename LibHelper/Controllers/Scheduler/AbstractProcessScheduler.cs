using System;

namespace Bau.Libraries.LibHelper.Controllers.Scheduler
{
	/// <summary>
	///		Ejecución de procesos cada cierto tiempo
	/// </summary>
	public abstract class AbstractProcessScheduler
	{ // Variables privadas
			private System.Timers.Timer objTimer;

		public AbstractProcessScheduler(bool blnAutoStart, int intMinutesBetweenProcess = 60)
		{ // Indica que está detenido
				Paused = true;
			// Inicializa la fecha de última ejecución
				DateLastExecute = DateTime.Now.AddMinutes(-(intMinutesBetweenProcess - 10));
			// Temporizador
				objTimer = new System.Timers.Timer(60000);
				objTimer.Elapsed += (objSender, evntArgs) => { ProcessTimer(); };
			// Arranca el proceso si es de inicio automático
				if (blnAutoStart)
					Start();
		}

		/// <summary>
		///		Arranca el proceso
		/// </summary>
		public void Start()
		{ Paused = false;
			objTimer.Enabled = true;
			objTimer.Start();
		}

		/// <summary>
		///		Detiene el proceso
		/// </summary>
		public void Pause()
		{ Paused = true;
			objTimer.Enabled = false;
			objTimer.Stop();
		}

		/// <summary>
		///		Procesa los datos
		/// </summary>
		private void ProcessTimer()
		{ // Detiene el temporizador
				objTimer.Stop();
			// Si se debe ejecutar, crea un hilo nuevo
				if (MustExecute())
					{ System.Threading.Tasks.Task objTaskCompiler = new System.Threading.Tasks.Task(() => CallProcess());

							// Arranca el hilo
								objTaskCompiler.Start();
					}
		}

		/// <summary>
		///		Llama al procesador real (ya en un hilo nuevo)
		/// </summary>
		private void CallProcess()
		{ // Llama al proceso interno
				Process();
			// Arranca de nuevo el temporizador
				if (!Paused)
					Start();
			// Indica la última vez que se ha ejecutado
				DateLastExecute = DateTime.Now;
		}
		
		/// <summary>
		///		Comprueba si se debe ejecutar
		/// </summary>
		private bool MustExecute()
		{ return !Paused && DateTime.Now > DateLastExecute.AddMinutes(MinutesBetweenProcess);
		}

		/// <summary>
		///		Realiza el proceso particular
		/// </summary>
		protected abstract void Process();

		/// <summary>
		///		Indica si está detenido
		/// </summary>
		public bool Paused { get; set; }

		/// <summary>
		///		Hora de última ejecución
		/// </summary>
		public DateTime DateLastExecute { get; private set; }

		/// <summary>
		///		Minutos entre proceso
		/// </summary>
		public int MinutesBetweenProcess { get; private set; }
	}
}