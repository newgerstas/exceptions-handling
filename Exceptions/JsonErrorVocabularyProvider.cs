using System;
using System.Collections;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ExceptionStrategy.Exceptions
{
    public class JsonErrorVocabularyProvider : IErrorVocabularyProvider
    {
        private readonly string catalogPath;
        private readonly string defaultCulture;

        public JsonErrorVocabularyProvider(string catalogPath, string defaultCulture = "en-US")
        {
            this.catalogPath = catalogPath ?? throw new ArgumentNullException(nameof(catalogPath));
            this.defaultCulture = defaultCulture ?? throw new ArgumentNullException(nameof(defaultCulture));
        }

        public IErrorVocabulary Create(string culture)
        {
            var filesToLookup = new[] {
                culture ?? defaultCulture,
                defaultCulture
            }
            .Select(p => Path.Combine(catalogPath, p + ".json"))
            .Distinct();

            var filePath = filesToLookup.FirstOrDefault(p => File.Exists(p));

            if (filePath == null)
            {
                throw new FileNotFoundException(
                    $"No json error vocabulary file is found in catalog '{catalogPath}. Looked up by:"
                     + $"{Environment.NewLine}{string.Join(Environment.NewLine, filesToLookup)}'");
            }

            return new JsonErrorVocabulary(filePath);
        }

        private class JsonErrorVocabulary : IErrorVocabulary
        {
            private readonly Lazy<JObject> root;

            public JsonErrorVocabulary(string pathToFile)
            {
                if (string.IsNullOrEmpty(pathToFile))
                {
                    throw new System.ArgumentException("Path can't be empty or null", nameof(pathToFile));
                }

                root = new Lazy<JObject>(() => JObject.Parse(File.ReadAllText(pathToFile)));
            }

            public ErrorInfo Translate(ABCException exception)
            {
                var node = root.Value;
                int status = 0;
                string message = null;

                // go down the tree following path in error code
                foreach (var key in exception.Code)
                {
                    if (node.ContainsKey(key))
                    {
                        if (node[key] is JObject obj)
                        {
                            // if we found child node - keep going down
                            node = obj;

                            // on every level of the tree try to get basic info or keep upper level if not found
                            status = TryGetValue<int?>(node, "$status") ?? status;
                            message = TryGetValue<string>(node, "$message") ?? message;

                            continue;
                        }
                        else if (node[key] is JValue val)
                        {
                            // if not a child node - return value assuming it is convertable to string
                            message = "" + val.Value;
                            break;
                        }
                    }

                    // if no child node is found by key - break the loop and return whatever is found for now
                    break;
                }

                return new ErrorInfo
                {
                    Status = status,
                    Message = BindProps(message, exception.Data)
                };
            }

            private static string BindProps(string template, IDictionary props)
            {
                return props.Keys
                    .OfType<object>()
                    .Select(p => (key: p, val: props[p]))
                    .Aggregate(template, (t, p) => t.Replace("{" + p.key + "}", p.val?.ToString()));
            }

            private static T TryGetValue<T>(JObject obj, string name)
            {
                return obj.ContainsKey(name)
                    ? obj[name].Value<T>()
                    : default(T);
            }
        }
    }
}