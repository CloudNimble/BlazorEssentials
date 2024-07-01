using CloudNimble.BlazorEssentials.Tests.IndexedDb.Sample;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.BlazorEssentials.Tests.IndexedDb
{


    [TestClass]
    public class IndexedDbDatabaseTests
    {


        [TestMethod]
        public void Constructor_ShouldLoadObjectStores()
        {
            var db = new SampleDB(null);
            db.Should().NotBeNull();
            db.Name.Should().Be("SampleDB");
            db.Version.Should().Be(1);
            db.ObjectStores.Should().NotBeNull().And.HaveCount(2);
            db.ObjectStores[0].Name.Should().Be("SampleEmployees");
            db.ObjectStores[0].AutoIncrement.Should().BeTrue();
            db.SampleEmployees.Should().NotBeNull();
            db.SampleEmployees.Name.Should().Be("SampleEmployees");
            db.ObjectStores[1].Name.Should().Be("SampleCustomers");
            db.ObjectStores[1].AutoIncrement.Should().BeFalse();
            db.SampleCustomers.Should().NotBeNull();
            db.SampleCustomers.Name.Should().Be("SampleCustomers");
            db.SampleCustomers.Indexes.Should().NotBeNull().And.HaveCount(1);
        }

    }

}
