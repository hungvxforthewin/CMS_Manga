using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CRMBussiness
{
    public class ExcelExtension
    {
        public static double string_double(string s)
        {
            double temp = 0;
            double dtemp = 0;
            int b = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '.' || s[i] == ',')
                {
                    i++;
                    while (i < s.Length)
                    {
                        dtemp = (dtemp * 10) + (int)char.GetNumericValue(s[i]);
                        i++;
                        b++;
                    }
                    temp = temp + (dtemp * Math.Pow(10, -b));
                    return temp;
                }
                else
                {
                    temp = (temp * 10) + (int)char.GetNumericValue(s[i]);
                }
            }
            return temp;
        }
        public static List<T> ReadtoList<T>(string pathFile, ref List<string> listErrors) where T : new()
        {
            try
            {
                using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(pathFile, true))
                {
                    WorkbookPart workbookPart = spreadSheet.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    var list = new List<T>();
                    var PropertiesModel = typeof(T).GetProperties();
                    List<PropertyOfModel_ExcelColumn> listProperty = new List<PropertyOfModel_ExcelColumn>();
                    foreach (var item in PropertiesModel)
                    {
                        ExcelColumn ec = (ExcelColumn)Attribute.GetCustomAttributes(item, typeof(ExcelColumn)).SingleOrDefault();
                        if (ec != null)
                        {
                            listProperty.Add(new PropertyOfModel_ExcelColumn(item.Name, ec.ColumnName));
                        }
                    }
                    if (listProperty.Count > 0)
                    {
                        foreach (var row in sheetData.Elements<Row>())
                        {
                            if (row.RowIndex > 1)
                            {
                                var obj = new T();
                                bool check = false;
                                foreach (var cell in row.Elements<Cell>())
                                {
                                    if (cell.CellValue != null)
                                    {
                                        check = true;
                                        Regex regex = new Regex("[A-Za-z]+");
                                        Match match = regex.Match(cell.CellReference);
                                        string columnName = match.Value;

                                        PropertyOfModel_ExcelColumn PE = listProperty.Where(t => t.ExcelColumn == columnName).SingleOrDefault();
                                        if (PE != null)
                                        {
                                            string PropertyName = PE.PropertyOfModel;
                                            try
                                            {
                                                string cellValue = "";
                                                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                                                {
                                                    int id = -1;

                                                    if (Int32.TryParse(cell.InnerText, out id))
                                                    {
                                                        SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                                                        if (item.Text != null)
                                                        {
                                                            cellValue = item.Text.Text;
                                                        }
                                                        else if (item.InnerText != null)
                                                        {
                                                            cellValue = item.InnerText;
                                                        }
                                                        else if (item.InnerXml != null)
                                                        {
                                                            cellValue = item.InnerXml;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    cellValue = cell.CellValue.Text;
                                                }
                                                var nullable = obj.GetType().GetProperty(PropertyName).PropertyType;
                                                //    //check Nullable Column
                                                if (nullable.Name == "Nullable`1")
                                                {
                                                    var type = obj.GetType().GetProperty(PropertyName).PropertyType.GenericTypeArguments[0];
                                                    if (type == typeof(DateTime))
                                                    {
                                                        var value = DateTime.FromOADate(double.Parse(cellValue));
                                                        obj.GetType().GetProperty(PropertyName).SetValue(obj, value);
                                                    }
                                                    else if (type == typeof(double))
                                                    {

                                                        double value = string_double(cellValue);
                                                        obj.GetType().GetProperty(PropertyName).SetValue(obj, value);
                                                    }
                                                    else if (type == typeof(decimal))
                                                    {

                                                        decimal value = decimal.Parse(string_double(cellValue).ToString());
                                                        obj.GetType().GetProperty(PropertyName).SetValue(obj, value);
                                                    }
                                                    else
                                                    {
                                                        var value = Convert.ChangeType(cellValue, Nullable.GetUnderlyingType(obj.GetType().GetProperty(PropertyName).PropertyType));
                                                        obj.GetType().GetProperty(PropertyName).SetValue(obj, value);

                                                    }
                                                }
                                                else
                                                {
                                                    var value = Convert.ChangeType(cellValue, obj.GetType().GetProperty(PropertyName).PropertyType);
                                                    //(usedrange.Cells[row, col] as Excel.Range).Value;
                                                    obj.GetType().GetProperty(PropertyName).SetValue(obj, value);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                listErrors.Add(string.Format("Lỗi dữ liệu hàng {0} - Cột {1}", row.RowIndex, columnName));
                                                obj = default(T);
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (check && obj != null)
                                {
                                    list.Add(obj);
                                }
                            }
                        }
                        //spreadSheet.Close();
                        return list;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
        public class ExcelColumn : Attribute
        {
            public string ColumnName { get; set; }
            public ExcelColumn(string ColumnName)
            {
                this.ColumnName = ColumnName;
            }
        }
        public class PropertyOfModel_ExcelColumn
        {
            public string PropertyOfModel { get; set; }
            public string ExcelColumn { get; set; }
            public PropertyOfModel_ExcelColumn(string PropertyOfModel, string ExcelColumn)
            {
                this.PropertyOfModel = PropertyOfModel;
                this.ExcelColumn = ExcelColumn;
            }
        }
        public class AccountStaffViewModel
        {
            [ExcelColumn(ColumnName: "A")]
            public string EmployeeID { get; set; }
            [ExcelColumn(ColumnName: "B")]
            public string FullName { get; set; }
            [ExcelColumn(ColumnName: "C")]
            public string TeamDepart { get; set; }
            [ExcelColumn(ColumnName: "D")]
            public string Position { get; set; }
            [ExcelColumn(ColumnName: "E")]
            public string StartDate { get; set; }
            [ExcelColumn(ColumnName: "F")]
            public string DateOfBirth { get; set; }
            [ExcelColumn(ColumnName: "G")]
            public string CMT { get; set; }
            [ExcelColumn(ColumnName: "H")]
            public string DateIssued { get; set; }//NÀY CẤP
            [ExcelColumn(ColumnName: "I")]
            public string AddIssued { get; set; }//NƠI CẤP
            [ExcelColumn(ColumnName: "J")]
            public string PermanentAddress { get; set; }//ĐỊA CHỈ THƯỜNG CHÚ
            [ExcelColumn(ColumnName: "K")]
            public string CurrentResidence { get; set; }//CHÔ Ở HIỆN TẠI
            [ExcelColumn(ColumnName: "L")]
            public string Phone { get; set; }
            [ExcelColumn(ColumnName: "M")]
            public string Email { get; set; }
            [ExcelColumn(ColumnName: "N")]
            public string NumberBank { get; set; } //SỐ TÀI KHOẢN
            [ExcelColumn(ColumnName: "O")]
            public string Bank { get; set; } //NGÂN HÀNG
            public string ImportMessage { get; set; }
            public int rownumber { get; set; }
            public int Role { get; set; }
            public string TeamCode { get; set; }

            //HỢP ĐỒNG
            public string LaborContractName { get; set; }//TÊN HỢP ĐỒNG
            public int Duration { get; set; }//LOẠI HỢP ĐỒNG
        }
    }
}
