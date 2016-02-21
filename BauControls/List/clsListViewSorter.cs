using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Bau.Controls.List
{
	/// <summary>
	///		Clase para ordenación de elementos de un ListView
	/// </summary>
	internal class clsListViewSorter : IComparer, IComparer<ListViewItem>
	{	// Variables privadas
			private bool blnIgnoreCase = true;
			private SortOrder intSortOrder;
			private ListViewExtended.ListViewColumnType intColumnType;
			private int intColumnNumber;

    public clsListViewSorter(int intColumnNumber, ListViewExtended.ListViewColumnType intColumnType, 
														 SortOrder intSortOrder) : this(intColumnNumber, intColumnType, intSortOrder, true) 
		{ 
		}

    public clsListViewSorter(int intColumnNumber, ListViewExtended.ListViewColumnType intColumnType, SortOrder intSortOrder,
														 bool blnIgnoreCase) 
    { ColumnNumber = intColumnNumber;
			ColumnType = intColumnType;
			SortOrder = intSortOrder;
			IgnoreCase = blnIgnoreCase;
		}

		/// <summary>
		///		Implementa al método Compare de IComparer(ListViewItem)
		/// </summary>
		public int Compare(object objValue1, object objValue2)
		{ return Compare((ListViewItem) objValue1, (ListViewItem) objValue2);
		}
		
		/// <summary>
		///		Implementa el método Compare de IComparer
		/// </summary>
    public int Compare(ListViewItem lsiItem1, ListViewItem lsiItem2)
    { string strValue1, strValue2;
			int intValueCompare = 0;
				// Obtiene las cadenas del listView
					if (intColumnNumber == 0)
						{	strValue1 = lsiItem1.Text;
							strValue2 = lsiItem2.Text;
						}
					else
						{	if (lsiItem1.SubItems.Count > intColumnNumber) // ... aseguramos que existe la columna
								strValue1 = lsiItem1.SubItems[intColumnNumber].Text;
							else
								strValue1 = "";
							if (lsiItem2.SubItems.Count > intColumnNumber) // ... aseguramos que existe la columna
								strValue2 = lsiItem2.SubItems[intColumnNumber].Text;
							else
								strValue2 = "";
						}
				// Obtiene el valor de comparación
					switch (intColumnType)
						{ case ListViewExtended.ListViewColumnType.Date:
									intValueCompare = ConvertToDateTime(strValue1).CompareTo(ConvertToDateTime(strValue2));
								break;
							case ListViewExtended.ListViewColumnType.Number:
									intValueCompare = ConvertToDouble(strValue1).CompareTo(ConvertToDouble(strValue2));
								break;
							case ListViewExtended.ListViewColumnType.Text:
									intValueCompare = strValue1.CompareTo(strValue2);
								break;
						}
				// Devuelve el valor de comparación
					return intValueCompare * GetSortFactor();
		}

		/// <summary>
		///		Convierte una cadena a un valor double
		/// </summary>
		private double ConvertToDouble(string strValue)
		{ double dblValue;
		
				if (!double.TryParse(strValue, out dblValue))
					return double.MinValue;
				else
					return dblValue;
		}

		/// <summary>
		///		Convierte una cadena a un valor DateTime
		/// </summary>
		private DateTime ConvertToDateTime(string strValue)
		{ DateTime dtmValue;
		
				if (!DateTime.TryParse(strValue, out dtmValue))
					return new DateTime(1900, 1, 1);
				else
					return dtmValue;
		}
		
		/// <summary>
		///		Obtiene el factor de ordenación
		/// </summary>
		private int GetSortFactor()
		{ int intSortVector = 0;
		
				// Obtiene el valor de ordenación
					switch (SortOrder)
						{	case SortOrder.Ascending:
									intSortVector = 1;
								break;
							case SortOrder.Descending:
									intSortVector = -1;
								break;
						}
				// Devuelve el valor de ordenación
					return intSortVector;
		}
		
    public int ColumnNumber
    { get { return intColumnNumber; }
			set { intColumnNumber = value; }
		}

    public ListViewExtended.ListViewColumnType ColumnType
    { get { return intColumnType; }
			set { intColumnType = value; }
		}

    public bool IgnoreCase
    { get { return blnIgnoreCase; }
			set { blnIgnoreCase = value; }
		}

    public SortOrder SortOrder
    { get { return intSortOrder; }
			set { intSortOrder = value; }
		}
	}
}
