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
        public Task RegisterUser(string name, string password)
        {
            var pswdHash = Hasher.HashPassword(password);
            return _userRepository.Add(name, pswdHash);
        }
        public async Task<User?> GetUser(string userName, string passwd)
        {
            var user = await _userRepository.GetUser(userName);
            var verified = Hasher.VerifyPassword(passwd, user.PasswdHash);

            if (verified)
                return user;
            else 
                throw new InvalidOperationException();
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
        public async Task<List<User>> GetMembersOfTrip(int tripId)
        {
            List<int> userIDs = await _regRepository.GetMembersOfTrip(tripId);
            return await _userRepository.GetUsersByIDs(userIDs);
        }
        public async Task KickMember(int userId, int tripId)
        {
            await _regRepository.UpdateMember(tripId, "kicked", userId);
            await _tripRepository.DecreaseMembersCount(tripId);
            await CheckUserBan(userId);
        }
        public async Task CancelRegistration(int userId, int tripId)
        {
            int leftCount = await _regRepository.GetLeftsCount(tripId, userId);
            if (leftCount < 1)
            {
                await _regRepository.UpdateMember(tripId, "left", userId);
                await _tripRepository.DecreaseMembersCount(tripId);
            }
            else throw new InvalidOperationException();
        }
        public async Task AddMember(int userID, int tripID)
        {
            var curMembs = await _regRepository.GetCurrentMembersCount(tripID);
            var maxMembs = await _tripRepository.GetMaxMembersCount(tripID);
            var repeatReg = await _regRepository.AwareRepeatedRegistration(userID, tripID);

            if (curMembs < maxMembs)
            {
                if (repeatReg == true)
                {
                    await _regRepository.Add(userID, tripID);
                    await _tripRepository.IncreaseMembersCount(tripID);
                }
                else throw new InvalidOperationException("Повторная запись невозможна");
            }
            else throw new InvalidOperationException("Ошибка записи: мест нет");
        }
        public async Task<List<Trip>> GetUserRegistrations(int userId)
        {
            List<int> tripIDs = await _regRepository.GetUserRegs(userId);
            return await _tripRepository.GetTripsByIDs(tripIDs);
        }
        public async Task<List<Trip>> GetUserTrips(int userId)
        {
            return await _tripRepository.GetTripsByCreatorID(userId);
        }
        public Task<List<Trip>> GetAllTrips(int userId)
        {
            return _tripRepository.ReadAll(userId);
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
        public async Task CheckUserBan(int userId)
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
