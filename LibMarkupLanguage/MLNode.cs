using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Datos de un nodo
	/// </summary>
	public class MLNode : MLItemBase
	{
		public MLNode() : this(null, null) {}
		
		public MLNode(string strName) : this(strName, null) {}
		
		public MLNode(string strName, string strValue) : base(strName, strValue)
		{ Attributes = new MLAttributesCollection();
			Nodes = new MLNodesCollection();
			NameSpaces = new MLNameSpacesCollection();
		}
		
		/// <summary>
		///		Atributos
		/// </summary>
		public MLAttributesCollection Attributes { get; private set; }
		
		/// <summary>
		///		Nodos
		/// </summary>
		public MLNodesCollection Nodes { get; private set; }
		
		/// <summary>
		///		Espacios de nombres
		/// </summary>
		public MLNameSpacesCollection NameSpaces { get; private set; }
	}
}
