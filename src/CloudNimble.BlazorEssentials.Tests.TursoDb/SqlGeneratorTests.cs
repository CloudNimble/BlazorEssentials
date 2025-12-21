using CloudNimble.BlazorEssentials.Tests.TursoDb.Sample;
using CloudNimble.BlazorEssentials.TursoDb.Schema;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CloudNimble.BlazorEssentials.Tests.TursoDb
{

    [TestClass]
    public class SqlGeneratorTests
    {

        [TestMethod]
        public void GenerateCreateTable_ShouldCreateValidDDL()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();
            var sql = SqlGenerator.GenerateCreateTable(metadata);

            sql.Should().NotBeNullOrWhiteSpace();
            sql.Should().Contain("CREATE TABLE IF NOT EXISTS users");
            sql.Should().Contain("Id INTEGER PRIMARY KEY AUTOINCREMENT");
            sql.Should().Contain("name TEXT");
            sql.Should().Contain("email TEXT");
            sql.Should().Contain("age INTEGER");
            sql.Should().Contain("is_active INTEGER");
        }

        [TestMethod]
        public void GenerateCreateIndex_ShouldCreateValidDDL()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();
            var indexStatements = SqlGenerator.GenerateCreateIndexes(metadata).ToList();

            indexStatements.Should().NotBeNull();
            indexStatements.Should().HaveCount(1);
            indexStatements[0].Should().Contain("CREATE UNIQUE INDEX");
            indexStatements[0].Should().Contain("ON users (email)");
        }

        [TestMethod]
        public void GenerateSelectAll_ShouldCreateValidQuery()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();
            var sql = SqlGenerator.GenerateSelectAll(metadata);

            sql.Should().Be("SELECT * FROM users");
        }

        [TestMethod]
        public void GenerateSelectByKey_ShouldContainWhereClause()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();
            var sql = SqlGenerator.GenerateSelectByKey(metadata);

            sql.Should().Contain("SELECT * FROM users WHERE Id = ?");
        }

        [TestMethod]
        public void GenerateCount_ShouldContainCountExpression()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();
            var sql = SqlGenerator.GenerateCount(metadata);

            sql.Should().Contain("SELECT COUNT(*)");
            sql.Should().Contain("FROM users");
        }

        [TestMethod]
        public void GenerateDelete_ShouldCreateValidQuery()
        {
            var metadata = EntityMetadataCache.GetOrCreate<User>();
            var sql = SqlGenerator.GenerateDelete(metadata);

            sql.Should().Be("DELETE FROM users WHERE Id = ?");
        }

    }

}
