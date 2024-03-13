
using ComConsole.src.Enums;
using ComConsole.src.Testes;

//!testes banco de dados
//+define o banco a usar
ETipoBanco tipoBanco = ETipoBanco.Firebird;
//ETipoBanco tipoBanco = ETipoBanco.Postgres;
DBTesting.Test(tipoBanco);
