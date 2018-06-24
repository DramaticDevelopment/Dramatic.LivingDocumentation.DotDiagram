
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dramatic.LivingDocumentation.DotDiagram;


namespace TESTER
{
    class Program
    {
        static void Main(string[] args)
        {

            DotGraph graph              = new DotGraph("simple test");
            DotGraph.Digraph digraph    = graph.GetDigraph();

            digraph.AddNode("Car").SetLabel("My Car 2").SetComment("This is BMW 2").SetOptions(DotStyles.STUB_NODE_OPTIONS);
            digraph.AddNode("Wheel").SetLabel("Its wheels").SetComment("The wheels of my car");
            digraph.AddAssociation("Car", "Wheel").SetLabel("4*").SetComment("There are 4 wheels").SetOptions(DotStyles.ASSOCIATION_EDGE_STYLE);

            string dot = graph.Render();

            // Create an image from the dot content
            var imageFilePath = GraphVizDot.RenderImage(dot, "png");
            if (File.Exists(imageFilePath))
            {
                Console.WriteLine($"Opening {imageFilePath}...");
                System.Diagnostics.Process.Start(imageFilePath);
            }

            Console.WriteLine("Press enter to exit;");
            Console.ReadLine();
        }
    }
}
