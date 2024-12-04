using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableApp.Models;
using TimeTableApp.Repository;

namespace TimeTableApp
{
    public class TimeTableGenerator
    {
        private readonly TeachersRepository _teachersRepo;
        private readonly SubjectsRepository _subjectsRepo;
        private readonly RoomsRepository _roomsRepo;
        private readonly GroupsRepository _groupsRepo;
        private readonly TimeTableRepo _timetableRepo;
        private static readonly object lockObj = new object();
        private readonly Dictionary<string, object> roomLocks = new Dictionary<string, object>();
        //private Dictionary<Guid, List<TimetableEntry>> globalRoomAssignments = new Dictionary<Guid, List<TimetableEntry>>();
        private ConcurrentDictionary<Guid, List<TimetableEntry>> globalRoomAssignments = new ConcurrentDictionary<Guid, List<TimetableEntry>>();

        private object GetRoomLock(Room room, int hour, TimetableEntry.Days day)
        {
            string key = $"{room._id}-{hour}-{day}";
    
            lock (roomLocks)
            {
                if (!roomLocks.ContainsKey(key))
                {
                    roomLocks[key] = new object(); 
                }
                return roomLocks[key];
            }
        }



        public TimeTableGenerator(TeachersRepository teachersRepo, SubjectsRepository subjectsRepo,
                                  RoomsRepository roomsRepo, GroupsRepository groupsRepo, TimeTableRepo timetableRepo)
        {
            _teachersRepo = teachersRepo;
            _subjectsRepo = subjectsRepo;
            _roomsRepo = roomsRepo;
            _groupsRepo = groupsRepo;
            _timetableRepo = timetableRepo;
        }

        // public void GenerateTimetable()
        // {
        //     var teachers = _teachersRepo.GetTeachers();
        //     var subjects = _subjectsRepo.GetSubjects();
        //     var rooms = _roomsRepo.GetRooms();
        //     var groups = _groupsRepo.GetGroups();
        //     var days = Enum.GetValues(typeof(TimetableEntry.Days)).Cast<TimetableEntry.Days>();
        //     var hours = Enumerable.Range(8, 8);
        //
        //     var validTimetables = new ConcurrentBag<TimetableEntry>();
        //
        //     Parallel.ForEach(groups, group =>
        //     {
        //         GenerateTimetableForGroup(group, teachers, subjects, rooms, days, hours, validTimetables);
        //     });
        //     
        //     if (!ValidateFinalTimetable(validTimetables, groups))
        //     {
        //         Console.WriteLine("Timetable generation failed: Missing or incomplete subject allocations.");
        //         return;
        //     }
        //
        //
        //     foreach (var entry in validTimetables)
        //     {
        //         _timetableRepo.AddEntry(entry);
        //     }
        //
        //     Console.WriteLine("Exhaustive timetable generation completed.");
        // }
        // public void GenerateTimetable()
        // {
        //     var teachers = _teachersRepo.GetTeachers();
        //     var subjects = _subjectsRepo.GetSubjects();
        //     var rooms = _roomsRepo.GetRooms();
        //     var groups = _groupsRepo.GetGroups();
        //     var days = Enum.GetValues(typeof(TimetableEntry.Days)).Cast<TimetableEntry.Days>();
        //     var hours = Enumerable.Range(8, 8);
        //
        //     bool isValidTimetable = false;
        //     //ConcurrentBag<TimetableEntry> validTimetables = new ConcurrentBag<TimetableEntry>();
        //     ConcurrentBag<TimetableEntry> validTimetables = new ConcurrentBag<TimetableEntry>();
        //
        //     int maxAttempts = 100; 
        //     int attemptCount = 0;
        //
        //     while (!isValidTimetable)
        //     {
        //         attemptCount++;
        //
        //         // Clear the previous attempt's results
        //         validTimetables.Clear();
        //
        //         // Generate timetables in parallel for each group
        //         Parallel.ForEach(groups, group =>
        //         {
        //             GenerateTimetableForGroup(group, teachers, subjects, rooms, days, hours, validTimetables);
        //         });
        //
        //         // Validate the generated timetable
        //         isValidTimetable = ValidateFinalTimetable(validTimetables, groups);
        //
        //         if (isValidTimetable)
        //         {
        //             // If the timetable is valid, break the loop and save it
        //             foreach (var entry in validTimetables)
        //             {
        //                 _timetableRepo.AddEntry(entry);
        //             }
        //             Console.WriteLine("Exhaustive timetable generation completed.");
        //             break;
        //         }
        //         else
        //         {
        //             // If the timetable is invalid, retry
        //             //CurrentTimeTableToString(validTimetables);
        //            // Console.WriteLine($"Attempt {attemptCount} failed. Retrying...");
        //         }
        //     }
        //
        //     // If no valid timetable was found after the maximum number of attempts, notify the user
        //     if (!isValidTimetable)
        //     {
        //         Console.WriteLine("Unable to generate a valid timetable after multiple attempts.");
        //     }
        //     Console.WriteLine($"Attempt {attemptCount} failed. Retrying...");
        // }
        public void GenerateTimetable()
        {
            var teachers = _teachersRepo.GetTeachers();
            var subjects = _subjectsRepo.GetSubjects();
            var rooms = _roomsRepo.GetRooms();
            var groups = _groupsRepo.GetGroups();
            var days = Enum.GetValues(typeof(TimetableEntry.Days)).Cast<TimetableEntry.Days>();
            var hours = Enumerable.Range(8, 8);

            int maxAttempts = 5000;
            bool isValidTimetable = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                ConcurrentBag<TimetableEntry> validTimetables = new ConcurrentBag<TimetableEntry>();

                // Generate timetables in parallel for each group
                // Parallel.ForEach(groups, group =>
                // {
                //     var localTimetable = new ConcurrentBag<TimetableEntry>();
                //     GenerateTimetableForGroup(group, teachers, subjects, rooms, days, hours, localTimetable);
                //
                //     foreach (var entry in localTimetable)
                //     {
                //         validTimetables.Add(entry);
                //     }
                // });
                Parallel.ForEach(groups, group =>
                {
                    var localTimetable = new ConcurrentBag<TimetableEntry>();
                    GenerateTimetableForGroup(group, teachers, subjects, rooms, days, hours, localTimetable);

                    lock (validTimetables) // Minimized locking for merging local schedules
                    {
                        foreach (var entry in localTimetable)
                        {
                            validTimetables.Add(entry);
                        }
                    }
                });


                // Validate the generated timetable
                if (ValidateFinalTimetable(validTimetables, groups))
                {
                    // Save the valid timetable
                    foreach (var entry in validTimetables)
                    {
                        _timetableRepo.AddEntry(entry);
                    }
                    Console.WriteLine("Valid timetable generated!");
                    isValidTimetable = true;
                    break;
                }
            }

