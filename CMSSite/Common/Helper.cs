using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static CRMSite.Common.SiteConst;

namespace CRMSite.Common
{
    public static class Helper
    {
        private static string _imageLocation;
        private static string _fileLocation;
        private const string DEFAULT_SUBJECT = "Nội dung email: ";
        private static readonly List<string> ImageTypeList = new List<string> { ".jpg", ".png", ".jpeg", ".svg", ".gif" };

        #region UploadFile
        public static UploadStatus UploadFile(IFormFile file, string containerUrl, out string filename)
        {
            filename = null;
            // validate selected files
            if (file == null)
                return UploadStatus.NOT_SELECT_FILE;

            if (file.Length == 0)
                return UploadStatus.NO_CONTENT;

            if (file.Length >= 2097152)
                return UploadStatus.TOO_BIG;    

            // create file path
            string extension = Path.GetExtension(file.FileName);
            string fileName = file.Name + SiteConst.Underscore + Guid.NewGuid().ToString().Substring(0,8) + extension;
            string filePath = null;
            
            //check file format(do not)
            if (ImageTypeList.Find(x => x == extension.ToLower()) == null)
            {
                return UploadStatus.INVALID_FORMAT;
            }

            DateTime now = DateTime.Now;
            // generate folders by current date
            string subFolder = Path.Combine(now.Year.ToString(), now.Month.ToString(), now.Day.ToString());
            // contain file url
            filePath = Path.Combine(containerUrl, subFolder);
            // check existed directory
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            //filePath += SiteConst.DoubleBackSlash + fileName;
            // full url of file
            filePath = Path.Combine(filePath, fileName);

            // save file(s)
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream).Wait();
                    filename = Path.Combine(subFolder, fileName);
                }
            }
            catch (Exception ex)
            {
                return UploadStatus.FAILURE;
            }

            return UploadStatus.SUCCESS;
        }
        #endregion

        #region UploadFiles
        public static ErrorCodeEnum UploadManyFiles(List<IFormFile> files, string containerUrl,
            string fileFormat, out string filename)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                .Build();

            _imageLocation = configuration["Configurations:ImageAddress"];
            _fileLocation = configuration["Configurations:FileAddress"];

            filename = null;
            // validate selected files
            if (files == null)
                return ErrorCodeEnum.NOT_SELECT_FILE;

            if (files.Where(x => x.Length == 0).FirstOrDefault() != null)
                return ErrorCodeEnum.FILE_NO_CONTENT;

            // check existed directory
            string filePath = null;
            if (fileFormat == SiteConst.FileFormat.Image)
            {
                filePath = _imageLocation + containerUrl;
            }
            else
            {
                filePath = _fileLocation + containerUrl;
            }
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            List<string> uploadedFile = new List<string>();
            foreach (var item in files)
            {
                // create file path
                string extension = "." + item.FileName.Split('.')[1];
                string fileName = Guid.NewGuid() + extension;
                string filePathValue = filePath + "\\" + fileName;

                // save file(s)
                try
                {
                    using (var stream = new FileStream(filePathValue, FileMode.Create))
                    {
                        item.CopyToAsync(stream).Wait();
                        uploadedFile.Add(containerUrl + "\\" + fileName);
                    }
                }
                catch (Exception ex)
                {
                    foreach (var file in uploadedFile)
                    {
                        DeleteFile(file, fileFormat);
                    }
                    return ErrorCodeEnum.UPLOAD_FAILURE;
                }
            }
            filename = string.Join(SiteConst.SeparateChar, uploadedFile);
            return ErrorCodeEnum.UPLOAD_SUCCESS;
        }
        #endregion

        #region DeleteFile
        /// <summary>
        /// Delete uploaded file(s)
        /// </summary>
        /// <param name="filename">file name</param>
        /// <param name="fileType">type of the file</param>
        /// <param name="fileFormat">format(excel,word,...)</param>
        public static void DeleteFile(string filename, string fileFormat)
        {
            string filePath = null;
            if (fileFormat == SiteConst.FileFormat.Image)
            {
                filePath = _imageLocation + filename;
            }
            else
            {
                filePath = _fileLocation + filename;
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        #endregion

        #region SendEmail
        /// <summary>
        /// Send a mail to email address
        /// </summary>
        /// <param name="email">email address</param>
        /// <param name="msg">message</param>
        /// <param name="subject">subjet of mail</param>
        /// <returns>true|false</returns>
        public static async Task<bool> SendEmailAsync(string email, string msg, string subject = "")
        {
            // Initialization.  
            bool isSend = false;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                .Build();

            //Get config
            string emailAddress = configuration["Configurations:ServerEmail"];
            string passwordMail = configuration["Configurations:PasswordMail"];

            string mailDisplayName = configuration["Configurations:MailDisplayName"];
            string host = configuration["Configurations:ServerEmailHost"];
            string port = configuration["Configurations:ServerEmailPort"];
            string ssl = configuration["Configurations:ServerEmailUseSSL"];

            // Initialization. 
            var body = msg;
            var message = new MailMessage();

            // Settings.  
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress(emailAddress, mailDisplayName);
            message.Subject = !string.IsNullOrEmpty(subject) ? subject : DEFAULT_SUBJECT;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            message.Body = body;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.IsBodyHtml = true;

            try
            {
                using (var smtp = new SmtpClient())
                {
                    // Settings.  
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(emailAddress, passwordMail);
                    smtp.Host = host;
                    smtp.Port = Convert.ToInt32(port);
                    smtp.EnableSsl = Convert.ToBoolean(ssl);

                    // Sending  
                    await smtp.SendMailAsync(message);

                    // Settings.  
                    isSend = true;
                }
            }
            catch
            {
                // Info  
                //throw ex;
                isSend = false;
            }
            // info.  
            return isSend;
        }
        #endregion

        #region GetErros
        public static List<string> GetErrors(ModelStateDictionary errors)
        {
            List<string> errorList = new List<string>();
            foreach (var modelState in errors)
            {
                foreach (var error in modelState.Value.Errors)
                {
                    errorList.Add(error.ErrorMessage);
                }
            }
            return errorList;
        }
        #endregion

        #region GetRoleName - khanhkk
        public static string GetRoleName(byte role)
        {
            Dictionary<byte, string> roleList = new Dictionary<byte, string>
            {
                //{ 1, "Admin" },
                { 2, "Kế toán" },
                { 3, "HR" },
                { 4, "Capital Consultant" },
                { 5, "Capital Leader" },
                { 6, "Capital Manager" },
                { 7, "Admin" },
                { 8, "TeleSale" },
                { 9, "Leader TeleSale" },
                { 10, "Capital Director" },
                { 11, "Collabrator" },
            };

            return roleList.GetValueOrDefault(role);
        }
        #endregion

        #region GetRole
        public static byte GetRole(string roleName)
        {
            Dictionary<string, byte> roleList = new Dictionary<string, byte>
            {
                //{ "Admin", 1 },
                { "Kế toán", 2 },
                { "HR", 3 },
                { "Capital Consultant", 4 }, //sale
                { "Capital Leader", 5 }, //sale leader
                { "Capital Manager", 6}, //sale manager
                { "Admin", 7 }, //sale admin
                { "TeleSale", 8 },
                { "Leader TeleSale", 9 },
                { "Capital Director", 10 }, //truong khoi
                { "Collabrator", 11 }, //ctv
            };

            return roleList.GetValueOrDefault(roleName);
        }
        #endregion

        #region DeleteImage
        public static bool DeleteImage(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
