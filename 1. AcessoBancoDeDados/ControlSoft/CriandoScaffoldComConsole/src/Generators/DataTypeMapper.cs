using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CriandoScaffoldComConsole.src.Generators;

public class DataTypeMapper {
  public string MapFromFirebirdType(DataRow columnRow) {
    var fieldType = columnRow["FIELD_TYPE"] != DBNull.Value ? Convert.ToInt32(columnRow["FIELD_TYPE"]) : -1;
    var fieldSubType = columnRow["FIELD_SUB_TYPE"] != DBNull.Value ? Convert.ToInt32(columnRow["FIELD_SUB_TYPE"]) : -1;
    var fieldLength = columnRow["FIELD_LENGTH"] != DBNull.Value ? Convert.ToInt32(columnRow["FIELD_LENGTH"]) : 0;

    return fieldType switch {
      7 => "short",
      8 => "int",
      9 => "int", // Numeric or decimal
      10 => "float",
      12 => "DateTime",
      13 => "TimeSpan",
      14 or 37 => "string", // CHAR or VARCHAR
      16 => fieldLength == 0 ? "long" : "decimal", // INT64 or NUMERIC/DECIMAL
      23 => "bool",
      27 => "double",
      35 => "DateTime",
      261 => "string", //!BLOB
      _ => throw new Exception("Tipo não definido")
    };
  }
}