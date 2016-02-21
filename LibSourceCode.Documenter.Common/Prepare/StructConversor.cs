using System;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;
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
		internal StructDocumentationModelCollection Convert(NameSpaceGroupModelCollection objColGroups)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Convierte los grupos
					foreach (NameSpaceGroupModel objGroup in objColGroups)
						objColStructs.Add(Convert(objGroup));
				// Devuelve la colección de estructuras
					return objColStructs;
		}

		/// <summary>
		///		Convierte un espacio de nombres en una estructura
		/// </summary>
		private StructDocumentationModel Convert(NameSpaceGroupModel objGroup)
		{ StructDocumentationModel objStruct = Create(null, objGroup.NameSpace, "NameSpace");

				// Añade los datos
					objStruct.Childs.AddRange(Convert(objStruct, objGroup.NameSpace.Items));
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Convierte una serie de elementos en estructuras
		/// </summary>
		private StructDocumentationModelCollection Convert(StructDocumentationModel objStruct, LanguageStructModelCollection objColItems)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Convierte los elementos
					foreach (LanguageStructModel objItem in objColItems)
						switch (objItem.IDType)
							{	case LanguageStructModel.StructType.Class:
										objColStructs.Add(ConvertClass(objStruct, objItem as ClassModel));
									break;
								case LanguageStructModel.StructType.CompilationUnit:
										objColStructs.AddRange(Convert(objStruct, objItem.Items));
									break;
								case LanguageStructModel.StructType.Constructor:
										objColStructs.Add(ConvertConstructor(objStruct, objItem as ConstructorModel));
									break;
								case LanguageStructModel.StructType.Enum:
										objColStructs.Add(ConvertEnum(objStruct, objItem as EnumModel));
									break;
								case LanguageStructModel.StructType.EnumMember:
										objColStructs.Add(ConvertEnumMember(objStruct, objItem as EnumMemberModel));
									break;
								case LanguageStructModel.StructType.Interface:
										objColStructs.Add(ConvertInterface(objStruct, objItem as InterfaceModel));
									break;
								case LanguageStructModel.StructType.Method:
										objColStructs.Add(ConvertMethod(objStruct, objItem as MethodModel));
									break;
								case LanguageStructModel.StructType.Property:
										objColStructs.Add(ConvertProperty(objStruct, objItem as PropertyModel));
									break;
								case LanguageStructModel.StructType.Struct:
										objColStructs.Add(ConvertStruct(objStruct, objItem as StructModel));
									break;
							}
				// Devuelve la colección de estructuras
					return objColStructs;
		}

		/// <summary>
		///		Convierte una interface
		/// </summary>
		private StructDocumentationModel ConvertInterface(StructDocumentationModel objParent, InterfaceModel objInterface)
		{ StructDocumentationModel objStruct = Create(objParent, objInterface, "Interface");

				// Añade los parámetros de la clase compleja base
					AddParametersBaseComplex(objStruct, objInterface);
				// Crea los elementos hijo
					objStruct.Childs.AddRange(Convert(objStruct, objInterface.Items));
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Convierte una clase
		/// </summary>
		private StructDocumentationModel ConvertClass(StructDocumentationModel objParent, ClassModel objClass)
		{ StructDocumentationModel objStruct = Create(objParent, objClass, "Class");

				// Añade los parámetros de la clase compleja base
					AddParametersBaseComplex(objStruct, objClass);
				// Añade los parámetros
					objStruct.Parameters.Add("IsSealed", objClass.IsSealed.ToString());
					objStruct.Parameters.Add("IsStatic", objClass.IsStatic.ToString());
					objStruct.Parameters.Add("IsAbstract", objClass.IsAbstract.ToString());
				// Añade los elementos hijo
					objStruct.Childs.AddRange(Convert(objStruct, objClass.Items));
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Convierte una estructura
		/// </summary>
		private StructDocumentationModel ConvertStruct(StructDocumentationModel objParent, StructModel objItem)
		{ StructDocumentationModel objStruct = Create(objParent, objItem, "Struct");

				// Añade los parámetros de la clase compleja base
					AddParametersBaseComplex(objStruct, objItem);
				// Crea los elementos hijo
					objStruct.Childs.AddRange(Convert(objStruct, objItem.Items));
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Añade los parámetros de un elemento complejo
		/// </summary>
		private void AddParametersBaseComplex(StructDocumentationModel objStruct, BaseComplexModel objComplex)
		{ LanguageStructModel objNameSpace = objComplex.GetNameSpace();

				// Añade la clase base
					objStruct.Childs.Add(GetTypeStruct(objStruct, "BaseClass", objComplex.BaseClass));
					objStruct.Parameters.Add(GetTypeStructShort("BaseType", objComplex.BaseClass));
				// Añade las interfaces
					foreach (string strInterface in objComplex.Interfaces)
						objStruct.Parameters.Add("Interface", strInterface);
				// Añade los parámetros tipados
					AddListTypeParameter(objStruct, objComplex.TypeParameters);
		}

		/// <summary>
		///		Obtiene un parámetro para la lista de tipo de parámetros
		/// </summary>
		private void AddListTypeParameter(StructDocumentationModel objParent, List<TypeParameterModel> objColTypeParameters)
		{ if (objColTypeParameters != null && objColTypeParameters.Count > 0)
				foreach (TypeParameterModel objTypeParameter in objColTypeParameters)
					{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent, StructDocumentationModel.ScopeType.Global,
																																							objTypeParameter.Name, "TypeParameter", 0);
							
							// Añade el tipo
								objStruct.Childs.Add(GetTypeStruct(objStruct, "Type", objTypeParameter.Type));
							// Añade la estructura al padre
								objParent.Childs.Add(objStruct);
					}
		}

		/// <summary>
		///		Añade un parámetro con la lista de argumentos
		/// </summary>
		private void AddListArguments(StructDocumentationModel objParent, BaseMethodModel objMethod, List<ArgumentModel> objColArguments)
		{ if (objColArguments != null && objColArguments.Count > 0)
				foreach (ArgumentModel objArgument in objColArguments)
					{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent, StructDocumentationModel.ScopeType.Global, objArgument.Name, "Argument", 0);

							// Añade el tipo
								objStruct.Childs.Add(GetTypeStruct(objStruct, "TypeStruct", objArgument.Type));
							// Añade los parámetros del argumento
								objStruct.Parameters.Add(GetTypeStructShort("Type", objArgument.Type));
								objStruct.Parameters.Add("RefType", objArgument.RefType.ToString());
								objStruct.Parameters.Add("IsOptional", objArgument.IsOptional);
								objStruct.Parameters.Add("IsParams", objArgument.IsParams);
								objStruct.Parameters.Add("IsThis", objArgument.IsThis);
								objStruct.Parameters.Add("Default", objArgument.Default);
								objStruct.Parameters.Add("Summary", ConvertRemarks(objMethod.RemarksXml.GetParameterRemarks(objArgument.Name)));
							// Añade la estrucutra al padre
								objParent.Childs.Add(objStruct);
					}
		}

		/// <summary>
		///		Obtiene un parámetro con los datos de un tipo (para la definición corta del tipo)
		/// </summary>
		private StructParameterModel GetTypeStructShort(string strName, TypedModel objType)
		{ if (objType != null)
				return new StructParameterModel(strName, objType.Name, objType.Name);
			else
				return new StructParameterModel(strName, "", "");
		}

		/// <summary>
		///		Obtiene el parámetro de un tipo
		/// </summary>
		private StructDocumentationModel GetTypeStruct(StructDocumentationModel objParent, string strName, TypedModel objType)
		{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent, StructDocumentationModel.ScopeType.Global,
																																				strName, "Type", 0);

				// Si realmente se le pasa un tipo
					if (objType != null)
						{ // Asigna los datos del tipo
								objStruct.Childs.Add(GetTypeStruct(objParent, "BaseTypeStruct", objType.BaseType));
								objStruct.Parameters.Add(GetTypeStructShort("BaseType", objType.BaseType));
								objStruct.Parameters.Add("NameSpace", objType.NameSpace);
								objStruct.Parameters.Add("IsVoid", objType.IsVoid);
								objStruct.Parameters.Add("IsArray", objType.IsArray);
								objStruct.Parameters.Add("Dimensions", objType.Dimensions);
							// Añade las restricciones
								AddListConstraints(objStruct, objType.Constraints);
						}
				// Devuelve el parámetro
					return objStruct;
		}

		/// <summary>
		///		Añade una lista de restricciones a los parámetros
		/// </summary>
		private void AddListConstraints(StructDocumentationModel objParent, List<TypedModel> objColConstraints)
		{ if (objColConstraints != null && objColConstraints.Count > 0)
				foreach (TypedModel objConstraint in objColConstraints)
					{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent, StructDocumentationModel.ScopeType.Global,
																																							objConstraint.Name, "Constraint", 0);

							// Añade los parámetros
								objStruct.Childs.Add(GetTypeStruct(objStruct, "ConstraintStruct", objConstraint));
								objStruct.Parameters.Add(GetTypeStructShort("Type", objConstraint));
							// Añade la restricción al padre al parámetro
								objParent.Childs.Add(objStruct);
					}
		}

		/// <summary>
		///		Convierte un enumerado
		/// </summary>
		private StructDocumentationModel ConvertEnum(StructDocumentationModel objParent, EnumModel objItem)
		{ StructDocumentationModel objStruct = Create(objParent, objItem, "Enum");

				// Crea los elementos hijo
					objStruct.Childs.AddRange(Convert(objStruct, objItem.Items));
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Convierte un miembro de enumerado
		/// </summary>
		private StructDocumentationModel ConvertEnumMember(StructDocumentationModel objParent, EnumMemberModel objItem)
		{ return Create(objParent, objItem, "EnumMember");
		}

		/// <summary>
		///		Convierte un constructor
		/// </summary>
		private StructDocumentationModel ConvertConstructor(StructDocumentationModel objParent, ConstructorModel objItem)
		{ StructDocumentationModel objStruct = Create(objParent, objItem, "Constructor");

				// Añade los argumentos
					objStruct.Parameters.Add(GetPrototype(objItem));
					AddListArguments(objStruct, objItem, objItem.Arguments);
					AddListTypeParameter(objStruct, objItem.TypeParameters);
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Convierte un método
		/// </summary>
		private StructDocumentationModel ConvertMethod(StructDocumentationModel objParent, MethodModel objItem)
		{ StructDocumentationModel objStruct = Create(objParent, objItem, "Method");

				// Añade los argumentos
					objStruct.Parameters.Add(GetPrototype(objItem));
					AddListArguments(objStruct, objItem, objItem.Arguments);
					AddListTypeParameter(objStruct, objItem.TypeParameters);
				// Añade el valor de retorno
					objStruct.Childs.Add(GetReturnValue(objStruct, objItem.ReturnType, 
																							ConvertRemarks(objItem.RemarksXml.Returns)));
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Obtiene el valor de retorno
		/// </summary>
		private StructDocumentationModel GetReturnValue(StructDocumentationModel objParent, TypedModel objType, string strRemarks)
		{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent, StructDocumentationModel.ScopeType.Global, 
																																				"ReturnValue", "ReturnValue", 0);

				// Añade el tipo
					objStruct.Childs.Add(GetTypeStruct(objStruct, "ReturnValueStruct", objType));
					objStruct.Parameters.Add(GetTypeStructShort("Type", objType));
				// Añade los valores
					objStruct.Parameters.Add("Summary", ConvertRemarks(strRemarks));
				// Devuelve el parámetro
					return objStruct;
		}

		/// <summary>
		///		Convierte una propiedad
		/// </summary>
		private StructDocumentationModel ConvertProperty(StructDocumentationModel objParent, PropertyModel objItem)
		{ StructDocumentationModel objStruct = Create(objParent, objItem, "Property");

				// Añade los argumentos
					objStruct.Parameters.Add(GetPrototype(objItem));
					AddListTypeParameter(objStruct, objItem.TypeParameters);
					AddListArguments(objStruct, objItem, objItem.Arguments);
				// Añade los métodos get y set
					ConvertPropertyMethod(objStruct, objItem.GetMethod, "get");
					ConvertPropertyMethod(objStruct, objItem.SetMethod, "set");
				// Añade el valor de retorno
					if (objItem.GetMethod != null)
						objStruct.Childs.Add(GetReturnValue(objStruct, objItem.GetMethod.ReturnType, 
																								ConvertRemarks(objItem.GetMethod.RemarksXml.Returns)));
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Convierte un método de una propiedad
		/// </summary>
		private void ConvertPropertyMethod(StructDocumentationModel objParent, MethodModel objMethod, string strName)
		{	if (objMethod != null)
				{ StructDocumentationModel objStruct = ConvertMethod(objParent, objMethod);

						// Cambia el nombre
							objStruct.Name = strName;
						// ... y se lo añade a la colección
							objParent.Childs.Add(objStruct);
				}
		}

		/// <summary>
		///		Crea una estructura de documentación
		/// </summary>
		private StructDocumentationModel Create(StructDocumentationModel objParent, LanguageStructModel objItem, string strType)
		{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent, GetScope(objItem), objItem.Name, 
																																				strType, objItem.Order);
			LanguageStructModel objNameSpace = objItem.GetNameSpace();

				// Añade el espacio de nombres
					if (objNameSpace != null)
						objStruct.Parameters.Add("NameSpace", objNameSpace.Name, objNameSpace.Name);
				// Añade los parámetros básicos
					objStruct.Parameters.Add("Summary", ConvertRemarks(objItem.RemarksXml.Summary));
					objStruct.Parameters.Add("Remarks", ConvertRemarks(objItem.RemarksXml.Remarks));
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Obtiene el ámbito
		/// </summary>
		private StructDocumentationModel.ScopeType GetScope(LanguageStructModel objStruct)
		{ switch (objStruct.Modifier)
				{	case LanguageStructModel.ModifierType.Internal:
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
		private StructParameterModel GetPrototype(LanguageStructModel objStruct)
		{ string strPrototype = "";

				// Obtiene los datos del prototipo
					switch (objStruct.IDType)
						{	case LanguageStructModel.StructType.Property:
									strPrototype = GetPropertyPrototype(objStruct as PropertyModel);
								break;
							case LanguageStructModel.StructType.Constructor:
									strPrototype = GetMethodPrototype(objStruct as ConstructorModel);
								break;
							case LanguageStructModel.StructType.Method:
									MethodModel objMethod = objStruct as MethodModel;
										
										if (objMethod != null)
											strPrototype = GetMethodPrototype(objMethod, objMethod.IsAsync, objMethod.ReturnType);
								break;
						}
				// Devuelve el nodo
					return new StructParameterModel("Prototype", strPrototype, null);
		}

		/// <summary>
		///		Obtiene el prototipo de una propiedad
		/// </summary>
		private string GetPropertyPrototype(PropertyModel objProperty)
		{ string strPrototype = GetMethodPrototypeDefinition(objProperty, false, objProperty.GetMethod.ReturnType);

				// Argumentos
					if (objProperty.Arguments.Count > 0)
						strPrototype += "[" + GetArgumentsPrototype(objProperty) + "]";
				// Get y set
					strPrototype += " { ";
					strPrototype += GetPropertyMethodPrototype(objProperty.Modifier, objProperty.GetMethod, "get");
					strPrototype += " " + GetPropertyMethodPrototype(objProperty.Modifier, objProperty.SetMethod, "set");
					strPrototype += " }";
				// Devuelve el prototipo
					return strPrototype;
		}

		/// <summary>
		///		Obtiene el prototipo de una función
		/// </summary>
		private string GetMethodPrototype(BaseMethodModel objMethod, bool blnIsAsync = false, TypedModel objReturnType = null)
		{	string strPrototype = GetMethodPrototypeDefinition(objMethod, blnIsAsync, objReturnType);

				// Añade los argumentos
					strPrototype += "(" + GetArgumentsPrototype(objMethod) + ")";
				// Devuelve el prototipo
					return strPrototype;
		}

		/// <summary>
		///		Obtiene la definición (el prefijo) de un prototipo de un método
		/// </summary>
		private string GetMethodPrototypeDefinition(BaseMethodModel objMethod, bool blnIsAsync, TypedModel objReturnType)
		{ string strPrototype = "";
		
				// Añade los modificadores
					strPrototype += GetModifierText(objMethod.Modifier);
					if (blnIsAsync)
						strPrototype += " async";
					if (objMethod.IsAbstract)
						strPrototype += " abstract";
					if (objMethod.IsOverride)
						strPrototype += " override";
					if (objMethod.IsSealed)
						strPrototype += " sealed";
					if (objMethod.IsStatic)
						strPrototype += " static";
					if (objMethod.IsVirtual)
						strPrototype += " virtual";
				// Añade el valor de retorno
					if (objReturnType != null)
						strPrototype += " " + GetLinkTypeName(objReturnType);
				// Añade el nombre del método
					strPrototype += " " + objMethod.Name;
				// Añade los genéricos
					strPrototype += GetMethodPrototypeGenerics(objMethod.TypeParameters);
				// Devuelve la definición de prototipo
					return strPrototype;
		}

		/// <summary>
		///		Obtiene el prototipo de un método Get o Set de una propiedad
		/// </summary>
		private string GetPropertyMethodPrototype(LanguageStructModel.ModifierType intModifier, MethodModel objMethod, string strMethodName)
		{	string strPrototype = "";

				// Añade los datos al prototipo
					if (objMethod != null)
						{ // Añade el modificador (si es distinto al de la propiedad, es decir: public Property { private set }
								if (intModifier != objMethod.Modifier)
									strPrototype += GetModifierText(objMethod.Modifier) + " ";
							// Añade el nombre
								strPrototype = strMethodName + ";";
						}
				// Devuelve el prototipo
					return strPrototype;
		}

		/// <summary>
		///		Obtiene las definiciones de genéricos
		/// </summary>
		private string GetMethodPrototypeGenerics(List<TypeParameterModel> objColTypeParameters)
		{ string strPrototype = "";

				// Añade las definiciones de parámetros
					if (objColTypeParameters != null && objColTypeParameters.Count > 0)
						{ // Apertura
								strPrototype += "<";
							// Añade los parámetros
								for (int intIndex = 0; intIndex < objColTypeParameters.Count; intIndex++)
									{ // Añade el nombre
											strPrototype += objColTypeParameters[intIndex].Name;
										// Añade el separador
											if (intIndex < objColTypeParameters.Count - 1)
												strPrototype += ", ";
									}
							// Cierre
								strPrototype += ">";
						}
				// Devuelve los parámetros
					return strPrototype;
		}

		/// <summary>
		///		Obtiene los argumentos para un prototipo
		/// </summary>
		private string GetArgumentsPrototype(BaseMethodModel objMethod)
		{ string strPrototype = "";

				// Añade la cadena con los argumentos
					for (int intIndex = 0; intIndex < objMethod.Arguments.Count; intIndex++)
						{ // Añade los nodos de los argumentos
								strPrototype += GetArgumentData(objMethod.Arguments[intIndex]);
							// Añade un nodo de separación
								if (intIndex < objMethod.Arguments.Count - 1)
									strPrototype += ", ";
						}
				// Devuelve los argumentos
					return strPrototype;
		}

		/// <summary>
		///		Obtiene los datos de un argumento
		/// </summary>
		private string GetArgumentData(ArgumentModel objArgument)
		{ string strPrototype = "";

				// Añade los datos del argumento
					if (objArgument.IsThis)
 						strPrototype += " this";
				// Añade el tipo de referencia
					switch (objArgument.RefType)
						{	case ArgumentModel.ArgumentType.ByOut:
									strPrototype += " out";
								break;
							case ArgumentModel.ArgumentType.ByRef:
									strPrototype += " ref";
								break;
						}
				// Añade el valor que indica si es un array de parámetros
					if (objArgument.IsParams)
						strPrototype += " params";
				// Añade el tipo
					strPrototype += " " + GetLinkTypeName(objArgument.Type);
				// Añade el nombre del argumento
					strPrototype += " " + objArgument.Name;
				// Si tiene un valor por defecto, lo añade
					if (objArgument.IsOptional)
						strPrototype += " = " + objArgument.Default;
				// Devuelve la cadena del argumento
					return strPrototype.TrimIgnoreNull().Replace("  ", " ");
		}

		/// <summary>
		///		Obtiene el vínculo al nombre nombre del tipo
		/// </summary>
		private string GetLinkTypeName(TypedModel objType)
		{ string strType = objType.Name;

				// Si se trata de un array, añade los datos
					if (objType.IsArray)
						strType = strType + "[]";
				// Devuelve la cadena del tipo
					return strType;
		}

		/// <summary>
		///		Conversión de comentarios
		/// </summary>
		private string ConvertRemarks(string strRemarks)
		{ // Quita los see, seealso... 
				strRemarks = strRemarks.ReplaceWithStringComparison("<", "&lt;");
				strRemarks = strRemarks.ReplaceWithStringComparison(">", "&gt;");
				strRemarks = strRemarks.ReplaceWithStringComparison("\"", "'");
			// Devuelve los comentarios
				return strRemarks;
		}

		/// <summary>
		///		Obtiene el texto del modificador
		/// </summary>
		private string GetModifierText(LanguageStructModel.ModifierType intIDModifier)
		{ switch (intIDModifier)
				{	case LanguageStructModel.ModifierType.Private:
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
						return "";
				}
		}
	}
}
