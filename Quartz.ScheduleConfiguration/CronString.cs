using System;
using Quartz.ScheduleConfiguration.ConfigurationSection;

namespace Quartz.ScheduleConfiguration
{
    public static class CronString
    {
        public static string FromSchedule(ScheduleElement config)
        {
            var hours = GenerateCronValue(config.Hours);
            var minutes = GenerateCronValue(config.Minutes);
            var seconds = GenerateCronValue(config.Seconds);
            return $"{seconds} {minutes} {hours} * * ? *";
        }

        private static string GenerateCronValue(UnitElement unit)
        {
            switch (unit.Type)
            {
                case ScheduleType.All:
                    return "*";
                case ScheduleType.Values:
                    return unit.Values;
                case ScheduleType.Interval:
                    return $"{unit.Start}/{unit.Interval}";
                case ScheduleType.Range:
                    return $"{unit.From}-{unit.To}";
                default:
                    throw new InvalidOperationException($"ScheduleType: '{unit.Type}' is not supported. Please check your configuration file.");
            }
        }
    }
}