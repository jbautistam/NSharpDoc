using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Bau.Controls.Combos
{
	/// <summary>
	///		Extensión del comboBox para que admita elemento con ID de tipo cadena y Text
	/// </summary>
	public class ComboBoxExtendedString : ComboBox
	{
		public ComboBoxExtendedString()
		{ DropDownStyle = ComboBoxStyle.DropDownList;
		}
		
		/// <summary>
		///		Añade un elemento al combo
		/// </summary>
		public void AddItem(string strID, string strText)
		{ AddItem(new clsComboItemString(strID, strText));
		}
		
		/// <summary>
		///		Añade un elemento al combo
		/// </summary>
		public void AddItem(clsComboItemString objItem)
		{ // Añade el elemento
				Items.Add(objItem);
			// Cambia el miembro a visualizar y el oculto
				DisplayMember = "Text";
				ValueMember = "ID";
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]		
		public string SelectedID
		{ get
				{ if (SelectedItem == null)
						return null;
					else
						return ((clsComboItemString) SelectedItem).ID;
				}
			set 
				{ foreach (clsComboItemString objItem in Items)
						if (objItem.ID == value)
							SelectedItem = objItem;
				}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]		
		public new string SelectedText
		{ get
				{ if (SelectedItem == null)
						return null;
					else
						return ((clsComboItemString) SelectedItem).Text;
				}
			set 
				{ foreach (clsComboItemString objItem in Items)
						if (objItem.Text.Equals(value, StringComparison.CurrentCultureIgnoreCase))
							SelectedItem = objItem;
				}
		}
	}
}
