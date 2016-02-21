using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.Tree
{
	/// <summary>
	///		Extensión del treeView de Windows.Form
	/// </summary>
	public class TreeViewExtended : TreeView
	{ // Constantes
			private const string cnstStrTextEmptyChild = "#¬282%¬¬¬€212#";
		// Delegados
			public delegate void LoadChildsHandler(object objSender, TreeViewExtendedEventArgs evnArgs);
			public delegate void NodeDoubleClickHandler(object objSender, TreeViewExtendedEventArgs evnArgs);
			public delegate void NodeDeleteClickHandler(object objSender, TreeViewExtendedEventArgs evnArgs);
			public delegate void DropItemHandler(object objSender, TreeViewExtendedDropEventArgs evnDrop);
		// Eventos
			public event LoadChildsHandler LoadChilds;
			public event NodeDoubleClickHandler NodeDoubleClick;
			public event NodeDeleteClickHandler NodeDelete;
			public event DropItemHandler DropItem;
		// Enumerados
			private enum TreeEventType
				{ LoadChildsEvent,
					NodeDoubleClickEvent,
					NodeDelete
				}
		// Variables privadas
			private bool blnCheckRecursive = false, blnChecking = false;
			private TreeNodeKeyCollection objColNodesKeys = new TreeNodeKeyCollection();
			
		public TreeViewExtended()
		{ HotTracking = false;
			LabelEdit = false;
			ShowLines = true;
			ShowNodeToolTips = true;
			ShowPlusMinus = true;
			ShowRootLines = true;
		}

		/// <summary>
		///		Añade un nodo a un treeView
		/// </summary>
		public TreeNode AddNode(TreeNode trnParent, int intIDType, int? intID, string strText,
														bool blnWithChilds)
		{ return AddNode(trnParent, intIDType, intID, strText, blnWithChilds, -1, Color.Transparent, false);
		}

		/// <summary>
		///		Añade un nodo a un treeView
		/// </summary>
		public TreeNode AddNode(TreeNode trnParent, int intIDType, int? intID, 
														string strText, bool blnWithChilds, int intImageIndex)
		{ return AddNode(trnParent, intIDType, intID, strText, blnWithChilds, intImageIndex, Color.Transparent, false);
		}

		/// <summary>
		///		Añade un nodo a un treeView
		/// </summary>
		public TreeNode AddNode(TreeNode trnParent, int intIDType, int? intID, 
														string strText, bool blnWithChilds, int intImageIndex, Color clrNode)
		{ return AddNode(trnParent, intIDType, intID, strText, blnWithChilds, intImageIndex, clrNode, false);
		}

		/// <summary>
		///		Añade un nodo a un treeView
		/// </summary>
		public TreeNode AddNode(TreeNode trnParent, int intIDType, int? intID,  
														string strText, bool blnWithChilds, int intImageIndex, Color clrNode, bool blnBold)
		{ return AddNode(trnParent, new TreeNodeKey(intIDType, intID), strText, blnWithChilds, intImageIndex, clrNode, blnBold);
		}
		
		/// <summary>
		///		Añade un nodo a un treeView
		/// </summary>
		public TreeNode AddNode(TreeNode trnParent, TreeNodeKey objNodeKey, string strText,
														bool blnWithChilds)
		{ return AddNode(trnParent, objNodeKey, strText, blnWithChilds, -1, Color.Transparent, false);
		}
		
		/// <summary>
		///		Añade un nodo a un treeView
		/// </summary>
		public TreeNode AddNode(TreeNode trnParent, TreeNodeKey objNodeKey, string strText,
														bool blnWithChilds, int intImageIndex)
		{ return AddNode(trnParent, objNodeKey, strText, blnWithChilds, intImageIndex, Color.Transparent, false);
		}
		
		/// <summary>
		///		Añade un nodo a un treeView
		/// </summary>
		public TreeNode AddNode(TreeNode trnParent, TreeNodeKey objNodeKey, string strText,
														bool blnWithChilds, int intImageIndex, Color clrNode)
		{ return AddNode(trnParent, objNodeKey, strText, blnWithChilds, intImageIndex, clrNode, false);
		}
		
		/// <summary>
		///		Añade un nodo a un treeView
		/// </summary>
		public TreeNode AddNode(TreeNode trnParent, TreeNodeKey objNodeKey, string strText,
														bool blnWithChilds, int intImageIndex, Color clrNode, bool blnBold)
		{ TreeNode trnNode;
		
				// Añade el nodo al árbol
					if (trnParent == null)
						trnNode = Nodes.Add(objNodeKey.ID.ToString(), strText, intImageIndex, intImageIndex);
					else
						trnNode = trnParent.Nodes.Add(objNodeKey.ID.ToString(), strText, intImageIndex, intImageIndex);
				// Asigna las claves
					trnNode.Tag = objNodeKey;
				// Cambia la fuente del nodo
					if (blnBold)
						trnNode.NodeFont = new Font(this.Font, FontStyle.Bold);
				// Cambia el color del nodo
					if (clrNode != Color.Transparent)
						trnNode.ForeColor = clrNode;
				// Si puede tener hijos le añade un nodo vacío
					if (blnWithChilds)
						trnNode.Nodes.Add(cnstStrTextEmptyChild);
				// Devuelve el nodo
					return trnNode;
		}

		/// <summary>
		///		Comprueba si un nodo tiene una clave de tipo <see cref="TreeNodeKey"/>
		/// </summary>
		private bool IsNodeWithKey(TreeNode trnNode)
		{ return trnNode != null && trnNode.Tag != null && trnNode.Tag is TreeNodeKey;
		}		
		
		/// <summary>
		///		Comprueba si un nodo es de un tipo y con una clave
		/// </summary>
		private bool CompareNode(TreeNode trnNode, int intIDType, int? intID)
		{ if (IsNodeWithKey(trnNode))
				return ((trnNode.Tag as TreeNodeKey).IDType == intIDType &&
								(trnNode.Tag as TreeNodeKey).ID == intID);
			else
				return false;
		}
		
		/// <summary>
		///		Lanza un evento
		/// </summary>
		private void RaiseEvent(TreeEventType intType, TreeNode trnNode)
		{ if (IsNodeWithKey(trnNode))
				switch (intType)
					{ case TreeEventType.LoadChildsEvent:
								// Limpia los hijos del nodo
									trnNode.Nodes.Clear();
								// Carga los hijos
									if (LoadChilds != null)
										LoadChilds(this, new TreeViewExtendedEventArgs(trnNode, trnNode.Tag as TreeNodeKey));
							break;
						case TreeEventType.NodeDoubleClickEvent:
								if (NodeDoubleClick != null)
									NodeDoubleClick(this, new TreeViewExtendedEventArgs(trnNode, trnNode.Tag as TreeNodeKey));
							break;
						case TreeEventType.NodeDelete:
								if (NodeDelete != null)
									NodeDelete(this, new TreeViewExtendedEventArgs(trnNode, trnNode.Tag as TreeNodeKey));
							break;
					}
		}	
		
		/// <summary>
		///		Chequea los nodos hijo recursivamente
		/// </summary>
		private void CheckRecursiveNodesChild(TreeNode trnNode, bool blnChecked)
		{ // Chequea los nodos hijos recursivamente
				foreach (TreeNode trnChild in trnNode.Nodes)
					{ // Chequea el nodo
							trnChild.Checked = blnChecked;
						// Chequea recursivamente
							CheckRecursiveNodesChild(trnChild, blnChecked);
					}
		}
		
		/// <summary>
		///		Chequea los nodos padre recursivamente
		/// </summary>
		private void CheckRecursiveNodesParent(TreeNode trnNode)
		{ bool blnChildsSelected = true;
		
				if (trnNode.Parent != null)
					{	// Pasa al nodo padre
							trnNode = trnNode.Parent;
							// Chequea el nodo padre si todos los hijos están chequeados
							foreach (TreeNode trnChild in trnNode.Nodes)
								if (!trnChild.Checked)
									blnChildsSelected = false;
							trnNode.Checked = blnChildsSelected;
						// Chequea hacia arriba
							CheckRecursiveNodesParent(trnNode);
					}
		}
		
		/// <summary>
		///		Chequea un nodo
		/// </summary>
		public void CheckNode(int intIDType, int? intID, bool blnChecked)
		{ // Indica que está lanzando un proceso
				blnChecking = true;
			// Chequea los nodos
				CheckNodesRecursive(Nodes, intIDType, intID, blnChecked);
			// Indica que ha terminado el proceso de chequeo
				blnChecking = false;
		}

		/// <summary>
		///		Chequea recursivamente un nodo cuya clave coincida con el prefijo y el ID
		/// </summary>
		private void CheckNodesRecursive(TreeNodeCollection objColNodes, int intIDType, int? intID, bool blnChecked)
		{ foreach (TreeNode trnNode in objColNodes)
				{ // Si es el nodo adecuado, lo selecciona
						if (CompareNode(trnNode, intIDType, intID))
							trnNode.Checked = blnChecked;
					// Recorre los nodos hijos
						CheckNodesRecursive(trnNode.Nodes, intIDType, intID, blnChecked);
				}
		}

		/// <summary>
		///		Busca un nodo en el árbol
		/// </summary>
		public TreeNode SearchNode(int intIDType, int? intID)
		{ return SearchNodeRecursive(Nodes, intIDType, intID);
		}
		
		/// <summary>
		///		Busca recursivamente por los nodos del árbol
		/// </summary>
		private TreeNode SearchNodeRecursive(TreeNodeCollection objColNodes, int intIDType, int? intID)
		{ TreeNode trnNodeFound = null;
			
				// Recorre los nodos buscando el seleccionado
					foreach (TreeNode trnNode in objColNodes)
						if (trnNodeFound == null) // ... si aún no se ha encontrado nada
							{ // Si es el nodo buscado, lo devuelve
									if (CompareNode(trnNode, intIDType, intID))
										trnNodeFound = trnNode;
									else
										trnNodeFound = SearchNodeRecursive(trnNode.Nodes, intIDType, intID);
							}
				// Devuelve el nodo localizado
					return trnNodeFound;
		}

		/// <summary>
		///		Obtiene una colección con los nodos seleccionados
		/// </summary>
		public TreeNodeKeyCollection GetKeysCheckedNodes()
		{ TreeNodeKeyCollection objColNodesKeys = new TreeNodeKeyCollection();
		
				// Recorre el árbol buscando los nodos seleccionados
					foreach (TreeNode trnNode in Nodes)
						GetCheckedNodesRecursive(trnNode, objColNodesKeys);
				// Devuelve la colección de nodos
					return objColNodesKeys;
		}

		/// <summary>
		///		Obtiene una colección con los nodos seleccionados
		/// </summary>
		private void GetCheckedNodesRecursive(TreeNode trnNode, TreeNodeKeyCollection objColNodesKeys)
		{ // Si el nodo está seleccionado, lo añade a la colección
				if (IsNodeWithKey(trnNode) && trnNode.Checked)
					objColNodesKeys.Add(trnNode.Tag as TreeNodeKey);
			// Recorre los nodos hijos
				foreach (TreeNode trnChild in trnNode.Nodes)
					GetCheckedNodesRecursive(trnChild, objColNodesKeys);
		}
		
		/// <summary>
		///		Guarda los nodos abiertos
		/// </summary>
		public void SaveOpenNodes()
		{ // Limpia la colección
				objColNodesKeys.Clear();
			// Recorre los nodos almacenando los nodos abiertos
				SaveOpenNodes(Nodes);
		}
		
		/// <summary>
		///		Graba recursivamente los nodos abiertos
		/// </summary>
		private void SaveOpenNodes(TreeNodeCollection trnNodeCollection)
		{	foreach (TreeNode trnNode in trnNodeCollection)
				if (IsNodeWithKey(trnNode) && trnNode.IsExpanded)
					{	// Añade la clave del nodo a la colección
							objColNodesKeys.Add(trnNode.Tag as TreeNodeKey);
						// Graba los nodos hijos abiertos
							SaveOpenNodes(trnNode.Nodes);
					}
		}
		
		/// <summary>
		///		Recupera los nodos abiertos
		/// </summary>
		public void RestoreOpenNodes()
		{	TreeNode trnNode;
			
				foreach (TreeNodeKey objNodeKey in objColNodesKeys)
					if (objNodeKey != null)
						{	// Busca el nodo (puede que se haya eliminado el elemento que lo representa)
								trnNode = SearchNode(objNodeKey.IDType, objNodeKey.ID);
							// Lanza el evento de apertura de nodos
								if (trnNode != null && trnNode.Nodes.Count == 1 && trnNode.Nodes[0].Text == cnstStrTextEmptyChild)
									{	// Quita el nodo hijo falso
											trnNode.Nodes.Clear();
										// Lanza el evento de carga de hijos el nodo
											RaiseEvent(TreeEventType.LoadChildsEvent, trnNode);
									}
							// Abre el nodo
								if (trnNode != null)
									trnNode.Expand();
						}
		}
		
		/// <summary>
		///		Obtiene la clave de un nodo
		/// </summary>
		public TreeNodeKey GetKey(TreeNode trnNode)
		{ if (IsNodeWithKey(trnNode))
				return trnNode.Tag as TreeNodeKey;
			else
				return null;
		}
		
		/// <summary>
		///		Expande los nodos y selecciona el primer elemento
		/// </summary>
		public new void ExpandAll()
		{	// Expande los nodos
				base.ExpandAll();
			// Selecciona el primer elemento
				if (Nodes.Count > 0)
					TopNode = Nodes[0];
		}
		
		/// <summary>
		///		Sobrescribe el evento OnBeforeExpand
		/// </summary>
		protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
		{	// Si se debe llamar al evento de cargar nodos
				if (e.Node != null && e.Node.Nodes != null && e.Node.Nodes.Count == 1 && 
						e.Node.Nodes[0].Text == cnstStrTextEmptyChild)
					RaiseEvent(TreeEventType.LoadChildsEvent, e.Node);
			// Llama al evento base 
				base.OnBeforeExpand(e);
		}

		/// <summary>
		///		Sobrescribe el evento OnMouseDown
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e)
		{ TreeNode trnNode = GetNodeAt(e.X, e.Y);
						
				// Selecciona el nodo
					if (trnNode == null && e.Button == MouseButtons.Left)
						SelectedNode = null;
					else
						// Llama al evento base
							base.OnMouseDown(e);
		}
		
		/// <summary>
		///		Sobrescribe el evento OnNodeMouseDoubleClick
		/// </summary>
		protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
		{ // Lanza el evento
				if (e.Node != null)
					RaiseEvent(TreeEventType.NodeDoubleClickEvent, e.Node);
			// Llama al evento base
				base.OnNodeMouseDoubleClick(e);
		}

		/// <summary>
		///		Sobrescribe el evento OnKeyUp
		/// </summary>
		protected override void OnKeyUp(KeyEventArgs e)
		{ // Lanza el evento
				if (SelectedNode != null)
					{	if (e.KeyCode == Keys.Delete)
							RaiseEvent(TreeEventType.NodeDelete, SelectedNode);
						else if (SelectedNode != null && (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter))
							RaiseEvent(TreeEventType.NodeDoubleClickEvent, SelectedNode);
					}
			// Llama al evento base
				base.OnKeyUp(e);
		}

		/// <summary>
		///		Sobrescribe el evento OnAfterCheck
		/// </summary>
		protected override void OnAfterCheck(TreeViewEventArgs e)
		{ // Selecciona / deselecciona los nodos recursivamente
				if (e.Node != null && CheckRecursive && !blnChecking)
					{ // Indica que se está chequeando
							blnChecking = true;
						// Chequea recursivamente
							CheckRecursiveNodesChild(e.Node, e.Node.Checked);
							CheckRecursiveNodesParent(e.Node);
						// Indica que no se está chequeando
							blnChecking = false;
					}
			// Llama al evento base
				base.OnAfterCheck(e);
		}
		
		/// <summary>
		///		Sobrescribe el manejador de eventos OnItemDrag para cuando se empieza a arrastrar un nodo
		/// </summary>
		protected override void OnItemDrag(ItemDragEventArgs evnDrag)
		{ // Cambia el efecto de dragging
				DoDragDrop(evnDrag.Item, DragDropEffects.Link);
			// Llama al evento base
 				base.OnItemDrag(evnDrag);
		}
		
		/// <summary>
		///		Sobrescribe el manejador de eventos OnGiveFeedback para dar información al usuario mediante el cursor
		/// </summary>
		protected override void  OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
		{ // Cambia el cursor
				if ((gfbevent.Effect & DragDropEffects.Move) > 0)
					Cursor.Current = Cursors.Hand;
				else if ((gfbevent.Effect & DragDropEffects.Copy) > 0)
					Cursor.Current = Cursors.UpArrow;
				else if ((gfbevent.Effect & DragDropEffects.Scroll) > 0)
					Cursor.Current = Cursors.PanSouth;
				else
					Cursor.Current = Cursors.Default;
			// Llama al evento base
 				base.OnGiveFeedback(gfbevent);
		}

		/// <summary>
		///		Sobrescribe el manejador de eventos OnDragEnter para cuando se arrastrar un dato sobre el árbol
		/// </summary>
		protected override void OnDragEnter(DragEventArgs evnDrag)
		{ // Asigna el efecto al elemento que se ha desplazado sobre el árbol
				evnDrag.Effect = DragDropEffects.Move;
			// Llama al evento base
				base.OnDragEnter(evnDrag);
		}
		
		/// <summary>
		///		Sobrescribe el manejador de eventos OnDragEnter para cuando se arrastrar un dato sobre el árbol
		/// </summary>
		protected override void OnDragDrop(DragEventArgs evnDrag)
		{ TreeNode trnNode = GetNodeAt(evnDrag.X, evnDrag.Y);
						
				if (DropItem != null)
					DropItem(this, new TreeViewExtendedDropEventArgs(GetNodeAt(PointToClient(new Point(evnDrag.X, evnDrag.Y))),
																													 evnDrag.Data, evnDrag.KeyState));
			// Llama al evento base
				base.OnDragEnter(evnDrag);
		}

		[Browsable(true), Description("Chequea recursivamente los nodos del árbol")]
		public bool CheckRecursive
		{ get { return blnCheckRecursive; }
			set { blnCheckRecursive = value; }
		}
		
		[Browsable(false)]
		public TreeNodeKey SelectedKey
		{ get
				{ if (IsNodeWithKey(SelectedNode))
						return SelectedNode.Tag as TreeNodeKey;
					else
						return null;
				}
		}
		
		[Browsable(false)]
		public int? SelectedID
		{ get
				{ if (IsNodeWithKey(SelectedNode))
						return (SelectedNode.Tag as TreeNodeKey).ID;
					else
						return null;
				}
		}
		
		[Browsable(false)]
		public int SelectedIDType
		{ get
				{ if (IsNodeWithKey(SelectedNode))
						return (SelectedNode.Tag as TreeNodeKey).IDType;
					else
						return int.MinValue;
				}
		}
	}
}
