﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TDD.Test
{
    public class CriminalDb_Tests
    {
        [Theory]
        [InlineData(1), InlineData(5), InlineData(15), InlineData(365), InlineData(1000)]
        void Imprison_Term_Should_Be_Positive_test(int term)
        {
            var db = new CriminalDb();
            var criminal = new Criminal
            {
                Id = 1,
                Name = "asd",
                Surname = "asda",
                Code = "123",
                Crime = "stole bike",
                DateOfBirth = new DateTime(1998, 12, 12)
            };

            db.Imprison(criminal, term);
        }

        [Theory]
        [InlineData(2000, 1, 1), InlineData(2001, 1, 2), InlineData(1991, 1, 1), InlineData(2010, 1, 1)]
        void Imprison_Age_Should_Greater_Than_8_test(int y, int m, int d)
        {
            var db = new CriminalDb();
            int term = 8;
            var criminal = new Criminal
            {
                Id = 1,
                Name = "asd",
                Surname = "asda",
                Code = "123",
                Crime = "stole bike",
                DateOfBirth = new DateTime(y, m, d)
            };

            db.Imprison(criminal, term);
        }

        [Theory]
        [InlineData(2012, 1, 1), InlineData(2017, 1, 2), InlineData(2015, 1, 1), InlineData(2014, 1, 1)]
        void Imprison_Age_Should_Less_Than_8_test(int y, int m, int d)
        {
            var db = new CriminalDb();
            int term = 8;
            var criminal = new Criminal
            {
                Id = 1,
                Name = "asd",
                Surname = "asda",
                Code = "123",
                Crime = "stole bike",
                DateOfBirth = new DateTime(y, m, d)
            };

            var ex = Assert.ThrowsAny<Exception>(() => db.Imprison(criminal, term));

            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        [Theory]
        [InlineData(0), InlineData(-5), InlineData(Int32.MaxValue), InlineData(Int32.MinValue)]
        void Imprison_Term_Should_Be_Positive_test_Exception(int term)
        {
            var db = new CriminalDb();
            var criminal = new Criminal
            {
                Id = 1,
                Name = "asd",
                Surname = "asda",
                Code = "123",
                Crime = "stole bike",
                DateOfBirth = new DateTime(1998, 12, 12)
            };

            var ex = Assert.ThrowsAny<Exception>(() => db.Imprison(criminal, term));

            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        [Fact]
        void Imprison_Criminal_Null_Test()
        {
            var db = new CriminalDb();
            var criminal = default(Criminal);
            int term = 10;

            var ex = Assert.ThrowsAny<Exception>(() => db.Imprison(criminal, term));

            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}