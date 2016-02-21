using System;
using System.Data;

using Bau.Libraries.LibDBProvidersBase;
using Bau.Libraries.LibDBProvidersBase.Parameters;
using Bau.Libraries.LibDBProvidersBase.Providers.SQLServer;
using Bau.Libraries.LibDBSchema;
using Bau.Libraries.LibDBSchema.DataSchema;

namespace Bau.Libraries.LibDBSchemaProvider.Providers
{
	/// <summary>
	///		Proveedor de esquema para SQL Server
	/// </summary>
	public class SchemaSqlServerProvider : Interfaces.ISchemaProvider<SQLServerConnectionString>
	{
		/// <summary>
		///		Clase para la carga de un esquema de una base de datos SQL Server
		/// </summary>
		public Schema LoadSchema(SQLServerConnectionString objConnectionString)
		{ Schema objSchema = new Schema();
		
				// Carga los datos del esquema
					LoadSchemaTables(objSchema, objConnectionString);
					LoadSchemaTriggers(objSchema, objConnectionString);
					LoadSchemaViews(objSchema, objConnectionString);
					LoadSchemaRoutines(objSchema, objConnectionString);
				// Devuelve el esquema
					return objSchema;
		}

		/// <summary>
		///		Carga las tablas del esquema de una base de datos
		/// </summary>
		public void LoadSchemaTables(Schema objSchema, SQLServerConnectionString  objConnectionString)
		{ using (SQLServerProvider objConnection = new SQLServerProvider(objConnectionString))
				{ // Abre la conexión
						objConnection.Open();
					// Limpia las tablas
						objSchema.Tables.Clear();
					// Obtiene los datos del esquema
						LoadTables(objConnection, objSchema);
					// Cierra la conexión
						objConnection.Close();
				}
		}

		/// <summary>
		///		Carga los desencadenadores del esquema de una base de datos
		/// </summary>
		public void LoadSchemaTriggers(Schema objSchema, SQLServerConnectionString  objConnectionString)
		{ using (SQLServerProvider objConnection = new SQLServerProvider(objConnectionString))
				{ // Abre la conexión
						objConnection.Open();
					// Limpia los triggers
						objSchema.Triggers.Clear();
					// Obtiene los datos del esquema
						LoadTriggers(objConnection, objSchema);
					// Cierra la conexión
						objConnection.Close();
				}
		}
		
		/// <summary>
		///		Carga las vistas del esquema de una base de datos
		/// </summary>
		public void LoadSchemaViews(Schema objSchema, SQLServerConnectionString  objConnectionString)
		{ using (SQLServerProvider objConnection = new SQLServerProvider(objConnectionString))
				{ // Abre la conexión
						objConnection.Open();
					// Limpia las vistas
						objSchema.Views.Clear();
					// Obtiene los datos del esquema
						LoadViews(objConnection, objSchema);
					// Cierra la conexión
						objConnection.Close();
				}
		}
		
		/// <summary>
		///		Carga las rutinas del esquema de una base de datos
		/// </summary>
		public void LoadSchemaRoutines(Schema objSchema, SQLServerConnectionString  objConnectionString)
		{ using (SQLServerProvider objConnection = new SQLServerProvider(objConnectionString))
				{ // Abre la conexión
						objConnection.Open();
					// Limpia las rutinas
						objSchema.Routines.Clear();
					// Obtiene los datos del esquema
						LoadRoutines(objConnection, objSchema);
					// Cierra la conexión
						objConnection.Close();
				}
		}
		
