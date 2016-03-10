using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;

namespace Bau.Libraries.LibVisualBasicParser.Parser
{
	/// <summary>
	///		Parser de código fuente en VisualBasic utilizando Roslyn
	/// </summary>
	internal class VisualBasicParser : LibNetParser.Common.Parser.NetParser
	{	// Variables privadas
			private SemanticModel objTreeSemantic;

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		protected override CompilationUnitModel ParseText(string strFileName, string strText)
		{ CompilationUnitModel objUnit = new CompilationUnitModel(strFileName);
			VisualBasicCompilation objCompilation;

				// Crea el modelo de compilación
					objCompilation = VisualBasicCompilation.Create("ParserText").AddSyntaxTrees(VisualBasicSyntaxTree.ParseText(strText));
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
				{	case SyntaxKind.SimpleImportsClause:
							ParseUsing(objNode, objParent);
						break;
					case SyntaxKind.NamespaceBlock:
							ParseNameSpace(objNode, objParent);
						break;
					case SyntaxKind.ClassBlock:
							ParseClass(objNode, objParent);
						break;
					case SyntaxKind.ConstructorBlock:
							ParseConstructor(objNode, objParent);
						break;
					case SyntaxKind.FunctionBlock:
					case SyntaxKind.SubBlock:
							ParseMethod(objNode, objParent);
						break;
					case SyntaxKind.PropertyBlock:
							ParseProperty(objNode, objParent);
						break;
					case SyntaxKind.EnumBlock:
							ParseEnum(objNode, objParent);
						break;
					case SyntaxKind.EnumMemberDeclaration:
							ParseEnumMember(objNode, objParent);
						break;
					case SyntaxKind.StructureBlock:	
							ParseStruct(objNode, objParent);
						break;
					case SyntaxKind.InterfaceBlock:
							ParseInterface(objNode, objParent);
						break;
				}
		}

		/// <summary>
		///		Interpreta una cláusula using
		/// </summary>
		private void ParseUsing(SyntaxNode objNode, LanguageStructModel objParent)
		{ SimpleImportsClauseSyntax objUsing = objNode as SimpleImportsClauseSyntax;

				// Añade el nombre a la lista de cadenas "Using"
					if (objUsing != null)
						base.AddUsing(objNode, objParent, objUsing.Name.ToFullString());
		}

		/// <summary>
		///		Interpreta un espacio de nombres
		/// </summary>
		private void ParseNameSpace(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseNameSpace(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as NamespaceBlockSyntax));
		}

		/// <summary>
		///		Interpreta una clase
		/// </summary>
		private void ParseClass(SyntaxNode objNode, LanguageStructModel objParent)
		{	base.ParseClass(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as ClassBlockSyntax));
		}

		/// <summary>
		///		Interpreta un interface
		/// </summary>
		private void ParseInterface(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseInterface(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as InterfaceBlockSyntax));
		}

		/// <summary>
		///		Interpreta los datos de una estructura
		/// </summary>
		private void ParseStruct(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseStruct(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as StructureBlockSyntax));
		}

		/// <summary>
		///		Interpreta un método
		/// </summary>
		private void ParseMethod(SyntaxNode objNode, LanguageStructModel objParent)
		{	base.ParseMethod(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as MethodBlockSyntax));
		}

		/// <summary>
		///		Interpreta un constructor
		/// </summary>
		private void ParseConstructor(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseConstructor(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as ConstructorBlockSyntax));
		}

		/// <summary>
		///		Interpreta una propiedad
		/// </summary>
		private void ParseProperty(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseProperty(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as PropertyBlockSyntax));
		}

		/// <summary>
		///		Interpreta un enumerado
		/// </summary>
		private void ParseEnum(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseEnum(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as EnumBlockSyntax));
		}

		/// <summary>
		///		Interpreta un miembro de un enumerado
		/// </summary>
		private void ParseEnumMember(SyntaxNode objNode, LanguageStructModel objParent)
		{ base.ParseEnumMember(objNode, objParent, objTreeSemantic.GetDeclaredSymbol(objNode as EnumMemberDeclarationSyntax));
		}
	}
}
