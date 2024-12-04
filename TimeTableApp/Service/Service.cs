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


        public Service(GroupsRepository groupsRepository, TeachersRepository teacherRepo, SubjectsRepository subjectRepo, RoomsRepository roomRepo, TimeTableRepo orarRepo, int startTime, int endTime)
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
       

        public bool doesGroupFitInRoom(Room r, Group g)
        {
            if (r.capacity >= g.nrOfKids)
                return true;
            return false;
        }
        public bool isSpecificRoomNeeded(Subject s)
        {
            if (s.specificRoom)
                return true;
            return false;
        }
       

        public void GenerateTimetable()
        {
            BackTracking(8, 0);
        }

        // public bool BackTracking(int index)
        // {
        //     if (index == groupsRepository.GetGroups().Count)
        //     {
        //         Console.WriteLine("Possible solution");
        //         foreach (var nuStiu in currentSolution)
        //         {
        //             Console.WriteLine($"{nuStiu.hour}, day {nuStiu.day}, subject {nuStiu.subject._id}, teacher {nuStiu.teacher.teacherName}, room {nuStiu.room._id}, group {nuStiu.group._id}");
        //         }
        //         if (isCompleteSolution(currentSolution))
        //         {
        //             foreach (var entry in currentSolution)
        //             {
        //                 timeTableRepo.AddEntry(entry); // Save the solution
        //             }
        //             return true;
        //         }
        //         return false;
        //     }
        //
        //     var groups = groupsRepository.GetGroups();
        //     var group = groups[index]; // Process current group
        //
        //     foreach (var subject in subjectsRepository.GetSubjects())
        //     {
        //         foreach (var teacher in teachersRepository.GetTeachers())
        //         {
        //             if (!group.doesGroupHaveSubject(subject._id, teacher._id))
        //             {
        //                 Console.WriteLine($"Group {group._id} cannot have subject {subject._id}");
        //                 continue;
        //             }
        //             if (!teacher.DoesTeacherTeachSubject(subject._id))
        //             {
        //                 Console.WriteLine($"Teacher {teacher.teacherName} cannot teach subject {subject._id}");
        //                 continue;
        //             }
        //
        //             foreach (var room in roomsRepository.GetRooms())
        //             {
        //                 if (!doesGroupFitInRoom(room, group)) continue;
        //                 if (isSpecificRoomNeeded(subject) && room._id != subject.roomId) continue;
        //
        //                 foreach (var day in days)
        //                 {
        //                     for (int hour = startTime; hour < endTime; hour++)
        //                     {
        //                         
        //                         if (isValid(group, room, teacher, subject, hour, day))
        //                         {
        //                             var timetableEntry = new TimetableEntry(teacher, subject, group, room, day, hour);
        //                             currentSolution.Add(timetableEntry);
        //                             Console.WriteLine($"Checking hour {hour}, day {day}, subject {subject._id}, teacher {teacher.teacherName}, room {room._id}, group {group._id}");
        //                             if (BackTracking(index + 1)) return true;
        //
        //                             currentSolution.RemoveAt(currentSolution.Count - 1);
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //
        //     return false;
        // }
