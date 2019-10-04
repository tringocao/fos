using FOS.Model.Domain;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.ExcelService
{
    public class ExcelService : IExcelService
    {
        async public Task<bool> ExportCSV(List<UserOrder> userOrder)
        {
            try
            {
                var file = Common.Constants.Constant.FileCsvDirectory;

                using (var stream = File.CreateText(file))
                {
                    string csvHeader = string.Format("{0},{1},{2},{3},{4}", "Name", "Food", "Price", "PayExtra", "Comment");
                    stream.WriteLine(csvHeader, Encoding.UTF8);
                    foreach (UserOrder u in userOrder)
                    {
                        string displayName = u.User.DisplayName;
                        string food = u.Food.ToString().Replace(',', ';');
                        string price = u.Price.ToString();
                        string payExtra = u.PayExtra.ToString();
                        string comment = "";
                        foreach (Comment c in u.Comments)
                        {
                            comment += c.Value.ToString() + " ;";
                        }

                        string csvRow = string.Format("{0},{1},{2},{3},{4}", displayName, food, price, payExtra, comment);

                        stream.WriteLine(csvRow, Encoding.UTF8);
                    }
                }
                await ConvertCSVToExcel();
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }
        public async Task<bool> ConvertCSVToExcel()
        {
            try
            {
                var check = await DeleteFile();
                string csvFilePath = Common.Constants.Constant.FileCsvDirectory;
                string excelFilePath = Common.Constants.Constant.FileXlsxDirectory;

                string worksheetsName = Common.Constants.Constant.FileXlsxName;
                bool firstRowIsHeader = true;

                var excelTextFormat = new ExcelTextFormat();
                excelTextFormat.Delimiter = ',';
                excelTextFormat.EOL = "\r";

                var excelFileInfo = new FileInfo(excelFilePath);
                var csvFileInfo = new FileInfo(csvFilePath);

                using (ExcelPackage package = new ExcelPackage(excelFileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                    worksheet.Cells["A1"].LoadFromText(csvFileInfo, excelTextFormat, OfficeOpenXml.Table.TableStyles.Medium25, firstRowIsHeader).AutoFitColumns();
                    package.Save();
                }

                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public async Task<bool> DeleteFile()
        {
            string authorsFile = Common.Constants.Constant.FileXlsxNameWithExtension;
            string rootFolder = Common.Constants.Constant.RootDirectory;
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(Path.Combine(rootFolder, authorsFile)))
                {
                    // If file found, delete it    
                    File.Delete(Path.Combine(rootFolder, authorsFile));
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");
                 return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
