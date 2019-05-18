using System;

namespace Bau.Controls.Combos
{
	/// <summary>
	///		Clase con el elemento de un combo
	/// </summary>
	public class clsComboItem
	{ 		
		public clsComboItem(int? intID, string text)
		{ ID = intID;
			Text = text;
		}
		
		public int? ID { get; set; }
		
		public string Text { get; set; }
	}
}
