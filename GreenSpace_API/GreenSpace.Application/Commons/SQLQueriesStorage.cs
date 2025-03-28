namespace GreenSpace.Application.Commons;

class SQLQueriesStorage
{
    public const string GET_ALL_USER = @"
                                        SELECT u.Id, Name, 
                                        [Email], Phone, Address,AvatarUrl,
                                        r.[RoleName] AS RoleName
                                        FROM [Users] u LEFT JOIN
                                        Roles r
                                        ON u.RoleID = r.Id
                                        WHERE u.IsDeleted = 0";
    public const string GET_USER_BY_ID = @"
                                        SELECT u.Id, Name, 
                                        [Email], u.Phone, u.Address,u.AvatarUrl, 
                                        r.[RoleName] AS RoleName
                                        FROM [Users] u LEFT JOIN
                                        Roles r
                                        ON u.RoleID = r.Id
                                        WHERE u.Id = @id";
    public const string GET_ALL_BAN_USER = @"
                                        SELECT u.Id, Name, 
                                        [Email], Phone, Address,AvatarUrl,
                                        r.[RoleName] AS RoleName
                                        FROM [Users] u LEFT JOIN
                                        Roles r
                                        ON u.RoleID = r.Id
                                        WHERE u.IsDeleted = 1";
    
}
