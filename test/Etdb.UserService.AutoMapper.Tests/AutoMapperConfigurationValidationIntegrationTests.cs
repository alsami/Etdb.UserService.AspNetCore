using Autofac;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Etdb.UserService.AutoMapper.Profiles;
using Xunit;

namespace Etdb.UserService.AutoMapper.Tests
{
    public class AutoMapperConfigurationValidationIntegrationTests
    {
        [Fact]
        public void AutoMapperConfigurationValidation_RegisterAutoMapper_Validate_Succeeds()
        {
            var container = new ContainerBuilder()
                .AddAutoMapper(typeof(UsersProfile).Assembly)
                .Build();

            var mapperConfiguration = container.Resolve<MapperConfiguration>();

            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}