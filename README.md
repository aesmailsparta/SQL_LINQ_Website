# SQL LINQ Website (C#/ASP.NET)
My first attempt at a ASP.NET MVC website, which outputs the results to a set of Northwind Database queries, using C# and LINQ and Microsofts Northwind Example Database

![alt text](https://raw.githubusercontent.com/aesmailsparta/SQL_LINQ_Website/master/Screenshots/Home.png "SQL LINQ Screenshot - Home")

## Technologies
C#, LINQ  

## Features
* Separate pages for each query
* Each page shows data with an appropriate view depending on query output
* Menu links to each query page
* Bootstrap used for basic styling  


## Code Highlights

#### Overview

###### Home Controller contains all logic/querys for every page.  

#### Linq Query 01 (Lambda)
This is an example of a Linq lambda query within the project, which returns all of the products that are stored in bottles, with the supplier details attached

```c#
var products = db.Products
				.Join(db.Suppliers, product => product.SupplierID, supplier => supplier.SupplierID, (product, supplier) => new { product, supplier })
				.Where(products => products.product.QuantityPerUnit.Contains("bottle"))
				.Select(products => new {products.product,products.supplier}).ToList();
```  

#### Linq Query 02 (Raw)  
This is an example of a raw expanded linq query within the project, which lists out all suppliers which have total sales exceeding $10000. 

```c#
 var Q3_2Result = (from od in db.Order_Details
                  join p in db.Products on od.ProductID equals p.ProductID
                  join s in db.Suppliers on p.SupplierID equals s.SupplierID
                  group od by s.SupplierID into odps
                  where odps.Sum(grp => (float)grp.Quantity * (float)grp.UnitPrice * (1 - (float)grp.Discount)) > 10000
                  select new { odps.Key, CompanyName = odps.Select(s => s.Product.Supplier.CompanyName).FirstOrDefault(), TotalSales = odps.Sum(grp => (float)grp.Quantity * (float)grp.UnitPrice * (1 - (float)grp.Discount)) }).ToList();
```  

## ScreenShots

![alt text](https://raw.githubusercontent.com/aesmailsparta/SQL_LINQ_Website/master/Screenshots/Query.png "SQL LINQ Screenshot - Query")
