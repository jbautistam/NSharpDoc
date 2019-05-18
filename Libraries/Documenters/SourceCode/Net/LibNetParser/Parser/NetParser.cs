using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Methods;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Simple;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs;

namespace Bau.Libraries.LibNetParser.Parser
{
	/// <summary>
	///		Base de las clases de interpretación de utilizando Roslyn
	/// </summary>
	public abstract class NetParser
	{
		/// <summary>
		///		Interpreta un texto
		/// </summary>
		internal abstract CompilationUnitModel ParseText(string fileName, string text);

		/// <summary>
		///		Interpreta los hijos de un nodo
		/// </summary>
		protected virtual void ParseChilds(SyntaxNode root, LanguageStructModel parent)
		{
			foreach (SyntaxNode node in root.ChildNodes())
				ParseNode(node, parent);
		}

		/// <summary>
		///		Interpreta los datos de un nodo
		/// </summary>
		protected abstract void ParseNode(SyntaxNode node, LanguageStructModel parent);

		/// <summary>
		///		Interpreta una cláusula using
		/// </summary>
		protected void AddUsing(SyntaxNode node, LanguageStructModel parent, string usingFullName)
		{
			if (!usingFullName.IsEmpty())
			{
				LanguageStructModel compilationUnit = parent;

					// Busca la unidad de compilación a la que está asociada este elemento
					while (compilationUnit != null && compilationUnit.Type != LanguageStructModel.StructType.CompilationUnit &&
						   compilationUnit.Parent != null)
						compilationUnit = compilationUnit.Parent;
					// Añade la cláusula using
					compilationUnit?.CompilationUnit.UsingClauses.Add(usingFullName);
			}
		}

		/// <summary>
		///		Interpreta un espacio de nombres
		/// </summary>
		protected void ParseNameSpace(SyntaxNode node, LanguageStructModel parent, INamespaceSymbol symbol)
		{
			NameSpaceModel nameSpace = parent.Items.CreateSpaceModel(parent);

			// Obtiene los datos del espacio de nombres
			if (symbol != null)
			{ 
				// Obtiene los datos del espacio de nombres
				InitStructModel(nameSpace, symbol, node);
				nameSpace.Name = GetFullNameNameSpace(symbol);
				// Interpreta los nodos
				ParseChilds(node, nameSpace);
			}
		}

		/// <summary>
		///		Interpreta una clase
		/// </summary>
		protected void ParseClass(SyntaxNode node, LanguageStructModel parent, INamedTypeSymbol symbol)
		{
			ClassModel classDef = parent.Items.CreateClass(parent);

				// Obtiene las propiedades de la clase
				classDef.IsStatic = symbol.IsStatic;
				classDef.IsSealed = symbol.IsSealed;
				classDef.IsAbstract = symbol.IsAbstract;
				// Asigna los datos comunes de la clase
				ParseNamedTypeSymbol(classDef, node, symbol);
		}

		/// <summary>
		///		Interpreta un interface
		/// </summary>
		protected void ParseInterface(SyntaxNode node, LanguageStructModel parent, INamedTypeSymbol symbol)
		{
			InterfaceModel interfaceDef = parent.Items.CreateInterface(parent);

				// Asigna las propiedades básicas
				ParseNamedTypeSymbol(interfaceDef, node, symbol);
		}

		/// <summary>
		///		Interpreta los datos de una estructura
		/// </summary>
		protected void ParseStruct(SyntaxNode node, LanguageStructModel parent, INamedTypeSymbol symbol)
		{
			StructModel structDoc = parent.Items.CreateStruct(parent);

				// Inicializa los elementos
				ParseNamedTypeSymbol(structDoc, node, symbol);
		}

		/// <summary>
		///		Interpreta los datos de un NamedTypeSymbol
		/// </summary>
		private void ParseNamedTypeSymbol(BaseComplexModel complex, SyntaxNode node, INamedTypeSymbol symbol)
		{   
			// Inicializa los datos básicos
			InitStructModel(complex, symbol, node);
			// Inicializa la clase base
			if (symbol.BaseType != null)
				complex.BaseClass = GetTypeData(symbol.BaseType);
			// Inicializa las interfaces
			foreach (INamedTypeSymbol objInterface in symbol.Interfaces)
				complex.Interfaces.Add(objInterface.Name);
			// Interpreta los parámetros genéricos
			complex.TypeParameters.AddRange(ParseTypePameters(symbol.TypeParameters));
			// Obtiene los métodos, propiedades y demás
			ParseChilds(node, complex);
		}

		/// <summary>
		///		Interpreta un método
		/// </summary>
		protected void ParseMethod(SyntaxNode node, LanguageStructModel parent, IMethodSymbol symbol)
		{
			MethodModel method = parent.Items.CreateMethod(parent);

				// Interpreta el método
				ParseMethod(node, method, symbol);
				// Asigna los elementos hijo (atributos)
				ParseChilds(node, method);
		}

