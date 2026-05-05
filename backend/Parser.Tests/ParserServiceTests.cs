using Microsoft.Extensions.Options;
using Parser.API.Models;
using Parser.API.Services;

namespace Parser.Tests;

public class ParserServiceTests
{
    private readonly XmlParserService _service;

    public ParserServiceTests()
    {
        var settings = Options.Create(new TaxSettings
        {
            Rate = 0.10m
        });

        _service = new XmlParserService(settings);
    }

    [Fact]
    public void Parse_ValidInput_ShouldReturnParsedResponse()
    {
        var input = """
                    Hi Patricia,
                    <expense>
                        <cost_centre>DEV632</cost_centre>
                        <total>35,000</total>
                        <payment_method>personal card</payment_method>
                    </expense>
                    """;

        var result = _service.Parse(input);

        // Assert
        Assert.Equal("DEV632", result.CostCentre);
        Assert.Equal(35000m, result.Total);
        Assert.Equal("personal card", result.PaymentMethod);
        Assert.Equal(3181.82m, result.SalesTax);
        Assert.Equal(31818.18m, result.TotalExcludingTax);
    }

    [Fact]
    public void Parse_MissingCostCentre_ShouldDefaultToUnknown()
    {
        var input = """
                    <expense>
                        <total>1000</total>
                    </expense>
                    """;

        var result = _service.Parse(input);

        // Assert
        Assert.Equal("UNKNOWN", result.CostCentre);
        Assert.Equal(1000m, result.Total);
    }

    [Fact]
    public void Parse_MissingTotal_ShouldThrowException()
    {
        var input = """
                    <expense>
                        <cost_centre>DEV632</cost_centre>
                    </expense>
                    """;

        var ex = Assert.Throws<Exception>(() => _service.Parse(input));

        //Assert
        Assert.Equal("Missing <total> tag.", ex.Message);
    }

    [Fact]
    public void Parse_InvalidTotal_ShouldThrowException()
    {
        var input = """
                    <expense>
                        <cost_centre>DEV632</cost_centre>
                        <total>abc</total>
                    </expense>
                    """;

        var ex = Assert.Throws<Exception>(() => _service.Parse(input));

        //Assert
        Assert.Equal("Invalid <total> value.", ex.Message);
    }

    [Fact]
    public void Parse_MissingClosingTag_ShouldThrowException()
    {
        var input = """
                    <expense>
                        <cost_centre>DEV632</cost_centre>
                        <total>1000</total>
                    """;

        var ex = Assert.Throws<Exception>(() => _service.Parse(input));

        //Assert
        Assert.Equal("Missing closing tag for <expense>", ex.Message);
    }

    [Fact]
    public void Parse_WrongClosingTagOrder_ShouldThrowException()
    {
        var input = """
                    <expense>
                        <total>1000</expense>
                    </total>
                    """;

        var ex = Assert.Throws<Exception>(() => _service.Parse(input));

        //Assert
        Assert.Contains("Expected", ex.Message);
    }

    [Fact]
    public void Parse_ShouldExtractVendorDescriptionAndDate()
    {
        var input = """
                    Hi Maria,
                    Please create a reservation for 10 at the
                    <vendor>Seaside Steakhouse</vendor>
                    for our
                    <description>development team celebration</description>
                    on <date>27 April 2022</date>
                    <total>5000</total>
                    """;

        var result = _service.Parse(input);

        // Assert
        Assert.Equal("Seaside Steakhouse", result.Vendor);
        Assert.Equal("development team celebration", result.Description);
        Assert.Equal("27 April 2022", result.Date);
    }

    [Fact]
    public void Parse_EmptyInput_ShouldThrowException()
    {
        var input = "";

        var ex = Assert.Throws<Exception>(() => _service.Parse(input));

        //Assert
        Assert.Equal("Input text is required.", ex.Message);
    }

    [Fact]
    public void Parse_ShouldUseConfiguredTaxRate()
    {
        var customService = new XmlParserService(
            Options.Create(new TaxSettings
            {
                Rate = 0.20m
            }));

        var input = """
                    <total>120</total>
                    """;

        var result = customService.Parse(input);

        // Assert
        Assert.Equal(20m, result.SalesTax);
        Assert.Equal(100m, result.TotalExcludingTax);
    }
}
