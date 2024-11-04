namespace TimeTableApp
{
    class Room 
    {
        private Guid _id { get; set; }
        private int capacity { get; set; }
        Room(int capacity)
        {
            this.capacity = capacity;
        }
    } }