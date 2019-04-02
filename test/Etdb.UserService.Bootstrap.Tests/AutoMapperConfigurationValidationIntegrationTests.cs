using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Etdb.UserService.AutoMapper.Profiles;
using Xunit;

namespace Etdb.UserService.Bootstrap.Tests
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
