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
		public void AddItem(int? intID, string strText)
		{ AddItem(new clsComboItem(intID, strText));
		}
		
		/// <summary>
		///		Añade un elemento al combo
		/// </summary>
		public void AddItem(clsComboItem objItem)
		{ // Añade el elemento
				Items.Add(objItem);
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
				{ foreach (clsComboItem objItem in Items)
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
						return ((clsComboItem) SelectedItem).Text;
				}
			set 
				{ foreach (clsComboItem objItem in Items)
						if (objItem.Text.Equals(value, StringComparison.CurrentCultureIgnoreCase))
							SelectedItem = objItem;
				}
		}
	}
}
