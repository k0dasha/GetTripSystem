using GetTripSystem.Entities;
using GetTripSystem.Interfaces;
using GetTripSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GetTripSystem
{
    public class FacadeDB : ICreateOperation, IManagement, IRegistration
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public FacadeDB(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public Task RegisterUser(string name, string password)
        {
            var pswdHash = Hasher.HashPassword(password);
            using (var scope = _scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
                return userRepository.Add(name, pswdHash); 
            }
        }
        public async Task<User?> GetUser(string userName, string passwd)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
                var user = await userRepository.GetUser(userName);

                if (user != null)
                {
                    var verified = Hasher.VerifyPassword(passwd, user.PasswdHash);
                    if (verified)
                        return user;
                    else
                        throw new InvalidOperationException("Неверный пароль");
                }
                else
                    throw new ArgumentException("Пользователь не найден");
            }

        }
        public async Task RegisterTrip(string tripName, string location, int curMembs, int maxMembs,
        int creatorID, string desc, DateTime date, string creatorContact)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();
                await tripRepository.Add(tripName, location, curMembs, maxMembs,
                    creatorID, desc, date, creatorContact);
            }
        }
        public async Task AddPicture(int tripId, string filePath)
        {
            var fileName = Hasher.HashPicture(filePath);
            ImageStorage.CopyImage(fileName, filePath);
            
            using (var scope = _scopeFactory.CreateScope())
            { 
                var picRepository = scope.ServiceProvider.GetRequiredService<PictureRepository>();
                await picRepository.Add(tripId, fileName); 
            } 
        }
        public List<Trip> ToSort(int parametr, List<Trip> list)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();
                {
                    if (parametr == 0)
                    {
                        return tripRepository.SortByDate(list);
                    }
                    else
                    { return tripRepository.SortByLocation(list); }
                }
            }
        }
        public async Task<List<User>> GetMembersOfTrip(int tripId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
                var regRepository = scope.ServiceProvider.GetRequiredService<RegistrationRepository>();
                {
                    {
                        List<int> userIDs = await regRepository.GetMembersOfTrip(tripId);
                        return await userRepository.GetUsersByIDs(userIDs);
                    }
                }
            }
            
        }
        public async Task KickMember(int userId, int tripId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();
                var regRepository = scope.ServiceProvider.GetRequiredService<RegistrationRepository>();
                {
                    {
                        await regRepository.UpdateMember(tripId, "kicked", userId);
                        await tripRepository.DecreaseMembersCount(tripId);
                    }
                }
            }
            await CheckUserBan(userId);
        }
        public async Task CancelRegistration(int userId, int tripId)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();
                var regRepository = scope.ServiceProvider.GetRequiredService<RegistrationRepository>();
                {
                    
                    int leftCount = await regRepository.GetLeftsCount(tripId, userId);
                    if (leftCount < 1)
                    {
                        await regRepository.UpdateMember(tripId, "left", userId);
                        await tripRepository.DecreaseMembersCount(tripId);
                    }
                    else throw new InvalidOperationException();
                }
            }
        }
        public async Task AddMember(int userID, int tripID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();
                var regRepository = scope.ServiceProvider.GetRequiredService<RegistrationRepository>();

                var curMembs = await regRepository.GetCurrentMembersCount(tripID);
                var maxMembs = await tripRepository.GetMaxMembersCount(tripID);
                var repeatReg = await regRepository.AwareRepeatedRegistration(userID, tripID);

                if (curMembs < maxMembs)
                {
                    if (repeatReg == true)
                    {
                        await regRepository.Add(userID, tripID);
                        await tripRepository.IncreaseMembersCount(tripID);
                    }
                    else throw new ArgumentException("Повторная запись невозможна");
                }
                else throw new InvalidOperationException("Ошибка записи: мест нет");
            }
        }
        public async Task<List<Trip>> GetUserRegistrations(int userId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();

                var regRepository = scope.ServiceProvider.GetRequiredService<RegistrationRepository>();
                List<int> tripIDs = await regRepository.GetUserRegs(userId);
                return await tripRepository.GetTripsByIDs(tripIDs);
            }
        }
        public async Task<List<Trip>> GetUserTrips(int userId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();
                return await tripRepository.GetTripsByCreatorID(userId);
            }
        }
        public async Task<List<Trip>> GetAllTrips(int userId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();
                var result = await tripRepository.ReadAll(userId);
                return result;
            }
        }
        public async Task<Trip?> GetTrip(int userID, int tripID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tripRepository = scope.ServiceProvider.GetRequiredService<TripRepository>();
                var regRepository = scope.ServiceProvider.GetRequiredService<RegistrationRepository>();

                var userStatus = await regRepository.GetUserStatus(userID, tripID);
                if (userStatus == "kicked")
                {
                    throw new InvalidOperationException("Запись недоступна");
                }
                else { return await tripRepository.ReadByID(tripID); }
            }
            
        }
        public async Task CheckUserBan(int userId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
                var regRepository = scope.ServiceProvider.GetRequiredService<RegistrationRepository>();

                int count = await regRepository.GetCountByUser(userId);

                if (count >= 3)
                {
                    await userRepository.UpdateStatus(userId);
                }
            }
        }
        public async Task<List<string>> GetPictures(int tripId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var picRepository = scope.ServiceProvider.GetRequiredService<PictureRepository>();

                var pictures = await picRepository.GetAll(tripId);

                var fullPaths = pictures
                    .Select(p => ImageStorage.GetImagePath(p.FileName))
                    .ToList();

                return fullPaths;
            }
        }
    }
}
