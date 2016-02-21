using System;
using System.Windows.Forms;

namespace Bau.Controls.Tree
{
	/// <summary>
	///		Argumentos del evento que se produce cuando se deja un elemento sobre un nodo de TreeViewExtended
	/// </summary>
	public class TreeViewExtendedDropEventArgs : EventArgs
	{ 
		public TreeViewExtendedDropEventArgs(TreeNode trnNode, IDataObject objDroppedItem, int intKeyState)
		{ Node = trnNode;
			DroppedItem = objDroppedItem;
			KeyState = intKeyState;
		}
		
		/// <summary>
		///		Nodo sobre el que se ha soltado el elemento (puede ser null)
		/// </summary>
		public TreeNode Node { get; set; }
		
		/// <summary>
		///		Elemento dejado sobre el nodo
		/// </summary>
		public IDataObject DroppedItem { get; set; }
		
		/// <summary>
		///		Estado del teclado (control, alt ...)
		/// </summary>
		public int KeyState { get; set; }
		
		/// <summary>
		///		Indica si es un movimiento de copia
		/// </summary>
		public bool IsCopy
		{ get { return (KeyState & 8) == 8; }
		}
	}
}
