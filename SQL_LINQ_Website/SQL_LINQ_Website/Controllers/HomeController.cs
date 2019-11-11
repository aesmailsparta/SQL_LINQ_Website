using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQL_LINQ_Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult One_One()
        {
            using (var db = new NorthwindEntities())
            {
                var customers = db.Customers.Where(c => c.City.ToLower() == "paris" || c.City.ToLower() == "london").ToList();
                ViewBag.qOneCustomers = customers;

            }

            return View();
        }

        public ActionResult One_Two()
        {
            using (var db = new NorthwindEntities())
            {
                var products = db.Products.Where(p => p.QuantityPerUnit.Contains("bottle")).ToList();
                ViewBag.qTwoProducts = products;
            }

            return View();
        }

        public ActionResult One_Three()
        {
            //using (var db = new NorthwindEntities())
            //{

            //    var products = db.Products.Join(
            //        db.Suppliers, p => p.SupplierID, s => s.SupplierID,
            //        (p, s) => new { p, s }
            //        ).Where(p => p.p.QuantityPerUnit.Contains("bottle")).Select(z => new {z.p,z.s}).ToList();
            //    ViewBag.q = products;
            //}

            using (var db = new NorthwindEntities())
            {

                var productsQ3 = (from p in db.Products
                                  where p.QuantityPerUnit.Contains("bottle")
                                  join s in db.Suppliers on p.SupplierID equals s.SupplierID
                                  select new { p.ProductID, p.ProductName, p.QuantityPerUnit, p.UnitsInStock, p.UnitPrice, s.CompanyName, s.Country }).ToList();

                List<(int ProductID, string ProductName, string QuantityPerUnit, short? UnitsInStock, decimal? UnitPrice, string CompanyName, string Country)> solidProducts =
                        new List<(int ProductID, string ProductName, string QuantityPerUnit, short? UnitsInStock, decimal? UnitPrice, string CompanyName, string Country)>();

                foreach (var item in productsQ3)
                {
                    var product = (item.ProductID,
                        item.ProductName,
                        item.QuantityPerUnit,
                        item.UnitsInStock,
                        item.UnitPrice,
                        item.CompanyName,
                        item.Country
                    );

                    solidProducts.Add(product);
                }

                ViewBag.product = solidProducts;

            }

            return View();

        }

        public ActionResult One_Four()
        {
            using (var db = new NorthwindEntities())
            {

                var Q4Result = (from p in db.Products
                                join c in db.Categories on p.CategoryID equals c.CategoryID
                                group c by c.CategoryName into pc
                                orderby pc.Count() descending
                                select new { CategoryName = pc.Key, ProductCount = pc.Count() }).ToList();

                List<(string CategoryName, int ProductCount)> CategorysList =
                        new List<(string CategoryName, int ProductCount)>();

                foreach (var item in Q4Result)
                {
                    var CategoryInfo = (
                        item.CategoryName,
                        item.ProductCount
                    );

                    CategorysList.Add(CategoryInfo);
                }

                ViewBag.Q4 = CategorysList;

            }

            return View();

        }

        public ActionResult One_Five()
        {

            using (var db = new NorthwindEntities())
            {

                var Q5Result = (from e in db.Employees
                                where e.Country.ToLower() == "uk"
                                select new { Fullname = e.TitleOfCourtesy + " " + e.FirstName + " " + e.LastName, e.City }).ToList();

                List<(string Fullname, string City)> EmployeeList =
                        new List<(string Fullname, string City)>();

                foreach (var item in Q5Result)
                {
                    var Employee = (
                        item.Fullname,
                        item.City
                    );

                    EmployeeList.Add(Employee);
                }

                ViewBag.Q5 = EmployeeList;

            }

            return View();

        }

        public ActionResult One_Six()
        {//SELECT Region.RegionID, Region.RegionDescription, SUM([Order Details].Quantity * [Order Details].UnitPrice) AS 'Total Sales'
         //FROM Territories
         //INNER JOIN EmployeeTerritories
         //ON Territories.TerritoryID = EmployeeTerritories.TerritoryID
         //INNER JOIN Region
         //ON Region.RegionID = Territories.RegionID
         //INNER JOIN Orders
         //ON EmployeeTerritories.EmployeeID = Orders.EmployeeID
         //INNER JOIN[Order Details]
         //ON Orders.OrderID = [Order Details].OrderID
         //GROUP BY Region.RegionID, Region.RegionDescription
         //HAVING SUM([Order Details].Quantity * [Order Details].UnitPrice) > 1000000;

            //using (var db = new NorthwindEntities())
            //{

            //    var Q6Result = (from t in db.Territories
            //                    join r in db.Regions on t.RegionID equals r.RegionID
            //                    join e in db.Employees on
            //                    group c by c.CategoryName into pc
            //                    orderby pc.Count() descending
            //                    select new { CategoryName = pc.Key, ProductCount = pc.Count() }).ToList();

            //    List<(string Fullname, string City)> ReigonList =
            //            new List<(string Fullname, string City)>();

            //    foreach(var item in Q6Result)
            //    {
            //        var RegionSalesInfo = (
            //           "",""

            //        );

            //        ReigonList.Add(RegionSalesInfo);
            //    }

            //    ViewBag.Q5 = ReigonList;

            //}

            return View();

        }

        public ActionResult One_Seven()
        {
            //SELECT COUNT(*) FROM Orders
            //WHERE Freight > 100 AND(ShipCountry = 'USA' OR ShipCountry = 'UK');

            using (var db = new NorthwindEntities())
            {

                var Q7Result = (from o in db.Orders
                                where o.Freight > 100 && (o.ShipCountry.ToLower() == "uk" || o.ShipCountry.ToLower() == "usa")
                                select o.OrderID);

                int NumberOfOrders = Q7Result.Count();

                ViewBag.Q7 = NumberOfOrders;

            }

            return View();

        }

        public ActionResult One_Eight()
        {
            //SELECT TOP 1 OrderID, Discount, ((UnitPrice * Quantity) * Discount) AS 'DiscountApplied' FROM[Order Details]
            //ORDER BY 'DiscountApplied' DESC, Discount DESC

            using (var db = new NorthwindEntities())
            {

                var Q8Result = (from od in db.Order_Details
                                orderby (((float)od.UnitPrice * (float)od.Quantity) * (float)od.Discount) descending, od.Discount descending
                                select new { od.OrderID, od.Discount, DiscountAmount = (((float)od.UnitPrice * (float)od.Quantity) * (float)od.Discount) }).First();

                (int OrderID, float Discount, float DiscountAmount) OrderInformation = (Q8Result.OrderID, Q8Result.Discount, Q8Result.DiscountAmount);


                ViewBag.Q8 = OrderInformation;

            }

            return View();

        }

        public ActionResult Three_One()
        {
            //SELECT Employees.EmployeeID, CONCAT(Employees.FirstName, ' ', Employees.LastName) AS EmployeeName, CONCAT(Managers.FirstName, ' ', Managers.LastName) AS ReportTo
            //FROM Employees
            //INNER JOIN Employees AS Managers
            //ON Employees.ReportsTo = Managers.EmployeeID

            using (var db = new NorthwindEntities())
            {

                var Q3_1Result = (from e in db.Employees
                                  join m in db.Employees on e.ReportsTo equals m.EmployeeID
                                  select new { e.EmployeeID, Fullname = e.FirstName + " " + e.LastName, ReportsTo = m.FirstName + " " + m.LastName }).ToList();

                List<(int EmployeeID, string FullName, string ReportsTo)> EmployeeManagersList =
                        new List<(int EmployeeID, string FullName, string ReportsTo)>();

                foreach (var item in Q3_1Result)
                {
                    var EmployeeManagerInfo = (
                        item.EmployeeID,
                        item.Fullname,
                        item.ReportsTo
                    );

                    EmployeeManagersList.Add(EmployeeManagerInfo);
                }

                ViewBag.Q3_1 = EmployeeManagersList;

            }

            return View();

        }

        public ActionResult Three_Two()
        {
            //SELECT Suppliers.SupplierID, Suppliers.CompanyName, FORMAT(SUM([Order Details].Quantity * [Order Details].UnitPrice * (1 -[Order Details].Discount)), 'C0', 'en-gb') AS 'TotalSales(Including Discount)'
            //FROM[Order Details]
            //JOIN Products
            //ON[Order Details].ProductID = Products.ProductID
            //JOIN Suppliers
            //ON Products.SupplierID = Suppliers.SupplierID
            //GROUP BY Suppliers.SupplierID, Suppliers.CompanyName
            //HAVING SUM([Order Details].Quantity * [Order Details].UnitPrice * (1 -[Order Details].Discount)) > 10000


            using (var db = new NorthwindEntities())
            {

                var Q3_2Result = (from od in db.Order_Details
                                  join p in db.Products on od.ProductID equals p.ProductID
                                  join s in db.Suppliers on p.SupplierID equals s.SupplierID
                                  group od by s.SupplierID into odps
                                  where odps.Sum(grp => (float)grp.Quantity * (float)grp.UnitPrice * (1 - (float)grp.Discount)) > 10000
                                  select new { odps.Key, CompanyName = odps.Select(s => s.Product.Supplier.CompanyName).FirstOrDefault(), TotalSales = odps.Sum(grp => (float)grp.Quantity * (float)grp.UnitPrice * (1 - (float)grp.Discount)) }).ToList();

                List<(int SupplierID, string SupplierName, float TotalSales)> SupplierSalesList =
                        new List<(int SupplierID, string SupplierName, float TotalSales)>();

                foreach (var item in Q3_2Result)
                {
                    var SupplierSalesInfo = (
                        item.Key,
                        item.CompanyName,
                        item.TotalSales
                    );

                    SupplierSalesList.Add(SupplierSalesInfo);
                }

                ViewBag.Q3_2 = SupplierSalesList;

            }

            return View();

        }

        public ActionResult Three_Three()
        {
            // --SELECT TOP 10 Orders.ShippedDate, Customers.ContactName, FORMAT(SUM([Order Details].UnitPrice * [Order Details].Quantity * (1 -[Order Details].Discount)), 'c') AS 'TotalSpent'
            //  --FROM[Order Details]
            //  --JOIN Orders
            //--ON Orders.OrderID = [Order Details].OrderID
            //--JOIN Customers
            //--ON Orders.CustomerID = Customers.CustomerID
            //--GROUP BY Customers.ContactName, Orders.ShippedDate
            //--HAVING Orders.ShippedDate > DATEADD(y, -1, MAX(Orders.ShippedDate))
            //--ORDER BY SUM([Order Details].UnitPrice * [Order Details].Quantity * (1 -[Order Details].Discount)) DESC

            using (var db = new NorthwindEntities())
            {

                DateTime MAXSHIPDATE = (DateTime)db.Orders.Max(o => o.ShippedDate);

                MAXSHIPDATE = MAXSHIPDATE.AddYears(-1);

                var Q3_3Result = (from od in db.Order_Details
                                  join o in db.Orders on od.OrderID equals o.OrderID
                                  join c in db.Customers on o.CustomerID equals c.CustomerID
                                  where o.ShippedDate > MAXSHIPDATE
                                  group od by c.ContactName into odoc
                                  orderby odoc.Sum(grp => (float)grp.UnitPrice * (float)grp.Quantity * (1 - (float)grp.Discount)) descending
                                  select new { odoc.Key, ShippedDate = odoc.Select(s => s.Order.ShippedDate).FirstOrDefault(), TotalSpent = odoc.Sum(grp => (float)grp.UnitPrice * (float)grp.Quantity * (1 - (float)grp.Discount)) }).Take(10).ToList();

                List<(string CustomerName, DateTime? ShippedDate, float TotalSpent)> CustomerSpendList =
                        new List<(string CustomerName, DateTime? ShippedDate, float TotalSpent)>();

                foreach (var item in Q3_3Result)
                {
                    var CustomerSpendInfo = (
                        item.Key,
                        item.ShippedDate,
                        item.TotalSpent
                    );

                    CustomerSpendList.Add(CustomerSpendInfo);
                }

                ViewBag.Q3_3 = CustomerSpendList;

            }

            return View();

        }
    }
}