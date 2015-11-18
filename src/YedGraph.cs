namespace LessMefMess
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class YedGraph
    {
        private const string FORMAT = @"Creator ""yFiles""{0}Version ""2.12""{0}graph [{1}{0}]";

        private readonly ICollection<Node> _nodes;
        private readonly ICollection<Edge> _edges;

        public YedGraph()
        {
            _nodes = new List<Node>();
            _edges = new List<Edge>();
        }

        public void Add(Node node)
        {
            _nodes.Add(node);
        }

        public void Add(Edge edge)
        {
            _edges.Add(edge);
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            foreach (var node in _nodes)
            {
                output.Append(node);
            }
            foreach (var edge in _edges)
            {
                output.Append(edge);
            }

            return string.Format(FORMAT, Environment.NewLine, output);
        }

        public class Node
        {
            private const string NODE_FORMAT = @"{0}node [ id {1} label ""{2}"" graphics [ {3}]]";

            public int Id { get; set; }

            public string Label { get; set; }

            public string Graphics { get; set; }

            public override string ToString()
            {
                return string.Format(NODE_FORMAT, Environment.NewLine, Id, Label, Graphics);
            }
        }

        public class Edge
        {
            private const string EDGE_FORMAT = @"{0}edge [ source {1} target {2} {3}graphics [ {4}]]";
            private const string LABEL_FORMAT = @"label ""{0}"" ";

            public Node Source { get; set; }
            public Node Target { get; set; }
            public string Label { get; set; }
            public string Graphics { get; set; }

            public override string ToString()
            {
                var label = string.IsNullOrWhiteSpace(Label) ? string.Empty : string.Format(LABEL_FORMAT, Label);
                return string.Format(EDGE_FORMAT, Environment.NewLine, Source.Id, Target.Id, label, Graphics);
            }
        }
    }
}