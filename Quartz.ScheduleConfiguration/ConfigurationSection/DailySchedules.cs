using System.Configuration;
using System.Linq;

namespace Quartz.ScheduleConfiguration.ConfigurationSection
{
    public class DailySchedules : System.Configuration.ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = false, IsKey = false, IsDefaultCollection = true)]
        public ScheduleCollection Items
        {
            get { return ((ScheduleCollection)(base[""])); }
            set { base[""] = value; }
        }

        public string GetCronValue(string scheduleName)
        {
            return CronString.FromSchedule(Items.Cast<ScheduleElement>().FirstOrDefault(e => e.Name == scheduleName));
        }
    }
}
