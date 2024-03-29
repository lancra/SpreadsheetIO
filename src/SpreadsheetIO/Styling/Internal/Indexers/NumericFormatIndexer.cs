using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers;

internal class NumericFormatIndexer : INumericFormatIndexer
{
    private const uint InitialIndex = 164U;

    private readonly BuiltInNumericFormats _builtInResources;
    private readonly ConstantNumericFormats _constantResources;
    private readonly IDictionary<NumericFormat, uint> _resourceIndexer = new Dictionary<NumericFormat, uint>();
    private uint _indexProvider;

    public NumericFormatIndexer(BuiltInNumericFormats builtInResources, ConstantNumericFormats constantResources)
    {
        _builtInResources = builtInResources;
        _constantResources = constantResources;
        _indexProvider = InitialIndex;
    }

    public IReadOnlyCollection<NumericFormat> Resources
        => (IReadOnlyCollection<NumericFormat>)_resourceIndexer.Keys;

    public uint NonBuiltInCount { get; private set; }

    public uint this[NumericFormat resource]
    {
        get
        {
            if (!_resourceIndexer.TryGetValue(resource, out var index))
            {
                throw new KeyNotFoundException(Messages.UnindexedResource);
            }

            return index;
        }
    }

    public uint Add(NumericFormat resource)
    {
        if (_resourceIndexer.TryGetValue(resource, out var index))
        {
            return index;
        }

        var isBuiltIn = _builtInResources.TryGetValue(resource.Code, out var builtInId);
        if (isBuiltIn)
        {
            _resourceIndexer.Add(resource, builtInId);
            return builtInId;
        }

        var isConstant = _constantResources.TryGetValue(resource.Code, out var constantId);
        if (isConstant)
        {
            _resourceIndexer.Add(resource, constantId);
            NonBuiltInCount++;
            return constantId;
        }

        index = _indexProvider;
        _resourceIndexer.Add(resource, index);
        _indexProvider++;
        NonBuiltInCount++;

        return index;
    }

    public void Clear()
    {
        _resourceIndexer.Clear();
        _indexProvider = InitialIndex;
        NonBuiltInCount = 0U;
    }
}
