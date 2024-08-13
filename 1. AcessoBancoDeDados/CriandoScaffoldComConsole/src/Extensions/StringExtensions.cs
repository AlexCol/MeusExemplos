using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CriandoScaffoldComConsole.src.Extensions;

public static class StringExtensions {
  public static string ToColumnNameForClass(this string columnName) {
    return columnName[0] + columnName.Substring(1).ToLower();
  }

  public static string ToClassNameFromTableName(this string tableName) {
    if (tableName.Contains("TB_")) {
      return "Tb" + tableName[3] + tableName.Substring(4).ToLower();
    }
    return tableName[0] + tableName.Substring(1).ToLower();
  }
}

/*
  FDictionary := TDictionary<String, String>.Create;
  FDictionary.Add('CODIGO', 'Codigo');
  FDictionary.Add('NOME', 'Nome');
  FDictionary.Add('DESCRICAO', 'Descricao');
  FDictionary.Add('EMPRESA', 'Empresa');
  FDictionary.Add('DT', 'Dt');
  FDictionary.Add('CAIXA', 'Caixa');
  FDictionary.Add('CADASTRO', 'Cadastro');
  FDictionary.Add('VLR', 'Vlr');
  FDictionary.Add('ORDEM', 'Ordem');
  FDictionary.Add('NF', 'NF');
  FDictionary.Add('MOTIVO', 'Motivo');
  FDictionary.Add('CLIENTE', 'Cliente');
  FDictionary.Add('PROPRIEDADE', 'Propriedade');
  FDictionary.Add('EMISSAO', 'Emissao');
  FDictionary.Add('USUARIO', 'Usuario');
  FDictionary.Add('TIPO', 'Tipo');
  FDictionary.Add('HORA', 'Hora');
  FDictionary.Add('BASE', 'Base');
  FDictionary.Add('NUMERO', 'Numero');
  FDictionary.Add('ESTADO', 'Estado');
  FDictionary.Add('CIDADE', 'Cidade');
  FDictionary.Add('PAIS', 'Pais');
  FDictionary.Add('PESO', 'Peso');
  FDictionary.Add('TARA', 'Tara');
  FDictionary.Add('UF', 'Uf');
  FDictionary.Add('DESTINO', 'Destino');
*/