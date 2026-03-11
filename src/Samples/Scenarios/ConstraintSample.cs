using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public abstract class ConstraintSample<TStruct, TClass>
    where TStruct : struct
    where TClass : DataSample, new()
{
    public TStruct StructValue { get; set; }

    public TClass? ClassValue { get; set; }

    public override string ToString()
    {
        return TypeHelper.ExpandedName(GetType());
    }
}
