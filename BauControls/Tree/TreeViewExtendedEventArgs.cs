using System;

namespace Bau.Controls.Tree
{
	/// <summary>
	///		Argumentos de los eventos de TreeViewExtended
	/// </summary>
	public class TreeViewExtendedEventArgs : EventArgs
	{ 	
		public TreeViewExtendedEventArgs(System.Windows.Forms.TreeNode trnNode, TreeNodeKey objTreeNodeKey)
		{ Node = trnNode;
			Keys = objTreeNodeKey;
		}
		
		/// <summary>
		///		Nodo al que se asocia el evento
		/// </summary>
		public System.Windows.Forms.TreeNode Node { get; set; }
		
		/// <summary>
		///		Claves del nodo
		/// </summary>
		public TreeNodeKey Keys { get; set; }
	}
}
