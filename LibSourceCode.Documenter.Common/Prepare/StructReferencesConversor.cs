using System;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;
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
		internal void ConvertReferences(StructDocumentationModelCollection objColStructs)
		{ Dictionary<string, StructDocumentationModel> dctStruct = new Dictionary<string, StructDocumentationModel>();
		
				// Añade las estructuras al diccionario
					FillDictionary(dctStruct, objColStructs);
				// Convierte las referencias
					ConvertReferences(dctStruct, objColStructs);
		}

		/// <summary>
		///		Rellena un diccionario con las estructuras de clase, interface...
		/// </summary>
		private void FillDictionary(Dictionary<string, StructDocumentationModel> dctStruct, StructDocumentationModelCollection objColStructs)
		{ foreach (StructDocumentationModel objStruct in objColStructs)
				{ // Añade la estructura al diccionario si es necesario
						if (MustAdd(objStruct) && !dctStruct.ContainsKey(NormalizeKey(objStruct.Name)))
							dctStruct.Add(NormalizeKey(objStruct.Name), objStruct);
					// Añade los elementos hijo
						FillDictionary(dctStruct, objStruct.Childs);
				}
		}

		/// <summary>
		///		Normaliza la clave de diccionario de una estructura
		/// </summary>
		private string NormalizeKey(string strName)
		{ return strName.TrimIgnoreNull().ToUpper();
		}

		/// <summary>
		///		Comprueba si se debe añadir una estructura al diccionario
		/// </summary>
		private bool MustAdd(StructDocumentationModel objStruct)
		{ return objStruct.Type.EqualsIgnoreCase("NameSpace") ||
						 objStruct.Type.EqualsIgnoreCase("Class") ||
						 objStruct.Type.EqualsIgnoreCase("Struct") ||
						 objStruct.Type.EqualsIgnoreCase("Enum") ||
						 objStruct.Type.EqualsIgnoreCase("Interface");
		}

		/// <summary>
		///		Convierte las referencias de las estructuras
		/// </summary>
		private void ConvertReferences(Dictionary<string, StructDocumentationModel> dctStruct, StructDocumentationModelCollection objColStructs)
		{ foreach (StructDocumentationModel objStruct in objColStructs)
				{ // Convierte las referencias de los parámetros 
						ConvertReferences(dctStruct, objStruct.Parameters);
					// convierte las referencias de las estructuras hija
						ConvertReferences(dctStruct, objStruct.Childs);
				}
		}

		/// <summary>
		///		Convierte las referencias de una colección de parámetros
		/// </summary>
		private void ConvertReferences(Dictionary<string, StructDocumentationModel> dctStruct, StructParameterModelDictionary objColParameters)
		{ foreach (KeyValuePair<string, StructParameterModel> objKeyPair in objColParameters.Parameters)
				{ // Convierte las referencias
						if (objKeyPair.Value != null && !objKeyPair.Value.ReferenceKey.IsEmpty())
							{ StructDocumentationModel objStruct = null;

									// Obtiene la estructura del diccionario
										if (dctStruct.TryGetValue(NormalizeKey(objKeyPair.Value.ReferenceKey), out objStruct))
											objKeyPair.Value.Reference = objStruct;
							}
					// Convierte los parámetros hijo
						ConvertReferences(dctStruct, objKeyPair.Value.Parameters);
				}
		}
	}
}
