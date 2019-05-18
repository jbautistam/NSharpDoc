using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Bau.Controls.Combos
{
	/// <summary>
	///		Extensión del comboBox para que admita elemento con ID y Text
	/// </summary>
	public class ComboBoxExtended : ComboBox
	{
		public ComboBoxExtended()
		{ DropDownStyle = ComboBoxStyle.DropDownList;
		}
		
		/// <summary>
		///		Añade un elemento al combo
		/// </summary>
		public void AddItem(int? intID, string text)
		{ AddItem(new clsComboItem(intID, text));
		}
		
		/// <summary>
		///		Añade un elemento al combo
		/// </summary>
		public void AddItem(clsComboItem item)
		{ // Añade el elemento
				Items.Add(item);
			// Cambia el miembro a visualizar y el oculto
				DisplayMember = "Text";
				ValueMember = "ID";
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]		
		public int? SelectedID
		{ get
				{ if (SelectedItem == null)
						return null;
					else
						return ((clsComboItem) SelectedItem).ID;
				}
			set 
				{ foreach (clsComboItem item in Items)
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
						return ((clsComboItem) SelectedItem).Text;
				}
			set 
				{ foreach (clsComboItem item in Items)
						if (item.Text.Equals(value, StringComparison.CurrentCultureIgnoreCase))
							SelectedItem = item;
				}
		}
	}
}
