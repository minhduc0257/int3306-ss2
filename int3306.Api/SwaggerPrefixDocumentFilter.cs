using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace int3306.Api
{
    public class SwaggerPrefixDocumentFilter : IDocumentFilter
    {
        private readonly string prefix;
        public SwaggerPrefixDocumentFilter(string prefix)
        {
            this.prefix = prefix;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.Keys.ToList();
            foreach (var path in paths)
            {
                var pathToChange = swaggerDoc.Paths[path];
                swaggerDoc.Paths.Remove(path);
                swaggerDoc.Paths.Add("/" + prefix + path, pathToChange);
            }
        }
    }
}