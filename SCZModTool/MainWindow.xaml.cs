using System;
using System.Collections.Generic;
using System.Windows;

namespace SCZModTool
{
    public partial class MainWindow : Window
    {
        private MitmInterceptor interceptor;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string baseUrl = baseUrlTextBox.Text;
            var numberPositions = new List<int> { 21, 23 }; // Indexe der {x}-Platzhalter
            var languages = new List<string> { "de_DE", "en_EN" }; // Beispielsprachen
            interceptor = new MitmInterceptor(baseUrl, numberPositions, languages);
            interceptor.Start();
        }
    }
}
