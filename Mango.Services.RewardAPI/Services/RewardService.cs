﻿using Mango.Services.RewardAPI.Data;
using Mango.Services.RewardAPI.Message;
using Mango.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.RewardAPI.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public RewardService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task UpdateReward(RewardMessage rewardMessage)
        {
            try
            {
                Reward reward = new()
                {
                    OrderId = rewardMessage.OrderId,
                    RewardsActivity = rewardMessage.RewardsActivity,
                    UserId = rewardMessage.UserId,
                    RewardsDate = DateTime.Now,
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.Rewards.AddAsync(reward);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
