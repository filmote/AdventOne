using System;
using System.Reflection;
using System.Web;

namespace AdventOne.Utils {

    public static class MimeType {

        // The get mime mapping method info
        private static readonly MethodInfo _getMimeMappingMethod = null;

        /// <summary>
        /// Static constructor sets up reflection.
        /// </summary>
        static MimeType() {
            // Load hidden mime mapping class and method from System.Web
            var assembly = Assembly.GetAssembly(typeof(HttpApplication));
            Type mimeMappingType = assembly.GetType("System.Web.MimeMapping");
            _getMimeMappingMethod = mimeMappingType.GetMethod("GetMimeMapping",
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        }

        /// <summary>
        /// Exposes the hidden Mime mapping method.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The mime mapping.</returns>
        public static string GetMimeMapping(string fileName) {
            return (string)_getMimeMappingMethod.Invoke(null /*static method*/, new[] { fileName });
        }
    }
}