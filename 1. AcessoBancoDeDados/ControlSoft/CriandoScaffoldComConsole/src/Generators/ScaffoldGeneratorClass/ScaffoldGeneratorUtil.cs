
using System.Data;
using CriandoScaffoldComConsole.Database;
using CriandoScaffoldComConsole.src.Models;
using FirebirdSql.Data.FirebirdClient;

namespace CriandoScaffoldComConsole.src.Generators;

public partial class ScaffoldGenerator {
  private List<string> GetTablesFromDatabase(FbConnection connection, string query) {
    var dataBaseList = new List<string>();
    var tables = DatabaseHelper.ExecuteQuery(connection, query);
    foreach (DataRow row in tables.Rows) {
      dataBaseList.Add(row["TABLE_NAME"].ToString()?.Trim());
    }

    return dataBaseList;
  }

  private List<ConstraintInfo> GetConstraints(FbConnection connection, string tableName) {
    var query = QueryContraints(tableName);
    var constraintsTable = DatabaseHelper.ExecuteQuery(connection, query);
    var constraintsList = ConstraintInfo.FromDataTable(constraintsTable);

    SetCircularReferences(connection, tableName, constraintsList);
    SetPKFromReferenceNumber(connection, constraintsList);

    return constraintsList;
  }

  private void SetCircularReferences(FbConnection connection, string tableName, List<ConstraintInfo> constraintsList) {
    for (int i = 0; i < constraintsList.Count; i++) {
      constraintsList[i].CircularReference = CheckIfCircularReference(connection, tableName, constraintsList[i].ReferencedTable);
    }
  }

  private void SetPKFromReferenceNumber(FbConnection connection, List<ConstraintInfo> constraintsList) {
    for (int i = 0; i < constraintsList.Count; i++) {
      if (constraintsList[i].ConstraintType != "FOREIGN KEY") continue;
      if (constraintsList[i].ConstraintNumber <= 1) continue;

      var query = QueryContraints(constraintsList[i].ReferencedTable);
      var constraintsTable = DatabaseHelper.ExecuteQuery(connection, query);
      var referenceConstraints = ConstraintInfo.FromDataTable(constraintsTable);

      constraintsList[i].PKNumberFromReferenceTable = referenceConstraints.Count(r => r.ConstraintType == "PRIMARY KEY");
    }
  }

  private void GenerateReferencedClasses(List<ConstraintInfo> constraints) {
    foreach (var constraint in constraints.Where(c => !string.IsNullOrEmpty(c.ReferencedTable))) {
      GenerateClasses(constraint.ReferencedTable);
    }
  }

  private bool CheckIfCircularReference(FbConnection connection, string tableName, string referencedTable) {
    var query = QueryContraints(referencedTable);
    var constraintsRefTable = DatabaseHelper.ExecuteQuery(connection, query);
    var constraintsRefList = ConstraintInfo.FromDataTable(constraintsRefTable);

    var circularConstraint = constraintsRefList.Any(l => l.ReferencedTable == tableName);
    return circularConstraint;
  }
}
