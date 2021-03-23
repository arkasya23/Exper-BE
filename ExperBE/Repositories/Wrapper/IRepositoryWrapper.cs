using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Repositories.Interfaces;

namespace ExperBE.Repositories.Wrapper
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        ITripRepository Trip { get; }
        IPersonalExpenseRepository PersonalExpense { get; }
        IGroupExpenseRepository GroupExpense { get; }
        INotificationRepository Notification { get; }
        IGroupExpenseUserRepository GroupExpenseUser { get; }
        Task SaveAsync();
    }
}
