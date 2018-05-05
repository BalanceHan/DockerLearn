using MyCoreBLL;
using System;
using NUnit.Framework;
using MyCoreDAL;
using CommonServiceLocator;
using SolrNet;

namespace XUnitTestCore
{
    [TestFixture]
    public class ThreadTest
    {
        private readonly ThreadFunc threadFunc;
        public ThreadTest()
        {
            threadFunc = new ThreadFunc();
        }

        //[Xunit.Theory]
        //[InlineData(-1)]
        //[InlineData(0)]
        //[InlineData(1)]
        //public void Test1(int value)
        //{
        //    var result = threadFunc.IsPrime(value);
        //    Xunit.Assert.False(result, $"{value}should not be prime");
        //}
        
        [OneTimeSetUp]
        public void Test1()
        {
            Startup.Init<Customer>("http://localhost:57337/solr");
        }

        [Test]
        public void Test2()
        {
            var customer = new Customer
            {
                ID = 1,
                CustomerName = "HanHang",
                CustomerPhone = "13661446092"
            };
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Customer>>();
            solr.Add(customer);
            solr.Commit();
        }

        [Test]
        public void Query()
        {
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Customer>>();
            var results = solr.Query(new SolrQueryByField("CustomerName", "HanHang"));
            Assert.AreEqual(1, results.Count);
            Console.WriteLine(results[0].CustomerPhone);
        }
    }
}
