using GetTripSystem.Interfaces;
using GetTripSystem.Repositories;
using Microsoft.EntityFrameworkCore;
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
        public Task RegisterTrip(int id, string tripName, string location, int curMembs, int maxMembs,
        int creatorID, string desc, DateTime date, string creatorContact)
        {
            return _tripRepository.Add(id, tripName, location, curMembs, maxMembs,
        creatorID, desc, date, creatorContact);
        }
        public async Task AddPicture(int tripId, string filePath)
        {
            var fileName = Hasher.HashPicture(); //Надо сделать прям путь с помощью имени файла?
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
        public async Task KickMember(int id, string status)
        {
            await _regRepository.UpdateMember(id, status = "kicked");
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
            else throw new InvalidOperationException("Trip is full");
        }
        
        public Task<List<Trip>> GetAllTrips()
        {
            //учесть наступление дат созданных походов?
            return _tripRepository.ReadAll();
        }
        public async Task<Trip?> GetTrip(int userID, int tripID)
        {
            //использовать getUserStatus()
            var userStatus = await _regRepository.GetUserStatus(userID, tripID);
            if (userStatus == "kicked")
            {
                return null;
            }
            else { return await _tripRepository.ReadByID(tripID); }
        }
    }
}
