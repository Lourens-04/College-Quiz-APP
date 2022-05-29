using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification
{
    public class VerifyLogIn
    {
        string userRole;

        public string CheckUser(string username, string password, List<string> users)
        {
            foreach (var verify in users)
            {
                string userID;
                string userPas;
                string userR;

                string[] allUsers = verify.Split(',');

                userID = allUsers[0];
                userPas = allUsers[1];
                userR = allUsers[2];

                if (userID == username && userPas == password)
                {
                    userRole = userR;
                }
            }

            return userRole;
        }
    }
}
