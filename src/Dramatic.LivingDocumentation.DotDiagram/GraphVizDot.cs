using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dramatic.LivingDocumentation.DotDiagram
{
    public class GraphVizDot
    {
        public static string ResolveDotEnginePath()
        {
            var dotPaths = new string[] {
                @"C:\Program Files\NuGet\Packages\Graphviz*\dot.exe",
                @"C:\program files*\GraphViz*\bin\dot.exe",
            };

            // Try to resolve the dot.exe TODO


            return @"C:\Program Files\GraphViz\bin\dot.exe";
        }

        /// <summary>
        /// Renders the dot content to a new image file (in the temporary directory) and returns the file path.
        /// </summary>
        /// <param name="dotContent"></param>
        /// <param name="imageExtension"></param>
        /// <returns></returns>
        public static string RenderImage(string dotContent, string imageExtension)
        {
            if (String.IsNullOrEmpty(imageExtension)) { throw new ArgumentNullException("imageExtension"); }

            string dotEngine            = ResolveDotEnginePath();

            string imageTempFilePath    = Path.GetTempFileName() + "." + imageExtension;
            string extension            = imageExtension.Trim(' ', '.').ToLowerInvariant();
            string dotContentPath       = imageTempFilePath + ".dotcontent";

            try
            {
                // Write the dot content to a file.
                using (var sw = new System.IO.StreamWriter(File.Open(dotContentPath, FileMode.OpenOrCreate), Encoding.GetEncoding("iso-8859-1")))
                {
                    sw.Write(dotContent);
                    sw.Flush();
                }

                System.Diagnostics.Process process          = new System.Diagnostics.Process();

                // Stop the process from opening a new window
                process.StartInfo.RedirectStandardOutput    = true;
                process.StartInfo.UseShellExecute           = false;
                process.StartInfo.CreateNoWindow            = true;

                // Setup executable and parameters
                process.StartInfo.FileName                  = dotEngine;
                process.StartInfo.Arguments                 = String.Format("\"{0}\" -T{1} -o \"{2}\" -Gdpi=72 -Gsize=\"6, 8.5\"", dotContentPath, extension, imageTempFilePath);

                // Go
                process.Start();

                // and wait dot.exe to complete and exit
                process.WaitForExit();
            }
            finally
            {
                // Remove the temp file again
                if (!String.IsNullOrEmpty(dotContentPath) && File.Exists(dotContentPath))
                {
                    File.Delete(dotContentPath);
                }
            }

            return imageTempFilePath;
        }
    }
}
