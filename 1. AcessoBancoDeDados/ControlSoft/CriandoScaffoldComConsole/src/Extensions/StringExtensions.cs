using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CriandoScaffoldComConsole.src.Util;

namespace CriandoScaffoldComConsole.src.Extensions;

public static class StringExtensions {
  private static readonly Dictionary<string, string> referenceWordsToCamelCase = WordsReference.GetReferenceWordsToCamelCase();

  public static string ConvertToClassPropName(this string columnName) {
    int wordSize;
    var modifiedWord = columnName.ToUpper();
    foreach (var referenceWord in referenceWordsToCamelCase) {
      if (modifiedWord.StartsWith(referenceWord.Key)) {
        wordSize = referenceWord.Key.Length;
        modifiedWord = modifiedWord.Replace(referenceWord.Key, referenceWord.Value);
        return modifiedWord.Substring(0, wordSize) + modifiedWord.Substring(wordSize).ConvertToClassPropName();
      } else {
        if (modifiedWord.Contains(referenceWord.Key)) {
          modifiedWord = modifiedWord.Replace(referenceWord.Key, referenceWord.Value);
        }
      }
    }
    return modifiedWord;
  }

  public static string ConvertToClassName(this string tableName) {
    if (tableName.Contains("TB_")) {
      return tableName.Substring(3).ConvertToClassPropName();
    }
    return tableName.ConvertToClassPropName();
  }
}