		/// <summary>
		///		Interpreta un constructor
		/// </summary>
		protected void ParseConstructor(SyntaxNode node, LanguageStructModel parent, IMethodSymbol symbol)
		{
			ConstructorModel constructor = parent.Items.CreateConstructor(parent);

				// Obtiene los datos básicos
				InitStructModel(constructor, symbol, node);
				if (constructor.Name.EqualsIgnoreCase(".ctor"))
					if (constructor.Parent != null && constructor.Parent is ClassModel)
						constructor.Name = constructor.Parent.Name;
				// Inicializa las propiedades
				constructor.IsStatic = symbol.IsStatic;
				// Asigna los argumentos
				constructor.Arguments.AddRange(ParseArguments(symbol.Parameters));
				// Asigna los elementos hijo (atributos)
				ParseChilds(node, constructor);
		}

		/// <summary>
		///		Interpreta una propiedad
		/// </summary>
		protected void ParseProperty(SyntaxNode node, LanguageStructModel parent, IPropertySymbol symbol)
		{
			PropertyModel property = parent.Items.CreateProperty(parent);

				// Obtiene los datos de la propiedad
				InitStructModel(property, symbol, node);
				// Asigna el método Get
				if (symbol.GetMethod != null)
				{
					property.GetMethod = new MethodModel(property);
					ParseMethod(node, property.GetMethod, symbol.GetMethod);
				}
				// Asigna el método Set
				if (symbol.SetMethod != null)
				{
					property.SetMethod = new MethodModel(property);
					ParseMethod(node, property.SetMethod, symbol.SetMethod);
				}
				// Inicializa las propiedades
				property.IsAbstract = symbol.IsAbstract;
				property.IsIndexer = symbol.IsIndexer;
				property.IsOverride = symbol.IsOverride;
				property.IsReadOnly = symbol.IsReadOnly;
				property.IsSealed = symbol.IsSealed;
				property.IsStatic = symbol.IsStatic;
				property.IsVirtual = symbol.IsVirtual;
				property.IsWithEvents = symbol.IsWithEvents;
				property.IsWriteOnly = symbol.IsWriteOnly;
				// Interpreta los argumentos
				property.Arguments.AddRange(ParseArguments(symbol.Parameters));
				// Asigna los elementos hijo (atributos)
				ParseChilds(node, property);
		}

		/// <summary>
		///		Interpreta los datos de un método
		/// </summary>
		private void ParseMethod(SyntaxNode node, MethodModel method, IMethodSymbol symbol)
		{ 
			// Obtiene los datos básicos del método
			InitStructModel(method, symbol, node);
			// Asigna las propiedades
			//symbol.AssociatedAnonymousDelegate;
			//symbol.AssociatedSymbol;
			//symbol.ExplicitInterfaceImplementations;
			method.IsAbstract = symbol.IsAbstract;
			method.IsAsync = symbol.IsAsync;
			method.IsCheckedBuiltin = symbol.IsCheckedBuiltin;
			method.IsDefinition = symbol.IsDefinition;
			method.IsExtensionMethod = symbol.IsExtensionMethod;
			method.IsExtern = symbol.IsExtern;
			method.IsGenericMethod = symbol.IsGenericMethod;
			method.IsImplicitlyDeclared = symbol.IsImplicitlyDeclared;
			method.IsOverride = symbol.IsOverride;
			method.IsSealed = symbol.IsSealed;
			method.IsStatic = symbol.IsStatic;
			method.IsVirtual = symbol.IsVirtual;
			method.ReturnType.IsVoid = symbol.ReturnsVoid;
			method.ReturnType = GetTypeData(symbol.ReturnType);
			//method.ReturnTypeCustomModifiers =  symbol.ReturnTypeCustomModifiers;
			//method.TypeArguments =  symbol.TypeArguments;
			// Asigna los tipos de parámetros
			method.TypeParameters.AddRange(ParseTypePameters(symbol.TypeParameters));
			// Asigna los argumentos
			method.Arguments.AddRange(ParseArguments(symbol.Parameters));
		}

		/// <summary>
		///		Interpreta una serie de parámetros tipados
		/// </summary>
		private List<TypeParameterModel> ParseTypePameters(ImmutableArray<ITypeParameterSymbol> typeParameters)
		{
			List<TypeParameterModel> parameters = new List<TypeParameterModel>();

				// Interpreta los parámetros
				if (typeParameters != null)
					foreach (ITypeParameterSymbol typeParameter in typeParameters)
					{
						TypeParameterModel parameter = new TypeParameterModel();

							// Asigna los datos
							parameter.Name = typeParameter.Name;
							parameter.Order = typeParameter.Ordinal;
							parameter.Type = GetTypeData(typeParameter);
							// Añade el parámetro a la colección
							parameters.Add(parameter);
					}
				// Devuelve la colección de parámetros
				return parameters;
		}

