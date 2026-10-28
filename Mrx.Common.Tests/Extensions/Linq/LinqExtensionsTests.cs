using Mrx.Common.Extensions;
using System.Data;
using System.Collections;
using System.ComponentModel;

namespace Mrx.Common.Tests;

public class LinqExtensionsTests
{
    [Fact]
    public void ToDataTable_MapsScalarValuesAndCustomColumnName()
    {
        var table = new[] { 3, 5 }.ToDataTable("Number");
        Assert.Equal(typeof(int), table.Columns[0].DataType);
        Assert.Equal("Number", table.Columns[0].ColumnName);
        Assert.Equal(2, table.Rows.Count);
        Assert.Equal(5, table.Rows[1][0]);
        var enumTable = new[] { RowState.Ready }.ToDataTable();
        Assert.Equal(typeof(int), enumTable.Columns[0].DataType);
        Assert.Equal((int)RowState.Ready, enumTable.Rows[0][0]);
        Assert.Empty(Array.Empty<string>().ToDataTable().Rows.Cast<DataRow>());
    }

    [Fact]
    public void ToDataTable_MapsBrowsablePropertiesEnumsAndNulls()
    {
        var table = new[] { new TableRow { Name = "first", State = RowState.Ready }, new TableRow { Name = null, State = null } }.ToDataTable();
        Assert.Equal(typeof(string), table.Columns[nameof(TableRow.Name)]!.DataType);
        Assert.Equal(typeof(int), table.Columns[nameof(TableRow.State)]!.DataType);
        Assert.Equal("first", table.Rows[0][nameof(TableRow.Name)]);
        Assert.Equal((int)RowState.Ready, table.Rows[0][nameof(TableRow.State)]);
        Assert.Equal(DBNull.Value, table.Rows[1][nameof(TableRow.Name)]);
        Assert.Equal(DBNull.Value, table.Rows[1][nameof(TableRow.State)]);
        Assert.False(table.Columns.Contains(nameof(TableRow.Hidden)));
        Assert.Equal(DBNull.Value, table.Rows[1][nameof(TableRow.OptionalCount)]);
    }

    [Fact]
    public void ToDataTable_RejectsNullSource()
        => Assert.Throws<ArgumentNullException>(() => LinqExtensions.ToDataTable<int>(null!));

    [Fact]
    public void PropertyOrdering_SupportsAscendingDescendingAndThenBy()
    {
        var rows = new[]
        {
            new SortRow("b", 1), new SortRow("a", 2), new SortRow("a", 1)
        }.AsQueryable();

        Assert.Equal(new[] { 2, 1, 0 }, rows.OrderByProperty(nameof(SortRow.Name)).ThenByProperty(nameof(SortRow.Rank)).Select(x => Array.IndexOf(rows.ToArray(), x)));
        Assert.Equal(new[] { "b", "a", "a" }, rows.OrderByProperty(nameof(SortRow.Name), true).Select(x => x.Name));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Missing")]
    public void PropertyOrdering_RejectsInvalidProperty(string propertyName)
        => Assert.Throws<ArgumentException>(() => new[] { new SortRow("a", 1) }.AsQueryable().OrderByProperty(propertyName));

    [Fact]
    public void PropertyOrdering_RejectsIndexerAndNullSource()
    {
        Assert.Throws<ArgumentException>(() => new[] { new IndexedRow() }.AsQueryable().OrderByProperty("Item"));
        Assert.Throws<ArgumentNullException>(() => LinqExtensions.OrderByProperty<SortRow>(null!, nameof(SortRow.Name)));
        Assert.Throws<ArgumentNullException>(() => LinqExtensions.ThenByProperty<SortRow>(null!, nameof(SortRow.Name)));
    }

    [Fact]
    public void Paging_PageZeroReturnsAll_AndPagedOverloadReportsCounts()
    {
        var source = Enumerable.Range(1, 5).AsQueryable();
        Assert.Equal(5, source.Paging(0, 2).Count());
        var page = source.Paging(2, 2, out var pageCount, out var totalCount);
        Assert.Equal(new[] { 3, 4 }, page);
        Assert.Equal(3, pageCount);
        Assert.Equal(5, totalCount);
    }

    [Fact]
    public void Paging_ReturnsEmptyForPageBeyondEnd_AndReportsEmptySource()
    {
        var source = new[] { 1, 2, 3 }.AsQueryable();
        Assert.Empty(source.Paging(4, 2));

        var emptyPage = Array.Empty<int>().AsQueryable().Paging(1, 10, out var pageCount, out var totalCount);
        Assert.Empty(emptyPage);
        Assert.Equal(0, pageCount);
        Assert.Equal(0, totalCount);
    }

    [Theory]
    [InlineData(-1, 2)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public void Paging_RejectsInvalidPageOrPageSize(int pageNumber, int rowsCount)
        => Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1, 2 }.AsQueryable().Paging(pageNumber, rowsCount));

