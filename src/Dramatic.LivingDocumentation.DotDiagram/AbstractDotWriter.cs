//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System.IO;
using System.Text;

namespace Dramatic.LivingDocumentation.DotDiagram
{

	public abstract class AbstractDotWriter : IDotWriter
	{
		public abstract string Folder {get;}
		public abstract string ImageExtension {get;}

		public virtual void WriteDot(string dotFilename, string dotContent)
		{
            string outputFileName = System.IO.Path.Combine(Folder, dotFilename, ".dot");
            //string outputFileName = Path + filename + ".dot";

            // Write the dot content tot the file
            using (var sw = new System.IO.StreamWriter(File.Open(outputFileName, FileMode.CreateNew), Encoding.GetEncoding("iso-8859-1")))
            {
                sw.Write(dotContent);
                sw.Flush();
            }
        }

		/// <summary>
		/// All-in-on convenience method
		/// </summary>
		/// <returns> The filename of the dot-generated picture for the given content </returns>
		public virtual string ToImage(string filename, string dotContent)
		{
			WriteDot(filename, dotContent);

			Render(filename);

			return filename + ImageExtension;
		}

        public abstract void Render(string filename);

    }

}