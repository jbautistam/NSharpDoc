using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibSourceCode.Documenter.Common.Prepare
{
	/// <summary>
	///		Clase de conversión para las referencias de las estructuras
	/// </summary>
	internal class StructReferencesConversor
	{
		/// <summary>
		///		Convierte las referencias de una serie de estructuras
		/// </summary>
		internal void ConvertReferences(StructDocumentationModelCollection structsDoc)
		{
			Dictionary<string, StructDocumentationModel> dctStruct = new Dictionary<string, StructDocumentationModel>();

				// Añade las estructuras al diccionario
				FillDictionary(dctStruct, structsDoc);
				// Convierte las referencias
				ConvertReferences(dctStruct, structsDoc);
		}

		/// <summary>
		///		Rellena un diccionario con las estructuras de clase, interface...
		/// </summary>
		private void FillDictionary(Dictionary<string, StructDocumentationModel> dctStruct, StructDocumentationModelCollection structsDoc)
		{
			foreach (StructDocumentationModel structDoc in structsDoc)
			{ 
				// Añade la estructura al diccionario si es necesario
				if (MustAdd(structDoc) && !dctStruct.ContainsKey(NormalizeKey(structDoc.Name)))
					dctStruct.Add(NormalizeKey(structDoc.Name), structDoc);
				// Añade los elementos hijo
				FillDictionary(dctStruct, structDoc.Childs);
			}
		}

		/// <summary>
		///		Normaliza la clave de diccionario de una estructura
		/// </summary>
		private string NormalizeKey(string name)
		{
			return name.TrimIgnoreNull().ToUpper();
		}

		/// <summary>
		///		Comprueba si se debe añadir una estructura al diccionario
		/// </summary>
		private bool MustAdd(StructDocumentationModel structDoc)
		{
			return structDoc.Type.EqualsIgnoreCase("NameSpace") ||
						structDoc.Type.EqualsIgnoreCase("Class") ||
						structDoc.Type.EqualsIgnoreCase("Struct") ||
						structDoc.Type.EqualsIgnoreCase("Enum") ||
						structDoc.Type.EqualsIgnoreCase("Interface");
		}

		/// <summary>
		///		Convierte las referencias de las estructuras
		/// </summary>
		private void ConvertReferences(Dictionary<string, StructDocumentationModel> dctStruct, StructDocumentationModelCollection structsDoc)
		{
			foreach (StructDocumentationModel structDoc in structsDoc)
			{ 
				// Convierte las referencias de los parámetros 
				ConvertReferences(dctStruct, structDoc.Parameters);
				// convierte las referencias de las estructuras hija
				ConvertReferences(dctStruct, structDoc.Childs);
			}
		}

		/// <summary>
		///		Convierte las referencias de una colección de parámetros
		/// </summary>
		private void ConvertReferences(Dictionary<string, StructDocumentationModel> dctStruct, StructParameterModelDictionary parameters)
		{
			foreach (KeyValuePair<string, StructParameterModel> keyPair in parameters.Parameters)
			{ // Convierte las referencias
				if (keyPair.Value != null && !keyPair.Value.ReferenceKey.IsEmpty())
				{
					StructDocumentationModel structDoc = null;

						// Obtiene la estructura del diccionario
						if (dctStruct.TryGetValue(NormalizeKey(keyPair.Value.ReferenceKey), out structDoc))
							keyPair.Value.Reference = structDoc;
				}
				// Convierte los parámetros hijo
				ConvertReferences(dctStruct, keyPair.Value.Parameters);
			}
		}
	}
}
