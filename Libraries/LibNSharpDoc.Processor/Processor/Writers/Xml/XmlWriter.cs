using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.Xml
{
	/// <summary>
	///		Generador de un archivo Xml para documentación
	/// </summary>
	public class XmlWriter : IDocumentWriter
	{
		/// <summary>
		///		Graba la documentación Nhaml en un archivo
		/// </summary>
		public void Save(DocumentFileModel document, MLIntermedialBuilder builderML, string fileNameTemplate, string path)
		{
			MLFile fileML = GetMLFile(document.Name, "", builderML.Root);

				// Graba el archivo
				Save(fileML, path, LibCommonHelper.Files.HelperFiles.Normalize(document.Name, false) + ".xml");
		}

		/// <summary>
		///		Graba un archivo XML
		/// </summary>
		private void Save(MLFile fileML, string path, string fileName)
		{   
			// Crea el directorio
			LibCommonHelper.Files.HelperFiles.MakePath(path);
			// Graba el archivo de documentación
			new LibMarkupLanguage.Services.XML.XMLWriter().Save(System.IO.Path.Combine(path, fileName), fileML);
		}

		/// <summary>
		///		Obtiene el archivo XML asociado a un documento
		/// </summary>
		private MLFile GetMLFile(string title, string description, MLNode nodeML)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add("Document");

				// Añade el contenido
				rootML.Nodes.Add("Title", title);
				rootML.Nodes.Add("Description", description);
				rootML.Nodes.Add("Content").Nodes.AddRange(nodeML.Nodes);
				rootML.Nodes.Add("DateCreate", DateTime.Now);
				// Devuelve el archivo
				return fileML;
		}
	}
}
