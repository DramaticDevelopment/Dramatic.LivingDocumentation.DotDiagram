using Dramatic.LivingDocumentation.DotDiagram;
using Structurizr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dramatic.Structurizr.IO.Dot
{
    public class DotWriter
    {
        public void Write(Workspace workspace, StringWriter writer)
        {
            foreach (var view in workspace.Views.SystemContextViews)    { Write(view, null, writer); }
            foreach (var view in workspace.Views.ContainerViews)        { Write(view, view.SoftwareSystem, writer); }
            foreach (var view in workspace.Views.ComponentViews)        { Write(view, view.Container, writer); }
        }


        public string Write(Workspace workspace)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                Write(workspace, writer);
            }

            return sb.ToString();
        }


        private void Write(View view, Element clusterElement, StringWriter writer)
        {
            try
            {
                DotGraph graph              = new DotGraph(view.Name);
                DotGraph.Digraph digraph    = graph.GetDigraph();
                DotGraph.Cluster cluster    = null;

                if (clusterElement != null)
                {
                    cluster = digraph.AddCluster(clusterElement.Id);
                    cluster.SetLabel(clusterElement.Name);
                }

                foreach (ElementView elementView in view.Elements)
                {
                    Element element = elementView.Element;

                    if (clusterElement != null && element.Parent == clusterElement)
                    {
                        cluster.AddNode(element.Id).SetLabel(element.Name);
                    }
                    else
                    {
                        digraph.AddNode(element.Id).SetLabel(element.Name);
                    }
                }

                foreach (RelationshipView relationshipView in view.Relationships)
                {
                    Relationship relationship = relationshipView.Relationship;
                    digraph.AddAssociation(relationship.SourceId, relationship.DestinationId).SetLabel(relationship.Description);
                }

                string output = graph.Render().Trim();
                writer.Write(output);
                writer.Write(Environment.NewLine);
                writer.Write(Environment.NewLine);
            }
            catch(Exception ex)
            {
                String message = ex.Message;
            }
        }
    }
}
