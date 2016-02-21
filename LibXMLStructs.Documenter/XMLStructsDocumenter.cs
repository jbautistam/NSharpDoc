using System;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibXMLStructs.Documenter
{
	/// <summary>
	///		Clase para documentación de estructuras de documentación en archivos XML
	/// </summary>
	public class XMLStructsDocumenter : LibNSharpDoc.Models.Interfaces.IDocumenter
	{
		/// <summary>
		///		Interpreta la documentación de un archivo XML
		/// </summary>
		public StructDocumentationModel Parse(StructParameterModelDictionary objParameters)
		{ string strFileName = objParameters.GetValue("FileName");

				if (!strFileName.IsEmpty())
					return new XMLStructsRepository().Load(objParameters.GetValue("FileName"));
				else
					throw new NotImplementedException("No se encuentra el nombre de archivo");
		}
	}
}