            if (!isValidTimetable)
            {
                Console.WriteLine("Failed to generate a valid timetable after maximum attempts.");
            }
        }

        public void CurrentTimeTableToString(ConcurrentBag<TimetableEntry> validTimetables)
        {
            Console.WriteLine("Current Timetable:");

            foreach (var entry in validTimetables)
            {
                // Format and print each entry in the timetable
                Console.WriteLine($"Teacher: {entry.teacher.teacherName}, " +
                                  $"Subject: {entry.subject.name}, " +
                                  $"Group: {entry.group._id}, " +
                                  $"Room: {entry.room._id}, " +
                                  $"Day: {entry.day}, " +
                                  $"Hour: {entry.hour}");
            }
        }

        private void GenerateTimetableForGroup(Group group, IEnumerable<Teacher> teachers, IEnumerable<Subject> subjects,
                                               IEnumerable<Room> rooms, IEnumerable<TimetableEntry.Days> days,
                                               IEnumerable<int> hours, ConcurrentBag<TimetableEntry> validTimetables)
        {
            var combinations = GetAllCombinations(group, teachers, subjects, rooms, days, hours);
            
            foreach (var combination in combinations)
            {
                if (IsValidEntry(combination, validTimetables))
                {
                    
                    lock (validTimetables)
                    {
                        validTimetables.Add(combination);
                    }
                }
            }
        }

