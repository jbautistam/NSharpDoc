using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bau.Controls.List
{
	/// <summary>
	///		Clase para comparación de grupos en el listView
	/// </summary>
	internal class clsListViewGroupSorter : IComparer<ListViewGroup>
	{	private SortOrder intSortOrder;
	
		public clsListViewGroupSorter(SortOrder intOrder)
		{	intSortOrder = intOrder;
		}

		/// <summary>
		///		Compara dos grupos
		/// </summary>
		public int Compare(ListViewGroup lswGroupX, ListViewGroup lswGroupY)
		{	int intResult = String.Compare(lswGroupX.Header, lswGroupY.Header, true);

				if (intSortOrder == SortOrder.Descending)
					intResult = 0 - intResult;
				return intResult;
		}
	}
}