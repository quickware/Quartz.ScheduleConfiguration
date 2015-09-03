using System;
using System.Configuration;

namespace Quartz.ScheduleConfiguration.ConfigurationSection
{
    public class UnitElement : ConfigurationElement
    {
        public static void ValidateUnit(object value)
        {
            var unit = value as UnitElement;
            if (unit == null)
                throw new NullReferenceException($"This callback can only be used for configuration properties of type {nameof(UnitElement)}. Also, the element must exist.");

            switch (unit.Type)
            {
                case ScheduleType.All:
                    break;

                case ScheduleType.Interval:
                    if (unit.Start == -1)
                        throw new ArgumentException($"property {nameof(unit.Start)} should be set and be a positive number.");
                    if (unit.Interval == -1)
                        throw new ArgumentException($"property {nameof(unit.Interval)} should be set and be a positive number.");
                    break;

                case ScheduleType.Range:
                    if (unit.From == -1)
                        throw new ArgumentException($"property {nameof(unit.From)} should be set and be a positive number.");
                    if (unit.To == -1)
                        throw new ArgumentException($"property {nameof(unit.To)} should be set and be a positive number.");
                    break;

                case ScheduleType.Values:
                    if (string.IsNullOrEmpty(unit.Values))
                        throw new ArgumentException($"property {nameof(unit.Values)} should be set.");
                    break;
            }
        }

        [ConfigurationProperty(nameof(Type), IsRequired = true)]
        public ScheduleType Type => (ScheduleType) this[nameof(Type)];

        [ConfigurationProperty(nameof(Values))]
        [RegexStringValidator(@"^(\d+(,\d+)*)?$")]
        public string Values => (string) this[nameof(Values)];

        [ConfigurationProperty(nameof(From), DefaultValue = -1)]
        [IntegerValidator(ExcludeRange = false, MinValue = -1, MaxValue = 60)]
        public int From => (int) this[nameof(From)];

        [ConfigurationProperty(nameof(To), DefaultValue = -1)]
        [IntegerValidator(ExcludeRange = false, MinValue = -1, MaxValue = 60)]
        public int To => (int) this[nameof(To)];

        [ConfigurationProperty(nameof(Start), DefaultValue = -1)]
        [IntegerValidator(ExcludeRange = false, MinValue = -1, MaxValue = 60)]
        public int Start => (int) this[nameof(Start)];

        [ConfigurationProperty(nameof(Interval), DefaultValue = -1)]
        [IntegerValidator(ExcludeRange = false, MinValue = -1, MaxValue = 60)]
        public int Interval => (int) this[nameof(Interval)];
    }
}