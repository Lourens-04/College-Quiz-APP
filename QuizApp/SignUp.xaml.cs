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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        //boolean error that will be used for error checking
        Boolean error = false;

        //calling the database model
        SchoolApplicationAppEntities db = new SchoolApplicationAppEntities();
        List<string> lectureModulesSelect = new List<string>();
        int ID;
        
        //declaring int values that will be used for database primary keys
        //----------------------------------------
        int uID = 100;
        //----------------------------------------

        public SignUp()
        {
            InitializeComponent();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if statement to check if there are any errors made
                if (Error() == false)
                {
                    //if statement to first check if the lecture password is equel to 1234
                    if (txbLecturePassword.Password == "1234")
                    {
                        //sets the user object to add to the database
                        var addUser = db.Set<User>();
                        //getting a primary key for user
                        uID = uID + db.Users.Count();
                        //adds the user details to the user table in the model
                        addUser.Add(new User { User_ID = uID, FirstName = txbUserFirstName.Text, LastName = txbUserLastName.Text, Email = txbEmail.Text, Password = txbPassword.Password.ToString(), UserRole = "lecture" });

                        //sets the lecture module object to add to the database
                        var addStudentCourse = db.Set<Lecture_Module>();

                        //seting the primary key to the courseID variable
                        foreach (var item in lbModule_Course.SelectedItems)
                        {
                            string[] lectureModuleID = item.ToString().Split(' ');
                            ID = Convert.ToInt32(lectureModuleID[0]);
                            //adds the lecture details to the lecture module table in the model
                            addStudentCourse.Add(new Lecture_Module { User_ID = uID, Module_ID = ID });
                        }
                        //saves all changes that were made in the database model
                        db.SaveChanges();
                    }
                    else
                    {
                        //sets the user object to add to the database
                        var addUser = db.Set<User>();
                        //getting a primary key for user
                        uID = uID + db.Users.Count();
                        //adds the user details to the user table in the model
                        addUser.Add(new User { User_ID = uID, FirstName = txbUserFirstName.Text, LastName = txbUserLastName.Text, Email = txbEmail.Text, Password = txbPassword.Password.ToString(), UserRole = "student" });

                        //sets the student course object to add to the database
                        var addStudentCourse = db.Set<User_Course>();

                        string[] lectureModuleID = lbModule_Course.SelectedItem.ToString().Split(' ');
                        ID = Convert.ToInt32(lectureModuleID[0]);

                        //adds the students details to the user course table in the model
                        addStudentCourse.Add(new User_Course { User_ID = uID, Course_ID = ID });

                        //saves all changes that were made in the database model
                        db.SaveChanges();
                    }

                    //message box to display that the user details saved and display a user username
                    MessageBox.Show("Thank you " + txbUserFirstName.Text + " for signing up, here is your username to log into the application: " + uID,
                            "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    //returns the user back to the log in screen after saving their details
                    MainWindow newWindow = new MainWindow();
                    newWindow.DataContext = this;
                    this.Hide();
                    newWindow.ShowDialog();
                }
                else
                {
                    //turns error variable back to false
                    error = false;
                }
            }
            catch (Exception)
            {
                //displays if the connection to the database is dropped
                MessageBox.Show("Information was not saved, check that you are connected to the internet",
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //returns the user back to the log in screen 
            MainWindow newWindow = new MainWindow();
            newWindow.DataContext = this;
            this.Hide();
            newWindow.ShowDialog();
        }

        public Boolean Error()
        {
            //if User first name textbox is empty then turn error boolean value to true
            //---------------------------------------------------------------
            if (txbUserFirstName.Text.Equals(""))
            {
                //turn error value to true
                error = true;

                //Message box to display that User first name was not enter
                MessageBox.Show("User first name was not enterd",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //---------------------------------------------------------------

            //if User last name textbox is empty then turn error boolean value to true
            //---------------------------------------------------------------
            if (txbUserLastName.Text.Equals(""))
            {
                //turn error value to true
                error = true;

                //Message box to display that User last name was not enter
                MessageBox.Show("User last name was not enterd",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //---------------------------------------------------------------

            //if User email textbox is empty then turn error boolean value to true
            //---------------------------------------------------------------
            if (txbEmail.Text.Equals(""))
            {
                //turn error value to true
                error = true;

                //Message box to display that email was not enter
                MessageBox.Show("User email was not enterd",
                        "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //---------------------------------------------------------------

            //if Email does not have an @ simbol
            //---------------------------------------------------------------
            string[] At = txbEmail.Text.Split('@');

            if (At.Length == 1)
            {
                //turn error value to true
                error = true;

                //Message box to display that Email does not have an @ simbol
                MessageBox.Show("You did not enter a good email address",
                "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //---------------------------------------------------------------

            //if password textbox is empty then turn error boolean value to true
            //---------------------------------------------------------------
            if (txbPassword.Password.Equals(""))
            {
                //turn error value to true
                error = true;

                //Message box to display that password was not enter
                MessageBox.Show("Password was not enterd",
                "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //---------------------------------------------------------------

            //if confirm password textbox is empty then turn error boolean value to true
            //---------------------------------------------------------------
            if (txbConfirmPassword.Password.Equals(""))
            {
                //turn error value to true
                error = true;

                //Message box to display that confirm password was not enter
                MessageBox.Show("Confirm Password was not enterd",
                "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //---------------------------------------------------------------

            //if confirm password is not equel to password turn boolean value to true
            //---------------------------------------------------------------
            if (txbPassword.Password != txbConfirmPassword.Password)
            {
                //turn error value to true
                error = true;

                //Message box to display that confirm password was not equel to password
                MessageBox.Show("Confirm Password was not equel to password",
                "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //---------------------------------------------------------------

            //if password is not longer than 8 characters turn error to true
            //---------------------------------------------------------------
            if (txbPassword.Password.Length < 8)
            {
                //turn error value to true
                error = true;

                //Message box to display that password is not long enough
                MessageBox.Show("Password was not long enough, please make sure it is equel or grater than 8 characters",
                "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //---------------------------------------------------------------

            //if the combo box is not selected turn error to true
            //---------------------------------------------------------------
            if (lblSelectCourse.Content.Equals("Select Course :"))
            {
                if (lbModule_Course.SelectedIndex == -1)
                {
                    //turn error value to true
                    error = true;

                    //Message box to display that the combo box is not selected
                    MessageBox.Show("Please select course that will be assigned to your account",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            if (lblSelectCourse.Content.Equals("Select Module :"))
            {
                if (lbModule_Course.SelectedIndex == -1)
                {
                    //turn error value to true
                    error = true;

                    //Message box to display that the combo box is not selected
                    MessageBox.Show("Please select modules that will be assigned to your account",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            //---------------------------------------------------------------

            //Check if th checkbox for forecaster is checked = true
            if (ckTypeUser.IsChecked == true)
            {
                //if the password for lecture password  does not = to the hrdcoded pasword in the system
                if (!txbLecturePassword.Password.Equals("1234"))
                {
                    //Change check value to true
                    error = true;

                    //Message box that disply to the user that the weather forecaster password is incorrect
                    MessageBox.Show("Lecture Password is not enterd or is incorrect",
                    "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            return error;
        }

        private void CkTypeUser_Checked(object sender, RoutedEventArgs e)
        {
            //Will show that a password needs to be enterd if a user wants the role of lecture
            //-------------------------------------------------------------
            if (ckTypeUser.IsChecked == true)
            {
                lbModule_Course.SelectionMode = SelectionMode.Multiple;

                //shows the lecture password text box and lable 
                //-------------------------------------------------
                lblLecturePassword.Visibility = Visibility.Visible;
                txbLecturePassword.Visibility = Visibility.Visible;
                //-------------------------------------------------

                //turns label to select module
                lblSelectCourse.Content = "Select Module :";

                lectureModulesSelect.Clear();

                //clears combo box
                lbModule_Course.ItemsSource = null;

                //linQ to display all the modules
                //-------------------------------------------------
                var allModules = from m in db.Modules
                                 select new { m.Module_ID, m.ModuleCode };
                //-------------------------------------------------

                //foreach to loop through and add it to the combo box
                //-------------------------------------------------
                foreach (var aM in allModules)
                {
                    lectureModulesSelect.Add(aM.Module_ID.ToString() + " " + aM.ModuleCode);
                }
                //-------------------------------------------------
                lbModule_Course.ItemsSource = lectureModulesSelect;
            }
            //-------------------------------------------------------------
        }

        private void CkTypeUser_Unchecked(object sender, RoutedEventArgs e)
        {
            //Will show that a password needs to be enterd if a user wants the role of lecture
            //-------------------------------------------------------------
            if (ckTypeUser.IsChecked == false)
            {
                //hides the lecture password text box and lable 
                //-------------------------------------------------
                lblLecturePassword.Visibility = Visibility.Hidden;
                txbLecturePassword.Visibility = Visibility.Hidden;
                //-------------------------------------------------

                //turns label back to select course
                lblSelectCourse.Content = "Select Course :";

                lectureModulesSelect.Clear();

                //clears combo box
                lbModule_Course.ItemsSource = null;

                //linQ to display all the courses
                //-------------------------------------------------
                var allCources = from c in db.Courses
                                 select new { c.Course_ID, c.CourseCode };
                //-------------------------------------------------

                //foreach to loop through and add it to the combo box
                //-------------------------------------------------
                foreach (var aC in allCources)
                {
                    lectureModulesSelect.Add(aC.Course_ID.ToString() + " " + aC.CourseCode);
                }
                //-------------------------------------------------
                lbModule_Course.ItemsSource = lectureModulesSelect;
            }
            //-------------------------------------------------------------
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //closing the application if the user closes the application
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //hides the lecture password text box and lable 
                //-------------------------------------------------
                lblLecturePassword.Visibility = Visibility.Hidden;
                txbLecturePassword.Visibility = Visibility.Hidden;
                //-------------------------------------------------

                lectureModulesSelect.Clear();

                //clears combo box
                lbModule_Course.ItemsSource = null;

                //linQ to display all the courses
                //-------------------------------------------------
                var allCources = from c in db.Courses
                                 select new { c.Course_ID, c.CourseCode };
                //-------------------------------------------------

                //foreach to loop through and add it to the combo box
                //-------------------------------------------------
                foreach (var aC in allCources)
                {
                    lectureModulesSelect.Add(aC.Course_ID.ToString() + " " + aC.CourseCode);
                }
                //-------------------------------------------------

                lbModule_Course.ItemsSource = lectureModulesSelect;
            }
            catch (Exception)
            {
                //displays if the connection to the database is dropped
                MessageBox.Show("Lost connection to the database, check that you are connected to the internet",
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                //returns the user back to the log in screen 
                MainWindow newWindow = new MainWindow();
                newWindow.DataContext = this;
                this.Hide();
                newWindow.ShowDialog();
            }
        }
    }
}
