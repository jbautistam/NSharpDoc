using System;

namespace Bau.Controls.ListRepeat
{
	/// <summary>
	///		Clase con los datos de un control <see cref="ListBoxControls"/>
	/// </summary>
	internal class clsListControl
	{ // Variables privadas
			private System.Windows.Forms.Control ctlControl;
			private int intTop, intPaddingLeft;
			
		public clsListControl(int intPaddingLeft, System.Windows.Forms.Control ctlControl)
		{ PaddingLeft = intPaddingLeft;
			Control = ctlControl;
		}
		
		public System.Windows.Forms.Control Control
		{ get { return ctlControl; }
			set { ctlControl = value; }
		}
		
		public int Top
		{ get { return intTop; }
			set { intTop = value; }
		}
		
		public int PaddingLeft
		{ get { return intPaddingLeft; }
			set { intPaddingLeft = value; }
		}
	}
}
