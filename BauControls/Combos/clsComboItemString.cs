using System;

namespace Bau.Controls.Combos
{
	/// <summary>
	///		Clase con el elemento de un combo
	/// </summary>
	public class clsComboItemString
	{ 		
		public clsComboItemString(string intID, string strText)
		{ ID = intID;
			Text = strText;
		}
		
		public string ID { get; set; }
		
		public string Text { get; set; }
	}
}
