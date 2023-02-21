using System.Collections.Generic;
using System.Linq;
using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;

public static class MinhasExtensions
{
    public static Dictionary<string, string[]> ConverteParaProblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications.GroupBy(g => g.Key)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(x => x.Message).ToArray()
                            );
    }
}