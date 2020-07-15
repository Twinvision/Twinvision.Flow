using System;
using System.Collections.Generic;

namespace Twinvision.Flow.Tests
{
    public class TestRecordClass
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Active { get; set; }
    }

    public static class TestRecordData
    {
        public readonly static List<TestRecordClass> ListRecords = new List<TestRecordClass>() { new TestRecordClass() { Name = "John Smith", EmailAddress = "smith@company.com", BirthDate = new DateTime(1990, 1, 15), Active = true } };
        public static System.Data.DataTable TableRecords()
        {
            System.Data.DataTable table = new System.Data.DataTable("Test");
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("EmailAddress", typeof(string));
            table.Columns.Add("BirthDate", typeof(DateTime));
            table.Columns.Add("Active", typeof(bool));
            System.Data.DataRow row = table.NewRow();
            row["Name"] = "John Smith";
            row["EmailAddress"] = "smith@company.com";
            row["BirthDate"] = new DateTime(1990, 1, 15);
            row["Active"] = true;
            table.Rows.Add(row);
            return table;
        }
    }
}
