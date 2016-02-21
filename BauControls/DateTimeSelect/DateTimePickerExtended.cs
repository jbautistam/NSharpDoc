using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.DateTimeSelect
{
	/// <summary>
	///		Control para seleccionar fecha y hora
	/// </summary>
	public partial class DateTimePickerExtended : UserControl
	{ // Delegados
			public delegate void ChangedHandler(object sender, EventArgs e);
		// Eventos
			public event ChangedHandler Changed;
		// Variables privadas
			private bool blnShowHours;
			private ctlCalendar mntCalendar = new ctlCalendar();
			private PopupWindow.PopUpWindow wndPopup;
			private Color clrBackColor;
			
		public DateTimePickerExtended()
		{	// Inicializa el componente
				InitializeComponent();
			// Inicializa el popup
				wndPopup = new Bau.Controls.PopupWindow.PopUpWindow(mntCalendar, this);
				mntCalendar.ControlParent = this;
			// Inicializa las variables
				Value = null;
				ShowHours = false;
			// Guarda el color del fondo
				clrBackColor = txtHour.BackColor;
			// Redimensiona el control
				ResizeControl();
		}

		/// <summary>
		///		Redimensiona el control
		/// </summary>
		private void ResizeControl()
		{ // Redimensiona el ancho y el alto
				if (ShowHours)
					Width = txtMinute.Left + txtMinute.Width;
				else
					Width = cmdShowCalendar.Left + cmdShowCalendar.Width;
				Height = txtDate.Top + txtDate.Height;
			// Oculta muestra los controles de horas y minutos
				txtHour.Visible = ShowHours;
				txtMinute.Visible = ShowHours;
		}

		/// <summary>
		///		Muestra el calendario
		/// </summary>
		private void ShowCalendar()
		{ // Asigna la fecha al calendario
				mntCalendar.Value = (Value ?? DateTime.Now);
			// Muestra el calendario
				wndPopup.Show();
		}

		/// <summary>
		///		Oculta el calendario
		/// </summary>
		internal void HideCalendar()
		{ Value = mntCalendar.Value;
			wndPopup.Hide();
		}
		
		/// <summary>
		///		Obtiene la fecha en formato de cadena
		/// </summary>
		private string GetDate()
		{ string strDate = "";

				// Obtiene la fecha		
					if (string.IsNullOrEmpty(txtDate.Text))
						strDate = null;
					else if (!ShowHours)
						strDate = txtDate.Text;
					else
						strDate = txtDate.Text + " " + ((int) txtHour.Value).ToString() + ":" + 
																					 ((int) txtMinute.Value).ToString() + ":00";
				// Devuelve la cadena con la fecha
					return strDate;
		}
		
		/// <summary>
		///		Comprueba si los datos introducidos son correctos
		/// </summary>
		private new bool Validate()
		{ DateTime dtmValue;
		
				return DateTime.TryParse(GetDate(), out dtmValue);
		}
		
		/// <summary>
		///		Sobrescribe el evento OnResize
		/// </summary>
		protected override void OnResize(EventArgs e)
		{	ResizeControl();
		}
		
		[Browsable(true), DefaultValue(false)]
		public bool ShowHours
		{ get { return blnShowHours; }
			set 
				{ // Cambia el valor
						blnShowHours = value;
					// Redimensiona
						ResizeControl();
				}
		}

		[Browsable(true)]
		public DateTime? Value
		{ get 
				{ if (Validate())
						return Convert.ToDateTime(GetDate()); 
					else
						return null;
				}
			set 
				{ if (value == null)
						{ txtDate.Text = "";
							txtHour.Value = 0;
							txtMinute.Value = 0;
						}
					else
						{ txtDate.Text = (value ?? DateTime.Now).ToShortDateString();
							txtHour.Value = (value ?? DateTime.Now).Hour;
							txtMinute.Value = (value ?? DateTime.Now).Minute;
						}
				}
		}
		
		[Browsable(false)]
		public bool IsNull
		{ get { return Value == null; }
		}
		
		[Browsable(true)]
		public Color BackColorEdit
		{	get {	return clrBackColor; }
			set
				{ // Guarda el color de fondo
						clrBackColor = value;
					// Cambia el color del fondo de los controles
						txtDate.BackColor = value;
						txtHour.BackColor = value;
						txtMinute.BackColor = value;
						if (Parent != null)
							base.BackColor = Parent.BackColor;
				}
		}

		[Browsable(true), DefaultValue(true)]
		public new bool Enabled
		{ get { return !txtDate.ReadOnly; }
			set 
				{ // Activa / desactiva los controles
						txtDate.ReadOnly = !value; 
						txtHour.ReadOnly = txtDate.ReadOnly;
						txtMinute.ReadOnly = txtDate.ReadOnly;
						cmdShowCalendar.Enabled = value;
					// Cambia el color de fondo
						if (txtDate.ReadOnly)
							txtDate.BackColor = Color.FromKnownColor(KnownColor.Control);
						else
							txtDate.BackColor = BackColorEdit;
						txtHour.BackColor = txtDate.BackColor;
						txtMinute.BackColor = txtDate.BackColor;
				}
		}
		
		private void cmdShowCalendar_Click(object sender, EventArgs e)
		{ ShowCalendar();
		}

		private void txtDate_TextChanged(object sender, EventArgs e)
		{ if (Changed != null)
				Changed(this, e);
		}
	}
}
