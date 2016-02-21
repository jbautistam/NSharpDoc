using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibNSharpDoc.Processor.Models.Templates
{
	/// <summary>
	///		Colección de <see cref="TemplateModel"/>
	/// </summary>
	internal class TemplateModelCollection : List<TemplateModel>
	{
		internal TemplateModelCollection(string strPath)
		{ Path = strPath;
		}

		/// <summary>
		///		Añade una plantilla a la colección
		/// </summary>
		internal void Add(TemplateModel.TemplateType intIDType, string strName, string strNameStructType, 
											string strRelativeFileName, string strRootTemplate)
		{ Add(new TemplateModel(intIDType, strName, strNameStructType, Path, strRelativeFileName, strRootTemplate));
		}

		/// <summary>
		///		Busca una plantilla en la colección
		/// </summary>
		internal TemplateModel Search(string strNameStructType)
		{ return this.FirstOrDefault(objTemplate => objTemplate.NameStructType.EqualsIgnoreCase(strNameStructType));
		}

		/// <summary>
		///		Comprueba si se debe generar un archivo de una estructura por los parámetros seleccionados por el usuario
		/// </summary>
		internal bool MustGenerateFile(StructDocumentationModel objStructDocument, Projects.DocumentationParameters objParameters)
		{ // Comprueba si se debe generar
				if (objStructDocument.Parent == null)
					return true;
				else if (MustGenerateDocumentation(objStructDocument, objParameters))
					{ // Comprueba si hay alguna plantilla definida para este tipo en la colección
							foreach (TemplateModel objTemplate in this)
								if (objTemplate.NameStructType.EqualsIgnoreCase(objStructDocument.Type))
									return true;
					}
			// Si ha llegado hasta aquí es porque no se debe generar
				return false;
		}

		/// <summary>
		///		Comprueba si se debe generar el documento de una estructura a partir de su ámbito
		/// </summary>
		internal bool MustGenerateDocumentation(StructDocumentationModel objStructDocument, Projects.DocumentationParameters objParameters)
		{ switch (objStructDocument.Scope)
				{	case StructDocumentationModel.ScopeType.Internal:
						return objParameters.ShowInternal;
					case StructDocumentationModel.ScopeType.Private:
						return objParameters.ShowPrivate;
					case StructDocumentationModel.ScopeType.Protected:
						return objParameters.ShowProtected;
					case StructDocumentationModel.ScopeType.Global:
						return true;
					default:
						return objParameters.ShowPublic;
				}
		}

		/// <summary>
		///		Directorio base para las plantillas
		/// </summary>
		internal string Path { get; }
	}
}
