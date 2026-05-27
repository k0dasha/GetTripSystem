using GetTripSystem.Interfaces;
using GetTripSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;
using static GetTripSystem.DAL;

namespace GetTripSystem
{
    public class FacadeDB : ICreateOperation, IManagement, IRegistration
    {
        private readonly UserRepository _userRepository;
        private readonly TripRepository _tripRepository;
        private readonly RegistrationRepository _regRepository;
        private readonly PictureRepository _picRepository;

        public FacadeDB(UserRepository userRepository, TripRepository tripRepository, 
            PictureRepository pictureRepository, RegistrationRepository registrationRepository)
        {
            _userRepository = userRepository;
            _tripRepository = tripRepository;
            _picRepository = pictureRepository;
            _regRepository = registrationRepository;
        }
        public Task AddUser(string name, string password)
        {
            var pswdHash = Hasher.HashPassword(password);
            return _userRepository.Add(name, pswdHash);
        }
        public Task RegisterTrip(string tripName, string location, int curMembs, int maxMembs,
        int creatorID, string desc, DateTime date, string creatorContact)
        {
            return _tripRepository.Add(tripName, location, curMembs, maxMembs,
        creatorID, desc, date, creatorContact);
        }
        public async Task AddPicture(int tripId, string filePath)
        {
            var fileName = Hasher.HashPicture(filePath);

            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GetTripSystem", "Images");
            Directory.CreateDirectory(folder);
            string newPath = Path.Combine(folder, fileName);
            try
            {
                File.Copy(filePath, newPath);
            }
            catch (IOException ex)
            {
                throw new Exception("Ошибка копирования файла", ex);
            }
            await _picRepository.Add(tripId, fileName);
        }
        public Task<List<Trip>> ToSort(int parametr)
        {
            if (parametr == 0)
            {
                return _tripRepository.SortByDate();
            }
            else
            { return _tripRepository.SortByLocation(); }
        }
        public async Task<List<string>> GetMembersOfTrip(int tripId)
        {
            List<int> userIDs = await _regRepository.GetMembersOfTrip(tripId);
            return await _userRepository.GetUsersNamesByIDs(userIDs);
        }
        public async Task KickMember(int id) //CHECK
        {
            await _regRepository.UpdateMember(id, "kicked");
            await CheckUserBan(id);
        }
        public async Task AddMember(int userID, int tripID)
        {
            var curMembs = await _regRepository.GetCurrentMembersCount(tripID);
            var maxMembs = await _tripRepository.GetMaxMembersCount(tripID);

            if (curMembs < maxMembs)
            {
                await _regRepository.Add(userID, tripID);
                await _tripRepository.IncreaseMembersCount(tripID);
            }
            else throw new InvalidOperationException("Ошибка записи: мест нет");
        }
        
        public Task<List<Trip>> GetAllTrips()
        {
            return _tripRepository.ReadAll();
        }
        public async Task<Trip?> GetTrip(int userID, int tripID)
        {
            var userStatus = await _regRepository.GetUserStatus(userID, tripID);
            if (userStatus == "kicked")
            {
                throw new InvalidOperationException("Запись недоступна");
            }
            else { return await _tripRepository.ReadByID(tripID); }
        }
        public async Task CheckUserBan(int userId) //CHECK
        {
            int count = await _regRepository.GetCountByUser(userId);

            if (count >= 3)
            {
                await _userRepository.UpdateStatus(userId);
            }
        }
        public async Task<List<string>> GetPictures(int tripId)
        {
            return await _picRepository.GetAll(tripId);
        }
    }
}
