using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;
        public UserRepository(AsiBasecodeDBContext dBContext, IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
            _dbContext = dBContext;
        }

        public IQueryable<User> GetUsers()
        {
            return this.GetDbSet<User>();
        }

        public bool UserExists(string userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }
        public User GetUserByUserId(string userId)
        {
            return this.GetDbSet<User>().FirstOrDefault(x => x.UserId == userId);
        }

        public void UpdateUser(User user)
        {
            var dbSet = this.GetDbSet<User>();
            dbSet.Update(user);
            UnitOfWork.SaveChanges();
        }

        public User GetUserByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            return _dbContext.Users.FirstOrDefault(u => u.Token == token && u.TokenExpiry > DateTime.Now);
        }

    }
}
