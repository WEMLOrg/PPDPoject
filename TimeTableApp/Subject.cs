namespace TimeTableApp
{
    class Subject
    {
        private Guid _id { get; set; }
        private int nrCopii { get; set; }
        private string name { get; set; }
        private bool specificRoom {  get; set; }
        private Guid roomId {  get; set; }

        Subjects(int nr, string name, true, Guid roomId)
        {
            _id = Guid.NewGuid();
            this.nrCopii = nr;
            this.name = name;   
            this.specificRoom = specificRoom;
            this.roomId = roomId;
        }
        Subjects(int nr, string name, false)
        {
            _id = Guid.NewGuid();
            this.nrCopii = nr;
            this.name = name;
            this.specificRoom = specificRoom;
        }

    }
}