// ------------------------------public --------------------------------------------------------------------------------------
// <copyright file="Converter.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Converter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Lib.AdvancedProperties;
using Lib.Tools.BO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Weblib.Helpers
{
    public class ExcelExportHelper
    {
        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable(Dictionary<long, ItemBase> data, AdvancedProperties properties, bool includeSystemColumns)
        {
            DataTable dataTable = new DataTable();

            if (includeSystemColumns)
                dataTable.Columns.Add("Id", typeof(long));
            foreach (Lib.AdvancedProperties.AdvancedProperty property in properties)
            {
                dataTable.Columns.Add(property.Common.PrintName, typeof(string));
            }
            if (includeSystemColumns)
            {
                dataTable.Columns.Add("Creat la data", typeof(string));
                dataTable.Columns.Add("Autor", typeof(string));
            }
            if (data.Count > 0)
            {
                foreach (var pitem in data.Values)
                {
                    object[] values = new object[properties.Count + (includeSystemColumns ? 3 : 0)];
                    var i = 0;
                    if (includeSystemColumns)
                    {
                        values[i] = pitem.Id;
                        i++;
                    }
                    foreach (Lib.AdvancedProperties.AdvancedProperty property in properties)
                    {
                        values[i] = property.GetDataProcessor().ToString(property.PropertyDescriptor.GetValue(pitem), property, pitem, DisplayMode.Excell).Replace("&nbsp;", "").Trim();
                        i++;
                    }
                    if (includeSystemColumns)
                    {
                        values[i] = pitem.DateCreated.ToString();
                        i++;
                        values[i] = pitem.CreatedBy.Login;
                        i++;
                    }
                    dataTable.Rows.Add(values);
                }
            }
            else
            {
                dataTable.Rows.Add();
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string heading = "", DataTable additionalIfo = null, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = 1;// String.IsNullOrEmpty(heading) ? 1 : 3;
                
                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                if (additionalIfo != null)
                {
                    var position = dataTable.Rows.Count + 2;
                    workSheet.Cells["A" + position].LoadFromDataTable(additionalIfo, false);

                }

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    //int maxLength = columnCells.Max(cell => (cell.Value!=null?cell.Value.ToString().Count():0));
                    //if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                    r.Style.WrapText = true;

                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                }
                // format additional info
                if (additionalIfo != null)
                {
                    using (ExcelRange r = workSheet.Cells[dataTable.Rows.Count + 2, 1, startRowFrom + dataTable.Rows.Count + additionalIfo.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#e9f0eb"));

                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                        r.Style.WrapText = true;

                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        r.Style.Font.Bold = true;
                    }
                }

                if (columnsToTake != null && columnsToTake.Length > 0)
                {
                    // removed ignored columns  
                    for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                    {
                        if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                        {
                            workSheet.DeleteColumn(i + 1);
                        }
                    }
                }

                /* if (!String.IsNullOrEmpty(heading))
                 {
                     workSheet.Cells["A1"].Value = heading;
                     workSheet.Cells["A1"].Style.Font.Size = 20;

                     workSheet.InsertColumn(1, 1);
                     workSheet.InsertRow(1, 1);
                     workSheet.Column(1).Width = 5;
                 }*/

                result = package.GetAsByteArray();
                }

            return result;
        }

        public static byte[] ExportExcel(Dictionary<long, ItemBase> data, AdvancedProperties properties, string Heading = "",bool includeSystemColumns=false, DataTable additionalInfo = null, params string[] ColumnsToTake)
        {
            return ExportExcel(ListToDataTable(data, properties, includeSystemColumns), Heading, additionalInfo, ColumnsToTake);
        }

        public static List<string[]> ReadExcelImportedData(FileInfo File)
        {
            var outData = new List<string[]>();
            using (ExcelPackage package = new ExcelPackage(File))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                int colCount = worksheet.Dimension.End.Column;  //get Column Count
                int rowCount = worksheet.Dimension.End.Row;     //get row count

                for(int col = 2; col <= colCount; col++)
                {
                    var rowData = new string[rowCount - 1];
                    for(int row = 2; row <= rowCount; row++)
                    {
                        rowData[row - 2] = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : string.Empty;
                    }
                    outData.Add(rowData);
                }
                //for(int row = 2; row <= rowCount; row++)
                //{
                //    var rowData = new string[colCount - 1];
                //    for(int col = 2; col <= colCount; col++)
                //    {
                //        rowData[col - 2] = worksheet.Cells[row,col].Value != null ? worksheet.Cells[row, col].Value.ToString() : string.Empty;
                //    }
                //    outData.Add(rowData);
                //}
            }

            return outData;
        }

    }
}
