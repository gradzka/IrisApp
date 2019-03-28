// Based on https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/master/MainDemo.Wpf/Domain/SelectableViewModel.cs

namespace IrisApp.Models.Home
{
    public class LogModel
    {
        public char Code { get; set; }

        public string Description { get; set; }

        public bool IsSelected { get; set; }

        public string Name { get; set; }
    }
}
