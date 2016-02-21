using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Colección de <see cref="MLNode"/>
	/// </summary>
	public class MLNodesCollection : MLItemsBaseCollection<MLNode>
	{
		/// <summary>
		///		Añade una serie de nodos a un nodo
		/// </summary>
		public void Add(string strName, MLNodesCollection objColMLNodes)
		{ MLNode objMLNode = Add(strName);
		
				// Añade los nodos
					objMLNode.Nodes.AddRange(objColMLNodes);
		}
	}
}
