using System;
using System.Collections.Generic;

namespace Bau.Controls.Tree
{
	/// <summary>
	///		Colecci�n de <see cref="TreeNodeKey"/>
	/// </summary>
	public class TreeNodeKeyCollection : List<TreeNodeKey>
	{
		/// <summary>
		///		A�ade un nodo a la colecci�n
		/// </summary>
		public void Add(int intIDType, int? intID)
		{ Add(new TreeNodeKey(intIDType, intID));
		}
		
		/// <summary>
		///		Busca un ID de un tipo determinado
		/// </summary>
		public int? SearchID(int intIDType)
		{ int? intID = null;
		
				// Recorre los elementos de la colecci�n buscando el ID
					foreach (TreeNodeKey objKey in this)
						if (intID == null)
							{	if (objKey.IDType == intIDType)
									intID = objKey.ID;
								else
									intID = objKey.SearchID(intIDType);
							}
				// Devuelve el ID localizado
					return intID;
		}

		/// <summary>
		///		A�ade una colecci�n a la colecci�n actual
		/// </summary>
		public void AddCollection(TreeNodeKey objKeyParent)
		{ // A�ade el padre
				Add(objKeyParent.IDType, objKeyParent.ID);
			// A�ade la colecci�n
				foreach (TreeNodeKey objKey in objKeyParent.KeysCollection)
					Add(objKey.IDType, objKeyParent.ID);
		}
	}
}
