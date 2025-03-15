namespace GreenSpace.Application.Commons;

class SQLQueriesStorage
{
    public const string GET_ALL_USER = @"
                                        SELECT u.Id, Name, 
                                        [Email], Phone, 
                                        r.[Name] AS RoleName
                                        FROM [Users] u LEFT JOIN
                                        Roles r
                                        ON u.RoleID = r.Id
                                        WHERE u.IsDeleted = 0";
    public const string GET_USER_BY_ID = @"
                                        SELECT u.Id, Name, 
                                        [Email], u.Phone, 
                                        r.[RoleName] AS RoleName
                                        FROM [Users] u LEFT JOIN
                                        Roles r
                                        ON u.RoleID = r.Id
                                        WHERE u.Id = @id
                                        AND u.IsDeleted = 0";
}
