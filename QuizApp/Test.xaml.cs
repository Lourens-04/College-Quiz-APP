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
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        //declaring test id, user id and user role
        string testId, userID, userRole;
        //created delegate to count the user marks
        public delegate void Count(int value);
        //calling the student object
        Student test = new Student();
        //created list to store all the questions of a test
        List<CreatedTestLayout> testDisplay = new List<CreatedTestLayout>();
        //list of type string to store a user answers
        List<string> answers = new List<string>();
        //declaring question number to keep track of a user question
        int qnum = 0;
        //declaring a user marks
        int uMark = 0;
        //declaring question order to display to the user on their test 
        int qOrder;
        //variable to check for errors
        Boolean error = false;

        public Test(string testId, string userRole, string userID)
        {
            InitializeComponent();
            this.testId = testId;
            this.userRole = userRole;
            this.userID = userID;
            testDisplay.AddRange(test.DisplayTest(testId));
        }

        //next button to go to the next question
        //---------------------------------------------------------------------------------------
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            //if stement to see if there are errors made by the user
            if (Error() == false)
            {
                //increments question number by 1
                qnum++;

                //if to see if the question number is less or equel to the answer list count
                if (qnum >= answers.Count)
                {
                    //turns the radio buttons to false
                    rbtnQA.IsChecked = false;
                    rbtnQB.IsChecked = false;
                    rbtnQC.IsChecked = false;
                }
                else
                {
                    //if to see if answer at question number equels A 
                    if (answers[qnum] == "A")
                    {
                        //turn the radio buttons to true
                        rbtnQA.IsChecked = true;
                    }
                    //if to see if answer at question number equels B
                    if (answers[qnum] == "B")
                    {
                        //turn the radio buttons to true
                        rbtnQB.IsChecked = true;
                    }
                    //if to see if answer at question number equels C
                    if (answers[qnum] == "C")
                    {
                        //turn the radio buttons to true
                        rbtnQC.IsChecked = true;
                    }
                }

                //if to see if the question number is less or equel to the answer list count minus 1
                if (qnum <= testDisplay.Count - 1)
                {
                    //question order variable equel to question number
                    qOrder = qnum;
                    //clears the rich text block
                    rtxbQuestion.Document.Blocks.Clear();
                    //adds the question to the rich text block
                    rtxbQuestion.AppendText("Q." + (qOrder + 1) + ") " + testDisplay[qnum].Question + "\n");
                    //change the mark of the question to the mark for the question
                    lblMark.Content = "Mark : " + testDisplay[qnum].QMark;
                    //sets the content of the radio button to one of the options
                    rbtnQA.Content = "A. " + testDisplay[qnum].QA;
                    //sets the content of the radio button to one of the options
                    rbtnQB.Content = "B. " + testDisplay[qnum].QB;
                    //sets the content of the radio button to one of the options
                    rbtnQC.Content = "C. " + testDisplay[qnum].QC;
                }

                //if question number equels test display count minus 1
                if (qnum == testDisplay.Count - 1)
                {
                    //hides the next button
                    btnNext.Visibility = Visibility.Hidden;
                    //shows the back button
                    btnBack.Visibility = Visibility.Visible;
                }
            }
            else
            {
                //turn error variable to false
                error = false;
            }
        }
        //---------------------------------------------------------------------------------------

        private void Window_Closed(object sender, EventArgs e)
        {
            //lets the user close the program
            Application.Current.Shutdown();
        }

        private void RbtnQA_Checked(object sender, RoutedEventArgs e)
        {
            //if to see if the question number equels to the total items in the answer list
            if (qnum == answers.Count)
            {
                //adds A to the answer list
                answers.Add("A");
            }
            else
            {
                //answer at question number equel A
                answers[qnum] = "A";
            }
        }

        private void RbtnQB_Checked(object sender, RoutedEventArgs e)
        {
            //if to see if the question number equels to the total items in the answer list
            if (qnum == answers.Count)
            {
                //adds B to the answer list
                answers.Add("B");
            }
            else
            //answer at question number equel B
            {
                answers[qnum] = "B";
            }
        }

        private void RbtnQC_Checked(object sender, RoutedEventArgs e)
        {
            //if to see if the question number equels to the total items in the answer list
            if (qnum == answers.Count)
            {
                //adds C to the answer list
                answers.Add("C");
            }
            else
            {
                //answer at question number equel C
                answers[qnum] = "C";
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if stement to see if there are errors made by the user
                if (Error() == false)
                {
                    //if to see if the test diplay total equels question number
                    if (testDisplay.Count - 1 == qnum)
                    {
                        //string answer declared
                        string ans = "";
                        //question number equels to 0
                        qnum = 0;
                        //foreach that will go through all the items in the answer list
                        foreach (var usersAns in answers)
                        {
                            //answer equels to answer
                            ans = ans + usersAns + " ";
                            //if to see if question answer in test display at question number equels user answer
                            if (testDisplay[qnum].QAns == usersAns)
                            {
                                //calls the delegate and equels it to user mark method
                                Count countMarks = UserMarkCount;
                                //send the question mark to the method
                                countMarks(Convert.ToInt32(testDisplay[qnum].QMark));
                            }
                            //plus 1 to question number
                            qnum++;
                        }
                        //trims the answer string
                        ans = ans.Trim();
                        //sends details to the save student method in the student class to save to the database 
                        test.SaveStudentTest(Convert.ToInt32(testId), Convert.ToInt32(userID), ans, uMark);
                        //message box that says the te test have been saved
                        MessageBox.Show("Thank you for taking the test.",
                                "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        //returns the user to the memo page to see how they done on the test
                        TestMemo newWindow = new TestMemo(testId, userID, userRole);
                        newWindow.DataContext = this;
                        this.Hide();
                        newWindow.ShowDialog();
                    }
                    else
                    {
                        //message box that tells a user the test is not completed yet
                        MessageBox.Show("The test is not completed yet",
                                "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    //turns error variable to false
                    error = false;
                }
            }
            catch (Exception)
            {
                //displays if the connection to the database is dropped
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //method to calculate a user marks
        //----------------------------------------
        public void UserMarkCount(int mark)
        {
            //adds up user marks
            uMark = uMark + mark;
        }
        //----------------------------------------

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //returns a user back to the user menu
            UserMenu newWindow = new UserMenu(userID, userRole);
            newWindow.DataContext = this;
            this.Hide();
            newWindow.ShowDialog();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //if statement to see if there are any erros made
            if (Error() == false)
            {
                //minuses 1 of question number
                qnum--;
                //if to see if the question number is grater than 0
                if (qnum > 0)
                {
                    //if the question number is grater or equel to answer total items
                    if (qnum >= answers.Count)
                    {
                        //turns all radio buttons to false
                        rbtnQA.IsChecked = false;
                        rbtnQB.IsChecked = false;
                        rbtnQC.IsChecked = false;
                    }
                    else
                    {
                        //if to see if the answer at question number is equel to A
                        if (answers[qnum] == "A")
                        {
                            //radio button equel to true
                            rbtnQA.IsChecked = true;
                        }
                        //if to see if the answer at question number is equel to B
                        if (answers[qnum] == "B")
                        {
                            //radio button equel to true
                            rbtnQB.IsChecked = true;
                        }
                        //if to see if the answer at question number is equel to C
                        if (answers[qnum] == "C")
                        {
                            //radio button equel to true
                            rbtnQC.IsChecked = true;
                        }
                    }

                    //question order equels to the question number
                    qOrder = qnum;
                    //clears the rich text box item
                    rtxbQuestion.Document.Blocks.Clear();
                    //adds the question to the rich text box
                    rtxbQuestion.AppendText("Q." + (qOrder + 1) + ") " + testDisplay[qnum].Question + "\n");
                    //change the mark of the question to the mark for the question
                    lblMark.Content = "Mark : " + testDisplay[qnum].QMark;
                    //sets the content of the radio button to one of the options
                    rbtnQA.Content = "A. " + testDisplay[qnum].QA;
                    //sets the content of the radio button to one of the options
                    rbtnQB.Content = "B. " + testDisplay[qnum].QB;
                    //sets the content of the radio button to one of the options
                    rbtnQC.Content = "C. " + testDisplay[qnum].QC;
                }
                else
                {
                    //turn Visibility back button to hidden and next to visable
                    btnBack.Visibility = Visibility.Hidden;
                    btnNext.Visibility = Visibility.Visible;
                    //question number equel to 0
                    qnum = 0;
                    //if question number equel to 0
                    if (qnum == 0)
                    {
                        //if the question number is grater or equel to answer total items
                        if (qnum >= answers.Count)
                        {
                            //turns all radio buttons to false
                            rbtnQA.IsChecked = false;
                            rbtnQB.IsChecked = false;
                            rbtnQC.IsChecked = false;
                        }
                        else
                        {
                            //if to see if the answer at question number is equel to A
                            if (answers[qnum] == "A")
                            //radio button equel to true
                            {
                                rbtnQA.IsChecked = true;
                            }
                            //if to see if the answer at question number is equel to B
                            if (answers[qnum] == "B")
                            {
                                //radio button equel to true
                                rbtnQB.IsChecked = true;
                            }
                            //if to see if the answer at question number is equel to C
                            if (answers[qnum] == "C")
                            {
                                //radio button equel to true
                                rbtnQC.IsChecked = true;
                            }
                        }
                        //question order equels to the question number
                        qOrder = qnum;
                        //clears the rich text box item
                        rtxbQuestion.Document.Blocks.Clear();
                        //adds the question to the rich text box
                        rtxbQuestion.AppendText("Q." + (qOrder + 1) + ") " + testDisplay[qnum].Question + "\n");
                        //change the mark of the question to the mark for the question
                        lblMark.Content = "Mark : " + testDisplay[qnum].QMark;
                        //sets the content of the radio button to one of the options
                        rbtnQA.Content = "A. " + testDisplay[qnum].QA;
                        //sets the content of the radio button to one of the options
                        rbtnQB.Content = "B. " + testDisplay[qnum].QB;
                        //sets the content of the radio button to one of the options
                        rbtnQC.Content = "C. " + testDisplay[qnum].QC;
                    }
                }
            }
            else
            {
                //turns error variable to false
                error = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //shows what user is loged in 
                lblLogInDetails.Content = "Loged In As : " + userID;
                //if to see if the test diplay total equels question number
                if (qnum == testDisplay.Count - 1)
                {
                    //hides next button 
                    btnNext.Visibility = Visibility.Hidden;
                }
                //if question number equel to 0
                if (qnum == 0)
                {
                    //if the question number is grater or equel to answer total items
                    if (qnum >= answers.Count)
                    {
                        //turns all radio buttons to false
                        rbtnQA.IsChecked = false;
                        rbtnQB.IsChecked = false;
                        rbtnQC.IsChecked = false;
                    }
                    else
                    {
                        //if to see if the answer at question number is equel to A
                        if (answers[qnum] == "A")
                        {
                            //radio button equel to true
                            rbtnQA.IsChecked = true;
                        }
                        //if to see if the answer at question number is equel to B
                        if (answers[qnum] == "B")
                        {
                            //radio button equel to true
                            rbtnQB.IsChecked = true;
                        }
                        //if to see if the answer at question number is equel to C
                        if (answers[qnum] == "C")
                        {
                            //radio button equel to true
                            rbtnQC.IsChecked = true;
                        }
                    }
                    //question order equels to the question number
                    qOrder = qnum;
                    //clears the rich text box item
                    rtxbQuestion.Document.Blocks.Clear();
                    //adds the question to the rich text box
                    rtxbQuestion.AppendText("Q." + (qOrder + 1) + ") " + testDisplay[qnum].Question + "\n");
                    //change the mark of the question to the mark for the question
                    lblMark.Content = "Mark : " + testDisplay[qnum].QMark;
                    //sets the content of the radio button to one of the options
                    rbtnQA.Content = "A. " + testDisplay[qnum].QA;
                    //sets the content of the radio button to one of the options
                    rbtnQB.Content = "B. " + testDisplay[qnum].QB;
                    //sets the content of the radio button to one of the options
                    rbtnQC.Content = "C. " + testDisplay[qnum].QC;
                    //hides the back button
                    btnBack.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            {
                //displays if the connection to the database is dropped
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public Boolean Error()
        {
            //if statement to see if the user have not chosen an answer to a question
            //------------------------------------------------------------------------------------------
            if (rbtnQA.IsChecked == false && rbtnQB.IsChecked == false && rbtnQC.IsChecked == false)
            {
                error = true;
                MessageBox.Show("A answer was not selected, please insure that a answer is selected to continue.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //------------------------------------------------------------------------------------------

            //return the boolean value
            return error;
        }
    }
}

