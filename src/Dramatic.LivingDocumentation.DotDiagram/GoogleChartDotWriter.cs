//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

namespace io.github.livingdocumentation.dotdiagram
{
	using HttpRequest = com.github.kevinsawicki.http.HttpRequest;


	public class GoogleChartDotWriter : AbstractDotWriter
	{

		private const string GOOGLE_CHART_API = "http://chart.googleapis.com/chart";
		private readonly string path;

		/// <param name="path">
		///            The path to dot and png files, must end with a slash </param>
		public GoogleChartDotWriter(string path)
		{
			this.path = path;
		}

		public override string Path
		{
			get
			{
				return path;
			}
		}

		public override string ImageExtension
		{
			get
			{
				return ".png";
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void render(String filename) throws InterruptedException, IOException
		public override void render(string filename)
		{
			string dot = read(path + filename + ".dot");
			HttpRequest httpRequest = HttpRequest.get(GOOGLE_CHART_API, true, "cht", "gv", "chl", dot);
			if (httpRequest.ok())
			{
				using (System.IO.Stream @is = httpRequest.stream())
				{
					Files.copy(@is, Paths.get(path + filename + ImageExtension));
				}
			}
			else
			{
				throw new DotDiagramException("Errors calling Graphviz chart.googleapis.com");
			}
		}


//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static String read(String input) throws IOException
		private static string read(string input)
		{
			using (System.IO.StreamReader buffer = new System.IO.StreamReader(new System.IO.FileStream(input, System.IO.FileMode.Open, System.IO.FileAccess.Read)))
			{
				return buffer.lines().filter(l => !l.Trim().StartsWith("//")).filter(l => !l.Trim().StartsWith("#")).collect(Collectors.joining("\n")); //ignore comment -  ignore comment
			}
		}

		public override string ToString()
		{
			return "GoogleChartDotWriter path=" + path + " imageExtension=" + ImageExtension;
		}
	}

}