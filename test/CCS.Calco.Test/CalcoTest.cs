namespace CCS.Calco.Test;

public class CalcoTest
{
    private EntityFilterTest filter;
    private IEnumerable<EntityTest> entities;

    [SetUp]
    public void Setup()
    {
        entities = new List<EntityTest>() {
            new EntityTest(){ Id = Guid.NewGuid(), Name = "Stefano", Age = 28 },
            new EntityTest(){ Id = Guid.NewGuid(), Name = "Lissette", Age = 25 },
            new EntityTest(){ Id = Guid.NewGuid(), Name = "Miriam", Age = 26 },
        };
    }

    [Test]
    public void TestAscending()
    {
        filter = new EntityFilterTest() {
            PageOrder="Name",
        };

        IQueryable<EntityTest> entitiesQuery = filter.CreateQuery(entities);
        IEnumerable<EntityTest> entitiesOrdered = entitiesQuery.ToArray();
        Assert.AreEqual("Lissette", entitiesOrdered.First().Name);
    }

    [Test]
    public void TestDescending()
    {
        filter = new EntityFilterTest() {
            PageOrder="Name:des",
        };

        IQueryable<EntityTest> entitiesQuery = filter.CreateQuery(entities);
        IEnumerable<EntityTest> entitiesOrdered = entitiesQuery.ToArray();
        Assert.AreEqual("Stefano", entitiesOrdered.First().Name);
    }
}