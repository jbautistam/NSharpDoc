using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.List
{
	/// <summary>
	///		Listview con la barra de herramientas y el menú de nuevo/modificar/borrar
	/// </summary>
	public partial class ListUpdatable : UserControl
	{ // Delegados
			public delegate void UpdateRecordHandler(object objSender, string strKey);
			public delegate void DeleteRecordHandler(object objSender, string strKey);
			public delegate void MoveRecordHandler(object objSender, string strKey, bool blnUp);
		// Eventos
			public event UpdateRecordHandler OnUpdateRecord;
			public event DeleteRecordHandler OnDeleteRecord;
			public event MoveRecordHandler OnMoveRecord;
			public event EventHandler Changed;
			public event EventHandler SelectedIndexChanged;
		// Variables privadas
			private bool blnWithCheckBoxed = false, blnLabelEdit = false;
			private bool blnWithNew = true, blnWithUpdate = true, blnWithDelete = true;
			private bool blnWithUp = false, blnWithDown = false;
			private bool blnSortWithColumnClick = true, blnGroupWithColumnDoubleClick = true;
			
		public ListUpdatable()
		{	InitializeComponent();
			SortWithColumnClick = true;
			GroupWithColumnDoubleClick = true;
		}

		/// <summary>
		///		Limpia los datos del control
		/// </summary>
		public void Clear()
		{ lswItems.Clear();
		}
		
		/// <summary>
		///		Añade una columna
		/// </summary>
		public void AddColumn(int intWidth, string strText)
		{ AddColumn(intWidth, strText, Bau.Controls.List.ListViewExtended.ListViewColumnType.Text);
		}

		/// <summary>
		///		Añade una columna
		/// </summary>
		public void AddColumn(int intWidth, string strText, Bau.Controls.List.ListViewExtended.ListViewColumnType intColumnType)
		{ lswItems.AddColumn(intWidth, strText, intColumnType);
		}
		
		/// <summary>
		///		Añade un registro
		/// </summary>
		public ListViewItem AddRecord(string strKey, string strText)
		{ return lswItems.AddItem(strKey, strText);
		}		

		/// <summary>
		///		Agrupa por una columna
		/// </summary>
		public void GroupByColumn(int intColumn)
		{ lswItems.GroupByColumn(intColumn);
		}

		/// <summary>
		///		Selecciona un elemento por su ID
		/// </summary>
		public void SelectItem(string strKey)
		{ lswItems.SelectItem(strKey);
		}
		
		/// <summary>
		///		Lanza el evento de modificación
		/// </summary>
		private void RaiseEventChanged()
		{ if (Changed != null)
				Changed(this, EventArgs.Empty);
		}
		
		[Browsable(true), DefaultValue(false)]
		public bool CheckBoxes
		{ get { return blnWithCheckBoxed; }
			set 
				{ blnWithCheckBoxed = value; 
					lswItems.CheckBoxes = value;
				}
		}
		
		[Browsable(true), DefaultValue(true)]
		public bool WithNew
		{ get { return blnWithNew; }
			set 
				{ blnWithNew = value;
					cmdNew.Visible = blnWithNew; 
					mnuNew.Visible = blnWithNew;
				}
		}
		
		[Browsable(true), DefaultValue(true)]
		public bool WithUpdate
		{ get { return blnWithUpdate; }
			set 
				{ blnWithUpdate = value;
					cmdUpdate.Visible = blnWithUpdate;
					mnuUpdate.Visible = blnWithUpdate;
				}
		}
		
		[Browsable(true), DefaultValue(true)]
		public bool WithDelete
		{ get { return blnWithDelete; }
			set
				{ blnWithDelete = value;
					cmdDelete.Visible = blnWithDelete;
					mnuDelete.Visible = blnWithDelete; 
				}
		}
		
		[Browsable(true), DefaultValue(false)]
		public bool WithUp
		{ get { return blnWithUp; }
			set
				{ blnWithUp = value;
					cmdUp.Visible = blnWithUp;
				}
		}
		
		[Browsable(true), DefaultValue(false)]
		public bool WithDown
		{ get { return blnWithDown; }
			set
				{ blnWithDown = value;
					cmdDown.Visible = blnWithDown;
				}
		}
		
		[Browsable(false)]
		public int Count
		{ get { return lswItems.Items.Count; }
		}

		[Browsable(false)]
		public ListView.ColumnHeaderCollection Columns
		{ get { return lswItems.Columns; }
		}
		
		[Browsable(true)]
		public ImageList Images
		{ get { return imlImages; }
		}		
		
		/// <summary>
		///		Elementos de la lista
		/// </summary>
		[Browsable(false)]
		public ListView.ListViewItemCollection Items
		{ get { return lswItems.Items; }
		}
		
		/// <summary>
		///		Elementos seleccionados
		/// </summary>
		[Browsable(false)]
		public ListView.SelectedListViewItemCollection SelectedItems
		{ get { return lswItems.SelectedItems; }
		}
		
		/// <summary>
		///		Indica si se pueden editar las etiquetas
		/// </summary>
		[Browsable(true), DefaultValue(false)]
		public bool LabelEdit
		{ get { return blnLabelEdit; }
			set
				{ blnLabelEdit = value;
					lswItems.LabelEdit = blnLabelEdit;
				}
		}
		
		/// <summary>
		///		Lista de imágenes
		/// </summary>
		[Browsable(true)]
		public ImageList ImageList
		{ get { return lswItems.SmallImageList; }
			set { lswItems.SmallImageList = value; }
		}

		/// <summary>
		///		Indica si se pueden seleccionar varios elementos
		/// </summary>
		[Browsable(true)]
		public bool MultiSelect
		{ get { return lswItems.MultiSelect; }
			set { lswItems.MultiSelect = value; }
		}

		/// <summary>
		///		Indica si se deben mostrar los toolTip de los elementos
		/// </summary>
		[Browsable(true)]
		public bool ShowItemToolTips
		{ get { return lswItems.ShowItemToolTips; }
			set { lswItems.ShowItemToolTips = value; }
		}

		/// <summary>
		///		Indica si se deben ordenar los datos al pulsar sobre la columna
		/// </summary>
		[Browsable(true), DefaultValue(true)]
		public bool SortWithColumnClick
		{ get { return blnSortWithColumnClick; }
			set
				{ blnSortWithColumnClick = value;
					lswItems.SortWithColumnClick = value;
				}
		}

		/// <summary>
		///		Indica si se deben agrupar los datos al pulsar sobre la lista
		/// </summary>
		[Browsable(true), DefaultValue(true)]
		public bool GroupWithColumnDoubleClick
		{ get { return blnGroupWithColumnDoubleClick; }
			set
				{ blnGroupWithColumnDoubleClick = value;
					lswItems.GroupWithColumnDoubleClick = value;
				}
		}

		private void cmdNew_Click(object sender, EventArgs e)
		{ if (OnUpdateRecord != null)
				OnUpdateRecord(this, null);
			RaiseEventChanged();
		}

		private void cmdUpdate_Click(object sender, EventArgs e)
		{ if (OnUpdateRecord != null && !string.IsNullOrEmpty(lswItems.SelectedKey))
				OnUpdateRecord(this, lswItems.SelectedKey);
			RaiseEventChanged();
		}

		private void cmdDelete_Click(object sender, EventArgs e)
		{ if (OnDeleteRecord != null && !string.IsNullOrEmpty(lswItems.SelectedKey))
				OnDeleteRecord(this, lswItems.SelectedKey);
			RaiseEventChanged();
		}

		private void lswItems_ItemDoubleClick(object objSender, string strKey)
		{ if (OnUpdateRecord != null && WithUpdate && !string.IsNullOrEmpty(strKey))
				OnUpdateRecord(this, strKey);
		}

		private void cmdUp_Click(object sender, EventArgs e)
		{ if (OnMoveRecord != null && WithUp && !string.IsNullOrEmpty(lswItems.SelectedKey))
				OnMoveRecord(this, lswItems.SelectedKey, true);
			RaiseEventChanged();
		}

		private void cmdDown_Click(object sender, EventArgs e)
		{ if (OnMoveRecord != null && WithDown && !string.IsNullOrEmpty(lswItems.SelectedKey))
				OnMoveRecord(this, lswItems.SelectedKey, false);
			RaiseEventChanged();
		}

		private void lswItems_ItemChecked(object sender, ItemCheckedEventArgs e)
		{ RaiseEventChanged();
		}

		private void lswItems_SelectedIndexChanged(object sender, EventArgs e)
		{ if (SelectedIndexChanged != null)
				SelectedIndexChanged(this, e);
		}
	}
}
