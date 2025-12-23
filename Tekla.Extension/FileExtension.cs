using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Helps to work with files in Tekla model
    /// </summary>
    public static class FileExtension
    {
        /// <summary>
        /// The name of the attributes folder.
        /// </summary>
        public const string AttributeFolder = "attributes";
        /// <summary>
        /// Return only name of files for loading attributes
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<string> GetFilesNames(string searchPattern)
        {
            HashSet<string> files = new();

            string projectPaths = string.Empty;
            string systemPaths = string.Empty;
            string firmPaths = string.Empty;
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_SYSTEM", ref systemPaths);
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_FIRM", ref firmPaths);
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_PROJECT", ref projectPaths);

            GetFiles(files, searchPattern, projectPaths, true);
            GetFiles(files, searchPattern, systemPaths, true);
            GetFiles(files, searchPattern, firmPaths, true);

            string modelPath = Path.Combine(new Model().GetInfo().ModelPath, AttributeFolder);
            GetFiles(files, searchPattern, modelPath, true);
            var sortedSet = files.ToList();
            sortedSet.Sort();
            return sortedSet;
        }
        /// <summary>
        /// Return full path of file
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<string> GetFilesFull(string searchPattern)
        {
            HashSet<string> files = new();

            string projectPaths = string.Empty;
            string systemPaths = string.Empty;
            string firmPaths = string.Empty;
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_SYSTEM", ref systemPaths);
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_FIRM", ref firmPaths);
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_PROJECT", ref projectPaths);

            GetFiles(files, searchPattern, projectPaths, false);
            GetFiles(files, searchPattern, systemPaths, false);
            GetFiles(files, searchPattern, firmPaths, false);

            string modelPath = Path.Combine(new Model().GetInfo().ModelPath, AttributeFolder);
            GetFiles(files, searchPattern, modelPath, false);
            return files;
        }

        private static void GetFiles(HashSet<string> hashSet, string searchPattern, string currentPaths, bool isOnlyFilename)
        {
            if (currentPaths.Length == 0)
                return;

            string[] paths = currentPaths.Split(';');
            foreach (string path in paths)
            {
                if (!Directory.Exists(path))
                    continue;

                string[] filesPath = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
                foreach (string file in filesPath)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);

                    if (isOnlyFilename)
                        hashSet.Add(fileName);
                    else
                        hashSet.Add(file);
                }
            }
        }
    }
}
