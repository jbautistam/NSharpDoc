using System;

using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers
{
	/// <summary>
	///		Interface para las clases de grabación de documentación
	/// </summary>
	public interface IDocumentWriter
	{
		/// <summary>
		///		Graba la documentación en un archivo
		/// </summary>
		void Save(DocumentFileModel document, MLIntermedialBuilder builderML, string fileNameTemplate, string path);
	}
}
