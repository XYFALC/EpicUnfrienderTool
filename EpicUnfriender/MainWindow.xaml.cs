using System.Windows;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace EpicUnfriender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    

    public partial class MainWindow : Window
    {

        private MainController controller;


        public MainWindow()
        {
            InitializeComponent();
            controller = new MainController();
        }

       
    public void Button_Click(object sender, RoutedEventArgs e)
            {
            controller.TakeScreenshot();
            System.Drawing.Point friendsButton = ImageDetection.GetImageCoordinates("Friendsbutton.jpg");
            MouseController.SetCursorPos(friendsButton.X, friendsButton.Y);
            MouseController.LeftClick();


            controller.TakeScreenshot();
            System.Drawing.Point friendsText = ImageDetection.GetImageCoordinates("Addfriendicon.jpg");
            MouseController.SetCursorPos(friendsText.X, friendsText.Y+310);

            //System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);  -- doet niks?

        }
    }
}