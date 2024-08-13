using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CriandoScaffoldComConsole.src.Generators;

public class DataTypeMapper
{
  public string MapFromFirebirdType(DataRow columnRow)
  {
    var fieldType = Convert.ToInt32(columnRow["FIELD_TYPE"]);
    var fieldSubType = Convert.ToInt32(columnRow["FIELD_SUB_TYPE"]);
    var fieldLength = Convert.ToInt32(columnRow["FIELD_LENGTH"]);

    return fieldType switch
    {
      7 when fieldSubType == 1 => "short",
      8 => "int",
      9 => "int", // Numeric or decimal
      10 => "float",
      12 => "DateTime",
      13 => "TimeSpan",
      14 or 37 => "string", // CHAR or VARCHAR
      16 => fieldSubType == 1 ? "long" : "decimal", // INT64 or NUMERIC/DECIMAL
      27 => "double",
      35 => "DateTime",
      _ => "object", // Fallback to object if type is not explicitly handled
    };
  }
}