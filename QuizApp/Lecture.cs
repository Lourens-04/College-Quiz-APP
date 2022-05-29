using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizApp
{
    class Lecture
    {
        //calling in the model of the database
        SchoolApplicationAppEntities db = new SchoolApplicationAppEntities();
        //Creating a list of type string to store all the created test
        List<string> Createdtest = new List<string>();
        //Creating a list of type string to store all the studentsthat are linked to the lecture
        List<string> allStudentsDis = new List<string>();
        //Creating a list of type string to store all the lecture modules
        List<string> lModules = new List<string>();
        int stIDToCheck=0;

        //Method to get the lectures test created and then return the list
        //--------------------------------------------------------------------------------------------
        public List<string> TestCreated(string user)
        {
            //clears the list to make sure there is no other data in there
            Createdtest.Clear();

            //linQ statement to get the tests Title, ID, Total and module name link to the test
            //----------------------------------------------------------------------------------
            var testCreated = from u in db.Users
                              join lm in db.Lecture_Module
                              on u.User_ID equals lm.User_ID
                              join m in db.Modules
                              on lm.Module_ID equals m.Module_ID
                              join t in db.Tests
                              on m.Module_ID equals t.Module_ID
                              where u.User_ID.ToString().Equals(user)
                              select new { t.TestTitle, t.TestDesc, t.Test_ID, t.TestTotal, m.ModuleCode };
            //----------------------------------------------------------------------------------

            //foreach that will loop trough the linQ then setting it to the list
            //------------------------------------------------------------------------------------
            foreach (var x in testCreated)
            {
                string[] getStatus = x.TestDesc.Split(',');

                string status = getStatus[1];
                
                Createdtest.Add(x.Test_ID.ToString() + "     |     " + x.TestTitle + "     |     " + x.TestTotal.ToString() + "     |     " + x.ModuleCode + "     |      " + status + "\n");
            }
            //------------------------------------------------------------------------------------

            //returns the list with the created test
            return Createdtest;
        }

        //Method to get all the students that are linked to the lecture
        //--------------------------------------------------------------------------------------------
        public List<string> ViewStudents(List<string> lModules)
        {
            //clears the list to make sure there is no other data in there
            allStudentsDis.Clear();

            //linQ statement to get all the students that are linked to the lecture
            //----------------------------------------------------------------------------------
            var allStudents = from u in db.Users
                              join uc in db.User_Course
                              on u.User_ID equals uc.User_ID
                              join c in db.Courses
                              on uc.Course_ID equals c.Course_ID
                              join mc in db.Course_Module
                              on c.Course_ID equals mc.Course_ID
                              join m in db.Modules
                              on mc.Module_ID equals m.Module_ID
                              select new { u.User_ID, u.FirstName, u.LastName, m.ModuleCode, m.Module_ID };
            //----------------------------------------------------------------------------------

            //foreash that will loop all the students and then all the modules to get only the students linked to the lecture
            //---------------------------------------------------------------------------------
            foreach (var student in allStudents)
            {
                
                foreach (var module in lModules)
                {
                    if (student.User_ID != stIDToCheck || stIDToCheck == 0)
                    {
                        if (module.Equals(student.Module_ID + " " + student.ModuleCode))
                        {
                            allStudentsDis.Add("Student ID : " + student.User_ID + " \nStudent First Name : " + student.FirstName + "\nStudent Last Name : " + student.LastName + "\n");
                            stIDToCheck = student.User_ID;
                        }
                    }
                }
            }
            //---------------------------------------------------------------------------------

            //return the list of all the students
            return allStudentsDis;
        }
        //--------------------------------------------------------------------------------------------

        //method to get all the modules that are linked to the lecture
        //------------------------------------------------------------------------------
        public List<string> LectureModules(string user)
        {
            lModules.Clear();
            var allLectureModules = from u in db.Users
                                    join lm in db.Lecture_Module
                                    on u.User_ID equals lm.User_ID
                                    join m in db.Modules
                                    on lm.Module_ID equals m.Module_ID
                                    where u.User_ID.ToString().Equals(user)
                                    select new { m.Module_ID, m.ModuleCode };

            //loops trough the linQ then adds it to list
            //-----------------------------------------------
            foreach (var alm in allLectureModules)
            {
                lModules.Add(alm.Module_ID + " " + alm.ModuleCode);
            }
            //-----------------------------------------------

            //returns the list
            return lModules;
        }
        //------------------------------------------------------------------------------

        //Method to save the test that the lecture made to the database
        //---------------------------------------------------------------------------------
        public void SaveTest(string testTitle, string testDesc, int moduleID, Dictionary<int, CreatedTestQuestions> questions, int testTotal)
        {
            //calling the test class from the database
            Test test = new Test();
            //calling the question class from the database
            Question question = new Question();
            //declaring ID's for the question and test
            //---------------------------
            
            //declaring a variable to set the test
            var addSettest = db.Set<Test>();
            //Adds the sets to the test in the model
            addSettest.Add(new Test {TestTitle = testTitle, TestDesc = (testDesc + ",unpublish"), Module_ID = moduleID, TestTotal = testTotal });

            //saves the changes made in the model
            db.SaveChanges();
            var ss = db.Tests.ToList().LastOrDefault();
            int tId = ss.Test_ID;

            //declaring a variable to set the question
            var addSetques = db.Set<Question>();
            //foreach to loop through to add all the questions to the questions table in the model
            foreach (var q in questions)
            {
                //Adds the sets to the question in the model
                addSetques.Add(new Question {TestQuestion = q.Value.Question, QA = q.Value.QA, QB = q.Value.QB, QC = q.Value.QC, QAns = q.Value.QAns, QMark = q.Value.QMark, Test_ID = tId });
            }

            //saves the changes made in the model
            db.SaveChanges();
        }
        //---------------------------------------------------------------------------------

        //method to that will be used to return result based on the user search for view student
        //-----------------------------------------------------------------------------------------
        public List<string> ViewStudents(List<string> lModules, string userS, string search)
        {
            //clears list
            allStudentsDis.Clear();

            //if statement to see if it is equel to All students
            if (search == "All Students")
            {
                //linQ statement to get all the students that are linked to the lecture
                //----------------------------------------------------------------------------------
                var allStudents = from u in db.Users
                                  join uc in db.User_Course
                                  on u.User_ID equals uc.User_ID
                                  join c in db.Courses
                                  on uc.Course_ID equals c.Course_ID
                                  join mc in db.Course_Module
                                  on c.Course_ID equals mc.Course_ID
                                  join m in db.Modules
                                  on mc.Module_ID equals m.Module_ID
                                  select new { u.User_ID, u.FirstName, u.LastName, m.ModuleCode, m.Module_ID };
                //----------------------------------------------------------------------------------

                //foreach that will loop trough the linQ then setting it to the list
                //------------------------------------------------------------------------------------
                foreach (var student in allStudents)
                {
                    foreach (var module in lModules)
                    {
                        if (module.Equals(student.Module_ID + " " + student.ModuleCode))
                        {
                            allStudentsDis.Add("Student ID : " + student.User_ID + "\nStudent First Name : " + student.FirstName + "\nStudent Last Name : " + student.LastName + "\n");
                        }
                    }
                }
                //-----------------------------------------------------------------------------------
            }
            //if statement to see if search is equel to student ID
            if (search == "Student ID")
            {
                //LinQ to get all the students based on the student first name
                //--------------------------------------------------------------------
                var allStudents = from u in db.Users
                                  join uc in db.User_Course
                                  on u.User_ID equals uc.User_ID
                                  join c in db.Courses
                                  on uc.Course_ID equals c.Course_ID
                                  join mc in db.Course_Module
                                  on c.Course_ID equals mc.Course_ID
                                  join m in db.Modules
                                  on mc.Module_ID equals m.Module_ID
                                  where u.User_ID.ToString().Equals(userS)
                                  select new { u.User_ID, u.FirstName, u.LastName, m.ModuleCode, m.Module_ID };
                //--------------------------------------------------------------------

                //foreach loop to get all the student in the link and add it to the list
                //-------------------------------------------------------------------
                foreach (var student in allStudents)
                {
                    foreach (var module in lModules)
                    {
                        if (module.Equals(student.Module_ID + " " + student.ModuleCode))
                        {
                            allStudentsDis.Add("Student ID : " + student.User_ID + "\nStudent First Name : " + student.FirstName + "\nStudent Last Name : " + student.LastName + "\n");
                        }
                    }
                }
                //-------------------------------------------------------------------
            }
            //if statement to see is search equel to student first name
            if (search == "Student First Name")
            {
                //linQ staement to get all the students with that first name
                //--------------------------------------------------------------------
                var allStudents = from u in db.Users
                                  join uc in db.User_Course
                                  on u.User_ID equals uc.User_ID
                                  join c in db.Courses
                                  on uc.Course_ID equals c.Course_ID
                                  join mc in db.Course_Module
                                  on c.Course_ID equals mc.Course_ID
                                  join m in db.Modules
                                  on mc.Module_ID equals m.Module_ID
                                  where u.FirstName.ToString().Equals(userS)
                                  select new { u.User_ID, u.FirstName, u.LastName, m.ModuleCode, m.Module_ID };
                //--------------------------------------------------------------------

                //foreach to loop through all the students and then add them to the list to be returned
                //--------------------------------------------------------------------
                foreach (var student in allStudents)
                {
                    foreach (var module in lModules)
                    {
                        if (module.Equals(student.Module_ID + " " + student.ModuleCode))
                        {
                            allStudentsDis.Add("Student ID : " + student.User_ID + "\nStudent First Name : " + student.FirstName + "\nStudent Last Name : " + student.LastName + "\n");
                        }
                    }
                }
                //--------------------------------------------------------------------
            }
            //if statement to see if it equels to student last name
            if (search == "Student Last Name")
            {
                //linQ to get all the student based on their last name
                //--------------------------------------------------------------------
                var allStudents = from u in db.Users
                                  join uc in db.User_Course
                                  on u.User_ID equals uc.User_ID
                                  join c in db.Courses
                                  on uc.Course_ID equals c.Course_ID
                                  join mc in db.Course_Module
                                  on c.Course_ID equals mc.Course_ID
                                  join m in db.Modules
                                  on mc.Module_ID equals m.Module_ID
                                  where u.LastName.ToString().Equals(userS)
                                  select new { u.User_ID, u.FirstName, u.LastName, m.ModuleCode, m.Module_ID };
                //--------------------------------------------------------------------

                //foreach to loop through all the students and then add them to the list to be returned
                //----------------------------------------------------------------------------
                foreach (var student in allStudents)
                {
                    foreach (var module in lModules)
                    {
                        if (module.Equals(student.Module_ID + " " + student.ModuleCode))
                        {
                            allStudentsDis.Add("Student ID : " + student.User_ID + "\nStudent First Name : " + student.FirstName + "\nStudent Last Name : " + student.LastName + "\n");
                        }
                    }
                }
                //-----------------------------------------------------------------------------
            }
            return allStudentsDis;
        }
        //-----------------------------------------------------------------------------------------

        //method to that will be used to return result based on the user search test created 
        //-----------------------------------------------------------------------------------------
        public List<string> TestCreated(string user, string test, string search)
        {
            //clears the list
            Createdtest.Clear();
            //if statement to see if search is equel to All test
            if ("All Tests" == search)
            {
                //linQ to get all the test created by a user
                //method to that will be used to return result based on the user search 
                //-----------------------------------------------------
                var testCreated = from u in db.Users
                                  join lm in db.Lecture_Module
                                  on u.User_ID equals lm.User_ID
                                  join m in db.Modules
                                  on lm.Module_ID equals m.Module_ID
                                  join t in db.Tests
                                  on m.Module_ID equals t.Module_ID
                                  where u.User_ID.ToString().Equals(user)
                                  select new { t.TestTitle, t.TestDesc, t.Test_ID, t.TestTotal, m.ModuleCode };
                //-----------------------------------------------------

                //foreach to loop through all the tests and then add them to the list to be returned
                //-------------------------------------------------------------------------------
                foreach (var x in testCreated)
                {
                    string[] status = x.TestDesc.Split(',');
                    Createdtest.Add(x.Test_ID.ToString() + "     |     " + x.TestTitle + "     |     " + x.TestTotal.ToString() + "     |     " + x.ModuleCode + "     |      " + status[1] + "\n");
                }
                //-------------------------------------------------------------------------------
            }
            //if statement that will check if search equels test name
            if ("Test Name" == search)
            {
                //linq staement to get all the test with the test name that was created by a user 
                //-----------------------------------------------------------------------
                var testCreated = from u in db.Users
                                  join lm in db.Lecture_Module
                                  on u.User_ID equals lm.User_ID
                                  join m in db.Modules
                                  on lm.Module_ID equals m.Module_ID
                                  join t in db.Tests
                                  on m.Module_ID equals t.Module_ID
                                  where u.User_ID.ToString().Equals(user)
                                  where t.TestTitle.Equals(test)
                                  select new { t.TestTitle, t.TestDesc, t.Test_ID, t.TestTotal, m.ModuleCode };
                //-----------------------------------------------------------------------

                //foreach to loop through all the tests and then add them to the list to be returned
                //---------------------------------------------------------------
                foreach (var x in testCreated)
                {
                    string[] status = x.TestDesc.Split(',');
                    Createdtest.Add(x.Test_ID.ToString() + "     |     " + x.TestTitle + "     |     " + x.TestTotal.ToString() + "     |     " + x.ModuleCode + "     |      " + status[1] + "\n");
                }
                //---------------------------------------------------------------
            }
            //if statement to see if the search equels module
            if ("Module" == search)
            {
                //linQ it get all the tests that were created by a user by module that the user typed
                //----------------------------------------------------------------------
                var testCreated = from u in db.Users
                                  join lm in db.Lecture_Module
                                  on u.User_ID equals lm.User_ID
                                  join m in db.Modules
                                  on lm.Module_ID equals m.Module_ID
                                  join t in db.Tests
                                  on m.Module_ID equals t.Module_ID
                                  where u.User_ID.ToString().Equals(user)
                                  where m.ModuleCode.Equals(test)
                                  select new { t.TestTitle, t.TestDesc, t.Test_ID, t.TestTotal, m.ModuleCode };
                //----------------------------------------------------------------------

                //foreach to loop through all the tests and then add them to the list to be returned
                //-------------------------------------------------------------
                foreach (var x in testCreated)
                {
                    string[] status = x.TestDesc.Split(',');
                    Createdtest.Add(x.Test_ID.ToString() + "     |     " + x.TestTitle + "     |     " + x.TestTotal.ToString() + "     |     " + x.ModuleCode + "     |      " + status[1] + "\n");
                }
                //--------------------------------------------------------------
            }

            //returns the list
            return Createdtest;
        }

        //getting the test details if the lecture wants to edit it
        public string TestDetails(string testId)
        {
            //int to get a test id
            int testIdToGetDetails = Convert.ToInt32(testId);
            //Getting the test that a lecture selected
            var detailsOfTest = db.Tests.Single(x => x.Test_ID == testIdToGetDetails);
            //splitting the test description and visibility
            string[] desc = detailsOfTest.TestDesc.Split(',');
            //returning the thes details
            return detailsOfTest.TestTitle + "&" + desc[0] + "&" + detailsOfTest.Module_ID;
        }

        //method to change a test visability for students
        public void Publish(string testId, string publish)
        {
            //int to get a test id
            int testIdToPublish = Convert.ToInt32(testId);
            //Getting the test that a lecture selected
            var publishTest = db.Tests.Single(x => x.Test_ID == testIdToPublish);
            //splitting the test description and visibility
            string[] descAndPublish = publishTest.TestDesc.Split(',');
            //string for the discription
            string desc;
            //if to string publish has the value publish or unpublish 
            if (publish.Equals("publish"))
            {
                //setting the test to hidden
                desc = descAndPublish[0] + ",unpublish";
            }
            else
            {
                //setting the test to visible
                desc = descAndPublish[0] + ",publish";
            }
            
            //changing the description in the database
            Test test = db.Tests.Single(x => x.Test_ID == testIdToPublish);
            test.TestDesc = desc;
            //saving the changes
            db.SaveChanges();
        }

        //edit method to change the test details, and questions if a lecture wants to change something
        public void Edit(string testTitle, string testDesc, int moduleID, Dictionary<int, CreatedTestQuestions> questions, int testTotal, string testID)
        {
            //int to get a test id
            int testIDInInt = Convert.ToInt32(testID);

            //changing the test details to the new values a lecture enterd
            //-----------------------------------------------------------------
            Test updateTest = db.Tests.Single(x => x.Test_ID == testIDInInt);
            updateTest.TestTitle = testTitle;
            updateTest.TestDesc = testDesc + ",unpublish";
            updateTest.Module_ID = moduleID;
            updateTest.TestTotal = testTotal;
            //-----------------------------------------------------------------

            //declaring a list of type int to store all the questions id's
            List<int> questionIDs = new List<int>();

            //getting all the questions of the test that is being edited
            var updateQuestions = db.Questions.Where(x => x.Test_ID == testIDInInt);

            //foreeach to set all the questions id in the questionIDs list
            //-------------------------------------------------------
            foreach (var item in updateQuestions)
            {
                questionIDs.Add(item.Question_ID); 
            }
            //-------------------------------------------------------

            //declaring an i of type int that will be used to get the ids in the questionIDs list
            int i = 0;
            //changing the values in the database to the new values the lecture enterd
            //---------------------------------------------------------------------
            foreach (var updatedQuedtions in questions)
            {
                int id = questionIDs[i];
                Question questionsInDB = db.Questions.Single(x => x.Question_ID == id);
                questionsInDB.TestQuestion = updatedQuedtions.Value.Question;
                questionsInDB.QA = updatedQuedtions.Value.QA;
                questionsInDB.QB = updatedQuedtions.Value.QB;
                questionsInDB.QC = updatedQuedtions.Value.QC;
                questionsInDB.QAns = updatedQuedtions.Value.QAns;
                questionsInDB.QMark = updatedQuedtions.Value.QMark;
                i++;
            }
            //---------------------------------------------------------------------

            //saves the changes made in the model
            db.SaveChanges();
        }
    }
}
