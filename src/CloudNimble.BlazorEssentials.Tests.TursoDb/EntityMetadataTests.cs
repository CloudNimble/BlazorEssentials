using CloudNimble.BlazorEssentials.Tests.TursoDb.Sample;
using CloudNimble.BlazorEssentials.TursoDb.Schema;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CloudNimble.BlazorEssentials.Tests.TursoDb
{

    [TestClass]
    public class EntityMetadataTests
    {

        [TestMethod]
        public void GetOrCreate_ShouldExtractTableName()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();

            metadata.Should().NotBeNull();
            metadata.TableName.Should().Be("users");
        }

        [TestMethod]
        public void GetOrCreate_ShouldExtractPrimaryKey()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();

            metadata.PrimaryKey.Should().NotBeNull();
            metadata.PrimaryKey!.Property.Name.Should().Be("Id");
            metadata.PrimaryKey.ColumnName.Should().Be("Id");
            metadata.PrimaryKey.AutoIncrement.Should().BeTrue();
        }

        [TestMethod]
        public void GetOrCreate_ShouldExtractColumns()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();

            metadata.Columns.Should().NotBeNull();
            metadata.Columns.Should().Contain(c => c.ColumnName == "name" && c.Property.Name == "Name");
            metadata.Columns.Should().Contain(c => c.ColumnName == "email" && c.Property.Name == "Email");
            metadata.Columns.Should().Contain(c => c.ColumnName == "age" && c.Property.Name == "Age");
            metadata.Columns.Should().Contain(c => c.ColumnName == "is_active" && c.Property.Name == "IsActive");
            metadata.Columns.Should().Contain(c => c.ColumnName == "created_at" && c.Property.Name == "CreatedAt");
        }

        [TestMethod]
        public void GetOrCreate_ShouldExtractIndexes()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();

            var indexedColumns = metadata.Columns.Where(c => c.HasIndex).ToList();
            indexedColumns.Should().NotBeNull();
            indexedColumns.Should().HaveCount(1);
            indexedColumns[0].ColumnName.Should().Be("email");
            indexedColumns[0].IsUniqueIndex.Should().BeTrue();
        }

        [TestMethod]
        public void GetOrCreate_ShouldCacheMetadata()
        {
            var metadata1 = EntityMetadataCache.GetOrCreate<User>();
            var metadata2 = EntityMetadataCache.GetOrCreate<User>();

            metadata1.Should().BeSameAs(metadata2);
        }

    }

}
