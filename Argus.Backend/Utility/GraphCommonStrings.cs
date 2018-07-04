
namespace Argus.Backend.Utility
{
    public static class GraphCommonStrings
    {
        //Relationship
        //Procedure->ProcedureStep, UserGroup->User
        public const string Include = "Include";
        //ProcedureStep->Procedure, User->UserGroup
        public const string BelongTo = "BelongTo";

        //ProcedureStep->ProcedureStep
        public const string Next = "Next";
        public const string Previous = "Previous";

        //UserGroup->ProcedureStep
        public const string Incharge = "Incharge";
        //ProcedureStep->UserGroup
        public const string HandleBy = "HandleBy";

        //User->UserGroup
        public const string Lead = "Lead";
        //UserGroup->User
        public const string LeadBy = "LeadBy";

        //User->Role
        public const string As = "As";

        //User->Task
        public const string Create = "Create";
        //Task->User
        public const string CreateBy = "CreateBy";

        //User->Task
        public const string Assigned = "Assigned";
        //Task->User
        public const string AssignTo = "AssignTo";

        //Task->ProcedureStep
        public const string CurrentStep = "CurrentStep";


    }
}