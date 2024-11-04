namespace TimeTableApp.Models
{
    public class Subject
    {
        public Guid _id { get; set; }
        public int nrCopii { get; set; }
        public string name { get; set; }
        public bool specificRoom {  get; set; }
        public Guid? roomId {  get; set; }

        public Subject(int nr, string name, Guid roomId)
        {
            _id = Guid.NewGuid();
            this.nrCopii = nr;
            this.name = name;   
            this.specificRoom = true;
            this.roomId = roomId;
        }
        public Subject(int nr, string name)
        {
            _id = Guid.NewGuid();
            this.nrCopii = nr;
            this.name = name;
            this.specificRoom = false;
            this.roomId = null;
        }

    }
}