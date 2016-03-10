using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Methods;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Simple;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs;

namespace Bau.Libraries.LibNetParser.Common.Parser
{
	/// <summary>
	///		Parser de código fuente en C# utilizando Roslyn
	/// </summary>
	public abstract class NetParser
	{	
		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		public CompilationUnitModel ParseFile(string strFileName)
		{ CompilationUnitModel objUnit = new CompilationUnitModel(strFileName);

				// Interpreta el archivo
					if (!System.IO.File.Exists(strFileName))
						objUnit.Root.Error = "No se encuentra el archivo " + strFileName;
					else
						objUnit = ParseText(strFileName, LibHelper.Files.HelperFiles.LoadTextFile(strFileName));
				// Devuelve la unidad de compilación
					return objUnit;
		}

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		protected abstract CompilationUnitModel ParseText(string strFileName, string strText);

		/// <summary>
		///		Interpreta los hijos de un nodo
		/// </summary>
		protected virtual void ParseChilds(SyntaxNode objRoot, LanguageStructModel objParent)
		{ foreach (SyntaxNode objNode in objRoot.ChildNodes())
				ParseNode(objNode, objParent);
		}

		/// <summary>
		///		Interpreta los datos de un nodo
		/// </summary>
		protected abstract void ParseNode(SyntaxNode objNode, LanguageStructModel objParent);

		/// <summary>
		///		Interpreta una cláusula using
		/// </summary>
		protected void AddUsing(SyntaxNode objNode, LanguageStructModel objParent, string strUsingFullName)
		{ if (!strUsingFullName.IsEmpty())
				{ LanguageStructModel objCompilationUnit = objParent;

						// Busca la unidad de compilación a la que está asociada este elemento
							while (objCompilationUnit != null && objCompilationUnit.IDType != LanguageStructModel.StructType.CompilationUnit &&
											objCompilationUnit.Parent != null)
								objCompilationUnit = objCompilationUnit.Parent;
						// Añade la cláusula using
							objCompilationUnit?.CompilationUnit.UsingClauses.Add(strUsingFullName);
				}
		}

		/// <summary>
		///		Interpreta un espacio de nombres
		/// </summary>
		protected void ParseNameSpace(SyntaxNode objNode, LanguageStructModel objParent, INamespaceSymbol objSymbol)
		{ NameSpaceModel objNameSpace = objParent.Items.CreateSpaceModel(objParent);

				// Obtiene los datos del espacio de nombres
					if (objSymbol != null)
						{ // Obtiene los datos del espacio de nombres
								InitStructModel(objNameSpace, objSymbol, objNode);
								objNameSpace.Name = GetFullNameNameSpace(objSymbol);
							// Interpreta los nodos
								ParseChilds(objNode, objNameSpace);
						}
		}

		/// <summary>
		///		Interpreta una clase
		/// </summary>
		protected void ParseClass(SyntaxNode objNode, LanguageStructModel objParent, INamedTypeSymbol objSymbol)
		{	ClassModel objClass = objParent.Items.CreateClass(objParent);

				// Obtiene las propiedades de la clase
					objClass.IsStatic = objSymbol.IsStatic;
					objClass.IsSealed = objSymbol.IsSealed;
					objClass.IsAbstract = objSymbol.IsAbstract;
				// Asigna los datos comunes de la clase
					ParseNamedTypeSymbol(objClass, objNode, objSymbol);
		}

		/// <summary>
		///		Interpreta un interface
		/// </summary>
		protected void ParseInterface(SyntaxNode objNode, LanguageStructModel objParent, INamedTypeSymbol objSymbol)
		{ InterfaceModel objInterface = objParent.Items.CreateInterface(objParent);

				// Asigna las propiedades básicas
					ParseNamedTypeSymbol(objInterface, objNode, objSymbol);
		}

		/// <summary>
		///		Interpreta los datos de una estructura
		/// </summary>
		protected void ParseStruct(SyntaxNode objNode, LanguageStructModel objParent, INamedTypeSymbol objSymbol)
		{ StructModel objStruct = objParent.Items.CreateStruct(objParent);

				// Inicializa los elementos
					ParseNamedTypeSymbol(objStruct, objNode, objSymbol);
		}

