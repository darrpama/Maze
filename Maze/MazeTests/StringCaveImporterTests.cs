using CaveModel;
using CaveModel.Exceptions;

namespace MazeTests;

public class StringCaveImporterTests
{
    [Fact]
    public void Given_Import_When_CorrectString_Then_ReturnsCorrectCaveCells()
    {
        const string importString = """
        3 5
        1 1 0 0 0
        0 1 0 1 0
        1 1 0 0 0
        """;
        var importer = new StringCaveImporter(importString);
        var cellsResult = importer.Import();
        var expected = new CaveCell[,]
        {
            { new(true), new(true), new(false), new(false), new(false) },
            { new(false), new(true), new(false), new(true), new(false) },
            { new(true), new(true), new(false), new(false), new(false) },
        };
        Assert.Equal(cellsResult, expected);
    }

    [Fact]
    public void Given_Import_When_StringWithEmptyLines_Then_ReturnsCorrectCaveCells()
    {
        const string importString = """

        3 5

        1 1 0 0 0
        0 1 0 1 0

        1 1 0 0 0
        """;
        var importer = new StringCaveImporter(importString);
        var cellsResult = importer.Import();
        var expected = new CaveCell[,]
        {
            { new(true), new(true), new(false), new(false), new(false) },
            { new(false), new(true), new(false), new(true), new(false) },
            { new(true), new(true), new(false), new(false), new(false) },
        };
        Assert.Equal(cellsResult, expected);
    }

    [Fact]
    public void Given_Import_When_StringWithIncorrectSize_Then_ThrowExceptionImportError()
    {
        const string importString = """
        51 51
        1 1 0 0 0
        0 1 0 1 0
        1 1 0 0 0
        """;
        var importer = new StringCaveImporter(importString);
        Assert.Throws<ImportCaveError>(() => { importer.Import(); });
    }
    
    [Fact]
    public void Given_Import_When_StringWithHighSizeCount_Then_ThrowExceptionImportError()
    {
        const string importString = """
        3 5 6 
        1 1 0 0 0
        0 1 0 1 0
        1 1 0 0 0
        """;
        var importer = new StringCaveImporter(importString);
        Assert.Throws<ImportCaveError>(() => { importer.Import(); });
    }
    
    [Fact]
    public void Given_Import_When_StringWithZeroSize_Then_ThrowExceptionImportError()
    {
        const string importString = """
        0 0 
        1 1 0 0 0
        0 1 0 1 0
        1 1 0 0 0
        """;
        var importer = new StringCaveImporter(importString);
        Assert.Throws<ImportCaveError>(() => { importer.Import(); });
    }
    
    [Fact]
    public void Given_Import_When_StringWithIncorrectColsCount_Then_ThrowExceptionImportError()
    {
        const string importString = """
        3 5
        1 1 0 0 0 1
        0 1 0 1 0 1
        1 1 0 0 0 1
        """;
        var importer = new StringCaveImporter(importString);
        Assert.Throws<ImportCaveError>(() => { importer.Import(); });
    }
    
    [Fact]
    public void Given_Import_When_StringWithIncorrectNumber_Then_ThrowExceptionImportError()
    {
        const string importString = """
        3 5
        5 1 0 0 0
        0 1 0 1 0
        1 1 0 0 0
        """;
        var importer = new StringCaveImporter(importString);
        Assert.Throws<ImportCaveError>(() => { importer.Import(); });
    }
}