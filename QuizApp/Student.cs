using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizApp
{
    class Student
    {
        //calling in the database model
        SchoolApplicationAppEntities db = new SchoolApplicationAppEntities();
        //creating a list of type string  to store all students tests
        List<string> studentTest = new List<string>();
        //creating a list of type string  to store all students tests results
        List<string> studentResult = new List<string>();
        //creating a list of type string  to store students memo
        List<string> testMemo = new List<string>();
        //creating a list where all the questions will be stored to display when a user takes a test
        public List<CreatedTestLayout> questions = new List<CreatedTestLayout>();

        //method to return a list for all the students tests
        //-------------------------------------------------------------------
        public List<string> StudentTests(string user)
        {
            //clears the list
            studentTest.Clear();

            //linQ to get all the student tests
            //----------------------------------------------------------
            var studentTests = from u in db.Users
                               join uc in db.User_Course
                               on u.User_ID equals uc.User_ID
                               join c in db.Courses
                               on uc.Course_ID equals c.Course_ID
                               join cm in db.Course_Module
                               on c.Course_ID equals cm.Course_ID
                               join m in db.Modules
                               on cm.Module_ID equals m.Module_ID
                               join t in db.Tests
                               on m.Module_ID equals t.Module_ID
                               where u.User_ID.ToString().Equals(user)
                               select new { t.Test_ID, t.TestTitle, t.TestDesc, t.TestTotal, c.CourseCode, m.ModuleCode };
            //--------------------------------------------------------------

            //checker to see if the test has not been taken yet
            Boolean check = false;

            //foreach to go through all the results of the linQ statement
            foreach (var st in studentTests)
            {
                //foreach that will get all the result tests
                foreach (var str in StudentResults(user))
                {
                    //if statement to see if the test id equel to the results test id
                    if (st.Test_ID.ToString() != str.Split(' ')[0])
                    {
                        //turns checker to false
                        check = false;
                    }
                    else
                    {
                        //turns checker to true
                        check = true;
                        //stops the foreach loop
                        break;
                    }
                }
                //if statement to see if checker is false
                if (check == false)
                {
                    string[] descAndPublish = st.TestDesc.Split(',');

                    if (descAndPublish[1].Equals("publish"))
                    {
                        //adds the details to the lists
                        studentTest.Add(st.Test_ID.ToString() + " " + "Test Title : " + st.TestTitle + "\n" + "Test Description : " + descAndPublish[0] + "\n" + "Test Total : " + st.TestTotal.ToString() + " | Course Name : " + st.CourseCode + " | Module Name : " + st.ModuleCode + "\n");
                    }
                }
                else
                {
                    //turns checker to false
                    check = false;
                }
            }

            //returns list
            return studentTest;
        }
        //-------------------------------------------------------------------

        //method to return all the test that the user have taken
        //---------------------------------------------------------------------------------------
        public List<string> StudentResults(string user)
        {
            //clears the list
            studentResult.Clear();

            //linQ to get all the student tests that the user have taken
            //----------------------------------------------------------
            var studentResults = from u in db.Users
                               join r in db.Results
                               on u.User_ID equals r.User_ID
                               join t in db.Tests
                               on r.Test_ID equals t.Test_ID
                               join uc in db.User_Course
                               on u.User_ID equals uc.User_ID
                               join c in db.Courses
                               on uc.Course_ID equals c.Course_ID
                               join cm in db.Course_Module
                               on c.Course_ID equals cm.Course_ID
                               join m in db.Modules
                               on cm.Module_ID equals m.Module_ID
                               where r.User_ID.ToString().Equals(user)
                               where m.Module_ID.Equals(t.Module_ID)
                               select new { t.Test_ID, t.TestTitle, t.TestDesc, t.TestTotal, c.CourseCode, m.ModuleCode, r.UserMark };
            //----------------------------------------------------------

            //foreach to go through all the results of the linQ statement
            foreach (var str in studentResults)
            {
                string[] desc = str.TestDesc.Split(',');
                //adds the details to the lists
                studentResult.Add(str.Test_ID.ToString() + " " + "Test Title : " + str.TestTitle + "\n" + "Test Description : " + desc[0] + "\n" + "Student Mark : " + str.UserMark.ToString() + " | Course Name : " + str.CourseCode + " | Module Name : " + str.ModuleCode + "\n");
            }

            return studentResult;
        }
        //---------------------------------------------------------------------------------------

       //method that will return a list to diplay a tests
       //-----------------------------------------------------------------------
        public List<CreatedTestLayout> DisplayTest(string test)
        {
            //foreach that will loop through all the test
            foreach (var t in db.Tests)
            {
                //if statement to see if test id equels test id the user chose
                if (t.Test_ID.ToString().Equals(test))
                {
                    //foreach to loop trough all the questions
                    foreach (var q in db.Questions)
                    {
                        //if statement to see if test id equels test id the user chose
                        if (q.Test_ID.Equals(t.Test_ID))
                        {
                            //adds the details to the list
                            questions.Add(new CreatedTestLayout(q.TestQuestion.ToString(), q.QA.ToString(), q.QB.ToString(), q.QC.ToString(), q.QAns.ToString(), q.QMark.ToString(), t.TestTitle.ToString(), t.TestDesc.ToString(), t.TestTotal.ToString()));
                        }
                    }
                }
            }
            //returns the list
            return questions;
        }
        //-----------------------------------------------------------------------

        //method to save a test taken by a student
        //-----------------------------------------------------
        public void SaveStudentTest(int testID, int userID, string userAns, int userMark)
        {
            //getting the result object in the dtabase model
            Result uResult = new Result();
            //sets the results table in the model to add data to the database
            var addUserResult = db.Set<Result>();
            //adds the user to the results table in the database model
            addUserResult.Add(new Result {Test_ID = testID, User_ID = userID, UserAns = userAns, UserMark = userMark });
            //saves changes made in the model
            db.SaveChanges();
        }
        //-----------------------------------------------------

        //method to return a list to display results to the student
        //----------------------------------------------------------------------------------------------------
        public List<string> Memo(string testID, string userID)
        {
            //linQ to get all the details that will be needed for a memo
            //-------------------------------------------------------------------
            var memo = from u in db.Users
                                 join r in db.Results
                                 on u.User_ID equals r.User_ID
                                 join t in db.Tests
                                 on r.Test_ID equals t.Test_ID
                                 join uc in db.User_Course
                                 on u.User_ID equals uc.User_ID
                                 join c in db.Courses
                                 on uc.Course_ID equals c.Course_ID
                                 join cm in db.Course_Module
                                 on c.Course_ID equals cm.Course_ID
                                 join m in db.Modules
                                 on cm.Module_ID equals m.Module_ID
                                 where u.User_ID.ToString().Equals(userID)
                                 where t.Test_ID.ToString().Equals(testID)
                                 where m.Module_ID.Equals(t.Module_ID)
                                 select new { t.Test_ID, t.TestTitle, t.TestDesc, t.TestTotal, c.CourseCode, m.ModuleCode,      r.UserMark, r.UserAns};
            //-------------------------------------------------------------------

            //declaring variables for caculating marks and to get certian data
            //-------------------------
            int i = 0;
            int mark = 0;
            //-------------------------

            //foreach to loop through the linQ statement
            foreach (var m in memo)
            {
                string[] desc = m.TestDesc.Split(',');
                //adds the test table details first
                testMemo.Add(m.TestTitle  + "        \n" + desc[0] + "\n" + m.CourseCode + "        " + m.ModuleCode + "        Score is " + m.UserMark + "/" + m.TestTotal + "\n------------------------------------------------------\n");
                //foreach loop that will loop trough all the questions
                foreach (var q in DisplayTest(testID))
                {
                    //splits the user ansers to give marks and for display
                    string[] ans = m.UserAns.Split(' ');

                    //if statement to see if the user answer is the same as the question answer
                    if (q.QAns == ans[i])
                    {
                        //gives the mark if the user is correct
                        mark = Convert.ToInt32(q.QMark);
                    }
                    else
                    {
                        //gives the user 0 if the user did not get it correct
                        mark = 0;
                    }
                    
                    //adds the question table details as well as user marks
                    testMemo.Add("Q." + (i+1) + ") " + q.Question + "\nA) " + q.QA + "        B) " + q.QB + "        C) " + q.QC + "\n" + "Question Answer is : " + q.QAns + "\nYour Answer was : " + ans[i] + "\nYour mark out of " + q.QMark + " is " + mark + "\n------------------------------------------------------\n");
                    //increments the variable i
                    i++;
                }
                break;
            }

            //returns list
            return testMemo;
        }
        //----------------------------------------------------------------------------------------------------
    }
}