        private IEnumerable<TimetableEntry> GetAllCombinations(Group group, IEnumerable<Teacher> teachers,
                                                               IEnumerable<Subject> subjects, IEnumerable<Room> rooms,
                                                               IEnumerable<TimetableEntry.Days> days,
                                                               IEnumerable<int> hours)
        {
            var entries = new List<TimetableEntry>();
            foreach (var subject in group.necessarySubjects)
            {
                var subjectId = subject.Key.Key;
                var teacherId = subject.Key.Value;
                var hoursRequired = subject.Value;

                var teacher = teachers.FirstOrDefault(t => t._id == teacherId);
                var subjectEntity = subjects.FirstOrDefault(s => s._id == subjectId);
                
                if (teacher == null || subjectEntity == null)
                    continue;
                
                // foreach (var day in days)
                // {
                //     foreach (var hour in hours)
                //     {
                //         foreach (var room in rooms)
                //         {
                //             entries.Add(new TimetableEntry(
                //                 teacher,
                //                 subjectEntity,
                //                 group,
                //                 room,
                //                 day,
                //                 hour
                //             ));
                //         }
                //     }
                // }
                
                foreach (var day in days)
                {
                    foreach (var hour in hours)
                    {
                        if (!IsSlotAvailableForGroup(group, day, hour))
                            continue; 

                        foreach (var room in rooms.Where(r => IsRoomCompatible(subjectEntity, r)))
                        {
                            
                            entries.Add(new TimetableEntry(teacher, subjectEntity, group, room, day, hour));
                        }
                    }
                }

            }

            return entries;
        }
        
        private bool IsSlotAvailableForGroup(Group group, TimetableEntry.Days day, int hour)
        {
            return !globalRoomAssignments.Values
                .SelectMany(entries => entries)
                .Any(entry => entry.group == group && entry.day == day && entry.hour == hour);
        }
        private bool IsRoomCompatible(Subject subject, Room room)
        {
            if (subject.specificRoom)
            {
                Console.WriteLine(subject.name + subject.specificRoom);
                return room._id == subject.roomId;
            }

            return true;
        }


        private bool IsValidEntry(TimetableEntry entry, IEnumerable<TimetableEntry> existingEntries)
        {
            lock (GetRoomLock(entry.room, entry.hour, entry.day))
            {
                if (existingEntries.Any(e =>
                        e.teacher == entry.teacher &&
                        e.day == entry.day &&
                        e.hour == entry.hour) ||
                    IsTeacherOverloaded(entry.teacher, existingEntries, 6, 20))
                {
                    
                    return false;
                }

                if (existingEntries.Any(e =>
                        e.room == entry.room &&
                        e.day == entry.day &&
                        e.hour == entry.hour) ||
                    IsRoomOverused(entry.room, entry.group, existingEntries))
                {
                    return false;
                }

                if (existingEntries.Any(e =>
                        e.group == entry.group &&
                        e.day == entry.day &&
                        e.hour == entry.hour))
                {
                    return false;
                }

                if (!IsSubjectBalanced(entry.subject, entry.group, existingEntries))
                {
                    return false;
                }

                if (GetSubjectAssignedHours(entry.subject, entry.group, existingEntries) >=
                    entry.group.necessarySubjects.First(ns => ns.Key.Key == entry.subject._id).Value)
                {
                    return false;
                }
                if (!IsRoomAvailable(entry.room, entry.group, entry.day, entry.hour, existingEntries, entry))
                {
                    Console.WriteLine($"Room occupied in that time, so this is not valid: {entry.group._id} {entry.room._id} {entry.day} {entry.hour} {entry.subject.name}");
                    return false;
                }

                if (!IsCorrectRoom(entry))
                {
                    return false;
                }
                
                return true;
            }
        }

