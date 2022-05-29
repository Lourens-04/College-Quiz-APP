using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
using Verification;

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //calling a dll method to verify a user log in
        VerifyLogIn logIn = new VerifyLogIn();
        //calling the database model to use it
        SchoolApplicationAppEntities db = new SchoolApplicationAppEntities();
        //boolean that will check for user errors
        Boolean error = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        //button to let the user log in
        //---------------------------------------------------------------
        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if staement to check if the text boxes are empty
                if (txbUsername.Text != "" && txbPassword.Password != "")
                {
                    //creating a list to store all the users in
                    List<string> users = new List<string>();
                    //linQ statement to get user ID, user password, userRole
                    var userRole = from u in db.Users
                                   select new { u.User_ID, u.Password, u.UserRole };

                    //foreach that will loop through the linQ and adding it to the list
                    //------------------------------------------------------------
                    foreach (var check in userRole)
                    {
                        users.Add(check.User_ID + "," + check.Password + "," + check.UserRole);
                    }
                    //------------------------------------------------------------

                    //if staement that will check if the user exists and then if the user role equels student
                    //------------------------------------------------------------------------
                    if (logIn.CheckUser(txbUsername.Text, txbPassword.Password.ToString(), users) == "student")
                    {
                        UserMenu newWindow = new UserMenu(txbUsername.Text, "student");
                        newWindow.DataContext = this;
                        this.Hide();
                        newWindow.ShowDialog();
                    }
                    //------------------------------------------------------------------------

                    //if staement that will check if the user exists and then if the user role equels lecture
                    //------------------------------------------------------------------------
                    if (logIn.CheckUser(txbUsername.Text, txbPassword.Password.ToString(), users) == "lecture")
                    {
                        UserMenu newWindow = new UserMenu(txbUsername.Text, "lecture");
                        newWindow.DataContext = this;
                        this.Hide();
                        newWindow.ShowDialog();
                    }
                    //------------------------------------------------------------------------

                    //if statement that will check if the user is not there and if user role is null
                    if (logIn.CheckUser(txbUsername.Text, txbPassword.Password.ToString(), users) == null)
                    {
                        //turn error to true 
                        error = true;
                    }

                }
                else
                {
                    //message box to tell the user the password or username is incorrect
                    MessageBox.Show("Username or Password is incorrect",
                            "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                //if staement to check if the error boolean is true
                if (error == true)
                {
                    //meesage box that will display that the username and password is inncorrect
                    MessageBox.Show("Username or Password is incorrect",
                            "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //turns error boolean to false
                    error = false;
                }
            }
            catch (Exception)
            {
                //displays if the connection to the database is dropped
                MessageBox.Show("The connection to the database is lost, please insure you are connected to the internet",
                            "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //---------------------------------------------------------------

        //button that will take the user to the sign up page
        //-----------------------------------------------------------
        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp newWindow = new SignUp();
            newWindow.DataContext = this;
            this.Hide();
            newWindow.ShowDialog();
        }
        //-----------------------------------------------------------

        //Method that will colose the program when the user exit its
        //-----------------------------------------------------------
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        //-----------------------------------------------------------
    }
}
