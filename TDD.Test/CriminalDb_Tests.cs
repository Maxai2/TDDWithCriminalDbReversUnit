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
        //------------------------------------------------------------------------------------------------
        //факт: передаем нормальный объект и не ждем исключение
        [Fact]
        void Imprision_With_Normal_Criminal()
        {
            var db = new CriminalDb();
            var criminal = new Criminal
            {
                Code = "Aadas221",
                Crime = "Help nan",
                DateOfBirth = new DateTime(2001, 2, 1),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 50);
        }
        //------------------------------------------------------------------------------------------------
        //факт: передаем нулл и ждем исключение
        [Fact]
        void Imprison_Criminal_Null_Test()
        {
            var db = new CriminalDb();
            var criminal = default(Criminal);
            int term = 10;

            var ex = Assert.ThrowsAny<Exception>(() => db.Imprison(criminal, term));

            Assert.IsType<ArgumentNullException>(ex);
        }
        //------------------------------------------------------------------------------------------------
        /*	term должен быть больше 0 и меньше или равно 5000
		теория: передаем несколько валидных days и не ждем исключение*/
        [Theory]
        [InlineData(1), InlineData(5), InlineData(15), InlineData(365), InlineData(5000)]
        void Imprison_Test_With_True_Term(int term)
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
        //------------------------------------------------------------------------------------------------
        /*term должен быть больше 0 и меньше или равно 5000
		теория: передаем несколько невалидных days и ждем исключение*/
        [Theory]
        [InlineData(0), InlineData(-32), InlineData(-15), InlineData(-365), InlineData(5050), InlineData(6000)]
        void Imprison_Test_With_False_Term(int term)
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
        //------------------------------------------------------------------------------------------------
        /*	возраст criminal должен быть больше 8 лет
		теория: передаем несколько criminal с валидной датой рождения и не ждем исключение*/
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

        //------------------------------------------------------------------------------------------------
        /*	возраст criminal должен быть больше 8 лет
		теория: передаем несколько criminal с невалидной датой рождения и ждем исключение*/
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


        //------------------------------------------------------------------------------------------------
        // преступник не должен быть уже заключен (если пытаемся посадить Jailed преступника - ловим исключение)

        /*	criminal не должен быть Jailed на момент вызова метода 
         *	факт: передаем criminal с Jailed = true и ждем исключение*/
        [Fact]
        void Imprision_Test_Criminal_Jailed_True()
        {
            var db = new CriminalDb();
            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 50);

            var ex = Assert.ThrowsAny<Exception>(() => db.Imprison(criminal, 20));

            Assert.IsType<ArgumentException>(ex);
        }

        //------------------------------------------------------------------------------------------------
        /*
	    criminal не должен быть Jailed на момент вызова метода
		факт: передаем criminal с Jailed = false и не ждем исключение*/
        [Fact]
        void Imprision_Test_Criminal_Jailed_False()
        {
            var db = new CriminalDb();
            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 50);
        }

        //------------------------------------------------------------------------------------------------
        /*
	    если criminal уже сидел, нужно обновить старую запись, а не добавлять новую
		факт: передаем существующий criminal и проверяем, не увеличилось ли количество в листе*/

        [Fact]
        void Imprission_List_Not_Add_If_Criminal_Was()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 50);

            int temp = db.GetAll().Count();

            db.Release(criminal);

            db.Imprison(criminal, 20);

            Assert.True(temp == db.GetAll().Count());
        }

        //------------------------------------------------------------------------------------------------

        /*если criminal уже сидел, нужно обновить старую запись, а не добавлять новую
		факт: передаем новый criminal и проверяем, увеличилось ли количество в листе*/

        [Fact]
        void Imprission_List_Add_If_Criminal_New()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };
            int temp = db.GetAll().Count();

            db.Imprison(criminal, 50);

            Assert.True(db.GetAll().Count() == temp + 1);
        }

        //------------------------------------------------------------------------------------------------

        /*	criminal не должен быть null
		факт: передаем нулл и ждем исключение*/

        [Fact]
        void Release_Null_Criminal_Test()
        {
            var db = new CriminalDb();

            var criminal = default(Criminal);

            var ex = Assert.ThrowsAny<Exception>(() => db.Release(criminal));

            Assert.IsType<ArgumentNullException>(ex);
        }

        //------------------------------------------------------------------------------------------------

        /*при использовании Realese надо проверить что Criminal есть в коллекции*/

        [Fact]
        void Release_Criminal_In_Collection()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            var ex = Assert.ThrowsAny<Exception>(() => db.Release(criminal));

            Assert.IsType<KeyNotFoundException>(ex);
        }


        //------------------------------------------------------------------------------------------------

        /*
	    criminal не должен быть null
		факт: передаем нормальный объекти не ждем исключение*/

        [Fact]
        void Release_Criminal_Test()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 60);

            db.Release(criminal);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    criminal должен быть Jailed
		факт: передаем criminal с Jailed = false и ждем исключение*/

        [Fact]
        void Release_Criminal_Test_Jailed_False()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 60);

            db.Release(criminal);

            var ex = Assert.ThrowsAny<Exception>(() => db.Release(criminal));

            Assert.IsType<ArgumentException>(ex);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    criminal должен быть Jailed
		факт: передаем criminal с Jailed = true и не ждем исключение*/

        [Fact]
        void Release_Criminal_Test_Jailed_True()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 60);

            db.Release(criminal);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    удалять criminal из листа не нужно
		факт: передаем criminal и проверяем, не уменьшилось ли количество в листе*/

        [Fact]
        void Release_Criminal_Test_Count_Criminals()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 60);

            int tempI = db.GetAll().Count();

            db.Release(criminal);

            int tempR = db.GetAll().Count();

            Assert.True(tempI == tempR);
        }

        //------------------------------------------------------------------------------------------------

        /*	
         *	должен возвращать коллекцию объектов Criminal
	        если коллекция пуста - должен возвращать пустую коллекцию*/

        [Fact]
        void GetAll_Test_Return_Empty_Collection()
        {
            var db = new CriminalDb();

            //var action = new Action(() => db.GetAll());

            Assert.Empty(db.GetAll());
        }

        //------------------------------------------------------------------------------------------------

        /*id должен быть больше 0
		теория: передаем несколько невалидных id и ждем исключение*/

        [Theory]
        [InlineData(0), InlineData(-20), InlineData(-4560), InlineData(-10)]
        void Get_False_Id_Test(int id)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 60);

            var ex = Assert.ThrowsAny<Exception>(() => db.Get(id));

            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        //------------------------------------------------------------------------------------------------ 

        /*id должен быть больше 0
		теория: передаем несколько валидных id и не ждем исключение*/

        [Theory]
        [InlineData(1), InlineData(2), InlineData(5)]
        void Get_True_Id_Test(int id)
        {
            var db = new CriminalDb();

            var criminal1 = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal1, 60);

            var criminal2 = new Criminal
            {
                Code = "A416",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 2,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal2, 60);

            var criminal5 = new Criminal
            {
                Code = "A426",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 5,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal5, 60);

            db.Get(id);
        }

        //------------------------------------------------------------------------------------------------

        /*	если Criminal с таким id не найден - метод должен вернуть null
		теория: передаем несколько невалидных id и ждем null*/

        [Theory]
        [InlineData(156), InlineData(456), InlineData(7894), InlineData(1562)]
        void Get_False_Id_Test_And_Wait_Null(int id)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 60);

            Assert.Null(db.Get(id));
        }

        //------------------------------------------------------------------------------------------------

        /*	если Criminal найден - метод должен вернуть ссылку на него
		теория: передаем несколько валидных id и ждем объекты типа Criminal ????*/

        [Theory]
        [InlineData(1)]
        void Get_False_Id_Test_And_Wait_Criminal(int id)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 60);

            Assert.Equal(criminal, db.Get(id));
        }

        //------------------------------------------------------------------------------------------------

        /*	criminal не должен быть null
		факт: передаем нулл и ждем исключение*/

        [Fact]
        void IncreaseTerm_Send_Null_And_Wait_Exception_Test()
        {
            var db = new CriminalDb();

            var ex = Assert.ThrowsAny<Exception>(() => db.IncreaseTerm(null, 50));

            Assert.IsType<ArgumentNullException>(ex);
        }

        //------------------------------------------------------------------------------------------------

        /*при использовании Get надо проверить что Criminal есть в коллекции*/

        [Fact]
        void IncreaseTerm_Criminal_In_Collection()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            var ex = Assert.ThrowsAny<Exception>(() => db.IncreaseTerm(criminal, 10));

            Assert.IsType<KeyNotFoundException>(ex);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    criminal не должен быть null
		факт: передаем нормальный объект и не ждем исключение*/

        [Fact]
        void IncreaseTerm_Send_Normal_Criminal()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 50);

            db.IncreaseTerm(criminal, 60);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    days должен быть положительный и не превышать 5000
		теория: передаем несколько валидных days и не ждем исключение*/

        [Theory]
        [InlineData(20), InlineData(365), InlineData(4900)]
        void IncreaseTerm_Send_True_Days(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 10);

            db.IncreaseTerm(criminal, days);
        }

        //------------------------------------------------------------------------------------------------

        /*
        days должен быть положительный и не превышать 5000
        теория: передаем несколько невалидных days и ждем исключение*/

        [Theory]
        [InlineData(-50), InlineData(-123), InlineData(6000)]
        void IncreaseTerm_Send_False_Days(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 10);

            var ex = Assert.ThrowsAny<Exception>(() => db.IncreaseTerm(criminal, days));

            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    метод должен увеличивать DaysTerm переданного criminal
		теория: передаем несколько валидных days и проверяем увеличение DaysTerm*/

        [Theory]
        [InlineData(20), InlineData(365), InlineData(4900)]
        void IncreaseTerm_Send_True_Days_And_Check_Their_Increase(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 10);

            int tempDays = db.Get(1).DaysTerm;

            db.IncreaseTerm(criminal, days);

            Assert.Equal(tempDays + days, db.Get(1).DaysTerm);
        }

        //------------------------------------------------------------------------------------------------

        /*суммарный DaysTerm не должен превышать 5000 (округлять до 5000)
		теория: передаем несколько days, в том числе очень больших и проверяем значение DaysTerm*/

        [Theory]
        [InlineData(5000), InlineData(4800), InlineData(4900)]
        void IncreaseTerm_Send_True_Days_And_Check_Their_Sum(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 500);

            db.IncreaseTerm(criminal, days);

            Assert.Equal(5000, db.Get(1).DaysTerm);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    criminal не должен быть null
		факт: передаем нулл и ждем исключение*/

        [Fact]
        void DecreaseTerm_Send_Null_And_Wait_Exc()
        {
            var db = new CriminalDb();

            var criminal = default(Criminal);

            var ex = Assert.ThrowsAny<Exception>(() => db.DecreaseTerm(criminal, 20));

            Assert.IsType<ArgumentNullException>(ex);
        }


        //------------------------------------------------------------------------------------------------

        /*при использовании Get надо проверить что Criminal есть в коллекции*/

        [Fact]
        void DecreaseTerm_Criminal_In_Collection()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            var ex = Assert.ThrowsAny<Exception>(() => db.DecreaseTerm(criminal, 10));

            Assert.IsType<KeyNotFoundException>(ex);
        }

        //------------------------------------------------------------------------------------------------

        /*	criminal не должен быть null
		факт: передаем нормальный объект и не ждем исключение*/

        [Fact]
        void DecreaseTerm_Send_Normal()
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 50);

            db.DecreaseTerm(criminal, 20);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    days должен быть положительный и не превышать 5000 
		теория: передаем несколько валидных days и не ждем исключение*/

        [Theory]
        [InlineData(20), InlineData(365), InlineData(450)]
        void DecreaseTerm_Send_True_Days(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 500);

            db.DecreaseTerm(criminal, days);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    days должен быть положительный и не превышать 5000 
		теория: передаем несколько невалидных days и ждем исключение*/

        [Theory]
        [InlineData(-50), InlineData(-123), InlineData(6000)]
        void DecreaseTerm_Send_False_Days(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 500);

            var ex = Assert.ThrowsAny<Exception>(() => db.DecreaseTerm(criminal, days));

            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        //------------------------------------------------------------------------------------------------

        /*
	    метод должен уменьшать DaysTerm переданного criminal
		теория: передаем несколько валидных days и проверяем уменьшение DaysTerm*/

        [Theory]
        [InlineData(20), InlineData(365), InlineData(450)]
        void DecreaseTerm_Send_True_Days_And_Check_Their_Decreaser(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 500);

            int tempDays = db.Get(1).DaysTerm;

            db.DecreaseTerm(criminal, days);

            Assert.Equal(tempDays - days, db.Get(1).DaysTerm);
        }

        //------------------------------------------------------------------------------------------------

        /*полученный в результате DaysTerm не должен быть меньше 0
		теория: передаем несколько days, в том числе очень больших и проверяем значение DaysTerm*/

        [Theory]
        [InlineData(40), InlineData(20), InlineData(500)]
        void DecreaseTerm_Send_True_Days_And_Check_Less_Than_Zero(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 10);

            db.DecreaseTerm(criminal, days);

            Assert.Equal(0, db.Get(1).DaysTerm);
        }

        //------------------------------------------------------------------------------------------------

        /*при достижении DaysTerm 0 - нужно выпустить criminal (сделать Jailed = false)
		теория: передаем несколько days, которые точно уменьшат DaysTerm до 0 и проверяем Jailed*/

        [Theory]
        [InlineData(40), InlineData(20), InlineData(500)]
        void DecreaseTerm_Send_True_Days_And_Check_Jailed_Is_False(int days)
        {
            var db = new CriminalDb();

            var criminal = new Criminal
            {
                Code = "A456",
                Crime = "Kill man",
                DateOfBirth = new DateTime(2000, 2, 12),
                Id = 1,
                Name = "qwerty",
                Surname = "Sqwerty"
            };

            db.Imprison(criminal, 10);

            db.DecreaseTerm(criminal, days);

            Assert.False(criminal.Jailed);
        }





















        //[Theory]
        //[InlineData(0), InlineData(-5), InlineData(Int32.MaxValue), InlineData(Int32.MinValue)]
        //void Imprison_Term_Should_Be_Positive_test_Exception(int term)
        //{
        //    var db = new CriminalDb();
        //    var criminal = new Criminal
        //    {
        //        Id = 1,
        //        Name = "asd",
        //        Surname = "asda",
        //        Code = "123",
        //        Crime = "stole bike",
        //        DateOfBirth = new DateTime(1998, 12, 12)
        //    };

        //    var ex = Assert.ThrowsAny<Exception>(() => db.Imprison(criminal, term));

        //    Assert.IsType<ArgumentOutOfRangeException>(ex);
        //}
    }
}
