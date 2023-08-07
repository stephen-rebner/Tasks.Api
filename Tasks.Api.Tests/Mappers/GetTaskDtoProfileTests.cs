﻿using AutoMapper;
using Tasks.Api.Mappers;

namespace Tasks.Api.Tests.Mappers;

public class GetTaskDtoProfileTests
{
    // add an nunit tests that checks if the mapping from Task to TaskDto is correct
    [Test]
    public void GetTaskDtoProfile_ShouldHaveAValidConfiguration_WhenAddingATaskDtoProfileToAnAutomapperConfig()
    {
        // arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<GetTaskDtoProfile>());
        
        config.AssertConfigurationIsValid();
    }
    
}