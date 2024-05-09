using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimeiroExemplo.src.Conversores;

public static class MyJson {
    public static string ObjToJson<T>(T obj) {
        return JsonSerializer.Serialize(obj);
    }
    public static T JsonToObj<T>(string jSon) {
        return JsonSerializer.Deserialize<T>(jSon);
    }
}
