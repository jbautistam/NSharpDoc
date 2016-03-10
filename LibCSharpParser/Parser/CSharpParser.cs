using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;

namespace Bau.Libraries.LibCSharpParser.Parser
{
	/// <summary>
	///		Parser de código fuente en C# utilizando Roslyn
	/// </summary>
	internal class CSharpParser : LibNetParser.Common.Parser.NetParser
	{	// Variables privadas
			private SemanticModel objTreeSemantic;

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		protected override CompilationUnitModel ParseText(string strFileName, string strText)
		{ CompilationUnitModel objUnit = new CompilationUnitModel(strFileName);
			CSharpCompilation objCompilation;

				// Crea el modelo de compilación
					objCompilation = CSharpCompilation.Create("ParserText").AddSyntaxTrees(CSharpSyntaxTree.ParseText(strText));
				// Obtiene el árbol semántico
					objTreeSemantic = objCompilation.GetSemanticModel(objCompilation.SyntaxTrees[0], true);
				// Interpreta los nodos
					ParseNodes(objUnit, objTreeSemantic.SyntaxTree.GetRoot());
				// Devuelve la unidad de compilación
					return objUnit;
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
		///		Interpreta los datos de un nodo
		/// </summary>
		protected override void ParseNode(SyntaxNode objNode, LanguageStructModel objParent)
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
					case SyntaxKind.AttributeList:
							ParseAttributeList(objNode, objParent);
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
						base.AddUsing(objNode, objParent, objUsing.Name.ToFullString());
		}

		/// <summary>
		///		Interpreta un espacio de nombres
		/// </summary>
		private void ParseNameSpace(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseNameSpace(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as NamespaceDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta una clase
		/// </summary>
		private void ParseClass(SyntaxNode objNode, LanguageStructModel objParent)
		{	base.ParseClass(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as ClassDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un interface
		/// </summary>
		private void ParseInterface(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseInterface(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as InterfaceDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta los datos de una estructura
		/// </summary>
		private void ParseStruct(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseStruct(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as StructDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un método
		/// </summary>
		private void ParseMethod(SyntaxNode objNode, LanguageStructModel objParent)
		{	base.ParseMethod(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as MethodDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un constructor
		/// </summary>
		private void ParseConstructor(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseConstructor(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as ConstructorDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta una propiedad
		/// </summary>
		private void ParseProperty(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseProperty(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as PropertyDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un enumerado
		/// </summary>
		private void ParseEnum(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseEnum(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as EnumDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un miembro de un enumerado
		/// </summary>
		private void ParseEnumMember(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseEnumMember(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as EnumMemberDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta una lista de atributos
		/// </summary>
		private void ParseAttributeList(SyntaxNode objNode, LanguageStructModel objParent)
		{ AttributeListSyntax objListAttributes = objNode as AttributeListSyntax;

				// Interpreta los atributos
					foreach (AttributeSyntax objAttribute in objListAttributes.Attributes)
						base.ParseAttributeList(objNode, objParent, objAttribute.ToFullString());
		}
	}
}