		/// <summary>
		///		Interpreta los datos de un NamedTypeSymbol
		/// </summary>
		private void ParseNamedTypeSymbol(BaseComplexModel objComplex, SyntaxNode objNode, INamedTypeSymbol objSymbol)
		{	// Inicializa los datos básicos
				InitStructModel(objComplex, objSymbol, objNode);	
			// Inicializa la clase base
				if (objSymbol.BaseType != null)
					objComplex.BaseClass = GetTypeData(objSymbol.BaseType);
			// Inicializa las interfaces
				foreach (INamedTypeSymbol objInterface in objSymbol.Interfaces)
					objComplex.Interfaces.Add(objInterface.Name);
			// Interpreta los parámetros genéricos
				objComplex.TypeParameters.AddRange(ParseTypePameters(objSymbol.TypeParameters));
			// Obtiene los métodos, propiedades y demás
				ParseChilds(objNode, objComplex);
		}

		/// <summary>
		///		Interpreta un método
		/// </summary>
		protected void ParseMethod(SyntaxNode objNode, LanguageStructModel objParent, IMethodSymbol objSymbol)
		{	MethodModel objMethod = objParent.Items.CreateMethod(objParent);			

				// Interpreta el método
					ParseMethod(objNode, objMethod, objSymbol);
				// Asigna los elementos hijo (atributos)
					ParseChilds(objNode, objMethod);
		}

		/// <summary>
		///		Interpreta un constructor
		/// </summary>
		protected void ParseConstructor(SyntaxNode objNode, LanguageStructModel objParent, IMethodSymbol objSymbol)
		{ ConstructorModel objConstructor = objParent.Items.CreateConstructor(objParent);

				// Obtiene los datos básicos
					InitStructModel(objConstructor, objSymbol, objNode);
					if (objConstructor.Name.EqualsIgnoreCase(".ctor"))
						if (objConstructor.Parent != null && objConstructor.Parent is ClassModel)
							objConstructor.Name = objConstructor.Parent.Name;						
				// Inicializa las propiedades
					objConstructor.IsStatic = objSymbol.IsStatic;
				// Asigna los argumentos
					objConstructor.Arguments.AddRange(ParseArguments(objSymbol.Parameters));
				// Asigna los elementos hijo (atributos)
					ParseChilds(objNode, objConstructor);
		}

		/// <summary>
		///		Interpreta una propiedad
		/// </summary>
		protected void ParseProperty(SyntaxNode objNode, LanguageStructModel objParent, IPropertySymbol objSymbol)
		{ PropertyModel objProperty = objParent.Items.CreateProperty(objParent);

				// Obtiene los datos de la propiedad
					InitStructModel(objProperty, objSymbol, objNode);
				// Asigna el método Get
					if (objSymbol.GetMethod != null)
						{ objProperty.GetMethod = new MethodModel(objProperty);
							ParseMethod(objNode, objProperty.GetMethod, objSymbol.GetMethod);
						}
				// Asigna el método Set
					if (objSymbol.SetMethod != null)
						{ objProperty.SetMethod = new MethodModel(objProperty);
							ParseMethod(objNode, objProperty.SetMethod, objSymbol.SetMethod);
						}
				// Inicializa las propiedades
					objProperty.IsAbstract = objSymbol.IsAbstract;
					objProperty.IsIndexer = objSymbol.IsIndexer;
					objProperty.IsOverride = objSymbol.IsOverride;
					objProperty.IsReadOnly = objSymbol.IsReadOnly;
					objProperty.IsSealed = objSymbol.IsSealed;
					objProperty.IsStatic = objSymbol.IsStatic;
					objProperty.IsVirtual = objSymbol.IsVirtual;
					objProperty.IsWithEvents = objSymbol.IsWithEvents;
					objProperty.IsWriteOnly = objSymbol.IsWriteOnly;
				// Interpreta los argumentos
					objProperty.Arguments.AddRange(ParseArguments(objSymbol.Parameters));
				// Asigna los elementos hijo (atributos)
					ParseChilds(objNode, objProperty);
		}

