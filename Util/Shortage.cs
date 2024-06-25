namespace ShortageRequests.Util
{
    public enum Category
    {
        Electronics,
        Food,
        Other
    }

    public enum Room
    {
        MeetingRoom,
        Kitchen,
        Bathroom
    }

    public class Shortage
    {
        public string Title { get; }
        public string Name { get; set; }
        public Room Room { get; }
        public Category Category { get; }
        public int Priority { get; }
        public DateTime Date { get; set; }


        public override string ToString()
        {
            return $"{Title, -15} | {Name, -15} | {Room, -11} | {Category, -12} | {Date.ToString("yyyy-MM-dd"), -10} | {Priority, -3}";
        }

        public Shortage(string title, string name, Room room, Category category, int priority)
        {
            Title = title;
            Name = name;
            Room = room;
            Category = category;
            Date = DateTime.Now.Date;
            Priority = priority;
        }
    }
}