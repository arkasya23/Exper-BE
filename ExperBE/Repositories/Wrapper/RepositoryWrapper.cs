using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Data;
using ExperBE.Repositories.Interfaces;

namespace ExperBE.Repositories.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ExperDbContext _context;
        private IUserRepository? _userRepository;
        private ITripRepository? _tripRepository;
        private IPersonalExpenseRepository? _personalExpenseRepository;
        private IGroupExpenseRepository? _groupExpenseRepository;
        private INotificationRepository? _notificationRepository;
        private IGroupExpenseUserRepository? _groupExpenseUserRepository;

        public RepositoryWrapper(ExperDbContext context) => _context = context;
        public IUserRepository User => _userRepository ??= new UserRepository(_context);
        public ITripRepository Trip => _tripRepository ??= new TripRepository(_context);
        public IPersonalExpenseRepository PersonalExpense => _personalExpenseRepository ??= new PersonalExpenseRepository(_context);
        public IGroupExpenseRepository GroupExpense => _groupExpenseRepository ??= new GroupExpenseRepository(_context);
        public INotificationRepository Notification => _notificationRepository ??= new NotificationRepository(_context);
        public IGroupExpenseUserRepository GroupExpenseUser => _groupExpenseUserRepository ??= new GroupExpenseUserRepository(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
