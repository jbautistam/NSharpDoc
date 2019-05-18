using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Templates;

namespace Bau.Libraries.LibNSharpDoc.Processor.Repository.Templates
{
	/// <summary>
	///		Repository de <see cref="TemplateModel"/>
	/// </summary>
	internal class TemplateRepository
	{
		/// <summary>
		///		Carga el archivo de definición de plantillas
		/// </summary>
		internal TemplateModelCollection Load(string pathTemplates)
		{
			TemplateModelCollection templates = new TemplateModelCollection(pathTemplates);
			string fileName = System.IO.Path.Combine(pathTemplates, "Templates.tpt");

				// Carga las plantillas
				if (System.IO.File.Exists(fileName))
				{
					MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

						foreach (MLNode rootML in fileML.Nodes)
							if (rootML.Name == "Templates")
								foreach (MLNode templateML in rootML.Nodes)
									if (templateML.Name == "Page" || templateML.Name == "Index")
									{
										TemplateModel.TemplateType type = TemplateModel.TemplateType.Page;
										string name = templateML.Attributes["Name"].Value;
										string structType = templateML.Attributes["StructType"].Value;
										string relativeFileName = templateML.Attributes["File"].Value;
										string rootTemplate = templateML.Attributes["RootTemplate"].Value;

											// Asigna el tipo de plantilla de los índices
											if (templateML.Name == "Index")
												type = TemplateModel.TemplateType.Index;
											// Devuelve el tipo de plantilla
											if ((type == TemplateModel.TemplateType.Index || !structType.IsEmpty()) && !relativeFileName.IsEmpty())
												templates.Add(type, name, structType, relativeFileName, rootTemplate);
									}
				}
				// Devuelve la colección de plantillas
				return templates;
		}

		/// <summary>
		///		Carga el texto de una plantilla raíz
		/// </summary>
		internal string LoadTextRootTemplate(string fileName)
		{
			string text = string.Empty;

				// Carga los datos de la plantilla
				if (System.IO.File.Exists(fileName))
					try
					{
						MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

							foreach (MLNode nodeML in fileML.Nodes)
								if (nodeML.Name == "Page")
									text = nodeML.Value;
					}
					catch { }
				// Devuelve el texto
				return text;
		}
	}
}
