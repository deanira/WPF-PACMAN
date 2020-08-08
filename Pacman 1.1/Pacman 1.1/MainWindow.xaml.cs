using Pacman_1._1;
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
using XamlAnimatedGif;
using System.IO;

namespace Pacman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        Boolean goup, godown, goleft, goright, enter, isGameOver;
        int score, redGhostSpeed, yellowGhostSpeed, pinkGhostX, pinkGhostY, coins, i;
        string myfile = @"Level1Score.txt";
        double playerSpeed;
        Rect pacmanHitBox;
        Random rand = new Random();
        //ImageBrush pacmanRight = new ImageBrush();

        private void btnNextLevel_Click(object sender, RoutedEventArgs e)
        {
            Level2 hal1 = new Level2();
            hal1.Show();
            this.Close();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow hal1 = new MainWindow();
            hal1.Show();
            this.Close();
        }

        public MainWindow()
        {
            InitializeComponent();

            MyCanvas.Focus();
            
            gameTimer.Tick += Timer_Tick;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            //pacmanRight.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/right.gif"));
            //pacman.Fill = pacmanRight;
            resetGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            txtScore.Text = "Score : " + score;

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            if (goleft && Canvas.GetLeft(pacman) > 0)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - playerSpeed);
                pacman.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/left.gif"));
            }
            if (goright && Canvas.GetLeft(pacman) + pacman.Width < Application.Current.MainWindow.Width - 15)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + playerSpeed);
                pacman.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/right.gif"));
            }
            if (goup && Canvas.GetTop(pacman) > 0)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - playerSpeed);
                // if go up is true and player is within the boundary from the top 
                // then we can use the set top to move the rec1 towards top of the screen
                pacman.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Up.gif"));
            }
            if (godown && Canvas.GetTop(pacman) + (pacman.Height) < Application.Current.MainWindow.Height - 40)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + playerSpeed);
                // if go down is true and player is within the boundary from the bottom of the screen
                // then we can set top of rec1 to move down
                pacman.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/down.gif"));
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "obs")
                {
                    Rect obsHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (pacmanHitBox.IntersectsWith(obsHitBox))
                    {
                        gameOver("You hit the wall");
                    }
                }
            }

            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "ghost")
                {
                    Rect ghostHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (pacmanHitBox.IntersectsWith(ghostHitBox))
                    {
                        gameOver("You hit the ghost");
                    }
                }
            }

            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "coin")
                {
                    ++coins;
                    Rect coinHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (pacmanHitBox.IntersectsWith(coinHitBox))
                    {
                        Canvas.SetLeft(x, -100);
                        ++score;
                    }
                }
            }

            if (score == 44)
            {
                endGame();
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up)
            {
                goup = true;
            }
            if(e.Key == Key.Down)
            {
                godown = true;
            }
            if (e.Key == Key.Left)
            {
                goleft = true;
            }
            if (e.Key == Key.Right)
            {
                goright = true;
            }
            if (e.Key == Key.Enter)
            {
                enter = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                goup = false;
            }
            if (e.Key == Key.Down)
            {
                godown = false;
            }
            if (e.Key == Key.Left)
            {
                goleft = false;
            }
            if (e.Key == Key.Right)
            {
                goright = false;
            }
            if (e.Key == Key.Enter)
            {
                enter = false;
            }
        }

        private void resetGame()
        {
            txtScore.Text = "Score : 0";
            score = 0;

            redGhostSpeed = 5;
            //yellowGhostSpeed = 5;
            //pinkGhostX = 5;
            //pinkGhostY = 5;
            playerSpeed = 8.5;

            isGameOver = false;
            Canvas.SetLeft(pacman, 53);
            Canvas.SetTop(pacman, 58);
            //pacman.Margin = "53,58,0,0";

            Canvas.SetLeft(redGhost, 301);
            Canvas.SetTop(redGhost, 117);
            //redGhost.Margin = "301,117,0,0";
            Canvas.SetLeft(yellowGhost, 301);
            Canvas.SetTop(yellowGhost, 499);
            //yellowGhost.Margin = "301,499,0,0";
            Canvas.SetLeft(pinkGhost, 489);
            Canvas.SetTop(pinkGhost, 280);
            //pinkGhost.Margin = "489,280,0,0";

            gameTimer.Start();
        }

        private void endGame()
        {
            gameTimer.Stop();
            isGameOver = true;
            txtGameOver.Text = "Y E A Y ! ! ! !";
            txtWhyGameOver.Text = "go to the next level";
            btnNextLevel.Visibility = Visibility.Visible;
            //MessageBox.Show(message + "\nGame Over\nPress Enter to Restart the game");
        }

        private void gameOver(string message)
        {
            gameTimer.Stop();
            isGameOver = true;
            txtGameOver.Text = "Game Over";
            txtWhyGameOver.Text = message;
            btnRestart.Visibility = Visibility.Visible;
            //MessageBox.Show(message + "\nGame Over\nPress Enter to Restart the game");
            String time = DateTime.Now.ToString("dddd , MMM dd yyyy, hh:mm:ss tt");
            if (!File.Exists(myfile))
            {
                using (StreamWriter sw = File.CreateText(myfile))
                {
                    sw.WriteLine(time + " : " + score);
                    sw.Close();
                }
            }
            using (StreamWriter sw = File.AppendText(myfile))
            {
                sw.WriteLine(time + " : " + score);
                sw.Close();
            }
        }
    }
}
