using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.NHaml
{
	/// <summary>
	///		Generador de un archivo NHaml para documentación
	/// </summary>
	public class NHamlWriter : IDocumentWriter
	{ 
		// Variables privadas
		private NHamlConversor conversor = new NHamlConversor();

		/// <summary>
		///		Graba la documentación Nhaml en un archivo
		/// </summary>
		public void Save(DocumentFileModel document, MLIntermedialBuilder builderML, string fileNameTemplate, string path)
		{
			Save(GetMLFile(document, ConvertMLNode(builderML)), System.IO.Path.Combine(path, document.GetPathLocal()));
		}

		/// <summary>
		///		Convierte una estructura XML en NHaml
		/// </summary>
		private string ConvertMLNode(MLIntermedialBuilder builderML)
		{
			return conversor.Convert(builderML);
		}

		/// <summary>
		///		Graba un archivo XML
		/// </summary>
		private void Save(MLFile fileML, string path)
		{
			string fileName = System.IO.Path.Combine(path, "Document.dcx");

				// Crea el directorio
				LibCommonHelper.Files.HelperFiles.MakePath(path);
				// Graba el archivo de documentación
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene el archivo XML asociado a un documento
		/// </summary>
		private MLFile GetMLFile(DocumentFileModel document, string strNHaml)
		{
			string description = document.Name;

				// Obtiene la descripción
				if (document.LanguageStruct != null)
					description = document.LanguageStruct.Name;
				// Obtiene el archivo
				return GetMLFile(document.Name, description, strNHaml);
		}

		/// <summary>
		///		Obtiene el archivo XML asociado a un documento
		/// </summary>
		private MLFile GetMLFile(string title, string description, string nhaml)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add("Document");

				// Añade el contenido
				rootML.Nodes.Add("Title", title);
				rootML.Nodes.Add("Description", description);
				rootML.Nodes.Add("Content", nhaml);
				rootML.Nodes.Add("ShowAtRss", false);
				rootML.Nodes.Add("Type", 2);
				rootML.Nodes.Add("Scope", 0);
				rootML.Nodes.Add("DateCreate", DateTime.Now);
				// Devuelve el archivo
				return fileML;
		}
	}
}
