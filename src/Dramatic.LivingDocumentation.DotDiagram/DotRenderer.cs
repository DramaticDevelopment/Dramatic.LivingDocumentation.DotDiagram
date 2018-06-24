//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Dramatic.LivingDocumentation.DotDiagram
{

	/// <summary>
	/// Renders a graph with basic UML diagrams elements into the dot syntax
	/// </summary>
	public class DotRenderer
	{

		private static readonly string NEWLINE  = Environment.NewLine; //string.Format("%n");
		protected internal const string TAB     = "\t";
		private const string OPEN_STEREOTYPE    = "\\<" + "\\<";
		private const string CLOSE_STEREOTYPE   = "\\>" + "\\>";
        private static int _digraphCounter       = 0;

		protected internal DotRenderer()
		{
		}

		public static string Fontname(string fontname)
		{
			return "fontname=\"" + fontname + "\"";
		}

		public static string Fontsize(int fontsize)
		{
			return "fontsize=" + fontsize;
		}

		public static string Options(string fontname, int fontsize)
		{
			return DotRenderer.Fontname(fontname) + "," + DotRenderer.Fontsize(fontsize);
		}

		public static string Options(bool isAbstract)
		{
			return Fontname(isAbstract ? "Verdana-Italic" : "Verdana") + ", " + Fontsize(9);
		}

		public static string Stereotype(string str)
		{
			return OPEN_STEREOTYPE + str + CLOSE_STEREOTYPE;
		}

		public static string OpenGraph(string title, string dir)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("# Class diagram ");
			sb.Append(title);
			sb.Append(NEWLINE);
			//sb.Append(String.Format("digraph G{0} {{", _digraphCounter++));
            sb.Append("digraph G {");

            if (!String.IsNullOrEmpty(title))
			{
				sb.Append(GraphTitle(title, dir));
			}

			sb.Append(OptionsEdge());
			sb.Append(OptionsNode());

			return sb.ToString();
		}

		public static string CloseGraph()
		{
			return NEWLINE + "}" + NEWLINE;
		}

		public static string OpenCluster(string id)
		{
			return NEWLINE + "subgraph " + id + " {";
		}

		public static string Cluster(string content)
		{
			return NEWLINE + "label = \"" + content + "\";";
		}

		public static string CloseCluster()
		{
			return NEWLINE + "}";
		}

		public static string WithDotNewLine(string s)
		{
			return NEWLINE + "//" + s;
		}

		public static string GraphTitle(string title, string dir)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(NEWLINE);
			sb.Append(TAB);
			sb.Append("graph");
			sb.Append(" ");
			sb.Append("[");
			sb.Append("labelloc=top,label=\"");
			sb.Append(title);
			sb.Append("\"");
			sb.Append(",");
			sb.Append(Options("Verdana", 12));

            if (!String.IsNullOrEmpty(dir))
			{
				sb.Append(",");
				sb.Append("rankdir=" + dir);
			}

			sb.Append("]");
			sb.Append(";");

			return sb.ToString();
		}

		public static string OptionsNode()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(NEWLINE);
			sb.Append(TAB);
			sb.Append("node");
			sb.Append(" ");
			sb.Append("[");
			sb.Append(Options("Verdana", 9));

			sb.Append(",");
			sb.Append("shape=record");

			sb.Append("]");
			sb.Append(";");

			return sb.ToString();
		}

		public static string OptionsEdge()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(NEWLINE);
			sb.Append(TAB);
			sb.Append("edge");
			sb.Append(" ");
			sb.Append("[");
			sb.Append(Options("Verdana", 9));

			sb.Append(",");
			sb.Append("labelfontname=\"");
			sb.Append("Verdana");
			sb.Append("\",labelfontsize=");
			sb.Append(9);

			sb.Append("]");
			sb.Append(";");

			return sb.ToString();
		}

		public static string Edge(string uniqueNameFrom, string uniqueNameTo, string comment, string labels, string edgeStyle)
		{
            if (String.IsNullOrEmpty(uniqueNameFrom) || String.IsNullOrEmpty(uniqueNameTo))
			{
				return String.Empty;
			}


			StringBuilder sb = new StringBuilder();

			sb.Append(NEWLINE);
			sb.Append(TAB);
			sb.Append("// ");
			sb.Append(comment);

			sb.Append(NEWLINE);
			sb.Append(TAB);
			sb.Append(uniqueNameFrom);
			sb.Append(" -> ");
			sb.Append(uniqueNameTo);
			sb.Append(" [");

            if (!String.IsNullOrEmpty(labels))
			{
				sb.Append(labels);
				sb.Append("  ");
				sb.Append(", ");
			}

            if (!String.IsNullOrEmpty(edgeStyle))
			{
				sb.Append(edgeStyle);
			}

			sb.Append("];");

			return sb.ToString();
		}

		public static string ToLines(List<string> cells)
		{
			return ToLines((string[]) cells.ToArray(), "\\n ", "");
		}

		private static string ToLines(string[] cells, string prefix, string postfix)
		{
			if (cells == null || cells.Length == 0)
			{
				return String.Empty;
			}
			
            StringBuilder sb = new StringBuilder();

			for (int i = 0; i < cells.Length; i++)
			{
				if (i > 0)
				{
					sb.Append(prefix);
				}

				sb.Append(cells[i]);
				sb.Append(postfix);
			}

			return sb.ToString();
		}

		public static string WrapText(string text, int length)
		{
			StringBuilder sb    = new StringBuilder();
			StringTokenizer st  = new StringTokenizer(text, new char[] { ',', '\t', '\n' });
			int lineLength      = 0;

			while (st.HasMoreTokens())
			{
				string token = st.NextToken();
				if (lineLength > length)
				{
					lineLength = 0;
					sb.Append("\\l");
				}

				sb.Append(token);
				lineLength += token.Length;
			}

			return sb.ToString();
		}

		public static string Node(string uniqueName, string label, string options)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(NEWLINE);
			sb.Append(TAB);
			sb.Append(uniqueName);
			sb.Append(" ");
			sb.Append("[");
			sb.Append("label=\"");
			sb.Append(label);
			sb.Append("\"");

            if (!String.IsNullOrEmpty(options))
			{
				sb.Append(", ");
				sb.Append(options);
			}

			sb.Append("]");

			return sb.ToString();
		}
	}
}