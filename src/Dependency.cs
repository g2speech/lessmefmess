namespace LessMefMess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Dependency : IComparable<Dependency>, IEquatable<Dependency>
    {
        private static readonly Regex _regex = new Regex(@"'(?<port>[^']+?)' (?<direction>from|on) part '(?<part>[^']+)'\.\s+Element: (?<element>.+?)$", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex _portRegex = new Regex(@"(?<site>[^ ]+) \((?:(?:,\s*)?(?<name>\w+)=""(?<value>[^""]*)"")+\)", RegexOptions.Compiled);

        private readonly RootCause _rootCause;
        private readonly IDictionary<string, string> _properties;
        private readonly string _site;
        private readonly string _part;
        private readonly Direction _direction;

        private Dependency(RootCause rootCause, string member, string partValue, Dictionary<string, string> properties, Direction directionEnum)
        {
            _site = member;
            _part = partValue;
            _properties = properties;
            _rootCause = rootCause;
            _direction = directionEnum;
        }

        public Direction Direction
        {
            get { return _direction; }
        }

        public string ContractName
        {
            get
            {
                string contractName;
                _properties.TryGetValue("ContractName", out contractName);
                return contractName;
            }
        }

        private string ParameterName
        {
            get
            {
                string parameterName;
                _properties.TryGetValue("Parameter", out parameterName);
                return parameterName;
            }
        }

        public string Name
        {
            get { return string.IsNullOrEmpty(ParameterName) ? _site : string.Format("{0}({1})", _site, ParameterName); }
        }

        public string Part
        {
            get { return _part; }
        }

        public static IEnumerable<Dependency> Dependencies(RootCause rootCause)
        {
            return _regex
                .Matches(rootCause.String)
                .Cast<Match>()
                .Select(match => Create(rootCause, match));
        }

        private static Dependency Create(RootCause rootCause, Match match)
        {
            var partValue = match.Groups["part"].Value;
            var portValue = match.Groups["port"].Value;
            var directionString = match.Groups["direction"].Value;

            var portMatch = _portRegex.Match(portValue);
            var siteValue = portMatch.Groups["site"].Value;
            Debug.Assert(siteValue.StartsWith(partValue));
            var member = siteValue.Substring(partValue.Length);
            var names = portMatch.Groups["name"].Captures.Cast<Capture>();
            var values = portMatch.Groups["value"].Captures.Cast<Capture>();

            var properties = names
                .Zip(values, (n, v) => new { n, v })
                .ToDictionary(p => p.n.Value, p => p.v.Value);

            var directionEnum = DirectionExtension.Create(directionString);

            return new Dependency(rootCause, member, partValue, properties, directionEnum);
        }

        public int CompareTo(Dependency other)
        {
            var compareDirection = Direction.CompareTo(other.Direction);
            if (compareDirection != 0)
                return compareDirection;

            var compareContract = string.Compare(ContractName, other.ContractName, StringComparison.Ordinal);
            if (compareContract != 0)
                return compareContract;

            var comparePart = string.Compare(Part, other.Part, StringComparison.Ordinal);
            if (comparePart != 0)
                return comparePart;

            var compareName = string.Compare(Name, other.Name, StringComparison.Ordinal);
            return compareName;
        }

        public bool Equals(Dependency other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Dependency;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            return Direction.GetHashCode() ^ ContractName.GetHashCode() ^ Part.GetHashCode() ^ Name.GetHashCode();
        }
    }
}