		/// <summary>
		///		Carga las tablas de un esquema
		/// </summary>
		private void LoadTables(SQLServerProvider objConnection, Schema objSchema)
		{ string strSQL;
			
				// Crea la cadena SQL
					strSQL = @"SELECT Tables.TABLE_CATALOG, Tables.TABLE_SCHEMA, Tables.TABLE_NAME,
														Tables.TABLE_TYPE, Objects.Create_Date, Objects.Modify_Date, Properties.Value AS Description
											 FROM INFORMATION_SCHEMA.TABLES AS Tables INNER JOIN sys.all_objects AS Objects
												 ON Tables.Table_Name = Objects.name
											 LEFT JOIN sys.extended_properties AS Properties
												 ON Objects.object_id = Properties.major_id
														AND Properties.minor_id = 0
														AND Properties.name = 'MS_Description'
											ORDER BY Tables.TABLE_NAME";
				// Carga las tablas
					using (IDataReader rdoTables = objConnection.ExecuteReader(strSQL, null, CommandType.Text))
						{ // Recorre la colección de registros
								while (rdoTables.Read())
									{ SchemaTable objTable = objTable = new SchemaTable(objSchema);
									
											// Asigna los datos del registro al objeto
												objTable.Catalog = (string) rdoTables.IisNull("TABLE_CATALOG");
												objTable.Schema = (string) rdoTables.IisNull("TABLE_SCHEMA");
												objTable.Name = (string) rdoTables.IisNull("TABLE_NAME");
												objTable.Type = GetTableType((string) rdoTables.IisNull("TABLE_TYPE"));
												objTable.DateCreate = (DateTime) rdoTables.IisNull("Create_Date");
												objTable.DateUpdate = (DateTime) rdoTables.IisNull("Modify_Date");
												objTable.Description = (string) rdoTables.IisNull("Description");
											// Añade el objeto a la colección (si es una tabla)
												if (objTable.Type == SchemaTable.TableType.Table)
													objSchema.Tables.Add(objTable);
									}
							// Cierra el recordset
								rdoTables.Close();
						}
				// Carga los datos de las tablas
					foreach (SchemaTable objTable in objSchema.Tables)
						{ LoadColumns(objConnection, objTable);
							LoadConstraints(objConnection, objTable);
						}
		}

		/// <summary>
		///		Obtiene el tipo de una tabla a partir de su descripción
		/// </summary>
		private SchemaTable.TableType GetTableType(string strTableType)
		{	if (strTableType.Equals("BASE TABLE", StringComparison.CurrentCultureIgnoreCase))
				return SchemaTable.TableType.Table;
			else if (strTableType.Equals("VIEW", StringComparison.CurrentCultureIgnoreCase))
				return SchemaTable.TableType.View;
			else
				return SchemaTable.TableType.Unknown;
		}
		
