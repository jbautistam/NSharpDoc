using System;

using Bau.Libraries.LibHelper.Extensors;
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
		internal TemplateModelCollection Load(string strPathTemplates)
		{ TemplateModelCollection objColTemplates = new TemplateModelCollection(strPathTemplates);
			string strFileName = System.IO.Path.Combine(strPathTemplates, "Templates.tpt");

				// Carga las plantillas
					if (System.IO.File.Exists(strFileName))
						{ MLFile objMLFile = new LibMarkupLanguage.Services.XML.XMLParser().Load(strFileName);

								foreach (MLNode objMLRoot in objMLFile.Nodes)
									if (objMLRoot.Name == "Templates")
										foreach (MLNode objMLTemplate in objMLRoot.Nodes)
											if (objMLTemplate.Name == "Page" || objMLTemplate.Name == "Index")
												{ TemplateModel.TemplateType intIDType = TemplateModel.TemplateType.Page;
													string strName = objMLTemplate.Attributes["Name"].Value;
													string strStructType = objMLTemplate.Attributes["StructType"].Value;
													string strRelativeFileName = objMLTemplate.Attributes["File"].Value;
													string strRootTemplate = objMLTemplate.Attributes["RootTemplate"].Value;

														// Asigna el tipo de plantilla de los índices
															if (objMLTemplate.Name == "Index")
																intIDType = TemplateModel.TemplateType.Index;
														// Devuelve el tipo de plantilla
															if ((intIDType == TemplateModel.TemplateType.Index || !strStructType.IsEmpty()) && !strRelativeFileName.IsEmpty())
																objColTemplates.Add(intIDType, strName, strStructType, strRelativeFileName, strRootTemplate);
												}
						}
				// Devuelve la colección de plantillas
					return objColTemplates;
		}

		/// <summary>
		///		Carga el texto de una plantilla raíz
		/// </summary>
		internal string LoadTextRootTemplate(string strFileName)
		{ string strText = "";

				// Carga los datos de la plantilla
					if (System.IO.File.Exists(strFileName))
						try
							{ MLFile objMLFile = new LibMarkupLanguage.Services.XML.XMLParser().Load(strFileName);

									foreach (MLNode objMLNode in objMLFile.Nodes)
										if (objMLNode.Name == "Page")
											strText = objMLNode.Value;
							}
						catch {}
				// Devuelve el texto
					return strText;
		}
	}
}
