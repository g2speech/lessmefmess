namespace LessMefMess
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class YedSerializer
    {
        private List<string> _contracts;
        private List<string> _parts;

        private List<YedGraph.Node> _contractNodes;
        private List<YedGraph.Node> _partNodes;
        private readonly YedGraph _yedGraph;

        private YedSerializer()
        {
            _yedGraph = new YedGraph();
        }

        public static void Save(IEnumerable<string> contracts, IEnumerable<string> parts, IEnumerable<Dependency> exports, IEnumerable<Dependency> imports)
        {
            var serializer = new YedSerializer();
            serializer.SetContracts(contracts);
            serializer.SetParts(parts);
            serializer.SetExports(exports);
            serializer.SetImports(imports);
            serializer.Save();
        }

        private void SetContracts(IEnumerable<string> contracts)
        {
            _contracts = contracts.ToList();
            _contractNodes = _contracts
                .Select((c, i) => new YedGraph.Node
                                      {
                                          Id = i,
                                          Label = c,
                                          Graphics = @"fill ""#CCFFFF""",
                                      })
                .ToList();
            _contractNodes.ForEach(_yedGraph.Add);
        }

        private void SetParts(IEnumerable<string> parts)
        {
            _parts = parts.ToList();
            var contractCount = _contractNodes.Count;
            _partNodes = _parts
                .Select((p, i) => new YedGraph.Node
                                      {
                                          Id = contractCount + i,
                                          Label = p,
                                          Graphics = @"fill ""#FFCCCC""",
                                      })
                .ToList();
            _partNodes.ForEach(_yedGraph.Add);
        }

        private void SetExports(IEnumerable<Dependency> exports)
        {
            var exportEdges = exports
                .Select(d => new YedGraph.Edge
                                 {
                                     Source = GetPart(d),
                                     Target = GetContract(d),
                                     Label = d.Name,
                                     Graphics = @"style ""dashed"" fill ""#008000"" targetArrow ""plain""",
                                 })
                .ToList();
            exportEdges.ForEach(_yedGraph.Add);
        }

        private void SetImports(IEnumerable<Dependency> imports)
        {
            var importEdges = imports
                .Select(d => new YedGraph.Edge
                                 {
                                     Source = GetContract(d),
                                     Target = GetPart(d),
                                     Label = d.Name,
                                     Graphics = @"fill ""#c00000"" sourceArrow ""white_delta""",
                                 })
                .ToList();
            importEdges.ForEach(_yedGraph.Add);
        }

        private void Save()
        {
            const string OUTPUT = @"C:\ProgramData\LessMefMess_output.gml";
            File.WriteAllText(OUTPUT, _yedGraph.ToString(), Encoding.UTF8);
            Process.Start(@"C:\Program Files (x86)\yEd\yEd.exe", OUTPUT);
        }

        private YedGraph.Node GetContract(Dependency d)
        {
            return _contractNodes[_contracts.IndexOf(d.ContractName)];
        }

        private YedGraph.Node GetPart(Dependency d)
        {
            return _partNodes[_parts.IndexOf(d.Part)];
        }
    }
}