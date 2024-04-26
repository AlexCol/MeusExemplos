using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Exemplo1.src.Testes;

namespace Exemplo1.src.Util;

public static class MyJsonOptions {
    public static JsonSerializerOptions GetOptions() {
        JsonSerializerOptions opt = new JsonSerializerOptions();
        opt.DictionaryKeyPolicy = new LowerCaseNamingPolicy();
        opt.PropertyNamingPolicy = new LowerCaseNamingPolicy();
        opt.PropertyNameCaseInsensitive = true;
        return opt;
    }
}
