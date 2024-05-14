using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CriandoDLL;

public static class StringExtension {
    public static void PrintNoConsole(this string text) {
        Console.WriteLine(text);
    }
}
