using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCustomJobs.src.Model;

public class JobRequest {
  public string Email { get; set; }
  public int Interval { get; set; }
}
