using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bau.Libraries.LibDBSchema.DataSchema 
{
	/// <summary>
	///		Clase con los datos de un trigger
	/// </summary>
	public class SchemaTrigger : SchemaItem
	{ 
		public SchemaTrigger(Schema objParent) : base(objParent)
		{
		}

		/// <summary>
		///		Tabla a la que se asocia el trigger
		/// </summary>
		public string Table { get; set; }
		
		/// <summary>
		///		Nombre del usuario
		/// </summary>
		public string UserName { get; set; }
		
		/// <summary>
		///		Categoría
		/// </summary>
		public int Category { get; set; }
		
    public bool IsExecuted { get; set; }
    
    public bool IsExecutionAnsiNullsOn { get; set; }
    
    public bool IsExecutionQuotedIdentOn { get; set; }
    
    public bool IsAnsiNullsOn { get; set; }
    
    public bool IsQuotedIdentOn { get; set; }
    
    public bool IsExecutionAfterTrigger { get; set; }
    
    public bool IsExecutionDeleteTrigger { get; set; }
    
    public bool IsExecutionFirstDeleteTrigger { get; set; }
    
    public bool IsExecutionFirstInsertTrigger { get; set; }
    
    public bool IsExecutionFirstUpdateTrigger { get; set; }
    
    public bool IsExecutionInsertTrigger { get; set; }
    
    public bool IsExecutionInsteadOfTrigger { get; set; }
    
    public bool IsExecutionLastDeleteTrigger { get; set; }
    
    public bool IsExecutionLastInsertTrigger { get; set; }
    
    public bool IsExecutionLastUpdateTrigger { get; set; }
    
    public bool IsExecutionTriggerDisabled { get; set; }
    
    public bool IsExecutionUpdateTrigger { get; set; }
    
    public DateTime? DateCreate { get; set; }
    
		public DateTime? DateReference { get; set; }
		
		public string Content { get; set; }
	}
}
