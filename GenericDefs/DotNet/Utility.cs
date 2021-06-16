using System;
using System.IO;
using System.Reflection;

namespace GenericDefs.DotNet
{
    public class Utility
    {
        /// <summary>
        /// Takes the full name of a resource and loads it in to a stream.
        /// </summary>
        /// <param name="resourceName">Assuming an embedded resource is a file
        /// called info.png and is located in a folder called Resources, it
        /// will be compiled in to the assembly with this fully qualified
        /// name: Full.Assembly.Name.Resources.info.png. That is the string
        /// that you should pass to this method.</param>
        /// <param name="embeddedInCallerAssembly">Use null if you dont know where the resource is w.r.t. the executing/calling assembly.</param>
        /// <returns></returns>
        public static Stream GetEmbeddedResourceStream(string resourceName, bool? embeddedInCallerAssembly)
        {
            if (embeddedInCallerAssembly.HasValue) {
                if (embeddedInCallerAssembly.Value) return Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);
                else return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            } else {
                var assemblyStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                if (assemblyStream == null && Assembly.GetCallingAssembly() != null) assemblyStream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);
                if(assemblyStream == null && Assembly.GetEntryAssembly() != null) assemblyStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName);
                if (assemblyStream == null) throw new Exception("Resource not found in Executing, Calling, Entry assemblies. Resource Name : " + resourceName);
                return assemblyStream;
            }
        }

        public static Stream GetEmbeddedResourceStream(string resourceName, Assembly assembly)
        {
            if (assembly == null) { throw new ArgumentNullException("Assembly cannot be null."); }
            else { return assembly.GetManifestResourceStream(resourceName); }
        }

        /// <summary>
        /// Get the list of all emdedded resources in the assembly.
        /// </summary>
        /// <returns>An array of fully qualified resource names</returns>
        public static string[] GetEmbeddedResourceNames()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames();
        }
    }

    public class StreamHelper
    {
        public static string GetDynamicallyBuiltAssembliesPath(bool isInCallingAssembly)
        {
            string retVal = string.Empty;
            using (var stream = Utility.GetEmbeddedResourceStream("AppRunner.dllpath.txt", isInCallingAssembly))
            {
                using (var sr = new StreamReader(stream))
                {
                    retVal = sr.ReadToEnd().Trim();
                }
            }

            return retVal;
        }

        public static string GetDynamicallyBuiltAssembliesPath(Assembly assembly)
        {
            string retVal = string.Empty;
            using (var stream = Utility.GetEmbeddedResourceStream("AppRunner.dllpath.txt", assembly))
            {
                using (var sr = new StreamReader(stream))
                {
                    retVal = sr.ReadToEnd().Trim();
                }
            }

            return retVal;
        }

        public static string GetSolutionPath()
        {
            string retVal = string.Empty;
            bool? nullBool = null;
            Assembly a = Assembly.GetEntryAssembly();
            string pName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (a == null)
            {
                return "D:/Work/Brilliant/";
            }

            try
            {
                var stream = Utility.GetEmbeddedResourceStream(a.GetName().Name + ".solutionpath.txt", nullBool);
            }
            catch
            {
                return "D:/Work/Brilliant/";
            }

            using (var stream = Utility.GetEmbeddedResourceStream(a.GetName().Name + ".solutionpath.txt", nullBool))
            {
                using (var sr = new StreamReader(stream))
                {
                    retVal = sr.ReadToEnd().Trim();
                }
            }

            return retVal;
        }
    }
}