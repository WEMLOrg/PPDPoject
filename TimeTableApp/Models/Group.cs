namespace TimeTableApp
{
    public class Group
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

        public bool doesGroupHaveSubject(Guid subject)
        {
            return necessarySubjects.ContainsKey(subject);
        }

        public static bool operator ==(Group t1, Group t2)
        {
            if (ReferenceEquals(t1, t2)) return true;
            if (t1 is null || t2 is null) return false;
            return t1._id == t2._id;
        }
        public static bool operator !=(Group t1, Group t2)
        {
            return !(t1 == t2);
        }
    }
}