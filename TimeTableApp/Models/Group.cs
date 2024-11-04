namespace TimeTableApp
{
    class Group
    {
        public Guid _id { get; set; }
        public int nrOfKids { get; set; }
        public Dictionary<Guid, int> necessarySubjects { get; set; }

        Group(int nr, Dictionary<Guid, int> necessarySubjects)
        {
            _id = Guid.NewGuid();
            nrOfKids = nr;
            necessarySubjects = necessarySubjects;
        }
    }
}