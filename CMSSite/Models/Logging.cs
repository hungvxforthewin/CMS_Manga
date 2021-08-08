using System.Collections.Generic;

namespace CRMSite.Models
{
    internal static class Logging
    {
        public static Dictionary<ActionResultValue, string> resultList = new Dictionary<ActionResultValue, string>
        {
            { ActionResultValue.InvalidInput, "Input invalid data" },
            { ActionResultValue.CreateSuccess, "Create new item successfully" },
            { ActionResultValue.CreateFailed, "Create new item failed" },
            { ActionResultValue.UpdateSuccess, "Update an item successfully" },
            { ActionResultValue.UpdateFailed, "Update an item failed" },
            { ActionResultValue.UploadFailed, "Upload a file failed" },
            { ActionResultValue.DeleteFailed, "Delete an item failed" },
            { ActionResultValue.DeleteSuccess, "Delete an item successfully" },
            { ActionResultValue.DeleteAllSuccess, "Delete all items successfully" },
            { ActionResultValue.DeleteAllFailed, "Delete all items failed" },
            { ActionResultValue.ImportFailed, "Import the data in excel file failed" },
            { ActionResultValue.ImportSuccess, "Import the data in excel file successfully" },
            { ActionResultValue.ViewSuccess, "View the item information successfully" },
            { ActionResultValue.ViewFailed, "View the item information failed" },
            { ActionResultValue.LoginFailed, "Login the system failed" },
            { ActionResultValue.LoginSuccess, "LOGIN THE SYSTEM SUCCESSFULLY" },
            { ActionResultValue.ThrowException, "Throw an error when the system attempts to connect to server" },
            { ActionResultValue.NotAllowAccess, "UNAUTHORIZED ACCESS" },
            { ActionResultValue.Logout, "LOGOUT SYSTEM SUCCESSFULLY" },
            { ActionResultValue.AccessSuccess, "Access the page successfully" },
            { ActionResultValue.GetInfoSuccess, "Get information successfully" },
            { ActionResultValue.NotFoundData, "Not found data" },
            { ActionResultValue.UploadSuccess, "Upload the file(s) successfully" },
            { ActionResultValue.SignFailed, "Sign a contract failed" },
            { ActionResultValue.SignSuccess, "Sign a contract successfully" },
            { ActionResultValue.PrintFailed, "Print a contract failed" },
            { ActionResultValue.PrintSuccess, "Print a contract successfully" },
            { ActionResultValue.ConfirmOTPSuccess, "Confirm the otp successfully" },
            { ActionResultValue.SentOTPSuccess, "Send an otp to a user successfully" },
            { ActionResultValue.SentOTPFailed, "Send an otp to a user failed" },
        };
    }

    public enum ActionType
    {
        GetInfo = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
        DeleteAll = 4,
        ViewInfo = 5,
        Import = 6,
        Export = 7,
        Login = 8,
        Logout = 9,
        Upload = 10,
        DoSomeThing = -1,
        Sign = 11,
        SendOTP = 12,
        ConfirmOTP = 13,
        Print = 14,
    }

    public enum ActionResultValue
    {
        InvalidInput = 0,
        CreateSuccess = 1,
        CreateFailed = 2,
        UpdateSuccess = 3,
        UpdateFailed = 4,
        DeleteSuccess = 5,
        DeleteFailed = 6,
        UploadFailed = 7,
        ViewSuccess = 8,
        ViewFailed = 9,
        DeleteAllSuccess = 10,
        DeleteAllFailed = 11,
        ImportSuccess = 12,
        ImportFailed = 13,
        ThrowException = 14,
        LoginSuccess = 15,
        LoginFailed = 16,
        NotAllowAccess = 17,
        Logout = 18,
        AccessSuccess = 19,
        GetInfoSuccess = 20,
        NotFoundData = 21, 
        UploadSuccess = 22,
        SignSuccess = 23,
        SignFailed = 24,
        PrintFailed = 25,
        PrintSuccess = 26,
        ConfirmOTPSuccess = 27,
        SentOTPSuccess = 28,
        SentOTPFailed = 29,
    }

    public class LogModel
    {
        public string Username { get; set; }

        public string Role { get; set; }

        public string AccessTarget { get; set; }

        public ActionType Action { get; set; } = ActionType.DoSomeThing;

        public string ItemName { get; set; }

        public string Data { get; set; }

        public ActionResultValue Result { get; set; } = ActionResultValue.ThrowException;

        public string Message { get; set; }

        //public DateTime ActionTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            string action = null;
            switch (Action)
            {
                case ActionType.Create:
                    action = "create a(an) new";
                    break;

                case ActionType.Delete:
                    action = "delete a(an)";
                    break;

                case ActionType.DeleteAll:
                    action = "delete all";
                    break;

                case ActionType.Export:
                    action = "export the list of";
                    break;

                case ActionType.GetInfo:
                    action = "get information about";
                    break;

                case ActionType.Import:
                    action = "import the list of";
                    break;

                case ActionType.Update:
                    action = "update the information of a(an)";
                    break;

                case ActionType.ViewInfo:
                    action = "view the detail information of a(an)";
                    break;

                case ActionType.Login:
                    action = "login the system";
                    break;

                case ActionType.Logout:
                    action = "logout the system";
                    break;

                case ActionType.Sign:
                    action = "sign a";
                    break;

                case ActionType.SendOTP:
                    action = "send an otp to user";
                    break;

                case ActionType.ConfirmOTP:
                    action = "confirm an otp what user is sent";
                    break;

                case ActionType.Print:
                    action = "print a(an)";
                    break;

                default:
                    action = "do something";
                    break;

            }

            string template = @$"{Username}[{Role}] has accessed the ""{AccessTarget}"" link to {action} {ItemName} with
    data({Data}(if any)),
    thus action result: {Logging.resultList.GetValueOrDefault(Result)},
    message: [{Message}]";
            return template;
        }

   //     public string ToString(string action, string result)
   //     {
   //         string template = @$"{Username}[{Role}] has accessed the ""{AccessTarget}"" link to { action },
   //thus action result: {result}";
   //         return template;
   //     }

        public string ToString(string message)
        {
            string template = $"{message}";
            return template;
        }
    }
}
