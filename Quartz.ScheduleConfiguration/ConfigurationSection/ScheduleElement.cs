using System.Configuration;

namespace Quartz.ScheduleConfiguration.ConfigurationSection
{
    public class ScheduleElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(Name), IsRequired = true)]
        public string Name => (string)this[nameof(Name)];

        [ConfigurationProperty(nameof(Hours), IsRequired = true)]
        [CallbackValidator(CallbackMethodName = nameof(UnitElement.ValidateUnit), Type = typeof(UnitElement))]
        public UnitElement Hours => (UnitElement) this[nameof(Hours)];

        [ConfigurationProperty(nameof(Minutes), IsRequired = true)]
        [CallbackValidator(CallbackMethodName = nameof(UnitElement.ValidateUnit), Type = typeof(UnitElement))]
        public UnitElement Minutes => (UnitElement)this[nameof(Minutes)];

        [ConfigurationProperty(nameof(Seconds), IsRequired = true)]
        [CallbackValidator(CallbackMethodName = nameof(UnitElement.ValidateUnit), Type = typeof(UnitElement))]
        public UnitElement Seconds => (UnitElement)this[nameof(Seconds)];
    }
}