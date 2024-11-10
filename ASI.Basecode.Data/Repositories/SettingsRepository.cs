using ASI.Basecode.Data.Interfaces;  // Import the interface
using ASI.Basecode.Data.Models;  // Import the models
using System.Linq;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _context;  // Database context for data access

        // Constructor to inject the ApplicationDbContext
        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get user settings by username
        public async Task<Settings> GetSettingsByUsernameAsync(string username)
        {
            // Use asynchronous query to fetch the settings
            return await _context.Settings.FirstOrDefaultAsync(s => s.Username == username);
        }

        // Update settings for the user
        public async Task UpdateSettings(string username, SettingsViewModel model)
        {
            // Fetch the existing settings
            var existingSettings = await _context.Settings.FirstOrDefaultAsync(s => s.Username == username);
            if (existingSettings != null)
            {
                // Update the settings fields (you'll need to map fields from the ViewModel)
                existingSettings.Field1 = model.Field1;  // Example field
                existingSettings.Field2 = model.Field2;  // Example field

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }

        // Reset user settings to default
        public async Task ResetSettingsToDefault(string username)
        {
            // Fetch the settings
            var settings = await _context.Settings.FirstOrDefaultAsync(s => s.Username == username);
            if (settings != null)
            {
                // Reset the settings to their default values
                settings.Field1 = "Default";  // Example default value
                settings.Field2 = "Default";  // Example default value

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }
    }
}