		/// <summary>
		///		Interpreta una serie de argumentos
		/// </summary>
		private List<ArgumentModel> ParseArguments(ImmutableArray<IParameterSymbol> parameters)
		{
			List<ArgumentModel> arguments = new List<ArgumentModel>();

			// Añade los argumentos
			if (parameters != null)
				foreach (IParameterSymbol parameter in parameters)
				{
					ArgumentModel argument = new ArgumentModel();

						// Asigna los datos básicos
						argument.Name = parameter.Name;
						argument.Order = parameter.Ordinal;
						argument.IsOptional = parameter.IsOptional;
						argument.IsParams = parameter.IsParams;
						argument.IsThis = parameter.IsThis;
						if (argument.IsOptional)
							argument.Default = GetDefaultValue(parameter.ExplicitDefaultValue);
						// Obtiene los datos del tipo
						argument.Type = GetTypeData(parameter.Type);
						// Asigna los datos del argumento
						switch (parameter.RefKind)
						{
							case RefKind.None:
									argument.RefType = ArgumentModel.ArgumentType.ByValue;
								break;
							case RefKind.Out:
									argument.RefType = ArgumentModel.ArgumentType.ByOut;
								break;
							case RefKind.Ref:
									argument.RefType = ArgumentModel.ArgumentType.ByRef;
								break;
						}
						// Añade el argumento a la colección
						arguments.Add(argument);
				}
			// Devuelve la colección de argumentos
			return arguments;
		}

		/// <summary>
		///		Interpreta un enumerado
		/// </summary>
		protected void ParseEnum(SyntaxNode root, LanguageStructModel parent, INamedTypeSymbol symbol)
		{
			EnumModel enumDef = parent.Items.CreateEnum(parent);

				// Inicializa los elementos
				InitStructModel(enumDef, symbol, root);
				// Obtiene los datos del tipo
				enumDef.MainType = GetTypeData(symbol.EnumUnderlyingType);
				// Obtiene los miembros del enumerado
				ParseChilds(root, enumDef);
		}

		/// <summary>
		///		Interpreta un miembro de un enumerado
		/// </summary>
		protected void ParseEnumMember(SyntaxNode node, LanguageStructModel parent, IFieldSymbol symbol)
		{
			EnumMemberModel member = parent.Items.CreateEnumMember(parent);

				// Inicializa los elementos
				InitStructModel(member, symbol, node);
		}

		/// <summary>
		///		Interpreta una lista de atributos
		/// </summary>
		protected void ParseAttributeList(SyntaxNode node, LanguageStructModel parent, string attributeFullContent)
		{
			if (!attributeFullContent.IsEmpty())
			{
				string[] attributes = attributeFullContent.Split('\r');

					if (attributes.Length > 0)
						foreach (string attributeContent in attributes)
							if (!attributeContent.IsEmpty())
							{
								string arguments;
								string name = attributeContent.Cut("(", out arguments);
								bool found = false;

									// Quita el paréntesis de cierre
									arguments = arguments.TrimIgnoreNull();
									while (!arguments.IsEmpty() && arguments.EndsWith(")") && !found)
									{
										arguments = arguments.Left(arguments.Length - 1);
										found = true;
									}
									// Añade los argumentos a los atributos del padre
									found = false;
									foreach (AttributeModel attribute in parent.Attributes)
										if (attribute.Name.EqualsIgnoreCase(name))
										{ 
											// Añade la cadena con los argumentos
											attribute.Arguments = attribute.Arguments.AddWithSeparator(arguments, ";");
											// Indica que se ha encontrado el atributo
											found = true;
										}
									// Si no lo ha encontrado, lo añade
									if (!found)
										parent.Attributes.Add(new AttributeModel { Name = name, Arguments = arguments });
							}
			}
		}

		/// <summary>
		///		Valor predeterminado
		/// </summary>
		private string GetDefaultValue(object defaultValue)
		{
			if (defaultValue == null)
				return null;
			else
				return defaultValue.ToString();
		}

		/// <summary>
		///		Obtiene los datos del tipo
		/// </summary>
		private TypedModel GetTypeData(ITypeSymbol symbol)
		{
			TypedModel type = new TypedModel();

				// Asigna los datos
				if (symbol != null)
				{ 
					// Añade los datos de tipo o array
					if (symbol is IArrayTypeSymbol symbolArray)
					{
						type.Name = symbolArray.ElementType.Name;
						type.IsArray = true;
						type.Dimensions = symbolArray.Rank;
						type.NameSpace = GetFullNameNameSpace(symbolArray);
					}
					else
					{
						type.Name = symbol.Name;
						type.NameSpace = GetFullNameNameSpace(symbol);
					}
					// Asigna el tipo base
					if (symbol.BaseType != null)
						type.BaseType = GetTypeData(symbol.BaseType);
					// Añade las restricciones al tipo (para los genéricos)
					if (symbol is ITypeParameterSymbol typeParameter)
						foreach (ITypeSymbol constraintType in typeParameter.ConstraintTypes)
							type.Constraints.Add(GetTypeData(constraintType.BaseType));
				}
				// Devuelve el tipo
				return type;
		}

