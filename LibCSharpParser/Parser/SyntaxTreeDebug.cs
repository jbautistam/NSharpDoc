using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bau.Libraries.LibCSharpParser.Parser
{
	/// <summary>
	///		Intérprete de nodos
	/// </summary>
	public class SyntaxTreeDebug
	{ 
		/// <summary>
		///		Interpreta el contenido de un texto
		/// </summary>
		public void GetSyntaxTree(string strText)
		{ // Obtiene el árbol sintáctico
				Tree = CSharpSyntaxTree.ParseText(strText);
			// Obtiene una cadena con los nodos
				ParseNodes(Tree.GetRoot(), 0);
		}

		/// <summary>
		///		Interpreta los nodos
		/// </summary>
		private void ParseNodes(SyntaxNode objRoot, int intIndent)
		{ // Imprime la información del nodo
				ParsedText.AppendLine(new String('\t', intIndent) + objRoot.Kind());
			// Imprime la información de los nodos
				foreach (SyntaxNode objNode in objRoot.ChildNodes())
					ParseNodes(objNode, intIndent + 1);
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
