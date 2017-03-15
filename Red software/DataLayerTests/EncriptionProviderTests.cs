using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Tests
{
    [TestClass()]
    public class EncriptionProviderTests
    {
        [TestMethod()]
        public void EncryptionTest()
        {
            string[] TestPasswords = { "", "a", "god", "1234567890", "qwerty", "qwertz", " ", null };

            foreach (EncriptionProvider.Supported_HA sha in Enum.GetValues(typeof(EncriptionProvider.Supported_HA)))
            {
                foreach (var pw in TestPasswords)
                {
                    var hash = EncriptionProvider.ComputeHash(pw, sha, null);
                    Assert.IsTrue(EncriptionProvider.Confirm(pw, hash, sha));
                }

            }
        }
    }
}