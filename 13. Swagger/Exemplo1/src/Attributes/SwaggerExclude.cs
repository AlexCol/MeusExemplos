namespace Exemplo1.src.Attributes;

public class SwaggerExclude {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SwaggerExcludeAttribute : Attribute {
    }
}
