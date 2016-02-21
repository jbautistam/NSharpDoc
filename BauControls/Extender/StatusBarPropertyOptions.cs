using System;

namespace Bau.Controls.Extender
{
	/// <summary>
	///		Contiene las opciones de los elementos que necesitan un texto en la barra de estado
	/// </summary>
	internal sealed class StatusBarPropertyOptions
	{ // Variables privadas		
			private string strMessage;
			private bool blnShowAsBlank;

		internal StatusBarPropertyOptions(string strMessage)
		{ Message = strMessage;
		}

		internal StatusBarPropertyOptions(bool blnShowAsBlank)
		{ ShowAsBlank = blnShowAsBlank;
		}

		internal string Message
		{	get { return strMessage; }
			set { strMessage = value; }
		}

		internal bool ShowAsBlank
		{ get { return blnShowAsBlank; }
			set { blnShowAsBlank = value; }
		}
	}
}