		/// <summary>
		///		Interpreta los datos de un método
		/// </summary>
		private void ParseMethod(SyntaxNode objNode, MethodModel objMethod, IMethodSymbol objSymbol)
		{ // Obtiene los datos básicos del método
				InitStructModel(objMethod, objSymbol, objNode);
			// Asigna las propiedades
				//objSymbol.AssociatedAnonymousDelegate;
				//objSymbol.AssociatedSymbol;
				//objSymbol.ExplicitInterfaceImplementations;
				objMethod.IsAbstract = objSymbol.IsAbstract;
				objMethod.IsAsync = objSymbol.IsAsync;
				objMethod.IsCheckedBuiltin = objSymbol.IsCheckedBuiltin;
				objMethod.IsDefinition = objSymbol.IsDefinition;
				objMethod.IsExtensionMethod = objSymbol.IsExtensionMethod;
				objMethod.IsExtern = objSymbol.IsExtern;
				objMethod.IsGenericMethod = objSymbol.IsGenericMethod;
				objMethod.IsImplicitlyDeclared = objSymbol.IsImplicitlyDeclared;
				objMethod.IsOverride = objSymbol.IsOverride;
				objMethod.IsSealed = objSymbol.IsSealed;
				objMethod.IsStatic = objSymbol.IsStatic;
				objMethod.IsVirtual = objSymbol.IsVirtual;
				objMethod.ReturnType.IsVoid = objSymbol.ReturnsVoid;
				objMethod.ReturnType = GetTypeData(objSymbol.ReturnType);
				//objMethod.ReturnTypeCustomModifiers =  objSymbol.ReturnTypeCustomModifiers;
				//objMethod.TypeArguments =  objSymbol.TypeArguments;
			// Asigna los tipos de parámetros
				objMethod.TypeParameters.AddRange(ParseTypePameters(objSymbol.TypeParameters));
			// Asigna los argumentos
				objMethod.Arguments.AddRange(ParseArguments(objSymbol.Parameters));
		}

		/// <summary>
		///		Interpreta una serie de parámetros tipados
		/// </summary>
		private List<TypeParameterModel> ParseTypePameters(ImmutableArray<ITypeParameterSymbol> objColTypeParameters)
		{ List<TypeParameterModel> objColParameters = new List<TypeParameterModel>();

				// Interpreta los parámetros
					if (objColTypeParameters != null)
						foreach (ITypeParameterSymbol objTypeParameter in objColTypeParameters)
							{ TypeParameterModel objParameter = new TypeParameterModel();

									// Asigna los datos
										objParameter.Name = objTypeParameter.Name;
										objParameter.Order = objTypeParameter.Ordinal;
										objParameter.Type = GetTypeData(objTypeParameter);
									// Añade el parámetro a la colección
										objColParameters.Add(objParameter);
							}
				// Devuelve la colección de parámetros
					return objColParameters;
		}

		/// <summary>
		///		Interpreta una serie de argumentos
		/// </summary>
		private List<ArgumentModel> ParseArguments(ImmutableArray<IParameterSymbol> objColParameters)
		{ List<ArgumentModel> objColArguments = new List<ArgumentModel>();

				// Añade los argumentos
					if (objColParameters != null)
						foreach (IParameterSymbol objParameter in objColParameters)
							{	ArgumentModel objArgument = new ArgumentModel();

									// Asigna los datos básicos
										objArgument.Name = objParameter.Name;
										objArgument.Order = objParameter.Ordinal;
										objArgument.IsOptional = objParameter.IsOptional;
										objArgument.IsParams = objParameter.IsParams;
										objArgument.IsThis = objParameter.IsThis;
										if (objArgument.IsOptional)
											objArgument.Default = GetDefaultValue(objParameter.ExplicitDefaultValue);
									// Obtiene los datos del tipo
										objArgument.Type = GetTypeData(objParameter.Type);
									// Asigna los datos del argumento
										switch (objParameter.RefKind)
											{	case RefKind.None:
														objArgument.RefType = ArgumentModel.ArgumentType.ByValue;
													break;
												case RefKind.Out:
														objArgument.RefType = ArgumentModel.ArgumentType.ByOut;
													break;
												case RefKind.Ref:
														objArgument.RefType = ArgumentModel.ArgumentType.ByRef;
													break;
											}
									// Añade el argumento a la colección
										objColArguments.Add(objArgument);
							}
				// Devuelve la colección de argumentos
					return objColArguments;
		}

