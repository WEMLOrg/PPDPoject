using TimeTableApp.Repository;
using TimeTableApp.Models;

namespace TimeTableApp
{
    class Service
    {
        public int startTime { get; set; }
        public int endTime { get; set; }
        public GroupsRepository groupsRepository { get; set; }
        public TeachersRepository teachersRepository { get; set; }
        public SubjectsRepository subjectsRepository { get; set; }
        public RoomsRepository roomsRepository { get; set; }
        public TimeTableRepo timeTableRepo { get; set; }
        public List<TimetableEntry.Days> days { get; set; }
        public List<TimetableEntry> currentSolution;


        public Service(GroupsRepository groupsRepository, TeachersRepository teacherRepo, SubjectsRepository subjectRepo, RoomsRepository roomRepo,  TimeTableRepo orarRepo, int startTime, int endTime)
        {
            groupsRepository = groupsRepository;
            teachersRepository = teacherRepo;
            subjectsRepository = subjectRepo;
            roomsRepository = roomRepo;
            timeTableRepo = orarRepo;
            this.groupsRepository = groupsRepository;
            this.startTime = startTime;
            this.endTime = endTime;
            days = new List<TimetableEntry.Days>();
            days.Add(TimetableEntry.Days.Monday);
            days.Add(TimetableEntry.Days.Tuesday);
            days.Add(TimetableEntry.Days.Wednesday);
            days.Add(TimetableEntry.Days.Thursday);
            days.Add(TimetableEntry.Days.Friday);
            currentSolution = new List<TimetableEntry>();
            Console.WriteLine("Loaded groups: " + groupsRepository.GetGroups().Count);
            Console.WriteLine("Loaded subjects: " + subjectsRepository.GetSubjects().Count);
            Console.WriteLine("Loaded teachers: " + teachersRepository.GetTeachers().Count);

        }
        public void BackTracking ()
        {
            Console.WriteLine("Back tracking");
            Console.WriteLine(currentSolution.Count);
            if (currentSolution.Count > 0 && isCompleteSolution(currentSolution))
            {
                foreach (var entry in currentSolution)
                {
                    timeTableRepo.AddEntry(entry);
                }
                return;
            }
            Console.WriteLine("Not complete solution");

            foreach (Subject subject in subjectsRepository.GetSubjects())
            {
                foreach (Room room in roomsRepository.GetRooms())
                {
                    foreach (Teacher teacher in teachersRepository.GetTeachers())
                    {
                        foreach (Group group in groupsRepository.GetGroups())
                        {
                            foreach (TimetableEntry.Days day in days)
                            {
                                for (int hour = startTime; hour <= endTime; hour++)
                                {   
                                    Console.WriteLine("in back, before is valid");
                                    if (isValid(group, room, teacher, subject, hour, day))
                                    {
                                        TimetableEntry validEntry =
                                            new TimetableEntry(teacher, subject, group, room, day, hour);
                                        Console.WriteLine($"Adding entry: {group._id} - {subject.name} at {hour} on {day}");

                                        currentSolution.Add(validEntry);
                                        
                                        BackTracking();
                                        currentSolution.RemoveAt(currentSolution.Count - 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public bool doesGroupFitInRoom(Room r, Group g)
        {
            if (r.capacity >= g.nrOfKids)
                return true;
            return false;
        }
        public bool isRoomAvailable(Room r, TimetableEntry.Days day, int hour)
        {
            foreach(TimetableEntry te in currentSolution)
            {
                if (te.day == day) 
                    if (te.hour == hour)
                        if (te.room == r)
                            return false;
            }
            return true;
        }
        public bool isSolution(TimetableEntry t) 
        {
            foreach (TimetableEntry te in currentSolution)
            {
                if (te == t)
                    return false;
                if (te.hour == t.hour && te.day == t.day)
                {
                    if (te.teacher == t.teacher || te.group == t.group || te.room == t.room)
                        return false;
                }
            }

            return true;
        }

        public bool doesGroupHaveAlreadyEnoughHoursForSubject(Subject s, Group g, Teacher t)
        {
            if (g.doesGroupHaveSubject(s._id, t._id))
            {
                int necessaryHours = g.necessarySubjects.GetValueOrDefault(new KeyValuePair<Guid, Guid>(s._id, t._id));
                foreach (TimetableEntry te in currentSolution)
                {
                    if (necessaryHours == 0)
                        return true;
                    if (te.subject == s && te.group == g)
                        necessaryHours--;
                }

                if (necessaryHours == 0)
                    return true;
            }

            return false;
        }
        public bool isSpecificRoomNeeded(Subject s)
        {
            if (s.specificRoom)
                return true;
            return false;
        }
        public bool isValid(Group g, Room r, Teacher t, Subject s, int h, TimetableEntry.Days d) 
        {
            Console.WriteLine($"Checking validity for Group: {g._id}, Subject: {s.name}, Teacher: {t.teacherName}, Room: {r._id}, Hour: {h}, Day: {d}");
            if (doesGroupFitInRoom(r, g) && g.doesGroupHaveSubject(s._id, t._id) &&
                t.DoesTeacherTeachSubject(s._id) && isRoomAvailable(r, d, h) &&
                !doesGroupHaveAlreadyEnoughHoursForSubject(s, g, t))
                if (isSpecificRoomNeeded(s))
                    if (r._id != s.roomId)
                    {
                        return false;
                    }
                    else
                        return true;

            return false;

        }
        
        private bool isCompleteSolution(List<TimetableEntry> currentSolution)
        {
            foreach (Group group in groupsRepository.GetGroups())
            {
                foreach (Subject subject in subjectsRepository.GetSubjects())
                {
                    var assignedTeachers = currentSolution
                        .Where(entry => entry.group == group && entry.subject == subject)
                        .Select(entry => entry.teacher)
                        .Distinct();

                    foreach (Teacher teacher in assignedTeachers)
                    {
                        int requiredHours = group.GetRequiredHoursForSubject(subject._id, teacher._id);

                        int scheduledHours = currentSolution
                            .Count(entry => entry.group == group && entry.subject == subject && entry.teacher == teacher);

                        if (scheduledHours < requiredHours)
                        {
                            Console.WriteLine("not complete sol " + scheduledHours + ' '+ requiredHours);
                            return false; 
                        }
                    }
                }
            }
            return true; 
        }

    }
}