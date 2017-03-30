using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Tests
{
    [TestClass()]
    public class PartnerProviderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Database.InitializeConnection(AppDomain.CurrentDomain.BaseDirectory + "\\Database.mdf");
            Database.InitializeTable<PartnerEntity>();
        }

        [TestMethod()]
        public void AddParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                PartnerProvider.Add(
                    new PartnerEntity()
                    {
                        Name = name
                    });
                PartnerProvider.Add(
                    new PartnerEntity()
                    {
                        Name = name,
                        Id = 1,
                    });
                PartnerProvider.Add(null);
            }
        }

        [TestMethod()]
        public void ModifyParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                PartnerProvider.Modify(
                    new PartnerEntity()
                    {
                        Name = name,
                    });
                PartnerProvider.Modify(
                    new PartnerEntity()
                    {
                        Name = name,
                        Id = 1,
                    });
                PartnerProvider.Modify(null);
            }
        }

        [TestMethod()]
        public void RemoveParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                PartnerProvider.Remove(
                    new PartnerEntity()
                    {
                        Name = name,
                    });
                PartnerProvider.Remove(
                    new PartnerEntity()
                    {
                        Name = name,
                        Id = 1,
                    });
                PartnerProvider.Remove(null);
            }
        }

        [TestMethod()]
        public void AddRemoveTest()
        {
            // Empty table
            Database.Remove<PartnerEntity>(p => true);
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => true));

            string[] StringParameters = { "", "a", "1234567890" };
            bool[] BoolParameters = { true, false };

            // Add
            foreach (var name in StringParameters)
                foreach (var code in StringParameters)
                    foreach (var address in StringParameters)
                        foreach (var customer in BoolParameters)
                            foreach (var dealer in BoolParameters)
                                Assert.IsTrue(PartnerProvider.Add(
                                    new PartnerEntity()
                                        {
                                            Name = name,
                                            Code = code,
                                            AccountNumber = address,
                                            Address = address,
                                            Customer = customer,
                                            Dealer = dealer,
                                            Email = address,
                                            Phone = address                                
                                        }));
            // Remove
            foreach (var parameter in StringParameters)
            {
                Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Name == parameter));
                var list = PartnerProvider.List(p => p.Name == parameter);
                foreach(var partner in list)
                    Assert.IsTrue(PartnerProvider.Remove(partner));
                Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Name == parameter));
                foreach (var partner in list)
                    Assert.IsFalse(PartnerProvider.Remove(partner));
            }
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => true));
        }

        [TestMethod()]
        public void ModifyTest()
        {
            // Empty table
            Database.Remove<PartnerEntity>(p => true);
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => true));
            string PartnerName1 = "Partner 1";
            string PartnerName2 = "Partner 2";

            // Add Partner 1
            Assert.IsTrue(PartnerProvider.Add(
                new PartnerEntity()
                {
                    Name = PartnerName1,
                    Code = PartnerName1,
                    AccountNumber = PartnerName1,
                    Address = PartnerName1,
                    Customer = false,
                    Dealer = false,
                    Email = PartnerName1,
                    Phone = PartnerName1
                }));

            // Partner 1 exists
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Name == PartnerName1));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Code == PartnerName1));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.AccountNumber == PartnerName1));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Address == PartnerName1));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Email == PartnerName1));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Phone == PartnerName1));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => !p.Customer));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => !p.Dealer));

            // Partner 2 does not exist
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Name == PartnerName2));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Code == PartnerName2));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.AccountNumber == PartnerName2));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Address == PartnerName2));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Email == PartnerName2));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Phone == PartnerName2));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Customer));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Dealer));

            // Prepare modified partner with the same Id
            var list = PartnerProvider.List(p => p.Name == PartnerName1);
            var partner = 
                new PartnerEntity()
                    {
                        Id = list.First().Id,
                        Name = PartnerName2,
                        Code = PartnerName2,
                        AccountNumber = PartnerName2,
                        Address = PartnerName2,
                        Customer = true,
                        Dealer = true,
                        Email = PartnerName2,
                        Phone = PartnerName2
                    };
            // Modify
            Assert.IsTrue(PartnerProvider.Modify(partner));
            
            // Partner 1 does not exist
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Name == PartnerName1));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Code == PartnerName1));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.AccountNumber == PartnerName1));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Address == PartnerName1));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Email == PartnerName1));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => p.Phone == PartnerName1));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => !p.Customer));
            Assert.IsFalse(Database.IsExist<PartnerEntity>(p => !p.Dealer));

            // Partner 2 exists
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Name == PartnerName2));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Code == PartnerName2));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.AccountNumber == PartnerName2));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Address == PartnerName2));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Email == PartnerName2));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Phone == PartnerName2));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Customer));
            Assert.IsTrue(Database.IsExist<PartnerEntity>(p => p.Dealer));
        }
    }
}