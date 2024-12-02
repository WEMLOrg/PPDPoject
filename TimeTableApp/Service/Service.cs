using TimeTableApp.Repository;
using TimeTableApp.Models;

namespace TimeTableApp
{
    class Service
    {
        public int startTime { get; set; }
        public int endTime { get; set; }
        public TeachersRepository teachersRepository { get; set; }
        public SubjectsRepository subjectsRepository { get; set; }
        public RoomsRepository roomsRepository { get; set; }
        public GroupsRepository groupsRepository { get; set; }
        public TimeTableRepo timeTableRepo { get; set; }
        public List<TimetableEntry.Days> days { get; set; }
        public List<TimetableEntry> currentSolution;

        public Service(GroupsRepository groupsRepository, TeachersRepository teacherRepo, SubjectsRepository subjectRepo, RoomsRepository roomRepo,  TimeTableRepo orarRepo, int startTime, int endTime)
        {
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
        // public void BackTracking ()
        // {
        //     Console.WriteLine("Back tracking");
        //     Console.WriteLine(currentSolution.Count);
        //     if (isCompleteSolution(currentSolution))
        //     {
        //         foreach (var entry in currentSolution)
        //         {
        //             timeTableRepo.AddEntry(entry);
        //         }
        //         return;
        //     }
        //     Console.WriteLine("Not complete solution");
        //
        //     foreach (Subject subject in subjectsRepository.GetSubjects())
        //     {
        //         foreach (Room room in roomsRepository.GetRooms())
        //         {
        //             foreach (Teacher teacher in teachersRepository.GetTeachers())
        //             {
        //                 foreach (Group group in groupsRepository.GetGroups())
        //                 {
        //                     foreach (TimetableEntry.Days day in days)
        //                     {
        //                         for (int hour = startTime; hour <= endTime; hour++)
        //                         {   
        //                             Console.WriteLine("in back, before is valid");
        //                             if (isValid(group, room, teacher, subject, hour, day))
        //                             {
        //                                 TimetableEntry validEntry =
        //                                     new TimetableEntry(teacher, subject, group, room, day, hour);
        //                                 Console.WriteLine($"Adding entry: {group._id} - {subject.name} at {hour} on {day}");
        //
        //                                 currentSolution.Add(validEntry);
        //                                 BackTracking();
        //                                 currentSolution.RemoveAt(currentSolution.Count - 1);
        //                             }
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }
        
        
        public void BackTracking()
{
    Console.WriteLine("Back tracking");
    if (isCompleteSolution(currentSolution))
    {
        foreach (var entry in currentSolution)
        {
            timeTableRepo.AddEntry(entry);
        }
        return;
    }

    var subjects = subjectsRepository.GetSubjects();
    var rooms = roomsRepository.GetRooms();
    var teachers = teachersRepository.GetTeachers();
    var groups = groupsRepository.GetGroups();

    while (subjects.Any() && rooms.Any() && teachers.Any() && groups.Any())
    {
        var subject = subjects.First();
        var room = rooms.First();
        var teacher = teachers.First();
        var group = groups.First();

        // Find a valid day combination
        var validDays = days.Where(d => isValid(group, room, teacher, subject, 8, d)).ToList();

        if (!validDays.Any())
        {
            subjects.RemoveAt(0);
            rooms.RemoveAt(0);
            teachers.RemoveAt(0);
            groups.RemoveAt(0);
            continue;
        }

        // Try each valid day
        foreach (TimetableEntry.Days day in validDays)
        {
            // Try each possible time slot (8:00-9:30, 9:30-11:00, etc.)
            for (int hour = startTime; hour <= endTime; hour++)
            {
                if (isValid(group, room, teacher, subject, hour, day))
                {
                    TimetableEntry validEntry =
                        new TimetableEntry(teacher, subject, group, room, day, hour);

                    // Check if this time slot is already used by this teacher for this subject
                    var conflictingEntry = currentSolution.FirstOrDefault(e => 
                        e.teacher == teacher && e.subject == subject && e.day == day && e.hour >= hour && e.hour < (hour + 2));

                    if (conflictingEntry != null)
                    {
                        continue; // Skip this combination as it conflicts with another entry
                    }

                    currentSolution.Add(validEntry);
                    BackTracking();
                    currentSolution.RemoveAt(currentSolution.Count - 1);
                }
            }
        }

        subjects.RemoveAt(0);
        rooms.RemoveAt(0);
        teachers.RemoveAt(0);
        groups.RemoveAt(0);
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

        // public bool doesGroupHaveAlreadyEnoughHoursForSubject(Subject s, Group g, Teacher t)
        // {
        //     if (g.doesGroupHaveSubject(s._id, t._id))
        //     {
        //         int necessaryHours = g.necessarySubjects.GetValueOrDefault(new KeyValuePair<Guid, Guid>(s._id, t._id));
        //         foreach (TimetableEntry te in currentSolution)
        //         {
        //             if (necessaryHours == 0)
        //                 return true;
        //             if (te.subject == s && te.group == g)
        //                 necessaryHours--;
        //         }
        //
        //         if (necessaryHours == 0)
        //             return true;
        //     }
        //
        //     return false;
        // }
        public bool isSpecificRoomNeeded(Subject s)
        {
            if (s.specificRoom)
                return true;
            return false;
        }
        // public bool isValid(Group g, Room r, Teacher t, Subject s, int h, TimetableEntry.Days d) 
        // {
        //     Console.WriteLine($"Checking validity for Group: {g._id}, Subject: {s.name}, Teacher: {t.teacherName}, Room: {r._id}, Hour: {h}, Day: {d}");
        //     if (doesGroupFitInRoom(r, g) &&
        //         g.doesGroupHaveSubject(s._id, t._id) &&
        //         t.DoesTeacherTeachSubject(s._id) &&
        //         isRoomAvailable(r, d, h) &&
        //         !doesGroupHaveAlreadyEnoughHoursForSubject(s, g, t) &&
        //         !isTeacherAlreadyTeachingSubjectOnDay(t, s, d)
        //         && !isHoursExceededForSubject(t, g, s))
        //     {
        //         if (isSpecificRoomNeeded(s))
        //         {
        //             if (r._id != s.roomId)
        //             {
        //                 return false;
        //             }
        //             else
        //                 return true;
        //         }
        //         else
        //         {
        //             return true;
        //         }
        //         
        //     }
        //     
        //     return false;
        //
        // }
        
        public bool isValid(Group group, Room room, Teacher teacher, Subject subject, int hour, TimetableEntry.Days day)
        {
            if (!doesGroupFitInRoom(room, group))
                return false;

            if (!group.doesGroupHaveSubject(subject._id, teacher._id))
                return false;

            if (!teacher.DoesTeacherTeachSubject(subject._id))
                return false;

            if (!isRoomAvailable(room, day, hour))
                return false;

            if (doesGroupHaveAlreadyEnoughHoursForSubject(subject, group, teacher))
                return false;

            if (isTeacherAlreadyTeachingSubjectOnDay(teacher, subject, day))
                return false;

            if (isHoursExceededForSubject(teacher, group, subject))
                return false;

            if (isSpecificRoomNeeded(subject) && room._id != subject.roomId)
                return false;

            return true;
        }


        
        public bool isTeacherAlreadyTeachingSubjectOnDay(Teacher t, Subject s, TimetableEntry.Days day)
        {
            foreach (TimetableEntry entry in currentSolution)
            {
                if (entry.teacher == t && entry.subject == s && entry.day == day)
                {
                    return true;
                }
            }
            return false;
        }
        
        private bool doesGroupHaveAlreadyEnoughHoursForSubject(Subject subject, Group group, Teacher teacher)
        {
            var requiredHours = group.GetRequiredHoursForSubject(subject._id, teacher._id);
            var scheduledHours = currentSolution.Count(entry => entry.subject == subject && entry.group == group);

            return scheduledHours >= requiredHours;
        }

        private bool isHoursExceededForSubject(Teacher teacher, Group group, Subject subject)
        {
            int totalScheduledHours = currentSolution.Count(entry => entry.teacher == teacher && entry.subject == subject);

            int requiredHours = group.GetRequiredHoursForSubject(subject._id, teacher._id);

            return totalScheduledHours > requiredHours;
        }

        

        
        private bool isCompleteSolution(List<TimetableEntry> currentSolution)
        {
            if (currentSolution.Count == 0)
                return false;
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