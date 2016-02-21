using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Methods;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Simple;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs;

namespace Bau.Libraries.LibRoslynManager.Parser
{
	/// <summary>
	///		Parser de código fuente utilizando Roslyn
	/// </summary>
	internal class FileParser
	{	// Variables privadas
			private SemanticModel objTreeSemantic;

		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		internal CompilationUnitModel ParseFile(string strFileName)
		{ CompilationUnitModel objUnit = new CompilationUnitModel(strFileName);

				// Interpreta el archivo
					if (!System.IO.File.Exists(strFileName))
						objUnit.Root.Error = "No se encuentra el archivo " + strFileName;
					else
						try
							{ ParseText(objUnit, LibHelper.Files.HelperFiles.LoadTextFile(strFileName));
							}
						catch (Exception objException)
							{ objUnit.Root.Error = "Error al interpretar " + strFileName + ". " + objException.Message;
							}
				// Devuelve la unidad de compilación
					return objUnit;
		}

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		internal CompilationUnitModel ParseText(string strText)
		{ CompilationUnitModel objUnit = new CompilationUnitModel("PlainText");

				// Interpreta el texto
					ParseText(objUnit, strText);
				// Devuelve la unidad de compilación
					return objUnit;
		}

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		private void ParseText(CompilationUnitModel objUnit, string strText)
		{ CSharpCompilation objCompilation;

				// Crea el modelo de compilación
					objCompilation = CSharpCompilation.Create("ParserText").AddSyntaxTrees(CSharpSyntaxTree.ParseText(strText));
				// Obtiene el árbol semántico
					objTreeSemantic = objCompilation.GetSemanticModel(objCompilation.SyntaxTrees[0], true);
				// Interpreta los nodos
					ParseNodes(objUnit, objTreeSemantic.SyntaxTree.GetRoot());
		}

		/// <summary>
		///		Interpreta la unidad de compilación
		/// </summary>
		private void ParseNodes(CompilationUnitModel objUnit, SyntaxNode objRoot)
		{ if (objRoot.Kind() == SyntaxKind.CompilationUnit)
				ParseChilds(objRoot, objUnit.Root);
			else
				objUnit.Root.Error = "No se encuentra el nodo de unidad de compilación en el árbol sintáctico";
		}

		/// <summary>
		///		Interpreta los hijos de un nodo
		/// </summary>
		private void ParseChilds(SyntaxNode objRoot, LanguageStructModel objParent)
		{ foreach (SyntaxNode objNode in objRoot.ChildNodes())
				ParseNode(objNode, objParent);
		}

		/// <summary>
		///		Interpreta los datos de un nodo
		/// </summary>
		private void ParseNode(SyntaxNode objNode, LanguageStructModel objParent)
		{ switch (objNode.Kind())
				{	case SyntaxKind.UsingDirective:
							ParseUsing(objNode, objParent);
						break;
					case SyntaxKind.NamespaceDeclaration:
							ParseNameSpace(objNode, objParent);
						break;
					case SyntaxKind.ClassDeclaration:
							ParseClass(objNode, objParent);
						break;
					case SyntaxKind.ConstructorDeclaration:
							ParseConstructor(objNode, objParent);
						break;
					case SyntaxKind.MethodDeclaration:
							ParseMethod(objNode, objParent);
						break;
					case SyntaxKind.PropertyDeclaration:
							ParseProperty(objNode, objParent);
						break;
					case SyntaxKind.EnumDeclaration:
							ParseEnum(objNode, objParent);
						break;
					case SyntaxKind.EnumMemberDeclaration:
							ParseEnumMember(objNode, objParent);
						break;
					case SyntaxKind.StructDeclaration:	
							ParseStruct(objNode, objParent);
						break;
					case SyntaxKind.InterfaceDeclaration:
							ParseInterface(objNode, objParent);
						break;
				}
		}

		/// <summary>
		///		Interpreta una cláusula using
		/// </summary>
		private void ParseUsing(SyntaxNode objNode, LanguageStructModel objParent)
		{ UsingDirectiveSyntax objUsing = objNode as UsingDirectiveSyntax;

				// Añade el nombre a la lista de cadenas "Using"
					if (objUsing != null)
						{ LanguageStructModel objCompilationUnit = objParent;

								// Busca la unidad de compilación a la que está asociada este elemento
									while (objCompilationUnit != null && objCompilationUnit.IDType != LanguageStructModel.StructType.CompilationUnit &&
												 objCompilationUnit.Parent != null)
										objCompilationUnit = objCompilationUnit.Parent;
								// Añade la cláusula using
									objCompilationUnit?.CompilationUnit.UsingClauses.Add(objUsing.Name.ToFullString());
						}
		}

