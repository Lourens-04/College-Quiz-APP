using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp
{
    class Search
    {
        //calling the database model
        SchoolApplicationAppEntities db = new SchoolApplicationAppEntities();

        //creating a list of type sring to store a user search
        List<string> search = new List<string>();

        //method to search by test title
        //-----------------------------------------------------------------------------------------------
        public List<string> SearchTestTitle(string user, string test)
        {
            //clear the list
            search.Clear();

            //linQ to get all the test that the user is looking for
            //-----------------------------------------------------------------------------------------------
            var sTestTitle   = from u in db.Users
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
            //-----------------------------------------------------------------------------------------------

            //foreach to loop through the linQ and then sets it into a list
            //----------------------------------------------------------
            foreach (var st in sTestTitle)
            {
                string[] desc = st.TestDesc.Split(',');
                if (st.TestTitle == test)
                {
                    search.Add(st.Test_ID.ToString() + " " + "Test Title : " + st.TestTitle + "\n" + "Test Description : " + desc[0] + "\n" + "Test Total : " + st.TestTotal.ToString() + " | Course Name : " + st.CourseCode + " | Module Name : " + st.ModuleCode + "\n");
                }
            }
            //----------------------------------------------------------

            //returns the list
            return search;
        }
        //-----------------------------------------------------------------------------------------------

        public List<string> SearchModule(string user, string test)
        {
            //clear lists
            search.Clear();

            //linQ to get all the modules based on the user search
            //---------------------------------------------------------------------------------------------
            var sModule = from u in db.Users
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
            //---------------------------------------------------------------------------------------------

            //foreach to loop through the linQ and then sets it into a list
            //------------------------------------------------------------------
            foreach (var st in sModule)
            {
                string[] desc = st.TestDesc.Split(',');
                if (st.ModuleCode == test)
                {
                    search.Add(st.Test_ID.ToString() + " " + "Test Title : " + st.TestTitle + "\n" + "Test Description : " + desc[0] + "\n" + "Test Total : " + st.TestTotal.ToString() + " | Course Name : " + st.CourseCode + " | Module Name : " + st.ModuleCode + "\n");
                }
            }
            //------------------------------------------------------------------

            //returns the list
            return search;
        }
    }
}
