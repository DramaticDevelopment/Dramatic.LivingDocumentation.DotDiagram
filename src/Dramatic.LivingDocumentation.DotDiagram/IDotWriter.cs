//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

namespace Dramatic.LivingDocumentation.DotDiagram
{

	public interface IDotWriter
	{
		void Render(string filename);
		void WriteDot(string dotFilename, string dotContent);
		string ToImage(string filename, string content);
	}
}