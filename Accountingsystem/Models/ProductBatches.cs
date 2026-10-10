using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class ProductBatches
    {
        /// <summary>
        /// get the producct from batch id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        public static Product getProductbyBatch_Id(int id) {
            var db = new AccountingContainer();
            var batch = db.ProductBatches.Find(id);
            var product = db.Products.Find(batch.Product_ID);
            return product;



        }

        public static void  CreateProductBatch()
        {
            var db = new AccountingContainer();
            var productList=db.Products.Where(a=>a.Prod_Type==Prod_Type.خدمة&&a.ProductBatches.FirstOrDefault()==null).ToList();
            var batchList = new List<ProductBatches>();
            foreach(var prod in productList)
            {
                foreach (var store in db.Stores) {
                    try
                    {
                        var batch = new ProductBatches();
                        batch.Product_ID = prod.Prod_ID;
                        batch.Store_ID = store.Store_ID;
                        batch.Product_Quantity = 1;
                        batch.SalesPrice = prod.Sale_Price;
                        batch.Batch_Date = DateTime.Now;
                        batch.IsActive = true;
                        batchList.Add(batch);
                       
                    }
                    catch (Exception ex)
                    {
                        var a = ex.Message;
                    }


                        }
            }

            try
            {
                if (batchList.Count() > 0)
                {
                    db.ProductBatches.AddRange(batchList);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {

            }
        }





        /// <summary>
        /// get All Product patch save in data base
        /// </summary>
        /// <returns> List<ProductBatches></returns>
        public static List<ProductBatches> getAllProduct()
        {
            var db = new AccountingContainer();
            var productList = db.ProductBatches.ToList();
            return productList;

        }
        public static List<StoreCard> getAllProductByCode(string  recipt)
        {
            var db = new AccountingContainer();
            var productList = db.StoreCards.Where(a=>a.Document_Serial==recipt).ToList();
            return productList;

        }
        public class ProductComparer : EqualityComparer<ProductBatches>
        {/// <summary>
        /// check is that Product is in the same store
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>bool</returns>
            public override bool Equals(ProductBatches x, ProductBatches y)
            {
                return x.Product_ID == y.Product_ID;
            }
            public override int GetHashCode(ProductBatches obj)
            {
                return obj == null ? 0 : obj.Product_ID;
            }
           
        }
    }
}