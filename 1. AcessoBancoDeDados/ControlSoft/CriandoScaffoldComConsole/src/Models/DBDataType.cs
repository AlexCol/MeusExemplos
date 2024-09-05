using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CriandoScaffoldComConsole.src.Models;

public class DBDataType {

  public int FieldType { get; private set; }
  public int FieldSubType { get; private set; }
  public int FieldLength { get; private set; }
  public int FieldPrecision { get; set; }
  public int FieldScale { get; set; }
  public bool Nullable { get; set; }
  public string PropName {
    get {
      return MapFieldToCSharpProp();
    }
  }
  public string TypeName {
    get {
      return GetTypeName();
    }
  }

  public DBDataType(int fieldType, int fieldSubType, int fieldLength, int fieldPrecision, int fieldScale, bool nullable) {
    FieldType = fieldType;
    FieldSubType = fieldSubType;
    FieldLength = fieldLength;
    FieldPrecision = fieldPrecision;
    FieldScale = fieldScale;
    Nullable = nullable;
  }

  private string MapFieldToCSharpProp() {
    //https://firebirdsql.org/file/documentation/chunk/en/refdocs/fblangref30/fblangref-appx04-fields.html    
    /*
    7 - SMALLINT 
    8 - INTEGER 
    10 - FLOAT 
    12 - DATE 
    13 - TIME 
    14 - CHAR 
    16 - BIGINT 
    23 - BOOLEAN 
    27 - DOUBLE PRECISION 
    35 - TIMESTAMP 
    37 - VARCHAR 
    261 - BLOB
    */
    return FieldType switch {
      7 => "short",
      8 => "int",
      10 => "float",
      12 => "DateTime",
      13 => "TimeSpan",
      14 or 37 => "string",
      16 => FieldLength == 0 ? "long" : "decimal", // INT64 or NUMERIC/DECIMAL
      23 => "bool",
      27 => "double",
      35 => "DateTime",
      261 => "string", //!BLOB
      _ => throw new Exception("Tipo não definido")
    };
  }

  private string GetTypeName() {
    var tiposTratados = new List<string>() { "decimal" };
    if (!tiposTratados.Contains(PropName)) return null;

    var typeName = ", TypeName = \"";

    typeName += PropName switch {
      "decimal" => $"NUMERIC({FieldPrecision}, {FieldScale * -1})",
      _ => throw new Exception("DataType não especificado.")
    };

    typeName += "\"";
    return typeName;
  }
}