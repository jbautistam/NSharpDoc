using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibDbProviders.Base.Schema;

namespace Bau.Libraries.LibDataBaseDocumenter
{
	/// <summary>
	///		Conversor a una serie de estructuras la información de esquema de una base de datos
	/// </summary>
	public class StructDataBaseConversor
	{
		/// <summary>
		///		Tipo de base de datos
		/// </summary>
		public enum DataBaseType
		{
			/// <summary>Odbc</summary>
			Odbc,
			/// <summary>SqlServer</summary>
			SqlServer
		}

		/// <summary>
		///		Convierte un esquema en una estructura de documentación
		/// </summary>
		public StructDocumentationModel Convert(DataBaseType type, StructParameterModelDictionary parameters)
		{
			Providers.BaseSchemaProvider provider = new Providers.ProviderFactory().GetInstance(type, parameters);

				return Convert(provider.GetSchema(), provider.DataBase, provider.Server);
		}

		/// <summary>
		///		Convierte un esquema en una estructura de documentación
		/// </summary>
		public StructDocumentationModel Convert(SchemaDbModel schema, string dataBase, string server)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global,
																			  dataBase, "DataBase", 0);

				// Añade los parámetros a la estructura
				structDoc.Parameters.Add("DataBase", dataBase);
				structDoc.Parameters.Add("Server", server);
				// Convierte el esquema
				structDoc.Childs.AddRange(ConvertTables(structDoc, schema));
				structDoc.Childs.AddRange(ConvertViews(structDoc, schema));
				structDoc.Childs.AddRange(ConvertProcedures(structDoc, schema.Routines));
				// Devuelve las estructuras convertidas
				return structDoc;
		}

		/// <summary>
		///		Convierte las tablas
		/// </summary>
		private StructDocumentationModelCollection ConvertTables(StructDocumentationModel parent, SchemaDbModel schema)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Crea la estructura de las tablas
				foreach (TableDbModel table in schema.Tables)
				{
					StructDocumentationModel structDoc = CreateStruct(parent, table, "Table");

						// Añade los parámetros de la tabla
						structDoc.Parameters.Add("Summary", table.Description);
						structDoc.Parameters.Add("DateCreate", Format(table.CreatedAt));
						structDoc.Parameters.Add("DateUpdate", Format(table.UpdatedAt));
						// Añade las columnas, restricciones, triggers...
						structDoc.Childs.AddRange(ConvertColumns(structDoc, table.Fields, table));
						structDoc.Childs.AddRange(ConvertConstrainst(structDoc, table.Constraints));
						structDoc.Childs.AddRange(ConvertTriggers(parent, schema.Triggers, table));
						// Añade la estructura a la colección
						structsDoc.Add(structDoc);
				}
				// Devuelve la colección de estructuras
				return structsDoc;
		}

		/// <summary>
		///		Convierte las vistas
		/// </summary>
		private StructDocumentationModelCollection ConvertViews(StructDocumentationModel parent, SchemaDbModel schema)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Crea la estructura de las vistas
				foreach (ViewDbModel view in schema.Views)
				{
					StructDocumentationModel structDoc = CreateStruct(parent, view, "View");

						// Añade los parámetros de la tabla
						structDoc.Parameters.Add("Prototype", view.Definition);
						structDoc.Parameters.Add("CheckOption", view.CheckOption);
						structDoc.Parameters.Add("IsUpdatable", view.IsUpdatable);
						structDoc.Parameters.Add("Summary", view.Description);
						structDoc.Parameters.Add("DateCreate", Format(view.CreatedAt));
						structDoc.Parameters.Add("DateUpdate", Format(view.UpdatedAt));
						// Añade las columnas
						structDoc.Childs.AddRange(ConvertColumns(structDoc, view.Fields, null));
						// Añade la estructura a la colección
						structsDoc.Add(structDoc);
				}
				// Devuelve la colección de estructuras
				return structsDoc;
		}

		/// <summary>
		///		Convierta las restricciones
		/// </summary>
		private StructDocumentationModelCollection ConvertConstrainst(StructDocumentationModel parent, List<ConstraintDbModel> constraints)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Convierte las restricciones
				foreach (ConstraintDbModel constraint in constraints)
				{
					StructDocumentationModel structDoc = CreateStruct(parent, constraint, "Constraint");

						// Añade los parámetros
						structDoc.Parameters.Add("Table", constraint.Table);
						structDoc.Parameters.Add("Column", constraint.Column);
						structDoc.Parameters.Add("Type", constraint.Type.ToString());
						structDoc.Parameters.Add("Position", constraint.Position);
						// Añade la estructura a la colección
						structsDoc.Add(structDoc);
				}
				// Devuelve la documentación
				return structsDoc;
		}

		/// <summary>
		///		Convierte los procedimientos
		/// </summary>
		private StructDocumentationModelCollection ConvertProcedures(StructDocumentationModel parent, List<RoutineDbModel> routines)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Crea la estructura de las rutinas
				foreach (RoutineDbModel routine in routines)
				{
					StructDocumentationModel structDoc = CreateStruct(parent, routine, routine.Type.ToString());

						// Añade los parámetros de la rutina
						structDoc.Parameters.Add("Summary", routine.Description);
						structDoc.Parameters.Add("Remarks", ExtractRemarks(routine.Content));
						structDoc.Parameters.Add("DateCreate", Format(routine.CreatedAt));
						structDoc.Parameters.Add("DateUpdate", Format(routine.UpdatedAt));
						structDoc.Parameters.Add("Prototype", routine.Content);
						// Añade la estructura a la colección
						structsDoc.Add(structDoc);
				}
				// Devuelve la colección de estructuras
				return structsDoc;
		}

		/// <summary>
		///		Extrae los comentarios del contenido
		/// </summary>
		private string ExtractRemarks(string content)
		{
			List<string> remarks = content.Extract(@"/\*\*", @"\*/");
			string result = string.Empty;

				// Concatena los resultados
				foreach (string remark in remarks)
				{
					string part = remark.TrimIgnoreNull();

						// Quita el /** y el */ de la cadena
						part = part.RemoveStart("/**");
						part = part.RemoveEnd("*/");
						// Añade la cadena al resultado
						result = result.AddWithSeparator(part.TrimIgnoreNull(), Environment.NewLine, false);
				}
				// Devuelve el resultado
				return result.TrimIgnoreNull();
		}

		/// <summary>
		///		Convierte los desencadenadores
		/// </summary>
		private StructDocumentationModelCollection ConvertTriggers(StructDocumentationModel parent, List<TriggerDbModel> triggers, TableDbModel table)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Añade los desencadenadores
				foreach (TriggerDbModel trigger in triggers)
					if (trigger.Table.EqualsIgnoreCase(table.Name))
					{
						StructDocumentationModel structDoc = CreateStruct(parent, trigger, "Trigger");

							// Añade los parámetros de la tabla
							structDoc.Parameters.Add("Table", trigger.Table);
							structDoc.Parameters.Add("UserName", trigger.UserName);
							structDoc.Parameters.Add("Category", trigger.Category);
							structDoc.Parameters.Add("IsExecuted", trigger.IsExecuted);
							structDoc.Parameters.Add("IsExecutionAnsiNullsOn", trigger.IsExecutionAnsiNullsOn);
							structDoc.Parameters.Add("IsExecutionQuotedIdentOn", trigger.IsExecutionQuotedIdentOn);
							structDoc.Parameters.Add("IsAnsiNullsOn", trigger.IsAnsiNullsOn);
							structDoc.Parameters.Add("IsQuotedIdentOn", trigger.IsQuotedIdentOn);
							structDoc.Parameters.Add("IsExecutionAfterTrigger", trigger.IsExecutionAfterTrigger);
							structDoc.Parameters.Add("IsExecutionDeleteTrigger", trigger.IsExecutionDeleteTrigger);
							structDoc.Parameters.Add("IsExecutionFirstDeleteTrigger", trigger.IsExecutionFirstDeleteTrigger);
							structDoc.Parameters.Add("IsExecutionFirstInsertTrigger", trigger.IsExecutionFirstInsertTrigger);
							structDoc.Parameters.Add("IsExecutionFirstUpdateTrigger", trigger.IsExecutionFirstUpdateTrigger);
							structDoc.Parameters.Add("IsExecutionInsertTrigger", trigger.IsExecutionInsertTrigger);
							structDoc.Parameters.Add("IsExecutionInsteadOfTrigger", trigger.IsExecutionInsteadOfTrigger);
							structDoc.Parameters.Add("IsExecutionLastDeleteTrigger", trigger.IsExecutionLastDeleteTrigger);
							structDoc.Parameters.Add("IsExecutionLastInsertTrigger", trigger.IsExecutionLastInsertTrigger);
							structDoc.Parameters.Add("IsExecutionLastUpdateTrigger", trigger.IsExecutionLastUpdateTrigger);
							structDoc.Parameters.Add("IsExecutionTriggerDisabled", trigger.IsExecutionTriggerDisabled);
							structDoc.Parameters.Add("IsExecutionUpdateTrigger", trigger.IsExecutionUpdateTrigger);
							structDoc.Parameters.Add("Summary", trigger.Description);
							structDoc.Parameters.Add("DateCreate", Format(trigger.CreatedAt));
							structDoc.Parameters.Add("DateUpdate", Format(trigger.UpdatedAt));
							structDoc.Parameters.Add("DateReference", trigger.DateReference);
							structDoc.Parameters.Add("Prototype", trigger.Content);
							// Añade la estructura
							structsDoc.Add(structDoc);
					}
					// Devuelve la colección de estructuras
					return structsDoc;
		}

		/// <summary>
		///		Convierta las columnas
		/// </summary>
		private StructDocumentationModelCollection ConvertColumns(StructDocumentationModel parent, List<FieldDbModel> fields, TableDbModel table)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Añade la información de las columnas
				foreach (FieldDbModel column in fields)
				{
					StructDocumentationModel structDoc = CreateStruct(parent, column, "Column");

						// Añade los parámetros de la columna
						structDoc.Parameters.Add("Summary", column.Description);
						structDoc.Parameters.Add("Default", column.Default);
						structDoc.Parameters.Add("IsNullable", !column.IsRequired);
						structDoc.Parameters.Add("DataType", column.DbType);
						structDoc.Parameters.Add("CharacterMaximumLength", column.Length);
						structDoc.Parameters.Add("NumericPrecision", column.NumericPrecision);
						structDoc.Parameters.Add("NumericPrecisionRadix", column.NumericPrecisionRadix);
						structDoc.Parameters.Add("NumericScale", column.NumericScale);
						structDoc.Parameters.Add("DateTimePrecision", column.DateTimePrecision);
						structDoc.Parameters.Add("CharacterSetName", column.CharacterSetName);
						structDoc.Parameters.Add("CollationCatalog", column.CollationCatalog);
						structDoc.Parameters.Add("CollationSchema", column.CollationSchema);
						structDoc.Parameters.Add("CollationName", column.CollationName);
						structDoc.Parameters.Add("IsIdentity", column.IsIdentity);
						// Obtiene el valor que indica si es una tabla foránea
						if (table != null)
							structDoc.Parameters.Add("IsForeignKey", CheckIsForeignKey(table, column));
						// Obtiene el tipo
						structDoc.Parameters.Add("Type", column.DbType);
						// Añade la estructura a la colección
						structsDoc.Add(structDoc);
				}
				// Devuelve la colección
				return structsDoc;
		}

		/// <summary>
		///		Comprueba si una columna es una clave foránea
		/// </summary>
		private bool CheckIsForeignKey(TableDbModel table, FieldDbModel column)
		{ 
			// Recorre las restricciones comprobando si es una clave foránea
			foreach (ConstraintDbModel constraint in table.Constraints)
				if (constraint.Type == ConstraintDbModel.ConstraintType.ForeignKey &&
						column.Name.EqualsIgnoreCase(constraint.Column))
					return true;
			// Si ha llegado hasta aquí es porque no es una clave foránea
			return false;
		}

		/// <summary>
		///		Obtiene el tipo de datos de una columna
		/// </summary>
		private string GetDataType(FieldDbModel column)
		{
			string type = column.DbType;

				// Añade los datos
				if (column.Length > 0)
					type += $"({column.Length})";
				// Devuelve el tipo
				return type;
		}

		/// <summary>
		///		Crea una estructura de documentación
		/// </summary>
		private StructDocumentationModel CreateStruct(StructDocumentationModel parent, BaseSchemaDbModel item, string type)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Global,
																			  item.Name, type, 0);

				// Añade los parámetros básicos
				structDoc.Parameters.Add("Schema", item.Schema);
				structDoc.Parameters.Add("Catalog", item.Catalog);
				structDoc.Parameters.Add("FullName", item.FullName);
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Formatea una fecha
		/// </summary>
		private string Format(DateTime? dtmValue)
		{
			if (dtmValue == null)
				return "-";
			else
				return string.Format("{0:dd-MM-yyyy hh:mm}", dtmValue);
		}

		/// <summary>
		///		Proveedores de base de datos
		/// </summary>
		private Dictionary<string, LibDbProviders.Base.IDbProvider> Providers { get; } = new Dictionary<string, LibDbProviders.Base.IDbProvider>();
	}
}
