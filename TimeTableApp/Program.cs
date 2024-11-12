using TimeTableApp.Repository;

namespace TimeTableApp
{
    //Generate a school timetable through exhaustive search.
    class Program
    {
        static void Main(string[] args)
        {
            int startTime=8, endTime=14;
            TeachersRepository teachersRepository = new TeachersRepository();
            SubjectsRepository subjectsRepository = new SubjectsRepository();
            RoomsRepository roomsRepository = new RoomsRepository();
            TimeTableRepo timeTableRepo = new TimeTableRepo();
            
            Service service = new Service(teachersRepository, subjectsRepository, roomsRepository, timeTableRepo, startTime, endTime);
            service.BackTracking();

            timeTableRepo.PrintToCSV("result.csv");
            Console.WriteLine("hel");
        }
    }
}