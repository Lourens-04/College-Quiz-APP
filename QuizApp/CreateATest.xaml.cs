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
    /// Interaction logic for CreateATest.xaml
    /// </summary>
    public partial class CreateATest : Window
    {
        //Declareing the user ID their role the test title and test description
        string userID, userRole, testTitle, testDesc, testID;

        //declareing the question number to display to a user and to keep track of created questions
        int qnum = 1;

        //Dictonary created to enable a user to go back to a question they created
        Dictionary<int, CreatedTestQuestions> questionsAns = new Dictionary<int, CreatedTestQuestions>();

        //Declaring the class to set the questions deatails the user enters
        CreatedTestQuestions test;

        //Boolean created to check for any errors that can happen
        Boolean error = false;

        //calling the lecture class to use some of the functions users need within it 
        Lecture lecturefun = new Lecture();

        Student testCreated = new Student();

        //declaring an int that will be used to see for what module is this test for
        int moduleID;

        //CreateATest method
        //-----------------------------------------------------------------------------------------
        public CreateATest(string userRole, string userID, int moduleID, string testTitle, string testDesc)
        {
            InitializeComponent();
            //sets userrole user id module id test title and test description
            this.userRole = userRole;
            this.userID = userID;
            this.moduleID = moduleID;
            this.testTitle = testTitle;
            this.testDesc = testDesc;
        }
        //-----------------------------------------------------------------------------------------

        //CreateATest method
        //-----------------------------------------------------------------------------------------
        public CreateATest(string userRole, string userID, int moduleID, string testTitle, string testDesc, string testID)
        {
            InitializeComponent();
            //sets userrole user id module id test title and test description
            this.userRole = userRole;
            this.userID = userID;
            this.moduleID = moduleID;
            this.testTitle = testTitle;
            this.testDesc = testDesc;
            this.testID = testID;
        }
        //-----------------------------------------------------------------------------------------

        //Button that will take the user back to the home screen if they wish to not save the test
        //-----------------------------------------------------------------------------------------
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            UserMenu newWindow = new UserMenu(userID, userRole);
            newWindow.DataContext = this;
            this.Hide();
            newWindow.ShowDialog();
        }
        //-----------------------------------------------------------------------------------------

        //button that is used to let the user view a previous question they added
        //-----------------------------------------------------------------------------------------
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //String array to split the question label to get the question number
            string[] lblQNum = lblInfo.Content.ToString().Split(' ');

            //setting the question num to a variable of type int that will hold the question num
            qnum = Convert.ToInt32(lblQNum[5]);

            //if statement to see if my created dictionary contains the question number/ question ID 
            if (questionsAns.ContainsKey(qnum - 1))
            {
                //Turns the question label back to the previous question
                lblInfo.Content = "Enter in a Question : " + (qnum - 1);

                //getting the question details that the user added and putting it into a holder
                CreatedTestQuestions holder = questionsAns[qnum - 1];

                //if staement to see if the question answer is equel to A
                if (holder.QAns == "A")
                {
                    //clears the rich text block 
                    rtxbQuestion.Document.Blocks.Clear();
                    //display the question to the rich text block
                    rtxbQuestion.AppendText(holder.Question);
                    //the text box will be populated with answer A 
                    txbQA.Text = holder.QA;
                    //the text box will be populated with answer B
                    txbQB.Text = holder.QB;
                    //the text box will be populated with answer C
                    txbQC.Text = holder.QC;
                    //the text box will be populated with the question mark 
                    txbQMark.Text = holder.QMark.ToString();
                    //turns the radio A button is checked valuee to true
                    rbtnQA.IsChecked = true;
                }
                //if staement to see if the question answer is equel to B
                if (holder.QAns == "B")
                {
                    //clears the rich text block 
                    rtxbQuestion.Document.Blocks.Clear();
                    //display the question to the rich text block
                    rtxbQuestion.AppendText(holder.Question);
                    //the text box will be populated with answer A 
                    txbQA.Text = holder.QA;
                    //the text box will be populated with answer B
                    txbQB.Text = holder.QB;
                    //the text box will be populated with answer C
                    txbQC.Text = holder.QC;
                    //the text box will be populated with the question mark 
                    txbQMark.Text = holder.QMark.ToString();
                    //turns the radio B button is checked valuee to true
                    rbtnQB.IsChecked = true;
                }
                //if staement to see if the question answer is equel to C
                if (holder.QAns == "C")
                {
                    //clears the rich text block 
                    rtxbQuestion.Document.Blocks.Clear();
                    //display the question to the rich text block
                    rtxbQuestion.AppendText(holder.Question);
                    //the text box will be populated with answer A 
                    txbQA.Text = holder.QA;
                    //the text box will be populated with answer B
                    txbQB.Text = holder.QB;
                    //the text box will be populated with answer C
                    txbQC.Text = holder.QC;
                    //the text box will be populated with the question mark 
                    txbQMark.Text = holder.QMark.ToString();
                    //turns the radio C button is checked valuee to true
                    rbtnQC.IsChecked = true;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //lecture logged in and the test they are creating
                lblLogInDetails.Content = "Loged In As : " + userID + "               Create a Test For : " + testTitle;

                //if to see if the test id variable is null or has a value
                if (testID != null)
                {
                    //taking all the questions in the database an populating it in the dictonary list to display to the user
                    //----------------------------------------------------------------------------------
                    int i = 1;
                    foreach (var item in testCreated.DisplayTest(testID))
                    {
                        test = new CreatedTestQuestions(item.Question, item.QA, item.QB, item.QC, item.QAns, Convert.ToInt32(item.QMark));
                        questionsAns.Add(i, test);
                        i++;
                    }
                    //----------------------------------------------------------------------------------

                    //String array to split the question label to get the question number
                    string[] lblQNum = lblInfo.Content.ToString().Split(' ');

                    //setting the question num to a variable of type int that will hold the question num
                    qnum = Convert.ToInt32(lblQNum[5]);

                    //if statement to see if my created dictionary contains the question number/ question ID 
                    if (questionsAns.ContainsKey(qnum))
                    {

                        //setting the question num to a variable of type int that will hold the question num
                        CreatedTestQuestions holder = questionsAns[qnum];

                        //if staement to see if the question answer is equel to A
                        if (holder.QAns == "A")
                        {
                            //clears the rich text block 
                            rtxbQuestion.Document.Blocks.Clear();
                            //display the question to the rich text block
                            rtxbQuestion.AppendText(holder.Question);
                            //the text box will be populated with answer A 
                            txbQA.Text = holder.QA;
                            //the text box will be populated with answer B
                            txbQB.Text = holder.QB;
                            //the text box will be populated with answer C
                            txbQC.Text = holder.QC;
                            //the text box will be populated with the question mark 
                            txbQMark.Text = holder.QMark.ToString();
                            //turns the radio A button is checked valuee to true
                            rbtnQA.IsChecked = true;
                        }
                        //if staement to see if the question answer is equel to B
                        if (holder.QAns == "B")
                        {
                            //clears the rich text block 
                            rtxbQuestion.Document.Blocks.Clear();
                            //display the question to the rich text block
                            rtxbQuestion.AppendText(holder.Question);
                            //the text box will be populated with answer A 
                            txbQA.Text = holder.QA;
                            //the text box will be populated with answer B
                            txbQB.Text = holder.QB;
                            //the text box will be populated with answer C
                            txbQC.Text = holder.QC;
                            //the text box will be populated with the question mark
                            txbQMark.Text = holder.QMark.ToString();
                            //turns the radio B button is checked valuee to true
                            rbtnQB.IsChecked = true;
                        }
                        //if staement to see if the question answer is equel to C
                        if (holder.QAns == "C")
                        {
                            //clears the rich text block 
                            rtxbQuestion.Document.Blocks.Clear();
                            //display the question to the rich text block
                            rtxbQuestion.AppendText(holder.Question);
                            //the text box will be populated with answer A 
                            txbQA.Text = holder.QA;
                            //the text box will be populated with answer B
                            txbQB.Text = holder.QB;
                            //the text box will be populated with answer C
                            txbQC.Text = holder.QC;
                            //the text box will be populated with the question mark 
                            txbQMark.Text = holder.QMark.ToString();
                            //turns the radio C button is checked valuee to true
                            rbtnQC.IsChecked = true;
                        }
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
        //-----------------------------------------------------------------------------------------

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            //if statement to see if the user has made any errors on the page that could break the program
            if (Error() == false)
            {
                if (testID != null)
                {
                    //String array to split the question label to get the question number
                    string[] lblQNum = lblInfo.Content.ToString().Split(' ');

                    //setting the question num to a variable of type int that will hold the question num
                    qnum = Convert.ToInt32(lblQNum[5]);

                    if (questionsAns.Count > Convert.ToInt32(qnum))
                    {
                        //Getting the test description to get all the text
                        string question = new TextRange(rtxbQuestion.Document.ContentStart, rtxbQuestion.Document.ContentEnd).Text;

                        //string array to split the test description so that I do not get /r and /n
                        string[] rtxbText = question.Split('\r');

                        //geting the question number for the next question
                        lblInfo.Content = "Enter in a Question : " + (qnum + 1);

                        //if statement to see if the A radio button is checked
                        if (rbtnQA.IsChecked == true)
                        {
                            //setting the question the question details the user just enterd into a object then into a list
                            test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "A", Convert.ToInt32(txbQMark.Text));
                        }
                        //if statement to see if the B radio button is checked
                        if (rbtnQB.IsChecked == true)
                        {
                            //setting the question the question details the user just enterd into a object then into a list
                            test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "B", Convert.ToInt32(txbQMark.Text));
                        }
                        //if statement to see if the C radio button is checked
                        if (rbtnQC.IsChecked == true)
                        {
                            //setting the question the question details the user just enterd into a object then into a list
                            test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "C", Convert.ToInt32(txbQMark.Text));
                        }

                        //if statement to see if my created dictionary contains the question number/ question ID 
                        if (questionsAns.ContainsKey(qnum))
                        {
                            //Gets the question that was added
                            questionsAns[qnum] = test;
                        }
                        else
                        {
                            //Adds the question if the question was not enterd
                            questionsAns.Add(qnum, test);
                        }

                        //if statement to see if my created dictionary contains the question number/ question ID 
                        if (questionsAns.ContainsKey(qnum + 1))
                        {
                            //Turns the question label back to the next question
                            lblInfo.Content = "Enter in a Question : " + (qnum + 1);

                            //setting the question num to a variable of type int that will hold the question num
                            CreatedTestQuestions holder = questionsAns[qnum + 1];

                            //if staement to see if the question answer is equel to A
                            if (holder.QAns == "A")
                            {
                                //clears the rich text block 
                                rtxbQuestion.Document.Blocks.Clear();
                                //display the question to the rich text block
                                rtxbQuestion.AppendText(holder.Question);
                                //the text box will be populated with answer A 
                                txbQA.Text = holder.QA;
                                //the text box will be populated with answer B
                                txbQB.Text = holder.QB;
                                //the text box will be populated with answer C
                                txbQC.Text = holder.QC;
                                //the text box will be populated with the question mark 
                                txbQMark.Text = holder.QMark.ToString();
                                //turns the radio A button is checked valuee to true
                                rbtnQA.IsChecked = true;
                            }
                            //if staement to see if the question answer is equel to B
                            if (holder.QAns == "B")
                            {
                                //clears the rich text block 
                                rtxbQuestion.Document.Blocks.Clear();
                                //display the question to the rich text block
                                rtxbQuestion.AppendText(holder.Question);
                                //the text box will be populated with answer A 
                                txbQA.Text = holder.QA;
                                //the text box will be populated with answer B
                                txbQB.Text = holder.QB;
                                //the text box will be populated with answer C
                                txbQC.Text = holder.QC;
                                //the text box will be populated with the question mark
                                txbQMark.Text = holder.QMark.ToString();
                                //turns the radio B button is checked valuee to true
                                rbtnQB.IsChecked = true;
                            }
                            //if staement to see if the question answer is equel to C
                            if (holder.QAns == "C")
                            {
                                //clears the rich text block 
                                rtxbQuestion.Document.Blocks.Clear();
                                //display the question to the rich text block
                                rtxbQuestion.AppendText(holder.Question);
                                //the text box will be populated with answer A 
                                txbQA.Text = holder.QA;
                                //the text box will be populated with answer B
                                txbQB.Text = holder.QB;
                                //the text box will be populated with answer C
                                txbQC.Text = holder.QC;
                                //the text box will be populated with the question mark 
                                txbQMark.Text = holder.QMark.ToString();
                                //turns the radio C button is checked valuee to true
                                rbtnQC.IsChecked = true;
                            }
                        }
                        else
                        {
                            //clears the rich text block 
                            rtxbQuestion.Document.Blocks.Clear();
                            //clears the text box for Question A
                            txbQA.Text = "";
                            //clears the text box for Question B
                            txbQB.Text = "";
                            //clears the text box for Question C
                            txbQC.Text = "";
                            //clears the text box for Question mark
                            txbQMark.Text = "";
                            //turn the radio buttons back to false
                            //-----------------------------
                            rbtnQA.IsChecked = false;
                            rbtnQB.IsChecked = false;
                            rbtnQC.IsChecked = false;
                            //-----------------------------
                        }
                    }
                    else
                    {
                        //message box to display to the user that thre is no test filled in
                        MessageBox.Show("There are no more questions to edit",
                                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    //Getting the test description to get all the text
                    string question = new TextRange(rtxbQuestion.Document.ContentStart, rtxbQuestion.Document.ContentEnd).Text;

                    //string array to split the test description so that I do not get /r and /n
                    string[] rtxbText = question.Split('\r');

                    //String array to split the question label to get the question number
                    string[] lblQNum = lblInfo.Content.ToString().Split(' ');

                    //setting the question num to a variable of type int that will hold the question num
                    qnum = Convert.ToInt32(lblQNum[5]);

                    //geting the question number for the next question
                    lblInfo.Content = "Enter in a Question : " + (qnum + 1);

                    //if statement to see if the A radio button is checked
                    if (rbtnQA.IsChecked == true)
                    {
                        //setting the question the question details the user just enterd into a object then into a list
                        test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "A", Convert.ToInt32(txbQMark.Text));
                    }
                    //if statement to see if the B radio button is checked
                    if (rbtnQB.IsChecked == true)
                    {
                        //setting the question the question details the user just enterd into a object then into a list
                        test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "B", Convert.ToInt32(txbQMark.Text));
                    }
                    //if statement to see if the C radio button is checked
                    if (rbtnQC.IsChecked == true)
                    {
                        //setting the question the question details the user just enterd into a object then into a list
                        test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "C", Convert.ToInt32(txbQMark.Text));
                    }

                    //if statement to see if my created dictionary contains the question number/ question ID 
                    if (questionsAns.ContainsKey(qnum))
                    {
                        //Gets the question that was added
                        questionsAns[qnum] = test;
                    }
                    else
                    {
                        //Adds the question if the question was not enterd
                        questionsAns.Add(qnum, test);
                    }

                    //if statement to see if my created dictionary contains the question number/ question ID 
                    if (questionsAns.ContainsKey(qnum + 1))
                    {
                        //Turns the question label back to the next question
                        lblInfo.Content = "Enter in a Question : " + (qnum + 1);

                        //setting the question num to a variable of type int that will hold the question num
                        CreatedTestQuestions holder = questionsAns[qnum + 1];

                        //if staement to see if the question answer is equel to A
                        if (holder.QAns == "A")
                        {
                            //clears the rich text block 
                            rtxbQuestion.Document.Blocks.Clear();
                            //display the question to the rich text block
                            rtxbQuestion.AppendText(holder.Question);
                            //the text box will be populated with answer A 
                            txbQA.Text = holder.QA;
                            //the text box will be populated with answer B
                            txbQB.Text = holder.QB;
                            //the text box will be populated with answer C
                            txbQC.Text = holder.QC;
                            //the text box will be populated with the question mark 
                            txbQMark.Text = holder.QMark.ToString();
                            //turns the radio A button is checked valuee to true
                            rbtnQA.IsChecked = true;
                        }
                        //if staement to see if the question answer is equel to B
                        if (holder.QAns == "B")
                        {
                            //clears the rich text block 
                            rtxbQuestion.Document.Blocks.Clear();
                            //display the question to the rich text block
                            rtxbQuestion.AppendText(holder.Question);
                            //the text box will be populated with answer A 
                            txbQA.Text = holder.QA;
                            //the text box will be populated with answer B
                            txbQB.Text = holder.QB;
                            //the text box will be populated with answer C
                            txbQC.Text = holder.QC;
                            //the text box will be populated with the question mark
                            txbQMark.Text = holder.QMark.ToString();
                            //turns the radio B button is checked valuee to true
                            rbtnQB.IsChecked = true;
                        }
                        //if staement to see if the question answer is equel to C
                        if (holder.QAns == "C")
                        {
                            //clears the rich text block 
                            rtxbQuestion.Document.Blocks.Clear();
                            //display the question to the rich text block
                            rtxbQuestion.AppendText(holder.Question);
                            //the text box will be populated with answer A 
                            txbQA.Text = holder.QA;
                            //the text box will be populated with answer B
                            txbQB.Text = holder.QB;
                            //the text box will be populated with answer C
                            txbQC.Text = holder.QC;
                            //the text box will be populated with the question mark 
                            txbQMark.Text = holder.QMark.ToString();
                            //turns the radio C button is checked valuee to true
                            rbtnQC.IsChecked = true;
                        }
                    }
                    else
                    {
                        //clears the rich text block 
                        rtxbQuestion.Document.Blocks.Clear();
                        //clears the text box for Question A
                        txbQA.Text = "";
                        //clears the text box for Question B
                        txbQB.Text = "";
                        //clears the text box for Question C
                        txbQC.Text = "";
                        //clears the text box for Question mark
                        txbQMark.Text = "";
                        //turn the radio buttons back to false
                        //-----------------------------
                        rbtnQA.IsChecked = false;
                        rbtnQB.IsChecked = false;
                        rbtnQC.IsChecked = false;
                        //-----------------------------
                    }
                }
            }
            else
            {
                //turns the error variable back to false
                error = false;
            }
        }
        
        //method to close the window if the user closes the program
        //------------------------------------------------------------
        private void Window_Closed(object sender, EventArgs e)
        {
            //closing the window
            Application.Current.Shutdown();
        }
        //------------------------------------------------------------

        //Button to save the answers if the user is done making the test
        //------------------------------------------------------------
        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if statement to see if the user has made any errors on the page that could break the program
                if (Error() == false)
                {
                    //Getting the test description to get all the text
                    string question = new TextRange(rtxbQuestion.Document.ContentStart, rtxbQuestion.Document.ContentEnd).Text;

                    //string array to split the test description so that I do not get /r and /n
                    string[] rtxbText = question.Split('\r');

                    //String array to split the question label to get the question number
                    string[] lblQNum = lblInfo.Content.ToString().Split(' ');

                    //setting the question num to a variable of type int that will hold the question num
                    qnum = Convert.ToInt32(lblQNum[5]);

                    //geting the question number for the next question
                    lblInfo.Content = "Enter in a Question : " + (qnum + 1);

                    //if statement to see if the A radio button is checked
                    if (rbtnQA.IsChecked == true)
                    {
                        //setting the question the question details the user just enterd into a object then into a list
                        test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "A", Convert.ToInt32(txbQMark.Text));
                    }
                    //if statement to see if the B radio button is checked
                    if (rbtnQB.IsChecked == true)
                    {
                        //setting the question the question details the user just enterd into a object then into a list
                        test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "B", Convert.ToInt32(txbQMark.Text));
                    }
                    //if statement to see if the C radio button is checked
                    if (rbtnQC.IsChecked == true)
                    {
                        //setting the question the question details the user just enterd into a object then into a list
                        test = new CreatedTestQuestions(rtxbText[0], txbQA.Text, txbQB.Text, txbQC.Text, "C", Convert.ToInt32(txbQMark.Text));
                    }

                    //if statement to see if my created dictionary contains the question number/ question ID 
                    if (questionsAns.ContainsKey(qnum))
                    {
                        //Gets the question that was added
                        questionsAns[qnum] = test;
                    }
                    else
                    {
                        //Adds the question if the question was not enterd
                        questionsAns.Add(qnum, test);
                    }

                    //if to see if the dictonary is empty
                    if (questionsAns.Count != 0)
                    {
                        //declaring a test total variable to calculate the total count of the test
                        int testTotal = 0;

                        //foreach to loop through all the mark to calculate the totlal marks
                        //------------------------------------------------------------
                        foreach (var calcTotal in questionsAns)
                        {
                            testTotal = testTotal + calcTotal.Value.QMark;
                        }
                        //------------------------------------------------------------

                        if (testID != null)
                        {
                            lecturefun.Edit(testTitle, testDesc, moduleID, questionsAns, testTotal, testID);
                        }
                        else
                        {
                            //Sending the varibles to the lecture class, to use the save method that is in there
                            lecturefun.SaveTest(testTitle, testDesc, moduleID, questionsAns, testTotal);
                        }

                        //message box to display to the user that the test was saved
                        MessageBox.Show("Test has been Added",
                                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                        //taking the user back to the home scrren 
                        //-----------------------------------------
                        UserMenu newWindow = new UserMenu(userID, userRole);
                        newWindow.DataContext = this;
                        this.Hide();
                        newWindow.ShowDialog();
                        //-----------------------------------------
                    }
                    else
                    {
                        //message box to display to the user that thre is no test filled in
                        MessageBox.Show("You have not added any questions to the test",
                                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    //turns the error variable back to false
                    error = false;
                }
            }
            catch (Exception)
            {
                //displaying to the user an error to the database
                MessageBox.Show("Connection to the database is lost please ensure you are connected to the internet",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //------------------------------------------------------------

        //Method to return a boolean if there was an error made by the user
        //----------------------------------------------------------------------------------------
        public Boolean Error()
        {
            //Getting the question in the rich text box
            string question = new TextRange(rtxbQuestion.Document.ContentStart, rtxbQuestion.Document.ContentEnd).Text;
            //splitting the rich text box text inside
            string[] rtxbText = question.Split('\r');
            //if staement to see if there is anything in question
            if (rtxbText[0] == "")
            {
                //turns boolean error to true
                error = true;
                //message box to say there is noting filled in
                MessageBox.Show("There is no question filled in.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //if staement to see if there is anything in question A text box
            if (txbQA.Text == "")
            {
                //turns boolean error to true
                error = true;
                //message box to say there is noting filled in
                MessageBox.Show("Answer A is not filled in.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //if staement to see if there is anything in question B text box
            if (txbQB.Text == "")
            {
                //turns boolean error to true
                error = true;
                //message box to say there is noting filled in
                MessageBox.Show("Answer B is not filled in.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //if staement to see if there is anything in question C text box
            if (txbQC.Text == "")
            {
                //turns boolean error to true
                error = true;
                //message box to say there is noting filled in
                MessageBox.Show("Answer C is not filled in.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //if statement to see if any radio button is selected
            if (rbtnQA.IsChecked == false && rbtnQB.IsChecked == false && rbtnQC.IsChecked == false)
            {
                //turns boolean error to true
                error = true;
                //message box to say there is no radio button selected
                MessageBox.Show("Correct answer is not selected.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            try
            {
                //if statementto see the question mark is an integer value
                if (Convert.ToInt32(txbQMark.Text) > 0)
                {
                }
            }
            catch (Exception)
            {
                //turns boolean error to true
                error = true;
                //message box to say that the mark enterd is not of type int
                MessageBox.Show("The mark you enterd is not a number or is equel to 0.",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
            //returns the boolean value
            return error;
        }
        //----------------------------------------------------------------------------------------
    }
}
