using System.Linq.Expressions;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping.Builders;

internal class ResourceMapBuilder<TResource> : ResourceMapBuilder, IInternalResourceMapBuilder<TResource>
    where TResource : class
{
    private readonly IMapBuilderFactory _mapBuilderFactory;

    public ResourceMapBuilder(IMapBuilderFactory mapBuilderFactory, IResourceMapBuilderValidator validator)
        : base(typeof(TResource), validator)
    {
        _mapBuilderFactory = mapBuilderFactory;
    }

    public IResourceMapBuilder<TResource> ContinuesOnResourceReadingFailure()
    {
        AddOrUpdateRegistration(new ContinueOnResourceReadingFailureResourceMapOption());
        return this;
    }

    public IResourceMapBuilder<TResource> HasDefaultPropertyReadingResolutions(
        params ResourcePropertyDefaultReadingResolution[] resolutions)
    {
        Guard.Against.NullOrEmpty(resolutions, nameof(resolutions));
        Guard.Against.NullElements(resolutions, nameof(resolutions));

        AddOrUpdateRegistration(new DefaultPropertyReadingResolutionsResourceMapOptionRegistration(resolutions));
        return this;
    }

    public IResourceMapBuilder<TResource> HasHeaderRowNumber(uint number)
    {
        Guard.Against.Zero(number, nameof(number));

        AddOrUpdateRegistration(new HeaderRowNumberResourceMapOption(number));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> Property<TProperty>(Expression<Func<TResource, TProperty>> propertyExpression)
    {
        Guard.Against.Null(propertyExpression, nameof(propertyExpression));

        var propertyMapBuilder = _mapBuilderFactory.CreateForProperty(propertyExpression);
        AddProperty(propertyMapBuilder);

        return propertyMapBuilder;
    }

    public IResourceMapBuilder<TResource> UsesHeaderStyle(Style style, string name = "")
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(
            new IndexerKey(!string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString(), IndexerKeyKind.Custom),
            style));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesHeaderStyle(BuiltInExcelStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesHeaderStyle(BuiltInPackageStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesBodyStyle(Style style, string name = "")
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(
            new IndexerKey(!string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString(), IndexerKeyKind.Custom),
            style));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesBodyStyle(BuiltInExcelStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesBodyStyle(BuiltInPackageStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesDateKind(CellDateKind dateKind)
    {
        Guard.Against.Null(dateKind, nameof(dateKind));

        AddOrUpdateRegistration(new DateKindMapOption(dateKind));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesStringKind(CellStringKind stringKind)
    {
        Guard.Against.Null(stringKind, nameof(stringKind));

        AddOrUpdateRegistration(new StringKindMapOption(stringKind));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesExplicitConstructor(params string[] propertyNames)
    {
        Guard.Against.NullOrEmpty(propertyNames, nameof(propertyNames));
        Guard.Against.NullOrEmptyElements(propertyNames, nameof(propertyNames));

        AddOrUpdateRegistration(new ExplicitConstructorResourceMapOptionRegistration(propertyNames));
        return this;
    }

    public IResourceMapBuilder<TResource> UsesImplicitConstructor()
    {
        AddOrUpdateRegistration(new ImplicitConstructorResourceMapOptionRegistration());
        return this;
    }
}
