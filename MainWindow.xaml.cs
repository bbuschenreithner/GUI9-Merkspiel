using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GUI9_Merkspiel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ButtonHandler> buttonHandlers;
        private List<int> buttonReihenfolge, buttonReihenfolgeClicked;
        private int buttonBlinkedCounterTemp;
        private DispatcherTimer mainTimer;
        private int blinkTimeMilliS;
        private Random random;
        private bool successdLevel;
        public MainWindow()
        {
            InitializeComponent();
            blinkTimeMilliS = 500;
            buttonHandlers = new List<ButtonHandler>();
            buttonHandlers.Add(new ButtonHandler(btn1, Brushes.Green, Brushes.LightGreen, blinkTimeMilliS));
            buttonHandlers.Add(new ButtonHandler(btn2, Brushes.Red, Brushes.Orange, blinkTimeMilliS));
            buttonHandlers.Add(new ButtonHandler(btn3, Brushes.Blue, Brushes.LightBlue, blinkTimeMilliS));
            buttonHandlers.Add(new ButtonHandler(btn4, Brushes.Yellow, Brushes.LightYellow, blinkTimeMilliS));
            random = new Random();
            buttonReihenfolge = new List<int>();
            buttonReihenfolgeClicked = new List<int>();
            buttonBlinkedCounterTemp = 0;
            mainTimer = new DispatcherTimer();
            mainTimer.Interval = TimeSpan.FromMilliseconds(blinkTimeMilliS * 2 + 1);
            mainTimer.Tick += MainTimer_Tick;
            successdLevel = true;
        }

        private void MainTimer_Tick(object? sender, EventArgs e)
        {
            if (buttonBlinkedCounterTemp < buttonReihenfolge.Count)
            {
                buttonHandlers[buttonReihenfolge[buttonBlinkedCounterTemp]].blinkStart();
                buttonBlinkedCounterTemp++;
            }
            else
            {
                mainTimer.Stop();
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (successdLevel)
            {
                int newBtnId = random.Next(0, 4);
                buttonReihenfolge.Add(newBtnId);
                buttonBlinkedCounterTemp = 0;
                mainTimer.Start();
                tblOutput.Text = "Drücke die blinkenden Buttons nach!";
                successdLevel = false;
                tblLevelCount.Text = buttonReihenfolge.Count.ToString();
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int btnID = Convert.ToInt32(btn.Name[3]) - 48 - 1;
            buttonReihenfolgeClicked.Add(btnID);
            if (buttonReihenfolgeClicked.Count > buttonReihenfolge.Count)
            {
                buttonReihenfolgeClicked.RemoveAt(0);
            }
            //tblDebug.Text += btnID.ToString();
            if (buttonReihenfolge.SequenceEqual(buttonReihenfolgeClicked))
            {
                tblOutput.Text = "Super!";
                successdLevel = true;
                buttonReihenfolgeClicked.Clear();
            }
        }
    }
    public class ButtonHandler
    {
        private Button btn;
        private bool state;
        private DispatcherTimer timer;
        private Brush bgNormal, bgBlink;
        private int blinkCounter;

        public ButtonHandler(Button btn, Brush bgNormal, Brush bgBlink, int blinkTime)
        {
            this.btn = btn;
            this.state = false;
            this.bgNormal = bgNormal;
            this.bgBlink = bgBlink;
            this.blinkCounter = 0;
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(blinkTime);
            this.timer.Tick += blink;
            this.btn.Background = bgNormal;
        }
        public void blink(object? sender, EventArgs e)
        {
            if (++blinkCounter > 2)
            {
                timer.Stop();
                blinkCounter = 0;
                return;
            }
            if (blinkCounter % 2 == 1)
            {
                btn.Background = bgBlink;
            }
            else
            {
                btn.Background = bgNormal;
            }
        }
        public void blinkStart()
        {
            timer.Start();
        }
    }
}
