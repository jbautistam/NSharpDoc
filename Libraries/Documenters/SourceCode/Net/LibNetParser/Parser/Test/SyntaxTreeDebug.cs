using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bau.Libraries.LibNetParser.Parser.Test
{
	/// <summary>
	///		Intérprete de nodos
	/// </summary>
	public class SyntaxTreeDebug
	{
		/// <summary>
		///		Interpreta el contenido de un texto
		/// </summary>
		public void GetSyntaxTree(string text)
		{ 
			// Obtiene el árbol sintáctico
			Tree = CSharpSyntaxTree.ParseText(text);
			// Obtiene una cadena con los nodos
			ParseNodes(Tree.GetRoot(), 0);
		}

		/// <summary>
		///		Interpreta los nodos
		/// </summary>
		private void ParseNodes(SyntaxNode root, int indent)
		{ 
			// Imprime la información del nodo
			ParsedText.AppendLine(new string('\t', indent) + root.Kind());
			// Imprime la información de los nodos
			foreach (SyntaxNode node in root.ChildNodes())
				ParseNodes(node, indent + 1);
		}

		/// <summary>
		///		Arbol sintáctico
		/// </summary>
		public SyntaxTree Tree { get; private set; }

		/// <summary>
		///		Texto interpretado
		/// </summary>
		public System.Text.StringBuilder ParsedText { get; set; } = new System.Text.StringBuilder();
	}
}
