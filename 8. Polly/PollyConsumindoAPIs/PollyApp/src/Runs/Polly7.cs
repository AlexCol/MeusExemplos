using PollyApp.src.MyPolicies;
using PollyApp.src.Services;

namespace PollyApp.src.Runs;

public static class Polly7 {
  public static void Run(TestService myTest) {
    var combinedPolicy = TestServicePolly.GetPolicy();
    combinedPolicy.Execute(() => {
      var response = myTest.Test();
      Console.WriteLine(response);
    });
  }
}
