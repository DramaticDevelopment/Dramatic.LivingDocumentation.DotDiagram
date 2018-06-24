//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dramatic.LivingDocumentation.DotDiagram
{

	/// <summary>
	/// A simple API to generate Dot (Graphviz) files from a tree of Node and
	/// Associations. The output is the content of a dot file.
	/// </summary>
	public sealed class DotGraph : Renderable
	{
		private const string NODE_ID_PREFIX     = "c";
		private const string CLUSTER_PREFIX     = "cluster_";

		private readonly AbstractNode _root;
		private readonly NodeRegistry _registry = new NodeRegistry();

		public DotGraph(string title)
		{
			this._root = new Digraph(_registry, title);
		}

		public DotGraph(string title, string direction)
		{
			this._root = new Digraph(_registry, title, direction);
		}

		public Digraph GetDigraph()
		{
			return (Digraph) _root;
		}

		public string PreloadNode(object id)
		{
			return _registry.NodeUniqueId(id);
		}

		public string Render()
		{
			return _root.Render();
		}

		public override string ToString()
		{
			return "DotGraph root: " + _root;
		}

		/// <summary>
		/// Stores unique node id by String node key
		/// </summary>
		public sealed class NodeRegistry
		{
			internal readonly IDictionary<object, string>   _nodeUids   = new Dictionary<object, string>();
			static int                                    _count      = 0;

			public string ExistingUniqueId(object id)
			{
                string uid = null;

                if (_nodeUids.ContainsKey(id))
                {
                    uid = _nodeUids[id];
                }
                
                return uid;
			}

			public string NodeUniqueId(object id)
			{
                if (_nodeUids.ContainsKey(id))
                {
                    return _nodeUids[id];
                }

                string nodeUid  = NODE_ID_PREFIX + _count++;
				_nodeUids[id]   = nodeUid;

				return nodeUid;
			}

			public override string ToString()
			{
				return "NodeRegistry: " + _nodeUids.Count + " nodes registered";
			}
		}

		/// <summary>
		/// Represents any abstract node (digraph, cluster, node, record cell)
		/// </summary>
		public abstract class AbstractNode : Renderable
		{
			protected internal readonly NodeRegistry                    _registry;
			protected internal readonly string                          _id;
			protected internal string                                   _comment;
			protected internal string                                   _label;
			protected internal string                                   _options;
			protected internal readonly ICollection<string>             _stereotypes    = new HashSet<string>();
            protected internal readonly Dictionary<String, Renderable>  _nodes          = new Dictionary<String, Renderable>();
            protected internal readonly ICollection<object>             _associations   = new HashSet<object>();

            public abstract string Render();


            public AbstractNode(NodeRegistry registry, string id)
			{
				this._registry  = registry;
				this._id        = id;
			}

			public virtual string Comment
			{
				get
				{
					return _comment;
				}
			}

			public virtual AbstractNode SetComment(string comment)
			{
				this._comment = comment;

				return this;
			}

			public virtual Node AddPossibleNode(object id)
			{
				string uid = _registry.ExistingUniqueId(id);
                if (String.IsNullOrEmpty(uid))
				{
					return null;
				}

				Node node = (Node) _nodes[uid];
				if (node == null)
				{
					node = new Node(_registry, uid);
					_nodes[uid] = node;
				}

				return node;
			}

			public virtual Node AddNode(object id)
			{
				string uid = _registry.NodeUniqueId(id);

                Node node;
                if (!_nodes.ContainsKey(uid))
                { 
					node = new Node(_registry, uid);
					_nodes[uid] = node;
				}
                else
                {
                    node = (Node)_nodes[uid];
                }

				return node;
			}

			public virtual Cluster AddCluster(object id)
			{
				string uid          = _registry.NodeUniqueId(id);
                Cluster clusterNode = null;

                if (_nodes.ContainsKey(uid))
                {
                    clusterNode = (Cluster)_nodes[uid];
                }
                else
				{
                    clusterNode = new Cluster(_registry, uid);
					_nodes[uid] = clusterNode;
				}

				return clusterNode;
			}

			public virtual AbstractNode AddStereotype(string stereotype)
			{
				_stereotypes.Add(DotRenderer.Stereotype(stereotype));
				return this;
			}

			public virtual Association AddExistingAssociation(object sourceId, object targetId)
			{
				string uid  = _registry.ExistingUniqueId(sourceId);
				string uid2 = _registry.ExistingUniqueId(targetId);

                if (!String.IsNullOrEmpty(uid) && !String.IsNullOrEmpty(uid2))
				{
					Association association = new Association(uid, uid2);
                    _associations.Add(association);

					return association;
				}

				return null;
			}

			public virtual Association AddExistingAssociation(object sourceId, object targetId, string label, string comment, string options)
			{
				string uid  = _registry.ExistingUniqueId(sourceId);
				string uid2 = _registry.ExistingUniqueId(targetId);

                if (!String.IsNullOrEmpty(uid) && !String.IsNullOrEmpty(uid2))
				{
					Association association = new Association(uid, uid2);
					_associations.Add(association);

                    if (!String.IsNullOrEmpty(label))
					{
						association.SetLabel(Label);
					}

                    if (!String.IsNullOrEmpty(comment))
					{
						association.SetComment(comment);
					}

                    if (!String.IsNullOrEmpty(options))
					{
						association.SetOptions(options);
					}

					return association;
				}
				return null;
			}

			public virtual Association AddAssociation(object sourceId, object targetId)
			{
				string uid  = _registry.NodeUniqueId(sourceId);
				string uid2 = _registry.NodeUniqueId(targetId);

                if (!String.IsNullOrEmpty(uid) && !String.IsNullOrEmpty(uid2))
				{
					Association association = new Association(uid, uid2);
					_associations.Add(association);

					return association;
				}
				return null;
			}

			public virtual string Label
			{
				get
				{
					return _label;
				}
			}

			public virtual AbstractNode SetLabel(string label)
			{
				this._label = label;
				return this;
			}

			public virtual string Options
			{
				get
				{
					return _options;
				}
			}

			public virtual AbstractNode SetOptions(string options)
			{
				this._options = options;
				return this;
			}

			public virtual ICollection<object> Associations
			{
				get
				{
					return _associations;
				}
			}

			public virtual string Id
			{
				get
				{
					return _id;
				}
			}

			public virtual ICollection<string> Stereotypes
			{
				get
				{
					return _stereotypes;
				}
			}

			protected internal virtual void RenderAssociations(StringBuilder @out)
			{
				System.Collections.IEnumerator it = _associations.GetEnumerator();
				while (it.MoveNext())
				{
					Renderable renderable = (Renderable) it.Current;
					@out.Append(renderable.Render());
				}
			}

			protected internal virtual void RenderNodes(StringBuilder @out)
			{
                List<Renderable> values     = new List<Renderable>((ICollection<Renderable>)_nodes.Values);
                IComparer<Renderable> comp  = new ComparatorAnonymousInnerClass(this);

				values.Sort(comp);

				IEnumerator it = values.GetEnumerator();				
                while (it.MoveNext())
				{
					Renderable renderable = (Renderable) it.Current;
					@out.Append(renderable.Render());
				}
			}

			private class ComparatorAnonymousInnerClass :  IComparer<Renderable> // Comparator<Renderable>
			{
				private readonly AbstractNode _outerInstance;

				public ComparatorAnonymousInnerClass(AbstractNode outerInstance)
				{
					this._outerInstance = outerInstance;
				}


				public virtual int Compare(Renderable r1, Renderable r2)
				{
					return r1.ToString().CompareTo(r2.ToString());
				}
			}

			public override string ToString()
			{
				return "Node";
			}

			/// <returns> true if this Node is equal to the given Node </returns>
			public override bool Equals(object arg0)
			{
				if (!(arg0 is AbstractNode))
				{
					return false;
				}

				AbstractNode other = (AbstractNode) arg0;
				if (this == other)
				{
					return true;
				}

				return other._id.Equals(_id);
			}

			public override int GetHashCode()
			{
				return _id.GetHashCode();
			}

		}

		/// <summary>
		/// Represents an actual node (dot node element)
		/// </summary>
		public sealed class Node : AbstractNode
		{

			public Node(NodeRegistry registry, string id) : base(registry, id)
			{
			}

			public override string Render()
			{
                if (String.IsNullOrEmpty(_label))
                {
                    return string.Empty;
                }

				StringBuilder outbuffer = new StringBuilder();

                if (!String.IsNullOrEmpty(_comment))
				{
                    outbuffer.Append(DotRenderer.WithDotNewLine(_comment));
				}

				IList<string> cells = new List<string>();
				cells.Add(_label);

				if (_stereotypes.Count > 0)
				{
					((List<string>)cells).AddRange(_stereotypes);
				}

				string content = DotRenderer.ToLines((List<string>)cells);

				string wrapText = DotRenderer.WrapText(content, 20);
                outbuffer.Append(DotRenderer.Node(_id, wrapText, _options));

				IEnumerator it = _associations.GetEnumerator();
				while (it.MoveNext())
				{
					AbstractAssociation abstractAssociation = (AbstractAssociation) it.Current;

                    outbuffer.Append(abstractAssociation.Render());
				}

				return outbuffer.ToString();
			}

			public override string ToString()
			{
				return "Node" + _id;
			}
		}

		/// <summary>
		/// Represents a dot Digraph element
		/// </summary>
		public sealed class Digraph : AbstractNode
		{

			internal readonly string _dir;

			public Digraph(NodeRegistry registry, string title) : this(registry, title, null)
			{
			}

			public Digraph(NodeRegistry registry, string title, string dir) : base(registry, title)
			{
				this._dir = dir;

				SetLabel(title);
			}

			public override string Render()
			{
				StringBuilder outbuffer = new StringBuilder();

				outbuffer.Append(DotRenderer.OpenGraph(_label, _dir));
				RenderNodes(outbuffer);
				RenderAssociations(outbuffer);

				outbuffer.Append(DotRenderer.CloseGraph());

				return outbuffer.ToString();
			}

			public override string ToString()
			{
				return "Digraph " + _id;
			}

			public AbstractNode FindNode(string identifier)
			{
				string uid      = _registry.NodeUniqueId(_id);
				IEnumerator it  = _nodes.Values.GetEnumerator();

				while (it.MoveNext())
				{
					AbstractNode node = (AbstractNode) it.Current;

					if (uid.Equals(node.Id))
					{
						return node;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Represents a Node in a dot diagram
		/// </summary>
		public sealed class Cluster : AbstractNode
		{

			public Cluster(NodeRegistry registry, string id) : base(registry, id)
			{
			}

			public override string Render()
			{
				StringBuilder outbuffer = new StringBuilder();
				IList<string> cells     = new List<string>();

				if (!String.IsNullOrEmpty(_label))
				{
					cells.Add(_label);
				}

				((List<string>)cells).AddRange(_stereotypes);
				string content = DotRenderer.ToLines((List<string>)cells);

				outbuffer.Append(DotRenderer.OpenCluster(CLUSTER_PREFIX + _id));
				outbuffer.Append(DotRenderer.Cluster(content));

				RenderNodes(outbuffer);
				RenderAssociations(outbuffer);

				outbuffer.Append(DotRenderer.CloseCluster());
				
                return outbuffer.ToString();
			}

			public override string ToString()
			{
				return "Cluster " + _id;
			}

		}

		/// <summary>
		/// Represents any association in a dot diagram
		/// 
		/// @author cyrille martraire
		/// </summary>
		public abstract class AbstractAssociation : Renderable
		{
			public abstract string Render();
			protected internal readonly string _sourceId;

			protected internal readonly string _targetId;

			protected string _label;

			protected internal string _comment;

			protected internal string _options;

			public AbstractAssociation(string sourceId, string targetId)
			{
				this._sourceId = sourceId;
				this._targetId = targetId;
			}

			public virtual string Label
			{
				get
				{
					return _label;
				}
			}


			public virtual AbstractAssociation SetLabel(string label)
			{
				if (!string.ReferenceEquals(label, null) && label.Length > 0)
				{
					this._label = label;
				}

				return this;
			}

			public virtual string Comment
			{
				get
				{
					return _comment;
				}
			}

			public virtual AbstractAssociation SetComment(string comment)
			{
				this._comment = comment;
				return this;
			}

			public virtual string Options
			{
				get
				{
					return _options;
				}
			}

			public virtual AbstractAssociation SetOptions(string options)
			{
				this._options = options;
				return this;
			}

			public virtual string TargetId
			{
				get
				{
					return _targetId;
				}
			}

			/// <returns> true if this Association is equal to the given Association </returns>
			public override bool Equals(object arg0)
			{
				if (!(arg0 is AbstractAssociation))
				{
					return false;
				}

				AbstractAssociation other = (AbstractAssociation) arg0;
				if (this == other)
				{
					return true;
				}

				return other._sourceId.Equals(_sourceId) && other._targetId.Equals(_targetId);
			}

			public override int GetHashCode()
			{
				return _sourceId.GetHashCode() ^ _targetId.GetHashCode();
			}

		}

		/// <summary>
		/// Represents an association from a node A to a node B
		/// </summary>
		public class Association : AbstractAssociation
		{

			public Association(string sourceId, string targetId) : base(sourceId, targetId)
			{
			}

			public override string Render()
			{
				StringBuilder outbuffer = new StringBuilder();
				string displayLabel     = _label == null ? null : "label=\"" + _label + "\"";

				outbuffer.Append(DotRenderer.Edge(_sourceId, _targetId, _comment, displayLabel, _options));

				return outbuffer.ToString();
			}

			public override string ToString()
			{
				return "Association from " + _sourceId + " to " + _targetId;
			}
		}

	}

}