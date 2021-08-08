using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text;
using Newtonsoft.Json;

namespace CRMSite.Common
{
    //Hungvx
    public static class Utils
    {
        public static Dictionary<int, string> GetDesribles<T>()
        {
            var dic = new Dictionary<int, string>();
            var values = System.Enum.GetValues(typeof(T));
            foreach (var item in values)
            {
                string description;
                try
                {
                    description = ((DescriptionAttribute)item.GetType().GetMember(item.ToString()).FirstOrDefault()
                        .GetCustomAttribute(typeof(DescriptionAttribute))).Description;
                }
                catch (Exception ex)
                {
                    description = item.ToString();
                }
                dic.Add((int)item, description);
            }
            return dic;
        }
        public static string IdentitySTT(int count, string name = null)
        {
            if (count >= 0 && count < 10)
            {
                return name + String.Format("00{0}", count + 1);
            }
            else if (10 <= count && count < 100)
            {
                return name + String.Format("0{0}", count + 1);
            }
            else if (count >= 100)
            {
                return name + String.Format("{0}", count++);
            }
            return string.Empty;
        }
        public static bool CheckDateTime(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                DateTime dob;
                bool validDOBDate = DateTime.TryParseExact(date, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dob);
                if (!validDOBDate)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public static bool CheckDateTime2(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                DateTime dob;
                bool validDOBDate = DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dob);
                if (!validDOBDate)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public static string MoneyToText(string value1)
        {
            Func<string, string> numToText = delegate (string num)
            {
                if (num == "0")
                {
                    return "không";
                }
                else if (num == "1")
                {
                    return "một";
                }
                else if (num == "2")
                {
                    return "hai";
                }
                else if (num == "3")
                {
                    return "ba";
                }
                else if (num == "4")
                {
                    return "bốn";
                }
                else if (num == "5")
                {
                    return "năm";
                }
                else if (num == "6")
                {
                    return "sáu";
                }
                else if (num == "7")
                {
                    return "bảy";
                }
                else if (num == "8")
                {
                    return "tám";
                }
                else if (num == "9")
                {
                    return "chín";
                }
                else if (num == "10")
                {
                    return "mười";
                }
                return "";
            };

            Func<string, string> docDonVi = null;
            docDonVi = (string value) =>
            {
                if (value.Length == 1)
                {
                    return numToText(value);
                }
                else if (value.Length == 2)
                {
                    if (value == "10")
                    {
                        return numToText(value);
                    }
                    else
                    {
                        if (value[0] == '1')
                        {
                            return "mười " + numToText(value[1].ToString());
                        }
                        var txt = numToText(value[0].ToString()) + " mươi";
                        if (value[1] != '0')
                        {
                            if (value[1] == '1')
                            {
                                return txt + " mốt";
                            }
                            return txt + " " + numToText(value[1].ToString());
                        }
                        return txt;
                    }
                }
                else
                {
                    var txt = "";
                    txt = numToText(value[0].ToString()) + " trăm";
                    if (value[1] == '0')
                    {
                        if (value[2] != '0')
                        {
                            txt += " linh " + numToText(value[2].ToString());
                        }
                    }
                    else
                    {
                        txt += " " + docDonVi(value[1].ToString() + value[2].ToString());
                    }
                    return txt;
                }
            };

            Func<string, int, List<string>> getDonvi = (string value, int boi) =>
            {
                List<string> donvi = new List<string>();
                var temp = "";
                for (var i = value.Length; i > 0; i--)
                {
                    var e = value[i - 1];
                    temp = e + temp;
                    if ((value.Length - i + 1) % boi == 0 || i == 1)
                    {
                        donvi.Insert(0, temp);
                        temp = "";
                    }
                }
                return donvi;
            };

            value1 = !string.IsNullOrWhiteSpace(value1) ? value1 : "0";
            value1 += "";
            var txt_return = "";

            var donvi1 = getDonvi(value1, 3);
            var hangTy = getDonvi(value1, 9);

            for (var i = 0; i < donvi1.Count; i++)
            {
                var txt1 = docDonVi(donvi1[i]);
                switch (donvi1.Count - i - 1)
                {
                    case 0:
                        break;
                    case 1:
                        txt1 += " nghìn";
                        break;
                    case 2:
                        txt1 += " triệu";
                        break;
                    case 3:
                        txt1 += " tỷ";
                        break;
                    case 4:
                        txt1 += " nghìn tỷ";
                        break;
                    case 5:
                        txt1 += " triệu tỷ";
                        break;
                    default:
                        txt1 += " tỷ tỷ";
                        break;
                }
                txt_return += " " + txt1;
                var conlai = donvi1.Skip(i + 1).Take(donvi1.Count - 1).ToList();
                var check = conlai.Count > 0 && int.Parse(string.Join("", conlai)) > 0;
                if (!check)
                {
                    break;
                }
                if (i != donvi1.Count - 1)
                {
                    txt_return += " "; //,
                }
            }
            txt_return = txt_return.Trim();
            txt_return = txt_return.Substring(0, 1).ToUpper() + txt_return.Substring(1);
            return txt_return;
        }
        public static string GenCodeContract(int count, string date = "SFG/SFI")
        {
            string name = string.Empty;
            if (count >= 0 && count < 9)
            {
                return name + String.Format("00{0}/{1}/{2}", count + 1, DateTime.Now.ToString("ddMM/yyyy"), date);
            }
            else if (9 <= count && count < 100)
            {
                return name + String.Format("0{0}/{1}/{2}", count + 1, DateTime.Now.ToString("ddMM/yyyy"), date);
            }
            else if (count >= 100 && count < 999)
            {
                return name + String.Format("{0}/{1}/{2}", count + 1, DateTime.Now.ToString("ddMM/yyyy"), date);
            }
            else
            {
                return name + String.Format("{0}/{1}/{2}", count + 1, DateTime.Now.ToString("ddMM/yyyy"), date);
            }
        }
        public static string GenCodeContractForAmber(int count, string code = "Amber")
        {
            string name = string.Empty;
            if (count >= 0 && count < 9)
            {
                return name + String.Format("00{0}/{1}/{2}", count + 1, DateTime.Now.ToString("ddMM/yyyy"), code);
            }
            else if (9 <= count && count < 100)
            {
                return name + String.Format("0{0}/{1}/{2}", count + 1, DateTime.Now.ToString("ddMM/yyyy"), code);
            }
            else if (count >= 100 && count < 999)
            {
                return name + String.Format("{0}/{1}/{2}", count + 1, DateTime.Now.ToString("ddMM/yyyy"), code);
            }
            else
            {
                return name + String.Format("{0}/{1}/{2}", count + 1, DateTime.Now.ToString("ddMM/yyyy"), code);
            }
        }
        #region ConvertToUnsign
        public static string convertToUnSign(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
        #endregion
        #region NumberToWords
        private static string NumberToWords(long number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                if((number / 1000) > 0 && (number / 1000) <= 1)
                {
                    words += NumberToWords(number / 1000) + " thousand ";
                    number %= 1000;
                }
                else
                {
                    words += NumberToWords(number / 1000) + " thousands ";
                    number %= 1000;
                }
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string ConvertCurrencyToText(long amount)
        {
            string textResult = NumberToWords(amount);
            string displayText = textResult.First().ToString().ToUpper() + textResult.Substring(1);
            return displayText;
        }
        #endregion

        #region GenerateOTP
        public static string Shuffle(this string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }

        public static string GenerateOTP()
        {
            StringBuilder otpBuilder = new StringBuilder(Guid.NewGuid().ToString().Substring(0, 6));
            string otpValue = otpBuilder.ToString().Shuffle();
            return otpValue;
        }
        #endregion

        #region ToDataString
        public static string ToDataString(this object obj)
        {
            if (obj != null)
            {
                string data = JsonConvert.SerializeObject(obj);
                return data;
            }
            else
            {
                return string.Empty;
            }    
        }
        #endregion

        #region ToMessageString
        public static string ToMessageString(this List<string> list)
        {
            if (list != null)
            {
                string message = string.Join(SiteConst.CommaChar, list);
                return message;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }

    public class CustomBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(decimal))
            {
                return new DecimalModelBinder();
            }

            return null;
        }
    }

    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == null)
            {
                return Task.CompletedTask;
            }
             
            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            // Remove unnecessary commas and spaces
            value = value.Replace(",", string.Empty).Trim();

            decimal myValue = 0;
            if (!decimal.TryParse(value, out myValue))
            {
                // Error
                bindingContext.ModelState.TryAddModelError(
                                        bindingContext.ModelName,
                                        "Could not parse MyValue.");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(myValue);
            return Task.CompletedTask;
        }
    }
    
    public static class ControllerExtensions
    {
        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
