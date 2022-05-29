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
    /// Interaction logic for TestMemo.xaml
    /// </summary>
    public partial class TestMemo : Window
    {
        //declaring the user id, user role, test id and student id
        string userID, userRole, testID, studentID;
        //declaring the user display variable to be used in this window
        int userDisplay;
        //calling the student class to use functions that will be needed for a student
        Student memoStudent = new Student();

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //takes the user back to the user menu
            UserMenu newWindow = new UserMenu(userID, userRole);
            newWindow.DataContext = this;
            this.Hide();
            newWindow.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //closes the application if the user closes it
            Application.Current.Shutdown();
        }

        public TestMemo(string testID, string userID, string userRole)
        {
            InitializeComponent();
            //setting the user id, user role and test id in this window 
            this.userID = userID;
            this.userRole = userRole;
            this.testID = testID;
        }

        public TestMemo(string studentID, string userID, string userRole, int userDisplay)
        {
            InitializeComponent();
            //setting the user id, user role, test id and user display in this window 
            //user display is to determine if a lecture is looking at a student marks
            this.userID = userID;
            this.userRole = userRole;
            this.studentID = studentID;
            this.userDisplay = userDisplay;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to see if the user display is equel to 1
            if (userDisplay == 1)
            {
                //foreach that will loop to get all the student deatals on a test they have taken
                foreach (var sdtr in memoStudent.StudentResults(studentID))
                {
                    //adding the result to the rich text box
                    rtxbUserMemoResults.AppendText(sdtr + "\n------------------------------------------------------------\n");
                }
            }
            else
            {
                //getting the memo for the student so they can view where they went wrong
                foreach (var x in memoStudent.Memo(testID, userID))
                {
                    //adding the result to the rich text box
                    rtxbUserMemoResults.AppendText(x);
                }
            }

        }
    }
}