    [Fact]
    public void Paging_RejectsNullSource()
    {
        Assert.Throws<ArgumentNullException>(() => LinqExtensions.Paging<int>(null!, 1, 10));
        Assert.Throws<ArgumentNullException>(() => LinqExtensions.Paging<int>(null!, 1, 10, out _, out _));
    }

    [Fact]
    public void SequenceHelpers_HandleEmptyAndNonEmptyCollections()
    {
        IEnumerable<int> missing = null!;
        Assert.True(missing.IsNullOrEmpty());
        Assert.False(missing.IsNotNullOrEmpty());
        Assert.True(Array.Empty<int>().IsNullOrEmpty());
        Assert.True(new[] { 1 }.IsNotNullOrEmpty());
        Assert.True(LinqExtensions.IsNullOrEmpty((IEnumerable)null!));
        Assert.False(LinqExtensions.IsNullOrEmpty((IEnumerable)new[] { 1 }));
        Assert.True(LinqExtensions.IsNotNullOrEmpty((IEnumerable)new[] { 1 }));
        Assert.False(LinqExtensions.IsNotNullOrEmpty((IEnumerable)null!));
        Assert.Equal(3, new[] { 1, 2, 3 }.GetNext(2));
        Assert.Equal(3, new[] { 1, 2, 3 }.GetNext(value => value == 2));
        Assert.Equal(1, new[] { 1, 2, 3 }.GetPrevious(2));
        Assert.Equal(1, new[] { 1, 2, 3 }.GetPrevious(value => value == 2));
        Assert.Equal(default, new[] { 1, 2 }.GetNext(2));
        Assert.Equal(default, new[] { 1, 2 }.GetNext(value => value == 99));
        Assert.Equal(default, new[] { 1, 2 }.GetPrevious(value => value == 1));
        Assert.Equal(default, new[] { 1, 2 }.GetPrevious(1));
        Assert.Equal(default, new[] { 1, 2 }.GetNext(_ => throw new InvalidOperationException()));
        Assert.Equal(default, new[] { 1, 2 }.GetPrevious(_ => throw new InvalidOperationException()));
        Assert.False(((IEnumerable)Array.Empty<int>()).IsNotNullOrEmpty());
        Assert.True(((IEnumerable)new[] { 1 }).Any());
        Assert.False(((IEnumerable)Array.Empty<int>()).Any());
        Assert.Equal(new[] { 2, 4, 6 }, new[] { 1, 2, 3 }.ForEach((value, _) => value * 2));

        var list = new List<int> { 1, 2, 3, 4 };
        list.RemoveRange(value => value % 2 == 0);
        Assert.Equal(new[] { 1, 3 }, list);
        list.RemoveRange(new List<int> { 1 });
        Assert.Equal(new[] { 3 }, list);
    }

    [Fact]
    public void GetNextAndPrevious_HandleEmptyMissingAndBoundaryValues()
    {
        var empty = Array.Empty<int>();
        Assert.Equal(default, empty.GetNext(1));
        Assert.Equal(default, empty.GetPrevious(1));

        var values = new[] { 0, 2, 4 };
        Assert.Equal(default, values.GetPrevious(0));
        Assert.Equal(default, values.GetNext(4));
        Assert.Equal(default, values.GetNext(value => value == 99));
        Assert.Equal(default, values.GetPrevious(value => value == 99));

        string?[] nullableValues = [null, "middle", "last"];
        Assert.Equal("middle", nullableValues.GetNext((string?)null));
        Assert.Null(nullableValues.GetPrevious((string?)null));
    }

    [Fact]
    public async Task WhenAllAsync_AwaitsAllTasks()
        => Assert.Equal(new[] { 1, 2 }, await new[] { Task.FromResult(1), Task.FromResult(2) }.WhenAllAsync());

    [Fact]
    public async Task WhenAllAsync_ReturnsEmptyForNoTasks()
        => Assert.Empty(await Array.Empty<Task<int>>().WhenAllAsync());

    [Fact]
    public async Task WhenAllAsync_PropagatesTaskFailure()
    {
        var tasks = new[]
        {
            Task.FromResult(1),
            Task.FromException<int>(new InvalidOperationException("task failed"))
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await tasks.WhenAllAsync());
        Assert.Equal("task failed", exception.Message);
    }

    private enum RowState { Ready = 2 }
    private sealed record TableRow { public string? Name { get; init; } public RowState? State { get; init; } public int? OptionalCount { get; init; } [Browsable(false)] public string? Hidden { get; init; } }
    private sealed record SortRow(string Name, int Rank);
    private sealed class IndexedRow { public int this[int index] => index; }
}

