using System;
using System.Data.SQLite;
using System.IO;

namespace SimplePOS
{
    public static class Db
    {
        public static string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pos.db");
        public static string ConnStr => $"Data Source={DbPath};Version=3;";

        public static void Init()
        {
            bool create = !File.Exists(DbPath);
            if (create) SQLiteConnection.CreateFile(DbPath);
            using var c = new SQLiteConnection(ConnStr);
            c.Open();
            string schema = @"
CREATE TABLE IF NOT EXISTS products(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  barcode TEXT UNIQUE NOT NULL,
  name TEXT NOT NULL,
  cost_price REAL NOT NULL,
  sale_price REAL NOT NULL,
  product_quantity REAL NOT NULL
);
CREATE TABLE IF NOT EXISTS sales(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  ts TEXT NOT NULL DEFAULT (datetime('now')),
  total REAL NOT NULL,
  profit REAL NOT NULL,
  customer_id INTEGER NOT NULL,
  customer_name TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS sale_items(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  sale_id INTEGER NOT NULL,
  product_id INTEGER NOT NULL,
  qty INTEGER NOT NULL,
  price REAL NOT NULL,
  cost_price REAL NOT NULL
);
CREATE TABLE IF NOT EXISTS customers (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    note TEXT
);
";
            using var cmd = new SQLiteCommand(schema, c);
            cmd.ExecuteNonQuery();
        }

        public static SQLiteConnection Open()
        {
            var x = new SQLiteConnection(ConnStr);
            x.Open();
            return x;
        }
    }
}