		/// <summary>
		///		Interpreta un espacio de nombres
		/// </summary>
		private void ParseNameSpace(SyntaxNode objNode, LanguageStructModel objParent)
		{ NameSpaceModel objNameSpace = objParent.Items.CreateSpaceModel(objParent);
			INamespaceSymbol objSymbol = objTreeSemantic.GetDeclaredSymbol(objNode as NamespaceDeclarationSyntax);

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
		private void ParseClass(SyntaxNode objNode, LanguageStructModel objParent)
		{	ClassModel objClass = objParent.Items.CreateClass(objParent);
			INamedTypeSymbol objSymbol = objTreeSemantic.GetDeclaredSymbol(objNode as ClassDeclarationSyntax);

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
		private void ParseInterface(SyntaxNode objNode, LanguageStructModel objParent)
		{ InterfaceModel objInterface = objParent.Items.CreateInterface(objParent);
			INamedTypeSymbol objSymbol = objTreeSemantic.GetDeclaredSymbol(objNode as InterfaceDeclarationSyntax);

				// Asigna las propiedades básicas
					ParseNamedTypeSymbol(objInterface, objNode, objSymbol);
		}

		/// <summary>
		///		Interpreta los datos de una estructura
		/// </summary>
		private void ParseStruct(SyntaxNode objNode, LanguageStructModel objParent)
		{ StructModel objStruct = objParent.Items.CreateStruct(objParent);
			INamedTypeSymbol objSymbol = objTreeSemantic.GetDeclaredSymbol(objNode as StructDeclarationSyntax);

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
		private void ParseMethod(SyntaxNode objNode, LanguageStructModel objParent)
		{	MethodModel objMethod = objParent.Items.CreateMethod(objParent);			

				// Interpreta el método
					ParseMethod(objNode, objMethod, objTreeSemantic.GetDeclaredSymbol(objNode as MethodDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un constructor
		/// </summary>
		private void ParseConstructor(SyntaxNode objNode, LanguageStructModel objParent)
		{ ConstructorModel objConstructor = objParent.Items.CreateConstructor(objParent);
			IMethodSymbol objSymbol = objTreeSemantic.GetDeclaredSymbol(objNode as ConstructorDeclarationSyntax);

				// Obtiene los datos básicos
					InitStructModel(objConstructor, objSymbol, objNode);
					if (objConstructor.Name.EqualsIgnoreCase(".ctor"))
						if (objConstructor.Parent != null && objConstructor.Parent is ClassModel)
							objConstructor.Name = objConstructor.Parent.Name;						
				// Inicializa las propiedades
					objConstructor.IsStatic = objSymbol.IsStatic;
				// Asigna los argumentos
					objConstructor.Arguments.AddRange(ParseArguments(objSymbol.Parameters));
		}

		/// <summary>
		///		Interpreta una propiedad
		/// </summary>
		private void ParseProperty(SyntaxNode objNode, LanguageStructModel objParent)
		{ PropertyModel objProperty = objParent.Items.CreateProperty(objParent);
			IPropertySymbol objSymbol = objTreeSemantic.GetDeclaredSymbol(objNode as PropertyDeclarationSyntax);

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
		private void ParseEnum(SyntaxNode objRoot, LanguageStructModel objParent)
		{ EnumModel objEnum = objParent.Items.CreateEnum(objParent);
			INamedTypeSymbol objSymbol = objTreeSemantic.GetDeclaredSymbol(objRoot as EnumDeclarationSyntax);

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
		private void ParseEnumMember(SyntaxNode objNode, LanguageStructModel objParent)
		{ EnumMemberModel objMember = objParent.Items.CreateEnumMember(objParent);
			IFieldSymbol objSymbol = objTreeSemantic.GetDeclaredSymbol(objNode as EnumMemberDeclarationSyntax);

				// Inicializa los elementos
					InitStructModel(objMember, objSymbol, objNode);
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
					// Obtiene los modificadores
						objStructItem.Modifier = ConvertAccesibility(objSymbol.DeclaredAccessibility);
				}
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
