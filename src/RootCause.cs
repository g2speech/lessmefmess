namespace LessMefMess
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class RootCause
    {
        private static readonly Regex _regex = new Regex(@"^\d+\)", RegexOptions.Compiled | RegexOptions.Multiline);

        private RootCause()
        {
        }

        public int Number { get; private set; }
        public string String { get; private set; }

        public IEnumerable<Dependency> Dependencies
        {
            get { return Dependency.Dependencies(this); }
        }

        public static IEnumerable<RootCause> GetRootCauses(string input)
        {
            return _regex
                .Split(input)
                .Skip(1)
                .Select(Create);
        }

        private static RootCause Create(string s, int i)
        {
            return new RootCause { Number = i, String = s };
        }
    }
}