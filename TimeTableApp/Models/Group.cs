namespace TimeTableApp
{
    public class Group
    {
        public Guid _id { get; set; }
        public int nrOfKids { get; set; }
        public Dictionary<KeyValuePair<Guid, Guid>, int> necessarySubjects { get; set; }
                                    //materieId, profId

        public Group(Guid id, int nr, Dictionary<KeyValuePair<Guid, Guid>, int> necessarySubj)
        {
            _id = id;
            nrOfKids = nr;
            this.necessarySubjects = necessarySubj;
        }

        public bool doesGroupHaveSubject(Guid subject, Guid teacher)
        {
            return necessarySubjects.ContainsKey(new KeyValuePair<Guid, Guid>(subject, teacher));
        }

        public bool doesGroupHaveSubject(Guid subject)
        {
            return necessarySubjects.Keys.Any(key => key.Key == subject);
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
        
        public int GetRequiredHoursForSubject(Guid subjectId, Guid teacherId)
        {
            var key = new KeyValuePair<Guid, Guid>(subjectId, teacherId);

            if (necessarySubjects.TryGetValue(key, out int requiredHours))
            {
                return requiredHours;
            }
    
            return 0;
        }

    }
}