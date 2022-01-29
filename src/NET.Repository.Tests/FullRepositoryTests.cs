using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

using NET.Repository.Application.Implementation;
using NET.Repository.Tests.Fakes.DTO;
using NET.Repository.Tests.Fakes.Specifications;
using NET.Repository.Full.Utility;
using static NET.Repository.Tests.Utility.UnifiedMethods;
using NET.Repository.Full;

#pragma warning disable xUnit1013

namespace NET.Repository.Tests
{
    public class FullRepositoryTests
    {
        /***************
        * `Get` testing
        */
        [Fact]
        public void Get_NoData_ReturnEmpty()
        {
            var mockContext = new Mock<DbContext>();
            var mockSet = GetQueryableMockDbSet<Cartoon>(Enumerable.Empty<Cartoon>());
            var repository = new FullRepository<Cartoon>(mockContext.Object, mockSet.Object);

            var actualResult = repository.Get();

            Assert.NotNull(actualResult);
            Assert.Empty(actualResult);
        }

        [Fact]
        public void Get_FakeData_ReturnIt()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);

            var actualResult = repository.Get();

            var expectedResult = context.Cartoons.ToList();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);
            Assert.Equal(expectedResult, actualResult, new CartoonComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_FakeDataWithSpecs_ReturnFiltrated(short id)
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);

            var actualResult = repository.Get(idSpec);

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            var expectedResult = context.Cartoons.AsQueryable()
                .Where(cartoon => cartoon.Id == id)
                .ToList();

            Assert.Equal(expectedResult, actualResult, new CartoonComparer());
        }

        [Fact]
        public void Get_FakeDataWithInvalidSpecs_ReturnEmpty()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(short.MaxValue);

            var actualResult = repository.Get(idSpec);

            Assert.NotNull(actualResult);
            Assert.Empty(actualResult);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_FakeDataWithPagination_ReturnPaginated(ushort pageSize)
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var paginationContext = new PaginationContext() { PageSize = pageSize };

            var actualResult = repository.Get(pageContext: paginationContext);

            Assert.NotNull(actualResult);
            Assert.True(actualResult.Count() <= paginationContext.PageSize);
        }

        [Fact]
        public void Get_FakeDataWithIncluders_ReturnExtended()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);

            var actualResult = repository.Get(includeProperties: cartoon => cartoon.Episodes);

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);
            Assert.All(actualResult, cartoon =>
            {
                Assert.NotNull(cartoon.Episodes);
                Assert.NotEmpty(cartoon.Episodes);
            });
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void Get_FakeDataWithSpecsAndPagination_ReturnIt(short id, ushort pageSize)
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);
            var paginationContext = new PaginationContext() { PageSize = pageSize };

            var actualResult = repository.Get(
                condition: idSpec,
                pageContext: paginationContext);

            var expectedResult = context.Cartoons.AsQueryable()
                .Where(cartoon => cartoon.Id == id)
                .ToList();

            Assert.Equal(expectedResult, actualResult, new CartoonComparer());

            Assert.NotNull(actualResult);
            Assert.True(actualResult.Count() <= paginationContext.PageSize);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_FakeDataWithSpecsAndIncluders_ReturnIt(short id)
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);

            var actualResult = repository.Get(
                condition: idSpec,
                includeProperties: cartoon => cartoon.Episodes);

            var expectedResult = context.Cartoons.AsQueryable()
                .Where(cartoon => cartoon.Id == id)
                .ToList();

            Assert.Equal(expectedResult, actualResult, new CartoonComparer());

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);
            Assert.All(actualResult, cartoon =>
            {
                Assert.NotNull(cartoon.Episodes);
                Assert.NotEmpty(cartoon.Episodes);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_FakeDataWithPaginationAndIncluders_ReturnIt(ushort pageSize)
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var paginationContext = new PaginationContext() { PageSize = pageSize };

            var actualResult = repository.Get(
                pageContext: paginationContext,
                includeProperties: cartoon => cartoon.Episodes);

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);
            Assert.All(actualResult, cartoon =>
            {
                Assert.NotNull(cartoon.Episodes);
                Assert.NotEmpty(cartoon.Episodes);
            });
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void Get_FakeDataWithSpecsAndPaginationAndIncluders_ReturnIt(short id, ushort pageSize)
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);
            var paginationContext = new PaginationContext() { PageSize = pageSize };

            var actualResult = repository.Get(
                condition: idSpec,
                pageContext: paginationContext,
                includeProperties: cartoon => cartoon.Episodes);

            var expectedResult = context.Cartoons.AsQueryable()
                .Where(cartoon => cartoon.Id == id)
                .ToList();

            Assert.Equal(expectedResult, actualResult, new CartoonComparer());

            Assert.NotNull(actualResult);
            Assert.True(actualResult.Count() <= paginationContext.PageSize);

            Assert.All(actualResult, cartoon =>
            {
                Assert.NotNull(cartoon.Episodes);
                Assert.NotEmpty(cartoon.Episodes);
            });
        }
        /***************
        * `Find` testing
        */
        [Fact]
        public void Find_NoData_ReturnDefault()
        {
            var mockContext = new Mock<DbContext>();
            var mockSet = GetQueryableMockDbSet<Cartoon>(Enumerable.Empty<Cartoon>());
            var repository = new FullRepository<Cartoon>(mockContext.Object, mockSet.Object);

            var actualResult = repository.Find();

            Assert.Null(actualResult);
        }

        [Fact]
        public void Find_FakeData_ReturnIt()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);

            var actualResult = repository.Find();

            var expectedResult = context.Cartoons.First();

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult, new CartoonComparer());

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Find_FakeDataWithSpecs_ReturnFiltrated(short id)
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);

            var actualResult = repository.Find(idSpec);

            var expectedResult = context.Cartoons.AsQueryable()
                .First(cartoon => cartoon.Id == id);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult, new CartoonComparer());
        }

        [Fact]
        public void Find_FakeDataWithInvalidSpecs_ReturnEmpty()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(short.MaxValue);

            var actualResult = repository.Find(idSpec);

            Assert.Null(actualResult);
        }

        [Fact]
        public void Find_FakeDataWithIncluders_ReturnExtended()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);

            var actualResult = repository.Find(includeProperties: cartoon => cartoon.Episodes);

            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.Episodes);
            Assert.NotEmpty(actualResult.Episodes);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Find_FakeDataWithSpecsAndIncluders_ReturnIt(short id)
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);

            var actualResult = repository.Find(
                condition: idSpec,
                includeProperties: cartoon => cartoon.Episodes);

            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.Episodes);
            Assert.NotEmpty(actualResult.Episodes);
        }

        /******************
        * `Create` testing
        */
        [Fact]
        public void Create_NewData_ReturnWithNoExcept()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            short newItemId = 5;

            repository.Insert(new Cartoon() { Id = newItemId, Name = It.IsAny<string>() });
            context.SaveChanges();

            var newItem = context.Cartoons.AsQueryable()
                .First(cartoon => cartoon.Id == newItemId);

            Assert.NotNull(newItem);
        }

        [Fact]
        public void Create_ExistingData_ReturnWithException()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var existingItem = context.Cartoons.First();

            var exception = Assert.ThrowsAny<Exception>(() =>
            {
                repository.Insert(new Cartoon() { Id = existingItem.Id, Name = existingItem.Name });
                context.SaveChanges();
            });

            Assert.NotNull(exception);
        }

        /******************
        * `Update` testing
        */
        [Fact]
        public void Update_ExistingData_ReturnWithNoExcept()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var existingItem = context.Cartoons.First();
            var existingItemId = existingItem.Id;
            var newNameData = "Tribute in 2024...";

            existingItem.Name = newNameData;

            repository.Update(existingItem);
            context.SaveChanges();

            Assert.Contains(existingItem, context.Cartoons);
        }

        [Fact]
        public void Update_UnknownData_ReturnWithException()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var unknownItem = new Cartoon() { Id = 3, Name = It.IsAny<string>() };

            var exception = Assert.ThrowsAny<Exception>(() =>
            {
                repository.Update(unknownItem);
                context.SaveChanges();
            });

            Assert.NotNull(exception);
        }

        /******************
        * `Delete` testing
        */
        [Fact]
        public void Delete_ExistingData_ReturnWithNoExcept()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var existingItem = context.Cartoons.First();

            repository.Delete(existingItem);
            context.SaveChanges();
            
            Assert.DoesNotContain(existingItem, context.Cartoons);
        }

        [Fact]
        public void Delete_UnknownData_ReturnWithException()
        {
            using var context = GetDefaultCartoonContext();
            var repository = new FullRepository<Cartoon>(context, context.Cartoons);
            var unknownItem = new Cartoon() { Id = 3, Name = It.IsAny<string>() };

            var exception = Assert.ThrowsAny<Exception>(() =>
            {
                repository.Delete(unknownItem);
                context.SaveChanges();
            });

            Assert.NotNull(exception);
        }
    }
}

#pragma warning restore xUnit1013