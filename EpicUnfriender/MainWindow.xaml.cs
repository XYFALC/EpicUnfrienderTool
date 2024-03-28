using System.Windows;
using System.Windows.Input;
using NHotkey.Wpf;
using NHotkey;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;

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
            // Create hotkeys
            HotkeyManager.Current.AddOrReplace("QPressed", Key.Q, ModifierKeys.Control, StopUnfriending);

            // Initiation buttons
            Start.IsEnabled = true;
            Stop.IsEnabled = false;

            Log("--------------------------------------------------------");
            Log("1. Start Epic-Games Launcher.");
            Log("2. Make sure you're logged-in.");
            Log("3. Make sure the launcher is fully visible on-screen.");
            Log("4. Stay idle in home screen and press start.");
            Log("--------------------------------------------------------");
        }

        public void Log(string Message)
        {
            Dispatcher.Invoke(() =>
            {
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                string logMessage = $"{currentTime} - ";

                // Set the time part (current time) to white color
                LogBox.AppendText(logMessage);
                LogBox.Select(LogBox.Text.Length - logMessage.Length, logMessage.Length);
                LogBox.Foreground = System.Windows.Media.Brushes.White;

                // Append the message part (green color)
                LogBox.AppendText(Message + "\n");
                LogBox.Select(LogBox.Text.Length - Message.Length - 1, Message.Length);
                LogBox.Foreground = System.Windows.Media.Brushes.LimeGreen;
            });
        }

        bool botrunning;

        public void StopUnfriending(object? sender, HotkeyEventArgs e)
        {
            if (botrunning == true){
                botrunning = false;
                Log("Bot stopped");
                controller.stopWorker();
                Start.IsEnabled = true;
                Stop.IsEnabled = false;
            }        
        }
        
        private void LogBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Scroll to the end whenever new content is added
            LogBox.ScrollToEnd();
        }

        // Start unfriending
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread workerThread = new Thread(() => controller.UnfriendSequence());
            workerThread.Start();
            Log("Bot started - Press (CTRL+Q) to stop");
            botrunning = true;
            Start.IsEnabled = false;
            Stop.IsEnabled = true;

            //Thread printThread = new Thread(() => print());
            //printThread.Start()
        }


        //public void print()
        //{
        //    for (int i = 0; i < 20; i++)
        //    {
        //        Thread.Sleep(500);
        //        Log(i+"");

        //    }
        //}
    }
} 