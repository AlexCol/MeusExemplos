using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers_Genericos.src.Attibutes.GenericController;

namespace Controllers_Genericos.src.Models;

[GenericControllerBrowse("/api/[controller]")]
public class Category {
  public int Id { get; set; }
  public string Name { get; set; } = "";
}