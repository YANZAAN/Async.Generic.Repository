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

#pragma warning disable xUnit1013

namespace NET.Repository.Tests
{
    public class RepositoryTests
    {
        /***************
        * `Get` testing
        */
        [Fact]
        public async Task Get_NoData_ReturnEmpty()
        {
            var mockContext = new Mock<DbContext>();
            var mockSet = GetQueryableMockDbSet<Cartoon>(Enumerable.Empty<Cartoon>());
            var repository = new Repository<Cartoon>(mockContext.Object, mockSet.Object);

            var actualResult = await repository.Get().ToListAsync();

            Assert.NotNull(actualResult);
            Assert.Empty(actualResult);
        }

        [Fact]
        public async Task Get_FakeData_ReturnIt()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);

            var actualResult = await repository.Get().ToListAsync();

            var expectedResult = context.Cartoons.ToList();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);
            Assert.Equal(expectedResult, actualResult, new CartoonComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_FakeDataWithSpecs_ReturnFiltrated(short id)
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);

            var actualResult = await repository.Get(idSpec).ToListAsync();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            var expectedResult = await context.Cartoons.AsQueryable().Where(cartoon => cartoon.Id == id).ToListAsync();

            Assert.Equal(expectedResult, actualResult, new CartoonComparer());
        }

        [Fact]
        public async Task Get_FakeDataWithInvalidSpecs_ReturnEmpty()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(short.MaxValue);

            var actualResult = await repository.Get(idSpec).ToListAsync();

            Assert.NotNull(actualResult);
            Assert.Empty(actualResult);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_FakeDataWithPagination_ReturnPaginated(ushort pageSize)
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var paginationContext = new PaginationContext() { PageSize = pageSize };

            var actualResult = await repository.Get(pageContext: paginationContext).ToListAsync();

            Assert.NotNull(actualResult);
            Assert.True(actualResult.Count <= paginationContext.PageSize);
        }

        [Fact]
        public async Task Get_FakeDataWithIncluders_ReturnExtended()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);

            var actualResult = await repository.Get(includeProperties: cartoon => cartoon.Episodes).ToListAsync();

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
        public async Task Get_FakeDataWithSpecsAndPagination_ReturnIt(short id, ushort pageSize)
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);
            var paginationContext = new PaginationContext() { PageSize = pageSize };

            var actualResult = await repository.Get(
                condition: idSpec,
                pageContext: paginationContext
            ).ToListAsync();

            var expectedResult = await context.Cartoons.AsQueryable()
                                                       .Where(cartoon => cartoon.Id == id)
                                                       .ToListAsync();

            Assert.Equal(expectedResult, actualResult, new CartoonComparer());

            Assert.NotNull(actualResult);
            Assert.True(actualResult.Count <= paginationContext.PageSize);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_FakeDataWithSpecsAndIncluders_ReturnIt(short id)
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);

            var actualResult = await repository.Get(
                condition: idSpec,
                includeProperties: cartoon => cartoon.Episodes
            ).ToListAsync();

            var expectedResult = await context.Cartoons.AsQueryable()
                                                       .Where(cartoon => cartoon.Id == id)
                                                       .ToListAsync();

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
        public async Task Get_FakeDataWithPaginationAndIncluders_ReturnIt(ushort pageSize)
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var paginationContext = new PaginationContext() { PageSize = pageSize };

            var actualResult = await repository.Get(
                pageContext: paginationContext,
                includeProperties: cartoon => cartoon.Episodes
            ).ToListAsync();

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
        public async Task Get_FakeDataWithSpecsAndPaginationAndIncluders_ReturnIt(short id, ushort pageSize)
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);
            var paginationContext = new PaginationContext() { PageSize = pageSize };

            var actualResult = await repository.Get(
                condition: idSpec,
                pageContext: paginationContext,
                includeProperties: cartoon => cartoon.Episodes
            ).ToListAsync();

            var expectedResult = await context.Cartoons.AsQueryable()
                                                       .Where(cartoon => cartoon.Id == id)
                                                       .ToListAsync();

            Assert.Equal(expectedResult, actualResult, new CartoonComparer());

            Assert.NotNull(actualResult);
            Assert.True(actualResult.Count <= paginationContext.PageSize);

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
        public async Task Find_NoData_ReturnDefault()
        {
            var mockContext = new Mock<DbContext>();
            var mockSet = GetQueryableMockDbSet<Cartoon>(Enumerable.Empty<Cartoon>());
            var repository = new Repository<Cartoon>(mockContext.Object, mockSet.Object);

            var actualResult = await repository.Find();

            Assert.Null(actualResult);
        }

        [Fact]
        public async Task Find_FakeData_ReturnIt()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);

            var actualResult = await repository.Find();

            var expectedResult = context.Cartoons.First();

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult, new CartoonComparer());

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Find_FakeDataWithSpecs_ReturnFiltrated(short id)
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);

            var actualResult = await repository.Find(idSpec);

            var expectedResult = await context.Cartoons.AsQueryable()
                                                       .FirstAsync(cartoon => cartoon.Id == id);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult, new CartoonComparer());
        }

        [Fact]
        public async Task Find_FakeDataWithInvalidSpecs_ReturnEmpty()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(short.MaxValue);

            var actualResult = await repository.Find(idSpec);

            Assert.Null(actualResult);
        }

        [Fact]
        public async Task Find_FakeDataWithIncluders_ReturnExtended()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);

            var actualResult = await repository.Find(includeProperties: cartoon => cartoon.Episodes);

            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.Episodes);
            Assert.NotEmpty(actualResult.Episodes);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Find_FakeDataWithSpecsAndIncluders_ReturnIt(short id)
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var idSpec = new CartoonIdEqualitySpecification(id);

            var actualResult = await repository.Find(
                condition: idSpec,
                includeProperties: cartoon => cartoon.Episodes
            );

            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.Episodes);
            Assert.NotEmpty(actualResult.Episodes);
        }

        /******************
        * `Create` testing
        */
        [Fact]
        public async Task Create_NewData_ReturnWithNoExcept()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            short newItemId = 5;

            var actualTaskResult = repository.Create(new Cartoon() { Id = newItemId, Name = It.IsAny<string>() });

            Assert.True(actualTaskResult.IsCompletedSuccessfully);

            var newItem = await context.Cartoons.AsQueryable().FirstAsync(cartoon => cartoon.Id == newItemId);

            Assert.NotNull(newItem);
        }

        [Fact]
        public async Task Create_ExistingData_ReturnWithException()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var existingItem = context.Cartoons.First();

            Func<Task> act = async () => await repository.Create(new Cartoon() { Id = existingItem.Id, Name = existingItem.Name });
            var exception = await Assert.ThrowsAnyAsync<Exception>(act);

            Assert.NotNull(exception);
        }

        /******************
        * `Update` testing
        */
        [Fact]
        public async Task Update_ExistingData_ReturnWithNoExcept()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var existingItem = context.Cartoons.First();
            var newNameData = "Tribute in 2024...";

            existingItem.Name = newNameData;

            var actualTaskResult = repository.Update(existingItem);

            Assert.True(actualTaskResult.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task Update_UnknownData_ReturnWithException()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var unknownItem = new Cartoon() { Id = 3, Name = It.IsAny<string>() };

            Func<Task> act = async () => await repository.Update(unknownItem);
            var exception = await Assert.ThrowsAnyAsync<Exception>(act);

            Assert.NotNull(exception);
        }

        /******************
        * `Delete` testing
        */
        [Fact]
        public async Task Delete_ExistingData_ReturnWithNoExcept()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var existingItem = context.Cartoons.First();

            var actualTaskResult = repository.Remove(existingItem);

            Assert.True(actualTaskResult.IsCompletedSuccessfully);
            Assert.DoesNotContain(existingItem, context.Cartoons);
        }

        [Fact]
        public async Task Delete_UnknownData_ReturnWithException()
        {
            using var context = await GetDefaultCartoonContext();
            var repository = new Repository<Cartoon>(context, context.Cartoons);
            var unknownItem = new Cartoon() { Id = 3, Name = It.IsAny<string>() };

            Func<Task> act = async () => await repository.Remove(unknownItem);
            var exception = await Assert.ThrowsAnyAsync<Exception>(act);

            Assert.NotNull(exception);
        }
    }
}

#pragma warning restore xUnit1013