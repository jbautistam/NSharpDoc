using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;

namespace Bau.Libraries.LibNetParser.Parser
{
	/// <summary>
	///		Parser de código fuente en VisualBasic utilizando Roslyn
	/// </summary>
	internal class VisualBasicParser : NetParser
	{   
		// Variables privadas
		private SemanticModel treeSemantic;

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		internal override CompilationUnitModel ParseText(string fileName, string text)
		{
			CompilationUnitModel unit = new CompilationUnitModel(fileName);
			VisualBasicCompilation compilation;
	
				// Crea el modelo de compilación
				compilation = VisualBasicCompilation.Create("ParserText").AddSyntaxTrees(VisualBasicSyntaxTree.ParseText(text));
				// Obtiene el árbol semántico
				treeSemantic = compilation.GetSemanticModel(compilation.SyntaxTrees[0], true);
				// Interpreta los nodos
				ParseNodes(unit, treeSemantic.SyntaxTree.GetRoot());
				// Devuelve la unidad de compilación
				return unit;
		}

		/// <summary>
		///		Interpreta la unidad de compilación
		/// </summary>
		private void ParseNodes(CompilationUnitModel unit, SyntaxNode root)
		{
			if (root.Kind() == SyntaxKind.CompilationUnit)
				ParseChilds(root, unit.Root);
			else
				unit.Root.Error = "No se encuentra el nodo de unidad de compilación en el árbol sintáctico";
		}

		/// <summary>
		///		Interpreta los datos de un nodo
		/// </summary>
		protected override void ParseNode(SyntaxNode node, LanguageStructModel parent)
		{
			switch (node.Kind())
			{
				case SyntaxKind.SimpleImportsClause:
						ParseUsing(node, parent);
					break;
				case SyntaxKind.NamespaceBlock:
						ParseNameSpace(node, parent);
					break;
				case SyntaxKind.ClassBlock:
						ParseClass(node, parent);
					break;
				case SyntaxKind.ConstructorBlock:
						ParseConstructor(node, parent);
					break;
				case SyntaxKind.FunctionBlock:
				case SyntaxKind.SubBlock:
						ParseMethod(node, parent);
					break;
				case SyntaxKind.PropertyBlock:
						ParseProperty(node, parent);
					break;
				case SyntaxKind.EnumBlock:
						ParseEnum(node, parent);
					break;
				case SyntaxKind.EnumMemberDeclaration:
						ParseEnumMember(node, parent);
					break;
				case SyntaxKind.StructureBlock:
						ParseStruct(node, parent);
					break;
				case SyntaxKind.InterfaceBlock:
						ParseInterface(node, parent);
					break;
			}
		}

		/// <summary>
		///		Interpreta una cláusula using
		/// </summary>
		private void ParseUsing(SyntaxNode node, LanguageStructModel parent)
		{
			SimpleImportsClauseSyntax usingDef = node as SimpleImportsClauseSyntax;

				// Añade el nombre a la lista de cadenas "Using"
				if (usingDef != null)
					AddUsing(node, parent, usingDef.Name.ToFullString());
		}

		/// <summary>
		///		Interpreta un espacio de nombres
		/// </summary>
		private void ParseNameSpace(SyntaxNode node, LanguageStructModel parent)
		{
			ParseNameSpace(node, parent, treeSemantic.GetDeclaredSymbol(node as NamespaceBlockSyntax));
		}

		/// <summary>
		///		Interpreta una clase
		/// </summary>
		private void ParseClass(SyntaxNode node, LanguageStructModel parent)
		{
			ParseClass(node, parent, treeSemantic.GetDeclaredSymbol(node as ClassBlockSyntax));
		}

		/// <summary>
		///		Interpreta un interface
		/// </summary>
		private void ParseInterface(SyntaxNode node, LanguageStructModel parent)
		{
			ParseInterface(node, parent, treeSemantic.GetDeclaredSymbol(node as InterfaceBlockSyntax));
		}

		/// <summary>
		///		Interpreta los datos de una estructura
		/// </summary>
		private void ParseStruct(SyntaxNode node, LanguageStructModel parent)
		{
			ParseStruct(node, parent, treeSemantic.GetDeclaredSymbol(node as StructureBlockSyntax));
		}

		/// <summary>
		///		Interpreta un método
		/// </summary>
		private void ParseMethod(SyntaxNode node, LanguageStructModel parent)
		{
			ParseMethod(node, parent, treeSemantic.GetDeclaredSymbol(node as MethodBlockSyntax));
		}

		/// <summary>
		///		Interpreta un constructor
		/// </summary>
		private void ParseConstructor(SyntaxNode node, LanguageStructModel parent)
		{
			ParseConstructor(node, parent, treeSemantic.GetDeclaredSymbol(node as ConstructorBlockSyntax));
		}

		/// <summary>
		///		Interpreta una propiedad
		/// </summary>
		private void ParseProperty(SyntaxNode node, LanguageStructModel parent)
		{
			ParseProperty(node, parent, treeSemantic.GetDeclaredSymbol(node as PropertyBlockSyntax));
		}

		/// <summary>
		///		Interpreta un enumerado
		/// </summary>
		private void ParseEnum(SyntaxNode node, LanguageStructModel parent)
		{
			ParseEnum(node, parent, treeSemantic.GetDeclaredSymbol(node as EnumBlockSyntax));
		}

		/// <summary>
		///		Interpreta un miembro de un enumerado
		/// </summary>
		private void ParseEnumMember(SyntaxNode node, LanguageStructModel parent)
		{
			ParseEnumMember(node, parent, treeSemantic.GetDeclaredSymbol(node as EnumMemberDeclarationSyntax));
		}
	}
}
