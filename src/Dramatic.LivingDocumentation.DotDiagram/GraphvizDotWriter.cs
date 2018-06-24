//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.IO;
using System.Text;

namespace Dramatic.LivingDocumentation.DotDiagram
{

	/// <summary>
	/// Java wrapper around the Graphviz Dot grapher; requires Graphviz to be
	/// installed on the machine, and read/write access to the disk for temporary
	/// .dot files.
	/// </summary>
	public sealed class GraphvizDotWriter : AbstractDotWriter
	{

        public override string Folder           { get { return _folder; } }
        public override string ImageExtension   { get { return _imageExtension; }  }

        private string _folder;
		private string _dotPath;
		private string _imageExtension;
		private string _commandTemplate;


		/// <param name="path">
		///            The path to dot and png files, must end with a slash </param>
		/// <param name="dotPath">
		///            The path to the dot executable, must end with a slash </param>
		/// <param name="cmd">
		///            The command-line template, e.g.
		///            "$0 dot -Tpng $1.dot -o $1.png -Gdpi=72 -Gsize="6,8.5"" </param>
		public GraphvizDotWriter(string path, string imageExtension, string cmdTemplate)
		{
            var dotPaths = new string[] {
                @"C:\Program Files\NuGet\Packages\Graphviz*\dot.exe",
                @"C:\program files*\GraphViz*\bin\dot.exe",
            };

            // Try to resolve the dot.exe TODO


            this._dotPath = @"C:\Program Files\GraphViz\bin\dot.exe";


            this._folder              = path;
			this._imageExtension    = imageExtension;
			this._commandTemplate   = cmdTemplate;
		}

		public string CommandTemplate
		{
			get
			{
				return _commandTemplate;
			}
		}



        /*
         * 
private static string GenDiagramFile(string pathToDotFile)
{
    var diagramFile = pathToDotFile.Replace(".dot", ".png");

    ExecuteCommand("dot", string.Format(@"""{0}"" -o ""{1}"" -Tpng", 
                 pathToDotFile, diagramFile));

    return diagramFile;
}

private static void ExecuteCommand(string command, string @params)
{
    Process.Start(new ProcessStartInfo(command, @params) {CreateNoWindow = true, UseShellExecute = false });
}
         */


        public override void Render(string filename)
		{
            string fullFilename = Path.Combine(this._folder, filename, ".dot");


            string dotContent = File.ReadAllText(fullFilename);


            string output = @".\external\tempgraph";
            File.WriteAllText(output, dot);

            System.Diagnostics.Process process          = new System.Diagnostics.Process();

            // Stop the process from opening a new window
            process.StartInfo.RedirectStandardOutput    = true;
            process.StartInfo.UseShellExecute           = false;
            process.StartInfo.CreateNoWindow            = true;

            // Setup executable and parameters
            process.StartInfo.FileName                  = this._dotPath;
            process.StartInfo.Arguments                 = string.Format(@"{0} -Tpng -O", output);

            // Go
            process.Start();
            // and wait dot.exe to complete and exit
            process.WaitForExit();




            object[] args = new object[] {_dotPath, _folder + filename};

			string command = MessageFormat.format(_commandTemplate, args);

			Process p = Runtime.Runtime.exec(command);

			System.IO.StreamReader reader = new System.IO.StreamReader(p.ErrorStream);

			string newLine = string.Format("%n");

			StringBuilder errorMessage = new StringBuilder();
			string line = null;
			while (!string.ReferenceEquals((line = reader.ReadLine()), null))
			{
				errorMessage.Append(newLine);
				errorMessage.Append(line);
			}
			int result = p.waitFor();
			if (result != 0)
			{
				throw new DotDiagramException("Errors running Graphviz on " + filename + ".dot" + errorMessage);
			}
		}

		public override string ToString()
		{
			return "GraphvizDotWriter path=" + _folder + " dot-path=" + _dotPath + " imageExtension=" + _imageExtension;
		}
	}

}