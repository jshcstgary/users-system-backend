using AutoMapper;

using Common.Models;
using Common.Models.Dto.Role;
using Common.Models.Dto.RoleOption;
using Common.Models.Dto.User;

namespace Common.Lib;

/// <summary>
/// It contains the mapping configuration for AutoMapper between all the entities of the system.
/// </summary>
/// <remarks>
/// It inherits from the <see cref="Profile"/> class, which allows grouping related mapping configurations.
/// </remarks>
public class MappingConfig : Profile
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MappingConfig"/> class. Defines the mappings between types.
	/// </summary>
	public MappingConfig()
	{
		// Mapping between Role and its DTOs, allowing bidirectional mapping.
		CreateMap<Role, RoleDto>().ReverseMap();
		CreateMap<Role, RoleCreateDto>().ReverseMap();
		CreateMap<Role, RoleUpdateDto>().ReverseMap();

		// Mapping between RoleOption and its DTOs, allowing bidirectional mapping.
		CreateMap<RoleOption, RoleOptionDto>().ReverseMap();
		CreateMap<RoleOption, RoleOptionCreateDto>().ReverseMap();
		CreateMap<RoleOption, RoleOptionUpdateDto>().ReverseMap();

		// Mapping between User and its DTOs, allowing bidirectional mapping.
		CreateMap<User, UserDto>().ReverseMap();
		CreateMap<User, UserCreateDto>().ReverseMap();
		CreateMap<User, UserUpdateDto>().ReverseMap();
	}
}
