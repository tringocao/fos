using FOS.Model.Domain;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FOS.Services.ExcelService
{
    public class ExcelService : IExcelService
    {
        async public Task<bool> ExportCSV(ExcelModel excelModel)
        {
            try
            {
                var file = Common.Constants.Constant.FileCsvDirectory;

                using (var stream = File.CreateText(file))
                {
                    string csvHeader = string.Format("{0},{1},{2},{3},{4}", "Event", "", "", "Restaurant", "");
                    stream.WriteLine(csvHeader, Encoding.UTF8);
                    string lineName = string.Format("{0},{1},{2},{3},{4}", "Name", excelModel.Event.Name, "", "Name", excelModel.RestaurantExcel.Name);
                    stream.WriteLine(lineName, Encoding.UTF8);
                    var restaurantAddress = excelModel.RestaurantExcel.Address.Replace(",", ";").ToString();
                    string lineHostAndAdress = string.Format("{0},{1},{2},{3},{4}", "Host", excelModel.Event.HostName, "", "Adress", restaurantAddress);
                    stream.WriteLine(lineHostAndAdress, Encoding.UTF8);
                    DateTime closeTime = DateTime.Parse(excelModel.Event.CloseTime.ToString());
                    string closeTimeLine = String.Format("{0:MM/dd/yyyy HH:mm}", closeTime);
                    string TimeToClose = string.Format("{0},{1}", "Time to close", closeTimeLine);
                    stream.WriteLine(TimeToClose, Encoding.UTF8);
                    string Status = string.Format("{0},{1}", "Status", excelModel.Event.Status);
                    stream.WriteLine(Status, Encoding.UTF8);
                    stream.WriteLine("", Encoding.UTF8);
                    string csvFoodHeader = string.Format("{0},{1},{2},{3},{4},{5}", "Name", "Users", "Amount", "Price", "Total", "Comment");
                    stream.WriteLine(csvFoodHeader, Encoding.UTF8);

                    foreach (FoodReport f in excelModel.FoodReport)
                    {
                        string foodReportName = f.Name;
                        string foodReportAmount = f.Amount.ToString();
                        string foodReportPrice = f.Price.ToString();
                        string foodReportTotal = f.Total.ToString();
                        string comment = "";
                        foreach (Comment c in f.Comments)
                        {
                            comment += c.Value.ToString() + " ;";
                        }
                        string foodReportUsers = "";
                        foreach (string us in f.UserIds)
                        {
                            User foodReportUser = excelModel.User.FirstOrDefault(u => u.Id == us);
                            foodReportUsers += foodReportUser.DisplayName + " ;";
                        }
                        string csvRow = string.Format("{0},{1},{2},{3},{4},{5}", foodReportName, foodReportUsers, foodReportAmount, foodReportPrice, foodReportTotal, comment);

                        stream.WriteLine(csvRow, Encoding.UTF8);
                    }
                    string total = string.Format("{0},{1},{2},{3},{4},{5}", "", "", "", "", excelModel.Total.ToString(), "");

                    stream.WriteLine(total, Encoding.UTF8);

                    stream.WriteLine("", Encoding.UTF8);
                    string csvUserHeader = string.Format("{0},{1},{2},{3},{4}", "User", "Food", "Price", "Pay Extra", "Comment");
                    stream.WriteLine(csvUserHeader, Encoding.UTF8);

                    foreach (UserOrder u in excelModel.UserOrder)
                    {
                        string userName = u.User.DisplayName;
                        string userFood = u.Food.Replace(",", ";").ToString();
                        string userPrice = u.Price.ToString();
                        string userPayExtra = u.PayExtra.ToString();
                        string comment = "";
                        foreach (Comment c in u.Comments)
                        {
                            comment += c.Value.ToString() + " ;";
                        }

                        string csvRow = string.Format("{0},{1},{2},{3},{4}", userName, userFood, userPrice, userPayExtra, comment);

                        stream.WriteLine(csvRow, Encoding.UTF8);
                    }
                }
                await ConvertCSVToExcel();
                return true;
            }
            catch (Exception e)
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
                bool firstRowIsHeader = false;

                var excelTextFormat = new ExcelTextFormat();
                excelTextFormat.Delimiter = ',';
                excelTextFormat.Encoding = new UTF8Encoding();
                excelTextFormat.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                excelTextFormat.Culture.DateTimeFormat.ShortDatePattern = "mm-dd-yyyy HH:mm";

                var excelFileInfo = new FileInfo(excelFilePath);
                var csvFileInfo = new FileInfo(csvFilePath);

                using (ExcelPackage package = new ExcelPackage(excelFileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                    worksheet.Cells["A1"].LoadFromText(csvFileInfo, excelTextFormat, OfficeOpenXml.Table.TableStyles.None, firstRowIsHeader).AutoFitColumns();
                    worksheet.Cells["A1:D1"].Style.Font.Bold = true;
                    worksheet.Cells["A7:B7"].Style.Font.Bold = true;
                    worksheet.Cells["A7:B7"].Style.Font.Bold = true;
                    worksheet.Cells["A7:C7"].Style.Font.Bold = true;
                    worksheet.Cells["A7:E7"].Style.Font.Bold = true;
                    worksheet.Cells["A7:F7"].Style.Font.Bold = true;


                    package.Save();
                    foreach (ExcelWorksheet worksheets in package.Workbook.Worksheets)
                    {

                        var dimension = worksheets.Dimension;
                        if (dimension == null) { continue; }
                        var cells = from row in Enumerable.Range(dimension.Start.Row, dimension.End.Row)
                                    from column in Enumerable.Range(dimension.Start.Column, dimension.End.Column)
                                        //where worksheet.Cells[row, column].Value.ToString() != String.Empty
                                    select worksheets.Cells[row, column];
                        try
                        {
                            foreach (var excelCell in cells)
                            {
                                try
                                {
                                    if (excelCell.Value.ToString().Equals("User") || excelCell.Value.ToString().Equals("Food")
                                        || excelCell.Value.ToString().Equals("Price") || excelCell.Value.ToString().Equals("Pay Extra")
                                        || excelCell.Value.ToString().Equals("Comment"))
                                    {
                                        worksheet.Cells[excelCell.Address].Style.Font.Bold = true;
                                    }

                                }
                                catch (Exception) { }
                            }

                        }
                        catch (Exception a) { Console.WriteLine(a.Message); }
                        package.Save();
                    }
                }


                return true;
            }
            catch (Exception e)
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
