using System;
using System.Collections.Generic;

namespace Bau.Controls.Tree
{
	/// <summary>
	///		Colección de <see cref="TreeNodeKey"/>
	/// </summary>
	public class TreeNodeKeyCollection : List<TreeNodeKey>
	{
		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public void Add(int intIDType, int? intID)
		{ Add(new TreeNodeKey(intIDType, intID));
		}
		
		/// <summary>
		///		Busca un ID de un tipo determinado
		/// </summary>
		public int? SearchID(int intIDType)
		{ int? intID = null;
		
				// Recorre los elementos de la colección buscando el ID
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
		///		Añade una colección a la colección actual
		/// </summary>
		public void AddCollection(TreeNodeKey objKeyParent)
		{ // Añade el padre
				Add(objKeyParent.IDType, objKeyParent.ID);
			// Añade la colección
				foreach (TreeNodeKey objKey in objKeyParent.KeysCollection)
					Add(objKey.IDType, objKeyParent.ID);
		}
	}
}
