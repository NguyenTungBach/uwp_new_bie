using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_SQLite.Entity;

namespace UWP_SQLite.Server
{
    class DataInitialization
    {
        public static int totalMoney;

        public static void CreatedTransaction()
        {
            SQLiteConnection cnn = new SQLiteConnection("transactionsqlite.db");
            string sqlPersonalTransaction = @"CREATE TABLE IF NOT EXISTS
            PersonalTransaction (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            Name VARCHAR( 140 ),
            Detail TEXT,
            Description TEXT,
            Money DOULE,
            CreatedDate DATE,
            Category INT
            );";
            using (var statement = cnn.Prepare(sqlPersonalTransaction))
            {
                statement.Step();
            }

            string sqlDropCategoryTransaction = @"DROP TABLE IF EXISTS CategoryTransaction;";
            using (var statement = cnn.Prepare(sqlDropCategoryTransaction))
            {
                statement.Step();
            }
            string sqlCategoryTransaction = @"CREATE TABLE IF NOT EXISTS
            CategoryTransaction (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            Name VARCHAR( 140 )
            );
            ";
            using (var statement = cnn.Prepare(sqlCategoryTransaction))
            {
                statement.Step();
            }
            using (var categoryTransaction = cnn.Prepare("INSERT INTO CategoryTransaction(Name) VALUES(?),(?),(?)"))
            {
                categoryTransaction.Bind(1, "Tien Tet");
                categoryTransaction.Bind(2, "Tien Tran Lot");
                categoryTransaction.Bind(3, "Tien An Va");
                categoryTransaction.Step();
            }
        }

        public static bool InsertTransaction(PersonalTransaction personal)
        {
            try
            {
                SQLiteConnection cnn = new SQLiteConnection("transactionsqlite.db");
                using (var personalTransaction = cnn.Prepare("INSERT INTO PersonalTransaction(Name, Detail, Description, Money, CreatedDate, Category) VALUES(?, ? , ? , ? , ?, ?)"))
                {
                    personalTransaction.Bind(1, personal.Name);
                    personalTransaction.Bind(2, personal.Description);
                    personalTransaction.Bind(3, personal.Detail);
                    personalTransaction.Bind(4, personal.Money);
                    personalTransaction.Bind(5, personal.CreatedDate.ToString("yyyy-MM-dd"));
                    personalTransaction.Bind(6, personal.Category);
                    personalTransaction.Step();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static List<PersonalTransaction> ListTransaction()
        {
            totalMoney = 0;
            var list = new List<PersonalTransaction>();
            try
            {
                SQLiteConnection cnn = new SQLiteConnection("transactionsqlite.db");
                using (var stt = cnn.Prepare("select * from PersonalTransaction"))
                {
                    while(stt.Step() == SQLiteResult.ROW)
                    {
                        var personal = new PersonalTransaction()
                        {
                            Name = (string)stt["Name"],
                            Detail = (string)stt["Detail"],
                            Description = (string)stt["Description"],
                            Money = Convert.ToDouble(stt["Money"]),
                            CreatedDate = Convert.ToDateTime(stt["CreatedDate"]),
                            Category = Convert.ToInt32(stt["Category"]),
                        };
                        list.Add(personal);
                        totalMoney += Convert.ToInt32(stt["Money"]);
                    }
                }
                //Debug.WriteLine(list[0]);
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Co loi list"+ ex);
                return null;
            }
        }

        public static List<CategoryTransaction> ListCategoryTransaction()
        {
            var listCategory = new List<CategoryTransaction>();
            try
            {
                SQLiteConnection cnn = new SQLiteConnection("transactionsqlite.db");
                using (var stt = cnn.Prepare("select * from CategoryTransaction"))
                {
                    while (stt.Step() == SQLiteResult.ROW)
                    {
                        var category = new CategoryTransaction()
                        {
                            Id = Convert.ToInt32(stt["Id"]),
                            Name = (string)stt["Name"],
                        };
                        listCategory.Add(category);
                    }
                }
                //Debug.WriteLine(list[0]);
                return listCategory;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Co loi list category" + ex);
                return null;
            }
        }

        public static List<PersonalTransaction> ListTransactionByCategory(int Category)
        {
            totalMoney = 0;
            var list = new List<PersonalTransaction>();
            try
            {
                SQLiteConnection cnn = new SQLiteConnection("transactionsqlite.db");
                using (var stt = cnn.Prepare($"select * from PersonalTransaction where Category = {Category}"))
                {
                    while (stt.Step() == SQLiteResult.ROW)
                    {
                        var personal = new PersonalTransaction()
                        {
                            Name = (string)stt["Name"],
                            Detail = (string)stt["Detail"],
                            Description = (string)stt["Description"],
                            Money = Convert.ToDouble(stt["Money"]),
                            CreatedDate = Convert.ToDateTime(stt["CreatedDate"]),
                            Category = Convert.ToInt32(stt["Category"]),
                        };
                        list.Add(personal);
                        totalMoney += Convert.ToInt32(stt["Money"]);
                    }
                }
                //Debug.WriteLine(list[0]);
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Co loi list" + ex);
                return null;
            }
        }

        public static List<PersonalTransaction> ListTransactionByStartDateAndEndDate(string startDate, string endDate)
        {
            totalMoney = 0;
            var list = new List<PersonalTransaction>();
            try
            {
                SQLiteConnection cnn = new SQLiteConnection("transactionsqlite.db");
                using (var stt = cnn.Prepare($"select * from PersonalTransaction where CreatedDate between '{startDate}' and '{endDate}'"))
                {
                    while (stt.Step() == SQLiteResult.ROW)
                    {
                        var personal = new PersonalTransaction()
                        {
                            Name = (string)stt["Name"],
                            Detail = (string)stt["Detail"],
                            Description = (string)stt["Description"],
                            Money = Convert.ToDouble(stt["Money"]),
                            CreatedDate = Convert.ToDateTime(stt["CreatedDate"]),
                            Category = Convert.ToInt32(stt["Category"]),
                        };
                        list.Add(personal);
                        totalMoney += Convert.ToInt32(stt["Money"]);
                    }
                }
                //Debug.WriteLine(list[0]);
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Co loi list" + ex);
                return null;
            }
        }
    }
}
