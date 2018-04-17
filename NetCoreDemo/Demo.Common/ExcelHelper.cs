using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Demo.Common
{
    /// <summary>
    /// excel帮助类型
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 获取导出excel的字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas">对应实体</param>
        /// <param name="columnName">excel中首行列名</param>
        /// <param name="outOfColumn">去除实体中不要的字段</param>
        /// <returns></returns>
        public static Byte[] GetByteToExportExcel<T>(List<T> datas, Dictionary<string, string> columnNames, List<string> outOfColumn, string sheetName = "Sheet1")
        {
            using (var fs = new MemoryStream())
            {
                using (var package = CreateExcelPackage(datas, columnNames, outOfColumn, sheetName))
                {
                    package.SaveAs(fs);

                    return fs.ToArray();
                }
            }
        }

        private static ExcelPackage CreateExcelPackage<T>(List<T> datas, Dictionary<string, string> columnNames, List<string> outOfColumns, string sheetName = "Sheet1")
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            //获取要反射的属性,加载首行
            Type myType = typeof(T);
            List<PropertyInfo> myPro = new List<PropertyInfo>();
            int i = 1;
            foreach (string key in columnNames.Keys)
            {
                PropertyInfo p = myType.GetProperty(key);
                myPro.Add(p);

                worksheet.Cells[1, i].Value = columnNames[key];
                i++;
            }


            int row = 2;
            foreach (T data in datas)
            {
                int column = 1;
                foreach (PropertyInfo p in myPro.Where(info => !outOfColumns.Contains(info.Name)))
                {
                    worksheet.Cells[row, column].Value = p == null ? "" : Convert.ToString(p.GetValue(data, null));
                    column++;
                }
                row++;
            }
            return package;
        }
    }
}
