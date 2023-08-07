using AutoMapper;
using Tasks.Api.Dtos;
using Tasks.Api.Mappers;
using Tasks.Data;
using Task = System.Threading.Tasks.Task;

namespace Tasks.Api.Tests.Mappers;

public class UpdateTaskDtoProfileTests
{
    // add an nunit tests that checks if the mapping from Task to TaskDto is correct
    [Test]
    public void UpdateTaskDtoProfile_ShouldHaveAValidConfiguration_WhenAddingATaskDtoProfileToAnAutomapperConfig()
    {
        // arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UpdateTaskDtoProfile>());
        
        config.AssertConfigurationIsValid();
    }
    
}