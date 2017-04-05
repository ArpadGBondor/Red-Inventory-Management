using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using EntityLayer;
using System.Collections.Generic;

namespace DataLayer.Tests
{
    [TestClass()]
    public class TransactionProviderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + "\\";
            string dbname = "DataLayer_Tests";
            if (File.Exists(dir + dbname + ".mdf"))
                Database.InitializeConnection(dir, dbname);
            else
                Database.CreateDatabase(dir, dbname);
            Assert.IsTrue(Database.Test());
            Assert.AreEqual(Database.Directory, dir);
            Assert.AreEqual(Database.DbName, dbname);
        }

        [TestMethod()]
        public void ListHeadTest()
        {



            
            
            
            //string
            //PartnerEntity partner = new PartnerEntity() { }

            //TransactionProvider.ListHead(p => true);
        }

        [TestMethod()]
        public void ListBodyTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ListInventoryTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ListInventoryDetailsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ListPartnerTransactionsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void IsExistHeadTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void IsExistBodyTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void TransactionTest()
        {
            for (int i = 1; i <= 10; i++)
                ProductProvider.Add(new ProductListEntity() { Name = string.Format("Product {0}",i), Cost_Price = i, Sell_Price = 2*i , Code = i.ToString()});
            for (int i = 1; i <= 10; i++)
                PartnerProvider.Add(new PartnerEntity() { Code = i.ToString(), Customer = (i % 2 == 0), Dealer = (i % 3 == 0), Name = string.Format("Partner {0}", i) });

            var productList = Database.ListTable<ProductEntity>(p => true);
            var partnerlist = PartnerProvider.List(p => true);

            decimal TotalPrice;
            TransactionHeadEntity head;
            List<TransactionBodyListEntity> body;
            TransactionBodyListEntity bodyEntity;

            foreach (var partner in partnerlist)
            {
                bool Incoming;
                if (partner.Customer)
                {
                    Incoming = false;
                    TotalPrice = 0;
                    body = new List<TransactionBodyListEntity>();
                    foreach (var product in productList)
                    {
                        bodyEntity = new TransactionBodyListEntity()
                            {
                                Body = new TransactionBodyEntity()
                                    {
                                        Price = (Incoming ? product.Cost_Price : product.Sell_Price),
                                        Product_Id = product.Id,
                                        Quantity = product.Id,
                                    },
                                Product = product
                            };

                        TotalPrice += (bodyEntity.Body.Price * bodyEntity.Body.Quantity);
                        body.Add(bodyEntity);
                    }
                    head = new TransactionHeadEntity() { Date = DateTime.Now.ToString("d"), Partner_Id = partner.Id, Incoming = Incoming, TotalPrice = TotalPrice };
                    TransactionProvider.AddOrModifyTransaction(head, body);
                }
                if (partner.Dealer)
                {
                    Incoming = true;
                    TotalPrice = 0;
                    body = new List<TransactionBodyListEntity>();
                    foreach (var product in productList)
                    {
                        bodyEntity = new TransactionBodyListEntity()
                        {
                            Body = new TransactionBodyEntity()
                            {
                                Price = (Incoming ? product.Cost_Price : product.Sell_Price),
                                Product_Id = product.Id,
                                Quantity = product.Id,
                            },
                            Product = product
                        };

                        TotalPrice += (bodyEntity.Body.Price * bodyEntity.Body.Quantity);
                        body.Add(bodyEntity);
                    }
                    head = new TransactionHeadEntity() { Date = DateTime.Now.ToString("d"), Partner_Id = partner.Id, Incoming = Incoming, TotalPrice = TotalPrice };
                    TransactionProvider.AddOrModifyTransaction(head, body);
                }
            }

            var transactionHeadList = TransactionProvider.ListHead(p => true);

            foreach(var headEntity in transactionHeadList)
            {
                body = TransactionProvider.ListBody(p => p.Transaction_Id == headEntity.Head.Id);
                TotalPrice = headEntity.Head.TotalPrice;



            }

        }

        [TestMethod()]
        public void RemoveTransactionTest()
        {
            throw new NotImplementedException();
        }
    }
}