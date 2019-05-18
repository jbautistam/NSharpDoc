using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base;

namespace Bau.Libraries.LibNetParser.Parser
{
	/// <summary>
	///		Parser de código fuente en C# utilizando Roslyn
	/// </summary>
	internal class CSharpParser : NetParser
	{   
		// Variables privadas
		private SemanticModel treeSemantic;

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		internal override CompilationUnitModel ParseText(string fileName, string text)
		{
			CompilationUnitModel unit = new CompilationUnitModel(fileName);
			CSharpCompilation compilation;

				// Crea el modelo de compilación
				compilation = CSharpCompilation.Create("ParserText").AddSyntaxTrees(CSharpSyntaxTree.ParseText(text));
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
				case SyntaxKind.UsingDirective:
						ParseUsing(node, parent);
					break;
				case SyntaxKind.NamespaceDeclaration:
						ParseNameSpace(node, parent);
					break;
				case SyntaxKind.ClassDeclaration:
						ParseClass(node, parent);
					break;
				case SyntaxKind.ConstructorDeclaration:
						ParseConstructor(node, parent);
					break;
				case SyntaxKind.MethodDeclaration:
						ParseMethod(node, parent);
					break;
				case SyntaxKind.PropertyDeclaration:
						ParseProperty(node, parent);
					break;
				case SyntaxKind.EnumDeclaration:
						ParseEnum(node, parent);
					break;
				case SyntaxKind.EnumMemberDeclaration:
						ParseEnumMember(node, parent);
					break;
				case SyntaxKind.StructDeclaration:
						ParseStruct(node, parent);
					break;
				case SyntaxKind.InterfaceDeclaration:
						ParseInterface(node, parent);
					break;
				case SyntaxKind.AttributeList:
						ParseAttributeList(node, parent);
					break;
			}
		}

		/// <summary>
		///		Interpreta una cláusula using
		/// </summary>
		private void ParseUsing(SyntaxNode node, LanguageStructModel parent)
		{
			UsingDirectiveSyntax directive = node as UsingDirectiveSyntax;

				// Añade el nombre a la lista de cadenas "Using"
				if (directive != null)
					AddUsing(node, parent, directive.Name.ToFullString());
		}

		/// <summary>
		///		Interpreta un espacio de nombres
		/// </summary>
		private void ParseNameSpace(SyntaxNode node, LanguageStructModel parent)
		{
			ParseNameSpace(node, parent, treeSemantic.GetDeclaredSymbol(node as NamespaceDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta una clase
		/// </summary>
		private void ParseClass(SyntaxNode node, LanguageStructModel parent)
		{
			ParseClass(node, parent, treeSemantic.GetDeclaredSymbol(node as ClassDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un interface
		/// </summary>
		private void ParseInterface(SyntaxNode node, LanguageStructModel parent)
		{
			ParseInterface(node, parent, treeSemantic.GetDeclaredSymbol(node as InterfaceDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta los datos de una estructura
		/// </summary>
		private void ParseStruct(SyntaxNode node, LanguageStructModel parent)
		{
			ParseStruct(node, parent, treeSemantic.GetDeclaredSymbol(node as StructDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un método
		/// </summary>
		private void ParseMethod(SyntaxNode node, LanguageStructModel parent)
		{
			ParseMethod(node, parent, treeSemantic.GetDeclaredSymbol(node as MethodDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un constructor
		/// </summary>
		private void ParseConstructor(SyntaxNode node, LanguageStructModel parent)
		{
			ParseConstructor(node, parent, treeSemantic.GetDeclaredSymbol(node as ConstructorDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta una propiedad
		/// </summary>
		private void ParseProperty(SyntaxNode node, LanguageStructModel parent)
		{
			ParseProperty(node, parent, treeSemantic.GetDeclaredSymbol(node as PropertyDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un enumerado
		/// </summary>
		private void ParseEnum(SyntaxNode node, LanguageStructModel parent)
		{
			ParseEnum(node, parent, treeSemantic.GetDeclaredSymbol(node as EnumDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta un miembro de un enumerado
		/// </summary>
		private void ParseEnumMember(SyntaxNode node, LanguageStructModel parent)
		{
			ParseEnumMember(node, parent, treeSemantic.GetDeclaredSymbol(node as EnumMemberDeclarationSyntax));
		}

		/// <summary>
		///		Interpreta una lista de atributos
		/// </summary>
		private void ParseAttributeList(SyntaxNode node, LanguageStructModel parent)
		{
			AttributeListSyntax objListAttributes = node as AttributeListSyntax;

				// Interpreta los atributos
				foreach (AttributeSyntax attribute in objListAttributes.Attributes)
					ParseAttributeList(node, parent, attribute.ToFullString());
		}
	}
}
