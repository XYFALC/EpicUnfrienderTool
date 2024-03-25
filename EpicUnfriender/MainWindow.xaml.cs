using System.Windows;
using Emgu.CV;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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
            while (true)
            {
                // Open friends menu if it's not already opened
                if (ImageDetection.CheckImageCoordinates("Add_Friend_Icon.jpg") < 0.95)
                {
                    //Click friends menu
                    System.Drawing.Point friendsButton = ImageDetection.GetImageCoordinates("Friends_Button.jpg");
                    MouseController.SetCursorPos(friendsButton.X, friendsButton.Y);
                    MouseController.LeftClick();
                }

                //Select friend in list
                System.Drawing.Point friendsText = ImageDetection.GetImageCoordinates("Add_Friend_Icon.jpg");
                MouseController.SetCursorPos(friendsText.X, friendsText.Y + 310);
                MouseController.LeftClick();

                //Wait for 'MORE OPTIONS' menu
                while (ImageDetection.CheckImageCoordinates("More_Options.jpg") < 0.95)
                {
                    Thread.Sleep(300);
                }
                System.Drawing.Point MoreOptions = ImageDetection.GetImageCoordinates("More_Options.jpg");
                MouseController.SetCursorPos(MoreOptions.X, MoreOptions.Y);
                MouseController.LeftClick();

                //Wait for 'REMOVE FRIEND' first prompts
                while (ImageDetection.CheckImageCoordinates("Remove_Friend.jpg") < 0.95)
                {
                    Thread.Sleep(300);
                }
                System.Drawing.Point removeFriend = ImageDetection.GetImageCoordinates("Remove_Friend.jpg");
                MouseController.SetCursorPos(removeFriend.X, removeFriend.Y);
                MouseController.LeftClick();

                // Close all tabs until none left            
                while (ImageDetection.CheckImageCoordinates("Close_Tab.jpg") > 0.95)
                {
                    System.Drawing.Point Close_Tab2 = ImageDetection.GetImageCoordinates("Close_Tab.jpg");
                    MouseController.SetCursorPos(Close_Tab2.X, Close_Tab2.Y);
                    MouseController.LeftClick();
                    Thread.Sleep(100);
                }
            }
        }
    }
}