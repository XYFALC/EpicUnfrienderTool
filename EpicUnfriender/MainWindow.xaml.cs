using System.Windows;
using System.Windows.Input;
using NHotkey.Wpf;
using NHotkey;

namespace EpicUnfriender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        // Fields
        readonly private MainController controller;

        public MainWindow()
        {
            // Init
            InitializeComponent();

            // Refer Maincontroller class
            controller = new MainController();
            LogBox.AppendText("New LogMessage " + "\n");
            // Create hotkeys
            HotkeyManager.Current.AddOrReplace("QPressed", Key.Q, ModifierKeys.Control | ModifierKeys.Alt, StopUnfriending);

            // Initiation buttons
            Start.IsEnabled = true;
            Stop.IsEnabled = false;
            
        }


        public void StopUnfriending(object? sender, HotkeyEventArgs e)
        {
            controller.stopWorker();
            Start.IsEnabled = true;
            Stop.IsEnabled = false;
        }

        // Start unfriending
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread workerThread = new Thread(() => controller.UnfriendSequence());
            workerThread.Start();
            Start.IsEnabled = false;
            Stop.IsEnabled = true;
        }
    }
} 