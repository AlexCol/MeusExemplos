using System.Data;
using ComConsole.src.Enums;

namespace ComConsole.src.Factory.Interface;

public interface IConnectionFactory {
	public IDbConnection Connect();
	public void Dispose();
	public ETipoBanco getTipoBanco();
}
