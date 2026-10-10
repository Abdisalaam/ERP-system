using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Utilites;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Models
{
    public partial class Order
    {
        public static List<CategoeryProductVM> GetAllCategoeryProducts(int categoeryId, string userId)
        {
            var db = new AccountingContainer();

            List<CategoeryProductVM> CategoeryProducts = new List<CategoeryProductVM>();
           
            var employee = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            var storeId = 1;
            if (employee != null)
            {
                var storeEmp = db.StoreEmployees.Where(a => a.Employee_Id == employee.Emp_Id).FirstOrDefault();
                if (storeEmp != null)
                {
                    storeId = storeEmp.Store_ID;
                }
                else
                {
                    storeId = 1;
                }
            }
            else
            {
                storeId = 1;
            }
            var patches = db.ProductBatches.Where(a => a.Store_ID == storeId && a.IsActive != false && a.Product_Quantity > 0 && a.Product.Prod_Type != Prod_Type.اولي).ToList();

            //var patches = db.ProductBatches.Where(s => s.Store_ID == storeId).ToList();
            foreach (var item in patches)
            {
                if (item.Product.Prod_Type != Prod_Type.اولي)
                {
                    if (item.Product.Prod_Type == Prod_Type.منتج)
                    {
                        if (item.Product_Quantity > 0)
                        {
                            if (item.Product.Category_ID != null)
                            {
                                if (item.Product.Category_ID.Value == categoeryId)
                                {
                                    var orderProductVM = new CategoeryProductVM();
                                    orderProductVM.PatchID = item.Batch_ID;
                                    orderProductVM.ProductID = item.Product_ID;
                                    orderProductVM.ProductName = item.Product.Prod_Name;
                                    orderProductVM.ProductCode = item.Product.Bar_Code;
                                    orderProductVM.ProductPrice = item.Product.Sale_Price.Value;
                                    orderProductVM.AvailableQuantity = item.Product_Quantity;
                                    orderProductVM.ProductImage = item.Product.Pro_Img;

                                    CategoeryProducts.Add(orderProductVM);
                                }
                            }
                        }
                    }
                    if (item.Product.Prod_Type == Prod_Type.مجمع)
                    {
                        var productQuantity = true;
                        foreach (var product in item.Product.ProductCompounds)
                        {
                            //var storeId = db.Stores.FirstOrDefault().Store_ID;
                            var productPatch = db.ProductBatches.Where(s => s.Store_ID == storeId && s.Product_ID == product.Product_Id).FirstOrDefault();

                            if (product.Quantity > productPatch.Product_Quantity)
                            {
                                productQuantity = false;
                            }
                        }
                        if (productQuantity)
                        {
                            if (item.Product.Category_ID != null)
                            {
                                if (item.Product.Category_ID.Value == categoeryId)
                                {
                                    var orderProductVM = new CategoeryProductVM();
                                    orderProductVM.PatchID = item.Batch_ID;
                                    orderProductVM.ProductID = item.Product_ID;
                                    orderProductVM.ProductName = item.Product.Prod_Name;
                                    orderProductVM.ProductCode = item.Product.Bar_Code;
                                    orderProductVM.ProductPrice = item.Product.Sale_Price.Value;
                                    orderProductVM.ProductImage = item.Product.Pro_Img;

                                    CategoeryProducts.Add(orderProductVM);
                                }
                            }
                        }
                    }
                    if (item.Product.Prod_Type == Prod_Type.خدمة)
                    {
                        if (item.Product.Category_ID != null)
                        {
                            if (item.Product.Category_ID.Value == categoeryId)
                            {
                                var orderProductVM = new CategoeryProductVM();
                                orderProductVM.PatchID = item.Batch_ID;
                                orderProductVM.ProductID = item.Product_ID;
                                orderProductVM.ProductName = item.Product.Prod_Name;
                                orderProductVM.ProductCode = item.Product.Bar_Code;
                                orderProductVM.ProductPrice = item.Product.Sale_Price.Value;
                                orderProductVM.ProductImage = item.Product.Pro_Img;

                                CategoeryProducts.Add(orderProductVM);
                            }
                        }
                    }


                }
            }
            return CategoeryProducts;
        }

        public static bool EndSelling()
        {
            try
            {
                var db = new AccountingContainer();
                var openCachierAndClose = db.StartAndEndSellings.FirstOrDefault();                  
                openCachierAndClose.EndDateAndTime = DateTime.Now;
                openCachierAndClose.OrderNumber = 0;
                db.Entry(openCachierAndClose).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool StartSelling()
        {
            try
            {
                var db = new AccountingContainer();
                var openCachierAndClose = db.StartAndEndSellings.FirstOrDefault();
                if (openCachierAndClose == null)
                {
                    StartAndEndSelling cachierOpenAndCloseShift = new StartAndEndSelling();
                    cachierOpenAndCloseShift.OrderNumber = 0;
                    cachierOpenAndCloseShift.StartingDateAndTime = DateTime.Now;
                    db.StartAndEndSellings.Add(cachierOpenAndCloseShift);
                    db.SaveChanges();
                    return true;
                }
                openCachierAndClose.OrderNumber = 0;
                openCachierAndClose.StartingDateAndTime = DateTime.Now;
                db.Entry(openCachierAndClose).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
          

        }

        public static OrderVM GetReverseOrderVMById(int id)
        {
            var db = new AccountingContainer();
            var reverseOrder = db.ReverseOrders.Find(id);
            if (reverseOrder != null)
            {
                OrderVM orderVM = new OrderVM();
                var paymentMethode = db.PaymentMethods.Find(reverseOrder.PaymentMethodeID);
                orderVM.ExtraTaxName = "";
                if (reverseOrder.Order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(reverseOrder.Order.SelectedExtraTaxId);
                    orderVM.ExtraTaxName = extraTax.Tax_Name;
                    orderVM.TotalExtraTaxAmount = (reverseOrder.Total_Amount * extraTax.Tax_Percentage) / 100;
                }
                //orderVM.TotalExtraTaxAmount = reverseOrder.Order.TotalNewTaxs;
                orderVM.DicountAmount = 0;
                orderVM.DicountPrecentage = 0;
                orderVM.OrderDetailsVM = new List<OrderDetailsVM>();
                foreach (var orderDetail in reverseOrder.ReverseOrderDetails)
                {
                    var product = db.Products.Find(orderDetail.ProductID);
                    OrderDetailsVM orderDetailsVM = new OrderDetailsVM();
                    orderDetailsVM.Id = orderDetail.Id;
                    orderDetailsVM.PatchId = orderDetail.PatchId;
                    orderDetailsVM.ProductID = orderDetail.ProductID;
                    orderDetailsVM.ProductName = product.Prod_Name;
                    orderDetailsVM.ProductPrice = orderDetail.Product_Price;
                    orderDetailsVM.Quantity = orderDetail.Quantity;
                    orderDetailsVM.ProductTaxAmount = orderDetail.Detail_Total_Tax;
                    orderDetailsVM.SubTotalProduct = orderDetail.Total_Amount;
                    orderVM.OrderDetailsVM.Add(orderDetailsVM);
                    if (orderDetail.TaxId > 0)
                    {
                        var productTax = db.Taxes.Find(orderDetail.TaxId);
                        orderVM.ProductTaxName = productTax.Tax_Name;
                        orderVM.TotalProductsTaxAmount += orderDetail.Detail_Total_Tax;
                    }
                    if (reverseOrder.DicountInPrecentage > 0)
                    {
                        orderVM.TotalDiscountAmount += (orderDetail.Product_Price * orderDetail.Quantity) / reverseOrder.DicountInPrecentage;
                        orderVM.DicountPrecentage = reverseOrder.DicountInPrecentage;
                    }
                    orderVM.TotaBeforeDiscountAndTax += orderDetail.Product_Price * orderDetail.Quantity;

                }
                if (reverseOrder.CustomerID > 0)
                {
                    var customer = db.Customers.Find(reverseOrder.CustomerID);
                    orderVM.Customer = customer;

                }

                if (reverseOrder.CustomerID > 0)
                {
                    orderVM.Customer = db.Customers.Find(reverseOrder.CustomerID);

                }
                var employee = db.Employees.Find(reverseOrder.EmployeeID);
                orderVM.Id = reverseOrder.Id;
                orderVM.OrderId = reverseOrder.OrderId;
                orderVM.Number = reverseOrder.Number;
                orderVM.OrderNumber = reverseOrder.OrderNumber;
                orderVM.Invoice_State = reverseOrder.Invoice_State;
                orderVM.PaymentMethodeName = paymentMethode.Payment_Type;
                orderVM.EmployeeName = employee.Emp_Name;
                orderVM.OrderDate = reverseOrder.ReverseOrderDate;
                orderVM.TotalNet = reverseOrder.Total_Amount + orderVM.TotalExtraTaxAmount;
                orderVM.TotalAfterDiscount = orderVM.TotaBeforeDiscountAndTax - orderVM.TotalDiscountAmount;
                return orderVM;
            }

            return null;
        }
        public static OrderVM GetOrderVMById(int id)
        {
            var db = new AccountingContainer();
            var order = db.Orders.Find(id);
            if (order != null)
            {
                OrderVM orderVM = new OrderVM();
                orderVM.OrderNumberCount = order.OrderCountNumber.ToString();

                orderVM.OrderId = order.Id;
                orderVM.Number = order.Number;
                orderVM.Id = order.Id;
                orderVM.CustomeId = order.CustomerID.Value;
                orderVM.EmployeeID = order.EmployeeID;
                orderVM.OrderType = order.OrderType;
                var paymentMethode = db.PaymentMethods.Find(order.PaymentMethodeID);
                orderVM.ExtraTaxName = "";
                orderVM.ReverseOrders = order.ReverseOrders.ToList();
                if (order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(order.SelectedExtraTaxId);
                    orderVM.ExtraTaxName = extraTax.Tax_Name;
                }
                orderVM.TotalExtraTaxAmount = order.TotalNewTaxs;
                orderVM.DicountAmount = 0;
                orderVM.DicountPrecentage = 0;
                orderVM.Invoice_State = order.Invoice_State;
                if (order.QRNumber != null)
                {
                    orderVM.QRNumber = order.QRNumber.Value;
                }
                orderVM.OrderDetailsVM = new List<OrderDetailsVM>();
                decimal _taxAmount = 0;
                foreach (var orderDetail in order.OrderDetails)
                {
                    var product = db.Products.Find(orderDetail.ProductID);
                    OrderDetailsVM orderDetailsVM = new OrderDetailsVM();
                    orderDetailsVM.Id = orderDetail.Id;
                    orderDetailsVM.PatchId = orderDetail.PatchId;
                    orderDetailsVM.ProductID = orderDetail.ProductID;
                    orderDetailsVM.ProductName = product.Prod_Name;
                    orderDetailsVM.AdditionalDetail = orderDetail.AdditionalDetail;
                    orderDetailsVM.ProductPrice = orderDetail.Product_Price;
                    orderDetailsVM.Quantity = orderDetail.Quantity;
                    orderDetailsVM.ProductTaxAmount = orderDetail.Detail_Total_Tax;
                    orderDetailsVM.SubTotalProduct = orderDetail.Total_Amount;
                    orderVM.OrderDetailsVM.Add(orderDetailsVM);
                    if (orderDetail.TaxId > 0)
                    {
                        var productTax = db.Taxes.Find(orderDetail.TaxId);
                         _taxAmount = (orderDetailsVM.ProductPrice * productTax.Tax_Percentage) / 100;
                        orderVM.ProductTaxName = productTax.Tax_Name;
                        orderVM.TotalProductsTaxAmount += orderDetail.Detail_Total_Tax;
                        orderDetailsVM.ProductPrice = orderDetailsVM.ProductPrice + _taxAmount;
                    }

                    orderVM.TotaBeforeDiscountAndTax += orderDetail.Product_Price * orderDetail.Quantity;

                }
                orderVM.TotalDiscountAmount = orderVM.TotaBeforeDiscountAndTax * (order.DicountInPrecentage / 100);
                orderVM.DicountPrecentage = order.DicountInPrecentage;

                if (order.CustomerID > 0)
                {
                    var customer = db.Customers.Find(order.CustomerID);
                    orderVM.Customer = customer;

                }

                var employee = db.Employees.Find(order.EmployeeID);
                orderVM.Id = order.Id;
                orderVM.OrderNumber = order.OrderNumber;
                orderVM.Comment = order.Comment;
                orderVM.PaymentMethodeName = paymentMethode.Payment_Type;
                orderVM.EmployeeName = employee.Emp_Name;
                orderVM.OrderDate = order.OrderDate;
                orderVM.TotalNet = order.Total_Amount + order.TotalNewTaxs;
                orderVM.TotalAfterDiscount = orderVM.TotaBeforeDiscountAndTax - orderVM.TotalDiscountAmount;
                return orderVM;
            }

            return null;
        }

        public static ReverseOrder GetReversdeOrderById(int reversdeOrderId)
        {
            var db = new AccountingContainer();
            var reversdeOrder = db.ReverseOrders.Find(reversdeOrderId);
            return reversdeOrder;
        }

        public static int ReversOrder(OrderVM orderVM)
        {
            var db = new AccountingContainer();
            var order = db.Orders.Find(orderVM.OrderId);
            var tempOrder = db.TempOrders.Find(orderVM.TempOrderId);
            if (order != null)
            {
                if (tempOrder != null)
                {
                    if (tempOrder.TempOrderDetails.Any())
                    {

                        ReverseOrder reverseOrder = new ReverseOrder();
                        var lastOrderNumber = db.ReverseOrders.OrderByDescending(c => c.Id).FirstOrDefault();
                        //if (lastOrderNumber == null)
                        //{
                        //    reverseOrder.OrderNumber = "RORD-001";
                        //}
                        //else
                        //{
                        //    reverseOrder.OrderNumber = "RORD-" + (Convert.ToInt32(lastOrderNumber.OrderNumber.Substring(5, lastOrderNumber.OrderNumber.Length - 5)) + 1).ToString("D3");
                        //}
                        reverseOrder.EmployeeID = order.EmployeeID;
                        reverseOrder.DicountInPrecentage = order.DicountInPrecentage;
                        reverseOrder.ReverseOrderDate = DateTime.Now;
                        reverseOrder.CustomerID = order.CustomerID;
                        var paymentMethode = db.PaymentMethods.Find(orderVM.SelectedPaymentMethodeID);
                        reverseOrder.PaymentMethodeID = paymentMethode.PaymentMethod_Id;
                        foreach (var item in tempOrder.TempOrderDetails)
                        {
                            var patch = db.ProductBatches.Find(item.PatchId);
                            decimal taxAmount = 0;
                            ReverseOrderDetail reverseOrderDetail = new ReverseOrderDetail();
                            reverseOrderDetail.PatchId = patch.Batch_ID;
                            reverseOrderDetail.ProductID = patch.Product.Prod_ID;
                            reverseOrderDetail.TaxId = 0;
                            reverseOrderDetail.Detail_Total_Tax = 0;
                            reverseOrderDetail.Quantity = item.Quantity;
                            reverseOrderDetail.Product_Price = item.ProductPrice;
                            decimal TotalBeforeDiscount = reverseOrderDetail.Product_Price * reverseOrderDetail.Quantity;
                            decimal discount = TotalBeforeDiscount * (order.DicountInPrecentage / 100);
                            decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                            if (item.TaxID > 0)
                            {
                                var tax = db.Taxes.Find(item.TaxID);
                                reverseOrderDetail.TaxId = tax.Tax_ID;
                                taxAmount = TotalAfterDiscount * (tax.Tax_Percentage / 100);
                                reverseOrderDetail.Detail_Total_Tax = taxAmount;
                            }
                            reverseOrderDetail.Total_Amount = TotalAfterDiscount + reverseOrderDetail.Detail_Total_Tax;
                            reverseOrderDetail.Product_Cost = patch.Product_Cost;
                            reverseOrderDetail.Detail_Total_Cost = 0;
                            reverseOrder.Total_Amount += reverseOrderDetail.Total_Amount;
                            reverseOrder.Total_Tax += reverseOrderDetail.Detail_Total_Tax;
                            reverseOrder.Total_Cost += reverseOrderDetail.Detail_Total_Cost;
                            reverseOrder.ReverseOrderDetails.Add(reverseOrderDetail);
                        }

                        reverseOrder.Invoice_State = TransactionState.Post;
                        reverseOrder.HasOff = false;
                        reverseOrder.OrderId = order.Id;
                        reverseOrder.Number = InvoiceSerial.generateSerialForReverse();

                        reverseOrder.OrderNumber = "RORD-" + System.Text.RegularExpressions.Regex.Match(reverseOrder.Number, @"\d+").Value;
                        db.ReverseOrders.Add(reverseOrder);
                        db.TempOrderDetails.RemoveRange(tempOrder.TempOrderDetails);
                        db.TempOrders.Remove(tempOrder);
                        db.SaveChanges();
                        MakeSalesinQrSales(reverseOrder);
                        CreateReverseInvoiceGL(reverseOrder);
                        return reverseOrder.Id;
                    }

                }

            }
            throw new NotImplementedException();
        }

        public static int AddAdditionalDetailToOrderDetail(int selectedAdditionalDetailId, int orderDetailId)
        {
            var db = new AccountingContainer();
            var tempOrderDetail = db.TempOrderDetails.Find(orderDetailId);
            var additionalDetail = db.AdditionalOrderDetails.Find(selectedAdditionalDetailId);
           

            tempOrderDetail.AdditionalDetail += " _ "  + additionalDetail.AdditionalDetail;
            db.Entry(tempOrderDetail).State = EntityState.Modified;
            db.SaveChanges();
            return tempOrderDetail.TempOrder.Id;
        }

        public static AdditionalOrderDetailVM PrepareAdditionalOrderDetail(int orderDetailId)
        {
            var db = new AccountingContainer();
            AdditionalOrderDetailVM additionalOrderDetailVM = new AdditionalOrderDetailVM();
            additionalOrderDetailVM.OrderDetailId = orderDetailId;
            additionalOrderDetailVM.AdditionalDetailList = new SelectList(db.AdditionalOrderDetails, "Id", "AdditionalDetail");
            return additionalOrderDetailVM;

        }

        public static OrderVM GetOrderVMByIdForReverse(int id)
        {
            var db = new AccountingContainer();
            var order = db.Orders.Find(id);
            if (order != null&&order.Invoice_State==TransactionState.Post)
            {
                var paymentMethodes = db.PaymentMethods.ToList();
                TempOrder tempOrder = new TempOrder();
                tempOrder.OrderNumber = order.OrderNumber;
                tempOrder.DicountInPrecentage = order.DicountInPrecentage;
                tempOrder.SelectedExtraTaxId = 0;
                OrderVM orderVM = new OrderVM();
                orderVM.OrderId = order.Id;
                var paymentMethode = db.PaymentMethods.Find(order.PaymentMethodeID);
                orderVM.ExtraTaxName = "";
                if (order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(order.SelectedExtraTaxId);
                    orderVM.ExtraTaxName = extraTax.Tax_Name;
                    tempOrder.SelectedExtraTaxId = extraTax.Tax_ID;
                }
                orderVM.TotalExtraTaxAmount = order.TotalNewTaxs;
                orderVM.DicountAmount = 0;
                orderVM.DicountPrecentage = 0;
                orderVM.OrderDetailsVM = new List<OrderDetailsVM>();
                foreach (var orderDetail in order.OrderDetails)
                {
                    var product = db.Products.Find(orderDetail.ProductID);
                    OrderDetailsVM orderDetailsVM = new OrderDetailsVM();
                    orderDetailsVM.Id = orderDetail.Id;
                    orderDetailsVM.PatchId = orderDetail.PatchId;
                    orderDetailsVM.ProductID = orderDetail.ProductID;
                    orderDetailsVM.ProductName = product.Prod_Name;
                    orderDetailsVM.ProductPrice = orderDetail.Product_Price;
                    orderDetailsVM.Quantity = orderDetail.Quantity;
                    orderVM.TotaBeforeDiscountAndTax += orderDetail.Product_Price * orderDetail.Quantity;
                    orderDetailsVM.ProductTaxAmount = orderDetail.Detail_Total_Tax;
                    orderDetailsVM.SubTotalProduct = orderDetail.Total_Amount;
                    orderVM.OrderDetailsVM.Add(orderDetailsVM);

                    if (orderDetail.TaxId > 0)
                    {
                        var productTax = db.Taxes.Find(orderDetail.TaxId);
                        orderVM.ProductTaxName = productTax.Tax_Name;
                        orderVM.TotalProductsTaxAmount += orderDetail.Detail_Total_Tax;
                    }
                    else
                    {
                        orderVM.ProductTaxName = "";
                        orderVM.TotalProductsTaxAmount += 0;
                    }



                }
                if (order.CustomerID > 0)
                {
                    var customer = db.Customers.Find(order.CustomerID);
                    orderVM.Customer = customer;

                }

                var employee = db.Employees.Find(order.EmployeeID);



                orderVM.DicountPrecentage = order.DicountInPrecentage;
                orderVM.TotalDiscountAmount = orderVM.TotaBeforeDiscountAndTax * (order.DicountInPrecentage / 100);

                orderVM.OrderNumber = order.OrderNumber;

                orderVM.PaymentMethodeName = paymentMethode.Payment_Type;
                orderVM.MyPaymentMethodesList = new SelectList(paymentMethodes.Where(a=>a.IsActive!=false), "PaymentMethod_Id", "Payment_Type");
                orderVM.SelectedPaymentMethodeID = paymentMethode.PaymentMethod_Id;
                orderVM.EmployeeName = employee.Emp_Name;
                orderVM.OrderDate = order.OrderDate;
                orderVM.Comment = order.Comment;
                orderVM.TotalNet = order.Total_Amount + order.TotalNewTaxs;
                orderVM.TotalAfterDiscount = orderVM.TotaBeforeDiscountAndTax - orderVM.TotalDiscountAmount;
                db.TempOrders.Add(tempOrder);
                db.SaveChanges();
                orderVM.Id = tempOrder.Id;
                orderVM.TempOrderId = tempOrder.Id;
                return orderVM;
            }

            return null;
        }
        public static List<OrderVM> GetAllReverseOrders()
        {
            var db = new AccountingContainer();
            var orders = db.ReverseOrders.OrderByDescending(e => e.Id).ToList();
            List<OrderVM> OrderVmList = new List<OrderVM>();
            foreach (var order in orders)
            {
                OrderVM orderVM = new OrderVM();
                var paymentMethode = db.PaymentMethods.Find(order.PaymentMethodeID);
                orderVM.OrignalOrderNumber = order.Order.OrderNumber;
                orderVM.DicountAmount = 0;
                foreach (var orderDetail in order.ReverseOrderDetails)
                {
                    if (orderDetail.TaxId > 0)
                    {
                        var productTax = db.Taxes.Find(orderDetail.TaxId);
                        orderVM.ProductTaxName = productTax.Tax_Name;
                        orderVM.TotalProductsTaxAmount += orderDetail.Detail_Total_Tax;
                    }
                    if (order.DicountInPrecentage > 0)
                    {
                        orderVM.TotalDiscountAmount += (orderDetail.Product_Price * orderDetail.Quantity) / order.DicountInPrecentage;
                    }

                }

                var employee = db.Employees.Find(order.EmployeeID);
                orderVM.Id = order.Id;
                orderVM.OrderId = order.OrderId;
                orderVM.OrderNumber = order.OrderNumber;
                if (order.CustomerID != 0)
                {
                    var customer = db.Customers.Find(order.CustomerID);
                    orderVM.CustomerName = customer.Customer_Name;
                }
                orderVM.PaymentMethodeName = paymentMethode.Payment_Type;
                orderVM.EmployeeName = employee.Emp_Name;
                orderVM.OrderDate = order.ReverseOrderDate;
                orderVM.TotalNet = order.Total_Amount;
                orderVM.TotalTax = order.Total_Tax;
                orderVM.TotalCost = order.Total_Cost;
                orderVM.Invoice_State = order.Invoice_State;
                orderVM.TotaBeforeDiscountAndTax = orderVM.TotalNet - orderVM.TotalDiscountAmount - orderVM.TotalProductsTaxAmount;
                OrderVmList.Add(orderVM);
            }
            return OrderVmList;
        }
        public static List<Order> GetLastHunderdOrders()
        {
            var db = new AccountingContainer();
            // get orders from the starting of selling date
            var opendShift = db.StartAndEndSellings.FirstOrDefault();
            if (opendShift != null && opendShift.StartingDateAndTime != null)
            {
                var orders = db.Orders.Where(s => s.Invoice_State != TransactionState.Void && s.OrderDate >= opendShift.StartingDateAndTime).OrderByDescending(e => e.Id).ToList();
                return orders;
            }
            else
            {
                var orders = db.Orders.Where(s => s.Invoice_State != TransactionState.Void).OrderByDescending(e => e.Id).Take(100).ToList();
                return orders;

            }
            //var orders = db.Orders.OrderByDescending(e => e.Id).Take(100).ToList();

           
        }

        //public static List<OrderVM> GetLastHunderdOrders()
        //{
        //    var db = new AccountingContainer();
        //    var orders = db.Orders.Where(s => s.Invoice_State != TransactionState.Void).OrderByDescending(e => e.Id).Take(100).ToList();
        //    //var orders = db.Orders.OrderByDescending(e => e.Id).Take(100).ToList();
        //    List<OrderVM> OrderVmList = new List<OrderVM>();
        //    foreach (var order in orders)
        //    {
        //        OrderVM orderVM = new OrderVM();
        //        var paymentMethode = db.PaymentMethods.Find(order.PaymentMethodeID);
        //        orderVM.ExtraTaxName = "";
        //        if (order.SelectedExtraTaxId > 0)
        //        {
        //            var extraTax = db.Taxes.Find(order.SelectedExtraTaxId);
        //            orderVM.ExtraTaxName = extraTax.Tax_Name;
        //        }
        //        orderVM.TotalExtraTaxAmount = order.TotalNewTaxs;
        //        orderVM.DicountAmount = 0;
        //        foreach (var orderDetail in order.OrderDetails)
        //        {
        //            if (orderDetail.TaxId > 0)
        //            {
        //                var productTax = db.Taxes.Find(orderDetail.TaxId);
        //                orderVM.ProductTaxName = productTax.Tax_Name;
        //                orderVM.TotalProductsTaxAmount += orderDetail.Detail_Total_Tax;
        //            }
        //            if (order.DicountInPrecentage > 0)
        //            {
        //                orderVM.TotalDiscountAmount += (orderDetail.Product_Price * orderDetail.Quantity) / order.DicountInPrecentage;
        //            }

        //        }

        //        var employee = db.Employees.Find(order.EmployeeID);
        //        orderVM.Id = order.Id;
        //        orderVM.OrderNumber = order.OrderNumber;
        //        if (order.CustomerID != 0)
        //        {
        //            var customer = db.Customers.Find(order.CustomerID);
        //            orderVM.CustomerName = customer.Customer_Name;
        //        }
        //        orderVM.PaymentMethodeName = paymentMethode.Payment_Type;
        //        orderVM.EmployeeName = employee.Emp_Name;
        //        orderVM.OrderDate = order.OrderDate;
        //        orderVM.Invoice_State = order.Invoice_State;
        //        orderVM.TotaBeforeDiscountAndTax = order.Total_Amount - order.Total_Tax;
        //        orderVM.TotalNet = order.Total_Amount + order.TotalNewTaxs;
        //        OrderVmList.Add(orderVM);
        //    }
        //    return OrderVmList;
        //}
        public static List<OrderVM> GetAllOrders(string user_Id)
        {
            var db = new AccountingContainer();
            var Emp = db.Employees.Where(a => a.Reg_ID == user_Id).FirstOrDefault();
            var orders = db.Orders.OrderByDescending(e => e.Id).ToList();
            if (Emp != null && Emp.SystemRoleId != 6)
            {

                orders = orders.Where(a => a.EmployeeID == Emp.Emp_Id).ToList();
            }


               
            List<OrderVM> OrderVmList = new List<OrderVM>();
            foreach (var order in orders)
            {
                OrderVM orderVM = new OrderVM();
                var paymentMethode = db.PaymentMethods.Find(order.PaymentMethodeID);
                orderVM.ExtraTaxName = "";
                if (order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(order.SelectedExtraTaxId);
                    orderVM.ExtraTaxName = extraTax.Tax_Name;
                }
                orderVM.EmployeeID = order.EmployeeID;
                orderVM.StoreId = order.StoreId;
                orderVM.TotalExtraTaxAmount = order.TotalNewTaxs;
                foreach (var orderDetail in order.OrderDetails)
                {
                    if (orderDetail.TaxId > 0)
                    {
                        var productTax = db.Taxes.Find(orderDetail.TaxId);
                        orderVM.ProductTaxName = productTax.Tax_Name;
                        orderVM.TotalProductsTaxAmount += orderDetail.Detail_Total_Tax;
                    }
                    if (order.DicountInPrecentage > 0)
                    {

                        orderVM.TotalDiscountAmount += (orderDetail.Product_Price * orderDetail.Quantity) / order.DicountInPrecentage;
                    }
                    orderVM.TotaBeforeDiscountAndTax += orderDetail.Product_Price * orderDetail.Quantity;

                }

                var employee = db.Employees.Find(order.EmployeeID);
                orderVM.Id = order.Id;
                orderVM.OrderNumber = order.OrderNumber;
                if (order.CustomerID != 0)
                {
                    var customer = db.Customers.Find(order.CustomerID);
                    orderVM.CustomerName = customer.Customer_Name;
                }
                orderVM.PaymentMethodeName = paymentMethode.Payment_Type;
                orderVM.EmployeeName = employee.Emp_Name;
                orderVM.OrderDate = order.OrderDate;
                orderVM.TotalNet = order.Total_Amount + order.TotalNewTaxs;
                orderVM.Invoice_State = order.Invoice_State;

                OrderVmList.Add(orderVM);
            }
            return OrderVmList;
        }

        public static List<Order> GetAllOrders(string user_Id, bool differentInvoice)
        {
            var db = new AccountingContainer();
            var List = new List<Order> { };
            var Emp = db.Employees.Where(a => a.Reg_ID == user_Id).FirstOrDefault();

            if (Emp != null && Emp.SystemRoleId != 6)
            {

                List = db.Orders.Where(a => a.EmployeeID == Emp.Emp_Id).ToList();


         
            }
            else
            {

                List = db.Orders.ToList();
            }


            return List;



        }


        public static List<OrderVM> GetAllOrdersNoExeption()
        {
            var db = new AccountingContainer();
            var orders = db.Orders.OrderByDescending(e => e.Id).ToList();
            List<OrderVM> OrderVmList = new List<OrderVM>();
            foreach (var order in orders)
            {
                OrderVM orderVM = new OrderVM();
                var paymentMethode = db.PaymentMethods.Find(order.PaymentMethodeID);
                orderVM.ExtraTaxName = "";
                if (order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(order.SelectedExtraTaxId);
                    orderVM.ExtraTaxName = extraTax.Tax_Name;
                }
                orderVM.EmployeeID = order.EmployeeID;
                orderVM.StoreId = order.StoreId;
                orderVM.TotalExtraTaxAmount = order.TotalNewTaxs;
                foreach (var orderDetail in order.OrderDetails)
                {
                    if (orderDetail.TaxId > 0)
                    {
                        var productTax = db.Taxes.Find(orderDetail.TaxId);
                        orderVM.ProductTaxName = productTax.Tax_Name;
                        orderVM.TotalProductsTaxAmount += orderDetail.Detail_Total_Tax;
                    }
                    if (order.DicountInPrecentage > 0)
                    {

                        orderVM.TotalDiscountAmount += (orderDetail.Product_Price * orderDetail.Quantity) / order.DicountInPrecentage;
                    }
                    orderVM.TotaBeforeDiscountAndTax += orderDetail.Product_Price * orderDetail.Quantity;

                }

                var employee = db.Employees.Find(order.EmployeeID);
                orderVM.Id = order.Id;
                orderVM.OrderNumber = order.OrderNumber;
                if (order.CustomerID != 0)
                {
                    var customer = db.Customers.Find(order.CustomerID);
                    orderVM.CustomerName = customer.Customer_Name;
                }
                orderVM.PaymentMethodeName = paymentMethode.Payment_Type;
                orderVM.EmployeeName = employee.Emp_Name;
                orderVM.OrderDate = order.OrderDate;
                orderVM.TotalNet = order.Total_Amount + order.TotalNewTaxs;


                OrderVmList.Add(orderVM);
            }
            return OrderVmList;
        }

        public static Order AddOrderNewOrder(OrderVM orderVM)
        {
            var db = new AccountingContainer();
            var order = new Order();
            var orderType = db.OrderTypes.Find(orderVM.SelectedTypeOrderId);
            var tempOrder = db.TempOrders.Find(orderVM.Id);
            if (tempOrder != null)
            {
                if (tempOrder.TempOrderDetails.Any())
                {
                    var lastOrderNumber = db.Orders.OrderByDescending(c => c.Id).FirstOrDefault();
                    if (lastOrderNumber == null)
                    {
                        order.OrderNumber = "ORD-001";
                    }
                    else
                    {
                        order.OrderNumber = "ORD-" + (Convert.ToInt32(lastOrderNumber.OrderNumber.Substring(4, lastOrderNumber.OrderNumber.Length - 4)) + 1).ToString("D3");
                    }
                    order.EmployeeID = orderVM.EmployeeID;
                    order.OrderTypeId = orderType.Id;
                    order.OrderType = orderType.Name;
                    order.Compleeted = true;
                    order.OrderDate = DateTime.Now;
                    order.CustomerID = orderVM.SelectedCustomer_Id;
                    order.StoreId = orderVM.StoreId;
                    order.PaymentMethodeID = orderVM.SelectedPaymentMethodeID;
                    order.Comment = orderVM.Comment;
                    order.TotalNewTaxs = 0;
                    order.Total_Cost = 0;
                    decimal extraTaxAmount = 0;
                    var qrMaster = new MasterQRInvoice();
                    qrMaster.QrGuid = Guid.NewGuid();
                    qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();
                    qrMaster.QRPaymentMethods = new List<QRPaymentMethod>();
                    qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();
                    foreach (var tempOrderDetails in tempOrder.TempOrderDetails.ToList())
                    {
                        var patch = db.ProductBatches.Find(tempOrderDetails.PatchId);
                        decimal taxAmount = 0;
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.PatchId = tempOrderDetails.PatchId;
                        orderDetail.ProductID = tempOrderDetails.ProductID;
                        orderDetail.Quantity = tempOrderDetails.Quantity;
                        orderDetail.Product_Price = tempOrderDetails.ProductPrice;
                        orderDetail.Product_Cost = patch.Product_Cost;
                        orderDetail.AdditionalDetail = tempOrderDetails.AdditionalDetail;
                        orderDetail.Detail_Total_Cost = patch.Product_Cost * tempOrderDetails.Quantity;
                        orderDetail.Detail_Total_Tax = 0;
                        orderDetail.TaxId = 0;
                        decimal TotalBeforeDiscount = tempOrderDetails.ProductPrice * tempOrderDetails.Quantity;
                        decimal discount = (TotalBeforeDiscount * orderVM.DicountPrecentage) / 100;
                        decimal TotalAfterDiscount = (tempOrderDetails.ProductPrice * tempOrderDetails.Quantity) - discount;
                        if (tempOrderDetails.TaxID > 0)
                        {
                            var tax = db.Taxes.Find(tempOrderDetails.TaxID);
                            orderDetail.TaxId = tax.Tax_ID;
                            //taxAmount = (orderDetail.Product_Price * orderDetail.Quantity) / tax.Tax_Percentage;
                            taxAmount = (TotalAfterDiscount * tax.Tax_Percentage) / 100;
                            orderDetail.Detail_Total_Tax = taxAmount;

                        }

                        //if (tempOrder.DicountInPrecentage > 0)
                        //{
                        //    discountAmount = (orderDetail.Product_Price * orderDetail.Quantity) / tempOrder.DicountInPrecentage;
                        //}

                        orderDetail.Total_Amount = TotalAfterDiscount + orderDetail.Detail_Total_Tax;
                        //orderDetail.Total_Amount = (orderDetail.Product_Price * orderDetail.Quantity) - discountAmount;
                        //orderDetail.Total_Amount += taxAmount;
                        orderDetail.Reverse_Quantity = orderDetail.Quantity;
                        orderDetail.Reverse_Total_Tax = orderDetail.Detail_Total_Tax;
                        orderDetail.Reverse_Total_Amount = orderDetail.Total_Amount;
                        order.Total_Amount += orderDetail.Total_Amount;
                        order.Total_Tax += orderDetail.Detail_Total_Tax;
                        order.Total_Cost += order.Total_Cost;
                        order.OrderDetails.Add(orderDetail);
                        ///QR
                        ///
                        ///QR Detail Master
                        ///
                        var invoiceQRDetail = new DetailQRInvoice();
                        invoiceQRDetail.Batch_ID = orderDetail.PatchId;
                        invoiceQRDetail.Product_Id = orderDetail.ProductID;
                        invoiceQRDetail.Product_Quantity = orderDetail.Quantity;
                        invoiceQRDetail.Product_Tax = orderDetail.TaxId;


                        invoiceQRDetail.Detail_Total_Cost = orderDetail.Detail_Total_Cost;

                        invoiceQRDetail.Total_Amount = orderDetail.Total_Amount;

                        invoiceQRDetail.Product_Price = orderDetail.Product_Price;
                        invoiceQRDetail.Detail_Total_Tax = orderDetail.Detail_Total_Tax;
                        invoiceQRDetail.ProductPriceCost = orderDetail.Product_Cost;
                        qrMaster.DetailQRInvoices.Add(invoiceQRDetail);


                    }
                    var realdetails = getMaterialOCompoundProduct(order.OrderDetails.ToList(), order.StoreId.Value);
                    foreach (var item in realdetails)
                    {
                        var productBatch = db.ProductBatches.Find(item.PatchId);
                        if (productBatch.Product_Quantity < item.Quantity && (productBatch.Product.Prod_Type == Prod_Type.منتج || productBatch.Product.Prod_Type == Prod_Type.اولي))
                        {
                            var productName = db.Products.Find(item.ProductID).Prod_Name;

                            var msg = Accountingsystem.Resources.MasterPOSInvoice.Thereisnotenoughproduct + " " + productName;
                            return null;

                        }

                    }


                    if (orderVM.SelectedTaxID > 0)
                    {
                        var extraTax = db.Taxes.Find(orderVM.SelectedTaxID);
                        extraTaxAmount = (order.Total_Amount * extraTax.Tax_Percentage) / 100;
                        order.SelectedExtraTaxId = orderVM.SelectedTaxID;
                        var qrtax = new TaxQRInvoice();
                        qrtax.Tax_Id = orderVM.SelectedTaxID.ToString();

                        qrtax.Amount = extraTaxAmount;
                        qrMaster.TaxQRInvoices.Add(qrtax);

                    }
                    order.TotalNewTaxs = extraTaxAmount;
                    order.DicountInPrecentage = orderVM.DicountPrecentage;
                    order.Invoice_State = TransactionState.Post;

                    order.HasOff = false;
                    var branchId = db.Employees.Find(order.EmployeeID).BranchSubledger_Id;

                    order.Branch_Id = branchId;

                    var PaymentQRDetail = new QRPaymentMethod();
                    PaymentQRDetail.PaymentMethod_Id = order.PaymentMethodeID;
                    PaymentQRDetail.Amount = order.TotalNewTaxs + order.Total_Amount;
                    PaymentQRDetail.Commution = 0;


                    qrMaster.QRPaymentMethods.Add(PaymentQRDetail);


                    qrMaster.AllowDiscount = 0;
                    qrMaster.ApprovalCode = "";
                    qrMaster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
                    qrMaster.Customer_Id = order.CustomerID;
                    qrMaster.Description = order.Comment;
                    qrMaster.Employee_Id = order.EmployeeID;
                    qrMaster.Invoice_Date = order.OrderDate;
                    qrMaster.Invoice_State = order.Invoice_State;
                    qrMaster.InvoiceType_Id = 4;
                    qrMaster.Master_Discount = order.DicountInPrecentage;

                    qrMaster.Reference = "";
                    qrMaster.Serial_Number = order.OrderNumber;
                    qrMaster.Store_ID = db.Stores.FirstOrDefault().Store_ID;
                    var subledgerId = db.Employees.Find(order.EmployeeID).Subldger_Id;
                    if (subledgerId == 0 || subledgerId == null)
                    {
                        subledgerId = db.SubLedgers.FirstOrDefault().SubLed_Id;
                    }
                    qrMaster.Subledger_Id = subledgerId;
                    qrMaster.TotalNewTaxs = order.TotalNewTaxs;
                    qrMaster.Account_Id = 76;
                    qrMaster.Total_Amount = order.Total_Amount;
                    qrMaster.InvoiceType = "فاتورة مبيعات كاشير";
                    qrMaster.Total_Payment_Method_Cost = 0;
                    qrMaster.Total_Tax = order.Total_Tax;
                    qrMaster.Notes = "";
                    qrMaster.Total_Cost = order.Total_Cost;
                    qrMaster.Number = InvoiceSerial.generateSerialForInvoice();
                    order.Number = qrMaster.Number;
                    order.OrderNumber = "ORD-" + System.Text.RegularExpressions.Regex.Match(order.Number, @"\d+").Value;
                    qrMaster.Serial_Number = order.OrderNumber;
                    qrMaster.DateOfCreate = DateTime.Now;
                   
                    db.MasterQRInvoices.Add(qrMaster);
                    db.SaveChanges();

                    order.QRNumber = qrMaster.Id;
                    db.Orders.Add(order);
                    db.TempOrderDetails.RemoveRange(tempOrder.TempOrderDetails);
                    db.TempOrders.Remove(tempOrder);
                    db.SaveChanges();
                    CreateInvoiceGL(order, db);
                    var openAndCloseCachier = db.StartAndEndSellings.FirstOrDefault();
                    openAndCloseCachier.OrderNumber += 1;
                    order.OrderCountNumber = openAndCloseCachier.OrderNumber;
                    db.Entry(openAndCloseCachier).State = EntityState.Modified;
                    db.SaveChanges();
                    return order;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public static List<OrderDetail> getMaterialOCompoundProduct(List<OrderDetail> Details, int Store_Id)
        {
            var db = new AccountingContainer();
            var ProductAll = db.Products.ToList();
            var project = ProductAll.Where(a => a.Prod_Type == Prod_Type.مجمع).Select(a => a.Prod_ID).ToList();
            var ProList = Details.Where(s => project.Contains(s.ProductID)).ToList();
            if (ProList.Count() == 0)
            {

            }
            else
            {
                List<OrderDetail> Details2 = new List<OrderDetail>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.ProductID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.ProductID == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new OrderDetail();
                            Detailnew.ProductID = subProduct.Product_Id;
                            Detailnew.Quantity = subProduct.Quantity * item.Quantity;
                            Detailnew.Reverse_Quantity = subProduct.Quantity * item.Quantity;
                            if (batch != null)
                            {
                                Detailnew.PatchId = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Quantity;
                                Detailnew.Product_Cost = (batch.Batch_Cost / batch.Product_Quantity);
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                                Detailnew.Product_Cost = 0;

                            }
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new OrderDetail();
                            Detailnew.ProductID = subProduct.Product_Id;
                            Detailnew.Quantity = dublicate.Quantity + (item.Quantity * subProduct.Quantity);
                            Detailnew.Reverse_Quantity = Detailnew.Quantity;
                            if (batch != null)
                            {
                                Detailnew.PatchId = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Quantity;
                                Detailnew.Product_Cost = (batch.Batch_Cost / batch.Product_Quantity);
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                                Detailnew.Product_Cost = 0;

                            }
                            Details.Remove(dublicate);
                            Details.Add(Detailnew);

                        }
                        var Delete = Details.Where(a => a.ProductID == subProduct.Master_Product_Id).FirstOrDefault();

                        Details.Remove(Delete);

                    }





                }

            }

            return Details;
        }
        public static string MakeVoidGL(int order_Id)
        {
            var db = new AccountingContainer();
            var order = db.Orders.Find(order_Id);
            order.Invoice_State = TransactionState.Void;
            
             db.SaveChanges();
            return MakeVoidForSales(order_Id);
        }
        public static String CreateInvoiceGL(Order master, AccountingContainer db)
        {
            var msg = "";
            //var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GLCode = master.OrderNumber;
            glMaster.GL_Date = master.OrderDate;
            glMaster.GL_Desc = master.Comment;
            glMaster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            glMaster.GLType_Id = "مبيعات كاشير";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;
            List<ProuctCostUtilites> ProductCostList = new List<ProuctCostUtilites> { };
            GLDetail AccountReceivableGL = new GLDetail(),
                AccountCustomerGL = new GLDetail(),
                AccountCustomerGLDebit = new GLDetail(),
               revenueGL = new GLDetail(),

               inventoryGL = new GLDetail(),
               totalCostGL = new GLDetail(),
               bankcomution = new GLDetail(),
               discount = new GLDetail(),
               fixedDiscount = new GLDetail();
            fixedDiscount.Bu_Id = 0;

            AccountReceivableGL.Bu_Id =
            AccountCustomerGL.Bu_Id =
            AccountCustomerGLDebit.Bu_Id =
            revenueGL.Bu_Id =
            inventoryGL.Bu_Id =
            totalCostGL.Bu_Id =
             discount.Bu_Id =
            bankcomution.Bu_Id = 0;

            var bankAccount = db.PaymentMethods.Find(master.PaymentMethodeID);

            int subledgerId = db.Employees.Find(master.EmployeeID).Subldger_Id.Value;
            if (subledgerId == 0 || subledgerId == null)
            {
                subledgerId = db.SubLedgers.FirstOrDefault().SubLed_Id;
            }



            var ChartOfAccounRecive = db.ChartOfAccountings.Find(bankAccount.Bank_Account_Id);
            AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
            AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;

            var ChartOfAccounRevenue = db.ChartOfAccountings.Find(76);
            revenueGL.Account_Id = ChartOfAccounRevenue.Account_Id;
            revenueGL.Account_Name = ChartOfAccounRevenue.Account_Name;
            revenueGL.SubLedger_Id = subledgerId;
            var store = db.Stores.FirstOrDefault();
            if (master.StoreId != null)
            {
                store = db.Stores.Find(master.StoreId.Value);
            }


            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

            var ChartOfAccounCost = db.ChartOfAccountings.Find(105);
            totalCostGL.Account_Id = ChartOfAccounCost.Account_Id;
            totalCostGL.Account_Name = ChartOfAccounCost.Account_Name;
            totalCostGL.SubLedger_Id = subledgerId;

            var DiscountCost = db.ChartOfAccountings.Find(78);
            discount.Account_Id = DiscountCost.Account_Id;
            discount.Account_Name = DiscountCost.Account_Name;
            discount.SubLedger_Id = subledgerId;

            var a = db.PaymentMethods.Find(master.PaymentMethodeID).Expense_Account_Id;
            if (a != null)
            {
                var bank = db.ChartOfAccountings.Find(a);
                bankcomution.Account_Id = bank.Account_Id;
                bankcomution.Account_Name = bank.Account_Name;
                bankcomution.SubLedger_Id = subledgerId;
            }

            AccountReceivableGL.Asset_Id =
           revenueGL.Asset_Id =

           inventoryGL.Asset_Id =
           AccountCustomerGL.Asset_Id =
           AccountCustomerGLDebit.Asset_Id =
            discount.Asset_Id =
           totalCostGL.Asset_Id = 0;
            var chartFixedDiscount = db.ChartOfAccountings.Find(184);
            fixedDiscount.Account_Id = chartFixedDiscount.Account_Id;
            fixedDiscount.Account_Name = chartFixedDiscount.Account_Name;
            fixedDiscount.SubLedger_Id = subledgerId;

            var ChartOfAccounPay = db.ChartOfAccountings.Find(37);
            AccountCustomerGL.Account_Id = ChartOfAccounPay.Account_Id;
            AccountCustomerGL.Account_Name = ChartOfAccounPay.Account_Name;
            AccountCustomerGLDebit.Account_Id = ChartOfAccounPay.Account_Id;
            AccountCustomerGLDebit.Account_Name = ChartOfAccounPay.Account_Name;
            var RealDetailList = GetMaterialOCompoundProduct(master.OrderDetails.ToList(), subledgerId);

            if (master.CustomerID != null && master.CustomerID != 0)
            {
                var customer = db.Customers.Find(master.CustomerID);
                var customerSubledger = db.Accounts.Where(k => k.Account_Name == customer.Customer_Name).FirstOrDefault();
                AccountCustomerGL.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGL.Account_CHild_Name = customerSubledger.Account_Name;
                AccountCustomerGLDebit.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGLDebit.Account_CHild_Name = customerSubledger.Account_Name;
            }
            var TotaL_After_Fixed = master.Total_Amount + master.TotalNewTaxs;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    switch (master.Invoice_State)
                    {

                        //Post Invoice
                        case TransactionState.Post:
                            master.Total_Cost = RealDetailList.Select(c => c.Detail_Total_Cost).Sum();
                            glMaster.GLState = TransactionState.Post;
                            var gl = db.GLMasters.Where(s => s.GLCode == glMaster.GLCode && s.GLState == glMaster.GLState).FirstOrDefault();
                            if (gl != null)
                            {
                                return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                            }

                            // Customer GL
                            AccountCustomerGL.GLDetail_Credit = master.Total_Amount + master.TotalNewTaxs;
                            AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount + master.TotalNewTaxs;

                            // Revenue GL

                            // Taxes GL


                            var pay = db.PaymentMethods.Find(master.PaymentMethodeID);

                            var Total_Payment_Method_Cost = (TotaL_After_Fixed * (pay.Percentage.Value) / 100);
                            bankcomution.GLDetail_Debit = Total_Payment_Method_Cost;
                            AccountReceivableGL.GLDetail_Debit = TotaL_After_Fixed - Total_Payment_Method_Cost;

                            foreach (var detail in master.OrderDetails)
                            {
                                var TotalBefore = detail.Product_Price * detail.Quantity;
                                revenueGL.GLDetail_Credit += TotalBefore;
                            }
                            discount.GLDetail_Debit = revenueGL.GLDetail_Credit * (master.DicountInPrecentage / 100);


                            db.ProductBatches.Load();
                            var errorMessage = new List<string> { };
                            foreach (var detail in RealDetailList)
                            {
                                var thisproduct = db.Products.Find(detail.ProductID);
                                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                {
                                    ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.ProductID && p.Store_ID == store.Store_ID).ToList().FirstOrDefault();
                                    if (detailBatch != null)
                                    {
                                        var productName = db.Products.Find(detail.ProductID).Prod_Name;
                                        if (detail.Quantity > detailBatch.Product_Quantity)
                                        {

                                            master.Invoice_State = TransactionState.Pending;
                                            //db.SaveChanges();
                                            errorMessage.Add(Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName);

                                        }
                                    }
                                    else
                                    {
                                        var productName = db.Products.Find(detail.ProductID).Prod_Name;
                                        master.Invoice_State = TransactionState.Pending;
                                        errorMessage.Add(Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName);
                                    }
                                }
                            }
                            if (errorMessage.Count() != 0)
                            {


                                string combindedString = string.Join("\n", errorMessage.ToArray());
                                return combindedString;
                            }

                            foreach (var detail in RealDetailList)
                            {
                                var thisproduct = db.Products.Find(detail.ProductID);
                                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                {
                                    ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.ProductID && p.Store_ID == store.Store_ID).ToList().FirstOrDefault();
                                    if (detailBatch != null)
                                    {


                                        var ProductCost = new ProuctCostUtilites();
                                        ProductCost.Product_ID = detailBatch.Product_ID;
                                        ProductCost.ProductCost = detailBatch.Product_Cost;

                                        detailBatch.Product_Quantity -= detail.Quantity;
                                        detailBatch.Batch_Cost -= detailBatch.Product_Cost * detail.Quantity;
                                        if (detailBatch.Product_Quantity == 0)
                                        {
                                            detailBatch.Product_Cost = 0;

                                        }
                                        else
                                        {
                                            detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                                        }

                                        db.Entry(detailBatch).State = EntityState.Modified;
                                        ProductCostList.Add(ProductCost);


                                    }
                                    else
                                    {

                                        var productName = db.Products.Find(detail.ProductID).Prod_Name;
                                        master.Invoice_State = TransactionState.Pending;

                                        return Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName;
                                    }
                                }
                            }

                            db.ProductBatches.Load();
                            foreach (var detail in RealDetailList)
                            {
                                var thisproduct = db.Products.Find(detail.ProductID);
                                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                {
                                    ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.ProductID && p.Store_ID == store.Store_ID).ToList().FirstOrDefault();
                                    if (detailBatch.Store_ID != null)
                                    {
                                        if (detailBatch.Product_Cost == 0)
                                        {
                                            var LastBatch = ProductCostList.Where(c => c.Product_ID == detail.ProductID).FirstOrDefault();
                                            detail.Product_Cost = LastBatch.ProductCost;
                                        }
                                        else
                                        {
                                            detail.Product_Cost = detailBatch.Product_Cost;
                                        }

                                        detail.Detail_Total_Cost = detail.Quantity * detail.Product_Cost;
                                        if (detail.Id != 0)
                                        {
                                            db.Entry(detail).State = EntityState.Modified;
                                        }
                                        //make product card for Report
                                        var storecard = new StoreCard();
                                        storecard.CostPrice = detail.Product_Cost;
                                        storecard.Date = DateTime.Now;
                                        storecard.Store_Id = store.Store_ID;
                                        storecard.Quantity = detail.Quantity;
                                        storecard.SalePrice = detail.Product_Price;
                                        storecard.Product_Id = detail.ProductID;
                                        storecard.IncreaseQuantity = false;
                                        storecard.TypeOfDocument = "فاتورة كاشير";
                                        storecard.Document_Serial = master.OrderNumber;
                                        storecard.TotalCost = detail.Quantity * detail.Product_Cost;
                                        storecard.TotalAmount = detail.Total_Amount;
                                        storecard.Account_Id = AccountCustomerGL.Account_Child_ID;
                                        storecard.Subledger_Id = subledgerId;
                                        db.StoreCards.Add(storecard);
                                        //Store.AddinginstorCard(storecard);
                                    }
                                }
                            }
                            if (master.SelectedExtraTaxId != null && master.SelectedExtraTaxId != 0)
                            {

                                var gldetailTax = new GLDetail();
                                var taxs = db.Taxes.Find(Convert.ToInt32(master.SelectedExtraTaxId.Value));
                                var AccountTax = db.ChartOfAccountings.Where(z => z.Account_Code == taxs.Tax_Code).FirstOrDefault();
                                gldetailTax.Account_Id = AccountTax.Account_Id;
                                gldetailTax.Account_Name = AccountTax.Account_Name;
                                gldetailTax.GLDetail_Credit = master.TotalNewTaxs;
                                gldetailTax.GLDetail_Debit = 0;
                                glMaster.GLDetails.Add(gldetailTax);






                            }
                            // Total Cost GL
                            totalCostGL.GLDetail_Debit = RealDetailList.Select(z => z.Detail_Total_Cost).ToList().Sum();
                            // Inventory GL
                            inventoryGL.GLDetail_Credit = RealDetailList.Select(z => z.Detail_Total_Cost).ToList().Sum();
                            break;


                        // Void Invoice
                        case TransactionState.Void:
                            glMaster.GLType_Id = "مبيعات نقدي عكسي";
                            glMaster.GLState = TransactionState.Void;
                            var glCheck = db.GLMasters.Where(s => s.GLCode == glMaster.GLCode && s.GLState == glMaster.GLState).FirstOrDefault();
                            if (glCheck != null)
                            {
                                return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                            }
                            var reverse = db.ReverseOrders.Where(c => c.OrderId == master.Id).ToList();
                            if (reverse.FirstOrDefault() != null)
                            {
                                msg = Resources.InvoiceMasterDetails.InvoiceCouldNotBeVoid;
                                Order salesmaster = db.Orders.Find(master.Id);
                                // Edit master invoice state to the new state
                                salesmaster.Invoice_State = TransactionState.Post;
                                db.Entry(salesmaster).State = EntityState.Modified;
                                db.SaveChanges();
                                return msg;
                            }
                            if (master.HasOff == true)
                            {

                                msg = Resources.InvoiceMasterDetails.InvoiceHasBeenIncluded + " ";
                                master.Invoice_State = TransactionState.Post;
                                db.SaveChanges();
                                return msg;
                            }
                            else
                            {
                                // Customer GL
                                AccountCustomerGL.GLDetail_Credit = TotaL_After_Fixed;
                                AccountCustomerGLDebit.GLDetail_Debit = TotaL_After_Fixed;


                                var pay2 = db.PaymentMethods.Find(master.PaymentMethodeID);
                                var Total_Payment_Methodcosts = (TotaL_After_Fixed * (pay2.Percentage.Value) / 100);
                                bankcomution.GLDetail_Credit = Total_Payment_Methodcosts;
                                AccountReceivableGL.GLDetail_Credit = TotaL_After_Fixed - Total_Payment_Methodcosts;

                                foreach (var detail in master.OrderDetails)
                                {
                                    var TotalBefore = detail.Product_Price * detail.Quantity;
                                    revenueGL.GLDetail_Debit += TotalBefore;
                                }
                                discount.GLDetail_Credit = revenueGL.GLDetail_Debit * (master.DicountInPrecentage / 100);
                                // Update store quantity
                                foreach (var detail in RealDetailList)
                                {
                                    var thisproduct = db.Products.Find(detail.ProductID);
                                    if (thisproduct.Prod_Type == Prod_Type.اولي || thisproduct.Prod_Type == Prod_Type.منتج)
                                    {
                                        ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.ProductID && p.Store_ID == store.Store_ID).ToList().FirstOrDefault();
                                        var storeCardOLd = db.StoreCards.Where(z => z.Product_Id == detail.ProductID && z.Document_Serial == master.OrderNumber).FirstOrDefault();
                                        detailBatch.Product_Quantity += storeCardOLd.Quantity;
                                        detailBatch.Batch_Cost += storeCardOLd.CostPrice * storeCardOLd.Quantity;
                                        detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                                        db.Entry(detailBatch).State = EntityState.Modified;


                                        var storecard = new StoreCard();
                                        storecard.CostPrice = detail.Detail_Total_Cost / detail.Quantity;
                                        storecard.Date = DateTime.Now;
                                        storecard.Store_Id = store.Store_ID;
                                        storecard.Quantity = storeCardOLd.Quantity;
                                        storecard.SalePrice = detail.Product_Price;
                                        storecard.Product_Id = detail.ProductID;
                                        storecard.IncreaseQuantity = true;
                                        storecard.TypeOfDocument = "استبعاد فاتورة كاشير";
                                        storecard.Document_Serial = master.OrderNumber;
                                        storecard.TotalCost = storeCardOLd.CostPrice * storeCardOLd.Quantity;
                                        storecard.TotalAmount = detail.Total_Amount;
                                        storecard.Account_Id = AccountCustomerGL.Account_Child_ID;
                                        storecard.Subledger_Id = subledgerId;
                                        db.StoreCards.Add(storecard);
                                        //Store.AddinginstorCard(storecard);
                                    }
                                }
                                // Total Cost GL
                                totalCostGL.GLDetail_Credit = master.Total_Cost;
                                // Inventory GL
                                inventoryGL.GLDetail_Debit = master.Total_Cost;
                                if (master.SelectedExtraTaxId > 0)
                                {

                                    var gldetailTax = new GLDetail();
                                    var taxs = db.Taxes.Find(Convert.ToInt32(master.SelectedExtraTaxId.Value));
                                    var AccountTax = db.ChartOfAccountings.Where(z => z.Account_Code == taxs.Tax_Code).FirstOrDefault();
                                    gldetailTax.Account_Id = AccountTax.Account_Id;
                                    gldetailTax.Account_Name = AccountTax.Account_Name;
                                    gldetailTax.GLDetail_Credit = 0;
                                    gldetailTax.GLDetail_Debit = master.TotalNewTaxs;
                                    glMaster.GLDetails.Add(gldetailTax);






                                }

                            }
                            break;
                        default:
                            break;
                    }
                    if (msg == "")
                    {

                        // Add all GL details to the master

                        glMaster.GLDetails.Add(AccountReceivableGL);
                        glMaster.GLDetails.Add(revenueGL);
                        if (inventoryGL.GLDetail_Credit != inventoryGL.GLDetail_Debit)
                        {
                            glMaster.GLDetails.Add(inventoryGL);
                            glMaster.GLDetails.Add(totalCostGL);
                        }
                        if (master.CustomerID != null && master.CustomerID != 0)
                        {
                            glMaster.GLDetails.Add(AccountCustomerGL);
                            glMaster.GLDetails.Add(AccountCustomerGLDebit);
                        }
                        if (bankcomution.GLDetail_Credit == 0 && bankcomution.GLDetail_Debit == 0)
                        {

                        }
                        else
                        {
                            glMaster.GLDetails.Add(bankcomution);

                        }
                        if (discount.GLDetail_Credit == 0 && discount.GLDetail_Debit == 0)
                        {

                        }
                        else
                        {
                            glMaster.GLDetails.Add(discount);

                        }
                        if (fixedDiscount.GLDetail_Credit == 0 && fixedDiscount.GLDetail_Debit == 0)
                        {

                        }
                        else
                        {
                            glMaster.GLDetails.Add(fixedDiscount);
                        }
                        glMaster = AddglDetailTax(glMaster, master.Id);
                        db.GLMasters.Add(glMaster);
                        GLMaster.GetBalanceForAccount(glMaster);
                        GLMaster.GetBalanceForSubLedger(glMaster);
                        msg = Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                        db.SaveChanges();
                        transaction.Commit();
                    }

                }

                catch (Exception)
                {
                    transaction.Rollback();
                    db.Dispose();
                }
            }
            if (master.Invoice_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }

            return msg;
        }
        public static List<OrderDetail> GetMaterialOCompoundProduct(List<OrderDetail> Details, int Store_Id)
        {
            var db = new AccountingContainer();

            var ProductAll = db.Products.ToList();
            var project = ProductAll.Where(a => a.Prod_Type == Prod_Type.مجمع).Select(a => a.Prod_ID).ToList();
            var ProList = Details.Where(s => project.Contains(s.ProductID)).ToList();
            if (ProList.Count() == 0)
            {

            }
            else
            {
                List<SalesReceiptDetails> Details2 = new List<SalesReceiptDetails>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.ProductID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.ProductID == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new OrderDetail();
                            Detailnew.ProductID = subProduct.Product_Id;
                            Detailnew.Quantity = subProduct.Quantity * item.Quantity;
                            Detailnew.Reverse_Quantity = subProduct.Quantity * item.Quantity;
                            if (batch != null)
                            {
                                Detailnew.PatchId = batch.Batch_ID;
                            }
                            if (batch != null && batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Quantity;
                                Detailnew.Product_Cost = (batch.Batch_Cost / batch.Product_Quantity);
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                                Detailnew.Product_Cost = 0;

                            }
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new OrderDetail();
                            Detailnew.ProductID = subProduct.Product_Id;
                            Detailnew.Quantity = dublicate.Quantity + (item.Quantity * subProduct.Quantity);
                            Detailnew.Reverse_Quantity = Detailnew.Quantity;
                            if (batch != null && batch != null)
                            {
                                Detailnew.PatchId = batch.Batch_ID;
                            }

                            if (batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Quantity;
                                Detailnew.Product_Cost = (batch.Batch_Cost / batch.Product_Quantity);
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                                Detailnew.Product_Cost = 0;

                            }
                            Details.Remove(dublicate);
                            Details.Add(Detailnew);

                        }
                        var Delete = Details.Where(a => a.ProductID == subProduct.Master_Product_Id).FirstOrDefault();

                        Details.Remove(Delete);

                    }





                }

            }

            return Details;
        }
        public static GLMaster AddglDetailTax(GLMaster gLMaster, int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceDetail = db.Orders.Find(invoiceId).OrderDetails;
            var numberofdifferentTax = invoiceDetail.Where(a => a.TaxId != 0).Select(a => a.TaxId).Distinct();
            if (numberofdifferentTax.Count() == 0)
            {
                return gLMaster;
            }
            foreach (var tax in numberofdifferentTax)
            {
                var gldetail = new GLDetail();
                var thistax = db.Taxes.Find(tax).Tax_Code;
                var taxChart = db.ChartOfAccountings.Where(a => a.Account_Code == thistax).FirstOrDefault();

                gldetail.Account_Id = taxChart.Account_Id;
                gldetail.Account_Name = taxChart.Account_Name;
                if (gLMaster.GLState == TransactionState.Post)
                {
                    gldetail.GLDetail_Credit = invoiceDetail.Where(a => a.TaxId == tax).Select(a => a.Detail_Total_Tax).Sum();
                    gldetail.GLDetail_Debit = 0;
                }
                else
                {
                    gldetail.GLDetail_Credit = 0;
                    gldetail.GLDetail_Debit = invoiceDetail.Where(a => a.TaxId == tax).Select(a => a.Detail_Total_Tax).Sum();
                }

                gLMaster.GLDetails.Add(gldetail);
            }

            return gLMaster;
        }

        public static List<OrderDetailsVM> GetAllOrderDetailsForReverse(int orderId)
        {
            var db = new AccountingContainer();
            List<OrderDetailsVM> orderDetailsVMs = new List<OrderDetailsVM>();
            //var orderDetailsList = db.TempOrderDetails.Where(e => e.TempOrderId == orderId).ToList();
            var order = db.Orders.Find(orderId);
            var orderDetailsList = order.OrderDetails.Where(e => e.OrderId == orderId).OrderByDescending(e => e.Id).ToList();
            var taxes = db.Taxes.ToList();
            string customerName = "";
            string customerPhone = "";
            string productTaxName = "";
            if (order.CustomerID > 0)
            {
                var customer = db.Customers.Find(order.CustomerID);
                customerName = customer.Customer_Name;
                customerPhone = customer.Customer_Phone;
            }
            foreach (var orderDetail in orderDetailsList)
            {

                var patch = db.ProductBatches.Find(orderDetail.PatchId);
                OrderDetailsVM orderDetailsVM = new OrderDetailsVM();
                orderDetailsVM.Id = orderDetail.Id;
                orderDetailsVM.OrderId = orderDetail.OrderId;
                orderDetailsVM.PatchId = orderDetail.PatchId;
                orderDetailsVM.ProductID = orderDetail.ProductID;
                orderDetailsVM.ProductName = patch.Product.Prod_Name;
                orderDetailsVM.ProductPrice = orderDetail.Product_Price;
                orderDetailsVM.Quantity = orderDetail.Quantity;
                orderDetailsVM.AvailableQuantity = patch.Product_Quantity;
                orderDetailsVM.ExtraTaxAmount = 0;
                decimal discount = 0;
                decimal totalbeforTaxandDiscount = orderDetail.Product_Price * orderDetail.Quantity;
                if (orderDetail.Order.DicountInPrecentage > 0)
                {
                    discount = (totalbeforTaxandDiscount * orderDetail.Order.DicountInPrecentage) / 100;
                }

                orderDetailsVM.DiscountAmount = discount;
                var productPriceAfterDiscount = totalbeforTaxandDiscount - discount;
                var totalTax = productPriceAfterDiscount;
                if (orderDetail.TaxId > 0)
                {
                    foreach (var tax in taxes)
                    {
                        if (orderDetail.TaxId == tax.Tax_ID)
                        {
                            orderDetailsVM.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                            orderDetailsVM.ProductTaxName = tax.Tax_Name;
                            productTaxName = tax.Tax_Name;
                        }

                    }
                }
                else
                {
                    orderDetailsVM.ProductTaxAmount = 0;

                }
                if (!string.IsNullOrEmpty(productTaxName))
                {
                    orderDetailsVM.ProductTaxName = productTaxName;
                }
                orderDetailsVM.SubTotalProductBeforeTax = orderDetailsVM.Quantity * orderDetailsVM.ProductPrice;
                orderDetailsVM.AmountAfterDiscount = orderDetailsVM.SubTotalProductBeforeTax - discount;
                orderDetailsVM.SubTotalProduct = productPriceAfterDiscount + orderDetailsVM.ProductTaxAmount;
                if (order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(orderDetail.Order.SelectedExtraTaxId);
                    orderDetailsVM.ExtraTaxAmount = (orderDetailsVM.SubTotalProduct * extraTax.Tax_Percentage) / 100;
                    orderDetailsVM.ExtraTaxName = extraTax.Tax_Name;
                }
                else
                {
                    orderDetailsVM.ExtraTaxName = "No extra tax";
                }
                orderDetailsVM.SubTotalOrder = orderDetailsVM.SubTotalProduct + orderDetailsVM.ExtraTaxAmount;
                orderDetailsVMs.Add(orderDetailsVM);


            }
            return orderDetailsVMs;
        }
        public static List<OrderDetailsVM> GetAllOrderDetailsToPrint(int orderId)
        {
            var db = new AccountingContainer();
            List<OrderDetailsVM> orderDetailsVMs = new List<OrderDetailsVM>();
            //var orderDetailsList = db.TempOrderDetails.Where(e => e.TempOrderId == orderId).ToList();

            var taxes = db.Taxes.ToList();
            var order = db.Orders.Find(orderId);
            var orderDetailsList = order.OrderDetails.Where(e => e.OrderId == orderId).OrderByDescending(e => e.Id).ToList();
            var store = db.Stores.Find(order.StoreId);
            string customerName = "";
            string customerTaxNumber = "";
            string customerPhone = "";
            string productTaxName = "";
            string companyName = "";
            string companyAddress = "";
            string companyPhoneNumber = "";
            string companyTaxNumber = "";
            byte[] CompanyLogo = null;
            var subLedger = db.SubLedgers.Where(i => i.SubLed_Id == order.Branch_Id).FirstOrDefault();
            Branch branch = null;
            if (subLedger != null)
            {
                 branch = db.Branches.Where(i => i.Branch_Name == subLedger.SubLed_Name).FirstOrDefault();
            }
           
            var company = db.Companies.FirstOrDefault();
            if (branch == null)
            {
                companyName = company.Comp_Name;
                companyAddress = company.Comp_Address;
                companyPhoneNumber = company.Comp_phone;
                companyTaxNumber = company.Tax_Number;
                if (company.CompanyPhotoDetails.FirstOrDefault() != null)
                {
                    CompanyLogo = company.CompanyPhotoDetails.FirstOrDefault().ProImg;
                }
            }
            if (branch != null)
            {
                companyName = branch.Branch_Name;
                companyAddress = branch.Branch_Address;
                companyPhoneNumber = branch.Branch_Phone;
                companyTaxNumber = company.Tax_Number;
                if (branch.BranchLogo != null)
                {
                    CompanyLogo = branch.BranchLogo;
                }
                else
                {
                    if (company.CompanyPhotoDetails.FirstOrDefault() == null)
                    {

                    }
                    else
                    {
                        CompanyLogo = company.CompanyPhotoDetails.FirstOrDefault().ProImg;
                    }
                }
            }
            if (order.CustomerID > 0)
            {
                var customer = db.Customers.Find(order.CustomerID);
                customerName = customer.Customer_Name;
                customerPhone = customer.Customer_Phone;
                customerTaxNumber = customer.Customer_TaxNumber;
            }
            decimal _taxAmount = 0;
            foreach (var orderDetail in orderDetailsList)
            {
                var patch = db.ProductBatches.Find(orderDetail.PatchId);
                OrderDetailsVM orderDetailsVM = new OrderDetailsVM();
                orderDetailsVM.CompanyName = companyName;
                orderDetailsVM.OrderId = orderId;
                orderDetailsVM.CompanyAdress = companyAddress;
  
                orderDetailsVM.CompanyPhone = companyPhoneNumber;
                orderDetailsVM.CompanyLogo = CompanyLogo;
                orderDetailsVM.CompanyTaxNymber = companyTaxNumber;
                orderDetailsVM.CustomerName = customerName;
                orderDetailsVM.CustomerTaxNumber = customerTaxNumber;
                orderDetailsVM.CustomerPhone = customerPhone;
                orderDetailsVM.OrderNumber = order.OrderNumber;
                orderDetailsVM.QRNumber = order.QRNumber;
                orderDetailsVM.Id = orderDetail.Id;
                orderDetailsVM.PatchId = orderDetail.PatchId;
                orderDetailsVM.OrderId = orderDetail.OrderId;
                orderDetailsVM.ProductID = orderDetail.ProductID;
                orderDetailsVM.ProductName = patch.Product.Prod_Name;
                orderDetailsVM.AdditionalDetail = orderDetail.AdditionalDetail;
                orderDetailsVM.ProductPrice = orderDetail.Product_Price;
                orderDetailsVM.Quantity = orderDetail.Quantity;
                orderDetailsVM.AvailableQuantity = patch.Product_Quantity;
                orderDetailsVM.ExtraTaxAmount = 0;
                decimal discount = 0;
                decimal totalBeforeTax = orderDetail.Product_Price * orderDetail.Quantity;
                if (order.DicountInPrecentage > 0)
                {
                    discount = (totalBeforeTax * order.DicountInPrecentage) / 100;
                }

                orderDetailsVM.DiscountAmount = discount;
                var productPriceAfterDiscount = totalBeforeTax - discount;
                if (orderDetail.TaxId > 0)
                {
                    foreach (var tax in taxes)
                    {
                        if (orderDetail.TaxId == tax.Tax_ID)
                        {
                            orderDetailsVM.ProductTaxAmount = (productPriceAfterDiscount * tax.Tax_Percentage) / 100;
                            orderDetailsVM.ProductTaxName = tax.Tax_Name;
                            productTaxName = tax.Tax_Name;
                            _taxAmount = (orderDetailsVM.ProductPrice * tax.Tax_Percentage) / 100;
                        }

                    }
                }
                else
                {
                    orderDetailsVM.ProductTaxAmount = 0;

                }

                if (!string.IsNullOrEmpty(productTaxName))
                {
                    orderDetailsVM.ProductTaxName = productTaxName;
                }

                orderDetailsVM.ProductPrice = orderDetailsVM.ProductPrice + _taxAmount;
                orderDetailsVM.SubTotalProductBeforeTax = orderDetailsVM.Quantity * orderDetailsVM.ProductPrice;
                orderDetailsVM.AmountAfterDiscount = orderDetailsVM.SubTotalProductBeforeTax - discount;
                orderDetailsVM.SubTotalProduct = productPriceAfterDiscount + orderDetailsVM.ProductTaxAmount;
                if (order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(order.SelectedExtraTaxId);
                    orderDetailsVM.ExtraTaxAmount = (orderDetailsVM.SubTotalProduct * extraTax.Tax_Percentage) / 100;
                    orderDetailsVM.ExtraTaxName = extraTax.Tax_Name;
                }
                else
                {
                    orderDetailsVM.ExtraTaxName = "No Extra tax";
                }
                orderDetailsVM.SubTotalOrder = orderDetailsVM.SubTotalProduct + orderDetailsVM.ExtraTaxAmount;
                orderDetailsVMs.Add(orderDetailsVM);
            }
            return orderDetailsVMs;
        }

        public static List<OrderDetailsVM> GetAllOrderDetails(int tempOrderId)
        {
            var db = new AccountingContainer();
            List<OrderDetailsVM> orderDetailsVMs = new List<OrderDetailsVM>();
            //var orderDetailsList = db.TempOrderDetails.Where(e => e.TempOrderId == orderId).ToList();

            var taxes = db.Taxes.ToList();
            var tempOrder = db.TempOrders.Find(tempOrderId);
            var orderDetailsList = tempOrder.TempOrderDetails.Where(e => e.TempOrderId == tempOrderId).OrderByDescending(e => e.Id).ToList();

            string customerName = "";
            string customerPhone = "";
            string productTaxName = "";
            if (tempOrder.CustomerID > 0)
            {
                var customer = db.Customers.Find(tempOrder.CustomerID);
                customerName = customer.Customer_Name;
                customerPhone = customer.Customer_Phone;
            }
            decimal price = 0;
            decimal _taxAmount = 0;
            decimal priceIncludTax = 0;
            foreach (var orderDetail in orderDetailsList)
            {
                var patch = db.ProductBatches.Find(orderDetail.PatchId);
                OrderDetailsVM orderDetailsVM = new OrderDetailsVM();
                orderDetailsVM.CustomerName = customerName;
                orderDetailsVM.CustomerPhone = customerPhone;
                orderDetailsVM.OrderNumber = tempOrder.OrderNumber;
                orderDetailsVM.Id = orderDetail.Id;
                orderDetailsVM.PatchId = orderDetail.PatchId;
                orderDetailsVM.OrderId = orderDetail.TempOrderId;
                orderDetailsVM.ProductID = orderDetail.ProductID;
                orderDetailsVM.ProductName = orderDetail.ProductName;
                orderDetailsVM.AdditionalDetail = orderDetail.AdditionalDetail;
                orderDetailsVM.ProductPrice = orderDetail.ProductPrice;
                price = orderDetail.ProductPrice;
                orderDetailsVM.Quantity = orderDetail.Quantity;
                orderDetailsVM.AvailableQuantity = patch.Product_Quantity;
                orderDetailsVM.ExtraTaxAmount = 0;
                decimal discount = 0;
                decimal totalBeforeTax = orderDetail.ProductPrice * orderDetail.Quantity;
                if (orderDetail.TempOrder.DicountInPrecentage > 0)
                {
                    discount = (totalBeforeTax * orderDetail.TempOrder.DicountInPrecentage) / 100;
                }

                orderDetailsVM.DiscountAmount = discount;
                var productPriceAfterDiscount = totalBeforeTax - discount;
                if (orderDetail.TaxID > 0)
                {
                    foreach (var tax in taxes)
                    {
                        if (orderDetail.TaxID == tax.Tax_ID)
                        {
                            orderDetailsVM.ProductTaxAmount = (productPriceAfterDiscount * tax.Tax_Percentage) / 100;
                            orderDetailsVM.ProductTaxName = tax.Tax_Name;
                            _taxAmount = (price * tax.Tax_Percentage) / 100; ;
                            //orderDetailsVM.ProductTaxName = tax.Tax_Name;
                        }

                    }
                }
                else
                {
                    orderDetailsVM.ProductTaxAmount = 0;
                    if (string.IsNullOrEmpty(productTaxName))
                    {
                        orderDetailsVM.ProductTaxName = "No vat";
                    }

                }


                priceIncludTax = price + _taxAmount;
                orderDetailsVM.ProductPrice = priceIncludTax;
                //orderDetailsVM.ProductPrice += orderDetailsVM.ProductTaxAmount;
                orderDetailsVM.SubTotalProductBeforeTax = orderDetailsVM.Quantity * orderDetailsVM.ProductPrice;
                orderDetailsVM.AmountAfterDiscount = orderDetailsVM.SubTotalProductBeforeTax - discount;
                orderDetailsVM.SubTotalProduct = productPriceAfterDiscount + orderDetailsVM.ProductTaxAmount;
                if (tempOrder.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(tempOrder.SelectedExtraTaxId);
                    orderDetailsVM.ExtraTaxAmount = (orderDetailsVM.SubTotalProduct * extraTax.Tax_Percentage) / 100;
                    orderDetailsVM.ExtraTaxName = extraTax.Tax_Name;
                }
                else
                {
                    orderDetailsVM.ExtraTaxName = "No Extra tax";
                }
                orderDetailsVM.SubTotalOrder = orderDetailsVM.SubTotalProduct + orderDetailsVM.ExtraTaxAmount;
                orderDetailsVMs.Add(orderDetailsVM);
            }
            return orderDetailsVMs;
        }
        public static List<OrderDetailsVM> GetAllRevesdOrderDetails(int reversdOrderId)
        {
            var db = new AccountingContainer();
            List<OrderDetailsVM> orderDetailsVMs = new List<OrderDetailsVM>();
            //var orderDetailsList = db.TempOrderDetails.Where(e => e.TempOrderId == orderId).ToList();

            var taxes = db.Taxes.ToList();
            var reversdeOrder = db.ReverseOrders.Find(reversdOrderId);
            var reversdeOrderDetailsList = reversdeOrder.ReverseOrderDetails.Where(e => e.ReverseOrderId == reversdOrderId).OrderByDescending(e => e.Id).ToList();

            string customerName = "";
            string customerPhone = "";
            string productTaxName = "";
            if (reversdeOrder.CustomerID > 0)
            {
                var customer = db.Customers.Find(reversdeOrder.CustomerID);
                customerName = customer.Customer_Name;
                customerPhone = customer.Customer_Phone;
            }
            foreach (var reversdeOrderDetail in reversdeOrderDetailsList)
            {
                var patch = db.ProductBatches.Find(reversdeOrderDetail.PatchId);
                OrderDetailsVM orderDetailsVM = new OrderDetailsVM();
                orderDetailsVM.CustomerName = customerName;
                orderDetailsVM.CustomerPhone = customerPhone;
                orderDetailsVM.OrderNumber = reversdeOrder.OrderNumber;
                orderDetailsVM.Id = reversdeOrderDetail.Id;
                orderDetailsVM.PatchId = reversdeOrderDetail.PatchId;
                orderDetailsVM.OrderId = reversdeOrderDetail.ReverseOrderId;
                orderDetailsVM.ProductID = reversdeOrderDetail.ProductID;
                orderDetailsVM.ProductName = patch.Product.Prod_Name;
                orderDetailsVM.ProductPrice = reversdeOrderDetail.Product_Price;
                orderDetailsVM.Quantity = reversdeOrderDetail.Quantity;
                orderDetailsVM.AvailableQuantity = patch.Product_Quantity;
                orderDetailsVM.ExtraTaxAmount = 0;
                decimal discount = 0;
                decimal totalBeforeTax = reversdeOrderDetail.Product_Price * reversdeOrderDetail.Quantity;
                if (reversdeOrder.DicountInPrecentage > 0)
                {
                    discount = (totalBeforeTax * reversdeOrder.DicountInPrecentage) / 100;
                }

                orderDetailsVM.DiscountAmount = discount;
                var productPriceAfterDiscount = totalBeforeTax - discount;
                if (reversdeOrderDetail.TaxId > 0)
                {
                    foreach (var tax in taxes)
                    {
                        if (reversdeOrderDetail.TaxId == tax.Tax_ID)
                        {
                            orderDetailsVM.ProductTaxAmount = (productPriceAfterDiscount * tax.Tax_Percentage) / 100;
                            productTaxName = tax.Tax_Name;
                            orderDetailsVM.ProductTaxName = tax.Tax_Name;
                        }

                    }
                }
                else
                {
                    orderDetailsVM.ProductTaxAmount = 0;

                }
                if (!string.IsNullOrEmpty(productTaxName))
                {
                    orderDetailsVM.ProductTaxName = productTaxName;
                }
                orderDetailsVM.SubTotalProductBeforeTax = orderDetailsVM.Quantity * orderDetailsVM.ProductPrice;
                orderDetailsVM.AmountAfterDiscount = orderDetailsVM.SubTotalProductBeforeTax - discount;
                orderDetailsVM.SubTotalProduct = productPriceAfterDiscount + orderDetailsVM.ProductTaxAmount;
                if (reversdeOrder.Order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(reversdeOrder.Order.SelectedExtraTaxId);
                    orderDetailsVM.ExtraTaxAmount = (orderDetailsVM.SubTotalProduct * extraTax.Tax_Percentage) / 100;
                    orderDetailsVM.ExtraTaxName = extraTax.Tax_Name;
                }
                else
                {
                    orderDetailsVM.ExtraTaxName = "No Extra tax";
                }
                orderDetailsVM.SubTotalOrder = orderDetailsVM.SubTotalProduct + orderDetailsVM.ExtraTaxAmount;
                orderDetailsVMs.Add(orderDetailsVM);
            }
            return orderDetailsVMs;
        }

        public static List<OrderDetailsVM> GetAllRevesdOrderofOrder(int orderId)
        {
            var db = new AccountingContainer();
            List<OrderDetailsVM> orderDetailsVMs = new List<OrderDetailsVM>();
            //var orderDetailsList = db.TempOrderDetails.Where(e => e.TempOrderId == orderId).ToList();

            var taxes = db.Taxes.ToList();
            var order = db.Orders.Find(orderId);
            var reversdeOrder = order.ReverseOrders.ToList();
            string customerName = "";
            string customerPhone = "";
            string productTaxName = "";
            if (order.CustomerID > 0)
            {
                var customer = db.Customers.Find(order.CustomerID);
                customerName = customer.Customer_Name;
                customerPhone = customer.Customer_Phone;
            }
            foreach (var reversde in reversdeOrder)
            {
                foreach (var reversdeOrderDetail in reversde.ReverseOrderDetails)
                {
                    var patch = db.ProductBatches.Find(reversdeOrderDetail.PatchId);
                    OrderDetailsVM orderDetailsVM = new OrderDetailsVM();
                    orderDetailsVM.CustomerName = customerName;
                    orderDetailsVM.CustomerPhone = customerPhone;
                    orderDetailsVM.OrderNumber = reversde.Order.OrderNumber;
                    orderDetailsVM.Id = reversdeOrderDetail.Id;
                    orderDetailsVM.PatchId = reversdeOrderDetail.PatchId;
                    orderDetailsVM.OrderId = reversdeOrderDetail.ReverseOrderId;
                    orderDetailsVM.ProductID = reversdeOrderDetail.ProductID;
                    orderDetailsVM.ProductName = patch.Product.Prod_Name;
                    orderDetailsVM.ProductPrice = reversdeOrderDetail.Product_Price;
                    orderDetailsVM.Quantity = reversdeOrderDetail.Quantity;
                    orderDetailsVM.AvailableQuantity = patch.Product_Quantity;
                    orderDetailsVM.ExtraTaxAmount = 0;
                    orderDetailsVM.ReversdeOrderDate = reversde.ReverseOrderDate;
                    decimal discount = 0;
                    decimal totalBeforeTax = reversdeOrderDetail.Product_Price * reversdeOrderDetail.Quantity;
                    if (reversde.DicountInPrecentage > 0)
                    {
                        discount = (totalBeforeTax * reversde.DicountInPrecentage) / 100;
                    }

                    orderDetailsVM.DiscountAmount = discount;
                    var productPriceAfterDiscount = totalBeforeTax - discount;
                    if (reversdeOrderDetail.TaxId > 0)
                    {
                        foreach (var tax in taxes)
                        {
                            if (reversdeOrderDetail.TaxId == tax.Tax_ID)
                            {
                                orderDetailsVM.ProductTaxAmount = (productPriceAfterDiscount * tax.Tax_Percentage) / 100;
                                orderDetailsVM.ProductTaxName = tax.Tax_Name;
                                orderDetailsVM.ProductTaxName = tax.Tax_Name;
                            }

                        }
                    }
                    else
                    {
                        orderDetailsVM.ProductTaxAmount = 0;
                        if (string.IsNullOrEmpty(productTaxName))
                        {
                            orderDetailsVM.ProductTaxName = "No vat";
                        }

                    }

                    orderDetailsVM.SubTotalProductBeforeTax = orderDetailsVM.Quantity * orderDetailsVM.ProductPrice;
                    orderDetailsVM.AmountAfterDiscount = orderDetailsVM.SubTotalProductBeforeTax - discount;
                    orderDetailsVM.SubTotalProduct = productPriceAfterDiscount + orderDetailsVM.ProductTaxAmount;
                    if (reversde.Order.SelectedExtraTaxId > 0)
                    {
                        var extraTax = db.Taxes.Find(reversde.Order.SelectedExtraTaxId);
                        orderDetailsVM.ExtraTaxAmount = (orderDetailsVM.SubTotalProduct * extraTax.Tax_Percentage) / 100;
                        orderDetailsVM.ExtraTaxName = extraTax.Tax_Name;
                    }
                    else
                    {
                        orderDetailsVM.ExtraTaxName = "No Extra tax";
                    }
                    orderDetailsVM.SubTotalOrder = orderDetailsVM.SubTotalProduct + orderDetailsVM.ExtraTaxAmount;
                    orderDetailsVMs.Add(orderDetailsVM);
                }

            }
            return orderDetailsVMs;
        }

        public static int DeleteHoldedOrder(int holdedOrderId)
        {
            var db = new AccountingContainer();
            var model = db.HoldedOrders.Find(holdedOrderId);
            if (model != null)
            {
                db.HoldedOrderDetails.RemoveRange(model.HoldedOrderDetails);
                db.HoldedOrders.Remove(model);
                db.SaveChanges();
                return db.HoldedOrders.Count();
            }
            return 0;
        }

        public static int GetHoldedOrder(int holdedOrderId)
        {
            var db = new AccountingContainer();
            var model = db.HoldedOrders.Find(holdedOrderId);
            if (model != null)
            {

                return model.Id;
            }
            return 0;
        }

        public static int HoldOrder(OrderVM orderVM)
        {
            var db = new AccountingContainer();

            HoldedOrder holdedOrder = new HoldedOrder();
            TempOrder tempOrder = db.TempOrders.Find(orderVM.Id);
            if (tempOrder != null)
            {
                holdedOrder.Comment = orderVM.Comment;
                holdedOrder.CustomerID = orderVM.SelectedCustomer_Id;
                holdedOrder.DicountInPrecentage = orderVM.DicountPrecentage;
                holdedOrder.EmployeeID = orderVM.EmployeeID;
                holdedOrder.OrderDate = DateTime.Now;
                holdedOrder.SelectedExtraTaxId = orderVM.SelectedTaxID;
                holdedOrder.SelectedPaymentMethodeId = orderVM.SelectedPaymentMethodeID;
                if (tempOrder.TempOrderDetails.Any())
                {
                    foreach (var item in tempOrder.TempOrderDetails)
                    {
                        HoldedOrderDetail holdedOrderDetails = new HoldedOrderDetail();
                        holdedOrderDetails.PatchId = item.PatchId;
                        holdedOrderDetails.ProductID = item.ProductID;
                        holdedOrderDetails.Quantity = item.Quantity;
                        holdedOrderDetails.ProductPrice = item.ProductPrice;
                        holdedOrderDetails.TaxID = 0;
                        if (item.TaxID > 0)
                        {
                            holdedOrderDetails.TaxID = item.TaxID;
                        }
                        holdedOrderDetails.ProductTaxAmount = item.ProductTaxAmount;
                        holdedOrder.HoldedOrderDetails.Add(holdedOrderDetails);

                    }
                }
                db.HoldedOrders.Add(holdedOrder);
                db.SaveChanges();
                return holdedOrder.Id;

            }
            return 0;
        }


        public static int GetBatchidByBarcode(string barcode, int orderId)
        {
            var db = new AccountingContainer();
            var order = db.TempOrders.Find(orderId);
            var Batch = db.ProductBatches.Where(a => a.Product.Bar_Code == barcode && (a.Store_ID == 1 || a.Store == null)).FirstOrDefault();
            if (Batch != null && (Batch.Product_Quantity > 0 || Batch.Store == null))
            {

                return 0/*AddOrderDetailsVM(1, orderId, Batch.Batch_ID)*/;
            }
            else
            {
                return 0;
            }
        }

        public static Tax GetTaxById(int taxId, int tempOrderId)
        {
            var db = new AccountingContainer();
            var tempOrder = db.TempOrders.Find(tempOrderId);
            var tax = db.Taxes.Find(taxId);
            if (tax != null)
            {
                tempOrder.SelectedExtraTaxId = tax.Tax_ID;
                db.Entry(tempOrder).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                tempOrder.SelectedExtraTaxId = 0;
                db.Entry(tempOrder).State = EntityState.Modified;
                db.SaveChanges();
            }


            return tax;
        }
        public static int AddCustomerToOrder(int customerId, int tempOrderId)
        {
            var db = new AccountingContainer();
            var tempOrder = db.TempOrders.Find(tempOrderId);
            var customer = db.Customers.Find(customerId);
            if (customer != null)
            {
                tempOrder.CustomerID = customer.Customer_Id;
                db.Entry(tempOrder).State = EntityState.Modified;
                db.SaveChanges();
                return customer.Customer_Id;
            }
            else
            {
                tempOrder.CustomerID = 0;
                db.Entry(tempOrder).State = EntityState.Modified;
                db.SaveChanges();
                return 0;
            }
        }
        public static List<OrderVM> GetAllHoldedOrders()
        {
            var db = new AccountingContainer();
            var orders = db.HoldedOrders.ToList();
            List<OrderVM> OrderVmList = new List<OrderVM>();
            foreach (var order in orders)
            {
                OrderVM orderVM = new OrderVM();
                orderVM.Id = order.Id;
                orderVM.ExtraTaxName = "";

                orderVM.DicountAmount = 0;
                orderVM.OrderDate = order.OrderDate;
                foreach (var orderDetail in order.HoldedOrderDetails)
                {
                    if (orderDetail.TaxID > 0)
                    {
                        var productTax = db.Taxes.Find(orderDetail.TaxID);
                        orderVM.ProductTaxName = productTax.Tax_Name;
                        orderVM.TotalProductsTaxAmount += orderDetail.ProductTaxAmount;
                    }
                    if (order.DicountInPrecentage > 0)
                    {
                        orderVM.DicountAmount = (orderDetail.ProductPrice * orderDetail.Quantity) / order.DicountInPrecentage;
                        orderVM.TotalDiscountAmount += (orderDetail.ProductPrice * orderDetail.Quantity) / order.DicountInPrecentage;
                    }
                    orderVM.TotalNet += (orderDetail.ProductPrice * orderDetail.Quantity) - orderVM.DicountAmount;
                    orderVM.TotalNet += orderDetail.ProductTaxAmount;

                }

                var employee = db.Employees.Find(order.EmployeeID);
                orderVM.Id = order.Id;
                if (order.CustomerID != 0)
                {
                    var customer = db.Customers.Find(order.CustomerID);
                    orderVM.CustomerName = customer.Customer_Name;
                }
                if (order.SelectedExtraTaxId > 0)
                {
                    var extraTax = db.Taxes.Find(order.SelectedExtraTaxId);
                    orderVM.TotalExtraTaxAmount = (orderVM.TotalNet * extraTax.Tax_Percentage) / 100;
                    orderVM.TotalNet += orderVM.TotalExtraTaxAmount;
                }
                OrderVmList.Add(orderVM);
            }
            return OrderVmList;
        }

        //public static List<HoldedOrder> GetAllHoldedOrders()
        //{
        //    var db = new AccountingContainer();
        //   return db.HoldedOrders.ToList();
        //}

        public static int DeleteOrderInvoiceDetails(int orderDetailId)
        {
            var db = new AccountingContainer();
            var model = db.TempOrderDetails.Find(orderDetailId);
            if (model != null)
            {
                db.TempOrderDetails.Remove(model);
                db.SaveChanges();
                return 1;
            }
            return 0;
        }
        public static int AddInvoiceDetailsToRevrse(int orderDetailId, int tempOrderId, int qty)
        {
            var db = new AccountingContainer();
            var orderDetail = db.OrderDetails.Find(orderDetailId);
            if (orderDetail != null && orderDetail.Reverse_Quantity > 0)
            {
                var patch = db.ProductBatches.Find(orderDetail.PatchId);
                var tax = db.Taxes.Find(orderDetail.TaxId);
                var order = orderDetail.Order;
                var tempOrder = db.TempOrders.Find(tempOrderId);
                if (tempOrder != null)
                {
                    foreach (var item in tempOrder.TempOrderDetails)
                    {
                        if (orderDetail.PatchId == item.PatchId)
                        {
                            if (item.Quantity < orderDetail.Reverse_Quantity)
                            {
                                item.Quantity += 1;
                                decimal discounta = 0;
                                if (tempOrder.DicountInPrecentage > 0)
                                {
                                    discounta = (orderDetail.Product_Price * orderDetail.Quantity) / tempOrder.DicountInPrecentage;
                                }
                                var productPriceAfterDiscounta = (orderDetail.Product_Price * orderDetail.Quantity) - discounta;
                                if (tax != null)
                                {
                                    var totalTax = productPriceAfterDiscounta;
                                    item.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                                }
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                                return item.Id;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }

                    TempOrderDetail tempOrderDetail = new TempOrderDetail();
                    tempOrderDetail.PatchId = orderDetail.PatchId;
                    tempOrderDetail.ProductID = orderDetail.ProductID;
                    tempOrderDetail.ProductName = patch.Product.Prod_Name;
                    tempOrderDetail.ProductPrice = orderDetail.Product_Price;
                    tempOrderDetail.Quantity = 1;
                    decimal discount = 0;
                    if (tempOrder.DicountInPrecentage > 0)
                    {
                        discount = (orderDetail.Product_Price * orderDetail.Quantity) / tempOrder.DicountInPrecentage;
                    }
                    var productPriceAfterDiscount = (orderDetail.Product_Price * orderDetail.Quantity) - discount;
                    if (tax != null)
                    {
                        var totalTax = productPriceAfterDiscount;
                        tempOrderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                        tempOrderDetail.TaxID = tax.Tax_ID;
                    }
                    else
                    {
                        tempOrderDetail.ProductTaxAmount = 0;
                        tempOrderDetail.TaxID = 0;
                    }
                    tempOrderDetail.TempOrderId = tempOrder.Id;
                    db.TempOrderDetails.Add(tempOrderDetail);
                    db.SaveChanges();
                    return tempOrderDetail.Id;
                }
            }
            return 0;
        }
        public static OrderVM PrepareOrderVMFoUpdate(int orderId)
        {
            var db = new AccountingContainer();
            var order = db.Orders.Find(orderId);
            if (order == null)
            {
                return null;
            }
            OrderVM orderVM = new OrderVM();
            var orderType = db.OrderTypes.ToList();
            orderVM.OrderTypeList = new SelectList(orderType, "Id", "Name");
            orderVM.SelectedTypeOrderId = order.OrderTypeId.Value;
            orderVM.Comment = order.Comment;
            orderVM.OrignalOrderNumber = order.OrderNumber;
            orderVM.OrderNumber = order.OrderNumber;
            orderVM.Id = order.Id;
            return orderVM;
        }
        public static int UpdateOrderType(OrderVM orderVM)
        {
            var db = new AccountingContainer();
            var order = db.Orders.Find(orderVM.Id);
            if (order == null)
            {
                return 0;
            }

            var orderType = db.OrderTypes.Find(orderVM.SelectedTypeOrderId);

            order.OrderType = orderType.Name;
            order.OrderTypeId = orderType.Id;
            order.Comment = orderVM.Comment;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return order.Id;
        }
        public static OrderVM PrepareOrderVM(string userId)
        {
            var db = new AccountingContainer();
            OrderVM orderVM = new OrderVM();
            orderVM.OrderProducts = new List<OrderProductVM>();
            var paymentMethodes = db.PaymentMethods.Where(a=>a.IsActive!=false).ToList();
            var orderType = db.OrderTypes.ToList();
            var employee = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (employee.BranchSubledger_Id > 0)
            {
                orderVM.EmpListList = new SelectList(db.Employees.Where(a => a.Reg_ID != null && a.BranchSubledger_Id == employee.BranchSubledger_Id).ToList(), "Emp_Id", "Emp_Name");
            }
            else
            {
                orderVM.EmpListList = new SelectList(db.Employees.Where(a => a.Reg_ID != null).ToList(), "Emp_Id", "Emp_Name");
            }
            orderVM.OrderTypeList = new SelectList(orderType, "Id", "Name");
            orderVM.SelectedTypeOrderId = orderType.FirstOrDefault().Id;
            List<object> taxesList = new List<object>();
            var taxes = db.Taxes.Where(a => a.Tax_Code.StartsWith("02") && a.IsActive != false);
            foreach (var tax in taxes)
                taxesList.Add(new
                {
                    Id = tax.Tax_ID,
                    Name = tax.Tax_Name + " % " + tax.Tax_Percentage
                });
            orderVM.MyTaxList = new SelectList(taxesList, "Id", "Name");
            var customersList =MasterPOSInvoice.GetAllCustomerForEmpList(userId);
            List<object> newList = new List<object>();
            foreach (var member in customersList)
                newList.Add(new
                {
                    Id = member.Customer_Id,
                    Name = member.Customer_Name + "        " + member.Customer_Phone
                });
            orderVM.MyCustomerList = new SelectList(newList, "Id", "Name");
            orderVM.OrderCategoery = db.Categories.ToList();
            var StoreId = 1;
            orderVM.EmployeeID = employee.Emp_Id;
            if (employee != null)
            {
                orderVM.EmployeeID = employee.Emp_Id;
                var storeEmp = db.StoreEmployees.Where(a => a.Employee_Id == employee.Emp_Id).FirstOrDefault();
                if (storeEmp != null)
                {
                    StoreId = storeEmp.Store_ID;
                }
                else
                {
                    StoreId = 1;
                }
            }
            else
            {
                StoreId = 1;
            }
            orderVM.StoreId = StoreId;
            var patches = db.ProductBatches.Where(a => a.Store_ID == StoreId && a.IsActive != false && a.Product_Quantity > 0 && a.Product.Prod_Type != Prod_Type.اولي).ToList();
            foreach (var item in patches)
            {
                if (item.Product.IsActive != false)
                {
                    if (item.Product.Prod_Type == Prod_Type.منتج)
                    {
                        if (item.Product_Quantity > 0)
                        {
                            var orderProductVMP = new OrderProductVM();
                            orderProductVMP.PatchID = item.Batch_ID;
                            orderProductVMP.ProductID = item.Product_ID;
                            orderProductVMP.ProductName = item.Product.Prod_Name;
                            orderProductVMP.ProductCode = item.Product.Bar_Code;
                            if (item.SalesPrice != null && item.SalesPrice != 0)
                            {
                                orderProductVMP.ProductPrice = item.SalesPrice.Value;
                            }
                            orderProductVMP.ProductPrice = item.Product.Sale_Price.Value;
                            orderProductVMP.AvailableQuantity = item.Product_Quantity;
                            orderProductVMP.ProductImage = item.Product.Pro_Img;
                            if (item.Product.Tax_Id > 0)
                            {
                                var tax = taxes.Where(t => t.Tax_ID == item.Product.Tax_Id).FirstOrDefault();
                                orderProductVMP.TaxName = tax.Tax_Name;
                                orderProductVMP.TaxPrecentage = tax.Tax_Percentage;
                                orderProductVMP.TaxID = tax.Tax_ID;
                            }
                            else
                            {
                                orderProductVMP.TaxName = "No Tax";
                                orderProductVMP.TaxPrecentage = 0;
                                orderProductVMP.TaxID = 0;
                            }
                            orderVM.OrderProducts.Add(orderProductVMP);
                        }
                    }
                    if (item.Product.Prod_Type == Prod_Type.مجمع)
                    {
                        var productQuantity = true;
                        foreach (var product in item.Product.ProductCompounds)
                        {
                            var storeId = StoreId;
                            var productPatch = db.ProductBatches.Where(s => s.Store_ID == storeId && s.Product_ID == product.Product_Id).FirstOrDefault();

                            if (productPatch!=null&&product.Quantity > productPatch.Product_Quantity)
                            {
                                productQuantity = false;
                            }
                        }
                        if (productQuantity)
                        {
                            var orderProductVM = new OrderProductVM();
                            orderProductVM.PatchID = item.Batch_ID;
                            orderProductVM.ProductID = item.Product_ID;
                            orderProductVM.ProductName = item.Product.Prod_Name;
                            orderProductVM.ProductCode = item.Product.Bar_Code;

                            orderProductVM.ProductPrice = item.Product.Sale_Price.Value;
                            if (item.SalesPrice != null && item.SalesPrice != 0)
                            {
                                orderProductVM.ProductPrice = item.SalesPrice.Value;
                            }
                            orderProductVM.ProductImage = item.Product.Pro_Img;
                            if (item.Product.Tax_Id > 0)
                            {
                                var tax = taxes.Where(t => t.Tax_ID == item.Product.Tax_Id).FirstOrDefault();
                                orderProductVM.TaxName = tax.Tax_Name;
                                orderProductVM.TaxPrecentage = tax.Tax_Percentage;
                                orderProductVM.TaxID = tax.Tax_ID;
                            }
                            else
                            {
                                orderProductVM.TaxName = "No Tax";
                                orderProductVM.TaxPrecentage = 0;
                                orderProductVM.TaxID = 0;
                            }
                            orderVM.OrderProducts.Add(orderProductVM);
                        }
                    }
                    if (item.Product.Prod_Type == Prod_Type.خدمة)
                    {
                        var orderProductVM = new OrderProductVM();
                        orderProductVM.PatchID = item.Batch_ID;
                        orderProductVM.ProductID = item.Product_ID;
                        orderProductVM.ProductName = item.Product.Prod_Name;
                        orderProductVM.ProductCode = item.Product.Bar_Code;
                        orderProductVM.ProductPrice = item.Product.Sale_Price.Value;
                        if (item.SalesPrice != null && item.SalesPrice != 0)
                        {
                            orderProductVM.ProductPrice = item.SalesPrice.Value;
                        }
                        orderProductVM.ProductImage = item.Product.Pro_Img;
                        if (item.Product.Tax_Id > 0)
                        {
                            var tax = taxes.Where(t => t.Tax_ID == item.Product.Tax_Id).FirstOrDefault();
                            orderProductVM.TaxName = tax.Tax_Name;
                            orderProductVM.TaxPrecentage = tax.Tax_Percentage;
                            orderProductVM.TaxID = tax.Tax_ID;
                        }
                        else
                        {
                            orderProductVM.TaxName = "No Tax";
                            orderProductVM.TaxPrecentage = 0;
                            orderProductVM.TaxID = 0;
                        }
                        orderVM.OrderProducts.Add(orderProductVM);
                    }



                }

            }
            var lastOrderNumber = db.Orders.OrderByDescending(c => c.Id).FirstOrDefault();
            if (lastOrderNumber == null)
            {
                orderVM.OrderNumber = "ORD-001";
            }
            else
            {
                orderVM.OrderNumber = "ORD-" + (Convert.ToInt32(lastOrderNumber.OrderNumber.Substring(4, lastOrderNumber.OrderNumber.Length - 4)) + 1).ToString("D3");
            }
            orderVM.EmployeeID = employee.Emp_Id;
            orderVM.EmployeeName = employee.Emp_Name;
            var paymentEmp = db.PaymentMethodEmployees.Where(a => a.Employee_Id == employee.Emp_Id ).ToList();
            if (paymentEmp.Count() > 0)
            {
                paymentMethodes = paymentEmp.Where(d=>d.PaymentMethod.IsActive!=false).Select(h => h.PaymentMethod).ToList();
               
            }
         
            orderVM.MyPaymentMethodesList = new SelectList(paymentMethodes, "PaymentMethod_Id", "Payment_Type");
            orderVM.SelectedPaymentMethodeID = 3;
            TempOrder tempOrder = new TempOrder();
            tempOrder.OrderNumber = orderVM.OrderNumber;
            db.TempOrders.Add(tempOrder);
            db.SaveChanges();
            orderVM.Id = tempOrder.Id;
            return orderVM;
        }

        public static OrderVM PrepareOrderVMFromHoldedOrder(string userId, int holdedOrderId)
        {
            var db = new AccountingContainer();
            HoldedOrder holdedOrder = db.HoldedOrders.Find(holdedOrderId);
            if (holdedOrder != null)
            {
                OrderVM orderVM = new OrderVM();

                orderVM.OrderProducts = new List<OrderProductVM>();
                var paymentMethodes = db.PaymentMethods.Where(a=>a.IsActive!=false).ToList();
                orderVM.DicountPrecentage = holdedOrder.DicountInPrecentage;
                var employee = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
                if (employee.BranchSubledger_Id > 0)
                {
                    orderVM.EmpListList = new SelectList(db.Employees.Where(a => a.Reg_ID != null && a.BranchSubledger_Id == employee.BranchSubledger_Id).ToList(), "Emp_Id", "Emp_Name");
                }
                else
                {
                    orderVM.EmpListList = new SelectList(db.Employees.Where(a => a.Reg_ID != null).ToList(), "Emp_Id", "Emp_Name");
                }
                List<object> taxesList = new List<object>();
                var taxes = db.Taxes.Where(a => a.Tax_Code.StartsWith("02") && a.IsActive != false);
                foreach (var tax in taxes)
                    taxesList.Add(new
                    {
                        Id = tax.Tax_ID,
                        Name = tax.Tax_Name + " % " + tax.Tax_Percentage
                    });
                orderVM.MyTaxList = new SelectList(taxesList, "Id", "Name");
                var customersList = MasterPOSInvoice.GetAllCustomerForEmpList(userId);
                List<object> newList = new List<object>();
                foreach (var member in customersList)
                    newList.Add(new
                    {
                        Id = member.Customer_Id,
                        Name = member.Customer_Name + "        " + member.Customer_Phone
                    });
                orderVM.MyCustomerList = new SelectList(newList, "Id", "Name");

                orderVM.OrderCategoery = db.Categories.ToList();
                var StoreId = 1;
                //var patches = db.ProductBatches.ToList();    
                orderVM.EmployeeID = employee.Emp_Id;

                if (employee != null)
                {
                    orderVM.EmployeeID = employee.Emp_Id;
                   var storeEmp = db.StoreEmployees.Where(a => a.Employee_Id == employee.Emp_Id).FirstOrDefault();
                    if (storeEmp != null)
                    {
                        StoreId = storeEmp.Store_ID;
                    }
                    else
                    {
                        StoreId = 1;
                    }
                }
                else
                {
                    StoreId = 1;
                }
                orderVM.StoreId = StoreId;

                var patches = db.ProductBatches.Where(a => a.Store_ID == StoreId && a.IsActive != false && a.Product_Quantity > 0 && a.Product.Prod_Type != Prod_Type.اولي).ToList();
                foreach (var item in patches)
                {
                    if (item.Product.IsActive != false)
                    {
                        if (item.Product.Prod_Type != Prod_Type.اولي)
                        {
                            if (item.Product.Prod_Type == Prod_Type.منتج)
                            {
                                if (item.Product_Quantity > 0)
                                {
                                    var orderProductVMP = new OrderProductVM();
                                    orderProductVMP.PatchID = item.Batch_ID;
                                    orderProductVMP.ProductID = item.Product_ID;
                                    orderProductVMP.ProductName = item.Product.Prod_Name;
                                    orderProductVMP.ProductCode = item.Product.Bar_Code;
                                    orderProductVMP.ProductPrice = item.Product.Sale_Price.Value;
                                    orderProductVMP.AvailableQuantity = item.Product_Quantity;
                                    orderProductVMP.ProductImage = item.Product.Pro_Img;
                                    if (item.Product.Tax_Id > 0)
                                    {
                                        var tax = taxes.Where(t => t.Tax_ID == item.Product.Tax_Id).FirstOrDefault();
                                        orderProductVMP.TaxName = tax.Tax_Name;
                                        orderProductVMP.TaxPrecentage = tax.Tax_Percentage;
                                        orderProductVMP.TaxID = tax.Tax_ID;
                                    }
                                    else
                                    {
                                        orderProductVMP.TaxName = "No Tax";
                                        orderProductVMP.TaxPrecentage = 0;
                                        orderProductVMP.TaxID = 0;
                                    }
                                    orderVM.OrderProducts.Add(orderProductVMP);
                                }
                            }
                            if (item.Product.Prod_Type == Prod_Type.مجمع)
                            {
                                var productQuantity = true;
                                foreach (var product in item.Product.ProductCompounds)
                                {
                                   // var storeId = db.Stores.FirstOrDefault().Store_ID;
                                    var productPatch = db.ProductBatches.Where(s => s.Product_ID == product.Product_Id && s.Store_ID == StoreId).FirstOrDefault();

                                    if (product.Quantity > productPatch.Product_Quantity)
                                    {
                                        productQuantity = false;
                                    }
                                }
                                if (productQuantity)
                                {
                                    var orderProductVM = new OrderProductVM();
                                    orderProductVM.PatchID = item.Batch_ID;
                                    orderProductVM.ProductID = item.Product_ID;
                                    orderProductVM.ProductName = item.Product.Prod_Name;
                                    orderProductVM.ProductCode = item.Product.Bar_Code;
                                    orderProductVM.ProductPrice = item.Product.Sale_Price.Value;
                                    orderProductVM.ProductImage = item.Product.Pro_Img;
                                    if (item.Product.Tax_Id > 0)
                                    {
                                        var tax = taxes.Where(t => t.Tax_ID == item.Product.Tax_Id).FirstOrDefault();
                                        orderProductVM.TaxName = tax.Tax_Name;
                                        orderProductVM.TaxPrecentage = tax.Tax_Percentage;
                                        orderProductVM.TaxID = tax.Tax_ID;
                                    }
                                    else
                                    {
                                        orderProductVM.TaxName = "No Tax";
                                        orderProductVM.TaxPrecentage = 0;
                                        orderProductVM.TaxID = 0;
                                    }
                                    orderVM.OrderProducts.Add(orderProductVM);
                                }
                            }
                            if (item.Product.Prod_Type == Prod_Type.خدمة)
                            {
                                var orderProductVM = new OrderProductVM();
                                orderProductVM.PatchID = item.Batch_ID;
                                orderProductVM.ProductID = item.Product_ID;
                                orderProductVM.ProductName = item.Product.Prod_Name;
                                orderProductVM.ProductCode = item.Product.Bar_Code;
                                orderProductVM.ProductPrice = item.Product.Sale_Price.Value;
                                orderProductVM.ProductImage = item.Product.Pro_Img;
                                if (item.Product.Tax_Id > 0)
                                {
                                    var tax = taxes.Where(t => t.Tax_ID == item.Product.Tax_Id).FirstOrDefault();
                                    orderProductVM.TaxName = tax.Tax_Name;
                                    orderProductVM.TaxPrecentage = tax.Tax_Percentage;
                                    orderProductVM.TaxID = tax.Tax_ID;
                                }
                                else
                                {
                                    orderProductVM.TaxName = "No Tax";
                                    orderProductVM.TaxPrecentage = 0;
                                    orderProductVM.TaxID = 0;
                                }
                                orderVM.OrderProducts.Add(orderProductVM);
                            }


                        }
                    }



                }


                var lastOrderNumber = db.Orders.OrderByDescending(c => c.Id).FirstOrDefault();
                if (lastOrderNumber == null)
                {
                    orderVM.OrderNumber = "ORD-001";
                }
                else
                {
                    orderVM.OrderNumber = "ORD-" + (Convert.ToInt32(lastOrderNumber.OrderNumber.Substring(4, lastOrderNumber.OrderNumber.Length - 4)) + 1).ToString("D3");
                }
                orderVM.EmployeeID = employee.Emp_Id;
                orderVM.EmployeeName = employee.Emp_Name;
                var paymentEmp = db.PaymentMethodEmployees.Where(a => a.Employee_Id == employee.Emp_Id).ToList();
                if (paymentEmp.Count() > 0)
                {
                    paymentMethodes = paymentEmp.Where(d => d.PaymentMethod.IsActive != false).Select(h => h.PaymentMethod).ToList();
                }

                orderVM.MyPaymentMethodesList = new SelectList(paymentMethodes, "PaymentMethod_Id", "Payment_Type");
                orderVM.SelectedPaymentMethodeID = paymentMethodes.FirstOrDefault().PaymentMethod_Id;
                orderVM.Comment = holdedOrder.Comment;
                if (holdedOrder.SelectedPaymentMethodeId > 0)
                {
                    orderVM.SelectedPaymentMethodeID = holdedOrder.SelectedPaymentMethodeId.Value;
                }
                if (holdedOrder.SelectedExtraTaxId > 0)
                {
                    var tempTax = db.Taxes.Find(holdedOrder.SelectedExtraTaxId);
                    orderVM.ExtraTaxName = tempTax.Tax_Name;
                    orderVM.SelectedTaxID = tempTax.Tax_ID;
                }
                if (holdedOrder.CustomerID > 0)
                {
                    orderVM.SelectedCustomer_Id = holdedOrder.CustomerID.Value;
                }
                TempOrder tempOrder = new TempOrder();
                tempOrder.OrderNumber = orderVM.OrderNumber;
                tempOrder.CustomerID = orderVM.SelectedCustomer_Id;
                tempOrder.SelectedExtraTaxId = orderVM.SelectedTaxID;
                foreach (var orderDetail in holdedOrder.HoldedOrderDetails)
                {
                    var patch = db.ProductBatches.Find(orderDetail.PatchId);
                    var tax = db.Taxes.Find(patch.Product.Tax_Id);
                    TempOrderDetail tempOrderDetail = new TempOrderDetail();
                    tempOrderDetail.PatchId = orderDetail.PatchId;
                    tempOrderDetail.ProductID = orderDetail.ProductID;
                    tempOrderDetail.ProductName = patch.Product.Prod_Name;
                    tempOrderDetail.ProductPrice = orderDetail.ProductPrice;
                    tempOrderDetail.Quantity = orderDetail.Quantity;
                    if (tax != null)
                    {
                        var totalTax = patch.Product.Sale_Price.Value * tempOrderDetail.Quantity;
                        tempOrderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                        tempOrderDetail.TaxID = tax.Tax_ID;
                    }
                    else
                    {
                        tempOrderDetail.ProductTaxAmount = 0;
                        tempOrderDetail.TaxID = 0;
                    }
                    tempOrder.TempOrderDetails.Add(tempOrderDetail);
                }
                db.TempOrders.Add(tempOrder);
                //db.HoldedOrderDetails.RemoveRange(holdedOrder.HoldedOrderDetails);
                //db.HoldedOrders.Remove(holdedOrder);
                db.SaveChanges();
                orderVM.Id = tempOrder.Id;
                return orderVM;
            }
            else
            {
                return PrepareOrderVM(userId);
            }

        }
        public static int AddOrderDetailsVM(decimal qty, int orderId, int patchId, int customer)
        {
            var db = new AccountingContainer();
            int temporderDetailsID = 0;
            var tempOrder = db.TempOrders.Find(orderId);
            var patch = db.ProductBatches.Find(patchId);
            var tax = db.Taxes.Find(patch.Product.Tax_Id);
            foreach (var orderDetail in tempOrder.TempOrderDetails)
            {
                if (orderDetail.PatchId == patchId)
                {
                    if (patch.Product.Prod_Type == Prod_Type.منتج)
                    {
                        decimal finalQuantity = orderDetail.Quantity + qty;
                        if (finalQuantity <= patch.Product_Quantity)
                        {
                            orderDetail.Quantity += qty;
                        }
                    }
                    if (patch.Product.Prod_Type == Prod_Type.مجمع)
                    {
                        var productQuantity = true;
                        foreach (var product in patch.Product.ProductCompounds)
                        {
                            //var storeId = db.Stores.FirstOrDefault().Store_ID;


                            var productPatch = db.ProductBatches.Where(s => s.Store_ID == patch.Store_ID && s.Product_ID == product.Product_Id).FirstOrDefault();

                            if (product.Quantity > productPatch.Product_Quantity)
                            {
                                productQuantity = false;
                            }
                        }
                        if (productQuantity)
                        {
                            orderDetail.Quantity += qty;
                        }

                    }

                    if (patch.Product.Prod_Type == Prod_Type.خدمة)
                    {
                        orderDetail.Quantity += qty;
                    }
                    decimal discount = 0;

                    decimal totalBeforTaxAndDiscount = orderDetail.ProductPrice * orderDetail.Quantity;
                    if (orderDetail.TempOrder.DicountInPrecentage > 0)
                    {
                        discount = (totalBeforTaxAndDiscount * orderDetail.TempOrder.DicountInPrecentage) / 100;
                    }
                    var productPriceAfterDiscount = totalBeforTaxAndDiscount - discount;
                    orderDetail.ProductTaxAmount = 0;
                    if (tax != null)
                    {
                        var totalTax = productPriceAfterDiscount;
                        orderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                    }

                    db.Entry(orderDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    temporderDetailsID = orderDetail.Id;
                   
                   
                    return tempOrder.TempOrderDetails.Sum(e => Convert.ToInt32(e.Quantity));

                }
            }

            if (tempOrder != null)
            {
                TempOrderDetail tempOrderDetail = new TempOrderDetail();
                tempOrderDetail.PatchId = patch.Batch_ID;
                tempOrderDetail.ProductID = patch.Product_ID;
                tempOrderDetail.ProductName = patch.Product.Prod_Name;
                tempOrderDetail.ProductPrice = InvoiceMaster.GetPriceSale(patch.Store_ID.Value, patch.Product_ID, customer);
                if (patch.Product.Prod_Type == Prod_Type.منتج)
                {
                    if (qty <= patch.Product_Quantity)
                    {
                        tempOrderDetail.Quantity = qty;
                        decimal discount = 0;
                        decimal totalBeforeTaxAndDiscount = tempOrderDetail.ProductPrice * tempOrderDetail.Quantity;
                        if (tempOrder.DicountInPrecentage > 0)
                        {
                            discount = (totalBeforeTaxAndDiscount * tempOrder.DicountInPrecentage) / 100;
                        }
                        var productPriceAfterDiscount = totalBeforeTaxAndDiscount - discount;
                        if (tax != null)
                        {
                            var totalTax = productPriceAfterDiscount;
                            tempOrderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                            tempOrderDetail.TaxID = tax.Tax_ID;
                        }
                        else
                        {
                            tempOrderDetail.ProductTaxAmount = 0;
                            tempOrderDetail.TaxID = 0;
                        }
                        tempOrderDetail.TempOrderId = tempOrder.Id;
                        db.TempOrderDetails.Add(tempOrderDetail);
                        db.SaveChanges();
                        temporderDetailsID = tempOrderDetail.Id;

                    }

                }

                if (patch.Product.Prod_Type == Prod_Type.مجمع)
                {
                    var productQuantity = true;
                    foreach (var item in patch.Product.ProductCompounds)
                    {
                        var storeId = db.Stores.FirstOrDefault().Store_ID;
                        var productPatch = db.ProductBatches.Where(s => s.Store_ID == storeId && s.Product_ID == item.Product_Id).FirstOrDefault();

                        if (item.Quantity > productPatch.Product_Quantity)
                        {
                            productQuantity = false;
                            break;
                        }
                    }
                    if (productQuantity)
                    {
                        tempOrderDetail.Quantity = qty;
                        decimal discount = 0;
                        decimal totalBeforeTaxAndDiscount = tempOrderDetail.ProductPrice * tempOrderDetail.Quantity;
                        if (tempOrder.DicountInPrecentage > 0)
                        {
                            discount = (totalBeforeTaxAndDiscount * tempOrder.DicountInPrecentage) / 100;
                        }
                        var productPriceAfterDiscount = totalBeforeTaxAndDiscount - discount;
                        if (tax != null)
                        {
                            var totalTax = productPriceAfterDiscount;
                            tempOrderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                            tempOrderDetail.TaxID = tax.Tax_ID;
                        }
                        else
                        {
                            tempOrderDetail.ProductTaxAmount = 0;
                            tempOrderDetail.TaxID = 0;
                        }
                        tempOrderDetail.TempOrderId = tempOrder.Id;
                        db.TempOrderDetails.Add(tempOrderDetail);
                        db.SaveChanges();
                        temporderDetailsID = tempOrderDetail.Id;
                    }


                }


                if (patch.Product.Prod_Type == Prod_Type.خدمة)
                {
                    tempOrderDetail.Quantity = qty;
                    decimal discount = 0;
                    decimal totalBeforeTaxAndDiscount = tempOrderDetail.ProductPrice * tempOrderDetail.Quantity;
                    if (tempOrder.DicountInPrecentage > 0)
                    {
                        discount = (totalBeforeTaxAndDiscount * tempOrder.DicountInPrecentage) / 100;
                    }
                    var productPriceAfterDiscount = totalBeforeTaxAndDiscount - discount;
                    if (tax != null)
                    {
                        var totalTax = productPriceAfterDiscount;
                        tempOrderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                        tempOrderDetail.TaxID = tax.Tax_ID;
                    }
                    else
                    {
                        tempOrderDetail.ProductTaxAmount = 0;
                        tempOrderDetail.TaxID = 0;
                    }
                    tempOrderDetail.TempOrderId = tempOrder.Id;
                    db.TempOrderDetails.Add(tempOrderDetail);
                    db.SaveChanges();
                    temporderDetailsID = tempOrderDetail.Id;
                }
            }
            return tempOrder.TempOrderDetails.Sum(e => Convert.ToInt32(e.Quantity));

        }

        public static int AddOrderDetailsVMFromScanner(decimal qty, int orderId, string productBarcode, int customer)
        {
            var db = new AccountingContainer();
            int temporderDetailsID = 0;
            var tempOrder = db.TempOrders.Find(orderId);
            var patch = db.ProductBatches.Where(e => e.Product.Bar_Code == productBarcode).FirstOrDefault();
            if (patch == null || tempOrder == null)
            {
                return 0;
            }
            int patchId = patch.Batch_ID;
            var tax = db.Taxes.Find(patch.Product.Tax_Id);
            foreach (var orderDetail in tempOrder.TempOrderDetails)
            {
                if (orderDetail.PatchId == patchId)
                {
                    if (patch.Product.Prod_Type == Prod_Type.منتج)
                    {
                        decimal finalQuantity = orderDetail.Quantity + qty;
                        if (finalQuantity <= patch.Product_Quantity)
                        {
                            orderDetail.Quantity += qty;
                        }
                    }
                    if (patch.Product.Prod_Type == Prod_Type.مجمع)
                    {
                        var productQuantity = true;
                        foreach (var product in patch.Product.ProductCompounds)
                        {
                            //var storeId = db.Stores.FirstOrDefault().Store_ID;
                            var productPatch = db.ProductBatches.Where(s => s.Store_ID == patch.Store_ID && s.Product_ID == product.Product_Id).FirstOrDefault();
                         
                            if (product.Quantity > productPatch.Product_Quantity)
                            {
                                productQuantity = false;
                            }
                        }
                        if (productQuantity)
                        {
                            orderDetail.Quantity += qty;
                        }

                    }

                    if (patch.Product.Prod_Type == Prod_Type.خدمة)
                    {
                        orderDetail.Quantity += qty;
                    }
                    decimal discount = 0;

                    decimal totalBeforTaxAndDiscount = orderDetail.ProductPrice * orderDetail.Quantity;
                    if (orderDetail.TempOrder.DicountInPrecentage > 0)
                    {
                        discount = (totalBeforTaxAndDiscount * orderDetail.TempOrder.DicountInPrecentage) / 100;
                    }
                    var productPriceAfterDiscount = totalBeforTaxAndDiscount - discount;
                    orderDetail.ProductTaxAmount = 0;
                    if (tax != null)
                    {
                        var totalTax = productPriceAfterDiscount;
                        orderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                    }

                    db.Entry(orderDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    temporderDetailsID = orderDetail.Id;
                    return tempOrder.TempOrderDetails.Sum(e => Convert.ToInt32(e.Quantity));


                }
            }
            if (tempOrder != null)
            {
                TempOrderDetail tempOrderDetail = new TempOrderDetail();
                tempOrderDetail.PatchId = patch.Batch_ID;
                tempOrderDetail.ProductID = patch.Product_ID;
                tempOrderDetail.ProductName = patch.Product.Prod_Name;
                tempOrderDetail.ProductPrice = InvoiceMaster.GetPriceSale(patch.Store_ID.Value, patch.Product_ID, customer);
                if (patch.Product.Prod_Type == Prod_Type.منتج)
                {
                    if (qty <= patch.Product_Quantity)
                    {
                        tempOrderDetail.Quantity = qty;
                        decimal discount = 0;
                        decimal totalBeforeTaxAndDiscount = tempOrderDetail.ProductPrice * tempOrderDetail.Quantity;
                        if (tempOrder.DicountInPrecentage > 0)
                        {
                            discount = (totalBeforeTaxAndDiscount * tempOrder.DicountInPrecentage) / 100;
                        }
                        var productPriceAfterDiscount = totalBeforeTaxAndDiscount - discount;
                        if (tax != null)
                        {
                            var totalTax = productPriceAfterDiscount;
                            tempOrderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                            tempOrderDetail.TaxID = tax.Tax_ID;
                        }
                        else
                        {
                            tempOrderDetail.ProductTaxAmount = 0;
                            tempOrderDetail.TaxID = 0;
                        }
                        tempOrderDetail.TempOrderId = tempOrder.Id;
                        db.TempOrderDetails.Add(tempOrderDetail);
                        db.SaveChanges();
                        temporderDetailsID = tempOrderDetail.Id;

                    }
                }
                if (patch.Product.Prod_Type == Prod_Type.مجمع)
                {
                    var productQuantity = true;
                    foreach (var item in patch.Product.ProductCompounds)
                    {
                        var storeId = db.Stores.FirstOrDefault().Store_ID;
                        var productPatch = db.ProductBatches.Where(s => s.Store_ID == storeId&&s.Product_ID== item.Product_Id).FirstOrDefault();

                        if (item.Quantity > productPatch.Product_Quantity)
                        {
                            productQuantity = false;
                            break;
                        }
                    }
                    if (productQuantity)
                    {
                        tempOrderDetail.Quantity = qty;
                        decimal discount = 0;
                        decimal totalBeforeTaxAndDiscount = tempOrderDetail.ProductPrice * tempOrderDetail.Quantity;
                        if (tempOrder.DicountInPrecentage > 0)
                        {
                            discount = (totalBeforeTaxAndDiscount * tempOrder.DicountInPrecentage) / 100;
                        }
                        var productPriceAfterDiscount = totalBeforeTaxAndDiscount - discount;
                        if (tax != null)
                        {
                            var totalTax = productPriceAfterDiscount;
                            tempOrderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                            tempOrderDetail.TaxID = tax.Tax_ID;
                        }
                        else
                        {
                            tempOrderDetail.ProductTaxAmount = 0;
                            tempOrderDetail.TaxID = 0;
                        }
                        tempOrderDetail.TempOrderId = tempOrder.Id;
                        db.TempOrderDetails.Add(tempOrderDetail);
                        db.SaveChanges();
                        temporderDetailsID = tempOrderDetail.Id;
                    }


                }


                if (patch.Product.Prod_Type == Prod_Type.خدمة)
                {
                    tempOrderDetail.Quantity = qty;
                    decimal discount = 0;
                    decimal totalBeforeTaxAndDiscount = tempOrderDetail.ProductPrice * tempOrderDetail.Quantity;
                    if (tempOrder.DicountInPrecentage > 0)
                    {
                        discount = (totalBeforeTaxAndDiscount * tempOrder.DicountInPrecentage) / 100;
                    }
                    var productPriceAfterDiscount = totalBeforeTaxAndDiscount - discount;
                    if (tax != null)
                    {
                        var totalTax = productPriceAfterDiscount;
                        tempOrderDetail.ProductTaxAmount = (totalTax * tax.Tax_Percentage) / 100;
                        tempOrderDetail.TaxID = tax.Tax_ID;
                    }
                    else
                    {
                        tempOrderDetail.ProductTaxAmount = 0;
                        tempOrderDetail.TaxID = 0;
                    }
                    tempOrderDetail.TempOrderId = tempOrder.Id;
                    db.TempOrderDetails.Add(tempOrderDetail);
                    db.SaveChanges();
                    temporderDetailsID = tempOrderDetail.Id;
                }


            }
            return tempOrder.TempOrderDetails.Sum(e => Convert.ToInt32(e.Quantity));

        }

        public static int AddOrderDiscountPrecentageToOrder(decimal precentageValue, int orderId)
        {
            var db = new AccountingContainer();
            var tempOrder = db.TempOrders.Find(orderId);
            tempOrder.DicountInPrecentage = precentageValue;
            db.Entry(tempOrder).State = EntityState.Modified;
            db.SaveChanges();
            return tempOrder.Id;
        }
        public static bool CheckIfEmpolyeeExisit(string userId)
        {
            var db = new AccountingContainer();
            var empolyee = db.Employees.Where(e => e.Reg_ID == userId).FirstOrDefault();
            if (empolyee == null)
            {
                return false;

            }
            return true;
        }
        /////////////forReverse
        ///

        public static String CreateReverseInvoiceGL(ReverseOrder master)
        {
            var msg = "";
            var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GLCode = master.OrderNumber;
            glMaster.GL_Date = master.ReverseOrderDate;
            Order invoiceMaster = db.Orders.Find(master.Order.Id);
            var detailInvoice = invoiceMaster.OrderDetails.ToList();
            glMaster.GL_Desc = invoiceMaster.Comment;
            glMaster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            glMaster.GLType_Id = "مردود مبيعات كاشير";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;
            GLDetail AccountReceivableGL = new GLDetail(),
               revenueGL = new GLDetail(),

               inventoryGL = new GLDetail(),
               totalCostGL = new GLDetail(),
        bankcomution = new GLDetail(),
          AccountCustomerGL = new GLDetail(),
                AccountCustomerGLDebit = new GLDetail(),
      discount = new GLDetail(),
      extraGL = new GLDetail()
      ;
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            AccountReceivableGL.Bu_Id =
            revenueGL.Bu_Id =
            inventoryGL.Bu_Id =
            totalCostGL.Bu_Id =
             discount.Bu_Id =
            bankcomution.Bu_Id = 0;
            if (master.PaymentMethodeID != 0 && master.PaymentMethodeID != null)
            {
                var bankAccount = db.PaymentMethods.Find(master.PaymentMethodeID);

                var ChartOfAccounRecive = db.ChartOfAccountings.Find(bankAccount.Bank_Account_Id);
                AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
                AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;
            }
            else if (master.CustomerID != null && master.CustomerID != 0)
            {

                var ChartOfAccounRecive = db.ChartOfAccountings.Find(37);
                AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
                AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;
                var cust = db.Customers.Find(master.CustomerID).Customer_Code;
                var cusAccount = db.Accounts.Where(g => g.Account_Code == cust).FirstOrDefault();
                AccountReceivableGL.Account_Child_ID = cusAccount.Accounts_Id;
                AccountReceivableGL.Account_CHild_Name = cusAccount.Account_Name;
            }
            else
            {
                var salesRecipt = db.Orders.Find(master.Order.Id);
                var bankAccount = db.PaymentMethods.Find(salesRecipt.PaymentMethodeID);

                var ChartOfAccounRecive = db.ChartOfAccountings.Find(bankAccount.Bank_Account_Id);
                AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
                AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;

            }
            decimal ExtraTax = 0;
            if (master.Order.SelectedExtraTaxId != 0 && master.Order.SelectedExtraTaxId != null)
            {
                var Tax = db.Taxes.Find(master.Order.SelectedExtraTaxId);

                var ChartOfAccounRecive = db.ChartOfAccountings.Where(T => T.Account_Code == Tax.Tax_Code).FirstOrDefault();
                extraGL.Account_Id = ChartOfAccounRecive.Account_Id;
                extraGL.Account_Name = ChartOfAccounRecive.Account_Name;
                ExtraTax = master.Total_Amount * (Tax.Tax_Percentage / 100);
            }

            var ChartOfAccounRevenue = db.ChartOfAccountings.Find(77);
            revenueGL.Account_Id = ChartOfAccounRevenue.Account_Id;
            revenueGL.Account_Name = ChartOfAccounRevenue.Account_Name;



            var store = db.Stores.FirstOrDefault();
            if (invoiceMaster.StoreId != null)
            {
                store = db.Stores.Find(invoiceMaster.StoreId.Value);
            }



            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

            var ChartOfAccounCost = db.ChartOfAccountings.Find(105);
            totalCostGL.Account_Id = ChartOfAccounCost.Account_Id;
            totalCostGL.Account_Name = ChartOfAccounCost.Account_Name;

            var DiscountCost = db.ChartOfAccountings.Find(78);
            discount.Account_Id = DiscountCost.Account_Id;
            discount.Account_Name = DiscountCost.Account_Name;
            int subledgerId = db.Employees.Find(master.EmployeeID).Subldger_Id.Value;
            if (subledgerId == 0 || subledgerId == null)
            {
                subledgerId = db.SubLedgers.FirstOrDefault().SubLed_Id;
            }

            discount.SubLedger_Id =
            revenueGL.SubLedger_Id =
            bankcomution.SubLedger_Id =
            totalCostGL.SubLedger_Id = subledgerId;

            if (master.PaymentMethodeID != null && master.PaymentMethodeID != 0)
            {

                var a = db.PaymentMethods.Find(master.PaymentMethodeID).Expense_Account_Id;
                if (a != null)
                {
                    var bank = db.ChartOfAccountings.Find(a);
                    bankcomution.Account_Id = bank.Account_Id;
                    bankcomution.Account_Name = bank.Account_Name;
                }
            }
            AccountReceivableGL.Asset_Id =
           revenueGL.Asset_Id =

           inventoryGL.Asset_Id =
            discount.Asset_Id =
           totalCostGL.Asset_Id = 0;
            if (master.CustomerID != 0 && master.CustomerID != null && master.PaymentMethodeID != 0)
            {
                var ChartOfAccounPay = db.ChartOfAccountings.Find(37);
                AccountCustomerGL.Account_Id = ChartOfAccounPay.Account_Id;
                AccountCustomerGL.Account_Name = ChartOfAccounPay.Account_Name;
                AccountCustomerGLDebit.Account_Id = ChartOfAccounPay.Account_Id;
                AccountCustomerGLDebit.Account_Name = ChartOfAccounPay.Account_Name;
                var customer = db.Customers.Find(master.CustomerID);
                var customerSubledger = db.Accounts.Where(k => k.Account_Name == customer.Customer_Name).FirstOrDefault();
                AccountCustomerGL.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGL.Account_CHild_Name = customerSubledger.Account_Name;
                AccountCustomerGLDebit.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGLDebit.Account_CHild_Name = customerSubledger.Account_Name;
            }
            var RealDetails = GetMaterialOCompoundProduct(master.ReverseOrderDetails.ToList(), store.Store_ID);
            master.Total_Cost = 0;

            switch (master.Invoice_State)
            {
                //Post Invoice
                case TransactionState.Post:
                    // Customer GL
                    glMaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(y => y.GLCode == glMaster.GLCode && y.GLState == glMaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }
                    AccountCustomerGL.GLDetail_Credit = master.Total_Amount;

                    AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount;
                    // Revenue GL

                    // Taxes GL

                    // Inventory GL

                    var pay = db.PaymentMethods.Find(master.PaymentMethodeID);
                    bankcomution.GLDetail_Credit = 0;
                    AccountReceivableGL.GLDetail_Credit = master.Total_Amount + ExtraTax;
                    extraGL.GLDetail_Debit = ExtraTax;
                    foreach (var detail in master.ReverseOrderDetails)
                    {
                        var TotalBefore = detail.Product_Price * detail.Quantity;
                        revenueGL.GLDetail_Debit += TotalBefore;
                    }
                    discount.GLDetail_Credit = revenueGL.GLDetail_Debit * (master.DicountInPrecentage / 100);

                    db.ProductBatches.Load();
                    foreach (var detail in master.ReverseOrderDetails)
                    {
                        //تعديل فى قيم الفاتورة بناءا على المرتجع
                        var record = detailInvoice.Where(m => m.ProductID == detail.ProductID).FirstOrDefault();
                        record.Reverse_Quantity = record.Reverse_Quantity - detail.Quantity;
                        record.Reverse_Total_Tax = record.Reverse_Total_Tax - detail.Detail_Total_Tax;
                        record.Reverse_Total_Amount = record.Reverse_Total_Amount - detail.Total_Amount;
                        db.Entry(record).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    foreach (var detail in RealDetails)
                    {
                        var thisproduct = db.Products.Find(detail.ProductID);
                        if (thisproduct.Prod_Type == Prod_Type.اولي || thisproduct.Prod_Type == Prod_Type.منتج)
                        {

                            ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.ProductID && p.Store_ID == store.Store_ID).FirstOrDefault();
                            var storeCardOLd = db.StoreCards.Where(a => a.Product_Id == detail.ProductID && a.Document_Serial == invoiceMaster.OrderNumber).FirstOrDefault();
                            detailBatch.Product_Quantity += detail.Quantity;

                            detailBatch.Batch_Cost += detail.Quantity * storeCardOLd.CostPrice;
                            detailBatch.Product_Cost = (detailBatch.Batch_Cost / detailBatch.Product_Quantity);

                            db.Entry(detailBatch).State = EntityState.Modified;

                            var storecard = new StoreCard();
                            storecard.CostPrice = storeCardOLd.CostPrice;

                            storecard.Date = DateTime.Now;
                            storecard.Store_Id = store.Store_ID;
                            storecard.Quantity = detail.Quantity;
                            storecard.SalePrice = detail.Product_Price;
                            storecard.Product_Id = detail.ProductID;
                            storecard.IncreaseQuantity = true;
                            storecard.TypeOfDocument = " فاتورة مردود كاشير";
                            storecard.Document_Serial = master.OrderNumber;
                            storecard.TotalCost = detail.Quantity * storeCardOLd.CostPrice;

                            storecard.TotalAmount = detail.Total_Amount;
                            storecard.Account_Id = AccountReceivableGL.Account_Child_ID;
                            storecard.Subledger_Id = revenueGL.SubLedger_Id;
                            Store.AddinginstorCard(storecard);
                            master.Total_Cost += storecard.TotalCost.Value;

                        }
                    }
                    // Total Cost GL
                    totalCostGL.GLDetail_Credit = master.Total_Cost;
                    inventoryGL.GLDetail_Debit = master.Total_Cost;
                    break;
                // Void Invoice
                case TransactionState.Void:
                    glMaster.GLType_Id = "مردود مبيعات كاشير عكسي";
                    glMaster.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(s => s.GLCode == glMaster.GLCode && s.GLState == glMaster.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }
                    var errorMessage = new List<string> { };
                    foreach (var detail in RealDetails)
                    {
                        var thisproduct = db.Products.Find(detail.ProductID);

                        ProductBatches detailBatch = db.ProductBatches.Where(c => c.Product_ID == detail.ProductID && c.Store_ID == store.Store_ID).ToList().FirstOrDefault();
                        if (detailBatch != null && detailBatch.Store_ID != null)
                        {
                            if (detail.Quantity > detailBatch.Product_Quantity)
                            {
                                var productName = db.Products.Find(detail.ProductID).Prod_Name;
                                master.Invoice_State = TransactionState.Post;
                                //db.SaveChanges();
                                errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + productName);

                            }
                        }

                    }
                    if (errorMessage.Count() != 0)
                    {


                        string combindedString = string.Join("\n", errorMessage.ToArray());
                        return combindedString;
                    }
                    if (master.HasOff == true)

                    {
                        msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                        master.Invoice_State = TransactionState.Post;
                        db.SaveChanges();
                        return msg;

                    }
                    else
                    {
                        // Customer GL

                        // Revenue GL

                        // Taxes GL
                        AccountCustomerGL.GLDetail_Credit = master.Total_Amount;

                        AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount;

                        // Inventory GL


                        var pay2 = db.PaymentMethods.Find(master.PaymentMethodeID);

                        //var Total_Payment_Method_Cost2 = (master.Total_Amount * (pay2.Percentage.Value) / 100);
                        //bankcomution.GLDetail_Debit = Total_Payment_Method_Cost2;


                        AccountReceivableGL.GLDetail_Debit = master.Total_Amount + ExtraTax;
                        extraGL.GLDetail_Credit = ExtraTax;
                        foreach (var detail in master.ReverseOrderDetails)
                        {
                            var TotalBefore = detail.Product_Price * detail.Quantity;
                            revenueGL.GLDetail_Credit += TotalBefore;
                        }
                        discount.GLDetail_Debit = revenueGL.GLDetail_Credit * (master.DicountInPrecentage / 100);
                        // Update store quantity
                        foreach (var detail in master.ReverseOrderDetails)
                        {
                            var record = detailInvoice.Where(m => m.ProductID == detail.ProductID).FirstOrDefault();
                            record.Reverse_Quantity = record.Reverse_Quantity + detail.Quantity;
                            record.Reverse_Total_Tax = record.Reverse_Total_Tax + detail.Detail_Total_Tax;
                            record.Reverse_Total_Amount = record.Reverse_Total_Amount + detail.Total_Amount;
                            db.Entry(record).State = EntityState.Modified;
                            db.SaveChanges();


                        }
                        foreach (var detail in RealDetails)
                        {
                            var thisproduct = db.Products.Find(detail.ProductID);
                            if (thisproduct.Prod_Type == Prod_Type.اولي || thisproduct.Prod_Type == Prod_Type.منتج)
                            {

                                ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.ProductID && p.Store_ID == store.Store_ID).FirstOrDefault();
                                //detailBatch.Product_Quantity -= detail.Product_Quantity;
                                //detailBatch.Batch_Cost -= detailBatch.Product_Cost * detail.Product_Quantity;
                                var storeCardOLd = db.StoreCards.Where(a => a.Product_Id == detail.ProductID && a.Document_Serial == invoiceMaster.OrderNumber).FirstOrDefault();
                                detailBatch.Product_Quantity -= detail.Quantity;
                                detailBatch.Batch_Cost -= detail.Quantity * storeCardOLd.CostPrice;
                                if (detailBatch.Product_Quantity == 0)
                                {
                                    detailBatch.Product_Cost = 0;

                                }
                                else
                                {
                                    detailBatch.Product_Cost = (detailBatch.Batch_Cost / detailBatch.Product_Quantity);
                                }

                                db.Entry(detailBatch).State = EntityState.Modified;

                                var storecard = new StoreCard();
                                if (detail.Quantity == 0)
                                {
                                    storecard.CostPrice = 0;
                                }
                                else
                                {
                                    storecard.CostPrice = detail.Detail_Total_Cost / detail.Quantity;
                                }
                                storecard.Date = DateTime.Now;

                                storecard.Store_Id = store.Store_ID;
                                storecard.Quantity = detail.Quantity;
                                storecard.SalePrice = detail.Product_Price;
                                storecard.Product_Id = detail.ProductID;
                                storecard.IncreaseQuantity = false;
                                storecard.TypeOfDocument = "استبعاد فاتورة مردود فورى";
                                storecard.Document_Serial = master.OrderNumber;
                                storecard.TotalCost = detail.Quantity * storeCardOLd.CostPrice;
                                storecard.CostPrice = storeCardOLd.CostPrice;
                                storecard.TotalAmount = detail.Total_Amount;
                                storecard.Account_Id = AccountReceivableGL.Account_Child_ID;

                                storecard.Subledger_Id = revenueGL.SubLedger_Id;
                                Store.AddinginstorCard(storecard);
                                master.Total_Cost += storecard.TotalCost.Value;

                            }
                        }
                        // Total Cost GL
                        totalCostGL.GLDetail_Debit = master.Total_Cost;
                        inventoryGL.GLDetail_Credit = master.Total_Cost;
                    }
                    break;
                default:
                    break;
            }
            if (msg == "")
            {
                // Add all GL details to the master
                glMaster.GLDetails.Add(AccountReceivableGL);
                glMaster.GLDetails.Add(revenueGL);
                if (inventoryGL.GLDetail_Credit != inventoryGL.GLDetail_Debit)
                {
                    glMaster.GLDetails.Add(inventoryGL);
                    glMaster.GLDetails.Add(totalCostGL);
                }
                if (extraGL.GLDetail_Credit != extraGL.GLDetail_Debit)
                {
                    glMaster.GLDetails.Add(extraGL);

                }

                if (bankcomution.GLDetail_Credit == 0 && bankcomution.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glMaster.GLDetails.Add(bankcomution);

                }
                if (discount.GLDetail_Credit == 0 && discount.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glMaster.GLDetails.Add(discount);

                }
                if (master.CustomerID != 0 && master.CustomerID != null && master.PaymentMethodeID != 0)
                {
                    glMaster.GLDetails.Add(AccountCustomerGL);
                    glMaster.GLDetails.Add(AccountCustomerGLDebit);
                }
                glMaster = AddglDetailTaxForReverse(glMaster, master.Id);
                db.GLMasters.Add(glMaster);
                GLMaster.GetBalanceForAccount(glMaster);
                GLMaster.GetBalanceForSubLedger(glMaster);
                msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                db.SaveChanges();
            }
            if (master.Invoice_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }
            return msg;
        }
        public static List<ReverseOrderDetail> GetMaterialOCompoundProduct(List<ReverseOrderDetail> Details, int Store_Id)
        {
            var db = new AccountingContainer();
            var ProductAll = db.Products.ToList();
            var project = ProductAll.Where(a => a.Prod_Type == Prod_Type.مجمع).Select(a => a.Prod_ID).ToList();
            var ProList = Details.Where(s => project.Contains(s.ProductID)).ToList();
            if (ProList.Count() == 0)
            {

            }
            else
            {

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.ProductID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.ProductID == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new ReverseOrderDetail();
                            Detailnew.ProductID = subProduct.Product_Id;
                            Detailnew.Quantity = subProduct.Quantity * item.Quantity;

                            Detailnew.PatchId = batch.Batch_ID;

                            if (batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Quantity;

                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;


                            }
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new ReverseOrderDetail();
                            Detailnew.ProductID = subProduct.Product_Id;
                            Detailnew.Quantity = dublicate.Quantity + (item.Quantity * subProduct.Quantity);

                            Detailnew.PatchId = batch.Batch_ID;

                            if (batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Quantity;

                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;


                            }
                            Details.Remove(dublicate);
                            Details.Add(Detailnew);

                        }
                        var Delete = Details.Where(a => a.ProductID == subProduct.Master_Product_Id).FirstOrDefault();

                        Details.Remove(Delete);

                    }





                }

            }

            return Details;
        }

        public static GLMaster AddglDetailTaxForReverse(GLMaster gLMaster, int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceDetail = db.ReverseOrders.Find(invoiceId).ReverseOrderDetails;
            var numberofdifferentTax = invoiceDetail.Where(a => a.TaxId != 0).Select(a => a.TaxId).Distinct();
            if (numberofdifferentTax.Count() == 0)
            {
                return gLMaster;
            }
            foreach (var tax in numberofdifferentTax)
            {
                var gldetail = new GLDetail();
                var thistax = db.Taxes.Find(tax).Tax_Code;
                var taxChart = db.ChartOfAccountings.Where(a => a.Account_Code == thistax).FirstOrDefault();

                gldetail.Account_Id = taxChart.Account_Id;
                gldetail.Account_Name = taxChart.Account_Name;
                if (gLMaster.GLState == TransactionState.Post)
                {
                    gldetail.GLDetail_Credit = 0;
                    gldetail.GLDetail_Debit = invoiceDetail.Where(a => a.TaxId == tax).Select(a => a.Detail_Total_Tax).Sum();
                }
                else
                {


                    gldetail.GLDetail_Credit = invoiceDetail.Where(a => a.TaxId == tax).Select(a => a.Detail_Total_Tax).Sum();
                    gldetail.GLDetail_Debit = 0;
                }

                gLMaster.GLDetails.Add(gldetail);
            }

            return gLMaster;
        }
        public static decimal updateDiscountDebendOnNet(decimal net, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.TempOrders.Find(Invoice_Id);


            if (invoiceTemp.TempOrderDetails.Count() == 0 || invoiceTemp.TempOrderDetails.Select(a => a.TaxID).Distinct().Count() > 1)
            {
                try
                {
                    return invoiceTemp.DicountInPrecentage;
                }
                catch
                {
                    return 0;
                }
            }
            var detail = invoiceTemp.TempOrderDetails.ToList();
            var taxId = invoiceTemp.TempOrderDetails.FirstOrDefault().TaxID;
            decimal invoiceTax = 0;
            if (taxId > 0)
            {
                invoiceTax = db.Taxes.Find(taxId).Tax_Percentage / 100;
            }
            decimal totalPrice = invoiceTemp.TempOrderDetails.Select(a => a.Quantity * a.ProductPrice).Sum();
            decimal discountTotal = (totalPrice + (invoiceTax * totalPrice) - net) / (1 + invoiceTax);
            decimal percentage = (discountTotal / totalPrice) * 100;
            decimal total = 0;
            foreach (var item in detail)
            {
               var totalamount= item.Quantity * item.ProductPrice;
                var TotalDiscount = totalamount * (percentage / 100);
                decimal? beforeTax = 0;
                if (item.TaxID == 0)
                {
                    item.ProductTaxAmount = 0;
                }
                else
                {
                    var tax = db.Taxes.Find(item.TaxID).Tax_Percentage;
                    beforeTax = totalamount - TotalDiscount;
                    item.ProductTaxAmount = beforeTax.Value * (tax / 100);
                }


               
                db.Entry(item).State = EntityState.Modified;
            
            }
           
            invoiceTemp.DicountInPrecentage = percentage;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return percentage;

        }
        public static String MakeVoidForSales(int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceMaster = db.Orders.Find(invoiceId);

            var Allreverse = db.ReverseOrders.Where(a => a.OrderId == invoiceId).FirstOrDefault();
            if (Allreverse != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.ThierMoreInvoiceForThat;
            }
            else
            {

                var master = new ReverseOrder();


                var lastOrderNumber = db.ReverseOrders.OrderByDescending(c => c.Id).FirstOrDefault();
                if (lastOrderNumber == null)
                {
                    master.OrderNumber = "RORD-001";
                }
                else
                {
                    master.OrderNumber = "RORD-" + (Convert.ToInt32(lastOrderNumber.OrderNumber.Substring(5, lastOrderNumber.OrderNumber.Length - 5)) + 1).ToString("D3");
                }

                var comp = db.Companies.FirstOrDefault();
                if (comp != null)
                {
                    var date = Convert.ToDateTime(DateTime.Now);
                    if (date < comp.StartFinancialDate)
                    {
                        master.ReverseOrderDate = DateTime.Now;
                    }
                    else
                    {
                        master.ReverseOrderDate = DateTime.Now;
                    }
                }
                else
                {
                    master.ReverseOrderDate = DateTime.Now;
                }




                if (master != null)
                {

                    master.OrderId = invoiceMaster.Id;
                    master.DicountInPrecentage = invoiceMaster.DicountInPrecentage;



                    master.CustomerID = invoiceMaster.CustomerID;


                    master.EmployeeID = invoiceMaster.EmployeeID;



                }



                master.Invoice_State = TransactionState.Post;







                decimal Sumtax = 0;
                decimal SumtAmount = 0;
                decimal SumTotalCost = 0;

                foreach (var item in invoiceMaster.OrderDetails.ToList())
                {
                    var details = new ReverseOrderDetail();
                    details.ProductID = item.ProductID;
                    details.Quantity = item.Quantity;
                    details.PatchId = item.PatchId;
                    details.Product_Price = item.Product_Price;
                   





                    item.Detail_Total_Cost = item.Detail_Total_Cost / item.Quantity;

                    item.Reverse_Quantity = 0;

                    details.TaxId = item.TaxId;


                    var salesInvoiceDetail = master.ReverseOrderDetails.Where(a => a.ProductID == item.ProductID).FirstOrDefault();
                    var productBatch = db.ProductBatches.Where(a => a.Batch_ID == item.PatchId ).FirstOrDefault();


                 


                    details.Detail_Total_Cost = (item.Detail_Total_Cost * item.Quantity);


                    decimal TotalBeforeDiscount = item.Product_Price * item.Quantity;
                    decimal discount = TotalBeforeDiscount * (master.DicountInPrecentage / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    if (item.TaxId == 0)
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(item.TaxId).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }



                    var price = details.Product_Price * details.Quantity;
                    details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                    SumtAmount += (decimal)details.Total_Amount;

                    Sumtax += (decimal)details.Detail_Total_Tax;
                    SumTotalCost += (decimal)details.Detail_Total_Cost;
                    master.Total_Tax = Sumtax;
                    master.Total_Amount = SumtAmount;
                    master.Total_Cost = SumTotalCost;


                    master.ReverseOrderDetails.Add(details);
                    master.Invoice_State = TransactionState.Post;


                    db.Entry(item).State = EntityState.Modified;




                    master.ReverseOrderDetails.Add(details);


                }

                master.CustomerID = invoiceMaster.CustomerID;

                master.PaymentMethodeID = invoiceMaster.PaymentMethodeID;


                master.DicountInPrecentage = invoiceMaster.DicountInPrecentage;
                master.Number = InvoiceSerial.generateSerialForReverse();
                db.ReverseOrders.Add(master);

                db.SaveChanges();

                DateTime DateNow = DateTime.Now;




                var transBatch = new TransactionBatch();
                transBatch.Date = DateNow;
                transBatch.OperationName = "مردود مبيعات كاشير";
                transBatch.OperationId = master.Id;
                transBatch.Operation_Code = master.OrderNumber;
                transBatch.Description = "عكس قيد مبيعات كاشير";
                transBatch.UserId = "noon";
                db.TransactionBatches.Add(transBatch);


                MakeSalesinQrSales(master);
                var msg = CreateReverseInvoiceGL(master);
                db.SaveChanges();
                db.Dispose();

                return msg;




            }



        }

        public static bool MakeSalesinQrSales(ReverseOrder revmaster)
        {
            var db = new AccountingContainer();


            var qrmaster = new ReverseQRInvoice();
            var master = db.Orders.Find(revmaster.OrderId);

            qrmaster.Account_Id = 76;
            qrmaster.Number = revmaster.Number;
            qrmaster.DateOfCreate = DateTime.Now;
            qrmaster.ApprovalCode = "";
            qrmaster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            qrmaster.Customer_Id = revmaster.Order.CustomerID;
            qrmaster.Description = "";
            qrmaster.Exchange_Rate = 0;
            qrmaster.HasOff = revmaster.HasOff;
            qrmaster.Invoice_Currency = "ريال سعودي";
            qrmaster.Invoice_Date = revmaster.ReverseOrderDate;
            qrmaster.Invoice_State = revmaster.Invoice_State;
            qrmaster.MasterQRInvoiceId = master.QRNumber.Value;
            qrmaster.Master_Discount = revmaster.DicountInPrecentage;
            qrmaster.PaymentMethod_Id = revmaster.PaymentMethodeID;
            qrmaster.Reference = "";
            qrmaster.Serial_Number = revmaster.OrderNumber;
            qrmaster.Store_ID = revmaster.Order.StoreId.Value;
         
            var subledgerId = db.Employees.Find(master.EmployeeID).Subldger_Id;
            if (subledgerId == 0 || subledgerId == null)
            {
                subledgerId = db.SubLedgers.FirstOrDefault().SubLed_Id;
            }
            qrmaster.SubLedger_ID = subledgerId;
            qrmaster.Total_Amount = revmaster.Total_Amount;
            qrmaster.Total_Cost = revmaster.Total_Cost;
            qrmaster.Total_Payment_Method_Cost = 0;
            qrmaster.Total_Tax = revmaster.Total_Tax;
            var qrmmaster = db.MasterQRInvoices.Find(qrmaster.MasterQRInvoiceId);
            qrmaster.ReversePOSDetails = new List<ReverseQRDetail>();
            foreach (var item in revmaster.ReverseOrderDetails)
            {
                var qrdetail = new ReverseQRDetail();
                if (item.PatchId != null)
                {
                    qrdetail.Batch_ID = item.PatchId;
                }
                qrdetail.DetailQRInvoiceDetail_Id = qrmmaster.DetailQRInvoices.Where(a => a.Product_Id == item.ProductID).FirstOrDefault().Detail_Id;
                qrdetail.Detail_Discount = 0;
                qrdetail.Detail_Discount_Amount = 0;
                qrdetail.Detail_Total_Cost = item.Detail_Total_Cost;
                qrdetail.Detail_Total_Tax = item.Detail_Total_Tax;
                qrdetail.Product_Id = item.ProductID;
                qrdetail.Product_Price = item.Product_Price;
                qrdetail.Product_Quantity = item.Quantity;
                qrdetail.Product_Tax = Convert.ToInt32(item.TaxId);
                qrdetail.Total_Amount = item.Total_Amount;

                qrmaster.ReversePOSDetails.Add(qrdetail);

            }
            db.ReverseQRInvoices.Add(qrmaster);
            db.SaveChanges();

            return true;
        }
    }
}