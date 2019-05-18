using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibSourceCode.Models.Groups;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Simple;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Methods;

namespace Bau.Libraries.LibSourceCode.Documenter.Common.Prepare
{
	/// <summary>
	///		Conversor a una serie de estructuras
	/// </summary>
	internal class StructConversor
	{
		/// <summary>
		///		Convierte un grupo de espacio de nombres en una serie de estructuras de documentación
		/// </summary>
		internal StructDocumentationModelCollection Convert(NameSpaceGroupModelCollection groups)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Convierte los grupos
				foreach (NameSpaceGroupModel group in groups)
					structsDoc.Add(Convert(group));
				// Devuelve la colección de estructuras
				return structsDoc;
		}

		/// <summary>
		///		Convierte un espacio de nombres en una estructura
		/// </summary>
		private StructDocumentationModel Convert(NameSpaceGroupModel group)
		{
			StructDocumentationModel structDoc = Create(null, group.NameSpace, "NameSpace");

				// Añade los datos
				structDoc.Childs.AddRange(Convert(structDoc, group.NameSpace.Items));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Convierte una serie de elementos en estructuras
		/// </summary>
		private StructDocumentationModelCollection Convert(StructDocumentationModel structDoc, LanguageStructModelCollection items)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Convierte los elementos
				foreach (LanguageStructModel item in items)
					switch (item.Type)
					{
						case LanguageStructModel.StructType.Class:
								structsDoc.Add(ConvertClass(structDoc, item as ClassModel));
							break;
						case LanguageStructModel.StructType.CompilationUnit:
								structsDoc.AddRange(Convert(structDoc, item.Items));
							break;
						case LanguageStructModel.StructType.Constructor:
								structsDoc.Add(ConvertConstructor(structDoc, item as ConstructorModel));
							break;
						case LanguageStructModel.StructType.Enum:
								structsDoc.Add(ConvertEnum(structDoc, item as EnumModel));
							break;
						case LanguageStructModel.StructType.EnumMember:
								structsDoc.Add(ConvertEnumMember(structDoc, item as EnumMemberModel));
							break;
						case LanguageStructModel.StructType.Interface:
								structsDoc.Add(ConvertInterface(structDoc, item as InterfaceModel));
							break;
						case LanguageStructModel.StructType.Method:
								structsDoc.Add(ConvertMethod(structDoc, item as MethodModel));
							break;
						case LanguageStructModel.StructType.Property:
								structsDoc.Add(ConvertProperty(structDoc, item as PropertyModel));
							break;
						case LanguageStructModel.StructType.Struct:
								structsDoc.Add(ConvertStruct(structDoc, item as StructModel));
							break;
					}
				// Devuelve la colección de estructuras
				return structsDoc;
		}

