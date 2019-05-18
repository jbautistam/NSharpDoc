using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base
{
	/// <summary>
	///		Base para las estructuras del lenguage
	/// </summary>
	public class LanguageStructModel : LanguageBaseModel
	{
		/// <summary>
		///		Tipo de modificador
		/// </summary>
		public enum ModifierType
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Público</summary>
			Public,
			/// <summary>Privado</summary>
			Private,
			/// <summary>Protegido</summary>
			Protected,
			/// <summary>Interno</summary>
			Internal,
			/// <summary>Protected e Internal</summary>
			ProtectedAndInternal,
			/// <summary>Protected o Internal</summary>
			ProtectedOrInternal
		}

		/// <summary>
		///		Tipo de estructura
		/// </summary>
		public enum StructType
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Unidad de compilación</summary>
			CompilationUnit,
			/// <summary>Espacio de nombres</summary>
			NameSpace,
			/// <summary>Clase</summary>
			Class,
			/// <summary>Interface</summary>
			Interface,
			/// <summary>Constructor</summary>
			Constructor,
			/// <summary>Método</summary>
			Method,
			/// <summary>Enumerado</summary>
			Enum,
			/// <summary>Miembro de un enumerado</summary>
			EnumMember,
			/// <summary>Propiedad</summary>
			Property,
			/// <summary>Estructura</summary>
			Struct
		}

		public LanguageStructModel(StructType typeId, LanguageStructModel parent, CompilationUnitModel compilationUnit = null)
		{
			Type = typeId;
			Parent = parent;
			CompilationUnit = compilationUnit;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public override string Debug(int indent)
		{
			return base.Debug(indent) +
						   new string('\t', indent) + Type +
						   " (" + RemarksXml.RawXml.ReplaceWithStringComparison(Environment.NewLine, "\\n") + ") [" + Modifier + "]" + Environment.NewLine;
		}

		/// <summary>
		///		Obtiene el espacio de nombres al que pertenece una estructura
		/// </summary>
		public LanguageStructModel GetNameSpace()
		{
			if (Type == StructType.NameSpace)
				return this;
			else if (Parent != null)
				return Parent.GetNameSpace();
			else
				return null;
		}

		/// <summary>
		///		Obtiene el espacio de nombres al que pertenece una estructura
		/// </summary>
		public string GetNameSpaceName()
		{
			LanguageStructModel nameSpace = GetNameSpace();

				// Obtiene el nombre del espacio de nombres
				if (nameSpace == null)
					return "";
				else
					return nameSpace.Name;
		}

		/// <summary>
		///		Ensamblado en el que se define la estructura
		/// </summary>
		public string Assembly { get; set; }

		/// <summary>
		///		Tipo de estructura
		/// </summary>
		public StructType Type { get; }

		/// <summary>
		///		Elemento padre
		/// </summary>
		public LanguageStructModel Parent { get; }

		/// <summary>
		///		Modificador del elemento
		/// </summary>
		public ModifierType Modifier { get; set; }

		/// <summary>
		///		Atributos asociados al elemento
		/// </summary>
		public System.Collections.Generic.List<AttributeModel> Attributes { get; } = new System.Collections.Generic.List<AttributeModel>();

		/// <summary>
		///		Comentarios XML
		/// </summary>
		public RemarksXml RemarksXml { get; set; } = new RemarksXml();

		/// <summary>
		///		Elementos hijo
		/// </summary>
		public LanguageStructModelCollection Items { get; } = new LanguageStructModelCollection();

		/// <summary>
		///		Unidad de compilación de la que sale el elemento
		/// </summary>
		public CompilationUnitModel CompilationUnit { get; }

		/// <summary>
		///		Orden del elemento
		/// </summary>
		public int Order { get; set; }
	}
}
