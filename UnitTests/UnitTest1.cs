using ShortageRequests.Util;

public class OperationsTests
{
    private List<Shortage> GetTestShortages()
    {
        return new List<Shortage>
        {
            new Shortage("Laptop", "Rokas", Room.MeetingRoom, Category.Electronics, 10),
            new Shortage("Toilet Paper", "Jonas", Room.Bathroom, Category.Other, 10),
            new Shortage("Laptop Charger", "Mantas", Room.MeetingRoom, Category.Electronics, 3),
            new Shortage("Milk", "Alisa", Room.Kitchen, Category.Food, 4),
            new Shortage("Projector", "Petras", Room.MeetingRoom, Category.Electronics, 5)
        };
    }

    [Fact]
    public void TestFilterByTitle()
    {
        var shortages = GetTestShortages();
        var input = "Laptop";
        using (var reader = new StringReader(input))
        {
            Console.SetIn(reader);
            var result = Operations.FilterByTitle(shortages);
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Contains(input, item.Title, StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public void TestFilterByCategory()
    {
        var shortages = GetTestShortages();
        var input = "1"; // Electronics
        using (var reader = new StringReader(input))
        {
            Console.SetIn(reader);
            var result = Operations.FilterByCategory(shortages);
            Assert.Equal(3, result.Count);
            Assert.All(result, item => Assert.Equal(Category.Electronics, item.Category));
        }
    }

    [Fact]
    public void TestFilterByRoom()
    {
        var shortages = GetTestShortages();
        var input = "2"; // Kitchen
        using (var reader = new StringReader(input))
        {
            Console.SetIn(reader);
            var result = Operations.FilterByRoom(shortages);
            Assert.Single(result);
            Assert.All(result, item => Assert.Equal(Room.Kitchen, item.Room));
        }
    }

    [Fact]
    public void TestFilterByCreatedOn()
    {
        var shortages = GetTestShortages();
        shortages[0].Date = new DateTime(2024, 01, 01);
        shortages[1].Date = new DateTime(2024, 02, 01);
        shortages[2].Date = new DateTime(2024, 03, 01);
        shortages[3].Date = new DateTime(2024, 04, 01);
        shortages[4].Date = new DateTime(2024, 05, 01);

        var input = "2024-03-01\n2024-05-01";
        using (var reader = new StringReader(input))
        {
            Console.SetIn(reader);
            var result = Operations.FilterByCreatedOn(shortages);
            Assert.Equal(3, result.Count);
            Assert.All(result, item => Assert.InRange(item.Date, new DateTime(2024, 03, 01), new DateTime(2024, 05, 01)));
        }
    }
    
}
