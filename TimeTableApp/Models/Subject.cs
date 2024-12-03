namespace TimeTableApp.Models
{
    public class Subject
    {
        public Guid _id { get; set; }
        public string name { get; set; }
        public bool specificRoom {  get; set; }
        public Guid? roomId {  get; set; }

        public Subject(Guid id, string name, Guid roomId)
        {
            this._id = id;
            this.name = name;   
            this.specificRoom = true;
            this.roomId = roomId;
        }
        public Subject(Guid id, string name)
        {
            _id = id;
            this.name = name;
            this.specificRoom = false;
            this.roomId = null;
        }

    }
}