		/// <summary>
		///		Carga los triggers de un esquema
		/// </summary>
		private void LoadTriggers(SQLServerProvider objConnection, Schema objSchema)
		{ string strSQL;
			
				// Crea la cadena SQL
					strSQL = @"SELECT tmpTables.name AS DS_Tabla, tmpTrigger.name AS DS_Trigger_Name,
														USER_NAME(tmpTrigger.uid) AS DS_User_Name, tmpTrigger.category AS NU_Category,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'IsExecuted') = 1) THEN 1 ELSE 0 END)) AS IsExecuted,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsAnsiNullsOn') = 1) THEN 1 ELSE 0 END)) AS ExecIsAnsiNullsOn,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsQuotedIdentOn') = 1) THEN 1 ELSE 0 END)) AS ExecIsQuotedIdentOn,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'IsAnsiNullsOn') = 1) THEN 1 ELSE 0 END)) AS IsAnsiNullsOn,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'IsQuotedIdentOn') = 1) THEN 1 ELSE 0 END)) AS IsQuotedIdentOn,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsAfterTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsAfterTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsDeleteTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsDeleteTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsFirstDeleteTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsFirstDeleteTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsFirstInsertTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsFirstInsertTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsFirstUpdateTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsFirstUpdateTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsInsertTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsInsertTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsInsteadOfTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsInsteadOfTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsLastDeleteTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsLastDeleteTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsLastInsertTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsLastInsertTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsLastUpdateTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsLastUpdateTrigger,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsTriggerDisabled') = 1) THEN 1 ELSE 0 END)) AS ExecIsTriggerDisabled,
														CONVERT(bit, (CASE WHEN (OBJECTPROPERTY(tmpTrigger.id, N'ExecIsUpdateTrigger') = 1) THEN 1 ELSE 0 END)) AS ExecIsUpdateTrigger,
														tmpTrigger.crdate AS FE_Create, tmpTrigger.refdate AS FE_Reference
											 FROM sys.sysobjects AS tmpTrigger INNER JOIN sys.sysobjects AS tmpTables
												 ON tmpTrigger.parent_obj = tmpTables.id
											 WHERE OBJECTPROPERTY(tmpTrigger.id, N'IsTrigger') = 1
												 AND OBJECTPROPERTY(tmpTrigger.id, N'IsMSShipped') = 0
											 ORDER BY tmpTables.Name, tmpTrigger.Name";
				// Carga los desencadenadores
					using (IDataReader rdoTriggers = objConnection.ExecuteReader(strSQL, null, CommandType.Text))
						{ // Recorre la colección de registros
								while (rdoTriggers.Read())
									{ SchemaTrigger objTrigger = new SchemaTrigger(objSchema);
									
											// Asigna los datos del registro al objeto
												objTrigger.Catalog = "TABLE_CATALOG"; // clsBaseDB.iisNull(rdoTables, "TABLE_CATALOG") as string;
												objTrigger.Schema = "TABLE_SCHEMA"; // clsBaseDB.iisNull(rdoTables, "TABLE_SCHEMA") as string;
												objTrigger.Table = (string) rdoTriggers.IisNull("DS_Tabla");
												objTrigger.Name = (string) rdoTriggers.IisNull("DS_Trigger_Name");
												objTrigger.UserName = (string) rdoTriggers.IisNull("DS_User_Name");
												objTrigger.Category = (int) rdoTriggers.IisNull("NU_Category");
												objTrigger.IsExecuted = (bool) rdoTriggers.IisNull("IsExecuted");
												objTrigger.IsExecutionAnsiNullsOn = (bool) rdoTriggers.IisNull("ExecIsAnsiNullsOn");
												objTrigger.IsExecutionQuotedIdentOn = (bool) rdoTriggers.IisNull("ExecIsQuotedIdentOn");
												objTrigger.IsAnsiNullsOn = (bool) rdoTriggers.IisNull("IsAnsiNullsOn");
												objTrigger.IsQuotedIdentOn = (bool) rdoTriggers.IisNull("IsQuotedIdentOn");
												objTrigger.IsExecutionAfterTrigger = (bool) rdoTriggers.IisNull("ExecIsAfterTrigger");
												objTrigger.IsExecutionDeleteTrigger = (bool) rdoTriggers.IisNull("ExecIsDeleteTrigger");
												objTrigger.IsExecutionFirstDeleteTrigger = (bool) rdoTriggers.IisNull("ExecIsFirstDeleteTrigger");
												objTrigger.IsExecutionFirstInsertTrigger = (bool) rdoTriggers.IisNull("ExecIsFirstInsertTrigger");
												objTrigger.IsExecutionFirstUpdateTrigger = (bool) rdoTriggers.IisNull("ExecIsFirstUpdateTrigger");
												objTrigger.IsExecutionInsertTrigger = (bool) rdoTriggers.IisNull("ExecIsInsertTrigger");
												objTrigger.IsExecutionInsteadOfTrigger = (bool) rdoTriggers.IisNull("ExecIsInsteadOfTrigger");
												objTrigger.IsExecutionLastDeleteTrigger = (bool) rdoTriggers.IisNull("ExecIsLastDeleteTrigger");
												objTrigger.IsExecutionLastInsertTrigger = (bool) rdoTriggers.IisNull("ExecIsLastInsertTrigger");
												objTrigger.IsExecutionLastUpdateTrigger = (bool) rdoTriggers.IisNull("ExecIsLastUpdateTrigger");
												objTrigger.IsExecutionTriggerDisabled = (bool) rdoTriggers.IisNull("ExecIsTriggerDisabled");
												objTrigger.IsExecutionUpdateTrigger = (bool) rdoTriggers.IisNull("ExecIsUpdateTrigger");
												objTrigger.DateCreate = (DateTime?) rdoTriggers.IisNull("FE_Create");
												objTrigger.DateReference = (DateTime?) rdoTriggers.IisNull("FE_Reference");
											// Añade el objeto a la colección (si es una tabla)
												objSchema.Triggers.Add(objTrigger);
									}
							// Cierra el recordset
								rdoTriggers.Close();
						}
				// Carga el contenido de los triggers
					foreach (SchemaTrigger objTrigger in objSchema.Triggers)
						objTrigger.Content = LoadHelpText(objConnection, objTrigger.Name);
		}

		/// <summary>
		///		Carga el texto de una función, procedimiento, trigger ...
		/// </summary>
		private string LoadHelpText(SQLServerProvider objConnection, string strName) 
		{ string strText = "";
		
				// Obtiene el texto resultante de llamar a la rutina sp_helptext
					try
						{ using (IDataReader rdoText = objConnection.ExecuteReader("EXEC sp_helptext '" + strName + "'", null, CommandType.Text))
								{ // Obtiene el texto
										while (rdoText.Read())
											strText += (string) rdoText.IisNull("Text");
									// Cierra el recordset
										rdoText.Close();
								}
						}
					catch {}
				// Devuelve el texto cargado
					return strText;
		}

		/// <summary>
		///		Carga las rutinas de la base de datos
		/// </summary>
		private void LoadRoutines(SQLServerProvider objConnection, Schema objSchema)
		{ string strSQL;
			
				// Crea la cadena SQL
					strSQL = @"SELECT Routine_Catalog AS Table_Catalog, Routine_Schema AS Table_Schema,
														Routine_Name AS Table_Name, Routine_Type, Routine_Definition
											 FROM Information_Schema.Routines
											 ORDER BY Routine_Name";
				// Carga los datos
					using (IDataReader rdoRoutines = objConnection.ExecuteReader(strSQL, null, CommandType.Text))
						{ // Lee los registros
								while (rdoRoutines.Read())
									{ SchemaRoutine objRoutine = new SchemaRoutine(objSchema);
									
											// Asigna los datos del recordset al objeto
												objRoutine.Catalog = (string) rdoRoutines.IisNull("Table_Catalog");
												objRoutine.Schema = (string) rdoRoutines.IisNull("Table_Schema");
												objRoutine.Name = (string) rdoRoutines.IisNull("Table_Name");
												objRoutine.Type = GetRoutineType((string) rdoRoutines.IisNull("Routine_Type"));
												objRoutine.Definition = (string) rdoRoutines.IisNull("Routine_Definition");
											// Añade el objeto a la colección
												objSchema.Routines.Add(objRoutine);
									}
							// Cierra el recordset
								rdoRoutines.Close();
						}
		}

		/// <summary>
		///		Obtiene el tipo de rutina
		/// </summary>
		private SchemaRoutine.RoutineType GetRoutineType(string strType)
		{ if (strType.Equals("PROCEDURE", StringComparison.CurrentCultureIgnoreCase))
				return SchemaRoutine.RoutineType.Procedure;
			else if (strType.Equals("FUNCTION", StringComparison.CurrentCultureIgnoreCase))
				return SchemaRoutine.RoutineType.Function;
			else
				return SchemaRoutine.RoutineType.Unknown;
		}
		
		/// <summary>
		///		Carga las restricciones de una tabla
		/// </summary>
		private void LoadConstraints(SQLServerProvider objConnection, SchemaTable objTable)
		{ ParametersDBCollection objColParameters = new ParametersDBCollection();
			string strSQL;
			
				// Añade los parámetros
					objColParameters.Add("@Table_Catalog", objTable.Catalog);
					objColParameters.Add("@Table_Schema", objTable.Schema);
					objColParameters.Add("@Table_Name", objTable.Name);
				// Crea la cadena SQL
					strSQL = @"SELECT TableConstraints.Table_Catalog, TableConstraints.Table_Schema, TableConstraints.Table_Name,
														ColumnConstraint.Column_Name, ColumnConstraint.Constraint_Name,
														TableConstraints.Constraint_Type, Key_Column.Ordinal_Position
											 FROM Information_Schema.Table_Constraints AS TableConstraints
											 INNER JOIN Information_Schema.Constraint_Column_Usage AS ColumnConstraint
													ON TableConstraints.Constraint_Catalog = ColumnConstraint.Constraint_Catalog
														AND TableConstraints.Constraint_Schema = ColumnConstraint.Constraint_Schema
														AND TableConstraints.Constraint_Name = ColumnConstraint.Constraint_Name
											 INNER JOIN Information_Schema.Key_Column_Usage AS Key_Column
													ON ColumnConstraint.Constraint_Catalog = Key_Column.Constraint_Catalog
														AND ColumnConstraint.Constraint_Schema = Key_Column.Constraint_Schema
														AND ColumnConstraint.Constraint_Name = Key_Column.Constraint_Name
														AND ColumnConstraint.Column_Name = Key_Column.Column_Name
											WHERE TableConstraints.Table_Catalog = @Table_Catalog
												AND TableConstraints.Table_Schema = @Table_Schema
												AND TableConstraints.Table_Name = @Table_Name
										 ORDER BY TableConstraints.Table_Name, TableConstraints.Constraint_Type, Key_Column.Ordinal_Position";
				// Carga los datos
					using (IDataReader rdoConstraint = objConnection.ExecuteReader(strSQL, objColParameters, CommandType.Text))
						{ // Lee los datos
								while (rdoConstraint.Read())
									{ SchemaConstraint objConstraint = new SchemaConstraint(objTable.Parent);
									
											// Asigna los datos del registro
												objConstraint.Catalog = (string) rdoConstraint.IisNull("Table_Catalog");
												objConstraint.Schema = (string) rdoConstraint.IisNull("Table_Schema");
												objConstraint.Table = (string) rdoConstraint.IisNull("Table_Name");
												objConstraint.Column = (string) rdoConstraint.IisNull("Column_Name");
												objConstraint.Name = (string) rdoConstraint.IisNull("Constraint_Name");
												objConstraint.Type = GetConstraintType((string) rdoConstraint.IisNull("Constraint_Type"));
												objConstraint.Position = (int) rdoConstraint.IisNull("Ordinal_Position");
											// Añade la restricción a la colección
												objTable.Constraints.Add(objConstraint);
									}
							// Cierra el recordset
								rdoConstraint.Close();
						}
		}

		/// <summary>
		///		Obtiene el tipo de una restricción a partir de su nombre
		/// </summary>
		private SchemaConstraint.ConstraintType GetConstraintType(string strType)
		{ if (strType.Equals("UNIQUE", StringComparison.CurrentCultureIgnoreCase))
				return SchemaConstraint.ConstraintType.Unique;
			else if (strType.Equals("PRIMARY KEY", StringComparison.CurrentCultureIgnoreCase))
				return SchemaConstraint.ConstraintType.PrimaryKey;
			else if (strType.Equals("FOREIGN KEY", StringComparison.CurrentCultureIgnoreCase))
				return SchemaConstraint.ConstraintType.ForeignKey;
			else
				return SchemaConstraint.ConstraintType.Unknown;
		}
		
		/// <summary>
		///		Carga la definición de vistas
		/// </summary>
		private void LoadViews(SQLServerProvider objConnection, Schema objSchema)
		{ string strSQL;
			
				// Crea la cadena SQL
					strSQL = @"SELECT Table_Catalog, Table_Schema, Table_Name, View_Definition, Check_Option, Is_Updatable
											 FROM Information_Schema.Views
											 ORDER BY Table_Name";
				// Carga las vistas
					using (IDataReader rdoViews = objConnection.ExecuteReader(strSQL, null, CommandType.Text))
						{ // Lee los registros
								while (rdoViews.Read())
									{ SchemaView objView = new SchemaView(objSchema);
									
											// Asigna los datos al objeto
												objView.Catalog = (string) rdoViews.IisNull("Table_Catalog");
												objView.Schema = (string) rdoViews.IisNull("Table_Schema");
												objView.Name = (string) rdoViews.IisNull("Table_Name");
												objView.Definition = (string) rdoViews.IisNull("View_Definition");
												objView.CheckOption = (string) rdoViews.IisNull("Check_Option");
												objView.IsUpdatable = !(((string) rdoViews.IisNull("Is_Updatable")).Equals("NO", StringComparison.CurrentCultureIgnoreCase));
											// Añade el objeto a la colección
												objSchema.Views.Add(objView);
									}
							// Cierra el recordset
								rdoViews.Close();
						}
				// Carga las columnas de la vista
					foreach (SchemaView objView in objSchema.Views)
						LoadColumns(objConnection, objView);
		}
		
		/// <summary>
		///		Carga las columnas de una tabla
		/// </summary>
		private void LoadColumns(SQLServerProvider objConnection, SchemaTable objTable)
		{ ParametersDBCollection objColParameters = new ParametersDBCollection();
			string strSQL;
			
				// Añade los parámetros
					objColParameters.Add("@Table_Catalog", objTable.Catalog);
					objColParameters.Add("@Table_Schema", objTable.Schema);
					objColParameters.Add("@Table_Name", objTable.Name);
				// Crea la cadena SQL
					strSQL = @"SELECT Columns.Column_Name, Columns.Ordinal_Position, Columns.Column_Default,
													  Columns.Is_Nullable, Columns.Data_Type, Columns.Character_Maximum_Length,
														CONVERT(int, Columns.Numeric_Precision) AS Numeric_Precision,
														CONVERT(int, Columns.Numeric_Precision_Radix) AS Numeric_Precision_Radix,
														CONVERT(int, Columns.Numeric_Scale) AS Numeric_Scale,
														CONVERT(int, Columns.DateTime_Precision) AS DateTime_Precision,
														Columns.Character_Set_Name, Columns.Collation_Catalog, Columns.Collation_Schema, Columns.Collation_Name,
														Objects.is_identity, Properties.value AS Description
											 FROM Information_Schema.Columns AS Columns INNER JOIN sys.all_objects AS Tables
												 ON Columns.Table_Name = Tables.name
											 INNER JOIN sys.columns AS Objects
												  ON Columns.Column_Name = Objects.name
														 AND Tables.object_id = Objects.object_id
											 LEFT JOIN sys.extended_properties AS Properties
												 ON Objects.object_id = Properties.major_id
														AND Properties.minor_id = Objects.column_id
														AND Properties.name = 'MS_Description'
											 WHERE Columns.Table_Catalog = @Table_Catalog
												 AND Columns.Table_Schema = @Table_Schema
												 AND Columns.Table_Name = @Table_Name
											 ORDER BY Ordinal_Position";
				// Carga los datos
					using (IDataReader rdoColumns = objConnection.ExecuteReader(strSQL, objColParameters, CommandType.Text))
						{ // Lee los datos
								while (rdoColumns.Read())
									{ SchemaColumn objColumn = new SchemaColumn(objTable.Parent);
									
											// Asigna los datos del registro
												objColumn.Name = (string) rdoColumns.IisNull("Column_Name") as string;
												objColumn.OrdinalPosition = rdoColumns.IisNull("Ordinal_Position", 0);
												objColumn.Default = (string) rdoColumns.IisNull("Column_Default");
												objColumn.IsNullable = !(((string) rdoColumns.IisNull("Is_Nullable")).Equals("no", StringComparison.CurrentCultureIgnoreCase));
												objColumn.DataType = (string) rdoColumns.IisNull("Data_Type");
												objColumn.CharacterMaximumLength = rdoColumns.IisNull("Character_Maximum_Length", 0);
												objColumn.NumericPrecision = rdoColumns.IisNull("Numeric_Precision", 0);
												objColumn.NumericPrecisionRadix = rdoColumns.IisNull("Numeric_Precision_Radix", 0);
												objColumn.NumericScale = rdoColumns.IisNull("Numeric_Scale", 0);
												objColumn.DateTimePrecision = rdoColumns.IisNull("DateTime_Precision", 0);
												objColumn.CharacterSetName = (string) rdoColumns.IisNull("Character_Set_Name");
												objColumn.CollationCatalog = (string) rdoColumns.IisNull("Collation_Catalog");
												objColumn.CollationSchema = (string) rdoColumns.IisNull("Collation_Schema");
												objColumn.CollationName = (string) rdoColumns.IisNull("Collation_Name");
												objColumn.IsIdentity = (bool) rdoColumns.IisNull("is_identity");
												objColumn.Description = (string) rdoColumns.IisNull("Description") as string;
											// Añade la columna a la colección
												objTable.Columns.Add(objColumn);
									}
							// Cierra el recordset
								rdoColumns.Close();
						}
		}
		
		/// <summary>
		///		Carga las columnas de la vista
		/// </summary>
		private void LoadColumns(SQLServerProvider objConnection, SchemaView objView)
		{ ParametersDBCollection objColParameters = new ParametersDBCollection();
			string strSQL;
			
				// Asigna lo parámetros
					objColParameters.Add("@View_Catalog", objView.Catalog);
					objColParameters.Add("@View_Schema", objView.Schema);
					objColParameters.Add("@View_Name", objView.Name);
				// Crea la cadena SQL
					strSQL = @"SELECT Table_Catalog, Table_Schema, Table_Name, Column_Name
											 FROM Information_Schema.View_Column_Usage
											 WHERE View_Catalog = @View_Catalog
												 AND View_Schema = @View_Schema
												 AND View_Name = @View_Name";
				// Carga las columnas
					using (IDataReader rdoColumns = objConnection.ExecuteReader(strSQL, objColParameters, CommandType.Text))
						{ // Lee los registros
								while (rdoColumns.Read())
									{ SchemaColumn objColumn = new SchemaColumn(objView.Parent);
									
											// Carga los datos de la columna
												objColumn.Catalog = (string) rdoColumns.IisNull("Table_Catalog");
												objColumn.Schema = (string) rdoColumns.IisNull("Table_Schema");
												objColumn.Table = (string) rdoColumns.IisNull("Table_Name");
												objColumn.Name = (string) rdoColumns.IisNull("Column_Name");
											// Añade la columna a la colección
												objView.Columns.Add(objColumn);
									}
							// Cierra el recordset
								rdoColumns.Close();
						}
		}
	}
}