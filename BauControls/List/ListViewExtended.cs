using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Bau.Controls.List
{ /// <summary>
	///		Control de extensión del listView
	/// </summary>
	public class ListViewExtended : ListView
	{ // Enumerados públicos
			public enum ListViewColumnType
				{	Text,
					Number,
					Date
				}
			public enum ColumnHeaderAdjust
				{ Header,
					Text
				}
		// Delegados
			public delegate void ItemDoubleClickHandler(object objSender, string strKey);
		// Eventos
			public event ItemDoubleClickHandler ItemDoubleClick;
			public event EventHandler ItemDeleteRequired;
		// Variables privadas
			private int intLastSortColumn = -1;
			
		public ListViewExtended()
		{ FullRowSelect = true;
			HideSelection = false;
			HotTracking = false;
			HoverSelection = false;
			LabelEdit = false;
			ShowGroups = false;
			View = View.Details;
		}
		
		/// <summary>
		///		Añade la columna ajustando al mayor elemento de texto
		/// </summary>
		public void AddColumn(string strText)
		{ AddColumn(-2, strText, ListViewColumnType.Text);
		}
		
		/// <summary>
		///		Añade la columna ajustando al mayor elemento de texto
		/// </summary>
		public void AddColumn(string strText, ListViewColumnType intColumnType)
		{ AddColumn(-2, strText, intColumnType);
		}
		
		/// <summary>
		///		Añade una columna
		/// </summary>
		public void AddColumn(int intWidth, string strText)
		{ AddColumn(intWidth, strText, ListViewColumnType.Text);
		}
		
		/// <summary>
		///		Añade una columna
		/// </summary>
		public void AddColumn(int intWidth, string strText, ListViewColumnType intColumnType)
		{ ColumnHeader objColumn = Columns.Add(strText, intWidth);
		
				objColumn.Tag = (int) intColumnType;
				if (intColumnType == ListViewColumnType.Number)
					objColumn.TextAlign = HorizontalAlignment.Right;
		}
		
		/// <summary>
		///		Añade un elemento
		/// </summary>
		public ListViewItem AddItem(long lngID, string strText)
		{ return AddItem(lngID.ToString(), strText, 1);
		}
		
		/// <summary>
		///		Añade un elemento
		/// </summary>
		public ListViewItem AddItem(string strKey, string strText)
		{ return AddItem(strKey, strText, 1);
		}
		
		/// <summary>
		///		Añade un elemento
		/// </summary>
		public ListViewItem AddItem(string strKey, string strText, int intImage)
		{ ListViewItem lsiItem = Items.Add(strKey, strText, intImage);
		
				// Asigna la clave al Tag del elemento
					lsiItem.Tag = strKey;
				// Devuelve el elemento
					return lsiItem;
		}

		/// <summary>
		///		Chequea un elemento
		/// </summary>
		public void CheckItem(string strKey, bool blnChecked)
		{ ListViewItem lsiItem = Items[strKey];
		
				if (lsiItem != null)
					lsiItem.Checked = blnChecked;
		}
		
		/// <summary>
		///		Chequea todos los elementos
		/// </summary>
		public void CheckAllItems(bool blnChecked)
		{ foreach (ListViewItem lsiItem in Items)
				lsiItem.Checked = blnChecked;
		}

		/// <summary>
		///		Selecciona un elemento por su clave
		/// </summary>
		public void SelectItem(string strKey)
		{ ListViewItem lsiItem = Items[strKey];
		
				// Borra los elementos seleccionados
					if (SelectedItems != null)
						SelectedItems.Clear();
				// Selecciona el elemento
					if (lsiItem != null)
						lsiItem.Selected = true;
		}

		/// <summary>
		///		Agrupa por una columna
		/// </summary>
		public void GroupByColumn(int intColumn)
		{ if (intColumn >= 0 && intColumn < Columns.Count)
				BuildGroups(intColumn);
			else
				ShowGroups = false;
		}
		
		/// <summary>
		///		Genera los grupos en el listView
		/// </summary>
		private void BuildGroups(int intGroupColumn)
		{	// Limpia los grupos
				Groups.Clear();
			// Obtiene el número de elementos para forzar que se realice cualquier cambio pendiente en la caché interna del ListView
			// Sin esto iterar sobre Items puede que no funcione correctamente si aún no se ha creado el handle del ListView
				int intDummy = Items.Count;

			// Separa los elementos de la lista en grupos utilizando la clave del grupo como descripción
			Dictionary<String, List<ListViewItem>> dctMap = new Dictionary<String, List<ListViewItem>>();
			
				foreach (ListViewItem lsiItem in Items) 
					{	string strKey = lsiItem.SubItems[intGroupColumn].Text;
					
							if (intGroupColumn == 0 && strKey.Length > 0 && GroupByFirstCharacter)
								strKey = strKey.Substring(0, 1);
							if (!dctMap.ContainsKey(strKey))
								dctMap[strKey] = new List<ListViewItem>();
							dctMap[strKey].Add(lsiItem);
					}
			// Crea una lista con los grupos necesarios
				List<ListViewGroup> lstGroups = new List<ListViewGroup>();
				
					foreach (string strKey in dctMap.Keys)
						lstGroups.Add(new ListViewGroup(strKey));
			// Ordena los grupos
				lstGroups.Sort(new clsListViewGroupSorter(Sorting));
			// Añade los grupos a la lista y dentro de cada grupo sus miembros
			// El orden es importante:
			// - la cabecera debe calculares antes de añadir el grupo a la lista, en otro caso, cambiar la cabecera
			//   provoca un redibujo desagradable (incluso aunque esté dentro de un bloque BeginUpdate...EndUpdate)
			// - el grupo se debe añadir antes que sus elementos, en otro caso se lanza una excepción
				clsListViewSorter objSorter = GetListSorter(intGroupColumn, Sorting);
				
					foreach (ListViewGroup lsgGroup in lstGroups) 
						{	Groups.Add(lsgGroup);
							dctMap[lsgGroup.Header].Sort(objSorter);
							lsgGroup.Items.AddRange(dctMap[lsgGroup.Header].ToArray());
						}
			// Muestra los grupos
				ShowGroups = true;
		}		
		
		/// <summary>
		///		Obtiene la clase que se utiliza para realizar la ordenación
		/// </summary>
		private clsListViewSorter GetListSorter(int intColumn, SortOrder intSortOrder)
		{ clsListViewSorter objSorter = new clsListViewSorter(intColumn, ListViewColumnType.Text, Sorting);
			int intColumnType;
		
				// Obtiene la ordenación dependiendo del tipo de columna
					if (Columns[intColumn].Tag != null && int.TryParse(Columns[intColumn].Tag.ToString(), out intColumnType))
						switch (intColumnType)
							{ case (int) ListViewColumnType.Date:
										objSorter = new clsListViewSorter(intColumn, ListViewColumnType.Date, Sorting);
									break;
								case (int) ListViewColumnType.Number:
										objSorter = new clsListViewSorter(intColumn, ListViewColumnType.Number, Sorting);
									break;
							}
				// Devuelve el objeto de ordenación
					return objSorter;
		}
		
		/// <summary>
		///		Sobrescribe el evento OnColumnClick
		/// </summary>
		protected override void OnColumnClick(ColumnClickEventArgs e)
		{ if (SortWithColumnClick || ShowGroups)
				{	// Obtiene la ordenación
						if (e.Column == intLastSortColumn)
							switch (Sorting)
								{ case SortOrder.Descending:
									case SortOrder.None:
											Sorting = SortOrder.Ascending;
										break;
									case SortOrder.Ascending:
											Sorting = SortOrder.Descending;
										break;
								}
					// Cambia la agrupación / ordenación
						if (ShowGroups)
							BuildGroups(e.Column);
						else
							ListViewItemSorter = GetListSorter(e.Column, Sorting);
					// Guarda la columna sobre la que se está ordenando
						intLastSortColumn = e.Column;
				}
			// Lanza el evento al base
				base.OnColumnClick(e);
		}

		/// <summary>
		///		Sobrescribe el evento OnMouseDown
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e)
		{ // Si se debe agrupar ...
				if (e.Button == MouseButtons.Left && e.Clicks == 2 && 
						(FocusedItem == null || (FocusedItem != null && !FocusedItem.Selected)) && GroupWithColumnDoubleClick)
				  { // Muestra / oculta los grupos
				      ShowGroups = !ShowGroups;
				    // Agrupa por una columna
				      if (ShowGroups)
				        GroupByColumn(0);
				  }
			// Lanza el evento base
				base.OnMouseDown(e);
		}
		
		/// <summary>
		///		Sobrescribe el evento OnMouseDoubleClick
		/// </summary>
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{ ListViewItem lsiItem = FocusedItem;
		
				// Si se ha encontrado algún elemento, lanza el evento
					if (lsiItem != null && ItemDoubleClick != null && lsiItem.Tag is string)
						ItemDoubleClick(this, (string) lsiItem.Tag);
				// Lanza el evento base
					base.OnMouseDoubleClick(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{ // Comprueba si se debe borrar
				if (SelectedItems != null && SelectedItems.Count > 0)
					{ if ((e.KeyData == Keys.Delete || e.KeyData == Keys.Back) && ItemDeleteRequired != null)
							ItemDeleteRequired(this, EventArgs.Empty);
					}
			// Lanza el evento base
				base.OnKeyDown(e);
		}
		/// <summary>
		///		Obtiene la clave del elemento seleccionado
		/// </summary>
		[Browsable(false)]
		public string SelectedKey
		{ get
				{ if (SelectedItems.Count > 0)
						return (string) SelectedItems[0].Tag;
					return null;
				}
			set
				{ ListViewItem lsiItem = Items[value];
				
						if (lsiItem != null)
							lsiItem.Selected = true;
				}
		}
		
		[Browsable(true), DefaultValue(true), Description("Indica si se ordena cuando se pulse sobre una columna")]
		public bool SortWithColumnClick { get; set; } 
		
		[Browsable(true), DefaultValue(false), Description("Indica si se agrupa")]
		public new bool ShowGroups
		{ get { return base.ShowGroups; }
			set
				{ if (base.ShowGroups != value) // ... evita las llamadas recursivas
						{	// Cambia el valor de la base
								base.ShowGroups = value;
							// Agrupa los elementos de la lista
								if (value)
									BuildGroups(0);
						}
				}
		}	
	
		[Browsable(true), DefaultValue(true), Description("Indica si se agrupa cuando se pulse dos veces sobre una columna")]
		public bool GroupWithColumnDoubleClick { get; set; }

		/// <summary>
		///		Indica si se debe agrupar la primera columna por el primer carácter
		/// </summary>
		[Browsable(true), DefaultValue(false), Description("Indica si se debe agrupar la primera columna por el primer carácter")]
		public bool GroupByFirstCharacter { get; set; }
	}
}