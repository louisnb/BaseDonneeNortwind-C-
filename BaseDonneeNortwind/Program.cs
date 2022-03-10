using LibraryCoreEFNortwind;
using LibraryCoreEFNortwind.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseDonneeNortwind
{
    class Program
    {
        static void Main(string[] args)
        {
            northwindContext context = new northwindContext();

            Console.WriteLine("Affiche tous les employés embauchés en 1994 habitant à London");
            var liste1 = context.Employees
                .Where(f => f.HireDate >= new DateTime(1994, 01, 01) && f.HireDate < new DateTime(1995, 01, 01) && f.City.Equals("London"));    
            AfficherEmployees(liste1.ToList());
            

            Console.WriteLine("Affiche tous les employés ayant 'Representative' dans leur titre");
            var liste2 = context.Employees.Where(f => f.Title.Contains("Representative"));
            AfficherEmployees(liste2.ToList());


            var moyenne = context.Products.Average(f => f.UnitPrice);
            Console.WriteLine($"Moyenne des produits {moyenne}");

            var liste3 = context.Products.Include(f => f.Category).AsQueryable();
            liste3 = liste3.Where(f => f.Category.CategoryName.Equals("Seafood"));
            AfficherProduit(liste3.ToList());

            


            Console.WriteLine("Affiche les commandes supérieures au 2 mai 1996");
            var liste4 = context.Orders.Where(f => f.OrderDate > new DateTime(1996, 05, 02));
            AfficherOrder(liste4.ToList());

            var liste5 = context.Orders
                .Include(f => f.Orderdetails)
                .Include(f => f.Orderdetails.Union<Product>)
                .AsQueryable();
            liste5 = liste5.Where(f => f.Orderdetails.ProductId == f.Orderdetails.Product.ProductId);
            liste5 = liste5.Where(f => f.Orderdetails.UnitPrice > 230);
            AfficherOrder(liste5.ToList());


            Console.WriteLine("Affiche les produits qui ne sont pas présent dans les OrderDetails");
            var liste6 = context.Products
                .Include(f => f.Orderdetails)
                .AsSplitQuery()
                .AsQueryable();
            liste6 = liste6.Where(f => f.ProductId < 12);
            AfficherProduit(liste6.ToList());

            Console.WriteLine("Affiche tous les OrderDetails dont le prix des produits est inférieur à 20 €, la quantité supérieure à 40 et le Discount est compris entre 0.2 et 0.3 valeurs comprises");
            var liste7 = context.Orderdetails
                .Where(f => f.UnitPrice < 20 && f.Quantity > 40)
                .Where(f => f.Discount >= 0.2 && f.Discount <= 0.3);
            AfficherOrderdetails(liste7.ToList());

            var liste8 = context.Employees
                .Include(f => f.Orders)
                .Include(f => f.Orders.Union<Orderdetail>)
                .AsQueryable();
            liste8 = liste8.Where(f => f.EmployeeId == f.Orders.EmployeeId);
            liste8 = liste8.Where(f => f.Orderdetail.Quantity > 120);
            AfficherEmployees(liste8.ToList());

        }

        static void AfficherProduit(List<Product> liste)
        {
            foreach (var product in liste)
            {
                Console.WriteLine($"{product.UnitPrice}");  
            }
        }

        static void AfficherEmployees(List<Employee> liste)
        {
            foreach (var employee in liste)
            {
                Console.WriteLine($"{employee.FirstName}");
            }
        }

        static void AfficherOrder(List<Order> liste)
        {
            foreach (var order in liste)
            {
                Console.WriteLine($"{order.OrderId}");
            }
        }

        static void AfficherOrderdetails(List<Orderdetail> liste)
        {
            foreach (var orderDetail in liste)
            {
                Console.WriteLine($"{orderDetail.ProductId}");
            }
        }
        
    }
}
