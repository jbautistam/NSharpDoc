using System;

using Bau.Libraries.LibCommonHelper.Extensors;
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
		public StructDocumentationModel Parse(StructParameterModelDictionary parameters)
		{
			string fileName = parameters.GetValue("FileName");

				if (!fileName.IsEmpty())
					return new XMLStructsRepository().Load(parameters.GetValue("FileName"));
				else
					throw new NotImplementedException("No se encuentra el nombre de archivo");
		}
	}
}