		/// <summary>
		///		Interpreta un enumerado
		/// </summary>
		protected void ParseEnum(SyntaxNode objRoot, LanguageStructModel objParent, INamedTypeSymbol objSymbol)
		{ EnumModel objEnum = objParent.Items.CreateEnum(objParent);

				// Inicializa los elementos
					InitStructModel(objEnum, objSymbol, objRoot);
				// Obtiene los datos del tipo
					objEnum.Type = GetTypeData(objSymbol.EnumUnderlyingType);
				// Obtiene los miembros del enumerado
					ParseChilds(objRoot, objEnum);
		}

		/// <summary>
		///		Interpreta un miembro de un enumerado
		/// </summary>
		protected void ParseEnumMember(SyntaxNode objNode, LanguageStructModel objParent, IFieldSymbol objSymbol)
		{ EnumMemberModel objMember = objParent.Items.CreateEnumMember(objParent);

				// Inicializa los elementos
					InitStructModel(objMember, objSymbol, objNode);
		}

		/// <summary>
		///		Interpreta una lista de atributos
		/// </summary>
		protected void ParseAttributeList(SyntaxNode objNode, LanguageStructModel objParent, string strAttributeFullContent)
		{ if (!strAttributeFullContent.IsEmpty())
				{ string [] arrStrAttributes = strAttributeFullContent.Split('\r');
					
						if (arrStrAttributes.Length > 0)
							foreach (string strAttribute in arrStrAttributes)
								if (!strAttribute.IsEmpty())
									{ string strArguments;
										string strName = strAttribute.Cut("(", out strArguments);
										bool blnFound = false;

											// Quita el paréntesis de cierre
												strArguments = strArguments.TrimIgnoreNull();
												while (!strArguments.IsEmpty() && strArguments.EndsWith(")") && !blnFound)
													{ strArguments = strArguments.Left(strArguments.Length - 1);
														blnFound = true;
													}
											// Añade los argumentos a los atributos del padre
												blnFound = false;
												foreach (AttributeModel objAttribute in objParent.Attributes)
													if (objAttribute.Name.EqualsIgnoreCase(strName))
														{ // Añade la cadena con los argumentos
																objAttribute.Arguments = objAttribute.Arguments.AddWithSeparator(strArguments, ";");
															// Indica que se ha encontrado el atributo
																blnFound = true;
														}
											// Si no lo ha encontrado, lo añade
												if (!blnFound)
													objParent.Attributes.Add(new AttributeModel { Name = strName, Arguments = strArguments });
									}
				}
		}

		/// <summary>
		///		Valor predeterminado
		/// </summary>
		private string GetDefaultValue(object objDefault)
		{ if (objDefault == null)
				return null;
			else
				return objDefault.ToString();
		}

		/// <summary>
		///		Obtiene los datos del tipo
		/// </summary>
		private TypedModel GetTypeData(ITypeSymbol objSymbol)
		{ TypedModel objType = new TypedModel();

				// Asigna los datos
					if (objSymbol != null)
						{ // Añade los datos de tipo o array
								if (objSymbol is IArrayTypeSymbol)
									{ IArrayTypeSymbol objArraySymbol = objSymbol as IArrayTypeSymbol;

											// Obtiene los datos del array
												objType.Name = objArraySymbol.ElementType.Name;
												objType.IsArray = true;
												objType.Dimensions = objArraySymbol.Rank;
												objType.NameSpace = GetFullNameNameSpace(objArraySymbol);
									}
								else
									{ objType.Name = objSymbol.Name;
										objType.NameSpace = GetFullNameNameSpace(objSymbol);
									}
							// Asigna el tipo base
								if (objSymbol.BaseType != null)
									objType.BaseType = GetTypeData(objSymbol.BaseType);
							// Añade las restricciones al tipo (para los genéricos)
								if (objSymbol is ITypeParameterSymbol)
									{ ITypeParameterSymbol objTypeParameter = objSymbol as ITypeParameterSymbol;
									
											foreach (ITypeSymbol objConstraintType in objTypeParameter.ConstraintTypes)
												objType.Constraints.Add(GetTypeData(objConstraintType.BaseType));
									}
						}
				// Devuelve el tipo
					return objType;
		}

