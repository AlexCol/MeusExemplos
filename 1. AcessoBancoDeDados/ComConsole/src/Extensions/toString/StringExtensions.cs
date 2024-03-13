using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComConsole.src.Enums;

namespace ComConsole.src.Extensions.toString;

public static class StringExtensions {
	public static string TrataComandoPorBanco(this string comando, ETipoBanco tipoBanco) {
		if ("MySql, Firebird".Contains(tipoBanco.ToString())) {
			return comando.Replace("?", "@");
		}

		if ("Postgres, Oracle".Contains(tipoBanco.ToString())) {
			return comando.Replace("?", ":");
		}

		return comando;
	}
}
