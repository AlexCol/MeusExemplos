using System.ComponentModel.DataAnnotations;
using Polly.CircuitBreaker;
using PollyApp.src.MyPolicies;
using PollyApp.src.Runs;
using PollyApp.src.Services;



string baseUrl = "http://localhost:5028";
var myTest = new TestService(baseUrl);

//! testando Polly < 7
//Polly7.Run(myTest);

//! testando Polly >= 8
Polly8.Run(myTest);