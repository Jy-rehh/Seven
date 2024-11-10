using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Settings> GetSettingsByUsernameAsync(string username)
        {
            // Fetch settings from the database by username
            return await _context.Settings.FirstOrDefaultAsync(s => s.Username == username);
        }

        public async Task UpdateSettings(string username, Settings settings)
        {
            var existingSettings = await _context.Settings
                .FirstOrDefaultAsync(s => s.Username == username);

            if (existingSettings != null)
            {
                // Update the settings
                existingSettings.Field1 = settings.Field1;
                existingSettings.Field2 = settings.Field2;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ResetSettingsToDefault(string username)
        {
            var existingSettings = await _context.Settings
                .FirstOrDefaultAsync(s => s.Username == username);

            if (existingSettings != null)
            {
                // Reset to default values
                existingSettings.Field1 = "DefaultField1";
                existingSettings.Field2 = "DefaultField2";
                await _context.SaveChangesAsync();
            }
        }
    }
}





