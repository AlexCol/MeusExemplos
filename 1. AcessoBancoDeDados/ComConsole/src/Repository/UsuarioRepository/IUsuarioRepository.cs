using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComConsole.src.Model;

namespace ComConsole.src.Repository.UsuarioRepository;

public interface IUsuarioRepository {
	public Usuario FindById(int id);
	public Usuario FindLatestByUseData(Usuario usuario);
	public Usuario Create(Usuario usuario);
	public Usuario Update(Usuario usuario);
	public void Delete(int id);
}