		/// <summary>
		///		Convierte una interface
		/// </summary>
		private StructDocumentationModel ConvertInterface(StructDocumentationModel parent, InterfaceModel interfaceDef)
		{
			StructDocumentationModel structDoc = Create(parent, interfaceDef, "Interface");

				// Añade los parámetros de la clase compleja base
				AddParametersBaseComplex(structDoc, interfaceDef);
				// Crea los elementos hijo
				structDoc.Childs.AddRange(Convert(structDoc, interfaceDef.Items));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Convierte una clase
		/// </summary>
		private StructDocumentationModel ConvertClass(StructDocumentationModel parent, ClassModel classDef)
		{
			StructDocumentationModel structDoc = Create(parent, classDef, "Class");

				// Añade los parámetros de la clase compleja base
				AddParametersBaseComplex(structDoc, classDef);
				// Añade los parámetros
				structDoc.Parameters.Add("IsSealed", classDef.IsSealed.ToString());
				structDoc.Parameters.Add("IsStatic", classDef.IsStatic.ToString());
				structDoc.Parameters.Add("IsAbstract", classDef.IsAbstract.ToString());
				// Añade los elementos hijo
				structDoc.Childs.AddRange(Convert(structDoc, classDef.Items));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Convierte una estructura
		/// </summary>
		private StructDocumentationModel ConvertStruct(StructDocumentationModel parent, StructModel item)
		{
			StructDocumentationModel structDoc = Create(parent, item, "Struct");

				// Añade los parámetros de la clase compleja base
				AddParametersBaseComplex(structDoc, item);
				// Crea los elementos hijo
				structDoc.Childs.AddRange(Convert(structDoc, item.Items));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Añade los parámetros de un elemento complejo
		/// </summary>
		private void AddParametersBaseComplex(StructDocumentationModel structDoc, BaseComplexModel complex)
		{
			LanguageStructModel nameSpace = complex.GetNameSpace();

				// Añade la clase base
				structDoc.Childs.Add(GetTypeStruct(structDoc, "BaseClass", complex.BaseClass));
				structDoc.Parameters.Add(GetTypeStructShort("BaseType", complex.BaseClass));
				// Añade las interfaces
				foreach (string strInterface in complex.Interfaces)
					structDoc.Parameters.Add("Interface", strInterface);
				// Añade los parámetros tipados
				AddListTypeParameter(structDoc, complex.TypeParameters);
		}

		/// <summary>
		///		Obtiene un parámetro para la lista de tipo de parámetros
		/// </summary>
		private void AddListTypeParameter(StructDocumentationModel parent, List<TypeParameterModel> typeParameters)
		{
			if (typeParameters != null && typeParameters.Count > 0)
				foreach (TypeParameterModel typeParameter in typeParameters)
				{
					StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Global,
																					  typeParameter.Name, "TypeParameter", 0);

						// Añade el tipo
						structDoc.Childs.Add(GetTypeStruct(structDoc, "Type", typeParameter.Type));
						// Añade la estructura al padre
						parent.Childs.Add(structDoc);
				}
		}

		/// <summary>
		///		Añade un parámetro con la lista de argumentos
		/// </summary>
		private void AddListArguments(StructDocumentationModel parent, BaseMethodModel method, List<ArgumentModel> arguments)
		{
			if (arguments != null && arguments.Count > 0)
				foreach (ArgumentModel argument in arguments)
				{
					StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Global, argument.Name, "Argument", 0);

						// Añade el tipo
						structDoc.Childs.Add(GetTypeStruct(structDoc, "TypeStruct", argument.Type));
						// Añade los parámetros del argumento
						structDoc.Parameters.Add(GetTypeStructShort("Type", argument.Type));
						structDoc.Parameters.Add("RefType", argument.RefType.ToString());
						structDoc.Parameters.Add("IsOptional", argument.IsOptional);
						structDoc.Parameters.Add("IsParams", argument.IsParams);
						structDoc.Parameters.Add("IsThis", argument.IsThis);
						structDoc.Parameters.Add("Default", argument.Default);
						structDoc.Parameters.Add("Summary", ConvertRemarks(method.RemarksXml.GetParameterRemarks(argument.Name)));
						// Añade la estrucutra al padre
						parent.Childs.Add(structDoc);
				}
		}

		/// <summary>
		///		Obtiene un parámetro con los datos de un tipo (para la definición corta del tipo)
		/// </summary>
		private StructParameterModel GetTypeStructShort(string name, TypedModel type)
		{
			if (type != null)
				return new StructParameterModel(name, type.Name, type.Name);
			else
				return new StructParameterModel(name, "", "");
		}

		/// <summary>
		///		Obtiene el parámetro de un tipo
		/// </summary>
		private StructDocumentationModel GetTypeStruct(StructDocumentationModel parent, string name, TypedModel type)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Global, name, "Type", 0);

