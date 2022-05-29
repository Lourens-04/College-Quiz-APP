using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp
{
    class LectureModuleSignUp
    {
        string moduleID;
        string moduleCode;
        Boolean moduleStatus;

        public LectureModuleSignUp(string moduleID, string moduleCode, bool moduleStatus)
        {
            this.moduleID = moduleID;
            this.moduleCode = moduleCode;
            this.moduleStatus = moduleStatus;
        }

        public string ModuleID { get => moduleID; set => moduleID = value; }
        public string ModuleCode { get => moduleCode; set => moduleCode = value; }
        public bool ModuleStatus { get => moduleStatus; set => moduleStatus = value; }
    }
}
