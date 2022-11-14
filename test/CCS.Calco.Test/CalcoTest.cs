namespace CCS.Calco.Test;

using CCS.Calco;

public class CalcoTest
{
    private EntityFilterTest filter;

    [SetUp]
    public void Setup()
    {
        filter = new EntityFilterTest() {
            PageOrder="Name",
        };
    }

    [Test]
    public void Test1()
    {
        IEnumerable<EntityTest> entities = new List<EntityTest>() {
            new EntityTest(){ Id = Guid.NewGuid(), Name = "Stefano", Age = 28 },
            new EntityTest(){ Id = Guid.NewGuid(), Name = "Lissette", Age = 25 },
            new EntityTest(){ Id = Guid.NewGuid(), Name = "Miriam", Age = 26 },
        };

        IQueryable<EntityTest> entitiesQuery = filter.CreateQuery(entities);
        IEnumerable<EntityTest> entitiesOrdered = entitiesQuery.ToArray();
        Assert.Pass();
    }
}