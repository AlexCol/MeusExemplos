using System.Data;
using CriandoScaffoldComConsole.Database;

namespace CriandoScaffoldComConsole.src.Generators;
public partial class ScaffoldGenerator {
  private List<string> TabelasProcessadas = new List<string>();
  private readonly DatabaseConnection _dbConnection;
  private readonly CodeBuilder _codeBuilder;
  private readonly DataTypeMapper _dataTypeMapper;
  private readonly FileGenerator _fileGenerator;
  public string NameSpace { get; set; } = "";
  public string BaseClass { get; set; } = "";

  public ScaffoldGenerator(DatabaseConnection dbConnection, FileGenerator fileGenerator) {
    _fileGenerator = fileGenerator;
    _dbConnection = dbConnection;
    _codeBuilder = new CodeBuilder();
    _dataTypeMapper = new DataTypeMapper();
  }

  public void GenerateClasses(string tableName) {
    if (TabelasProcessadas.Contains(tableName)) return;
    try {
      using var connection = _dbConnection.GetConnection();
      connection.Open();

      var columnsTable = GetColumnsTable(connection, tableName);
      if (columnsTable.Rows.Count == 0) throw new InvalidDataException($"Tabela {tableName} não existe.");

      var constraints = GetConstraints(connection, tableName).OrderByDescending(c => c.ConstraintType).ToList();

      var classCode = _codeBuilder.BuildClassCode(tableName, BaseClass, NameSpace, columnsTable, constraints, _dataTypeMapper);
      _fileGenerator.SaveClass(classCode, NameSpace, BaseClass);
      TabelasProcessadas.Add(tableName);

      GenerateReferencedClasses(constraints);
    } catch (Exception e) {
      throw new InvalidDataException($"Erro na tabela {tableName}. {e.Message}");
    }
  }

  public List<string> GetTablesFromList(List<string> list) {
    using var connection = _dbConnection.GetConnection();
    connection.Open();

    var query = CreateTablesQuery(list);
    var dataBaseList = GetTablesFromDatabase(connection, query);

    if (list.Count > 0 && list.Count != dataBaseList.Count)
      throw new InvalidDataException($"Tabelas não encontradas: {String.Join(",", list.Except(dataBaseList))}");

    return dataBaseList;
  }

}
