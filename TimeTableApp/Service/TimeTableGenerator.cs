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
        private Dictionary<Guid, List<TimetableEntry>> globalRoomAssignments = new Dictionary<Guid, List<TimetableEntry>>();

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

        public void GenerateTimetable()
        {
            var teachers = _teachersRepo.GetTeachers();
            var subjects = _subjectsRepo.GetSubjects();
            var rooms = _roomsRepo.GetRooms();
            var groups = _groupsRepo.GetGroups();
            var days = Enum.GetValues(typeof(TimetableEntry.Days)).Cast<TimetableEntry.Days>();
            var hours = Enumerable.Range(8, 8);

            var validTimetables = new ConcurrentBag<TimetableEntry>();

            Parallel.ForEach(groups, group =>
            {
                GenerateTimetableForGroup(group, teachers, subjects, rooms, days, hours, validTimetables);
            });
            
            // if (!ValidateFinalTimetable(validTimetables, groups))
            // {
            //     Console.WriteLine("Timetable generation failed: Missing or incomplete subject allocations.");
            //     return;
            // }


            foreach (var entry in validTimetables)
            {
                _timetableRepo.AddEntry(entry);
            }

            Console.WriteLine("Exhaustive timetable generation completed.");
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
                        //Console.WriteLine("valid combination found: ");
                        //Console.WriteLine(combination.day.ToString()+ " "+ combination.hour.ToString() + " "+combination.room._id.ToString() +" "+ combination.subject.name.ToString() +" "+ combination.teacher.teacherName.ToString() +" "+ combination.group._id.ToString());
                        //CurrentTimeTableToString(validTimetables);
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
            //Console.WriteLine("all subjects for group " + group._id.ToString());
            foreach (var subject in group.necessarySubjects)
            {
                //Console.WriteLine(subject.Key.ToString() + "from group " + group._id.ToString());
                var subjectId = subject.Key.Key;
                var teacherId = subject.Key.Value;
                var hoursRequired = subject.Value;

                var teacher = teachers.FirstOrDefault(t => t._id == teacherId);
                var subjectEntity = subjects.FirstOrDefault(s => s._id == subjectId);
                
                if (teacher == null || subjectEntity == null)
                    continue;
                
                foreach (var day in days)
                {
                    foreach (var hour in hours)
                    {
                        foreach (var room in rooms)
                        {
                           // if(group._id.ToString() == "c87c9e5b-3a5e-4f0e-badd-990396d12be7")
                               // Console.WriteLine(subjectEntity.name.ToString() + teacher.teacherName.ToString() + group.ToString() + room.ToString() + day.ToString() + hour.ToString());
                            entries.Add(new TimetableEntry(
                                teacher,
                                subjectEntity,
                                group,
                                room,
                                day,
                                hour
                            ));
                        }
                    }
                }
            }

            return entries;
        }

        private bool IsValidEntry(TimetableEntry entry, IEnumerable<TimetableEntry> existingEntries)
        {
            //Console.WriteLine(entry.group._id.ToString() + entry.room._id.ToString() + entry.day.ToString() + entry.hour.ToString());
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

                // Check room availability and usage balance
                if (existingEntries.Any(e =>
                        e.room == entry.room &&
                        e.day == entry.day &&
                        e.hour == entry.hour) ||
                    IsRoomOverused(entry.room, entry.group, existingEntries))
                {
                    //Console.WriteLine("room occupied in that time, so this is not valid: " + entry.group._id.ToString() + entry.room._id.ToString() + entry.day.ToString() + entry.hour.ToString() + entry.subject.name.ToString());
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
                    //Console.WriteLine($"Room occupied in that time, so this is not valid: {entry.group._id} {entry.room._id} {entry.day} {entry.hour} {entry.subject.name}");
                    return false;
                }




                return true;
            }
        }

        private bool IsRoomAvailable(Room room, Group group, TimetableEntry.Days day, int hour,
            IEnumerable<TimetableEntry> existingEntries, TimetableEntry entry)
        {
            lock (GetRoomLock(room, hour, day))
            {
                var occupiedRooms = globalRoomAssignments.GetValueOrDefault(room._id, new List<TimetableEntry>());

                if (occupiedRooms.Any(e =>
                        e.room == room &&
                        e.day == day &&
                        e.hour == hour &&
                        e.group != group))
                {
                   // Console.WriteLine($"Room already occupied at {day} {hour}: {group._id} cannot use {room._id}");
                    return false;
                }

                occupiedRooms.Add(entry);
                globalRoomAssignments[room._id] = occupiedRooms;

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
            return dailySubjectHours.All(hours => hours <= 2); // Max 2 hours per subject per day
        }

        private bool IsRoomOverused(Room room, Group group, IEnumerable<TimetableEntry> timetable)
        {
            var dailyRoomUsage = timetable.Where(e => e.room == room && e.group == group)
                                          .GroupBy(e => e.day)
                                          .Select(g => g.Count());
            return dailyRoomUsage.Any(count => count > 6); // Max 3 uses per day
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
