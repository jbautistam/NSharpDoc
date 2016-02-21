using System;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;

namespace Bau.Libraries.LibNSharpDoc.Processor.Repository.Projects
{
	/// <summary>
	///		Repository para <see cref="ProjectDocumentationModel"/>
	/// </summary>
	internal class ProjectRepository 
	{ // Constantes privadas
			private const string cnstStrTagRoot = "Project";
			private const string cnstStrTagDocumentationType = "DocumentationType";
			private const string cnstStrTagShowPublic = "ShowPublic";
			private const string cnstStrTagShowInternal = "ShowInternal";
			private const string cnstStrTagShowProtected = "ShowProtected";
			private const string cnstStrTagShowPrivate = "ShowPrivate";
			private const string cnstStrTagTemplatePath = "TemplatePath";
			private const string cnstStrTagProvider = "Provider";
			private const string cnstStrTagProviderPath = "Path";
			private const string cnstStrTagProviderTemplatePath = "TemplatePath";
			private const string cnstStrTagProviderAdditionalPath = "AdditionalPath";
			private const string cnstStrTagProviderType = "Type";
			private const string cnstStrTagProviderParameter = "Parameter";
			private const string cnstStrTagProviderParameterKey = "Key";
			private const string cnstStrTagProviderParameterValue = "Value";

		/// <summary>
		///		Carga los datos de un archivo
		/// </summary>
		internal ProjectDocumentationModel Load(string strFileName)
		{ ProjectDocumentationModel objProject = new ProjectDocumentationModel();

				// Asigna el nombre de archivo
					objProject.FileName = strFileName;
				// Carga los datos
					if (System.IO.File.Exists(strFileName))
						{ MLFile objMLFile = new MLSerializer().Parse(MLSerializer.SerializerType.XML, strFileName);

								foreach (MLNode objMLNode in objMLFile.Nodes)
									if (objMLNode.Name == cnstStrTagRoot)
										{ // Añade los parámetros básicos
												objProject.IDType = (ProjectDocumentationModel.DocumentationType) objMLNode.Nodes[cnstStrTagDocumentationType].Value.GetInt(0);
												objProject.OutputPath = objMLNode.Nodes[cnstStrTagProviderPath].Value;
												objProject.TemplatePath = objMLNode.Nodes[cnstStrTagProviderTemplatePath].Value;
												objProject.GenerationParameters.ShowPublic = objMLNode.Nodes[cnstStrTagShowPublic].Value.GetBool();
												objProject.GenerationParameters.ShowProtected = objMLNode.Nodes[cnstStrTagShowProtected].Value.GetBool();
												objProject.GenerationParameters.ShowInternal = objMLNode.Nodes[cnstStrTagShowInternal].Value.GetBool();
												objProject.GenerationParameters.ShowPrivate = objMLNode.Nodes[cnstStrTagShowPrivate].Value.GetBool();
											// Añade los proveedores
												foreach (MLNode objMLChild in objMLNode.Nodes)
													if (objMLChild.Name == cnstStrTagProvider)
														objProject.Providers.Add(LoadProvider(objMLChild));
										}
						}
				// Devuelve el proyecto
					return objProject;
		}

		/// <summary>
		///		Carga los datos de un proveedor
		/// </summary>
		private ProviderModel LoadProvider(MLNode objMLNode)
		{ ProviderModel objProvider = new ProviderModel();
			
				// Carga los datos
					objProvider.Type = objMLNode.Nodes[cnstStrTagProviderType].Value;
					objProvider.AdditionalDocumentsPath = objMLNode.Nodes[cnstStrTagProviderAdditionalPath].Value;
				// Carga los parámetros
					foreach (MLNode objMLChild in objMLNode.Nodes)
						if (objMLChild.Name == cnstStrTagProviderParameter)
							objProvider.Parameters.Add(objMLChild.Attributes[cnstStrTagProviderParameterKey].Value, 
																				 objMLChild.Nodes[cnstStrTagProviderParameterValue].Value);
				// Devuelve los datos del proveedor
					return objProvider;
		}

		/// <summary>
		///		Graba los datos de un mensaje
		/// </summary>
		internal void Save(string strFileName, ProjectDocumentationModel objProject)
		{ MLFile objMLFile = new MLFile();
			MLNode objMLNode = objMLFile.Nodes.Add(cnstStrTagRoot);

				// Añade los parámetros básicos
					objMLNode.Nodes.Add(cnstStrTagDocumentationType, (int) objProject.IDType);
					objMLNode.Nodes.Add(cnstStrTagProviderPath, objProject.OutputPath);
					objMLNode.Nodes.Add(cnstStrTagProviderTemplatePath, objProject.TemplatePath);
				// Añade los parámetros de documentación
					objMLNode.Nodes.Add(cnstStrTagShowPublic, objProject.GenerationParameters.ShowPublic);
					objMLNode.Nodes.Add(cnstStrTagShowProtected, objProject.GenerationParameters.ShowProtected);
					objMLNode.Nodes.Add(cnstStrTagShowInternal, objProject.GenerationParameters.ShowInternal);
					objMLNode.Nodes.Add(cnstStrTagShowPrivate, objProject.GenerationParameters.ShowPrivate);
				// Añade los nodos de proveedor
					foreach (ProviderModel objProvider in objProject.Providers)
						objMLNode.Nodes.Add(GetNodeProvider(objProvider));
				// Graba el archivo
					new MLSerializer().Save(MLSerializer.SerializerType.XML, objMLFile, strFileName);
		}

		/// <summary>
		///		Obtiene el nodo de un provedor
		/// </summary>
		private MLNode GetNodeProvider(ProviderModel objProvider) 
		{	MLNode objMLNode = new MLNode(cnstStrTagProvider);
		
				// Añade los nodos de proveedor
					objMLNode.Nodes.Add(cnstStrTagProviderType, objProvider.Type);
					objMLNode.Nodes.Add(cnstStrTagProviderAdditionalPath, objProvider.AdditionalDocumentsPath);
				// Añade los parámetros
					foreach (System.Collections.Generic.KeyValuePair<string, StructParameterModel> objParameter in objProvider.Parameters.Parameters)
						{ MLNode objMLChild = objMLNode.Nodes.Add(cnstStrTagProviderParameter);

								// Añade los valores del parámetro
									objMLChild.Attributes.Add(cnstStrTagProviderParameterKey, objParameter.Value.Key);
									objMLChild.Nodes.Add(cnstStrTagProviderParameterValue, objParameter.Value.Value?.ToString());
						}
				// Devuelve el nodo
					return objMLNode;
		}
	}
}