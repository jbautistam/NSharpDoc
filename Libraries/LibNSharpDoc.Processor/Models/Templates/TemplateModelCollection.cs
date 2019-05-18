using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;

namespace Bau.Libraries.LibNSharpDoc.Processor.Models.Templates
{
	/// <summary>
	///		Colección de <see cref="TemplateModel"/>
	/// </summary>
	internal class TemplateModelCollection : List<TemplateModel>
	{
		internal TemplateModelCollection(string path)
		{
			Path = path;
		}

		/// <summary>
		///		Añade una plantilla a la colección
		/// </summary>
		internal void Add(TemplateModel.TemplateType typeId, string name, string nameStructType,
						  string relativeFileName, string rootTemplate)
		{
			Add(new TemplateModel(typeId, name, nameStructType, Path, relativeFileName, rootTemplate));
		}

		/// <summary>
		///		Busca una plantilla en la colección
		/// </summary>
		internal TemplateModel Search(string nameStructType)
		{
			return this.FirstOrDefault(template => template.NameStructType.EqualsIgnoreCase(nameStructType));
		}

		/// <summary>
		///		Comprueba si se debe generar un archivo de una estructura por los parámetros seleccionados por el usuario
		/// </summary>
		internal bool MustGenerateFile(StructDocumentationModel structDocument, DocumentationParameters parameters)
		{ 
			// Comprueba si se debe generar
			if (structDocument.Parent == null)
				return true;
			else if (MustGenerateDocumentation(structDocument, parameters))
			{ 
				// Comprueba si hay alguna plantilla definida para este tipo en la colección
				foreach (TemplateModel template in this)
					if (template.NameStructType.EqualsIgnoreCase(structDocument.Type))
						return true;
			}
			// Si ha llegado hasta aquí es porque no se debe generar
			return false;
		}

		/// <summary>
		///		Comprueba si se debe generar el documento de una estructura a partir de su ámbito
		/// </summary>
		internal bool MustGenerateDocumentation(StructDocumentationModel structDocument, DocumentationParameters parameters)
		{
			switch (structDocument.Scope)
			{
				case StructDocumentationModel.ScopeType.Internal:
					return parameters.ShowInternal;
				case StructDocumentationModel.ScopeType.Private:
					return parameters.ShowPrivate;
				case StructDocumentationModel.ScopeType.Protected:
					return parameters.ShowProtected;
				case StructDocumentationModel.ScopeType.Global:
					return true;
				default:
					return parameters.ShowPublic;
			}
		}

		/// <summary>
		///		Directorio base para las plantillas
		/// </summary>
		internal string Path { get; }
	}
}
