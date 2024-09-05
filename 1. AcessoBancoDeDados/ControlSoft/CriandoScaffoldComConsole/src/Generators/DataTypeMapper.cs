
using System.Data;
using CriandoScaffoldComConsole.src.Models;

namespace CriandoScaffoldComConsole.src.Generators;

public class DataTypeMapper {
  public DBDataType MapFromFirebirdType(DataRow columnRow) {
    var fieldType = columnRow["FIELD_TYPE"] != DBNull.Value ? Convert.ToInt32(columnRow["FIELD_TYPE"]) : -1;
    var fieldSubType = columnRow["FIELD_SUB_TYPE"] != DBNull.Value ? Convert.ToInt32(columnRow["FIELD_SUB_TYPE"]) : -1;
    var fieldLength = columnRow["FIELD_LENGTH"] != DBNull.Value ? Convert.ToInt32(columnRow["FIELD_LENGTH"]) : 0;
    var fieldPrecision = columnRow["FIELD_PRECISION"] != DBNull.Value ? Convert.ToInt32(columnRow["FIELD_PRECISION"]) : 0;
    var fieldScale = columnRow["FIELD_SCALE"] != DBNull.Value ? Convert.ToInt32(columnRow["FIELD_SCALE"]) : 0;
    var nullable = columnRow["IS_NOT_NULL"].ToString().Trim().Equals("N");
    return new DBDataType(fieldType, fieldSubType, fieldLength, fieldPrecision, fieldScale, nullable);
  }
}