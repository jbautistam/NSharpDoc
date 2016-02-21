using System;

namespace Bau.Libraries.LibNSharpDoc.Models.Structs
{
	/// <summary>
	///		Modelo con los datos de una estructura a documentar
	/// </summary>
	public class StructDocumentationModel
	{ // Enumerados públicos
			/// <summary>
			///		Tipo de ámbito
			/// </summary>
			public enum ScopeType
				{ 
					/// <summary>Desconocido</summary>
					Unknown,
					/// <summary>Público</summary>
					Public,
					/// <summary>Privado</summary>
					Private,
					/// <summary>Protegido</summary>
					Protected,
					/// <summary>Interno</summary>
					Internal,
					/// <summary>Ambito global para aquella documentación (como la de índice) que siempre debe salir)</summary>
					Global
				}

		public StructDocumentationModel(StructDocumentationModel objParent, ScopeType intScope, string strName, string strType, int intOrder)
		{ Parent = objParent;
			Scope = intScope;
			Name = strName;
			Type = strType;
			Order = intOrder;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		internal string Debug(int intIndent)
		{ string strDebug = new string('\t', intIndent);

				// Crea la cadena de depuración
					strDebug += $"Name: {Name} - Scope: {Scope} Type: {Type} Order: {Order}" + Environment.NewLine;
				// Añade la cadena de depuración de los parámetros
					strDebug += Parameters.Debug(intIndent);
				// Añade la cadena de depuración de los hijos
					strDebug += Childs.Debug(intIndent + 1);
				// Devuelve la cadena de depuración
					return strDebug + Environment.NewLine;
		}

		/// <summary>
		///		Estructura padre
		/// </summary>
		public StructDocumentationModel Parent { get; }

		/// <summary>
		///		Ambito del elemento
		/// </summary>
		public ScopeType Scope { get; set; }

		/// <summary>
		///		Nombre
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Tipo
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		///		Orden
		/// </summary>
		public int Order { get; set; }

		/// <summary>
		///		Elementos hijo
		/// </summary>
		public StructDocumentationModelCollection Childs { get; } = new StructDocumentationModelCollection();

		/// <summary>
		///		Parámetros
		/// </summary>
		public StructParameterModelDictionary Parameters { get; } = new StructParameterModelDictionary();
	}
}
