using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for UserMenu.xaml
    /// </summary>
    public partial class UserMenu : Window
    {
        //calling the student class to be used in this class
        Student student = new Student();
        //calling the lecture class to be used in this class
        Lecture lecture = new Lecture();
        //declaring the user id and user role variables
        string userID, userRole;
        //calling the search class to be used in this class
        Search search = new Search();

        List<string> listTests = new List<string>();
        List<string> listResults = new List<string>();

        public UserMenu(string userID, string userRole)
        {
            InitializeComponent();
            // setting the user id and user role 
            this.userID = userID;
            this.userRole = userRole;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //displaying the user signed into the application
                lbLogInDetails.Content = "Loged In As : " + userID;

                //if to see if a student is signed in
                if (userRole.Equals("student"))
                {
                    btnPublishStatus.Visibility = Visibility.Hidden;
                    btnEdit.Visibility = Visibility.Hidden;

                    foreach (var item in student.StudentTests(userID))
                    {
                        listTests.Add(item);
                    }

                    foreach (var item in student.StudentResults(userID))
                    {
                        listResults.Add(item);
                    }

                    lbTest.ItemsSource = listTests;
                    lbResults.ItemsSource = listResults;
                }
                //if to see if a lecture signed in
                if (userRole.Equals("lecture"))
                {
                    btnPublishStatus.Visibility = Visibility.Hidden;

                    //changes the tab headings for a lecture
                    tiTabItem1.Header = "Test Created";
                    tiTabItem2.Header = "View Students";
                    //changing the buttons for lectures
                    btnTakeTest.Content = "Create Test";
                    btnViewMemo.Content = "View Student";

                    foreach (var item in lecture.TestCreated(userID))
                    {
                        listTests.Add(item);
                    }

                    foreach (var item in lecture.ViewStudents(lecture.LectureModules(userID)))
                    {
                        listResults.Add(item);
                    }

                    lbTest.ItemsSource = listTests;
                    lbResults.ItemsSource = listResults;
                }
            }
            catch (Exception)
            {
                //displaying to the user an error to the database
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }
        //button to take a test for student/ and create a test for a lecture
        private void BtnTakeTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if to see if the 1st tab item is selected
                if (tiTabItem1.IsSelected)
                {
                    //if to see if the conetent of the button is equel to take a test
                    if (btnTakeTest.Content.Equals("Take Test"))
                    {
                        //if to check if something in the list box is not selected
                        if (lbTest.SelectedIndex != -1)
                        {
                            //splits the content of the chosen item to get the test id
                            string[] userSelect = lbTest.SelectedItem.ToString().Split(' ');
                            //store the test idin user select variable (uS)
                            string uS = userSelect[0];
                            //takes the user to the test window where they can take the selected test
                            Test newWindow = new Test(uS, userRole, userID);
                            newWindow.DataContext = this;
                            this.Hide();
                            newWindow.ShowDialog();
                        }
                        else
                        {
                            //message box to display a user needs to select a test to be taken
                            MessageBox.Show("Please select a test to take",
                                "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    //if to see the button content equel to create a test
                    if (btnTakeTest.Content.Equals("Create Test"))
                    {
                        //takes the user to the test details window
                        TestDetails newWindow = new TestDetails(userID, userRole);
                        newWindow.DataContext = this;
                        this.Hide();
                        newWindow.ShowDialog();
                    }
                }
                else
                {
                    //if to see if the button equel take test
                    if (btnTakeTest.Content.Equals("Take Test"))
                    {
                        //displaying to the user they have select a test
                        MessageBox.Show("Please select a test to take",
                            "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception)
            {
                //displaying to the user an error to the database
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        //button sign out that will sign a user out of the application
        private void BtnSignOut_Click(object sender, RoutedEventArgs e)
        {
            //takes the user back to the log in page
            MainWindow newWindow = new MainWindow();
            newWindow.DataContext = this;
            this.Hide();
            newWindow.ShowDialog();
        }
        //method to close the program
        private void Window_Closed(object sender, EventArgs e)
        {
            //closes the program if the user wishes to do so
            Application.Current.Shutdown();
        }

        private void LbTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if to see if the selected index is garater or equel to minus 1
            if (lbTest.SelectedIndex > -1)
            {
                //changes the selected index of results list box to minus 1
                lbResults.SelectedIndex = -1;
            }

            if (userRole.Equals("lecture"))
            {
                if (lbTest.SelectedIndex != -1)
                {
                    string status = getBetween(lbTest.SelectedItem.ToString(), "      ", "\n");

                    if (status.Equals("publish"))
                    {
                        btnPublishStatus.Visibility = Visibility.Visible;
                        btnPublishStatus.Content = "UnPublish";
                    }
                    else
                    {
                        btnPublishStatus.Visibility = Visibility.Visible;
                        btnPublishStatus.Content = "Publish";
                    }
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if to see if the user role equel to lecture
            if (userRole == "lecture")
            {
                //checks if the 2nd tab item is selected
                if (tiTabItem2.IsSelected)
                {
                    //clears all items in the combo box
                    cbxSearchByTest.Items.Clear();
                    //loads the combo box with the following items to display to a lecture
                    cbxSearchByTest.Items.Add("All Students");
                    cbxSearchByTest.Items.Add("Student ID");
                    cbxSearchByTest.Items.Add("Student First Name");
                    cbxSearchByTest.Items.Add("Student Last Name");
                }
            }
            //if to see if the user role equel to lecture
            if (userRole == "lecture")
            {
                //checks if the 1st tab item is selected
                if (tiTabItem1.IsSelected)
                {
                    //clears all items in the combo box
                    cbxSearchByTest.Items.Clear();
                    //loads the combo box with the following items to display to a lecture
                    cbxSearchByTest.Items.Add("All Tests");
                    cbxSearchByTest.Items.Add("Test Name");
                    cbxSearchByTest.Items.Add("Module");
                }
            }
            //if the user role equels to student
            if (userRole == "student")
            {
                //checks if the 2nd tab item is selected
                if (tiTabItem2.IsSelected)
                {
                    //clears all items in the combo box
                    cbxSearchByTest.Items.Clear();
                    //loads the combo box with the following items to display to a student
                    cbxSearchByTest.Items.Add("All Results");
                    cbxSearchByTest.Items.Add("Test Name");
                    cbxSearchByTest.Items.Add("Module");
                }
            }
        }

        private void BtnViewMemo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //checks if the 2nd tab item is selected
                if (tiTabItem2.IsSelected)
                {
                    //if to check if button equels to view memo
                    if (btnViewMemo.Content.Equals("View Memo"))
                    {
                        //if to see if the list box selected index does not equel to 1
                        if (lbResults.SelectedIndex != -1)
                        {
                            //splits the user selected item to get the test id
                            string[] userSelect = lbResults.SelectedItem.ToString().Split(' ');
                            //sets the test id to the user select variable
                            string uS = userSelect[0];
                            //takes the user to the test memo window
                            TestMemo newWindow = new TestMemo(uS, userID, userRole);
                            newWindow.DataContext = this;
                            this.Hide();
                            newWindow.ShowDialog();
                        }
                        else
                        {
                            //message box to tell the user to select a test to view the memo
                            MessageBox.Show("Please select a Test to view Memo",
                                "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        //checks if the result list box selected index is not equel to minus 1
                        if (lbResults.SelectedIndex != -1)
                        {
                            //splits the user results selected item to get student id
                            string[] userSelect = lbResults.SelectedItem.ToString().Split(' ');
                            //sets the test id to the user select variable (uS)
                            string uS = userSelect[3];
                            //takes the user to the test memo window but to disply student results
                            TestMemo newWindow = new TestMemo(uS, userID, userRole, 1);
                            newWindow.DataContext = this;
                            this.Hide();
                            newWindow.ShowDialog();
                        }
                        else
                        {
                            //message box to tell the user a student is not selected
                            MessageBox.Show("Please select a student to view their results",
                                "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                else
                {
                    //if to see if the button equels view memo
                    if (btnTakeTest.Content.Equals("View Memo"))
                    {
                        //tells the user to select a test to view memo
                        MessageBox.Show("Please select a test to view memo",
                            "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        //tells the user to select a student to view their results
                        MessageBox.Show("Please select a student to view test taken",
                            "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception)
            {
                //displaying to the user an error to the database
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if to see if the combo box is equel to all test or all results
                if ("All Tests" == cbxSearchByTest.Text || "All Results" == cbxSearchByTest.Text)
                {
                    //if to see if the 1st tab item is selected
                    if (tiTabItem1.IsSelected)
                    {
                        //clears all the items in the list box
                        lbTest.ItemsSource = null;
                        listTests.Clear();
                        //foreach that will loop all the student test to display
                        foreach (var AllStudentTest in student.StudentTests(userID))
                        {
                            //display all the test in test list box
                            listTests.Add(AllStudentTest);
                        }

                        lbTest.ItemsSource = listTests;
                        //search text box cleared
                        txbSearchTextTest.Clear();
                    }
                    //if to see if the 2nd tab item is selected
                    if (tiTabItem2.IsSelected)
                    {
                        //clears all the items in the list box
                        lbResults.ItemsSource = null;
                        listResults.Clear();
                        //foreach that will loop all the student test results to display
                        foreach (var AllStudentTestResults in student.StudentResults(userID))
                        {
                            //display all the results in results list box
                            listResults.Add(AllStudentTestResults);
                        }
                        //search text box cleared
                        lbResults.ItemsSource = listResults;
                        txbSearchTextTest.Clear();
                    }
                }
                //if to see if the combo box equel to test name
                if ("Test Name" == cbxSearchByTest.Text)
                {
                    //if to see if the 1st tab item is selected
                    if (tiTabItem1.IsSelected)
                    {
                        //clears all the items in the list box
                        lbTest.ItemsSource = null;
                        listTests.Clear();
                        //loops all the test with the specified test title
                        foreach (var sTestTitle in search.SearchTestTitle(userID, txbSearchTextTest.Text))
                        {
                            //display all the results in test list box
                            listTests.Add(sTestTitle);
                        }
                        lbTest.ItemsSource = listTests;
                    }
                    //if to see if the 2nd tab item is selected
                    if (tiTabItem2.IsSelected)
                    {
                        //clears all the items in the list box
                        lbResults.ItemsSource = null;
                        listResults.Clear();
                        //loops all the test with the specified test title
                        foreach (var sTestTitle in search.SearchTestTitle(userID, txbSearchTextTest.Text))
                        {
                            //display all the results in results list box
                            listResults.Add(sTestTitle);
                        }
                        lbResults.ItemsSource = listResults;
                    }
                }
                //if to see if the combo box equel to module
                if ("Module" == cbxSearchByTest.Text)
                {
                    //if to see if the 1st tab item is selected
                    if (tiTabItem1.IsSelected)
                    {
                        //clears all the items in the list box
                        lbTest.ItemsSource = null;
                        listTests.Clear();
                        //loops all the test with the specified module name
                        foreach (var sModule in search.SearchModule(userID, txbSearchTextTest.Text))
                        {
                            //display all the results in test list box
                            listTests.Add(sModule);
                        }
                        lbTest.ItemsSource = listTests;
                    }
                    //if to see if the 2nd tab item is selected
                    if (tiTabItem2.IsSelected)
                    {
                        //clears all the items in the list box
                        lbResults.ItemsSource = null;
                        listResults.Clear();
                        //loops all the test with the specified module name
                        foreach (var sModule in search.SearchModule(userID, txbSearchTextTest.Text))
                        {
                            //display all the results in results list box
                            listResults.Add(sModule);
                        }
                        lbResults.ItemsSource = listResults;
                    }
                }
                //checks if the user role equels to lecture
                if (userRole == "lecture")
                {
                    //if to see if the 2nd tab item is selected
                    if (tiTabItem2.IsSelected)
                    {
                        //clears all the items in the list box
                        lbResults.ItemsSource = null;
                        listResults.Clear();
                        //loops all the students based on the user input
                        foreach (var studentResults in lecture.ViewStudents(lecture.LectureModules(userID), txbSearchTextTest.Text, cbxSearchByTest.Text))
                        {
                            listResults.Add(studentResults);
                        }
                        lbResults.ItemsSource = listResults;
                    }
                    //if to see if the 1st tab item is selected
                    if (tiTabItem1.IsSelected)
                    {
                        //if to see if the combo box equel to all test
                        if ("All Tests" == cbxSearchByTest.Text)
                        {
                            //clears all the items in the list box
                            lbTest.ItemsSource = null;
                            listTests.Clear();
                            foreach (var sdt in lecture.TestCreated(userID))
                            {
                                //adding the results to a list box
                                listTests.Add(sdt);
                            }
                            lbTest.ItemsSource = listTests;
                        }
                        //if to see if the combo box equel to test name
                        if ("Test Name" == cbxSearchByTest.Text)
                        {
                            //clears all the items in the list box
                            lbTest.ItemsSource = null;
                            listTests.Clear();
                            //loops trough all the tests created by the user with same test title the user inputs
                            foreach (var lTestTitle in lecture.TestCreated(userID, txbSearchTextTest.Text, "Test Name"))
                            {
                                //display all the results in results list box
                                listTests.Add(lTestTitle);
                            }
                            lbTest.ItemsSource = listTests;
                        }
                        //if to see if the combo box equel to module
                        if ("Module" == cbxSearchByTest.Text)
                        {
                            //clears all the items in the list box
                            lbTest.ItemsSource = null;
                            listTests.Clear();
                            //loops trough all the tests created by the user with same module name the user inputs
                            foreach (var lModule in lecture.TestCreated(userID, txbSearchTextTest.Text, "Module"))
                            {
                                //display all the results in results list box
                                listTests.Add(lModule);
                            }
                            lbTest.ItemsSource = listTests;
                        }
                    }
                    //if to see if all test are equel to combo box text
                    if ("All Tests" == cbxSearchByTest.Text)
                    {
                        //clears the search text box
                        txbSearchTextTest.Clear();
                    }
                }
            }
            catch (Exception)
            {
                //displaying to the user an error to the database
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private void btnPublishStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] userSelect = lbTest.SelectedItem.ToString().Split(' ');
                string uS = userSelect[0];
                string status = getBetween(lbTest.SelectedItem.ToString(), "      ", "\n");
                lecture.Publish(uS, status);

                listTests.Clear();
                lbTest.ItemsSource = null;

                foreach (var item in lecture.TestCreated(userID))
                {
                    listTests.Add(item);
                }

                lbTest.ItemsSource = listTests;
                btnPublishStatus.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                //displaying to the user an error to the database
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //checks if the result list box selected index is not equel to minus 1
                if (lbTest.SelectedIndex != -1)
                {
                    //splits the content of the chosen item to get the test id
                    string[] userSelect = lbTest.SelectedItem.ToString().Split(' ');
                    //store the test idin user select variable (uS)
                    string uS = userSelect[0];
                    //takes the user to the test window where they can take the selected test
                    TestDetails newWindow = new TestDetails(uS, userID, userRole);
                    newWindow.DataContext = this;
                    this.Hide();
                    newWindow.ShowDialog();
                }
                else
                {
                    //message box to tell the user a student is not selected
                    MessageBox.Show("Please select a test to view edit",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                //displaying to the user an error to the database
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LbResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if results list boxis grater than minus 1
            if (lbResults.SelectedIndex > -1)
            {
                //changes the test list box index to minus 1
                lbTest.SelectedIndex = -1;
            }

            btnPublishStatus.Visibility = Visibility.Hidden;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
