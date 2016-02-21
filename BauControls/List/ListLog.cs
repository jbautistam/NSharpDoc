using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.List
{
	/// <summary>
	///		Lista para mostrar mensajes de log
	/// </summary>
	public partial class ListLog : UserControl
	{ // Enumerados públicos
			public enum LogType
				{ Normal,
					Debug,
					Error
				}
				
		public ListLog()
		{ // Inicializa los componentes
				InitializeComponent();
			// Inicializa las propiedades
				MaxItems = 200;
			// Inicializa la lista
				Clear();
		}
		
		/// <summary>
		///		Limpia la lista
		/// </summary>
		public void Clear()
		{ InitListLog();
		}
		
		/// <summary>
		///		Inicializa la lista de log
		/// </summary>
		private void InitListLog()
		{ // Limpia la lista
				lswLog.Clear();
			// Asigna las columnas
				lswLog.AddColumn(150, "Hora");
				lswLog.AddColumn(1000, "Mensaje");
		}

		/// <summary>
		///		Añade un mensaje al log
		/// </summary>
		public void AddLog(string strMessage)
		{ AddLog(LogType.Normal, strMessage, Color.Black);
		}

		/// <summary>
		///		Añade un mensaje al log
		/// </summary>
		public void AddLog(string strMessage, Color clrColor)
		{ AddLog(LogType.Normal, strMessage, clrColor);
		}

		/// <summary>
		///		Añade un mensaje al log
		/// </summary>
		public void AddLog(LogType intType, string strMessage)
		{ AddLog(intType, strMessage, Color.Black);
		}

		/// <summary>
		///		Añade un mensaje al log
		/// </summary>
		public void AddLog(LogType intType, string strMessage, Color clrColor)
		{ ListViewItem lsiItem;
		
				// Comienza las modificaciones
					lswLog.BeginUpdate();
				// Añade el elemento
					lsiItem = lswLog.AddItem("", string.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now));
				// Añade los subitems
					lsiItem.SubItems.Add(strMessage);
				// Cambia los colores
					switch (intType)
						{ case LogType.Error:
									lsiItem.ForeColor = Color.Red;
								break;
							default:
									lsiItem.ForeColor = clrColor;
								break;
						}
				// Borra los elementos sobrantes
					RemoveFirstItems();
				// Selecciona el elemento
					if (lswLog.Items.Count > 0)
						lswLog.EnsureVisible(lswLog.Items.Count - 1);
				// Finaliza las modificaciones
					lswLog.EndUpdate();
		}

		/// <summary>
		///		Elimina los primeros elementos
		/// </summary>
		private void RemoveFirstItems()
		{ while (lswLog.Items.Count > MaxItems)
				lswLog.Items.RemoveAt(0);
		}
		
		/// <summary>
		///		Número máximo de elementos
		/// </summary>
		[Browsable(true), DefaultValue(200)]
		public int MaxItems { get; set; }
		
		private void cmdDelete_Click(object sender, EventArgs e)
		{ Clear();
		}
	}
}
