using System;
using Task3.DoNotChange;

namespace Task3
{
    public class UserTaskService
    {
        private readonly IUserDao _userDao;

        public UserTaskService(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public void AddTaskForUser(int userId, UserTask task)
        {
            if (userId < 0)
                throw new ArgumentException("Invalid userId");

            var user = _userDao.GetUser(userId);
            if (user == null)
                throw new UserNotFoundException($"User not found");

            var tasks = user.Tasks;
            foreach (var t in tasks)
            {
                if (string.Equals(task.Description, t.Description, StringComparison.OrdinalIgnoreCase))
                    throw new TaskAlreadyExistsException($"Task '{task.Description}' already exists for user {userId}");
            }

            tasks.Add(task);
        }
    }

    // Excepciones personalizadas
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
    }

    public class TaskAlreadyExistsException : Exception
    {
        public TaskAlreadyExistsException(string message) : base(message) { }
    }
}
