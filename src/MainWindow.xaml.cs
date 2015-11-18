namespace LessMefMess
{
    using System.Linq;
    using System.Windows;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnGenerate(object sender, RoutedEventArgs e)
        {
            var rootCauses = RootCause.GetRootCauses(ExceptionTrace.Text)
                .ToList();
            var dependencies = rootCauses
                .SelectMany(r => r.Dependencies)
                .Distinct()
                .ToList();

            var contracts = dependencies
                .Select(d => d.ContractName)
                .Distinct();
            var parts = dependencies
                .Select(d => d.Part)
                .Distinct();
            var exports = dependencies
                .Where(d => d.Direction == Direction.Ex);
            var imports = dependencies
                .Where(d => d.Direction == Direction.Im);

            YedSerializer.Save(contracts, parts, exports, imports);
        }
    }
}
