namespace CCS.Calco.Test;

public class EntityFilterTest : CalcoFilter<EntityTest>
{
    public string Name { get; set; }

    public EntityFilterTest()
    {
        MapOrder<EntityFilterTest>(filter => filter.Name, entity => entity.Name);
    }
}