//         public bool BackTracking(int index)
// {
//     bool found = false;
//     Console.WriteLine(found + "in index: ");
//     Console.WriteLine(index);
//     if (index == groupsRepository.GetGroups().Count)
//     {
//         Console.WriteLine("Possible solution");
//         foreach (var nuStiu in currentSolution)
//         {
//             Console.WriteLine($"{nuStiu.hour}, day {nuStiu.day}, subject {nuStiu.subject._id}, teacher {nuStiu.teacher.teacherName}, room {nuStiu.room._id}, group {nuStiu.group._id}");
//         }
//
//         if (isCompleteSolution(currentSolution))
//         {
//             foreach (var entry in currentSolution)
//             {
//                 timeTableRepo.AddEntry(entry); // Save the solution
//             }
//             return true;
//         }
//         return false;
//     }
//
//     var groups = groupsRepository.GetGroups();
//     var group = groups[index]; // Process current group
//     var numbersubjects = group.necessarySubjects.Count;
//     int count = 0;
//     foreach (var subjectEntry in group.necessarySubjects)
//     {
//         found = false;
//         var subjectId = subjectEntry.Key.Key;
//         var teacherId = subjectEntry.Key.Value;
//         var requiredHours = subjectEntry.Value;
//
//         var subject = subjectsRepository.GetSubjects().FirstOrDefault(s => s._id == subjectId);
//         var teacher = teachersRepository.GetTeachers().FirstOrDefault(t => t._id == teacherId);
//
//         if (subject == null || teacher == null) continue;
//         foreach (var room in roomsRepository.GetRooms() )
//         {
//             if (found) break;
//             if (!doesGroupFitInRoom(room, group)) continue;
//             if (isSpecificRoomNeeded(subject) && room._id != subject.roomId) continue;
//
//             foreach (var day in days)
//             {
//                 if (found) break;
//                 
//                 for (int hour = startTime; hour < endTime && !found ; hour++)
//                 {
//                     if (isValid(group, room, teacher, subject, hour, day))
//                     {
//                         var timetableEntry = new TimetableEntry(teacher, subject, group, room, day, hour);
//                         currentSolution.Add(timetableEntry); // Add current valid entry
//                         found = true;
//                         count++;
//                         if (count == numbersubjects)
//                         {
//                             
//                             Console.WriteLine("in if count = nrsubjects cu index" + index);
//                             if (BackTracking(index + 1))
//                             {
//                                 return true;
//                             }
//
//                             // If the solution is not found, backtrack
//                             currentSolution.RemoveAt(currentSolution.Count - 1);
//                         }
//
//                     }
//                 }
//             }
//         }
//     }
//     
//     return false;
// // }
// public bool BackTracking(int currentHour, int currentDayIndex)
// {
//     bool found = false;
//     var daysList = days;  // List of available days (e.g., [Monday, Tuesday, ...])
//
//     Console.WriteLine($"Starting backtracking at hour: {currentHour}, day index: {currentDayIndex}");
//
//     // Check if we've processed all time slots across all days
//     if (currentHour >= endTime && currentDayIndex >= daysList.Count)
//     {
//         Console.WriteLine("Possible solution:");
//         foreach (var nuStiu in currentSolution)
//         {
//             Console.WriteLine($"{nuStiu.hour}, day {nuStiu.day}, subject {nuStiu.subject._id}, teacher {nuStiu.teacher.teacherName}, room {nuStiu.room._id}, group {nuStiu.group._id}");
//         }
//
//         if (isCompleteSolution(currentSolution))
//         {
//             foreach (var entry in currentSolution)
//             {
//                 timeTableRepo.AddEntry(entry); // Save the solution
//             }
//             return true;
//         }
//         return false;
//     }
//
//     // Process subjects for all groups
//     var groups = groupsRepository.GetGroups();
//     foreach (var group in groups)
//     {
//         foreach (var subjectEntry in group.necessarySubjects)
//         {
//             found = false;
//             var subjectId = subjectEntry.Key.Key;
//             var teacherId = subjectEntry.Key.Value;
//             var requiredHours = subjectEntry.Value;
//
//             var subject = subjectsRepository.GetSubjects().FirstOrDefault(s => s._id == subjectId);
//             var teacher = teachersRepository.GetTeachers().FirstOrDefault(t => t._id == teacherId);
//
//             if (subject == null || teacher == null) continue;
//
//             foreach (var room in roomsRepository.GetRooms())
//             {
//                 if (found) break;
//                 if (!doesGroupFitInRoom(room, group)) continue;
//                 if (isSpecificRoomNeeded(subject) && room._id != subject.roomId) continue;
//
//                 // Get the current day from the list
//                 var currentDay = daysList[currentDayIndex];
//
//                 // Loop through the hours for this day
//                 for (int hour = currentHour; hour < endTime && !found; hour++)
//                 {
//                     if (isValid(group, room, teacher, subject, hour, currentDay))
//                     {
//                         var timetableEntry = new TimetableEntry(teacher, subject, group, room, currentDay, hour);
//                         currentSolution.Add(timetableEntry); // Add current valid entry
//                         found = true;
//
//                         // If all subjects are scheduled, proceed to the next time slot or day
//                         if (found)
//                         {
//                             // Check if we have reached the last day and hour
//                             if (currentDayIndex == daysList.Count - 1 && hour == endTime - 1)
//                             {
//                                 return true; // If we've filled all slots, return success.
//                             }
//
//                             // Proceed to the next hour, or go to the next day
//                             if (hour == endTime - 1)
//                             {
//                                 // Move to the next day after the last hour
//                                 if (BackTracking(startTime, currentDayIndex + 1)) // Move to next day
//                                 {
//                                     return true;
//                                 }
//                             }
//                             else
//                             {
//                                 // Continue with the next hour in the current day
//                                 if (BackTracking(hour + 1, currentDayIndex))
//                                 {
//                                     return true;
//                                 }
//                             }
//
//                             // If the solution is not found, backtrack
//                             currentSolution.RemoveAt(currentSolution.Count - 1);
//                         }
//                     }
//                 }
//             }
//         }
//     }
//
//     return false;
// }
public bool BackTracking(int currentHour, int currentDayIndex)
{
    if (currentHour >= endTime && currentDayIndex == days.Count)
    {
        Console.WriteLine("Possible solution:");
        foreach (var nuStiu in currentSolution)
        {
            Console.WriteLine($"{nuStiu.hour}, day {nuStiu.day}, subject {nuStiu.subject._id}, teacher {nuStiu.teacher.teacherName}, room {nuStiu.room._id}, group {nuStiu.group._id}");
        }

        // If this is a complete solution, save it and return true
        if (isCompleteSolution(currentSolution))
        {
            foreach (var entry in currentSolution)
            {
                timeTableRepo.AddEntry(entry); // Save the solution
            }
            return true;
        }
        return false;
    }

    // Process subjects for all groups
    var groups = groupsRepository.GetGroups();
    foreach (var group in groups)
    {
        foreach (var subjectEntry in group.necessarySubjects)
        {
            var subjectId = subjectEntry.Key.Key;
            var teacherId = subjectEntry.Key.Value;
            var subject = subjectsRepository.GetSubjects().FirstOrDefault(s => s._id == subjectId);
            var teacher = teachersRepository.GetTeachers().FirstOrDefault(t => t._id == teacherId);

            if (subject == null || teacher == null) continue;

            foreach (var room in roomsRepository.GetRooms())
            {
                if (!doesGroupFitInRoom(room, group)) continue;
                if (isSpecificRoomNeeded(subject) && room._id != subject.roomId) continue;
                if (currentDayIndex >= days.Count)
                {
                    return false;
                }
                if (currentDayIndex >= days.Count)
                {
                    return false;
                }

                // Get the current day from the list
                var currentDay = days[currentDayIndex];

                // Loop through the hours for this day
                for (int hour = currentHour; hour < endTime; hour++)
                {
                    if (isValid(group, room, teacher, subject, hour, currentDay))
                    {
                        var timetableEntry = new TimetableEntry(teacher, subject, group, room, currentDay, hour);
                        currentSolution.Add(timetableEntry); // Add current valid entry

                        // Proceed to the next hour, or go to the next day
                        if (hour == endTime - 1)
                        {
                            // If we're at the last hour of the day, move to the next day
                            if (BackTracking(8, currentDayIndex + 1)) // Move to the next day after finishing the current hour
                            {
                                return true;
                            }
                        }
                        else
                        {
                            // Otherwise, proceed to the next hour in the current day
                            if (BackTracking(hour + 1, currentDayIndex)) // Move to next hour in the same day
                            {
                                return true;
                            }
                        }

                        // If the solution is not found, backtrack
                        currentSolution.RemoveAt(currentSolution.Count - 1);
                    }
                }
            }
        }
    }

    // No solution found for this path
    return false;
}


        public bool isTeacherAlreadyTeachingSubjectOnDay(Teacher teacher, Subject subject, TimetableEntry.Days day)
        {
            return currentSolution.Any(entry =>
                entry.teacher == teacher &&
                entry.subject == subject &&
                entry.day == day);
        }
        public bool isHoursExceededForSubject(Teacher teacher, Group group, Subject subject)
        {
            int requiredHours = group.GetRequiredHoursForSubject(subject._id, teacher._id);
            int scheduledHours = currentSolution
                .Where(entry => entry.teacher == teacher && entry.group == group && entry.subject == subject)
                .Count();

            return scheduledHours >= requiredHours;
        }

        public bool isValid(Group group, Room room, Teacher teacher, Subject subject, int hour, TimetableEntry.Days day)
        {
            if (!doesGroupFitInRoom(room, group))
            {
               // Console.WriteLine($"Group {group._id} doesn't fit in room {room._id}.");
                return false;
            }

            if (!group.doesGroupHaveSubject(subject._id, teacher._id))
            {
               // Console.WriteLine($"Group {group._id} doesn't have subject {subject._id} for teacher {teacher.teacherName}.");
                return false;
            }

            if (!teacher.DoesTeacherTeachSubject(subject._id))
            {
               // Console.WriteLine($"Teacher {teacher.teacherName} can't teach subject {subject._id}.");
                return false;
            }
                if (!isRoomAvailable(room, day, hour))
                {
                  // Console.WriteLine($"Room {room._id} is not available at hour {hour} on day {day}.");
                    return false;
                }

                if (doesGroupHaveAlreadyEnoughHoursForSubject(subject, group, teacher))
                {
                  //  Console.WriteLine($"Group {group._id} already has enough hours for subject {subject._id}.");
                    return false;
                }

                if (isTeacherAlreadyTeachingSubjectOnDay(teacher, subject, day))
                {
                   // Console.WriteLine($"Teacher {teacher.teacherName} is already teaching subject {subject._id} on day {day}.");
                    return false;
                }

                if (isHoursExceededForSubject(teacher, group, subject))
                {
                   // Console.WriteLine($"Teacher {teacher.teacherName} has exceeded the hours for subject {subject._id}.");
                    return false;
                }

                if (isSpecificRoomNeeded(subject) && room._id != subject.roomId)
                {
                   // Console.WriteLine($"Subject {subject._id} requires a specific room {subject.roomId}, not room {room._id}.");
                    return false;
                }

                return true;
            }

          
            
            private bool isCompleteSolution(List<TimetableEntry> currentSolution)
            {
                foreach (var group in groupsRepository.GetGroups())
                {
                    foreach (var subject in subjectsRepository.GetSubjects())
                    {
                        // Check if this subject is assigned to the current group in any timetable entry
                        var subjectEntries = currentSolution
                            .Where(entry => entry.group._id == group._id && entry.subject._id == subject._id && group.doesGroupHaveSubject((subject._id)))
                            .ToList();

                        if (!subjectEntries.Any())
                        {
                            // No entry for this subject and group combination
                            // Console.WriteLine($"Subject {subject._id} is missing for group {group._id}");
                            return false;
                        }

                        // Check if all required hours for this subject and group are met
                        var assignedTeachers = subjectEntries
                            .Select(entry => entry.teacher)
                            .Distinct();

                        foreach (var teacher in assignedTeachers)
                        {
                            int requiredHours = group.GetRequiredHoursForSubject(subject._id, teacher._id);
                            int scheduledHours = subjectEntries
                                .Count(entry => entry.teacher == teacher);

                            if (scheduledHours < requiredHours)
                            {
                                Console.WriteLine($"Insufficient hours for Subject {subject._id} and Teacher {teacher._id} in Group {group._id}");
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

            public bool isRoomAvailable(Room room, TimetableEntry.Days day, int hour)
            {
                return !currentSolution.Any(te => te.room == room && te.day == day && te.hour == hour);
            }
            public bool doesGroupHaveAlreadyEnoughHoursForSubject(Subject subject, Group group, Teacher teacher)
            {
                int requiredHours = group.GetRequiredHoursForSubject(subject._id, teacher._id);
                int scheduledHours = currentSolution
                    .Count(te => te.subject == subject && te.group == group && te.teacher == teacher);

                return scheduledHours >= requiredHours;
            }

        }
    
}