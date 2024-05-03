namespace Exemplo1.src.Attributes;

public class SwaggerInclude {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SwaggerIncludeAttribute : Attribute {
    }
}
