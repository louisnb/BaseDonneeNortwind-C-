using LibraryCoreEFNortwind.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryCoreEFNortwind
{
    public class GestionNorthwind
    {
        private northwindContext model = new northwindContext();

        public List<Product> ChargerFilms()
        {
            return model.Products.Include(a => a.ProductId).ToList();
        }

        public Product AjouterProduit(Product product)
        {
            model.Products.Add(product);
            
            if (model.SaveChanges() > 0)
                return product;
            return null;
        }

        public Product RechercherProduit(int id)
        {
            return model.Products.Find(id);
        }

        public List<Product> RechercherNomProduit(string nom)
        {
            var liste = model.Products.Where(p => p.ProductName.StartsWith(nom));
            return liste.ToList();
        }

        public bool ModifierProduit(Product product)
        {
            model.Entry(product).State = EntityState.Modified;
            
            return (model.SaveChanges() > 0);
        }

        public bool SupprimerProduit(int id)
        {
            Product p = RechercherProduit(id);
            if (p == null)
                return false;
            return SupprimerProduit(p);
        }

        public bool SupprimerProduit(Product element)
        {
            if (element != null)
            {
                model.Products.Remove(element);
               
                return (model.SaveChanges() > 0);
            }
            return false;
        }
    }
}