        private bool IsCorrectRoom(TimetableEntry entry)
        {
            Group group = entry.group;
            Room room = entry.room;
            Subject subject = entry.subject;

            if (subject.specificRoom == true)
            {
                if (subject.roomId == room._id)
                    return true;
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        // private bool IsRoomAvailable(Room room, Group group, TimetableEntry.Days day, int hour,
        //     IEnumerable<TimetableEntry> existingEntries, TimetableEntry entry)
        // {
        //     lock (GetRoomLock(room, hour, day))
        //     {
        //         var occupiedRooms = globalRoomAssignments.GetValueOrDefault(room._id, new List<TimetableEntry>());
        //
        //         if (occupiedRooms.Any(e =>
        //                 e.room == room &&
        //                 e.day == day &&
        //                 e.hour == hour &&
        //                 e.group != group))
        //         {
        //            // Console.WriteLine($"Room already occupied at {day} {hour}: {group._id} cannot use {room._id}");
        //             return false;
        //         }
        //
        //         occupiedRooms.Add(entry);
        //         globalRoomAssignments[room._id] = occupiedRooms;
        //
        //         return true;
        //     }
        // }
        
        // private bool IsRoomAvailable(Room room, Group group, TimetableEntry.Days day, int hour,
        //     IEnumerable<TimetableEntry> existingEntries, TimetableEntry entry)
        // {
        //     // Create a copy of the existing entries to avoid modifying the collection during iteration
        //     var existingEntriesList = existingEntries.ToList(); 
        //
        //     lock (GetRoomLock(room, hour, day)) // Locking to ensure thread safety
        //     {
        //         // Iterate over the copied list of existing entries
        //         if (existingEntriesList.Any(e => 
        //                 e.room == room && 
        //                 e.day == day && 
        //                 e.hour == hour && 
        //                 e.group != group))
        //         {
        //             return false; // If any conflict is found, room is not available
        //         }
        //
        //         // Add the new entry after validation
        //         existingEntriesList.Add(entry); // Only add after the iteration is done
        //
        //         // Assuming you're updating the global collection or a specific room entry
        //         globalRoomAssignments[room._id] = existingEntriesList; 
        //         return true;
        //     }
        // }
        
        private bool IsRoomAvailable(Room room, Group group, TimetableEntry.Days day, int hour,
            IEnumerable<TimetableEntry> existingEntries, TimetableEntry entry)
        {
            lock (GetRoomLock(room, hour, day)) // Thread-safe check
            {
                // Check if the room is already occupied at the same time
                if (globalRoomAssignments.TryGetValue(room._id, out var roomEntries))
                {
                    if (roomEntries.Any(e => e.day == day && e.hour == hour && e.group != group))
                    {
                        return false;
                    }
                }

                // Add the entry after validation
                if (!globalRoomAssignments.ContainsKey(room._id))
                {
                    globalRoomAssignments[room._id] = new List<TimetableEntry>();
                }

                globalRoomAssignments[room._id].Add(entry); // Atomically add the new entry
                return true;
            }
        }

       



        private int GetSubjectAssignedHours(Subject subject, Group group, IEnumerable<TimetableEntry> timetable)
        {
            return timetable.Count(e => e.subject == subject && e.group == group);
        }


        private bool IsTeacherOverloaded(Teacher teacher, IEnumerable<TimetableEntry> timetable, int maxHoursPerDay, int maxHoursPerWeek)
        {
            foreach (var day in Enum.GetValues(typeof(TimetableEntry.Days)).Cast<TimetableEntry.Days>())
            {
                int dailyHours = timetable.Count(e => e.teacher == teacher && e.day == day);
                if (dailyHours > maxHoursPerDay)
                    return true;
            }

            int weeklyHours = timetable.Count(e => e.teacher == teacher);
            return weeklyHours > maxHoursPerWeek;
        }

        private bool IsSubjectBalanced(Subject subject, Group group, IEnumerable<TimetableEntry> timetable)
        {
            var dailySubjectHours = timetable.Where(e => e.subject == subject && e.group == group)
                                             .GroupBy(e => e.day)
                                             .Select(g => g.Count());
            return dailySubjectHours.All(hours => hours <= 5); // Max 2 hours per subject per day
        }

        private bool IsRoomOverused(Room room, Group group, IEnumerable<TimetableEntry> timetable)
        {
            var dailyRoomUsage = timetable.Where(e => e.room == room && e.group == group)
                                          .GroupBy(e => e.day)
                                          .Select(g => g.Count());
            return dailyRoomUsage.Any(count => count > 8); 
        }
        
        private bool ValidateFinalTimetable(IEnumerable<TimetableEntry> timetable, IEnumerable<Group> groups)
        {
            foreach (var group in groups)
            {
                foreach (var requiredSubject in group.necessarySubjects)
                {
                    var subjectId = requiredSubject.Key.Key;
                    var hoursRequired = requiredSubject.Value;

                    var totalAssignedHours = timetable.Count(e => 
                        e.group == group && e.subject._id == subjectId);

                    if (totalAssignedHours < hoursRequired)
                    {
                        return false; 
                    }
                }
            }
            return true;
        }

    }
}
