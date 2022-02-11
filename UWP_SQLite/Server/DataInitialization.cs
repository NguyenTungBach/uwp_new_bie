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
            string sql = @"CREATE TABLE IF NOT EXISTS
            PersonalTransaction (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            Name VARCHAR( 140 ),
            Detail TEXT,
            Description TEXT,
            Money DOULE,
            CreatedDate DATE,
            Category INT
            );";
            using (var statement = cnn.Prepare(sql))
            {
                statement.Step();
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