		/// <summary>
		///		Inicializa los datos de la estructura
		/// </summary>
		private void InitStructModel(LanguageStructModel objStructItem, ISymbol objSymbol, SyntaxNode objNode)
		{ if (objSymbol == null)
				objStructItem.Error = "No se encuentran datos del símbolo";
			else
				{ // Obtiene el nombre y los comentarios
						objStructItem.Name = objSymbol.Name;
						objStructItem.RemarksXml.RawXml = objSymbol.GetDocumentationCommentXml();
					// Añade los contenedores
						if (objSymbol.ContainingAssembly != null)
							objStructItem.Assembly = objSymbol.ContainingAssembly.Name;
					// Obtiene los modificadores
						objStructItem.Modifier = ConvertAccesibility(objSymbol.DeclaredAccessibility);
					// Añade los atributos
						foreach (AttributeData objAttribute in objSymbol.GetAttributes())
							objStructItem.Attributes.Add(GetAttribute(objAttribute));
				}
		}

		/// <summary>
		///		Obtiene los datos de un atributo
		/// </summary>
		private AttributeModel GetAttribute(AttributeData objSymbol)
		{ AttributeModel objAttribute = new AttributeModel();

				// Asigna los datos del atributo
					objAttribute.Name = objSymbol.AttributeClass.Name;
					objAttribute.ClassReferenced = objSymbol.AttributeClass.Name;
					objAttribute.NameSpaceReferenced = GetFullNameNameSpace(objSymbol.AttributeClass);
				// Añade los argumentos
					foreach (TypedConstant objTypeData in objSymbol.ConstructorArguments)
						{ System.Diagnostics.Debug.WriteLine(objTypeData.Type.Name, objTypeData.Value);
						}
					foreach (KeyValuePair<string, TypedConstant> objArgument in objSymbol.NamedArguments)
						switch (objArgument.Value.Kind)
							{	case TypedConstantKind.Array:
									break;
								case TypedConstantKind.Enum:
									break;
								case TypedConstantKind.Error:
									break;
								case TypedConstantKind.Primitive:
									break;
								case TypedConstantKind.Type:
										if (objArgument.Value.IsNull)
											objAttribute.Arguments = "null";
										else
											objAttribute.Arguments = objAttribute.Arguments.AddWithSeparator(objArgument.Value.Value.ToString(), " ");
									break;
							}
				// Devuelve el atributo
					return objAttribute;
		}

		/// <summary>
		///		Convierte la accesibilidad
		/// </summary>
		private LanguageStructModel.ModifierType ConvertAccesibility(Accessibility intAccessibility)
		{	switch (intAccessibility)
				{	case Accessibility.Internal:
						return LanguageStructModel.ModifierType.Internal;
					case Accessibility.Private:
						return LanguageStructModel.ModifierType.Private;
					case Accessibility.Protected:
						return LanguageStructModel.ModifierType.Protected;
					case Accessibility.ProtectedAndInternal:
						return LanguageStructModel.ModifierType.ProtectedAndInternal;
					case Accessibility.ProtectedOrInternal:
						return LanguageStructModel.ModifierType.ProtectedOrInternal;
					case Accessibility.Public:
						return LanguageStructModel.ModifierType.Public;
					default:
						return LanguageStructModel.ModifierType.Unknown;
				}
		}

		/// <summary>
		///		Obtiene el nombre completo de un espacio de nombres
		/// </summary>
		private string GetFullNameNameSpace(ISymbol objSymbol)
		{ string strName = objSymbol.Name;
			INamespaceSymbol objParent = objSymbol.ContainingNamespace;

				// Concatena los nombres de los espacios superiores
					while (objParent != null && !objParent.IsGlobalNamespace)
						{ // Añade el nombre
								strName = objParent.Name + "." + strName;
							// Pasa al espacio de nombres superior
								objParent = objParent.ContainingNamespace;
						}
				// Devuelve el nombre
					return strName;
		}
	}
}
