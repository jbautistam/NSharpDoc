using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Bau.Controls.Combos
{
	/// <summary>
	///		Extensi�n del comboBox para que admita elemento con ID de tipo cadena y Text
	/// </summary>
	public class ComboBoxExtendedString : ComboBox
	{
		public ComboBoxExtendedString()
		{ DropDownStyle = ComboBoxStyle.DropDownList;
		}
		
		/// <summary>
		///		A�ade un elemento al combo
		/// </summary>
		public void AddItem(string id, string text)
		{ AddItem(new clsComboItemString(id, text));
		}
		
		/// <summary>
		///		A�ade un elemento al combo
		/// </summary>
		public void AddItem(clsComboItemString item)
		{ // A�ade el elemento
				Items.Add(item);
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
				{ foreach (clsComboItemString item in Items)
						if (item.ID == value)
							SelectedItem = item;
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
				{ foreach (clsComboItemString item in Items)
						if (item.Text.Equals(value, StringComparison.CurrentCultureIgnoreCase))
							SelectedItem = item;
				}
		}
	}
}
