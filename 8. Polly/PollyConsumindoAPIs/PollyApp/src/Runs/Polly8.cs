using PollyApp.src.MyPolicies;
using PollyApp.src.Services;

namespace PollyApp.src.Runs;

public class Polly8 {
  public static void Run(TestService myTest) {
    var pipeline = TestServicePollyNewVersion.GetPolicy();
    pipeline.Execute(() => {
      var response = myTest.Test();
      Console.WriteLine(response);
    });
  }
}
