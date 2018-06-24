//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

namespace Dramatic.LivingDocumentation.DotDiagram
{

	/// <summary>
	/// The resource bundle for UML styles for Graphviz Dot
	/// </summary>
	public class DotStyles
	{
		private DotStyles()
		{ }

		public static readonly string CLASS_NODE_OPTIONS            = "fontname=\"Verdana\",fontsize=9,shape=record";
		public static readonly string NOTE_NODE_OPTIONS             = "style=filled,fillcolor=grey,fontname=\"Verdana\",fontsize=9";
		public static readonly string STUB_NODE_OPTIONS             = "color=grey,fontcolor=grey,fontname=\"Verdana\",fontsize=9";
		public static readonly string COLLABORATION_NODE_OPTIONS    = "shape=ellipse,style=dotted,fontname=\"Verdana\",fontsize=9";
		public static readonly string ELLIPSIS_NODE_OPTIONS         = "shape=plaintext";

		// ---
		public static readonly string ASSOCIATION_EDGE_STYLE        = "arrowhead=open";
		public static readonly string INSTANTIATION_EDGE_STYLE      = "arrowhead=open,style=dashed";
		public static readonly string IMPLEMENTS_EDGE_STYLE         = "back,arrowtail=empty,style=dashed";
		public static readonly string EXTENDS_EDGE_STYLE            = "dir=back,arrowtail=empty";
		public static readonly string NOTE_EDGE_STYLE               = "arrowhead=none,style=dashed";
		public static readonly string COLLABORATION_EDGE_STYLE      = "arrowhead=none,style=dotted";
		public static readonly string CLIENT_EDGE_STYLE             = "dir=back,arrowhead=open,arrowtail=empty";
	}

}