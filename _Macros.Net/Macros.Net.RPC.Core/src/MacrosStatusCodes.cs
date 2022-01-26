namespace Macros.Net.RPC.Core;

public enum MacrosStatusCodes {
    Ok = 200,
    NotFound = 404,
    ActionNotFound = 4041,
    ControllerNotFound = 4042,
    UnandledExpection = 5000,
    Unauthorized = 401,
    Forbidden = 403
}