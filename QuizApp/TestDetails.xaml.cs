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
using System.Windows.Shapes;

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for TestDetails.xaml
    /// </summary>
    public partial class TestDetails : Window
    {
        //Declareing user Id and user role to be used in the window
        string userID, userRole, testID;
        //calling the lecture class to use functions the lecure needs for this window
        Lecture testDe = new Lecture();
        //declaring a module id variable
        int mID;
        //boolean value that is equel to false
        Boolean error = false;
        
        public TestDetails(string userID, string userRole)
        {
            InitializeComponent();
            //setting the user id and user role to the variables in this class
            this.userID = userID;
            this.userRole = userRole;
        }

        public TestDetails(string testID, string userID, string userRole)
        {
            InitializeComponent();
            //setting the user id and user role to the variables in this class
            this.userID = userID;
            this.userRole = userRole;
            this.testID = testID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //foreach loop that will loop throuh all the lecture modules to display to the lecture
                foreach (var m in testDe.LectureModules(userID))
                {
                    cbxModuleID.Items.Add(m);
                }

                //checking if the test id variable is null to see if the user requested to edit a test or create a test
                if (testID != null)
                {
                    //splliting the details of the test
                    string[] testD = testDe.TestDetails(testID).Split('&');

                    //getting the module for the tets that the user wants to edit
                    //----------------------------------------------------------
                    int i = 0;
                    foreach (var testModuleId in cbxModuleID.Items)
                    {
                        string[] modulesOfCurrentUser = testModuleId.ToString().Split(' ');
                        if (modulesOfCurrentUser[0] == testD[2])
                        {
                            cbxModuleID.SelectedIndex = i;
                        }
                        i++;
                    }
                    //----------------------------------------------------------

                    //setting th test title textbox to the value in the database
                    txbtestTitle.Text = testD[0];

                    //setting the discription textbox to the value in the database
                    rtxbTestDesc.AppendText(testD[1]);
                }
            }
            catch (Exception)
            {
                //displays to the user there is an error to connect to the database
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            //if statement that will check fi there are any errors made by the users
            if (Error() == false)
            {
                //getting all the content in the test description
                string testDescr = new TextRange(rtxbTestDesc.Document.ContentStart, rtxbTestDesc.Document.ContentEnd).Text;

                //splitting the test description so that the /r and /n does not get saved
                string[] rtxbText = testDescr.Split('\r');

                //getting the module Id that the user chosed
                string[] m = cbxModuleID.SelectedItem.ToString().Split(' ');

                //setting the module id to the variable module id
                mID = Convert.ToInt32(m[0]);

                //description of test
                string testDesc = rtxbText[0];

                string newTestDesc = testDesc.Replace(',', ';');


                if (testID != null)
                {
                    //sending the user to the create a test window where they will the questions for the test
                    CreateATest newWindow = new CreateATest(userRole, userID, mID, txbtestTitle.Text.Replace('&', ' '), newTestDesc.Replace('&', ' '), testID);
                    newWindow.DataContext = this;
                    this.Hide();
                    newWindow.ShowDialog();
                }
                else
                {
                    //sending the user to the create a test window where they will the questions for the test
                    CreateATest newWindow = new CreateATest(userRole, userID, mID, txbtestTitle.Text.Replace('&',' '), newTestDesc.Replace('&', ' '));
                    newWindow.DataContext = this;
                    this.Hide();
                    newWindow.ShowDialog();
                }
            }
            else
            {
                //turns the error value to false
                error = false;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //closes the program if the user wishes to do so
            Application.Current.Shutdown();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //takes the user back to to the user menu
            UserMenu newWindow = new UserMenu(userID, userRole);
            newWindow.DataContext = this;
            this.Hide();
            newWindow.ShowDialog();
        }

        public Boolean Error()
        {
            //if to see if there is no item selected in the combo box
            if (cbxModuleID.SelectedIndex == -1)
            {
                //error variable to true
                error = true;
                //message to display to the user that there is no module chosen
                MessageBox.Show("Please select a module for the test.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //if to see if there is no test title
            if (txbtestTitle.Text == "")
            {
                //error variable to true
                error = true;
                //message to display to the user that there is no title
                MessageBox.Show("Please enter a title for the test.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //getting the content in the test description
            string testDesc = new TextRange(rtxbTestDesc.Document.ContentStart, rtxbTestDesc.Document.ContentEnd).Text;
            //splitting the test description so that the /r and /n does not get saved
            string[] rtxbText = testDesc.Split('\r');
            //checking if there is no test descriprion enterd
            if (rtxbText[0] == "")
            {
                //error variable to true
                error = true;
                //message to display to the user that there is no test description
                MessageBox.Show("Please enter a description for the test.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //return the boolean value
            return error;
        }
    }
}