				// Si realmente se le pasa un tipo
				if (type != null)
				{ 
					// Asigna los datos del tipo
					structDoc.Childs.Add(GetTypeStruct(parent, "BaseTypeStruct", type.BaseType));
					structDoc.Parameters.Add(GetTypeStructShort("BaseType", type.BaseType));
					structDoc.Parameters.Add("NameSpace", type.NameSpace);
					structDoc.Parameters.Add("IsVoid", type.IsVoid);
					structDoc.Parameters.Add("IsArray", type.IsArray);
					structDoc.Parameters.Add("Dimensions", type.Dimensions);
					// Añade las restricciones
					AddListConstraints(structDoc, type.Constraints);
				}
				// Devuelve el parámetro
				return structDoc;
		}

		/// <summary>
		///		Añade una lista de restricciones a los parámetros
		/// </summary>
		private void AddListConstraints(StructDocumentationModel parent, List<TypedModel> constraints)
		{
			if (constraints != null && constraints.Count > 0)
				foreach (TypedModel constraint in constraints)
				{
					StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Global, constraint.Name, "Constraint", 0);

						// Añade los parámetros
						structDoc.Childs.Add(GetTypeStruct(structDoc, "ConstraintStruct", constraint));
						structDoc.Parameters.Add(GetTypeStructShort("Type", constraint));
						// Añade la restricción al padre al parámetro
						parent.Childs.Add(structDoc);
				}
		}

		/// <summary>
		///		Convierte un enumerado
		/// </summary>
		private StructDocumentationModel ConvertEnum(StructDocumentationModel parent, EnumModel item)
		{
			StructDocumentationModel structDoc = Create(parent, item, "Enum");

				// Crea los elementos hijo
				structDoc.Childs.AddRange(Convert(structDoc, item.Items));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Convierte un miembro de enumerado
		/// </summary>
		private StructDocumentationModel ConvertEnumMember(StructDocumentationModel parent, EnumMemberModel item)
		{
			return Create(parent, item, "EnumMember");
		}

		/// <summary>
		///		Convierte un constructor
		/// </summary>
		private StructDocumentationModel ConvertConstructor(StructDocumentationModel parent, ConstructorModel item)
		{
			StructDocumentationModel structDoc = Create(parent, item, "Constructor");

				// Añade los argumentos
				structDoc.Parameters.Add(GetPrototype(item));
				AddListArguments(structDoc, item, item.Arguments);
				AddListTypeParameter(structDoc, item.TypeParameters);
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Convierte un método
		/// </summary>
		private StructDocumentationModel ConvertMethod(StructDocumentationModel parent, MethodModel item)
		{
			StructDocumentationModel structDoc = Create(parent, item, "Method");

				// Añade los argumentos
				structDoc.Parameters.Add(GetPrototype(item));
				AddListArguments(structDoc, item, item.Arguments);
				AddListTypeParameter(structDoc, item.TypeParameters);
				// Añade el valor de retorno
				structDoc.Childs.Add(GetReturnValue(structDoc, item.ReturnType, ConvertRemarks(item.RemarksXml.Returns)));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Obtiene el valor de retorno
		/// </summary>
		private StructDocumentationModel GetReturnValue(StructDocumentationModel parent, TypedModel objType, string strRemarks)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Global, "ReturnValue", "ReturnValue", 0);

				// Añade el tipo
				structDoc.Childs.Add(GetTypeStruct(structDoc, "ReturnValueStruct", objType));
				structDoc.Parameters.Add(GetTypeStructShort("Type", objType));
				// Añade los valores
				structDoc.Parameters.Add("Summary", ConvertRemarks(strRemarks));
				// Devuelve el parámetro
				return structDoc;
		}

		/// <summary>
		///		Convierte una propiedad
		/// </summary>
		private StructDocumentationModel ConvertProperty(StructDocumentationModel parent, PropertyModel item)
		{
			StructDocumentationModel structDoc = Create(parent, item, "Property");

				// Añade los argumentos
				structDoc.Parameters.Add(GetPrototype(item));
				AddListTypeParameter(structDoc, item.TypeParameters);
				AddListArguments(structDoc, item, item.Arguments);
				// Añade los métodos get y set
				ConvertPropertyMethod(structDoc, item.GetMethod, "get");
				ConvertPropertyMethod(structDoc, item.SetMethod, "set");
				// Añade el valor de retorno
				if (item.GetMethod != null)
					structDoc.Childs.Add(GetReturnValue(structDoc, item.GetMethod.ReturnType, ConvertRemarks(item.GetMethod.RemarksXml.Returns)));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Convierte un método de una propiedad
		/// </summary>
		private void ConvertPropertyMethod(StructDocumentationModel parent, MethodModel method, string name)
		{
			if (method != null)
			{
				StructDocumentationModel structDoc = ConvertMethod(parent, method);

					// Cambia el nombre
					structDoc.Name = name;
					// ... y se lo añade a la colección
					parent.Childs.Add(structDoc);
			}
		}

		/// <summary>
		///		Crea una estructura de documentación
		/// </summary>
		private StructDocumentationModel Create(StructDocumentationModel parent, LanguageStructModel item, string type)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(parent, GetScope(item), item.Name, type, item.Order);
			LanguageStructModel nameSpace = item.GetNameSpace();

				// Añade el espacio de nombres
				if (nameSpace != null)
					structDoc.Parameters.Add("NameSpace", nameSpace.Name, nameSpace.Name);
				// Añade los parámetros básicos
				structDoc.Parameters.Add("Summary", ConvertRemarks(item.RemarksXml.Summary));
				structDoc.Parameters.Add("Remarks", ConvertRemarks(item.RemarksXml.Remarks));
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Obtiene el ámbito
		/// </summary>
		private StructDocumentationModel.ScopeType GetScope(LanguageStructModel structDoc)
		{
			switch (structDoc.Modifier)
			{
				case LanguageStructModel.ModifierType.Internal:
					return StructDocumentationModel.ScopeType.Internal;
				case LanguageStructModel.ModifierType.Private:
					return StructDocumentationModel.ScopeType.Private;
				case LanguageStructModel.ModifierType.Protected:
				case LanguageStructModel.ModifierType.ProtectedAndInternal:
				case LanguageStructModel.ModifierType.ProtectedOrInternal:
					return StructDocumentationModel.ScopeType.Protected;
				default:
					return StructDocumentationModel.ScopeType.Public;
			}
		}
		/// <summary>
		///		Obtiene el prototipo de una estructura
		/// </summary>
		private StructParameterModel GetPrototype(LanguageStructModel structDoc)
		{
			string prototype = GetAttributesPrototype(structDoc);

				// Obtiene los datos del prototipo
				switch (structDoc.Type)
				{
					case LanguageStructModel.StructType.Property:
							prototype += GetPropertyPrototype(structDoc as PropertyModel);
						break;
					case LanguageStructModel.StructType.Constructor:
							prototype += GetMethodPrototype(structDoc as ConstructorModel);
						break;
					case LanguageStructModel.StructType.Method:
							MethodModel method = structDoc as MethodModel;

								if (method != null)
									prototype += GetMethodPrototype(method, method.IsAsync, method.ReturnType);
						break;
				}
				// Devuelve el nodo
				return new StructParameterModel("Prototype", prototype, null);
		}

		/// <summary>
		///		Obtiene el prototipo de los atributos de una estructura
		/// </summary>
		private string GetAttributesPrototype(LanguageStructModel structDoc)
		{
			string prototype = "";

				// Añade los atributos
				foreach (AttributeModel attribute in structDoc.Attributes)
					prototype = prototype.AddWithSeparator($"{attribute.Name}({attribute.Arguments})", ",");
				// Añade los corchetes
				if (!prototype.IsEmpty())
					prototype = "[" + prototype + "]" + Environment.NewLine;
				// Devuelve la cadena de prototipo
				return prototype;
		}

		/// <summary>
		///		Obtiene el prototipo de una propiedad
		/// </summary>
		private string GetPropertyPrototype(PropertyModel property)
		{
			string prototype = GetMethodPrototypeDefinition(property, false, property.GetMethod.ReturnType);

				// Argumentos
				if (property.Arguments.Count > 0)
					prototype += "[" + GetArgumentsPrototype(property) + "]";
				// Get y set
				prototype += " { ";
				prototype += GetPropertyMethodPrototype(property.Modifier, property.GetMethod, "get");
				prototype += " " + GetPropertyMethodPrototype(property.Modifier, property.SetMethod, "set");
				prototype += " }";
				// Devuelve el prototipo
				return prototype;
		}

		/// <summary>
		///		Obtiene el prototipo de una función
		/// </summary>
		private string GetMethodPrototype(BaseMethodModel method, bool isAsync = false, TypedModel returnType = null)
		{
			string prototype = GetMethodPrototypeDefinition(method, isAsync, returnType);

				// Añade los argumentos
				prototype += "(" + GetArgumentsPrototype(method) + ")";
				// Devuelve el prototipo
				return prototype;
		}

		/// <summary>
		///		Obtiene la definición (el prefijo) de un prototipo de un método
		/// </summary>
		private string GetMethodPrototypeDefinition(BaseMethodModel method, bool isAsync, TypedModel returnType)
		{
			string prototype = "";

				// Añade los modificadores
				prototype += GetModifierText(method.Modifier);
				if (isAsync)
					prototype += " async";
				if (method.IsAbstract)
					prototype += " abstract";
				if (method.IsOverride)
					prototype += " override";
				if (method.IsSealed)
					prototype += " sealed";
				if (method.IsStatic)
					prototype += " static";
				if (method.IsVirtual)
					prototype += " virtual";
				// Añade el valor de retorno
				if (returnType != null)
					prototype += " " + GetLinkTypeName(returnType);
				// Añade el nombre del método
				prototype += " " + method.Name;
				// Añade los genéricos
				prototype += GetMethodPrototypeGenerics(method.TypeParameters);
				// Devuelve la definición de prototipo
				return prototype;
		}

		/// <summary>
		///		Obtiene el prototipo de un método Get o Set de una propiedad
		/// </summary>
		private string GetPropertyMethodPrototype(LanguageStructModel.ModifierType modifier, MethodModel method, string methodName)
		{
			string prototype = string.Empty;

				// Añade los datos al prototipo
				if (method != null)
				{ 
					// Añade el modificador (si es distinto al de la propiedad, es decir: public Property { private set }
					if (modifier != method.Modifier)
						prototype += GetModifierText(method.Modifier) + " ";
					// Añade el nombre
					prototype = methodName + ";";
				}
				// Devuelve el prototipo
				return prototype;
		}

		/// <summary>
		///		Obtiene las definiciones de genéricos
		/// </summary>
		private string GetMethodPrototypeGenerics(List<TypeParameterModel> typeParameters)
		{
			string prototype = "";

				// Añade las definiciones de parámetros
				if (typeParameters != null && typeParameters.Count > 0)
				{ 
					// Apertura
					prototype += "<";
					// Añade los parámetros
					for (int index = 0; index < typeParameters.Count; index++)
					{ 
						// Añade el nombre
						prototype += typeParameters[index].Name;
						// Añade el separador
						if (index < typeParameters.Count - 1)
							prototype += ", ";
					}
					// Cierre
					prototype += ">";
				}
				// Devuelve los parámetros
				return prototype;
		}

		/// <summary>
		///		Obtiene los argumentos para un prototipo
		/// </summary>
		private string GetArgumentsPrototype(BaseMethodModel method)
		{
			string prototype = "";

				// Añade la cadena con los argumentos
				for (int index = 0; index < method.Arguments.Count; index++)
				{ 
					// Añade los nodos de los argumentos
					prototype += GetArgumentData(method.Arguments[index]);
					// Añade un nodo de separación
					if (index < method.Arguments.Count - 1)
						prototype += ", ";
				}
				// Devuelve los argumentos
				return prototype;
		}

		/// <summary>
		///		Obtiene los datos de un argumento
		/// </summary>
		private string GetArgumentData(ArgumentModel argument)
		{
			string prototype = "";

				// Añade los datos del argumento
				if (argument.IsThis)
					prototype += " this";
				// Añade el tipo de referencia
				switch (argument.RefType)
				{
					case ArgumentModel.ArgumentType.ByOut:
							prototype += " out";
						break;
					case ArgumentModel.ArgumentType.ByRef:
							prototype += " ref";
						break;
				}
				// Añade el valor que indica si es un array de parámetros
				if (argument.IsParams)
					prototype += " params";
				// Añade el tipo
				prototype += " " + GetLinkTypeName(argument.Type);
				// Añade el nombre del argumento
				prototype += " " + argument.Name;
				// Si tiene un valor por defecto, lo añade
				if (argument.IsOptional)
					prototype += " = " + argument.Default;
				// Devuelve la cadena del argumento
				return prototype.TrimIgnoreNull().Replace("  ", " ");
		}

		/// <summary>
		///		Obtiene el vínculo al nombre nombre del tipo
		/// </summary>
		private string GetLinkTypeName(TypedModel typeDef)
		{
			string type = typeDef.Name;

			// Si se trata de un array, añade los datos
			if (typeDef.IsArray)
				type = type + "[]";
			// Devuelve la cadena del tipo
			return type;
		}

		/// <summary>
		///		Conversión de comentarios
		/// </summary>
		private string ConvertRemarks(string remarks)
		{ 
			// Quita los see, seealso... 
			remarks = remarks.ReplaceWithStringComparison("<", "&lt;");
			remarks = remarks.ReplaceWithStringComparison(">", "&gt;");
			remarks = remarks.ReplaceWithStringComparison("\"", "'");
			// Devuelve los comentarios
			return remarks;
		}

		/// <summary>
		///		Obtiene el texto del modificador
		/// </summary>
		private string GetModifierText(LanguageStructModel.ModifierType modifier)
		{
			switch (modifier)
			{
				case LanguageStructModel.ModifierType.Private:
					return "private";
				case LanguageStructModel.ModifierType.Protected:
					return "protected";
				case LanguageStructModel.ModifierType.Internal:
					return "internal";
				case LanguageStructModel.ModifierType.ProtectedAndInternal:
				case LanguageStructModel.ModifierType.ProtectedOrInternal:
					return "protected internal";
				case LanguageStructModel.ModifierType.Public:
					return "public";
				default:
					return string.Empty;
			}
		}
	}
}
