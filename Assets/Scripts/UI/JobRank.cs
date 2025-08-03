using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobRoles
{
    TRAINEE = 1,
    SERVER = 2,
    SUPERVISOR = 4,
    MANAGER = 7,
    GENERAL_MANAGER = 9
}

public static class RoleParser
{
    public static string ParseRole(JobRoles job)
    {
        switch (job)
        {
            default:
            case JobRoles.TRAINEE:
                return "Trainee";
            case JobRoles.SERVER:
                return "Server";
            case JobRoles.SUPERVISOR:
                return "Supervisor";
            case JobRoles.MANAGER:
                return "Manager";
            case JobRoles.GENERAL_MANAGER:
                return "General Manager";
        }
    }
}
    
