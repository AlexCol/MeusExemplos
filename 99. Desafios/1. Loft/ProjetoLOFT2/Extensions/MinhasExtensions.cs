using System.Collections.Generic;
using System.Linq;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Extensions;
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

    public static ModelStateDictionary ConverteParaProblemDetailsM(this IReadOnlyCollection<Notification> notifications)
    {
        ModelStateDictionary modelState = new ModelStateDictionary();

        foreach (var notification in notifications)
        {
            modelState.AddModelError(notification.Key, notification.Message);
        }

        return modelState;
    }
}