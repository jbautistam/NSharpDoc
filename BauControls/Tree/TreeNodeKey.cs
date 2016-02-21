using System;

namespace Bau.Controls.Tree
{
	/// <summary>
	///		Clase con las posibles claves de un nodo
	/// </summary>
	public class TreeNodeKey
	{ 			
		public TreeNodeKey(int intIDType, int? intID) : this(intIDType, intID, null) {}
			
		public TreeNodeKey(int intIDType, int? intID, object objTag)
		{ IDType = intIDType;
			ID = intID;
			KeysCollection = new TreeNodeKeyCollection();
			Tag = objTag;
		}

		/// <summary>
		///		Busca un ID de un tipo determinado, si no lo encuentra devuelve el valor por defecto
		/// </summary>
		public int? SearchID(int intIDType)
		{ if (IDType == intIDType)
				return ID;
			else if (KeysCollection != null)
				return KeysCollection.SearchID(intIDType);
			else
				return null;
		}		
		
		public int IDType { get; set; }
		
		public int? ID { get; set; }
		
		public TreeNodeKeyCollection KeysCollection { get; set; }
		
		public object Tag { get; set; }
	}
}
