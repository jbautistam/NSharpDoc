using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.DateTimeSelect
{
	/// <summary>
	///		Control para mostrar un calendario en un popUp
	/// </summary>
	[ToolboxItem(false)]
	internal partial class ctlCalendar : UserControl
	{ // Variables privadas
			private DateTimePickerExtended fudDate;
			private DateTime? dtmValue = null;
			
		public ctlCalendar()
		{	InitializeComponent();
		}

		public DateTimePickerExtended ControlParent
		{ get { return fudDate; }
			set { fudDate = value; }
		}
		
		public DateTime? Value
		{ get { return dtmValue; }
			set 
				{ dtmValue = value; 
					mntCalendar.SelectionStart = value ?? DateTime.Now;
				}
		}

		private void mntCalendar_DateSelected(object sender, DateRangeEventArgs e)
		{ Value = e.Start;
			ControlParent.HideCalendar();
		}
	}
}
