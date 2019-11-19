﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using RepoDb.Extensions;
using RepoDb.Reflection;
using RepoDb.MySql.IntegrationTests.Models;
using RepoDb.MySql.IntegrationTests.Setup;
using System.Data.Common;
using System.Linq;

namespace RepoDb.MySql.IntegrationTests.Operations
{
    [TestClass]
    public class ExecuteReaderTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        #region Sync

        [TestMethod]
        public void TestExecuteReader()
        {
            using (var connection = new MySqlConnection(Database.ConnectionString))
            {
                // Setup
                var tables = Database.CreateCompleteTables(10);

                // Act
                using (var reader = connection.ExecuteReader("SELECT Id, ColumnInt, ColumnDateTime FROM [CompleteTable];"))
                {
                    while (reader.Read())
                    {
                        // Act
                        var id = reader.GetInt64(0);
                        var columnInt = reader.GetInt32(1);
                        var columnDateTime = reader.GetDateTime(2);
                        var table = tables.FirstOrDefault(e => e.Id == id);

                        // Assert
                        Assert.IsNotNull(table);
                        Assert.AreEqual(columnInt, table.ColumnInt);
                        Assert.AreEqual(columnDateTime, table.ColumnDateTime);
                    }
                }
            }
        }

        [TestMethod]
        public void TestExecuteReaderWithMultipleStatements()
        {
            using (var connection = new MySqlConnection(Database.ConnectionString))
            {
                // Setup
                var tables = Database.CreateCompleteTables(10);

                // Act
                using (var reader = connection.ExecuteReader("SELECT Id, ColumnInt, ColumnDateTime FROM [CompleteTable]; SELECT Id, ColumnInt, ColumnDateTime FROM [CompleteTable];"))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            // Act
                            var id = reader.GetInt64(0);
                            var columnInt = reader.GetInt32(1);
                            var columnDateTime = reader.GetDateTime(2);
                            var table = tables.FirstOrDefault(e => e.Id == id);

                            // Assert
                            Assert.IsNotNull(table);
                            Assert.AreEqual(columnInt, table.ColumnInt);
                            Assert.AreEqual(columnDateTime, table.ColumnDateTime);
                        }
                    } while (reader.NextResult());
                }
            }
        }

        [TestMethod]
        public void TestExecuteReaderAsExtractedEntity()
        {
            using (var connection = new MySqlConnection(Database.ConnectionString))
            {
                // Setup
                var tables = Database.CreateCompleteTables(10);

                // Act
                using (var reader = connection.ExecuteReader("SELECT * FROM [CompleteTable];"))
                {
                    // Act
                    var result = DataReader.ToEnumerable<CompleteTable>((DbDataReader)reader, connection).AsList();

                    // Assert
                    tables.AsList().ForEach(table => Helper.AssertPropertiesEquality(table, result.First(e => e.Id == table.Id)));
                }
            }
        }

        //[TestMethod]
        //public void TestExecuteReaderAsExtractedDynamic()
        //{
        //    using (var connection = new MySqlConnection(Database.ConnectionString))
        //    {
        //        // Setup
        //        var tables = Database.CreateCompleteTables(10);

        //        // Act
        //        using (var reader = connection.ExecuteReader("SELECT * FROM [CompleteTable];"))
        //        {
        //            // Act
        //            var result = DataReader.ToEnumerable((DbDataReader)reader, connection).AsList();

        //            // Assert
        //            tables.AsList().ForEach(table => Helper.AssertMembersEquality(table, result.First(e => e.Id == table.Id)));
        //        }
        //    }
        //}

        #endregion

        #region Async

        [TestMethod]
        public void TestExecuteReaderAsync()
        {
            using (var connection = new MySqlConnection(Database.ConnectionString))
            {
                // Setup
                var tables = Database.CreateCompleteTables(10);

                // Act
                using (var reader = connection.ExecuteReaderAsync("SELECT Id, ColumnInt, ColumnDateTime FROM [CompleteTable];").Result)
                {
                    while (reader.Read())
                    {
                        // Act
                        var id = reader.GetInt64(0);
                        var columnInt = reader.GetInt32(1);
                        var columnDateTime = reader.GetDateTime(2);
                        var table = tables.FirstOrDefault(e => e.Id == id);

                        // Assert
                        Assert.IsNotNull(table);
                        Assert.AreEqual(columnInt, table.ColumnInt);
                        Assert.AreEqual(columnDateTime, table.ColumnDateTime);
                    }
                }
            }
        }

        [TestMethod]
        public void TestExecuteReaderAsyncWithMultipleStatements()
        {
            using (var connection = new MySqlConnection(Database.ConnectionString))
            {
                // Setup
                var tables = Database.CreateCompleteTables(10);

                // Act
                using (var reader = connection.ExecuteReaderAsync("SELECT Id, ColumnInt, ColumnDateTime FROM [CompleteTable]; SELECT Id, ColumnInt, ColumnDateTime FROM [CompleteTable];").Result)
                {
                    do
                    {
                        while (reader.Read())
                        {
                            // Act
                            var id = reader.GetInt64(0);
                            var columnInt = reader.GetInt32(1);
                            var columnDateTime = reader.GetDateTime(2);
                            var table = tables.FirstOrDefault(e => e.Id == id);

                            // Assert
                            Assert.IsNotNull(table);
                            Assert.AreEqual(columnInt, table.ColumnInt);
                            Assert.AreEqual(columnDateTime, table.ColumnDateTime);
                        }
                    } while (reader.NextResult());
                }
            }
        }

        [TestMethod]
        public void TestExecuteReaderAsyncAsExtractedEntity()
        {
            using (var connection = new MySqlConnection(Database.ConnectionString))
            {
                // Setup
                var tables = Database.CreateCompleteTables(10);

                // Act
                using (var reader = connection.ExecuteReaderAsync("SELECT * FROM [CompleteTable];").Result)
                {
                    // Act
                    var result = DataReader.ToEnumerable<CompleteTable>((DbDataReader)reader, connection).AsList();

                    // Assert
                    tables.AsList().ForEach(table => Helper.AssertPropertiesEquality(table, result.First(e => e.Id == table.Id)));
                }
            }
        }

        //[TestMethod]
        //public void TestExecuteReaderAsyncAsExtractedDynamic()
        //{
        //    using (var connection = new MySqlConnection(Database.ConnectionString))
        //    {
        //        // Setup
        //        var tables = Database.CreateCompleteTables(10);

        //        // Act
        //        using (var reader = connection.ExecuteReaderAsync("SELECT * FROM [CompleteTable];").Result)
        //        {
        //            // Act
        //            var result = DataReader.ToEnumerable((DbDataReader)reader, connection).AsList();

        //            // Assert
        //            tables.AsList().ForEach(table => Helper.AssertMembersEquality(table, result.First(e => e.Id == table.Id)));
        //        }
        //    }
        //}

        #endregion
    }
}
