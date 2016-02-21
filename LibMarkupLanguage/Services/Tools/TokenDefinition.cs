using System;

namespace Bau.Libraries.LibMarkupLanguage.Services.Tools
{
	/// <summary>
	///		Definición de un token
	/// </summary>
	internal class TokenDefinition : IComparable<TokenDefinition>
	{
		internal TokenDefinition(int? intType, string strName) : this(intType, strName, null, null)
		{
		}

		internal TokenDefinition(int? intType, string strName, string strStart) : this(intType, strName, strStart, null)
		{
		}
		
		internal TokenDefinition(int? intType, string strName, string strStart, string strEnd)
		{ Type = intType;
			Name = strName;
			Start = strStart;
			End = strEnd;
		}

		/// <summary>
		///		Compara con otra definíción de contenido
		/// </summary>
		public int CompareTo(TokenDefinition objDefinition)
		{ if (objDefinition == null)
				return -1;
			else if (Start == objDefinition.Start)
				if (End == null)
					return -1;
				else
					return End.CompareTo(objDefinition.End);
			else
				return Start.CompareTo(objDefinition.Start);
		}
		
		/// <summary>
		///		Tipo de token
		/// </summary>
		internal int? Type { get; set; }
		
		/// <summary>
		///		Nombre del token
		/// </summary>
		internal string Name { get; set; }
		
		/// <summary>
		///		Indica si es un token sobre datos numéricos
		/// </summary>
		internal bool IsNumeric 
		{ get { return string.IsNullOrEmpty(Start); }
		}

		/// <summary>
		///		Cadena de inicio
		/// </summary>
		internal string Start { get; set; }
		
		/// <summary>
		///		Cadena de fin
		/// </summary>
		internal string End { get; set; }
	}
}