		/// <summary>
		///		Inicializa los datos de la estructura
		/// </summary>
		private void InitStructModel(LanguageStructModel structItem, ISymbol symbol, SyntaxNode node)
		{
			if (symbol == null)
				structItem.Error = "No se encuentran datos del símbolo";
			else
			{
				////? Hay un error que dice que no se encuentra un ensamblado
				//System.Xml.XmlOutputMethod intID = System.Xml.XmlOutputMethod.Xml;
				//System.Xml.XmlNodeType intType = System.Xml.XmlNodeType.Attribute;
				//try
				//{
				//System.Xml.XmlReader objReader = System.Xml.XmlReader.Create("<root></root>");
				//System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create("");
				//}
				//catch {}

				// Obtiene el nombre y los comentarios
				structItem.Name = symbol.Name;
				structItem.RemarksXml.RawXml = GetDocumentationComment(symbol, node); // symbol.GetDocumentationCommentXml();
																						 // Añade los contenedores
				if (symbol.ContainingAssembly != null)
					structItem.Assembly = symbol.ContainingAssembly.Name;
				// Obtiene los modificadores
				structItem.Modifier = ConvertAccesibility(symbol.DeclaredAccessibility);
				// Añade los atributos
				foreach (AttributeData attribute in symbol.GetAttributes())
					structItem.Attributes.Add(GetAttribute(attribute));
			}
		}

		/// <summary>
		///		Obtiene los comentarios de documentación de un símbolo
		/// </summary>
		private string GetDocumentationComment(ISymbol symbol, SyntaxNode node)
		{
			string xml = string.Empty;

				// Obtiene la lista de Trivia anteriores
				if (node.HasLeadingTrivia)
				{
					// Recorre la lista de trivia añadiendo los que son comentarios
					foreach (SyntaxTrivia trivia in node.GetLeadingTrivia())
						if (trivia.RawKind == 8544) // ... sí, está puesto el valor exacto pero n
							xml += trivia.ToFullString() + Environment.NewLine;
					// Quita los caracteres '///' de la cadena XML
					xml = xml.ReplaceWithStringComparison("///", "");
				}
				// Devuelve el XML
				return xml;
		}

		/// <summary>
		///		Obtiene los datos de un atributo
		/// </summary>
		private AttributeModel GetAttribute(AttributeData symbol)
		{
			AttributeModel attribute = new AttributeModel();

				// Asigna los datos del atributo
				attribute.Name = symbol.AttributeClass.Name;
				attribute.ClassReferenced = symbol.AttributeClass.Name;
				attribute.NameSpaceReferenced = GetFullNameNameSpace(symbol.AttributeClass);
				//// Debug
				//foreach (TypedConstant typeData in symbol.ConstructorArguments)
				//{
				//	System.Diagnostics.Debug.WriteLine(typeData.Type.Name, typeData.Value);
				//}
				// Añade los argumentos
				foreach (KeyValuePair<string, TypedConstant> argument in symbol.NamedArguments)
					switch (argument.Value.Kind)
					{
						case TypedConstantKind.Array:
							break;
						case TypedConstantKind.Enum:
							break;
						case TypedConstantKind.Error:
							break;
						case TypedConstantKind.Primitive:
							break;
						case TypedConstantKind.Type:
							if (argument.Value.IsNull)
								attribute.Arguments = "null";
							else
								attribute.Arguments = attribute.Arguments.AddWithSeparator(argument.Value.Value.ToString(), " ");
							break;
					}
				// Devuelve el atributo
				return attribute;
		}

		/// <summary>
		///		Convierte la accesibilidad
		/// </summary>
		private LanguageStructModel.ModifierType ConvertAccesibility(Accessibility accessibility)
		{
			switch (accessibility)
			{
				case Accessibility.Internal:
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
		private string GetFullNameNameSpace(ISymbol symbol)
		{
			string name = symbol.Name;
			INamespaceSymbol parent = symbol.ContainingNamespace;

				// Concatena los nombres de los espacios superiores
				while (parent != null && !parent.IsGlobalNamespace)
				{ 
					// Añade el nombre
					name = parent.Name + "." + name;
					// Pasa al espacio de nombres superior
					parent = parent.ContainingNamespace;
				}
				// Devuelve el nombre
				return name;
		}
	}
}
