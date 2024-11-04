namespace TimeTableApp.Models
{
    public class Subject
    {
        public Guid _id { get; set; }
        private int nrCopii { get; set; }
        private string name { get; set; }
        private bool specificRoom {  get; set; }
        private Guid? roomId {  get; set; }

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